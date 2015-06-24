using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Characters
{
    class Sneaky : MovingEntity
    {
        private double _elapsedTime;

        public Sneaky(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Sneaky;

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
            _elapsedTime += elapsedTime;

            UpdatePathPlanning();
            
            Behaviour = GameConfig.Bvr.Hide;
            UpdatePhysics(elapsedTime);

            // Update sprite 
            UpdateSprite(elapsedTime);

            if (HasEnemy && Vector2Helper.DistanceSq(Enemy.Position, Position) < 15000)
            {
                ReleasePoisonCloud();
            }
        }

        public void ReleasePoisonCloud()
        {
            if (_elapsedTime < 1.5) return;
            _elapsedTime = 0;

            var poisonCloud = AddPoisonCloud(Vector2Helper.ScalarSub(Position, Width/3));
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

        public override void UpdateGraphDrawing()
        {
            //throw new System.NotImplementedException();
        }
    }
}
