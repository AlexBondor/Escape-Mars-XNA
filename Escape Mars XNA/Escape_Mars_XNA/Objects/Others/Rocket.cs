using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Others
{
    class Rocket : BaseGameEntity
    {
        public Rocket(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Rocket;

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
        }
    }
}
