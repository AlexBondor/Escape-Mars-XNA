using System.Collections.Generic;
using System.IO;
using System.Linq;
using Escape_Mars_XNA.Character;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA
{
    class World
    {
        // Graph map
        public Graph MapGraph { get; private set; }

        // Node types, used for map creation
        private readonly string[] _mapTxtNode;

        private static World _instance;

        private static ContentManager _contentManager;

        public MovingEntity Robot;

        public List<BaseGameEntity> Objects { get; private set; }

        public int HealthPacksCount = 0;

        public int AmmoPacksCount = 0;

        public int RocketPartsCount = 0;

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
            var node = MapGraph.GetNodeByRowCol(1, 1);

            var robot = new Robot(new Vector2(node.Position.X + node.Width/2, node.Position.Y + node.Height/2));

            Robot = robot;
            Objects.Add(robot);

            var sneaky = new Sneaky(new Vector2(200, 200));
            Objects.Add(sneaky);

            var rocket = new Rocket(new Vector2(400, 300));
            Objects.Add(rocket);

            var laika = new Laika(new Vector2(500, 500));
            Objects.Add(laika);
        }

        // Load sprites for every object in scene
        public void Load(ContentManager contentManager)
        {
            _contentManager = contentManager;

            foreach (var item in Objects)
            {
                var file = item.GetType().Name;
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
            if (HealthPacksCount == 0)
            {
                AddHealthPack();
            }

            foreach (var item in Objects)
            {
                item.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        private void AddHealthPack()
        {
            var position = MapGraph.RandomValidNode(0, MapGraph.Rows - 1, 0, MapGraph.Cols - 1);

            var healthPack = new HealthPack(position);

            var file = healthPack.GetType().Name;
            healthPack.AnimatedSprite.Texture = _contentManager.Load<Texture2D>(file);

            Objects.Add(healthPack);

            HealthPacksCount ++;
        }

        // Draw all world objects
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(spriteBatch);
            foreach (var item in Objects)
            {
                item.Draw(spriteBatch);
            }
        }

        // Draw the world map
        public void DrawMap(SpriteBatch spriteBatch)
        {
            foreach (var node in MapGraph.Nodes)
            {
                node.Draw(spriteBatch);
            }
            foreach (var edge in MapGraph.DrawableEdges)
            {
                edge.Draw(spriteBatch);
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
        
        public void SearchTo(Vector2 to)
        {
            Robot.CreatePathTo(to);
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

        public List<Vector2> GetItemTypePositions(EntityFeature.Itm itemType)
        {
            var items = Objects.Where(o => o.ItemType == itemType);
            return items.Select(item => item.Position).ToList();
        }
    }
}
