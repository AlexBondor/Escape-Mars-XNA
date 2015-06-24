using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Others
{
    class Bullet : MovingEntity
    {
        public int Damage = GameConfig.BulletDamage;

        public Bullet(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Bullet;

            Width = 32;
            Height = 32;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            Position = position;

            MaxSpeed = 400;
            Mass = 0.1;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(6, 20) { Animate = false };

            SteeringBehaviour = new SteeringBehaviours(this);

            Behaviour = GameConfig.Bvr.Seek;

            SteeringPosition = Vector2.One;
        }

        public override void Update(double elapsedTime)
        {
            // If bullet out of viewing area
            if (Vector2Helper.DistanceSq(Position, SteeringPosition) < 100)
            {
                World.RemoveItemOfTypeFromPosition(Position, EntityFeature.Itm.Bullet);
            }

            UpdatePhysics(elapsedTime);

            AnimatedSprite.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CollisionBox.X = (int)Position.X;
            CollisionBox.Y = (int)Position.Y;
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

        public bool IsOutOfScreen()
        {
            return Position.X < 0 || Position.Y < 0 || Position.X > Vector2Helper.WindowWidth ||
                   Position.Y > Vector2Helper.WindowHeight;
        }
    }
}
