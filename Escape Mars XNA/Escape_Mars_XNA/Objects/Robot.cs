using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects
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

            Brain.Process();
            
            UpdatePhysics(elapsedTime);
            
            // Update sprite 
            UpdateSprite(elapsedTime);
        }

        public override void UpdateGraphDrawing()
        {
            World.UpdateGraph(PathPlanning.GetAStar());
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

            DrawBrainStack(spriteBatch);
        }

        private void DrawBrainStack(SpriteBatch spriteBatch)
        {
            var startPos = new Vector2(Position.X + Width / 2, Position.Y - Height / 2 - 20);

            DrawSubgoalStack(spriteBatch, Brain, startPos, 0);
            
        }

        private float DrawSubgoalStack(SpriteBatch spriteBatch, Goal.Goal goal, Vector2 position, int level)
        {
            var spaces = "";
            for (var i = 0; i < level; i++)
            {
                spaces = spaces.Insert(spaces.Length, "  ");
            }

            spriteBatch.DrawString(AnimatedSprite.Font, spaces + goal.Type, position, Color.White);

            var nextPos = position;
            nextPos.Y += 10;

            foreach (var subgoal in goal.Subgoals.Reverse())
            {
                nextPos.Y = DrawSubgoalStack(spriteBatch, subgoal, nextPos, level + 1);
            }
            return nextPos.Y;
        }
    }
}
