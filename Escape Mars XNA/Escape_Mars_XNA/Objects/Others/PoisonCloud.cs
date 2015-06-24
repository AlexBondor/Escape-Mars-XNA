using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Others
{
    class PoisonCloud:MovingEntity
    {
        public PoisonCloud(Vector2 position)
        {
            ItemType = EntityFeature.Itm.PoisonCloud;

            Width = 32;
            Height = 32;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            Position = position;

            MaxSpeed = 400;
            Mass = 0.1;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(10, 5) { Animate = true };

            SteeringBehaviour = new SteeringBehaviours(this);

            Behaviour = GameConfig.Bvr.Idle;

            SteeringPosition = Vector2.One;
        }

        public override void Update(double elapsedTime)
        {
            // If bullet out of viewing area
            if (AnimatedSprite.CurrentCol == 9)
            {
                World.RemoveItemOfTypeFromPosition(Position, EntityFeature.Itm.PoisonCloud);  
            }

            UpdatePhysics(elapsedTime);

            AnimatedSprite.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CollisionBox.X = (int)Position.X;
            CollisionBox.Y = (int)Position.Y;
            CollisionBox.Width = Width + Width/2;
            CollisionBox.Height = Width + Height/2;
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                CollisionBox,
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );
        }

        public override void UpdateGraphDrawing()
        {
            throw new System.NotImplementedException();
        }
    }
}
