﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Objects.Characters;
using Escape_Mars_XNA.Objects.Consumables;
using Escape_Mars_XNA.Objects.Others;
using Escape_Mars_XNA.Objects.RocketParts;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA
{
    class World
    {
        /**
         * GENERAL
         */
        private static World _instance;
        
        private  ContentManager _contentManager;
        public List<BaseGameEntity> Objects { get; private set; }

        public List<BaseGameEntity> ObjectsToBeRemoved = new List<BaseGameEntity>();

        public List<BaseGameEntity> ObjectsToBeAdded = new List<BaseGameEntity>();

        public bool Paused { get; set; }

        private SpriteBatch _spriteBatch;

        private bool _escapedMars;

        private bool _failedToEscapeMars;

        private string _additionalMessage;


        /**
         * GRAPH
         */
        // Graph map
        public Graph MapGraph { get; private set; }

        // Node types, used for map creation
        private readonly string[] _mapTxtNode;



        /**
         * ITEMS
         */
        public MovingEntity Robot;

        public MovingEntity Rocket;

        public int HealthPacksCount;

        public int AmmoPacksCount;

        public int RocketPartsCount;

        public int AliensCount;

        
        private World()
        {
            Objects = new List<BaseGameEntity>();
            MapGraph = new Graph(19, 24);

            _mapTxtNode = new string[19 * 24];
        }

        public static World Instance()
        {
            return _instance ?? (_instance = new World());
        }

        // Creates the world objects
        public void Create()
        {
            AmmoPacksCount = 0;
            HealthPacksCount = 0;
            RocketPartsCount = 3;
            AliensCount = 0;
            _escapedMars = false;
            _failedToEscapeMars = false;
            Paused = false;

            var node = MapGraph.GetNodeByRowCol(3, 1);
            var computer = new Computer(node.Position);
            Objects.Add(computer);

            node = MapGraph.GetNodeByRowCol(18, 11);
            var toolbox = new Toolbox(node.Position);
            Objects.Add(toolbox);

            node = MapGraph.GetNodeByRowCol(1, 17);
            var jewel = new Jewel(node.Position);
            Objects.Add(jewel);

            node = MapGraph.GetNodeByRowCol(17, 1);
            var laika = new Laika(Vector2Helper.ScalarAdd(node.Position, node.Width / 2));
            Objects.Add(laika);

            node = MapGraph.GetNodeByRowCol(1, 1);
            var robot = new Robot(Vector2Helper.ScalarAdd(node.Position, node.Width / 2));
            Robot = robot;
            Objects.Add(robot);

            var rocket = new Rocket(new Vector2(400, 300));
            Rocket = rocket;
            Objects.Add(rocket);
        }

        // Load sprites for every object in scene
        public void Load(ContentManager contentManager)
        {
            _contentManager = contentManager;

            foreach (var item in Objects)
            {
                var file = item.GetType().Name;
                item.AnimatedSprite.Font = contentManager.Load<SpriteFont>("Font");
                item.AnimatedSprite.Texture = contentManager.Load<Texture2D>(file);
            }

            // Load Map
            var mapTxtLines = File.ReadAllLines("Map.txt");
            var index = 0;
            foreach (var txtLine in mapTxtLines)
            {
                var replace = txtLine.Replace("  ", " ");
                var split = replace.Split(' ');
                foreach (var ch in split)
                {
                    _mapTxtNode[index] = ch;
                    index++;
                }
            }


            // Textures for nodes
            var dot = contentManager.Load<Texture2D>("Dot");
            var ground = contentManager.Load<Texture2D>("Ground");
            var rock = contentManager.Load<Texture2D>("Rock");
            var lineTexture = contentManager.Load<Texture2D>("LineTexture");

            foreach (var node in MapGraph.Nodes)
            {
                switch (_mapTxtNode[node.Index])
                {
                    case "0":
                        node.Texture = ground;
                        node.Active = true;
                        break;
                    case "1":
                        node.Texture = rock;
                        break;
                    default:
                        node.Texture = ground;
                        break;
                }
                node.Dot = dot;
            }
            foreach (var edge in MapGraph.DrawableEdges)
            {
                edge.Texture = lineTexture;
            }
        }

        // Update all world objects
        public void Update(GameTime gameTime)
        {
            if (Paused) return;

            if (AliensCount == 0)
            {
                for (var i = 0; i < GameConfig.MaxAlienCount; i++)
                {
                    AddAlien();
                }
            }

            if (HealthPacksCount == 0)
            {
                AddHealthPack();
            }

            if (AmmoPacksCount == 0)
            {
                AddAmmoPack();
            }

            foreach (var item in Objects)
            {
                item.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }

            RemoveAnyUnwantedObjects();

            AddAnyNewObjects();
        }

        private void AddAlien()
        {
            var position = MapGraph.RandomValidPosition(0, MapGraph.Rows, 0, MapGraph.Cols);
            var sneaky = new Sneaky(position)
            {
                AnimatedSprite =
                {
                    Font = _contentManager.Load<SpriteFont>("Font"),
                    Texture = _contentManager.Load<Texture2D>("Sneaky")
                }
                
            };

            Objects.Add(sneaky);

            Robot.Enemies.Add(sneaky);
            sneaky.Enemies.Add(Robot);

            AliensCount ++;
        }
        
        private void AddHealthPack()
        {
            var position = MapGraph.RandomValidPosition(0, MapGraph.Rows - 1, 0, MapGraph.Cols - 1);

            var healthPack = new HealthPack(position);

            var file = healthPack.GetType().Name;
            healthPack.AnimatedSprite.Texture = _contentManager.Load<Texture2D>(file);

            Objects.Add(healthPack);

            HealthPacksCount++;
        }

        private void AddAmmoPack()
        {
            var position = MapGraph.RandomValidPosition(0, MapGraph.Rows - 1, 0, MapGraph.Cols - 1);

            var ammoPack = new Ammo(position);

            var file = ammoPack.GetType().Name;
            ammoPack.AnimatedSprite.Texture = _contentManager.Load<Texture2D>(file);

            Objects.Add(ammoPack);

            AmmoPacksCount++;
        }
        
        private void RemoveAnyUnwantedObjects()
        {
            if (ObjectsToBeRemoved.Count == 0)
            {
                return;
            }
            foreach (var item in ObjectsToBeRemoved)
            {
                Objects.Remove(item);
            }
            ObjectsToBeRemoved.Clear();
        }

        private void AddAnyNewObjects()
        {
            if (ObjectsToBeAdded.Count == 0)
            {
                return;
            }
            foreach (var item in ObjectsToBeAdded)
            {
                Objects.Add(item);
            }
            ObjectsToBeAdded.Clear();
        }
        
        // Draw all world objects
        public void Draw()
        {
            DrawMap();
            foreach (var item in Objects)
            {
                item.Draw(_spriteBatch);
            }
            if (_failedToEscapeMars)
            {
                DisplayMessage("Failed to Escape Mars!", _additionalMessage);
            }
            if (_escapedMars)
            {
                DisplayMessage("Escaped Mars!", _additionalMessage);
            }
        }

        private void DisplayMessage(string message, string additionalMessage)
        {
            var bigFont = _contentManager.Load<SpriteFont>("BigFont");
            var font = _contentManager.Load<SpriteFont>("Font");
            _spriteBatch.DrawString(bigFont, message, new Vector2(200, 200), Color.White);
            _spriteBatch.DrawString(font, additionalMessage, new Vector2(200, 270), Color.White);
        }

        // Draw the world map
        public void DrawMap()
        {
            foreach (var node in MapGraph.Nodes)
            {
                node.Draw(_spriteBatch);
            }
            foreach (var edge in MapGraph.DrawableEdges)
            {
                edge.Draw(_spriteBatch);
            }
        }

        // Set the DisplayGraph bool of each node and edge
        public void DisplayGraph(bool displayGraph)
        {
            foreach (var node in MapGraph.Nodes)
            {
                node.DisplayGraph = displayGraph;
            }
            foreach (var edge in MapGraph.DrawableEdges)
            {
                edge.DisplayGraph = displayGraph;
            }
        }

        public void RobotMoveTo(Vector2 to)
        {
            Robot.Brain.AddSubgoal(new GoalFollowPath(Robot, to));
        }

        // Update the colors of the graph according the
        // result given by the AStar algorithm
        public void UpdateGraph(AStar aStar)
        {
            foreach (var edge in MapGraph.DrawableEdges)
            {
                edge.Color = Color.LightGray;
            }

            if (aStar.GetMinSpTree() == null) return;

            var mst = aStar.GetMinSpTree();
            foreach (var e in mst.Select(edge => MapGraph.GetDrawableEdge(edge.From, edge.To)))
            {
                e.Color = Color.Blue;
            }

            var path = aStar.GetPath();
            foreach (var e in path.Select(edge => MapGraph.GetDrawableEdge(edge.From, edge.To)))
            {
                e.Color = Color.Black;
            }
        }

        public List<Vector2> GetItemTypePositions(BaseGameEntity.Itm itemType)
        {
            var items = Objects.Where(o => o.ItemType == itemType);
            return items.Select(item => item.Position).ToList();
        }

        public Vector2 GetClosestItemTypePosition(Vector2 ownerPosition, BaseGameEntity.Itm itemType)
        {
            var items = GetItemTypePositions(itemType);

            var closest = new Vector2(float.MaxValue, float.MaxValue);

            foreach (var item in items)
            {
                if (Vector2Helper.DistanceSq(ownerPosition, closest) > Vector2Helper.DistanceSq(ownerPosition, item))
                {
                    closest = item;
                }
            }
            return closest;
        }

        public ContentManager GetContentManager()
        {
            return _contentManager;
        }

        public void DisplayInfo()
        {
            var position = new Vector2(10, 610);
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "G - Toggle graph", position, Color.Black);
            position.Y += 15;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "B - Toggle display goals stack", position, Color.Black);
            
            position.X += 170;
            position.Y -= 15;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "R - Restart game", position, Color.Black);
            position.Y += 15;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "P - Togle pause game", position, Color.Black);

            position.X += 170;
            position.Y -= 15;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "H - Damage robot", position, Color.Black);
            
            position.X += 170;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "0..4 - Limit stack level", position, Color.Black);
            position.Y += 15;
            _spriteBatch.DrawString(Robot.AnimatedSprite.Font, "Left mouse - Robot move to", position, Color.Black);
        }

        public void RemoveItemOfTypeFromPosition(Vector2 position, BaseGameEntity.Itm type)
        {
            var item = Objects.First(i => Vector2Helper.DistanceSq(i.Position, position) < 1 && i.ItemType == type);

            if (item == null) return;

            ObjectsToBeRemoved.Add(item);

            switch (item.ItemType)
            {
                case BaseGameEntity.Itm.HealthPack:
                    HealthPacksCount--;
                    break;
                case BaseGameEntity.Itm.Ammo:
                    AmmoPacksCount--;
                    break;
            }
        }

        public void GameOver(BaseGameEntity.Itm itemType)
        {
            Paused = true;
            _failedToEscapeMars = true;
            if (itemType == BaseGameEntity.Itm.Robot)
            {
                _additionalMessage = "Your robot couldn't stand the heat of Mars!";
            }
            else
            {
                _additionalMessage = "Your robot let the enemies destroy the rocket!";
            }
        }

        public void GameWon()
        {
            Paused = true;
            _escapedMars = true;
            _additionalMessage = "Your robot did a great job fighting with aliens!";
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
    }
}
