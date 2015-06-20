using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects
{
    class Ammo:BaseGameEntity
    {        
        public Ammo(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Ammo;

            Width = 16;
            Height = 16;

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
                new Rectangle((int)Position.X + Width / 2, (int)Position.Y + Height / 2, Width, Height),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                ); 
        }
    }
}
