using System.ComponentModel.Design;
using System.Security.Policy;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Character
{
    class Robot : MovingEntity
    {
        public Robot(Vector2 position)
        {
            // Sprite dimensions
            Width = 64;
            Height = 64;

            // Object properties
            MaxSpeed = 80;
            Mass = 0.1;      
                            
            // Set the initial position of the Robot
            Position = position;
            
            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(4, 5) {Animate = true}; 
                                    
            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);
        }

        // Compute the new values for the object vecotr
        public override void Update(double elapsedTime)
        {
            UpdatePathPlanning();

            Behaviour = Bvr.Explore;
            UpdatePhysics(elapsedTime);
            
            // Update sprite 
            UpdateSprite(elapsedTime);
        }

        // Draw the required portion of the sprite
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                new Rectangle((int) Position.X - Width / 2, (int) Position.Y - Height, Width, Height),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );    
        }
    }
}
