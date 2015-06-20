using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects
{
    class Jewel:BaseGameEntity
    {
        public Jewel(Vector2 position)
        {
            ItemType = EntityFeature.Itm.RocketPart;

            Width = 32;
            Height = 32;

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
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                new Rectangle((int)Position.X, (int)Position.Y, Width, Height),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                ); 
        }
    }
}
