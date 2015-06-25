using System.Collections.Generic;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Objects.Others;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Characters
{
    class Sneaky : MovingEntity
    {
        private double _releaseCloudTime;

        private double _cloudTakeTime;

        private List<PoisonCloud> _poisonClouds = new List<PoisonCloud>();
        private List<PoisonCloud> _cloudsToBeRemoved = new List<PoisonCloud>();

        public Sneaky(Vector2 position)
        {
            ItemType = Itm.Sneaky;

            // Sprite dimensions
            Width = 64;
            Height = 64;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            // Object properties
            MaxSpeed = 100;
            Mass = 0.1;

            // Set the initial position of the Robot
            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(16, 10) { Animate = true };
                                    
            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);

            Obstacles = World.MapGraph.GetObstacles();

            Enemy = World.Robot;
        }

        public override void Update(double elapsedTime)
        {
            _releaseCloudTime += elapsedTime;
            _cloudTakeTime += elapsedTime;

            UpdatePathPlanning();

            if (Vector2Helper.DistanceSq(Position, Enemy.Position) < GameConfig.PanicDistance)
            {
                Behaviour = SteeringBehaviours.Bvr.Hide;
            }
            else
            {
                Behaviour = SteeringBehaviours.Bvr.Seek;
                CreatePathTo(World.Rocket.Position);
            }
            UpdatePhysics(elapsedTime);

            // Update sprite 
            UpdateSprite(elapsedTime);

            if (HasEnemy && Vector2Helper.DistanceSq(Enemy.Position, Position) < GameConfig.AttackingRange ||
                Vector2Helper.DistanceSq(World.Rocket.Position, Position) < GameConfig.AttackingRange)
            {
                ReleasePoisonCloud();
            }

            NegotiatePoisonClouds();

            RemoveAnyUnwantedClouds();
        }

        private void RemoveAnyUnwantedClouds()
        {
            if (_cloudsToBeRemoved.Count == 0)
            {
                return;
            }

            foreach (var cloud in _cloudsToBeRemoved)
            {
                _poisonClouds.Remove(cloud);
            }

            _cloudsToBeRemoved.Clear();
        }

        public void ReleasePoisonCloud()
        {
            if (_releaseCloudTime < 1.5) return;
            _releaseCloudTime = 0;

            AddPoisonCloud(Vector2Helper.ScalarSub(Position, Width/3));
        }

        private void AddPoisonCloud(Vector2 posTarget)
        {
            var poisonCloud = new PoisonCloud(posTarget)
            {
                AnimatedSprite =
                {
                    Texture = World.GetContentManager().Load<Texture2D>("PoisonCloud")
                }
            };

            World.ObjectsToBeAdded.Add(poisonCloud);
            _poisonClouds.Add(poisonCloud);
        }

        private void NegotiatePoisonClouds()
        {
            if (_cloudTakeTime < 0.2) return;
            _cloudTakeTime = 0;

            if (_poisonClouds.Count == 0)
            {
                return;
            }

            foreach (var poisonCloud in _poisonClouds)
            {
                var enemyHitBox = Enemy.CollisionBox;

                if (enemyHitBox.Intersects(poisonCloud.CollisionBox))
                {
                    Enemy.TakeDamage(poisonCloud.Damage);
                }

                enemyHitBox = World.Rocket.CollisionBox;

                if (enemyHitBox.Intersects(poisonCloud.CollisionBox))
                {
                    World.Rocket.TakeDamage(poisonCloud.Damage);
                }

                // If bullet out of viewing area
                if (poisonCloud.AnimatedSprite.CurrentCol == 9)
                {
                    World.ObjectsToBeRemoved.Add(poisonCloud);
                    _cloudsToBeRemoved.Add(poisonCloud);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CollisionBox.X = (int)Position.X - Width / 2;
            CollisionBox.Y = (int)Position.Y - Height / 2;

            spriteBatch.Draw(
                AnimatedSprite.Texture,
                CollisionBox,
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );
            DrawHealthAndAmmo(spriteBatch);
        }

        protected override void OnDie()
        {
            if (_poisonClouds.Count != 0)
            {
                foreach (var cloud in _poisonClouds)
                {
                    World.ObjectsToBeRemoved.Add(cloud);    
                }
            }
        }
    }
}
