using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Others
{
    class Rocket : MovingEntity
    {
        public Rocket(Vector2 position)
        {
            Health = 300;

            ItemType = Itm.Rocket;

            // Sprite dimensions
            Width = 128;
            Height = 128;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(1, 5) {Animate = false};
        }

        public override void Update(double elapsedTime)
        {
            // Update sprite
            AnimatedSprite.Update(elapsedTime);
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
            var position = new Vector2(CollisionBox.X + Width / 3 + Width / 8, CollisionBox.Y - 20);

            spriteBatch.DrawString(AnimatedSprite.Font, Health.ToString(), position, new Color(0, 255, 0));
        }
        protected override void OnDie()
        {
            World.GameOver(ItemType);
        }
    }
}
