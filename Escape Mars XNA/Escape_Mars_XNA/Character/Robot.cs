using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Composite;
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
            ItemType = EntityFeature.Itm.Robot;

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

            Health = 40;

            Brain = new GoalThink(this);

            Brain.Activate();
        }

        // Compute the new values for the object vecotr
        public override void Update(double elapsedTime)
        {
            UpdatePathPlanning();

            World.UpdateGraph(PathPlanning.GetAStar());

            Brain.Process();

            //Console.WriteLine(Brain.Subgoals.Count);

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
