using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Characters
{
    class Laika:MovingEntity
    {
        public Laika(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Laika;

            // Sprite dimensions
            Width = 32;
            Height = 32;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            // Object properties
            MaxSpeed = 40;
            Mass = 0.1;

            // Set the initial position of the Robot
            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(3, 5) { Animate = true };

            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);

            Brain = new GoalThink(this); 
   
            Brain.Activate();
        }

        public override void Update(double elapsedTime)
        {
            UpdatePathPlanning();

            Brain.Process(); 

            UpdatePhysics(elapsedTime);

            // Update sprite 
            UpdateSprite(elapsedTime);
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
        }

        public override void UpdateGraphDrawing()
        {
            //throw new System.NotImplementedException();
        }
    }
}
