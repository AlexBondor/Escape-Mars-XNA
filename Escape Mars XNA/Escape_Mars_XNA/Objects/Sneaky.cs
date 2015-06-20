using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects
{
    class Sneaky : MovingEntity
    {
        public Sneaky(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Sneaky;

            // Sprite dimensions
            Width = 64;
            Height = 64;

            // Object properties
            MaxSpeed = 100;
            Mass = 0.1;

            // Set the initial position of the Robot
            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(16, 10) { Animate = true };
                                    
            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);
        }

        public override void Update(double elapsedTime)
        {
            UpdatePathPlanning();

            Behaviour = Bvr.Hide;
            Enemy = World.Robot;
            Obstacles = World.MapGraph.GetObstacles();
            UpdatePhysics(elapsedTime);

            // Update sprite 
            UpdateSprite(elapsedTime);  
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );
        }

        public override void UpdateGraphDrawing()
        {
            //throw new System.NotImplementedException();
        }
    }
}
