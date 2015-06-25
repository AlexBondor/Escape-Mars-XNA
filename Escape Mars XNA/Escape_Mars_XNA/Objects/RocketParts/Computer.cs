using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.RocketParts
{
    class Computer:BaseGameEntity
    {
        private MovingEntity _toFollow;

        public Computer(Vector2 position)
        {
            ItemType = Itm.RocketPart;

            Width = 32;
            Height = 32;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(1, 5) { Animate = false };
        }

        public override void Update(double elapsedTime)
        {
            AnimatedSprite.Update(elapsedTime);

            if (PickedUp)
            {
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            Position = _toFollow.Position;
        }

        public override void FollowMe(MovingEntity who)
        {
            _toFollow = who;
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
    }
}
