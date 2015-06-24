using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Consumables
{
    class HealthPack:BaseGameEntity
    {
        public HealthPack(Vector2 position)
        {
            ItemType = EntityFeature.Itm.HealthPack;

            Width = 16;
            Height = 16;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(1, 5) { Animate = false };
        }

        public override void Update(double elapsedTime)
        {
            AnimatedSprite.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CollisionBox.X = (int)Position.X + Width / 2;
            CollisionBox.Y = (int)Position.Y + Height / 2;
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                CollisionBox,
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                ); 
        }
    }
}
