﻿using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Character
{
    class Rocket : BaseGameEntity
    {
        public Rocket(Vector2 position)
        {
            // Sprite dimensions
            Width = 64;
            Height = 64;

            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(4, 5) {Animate = false};
        }

        public override void Update(double elapsedTime)
        {
            // Update sprite
            AnimatedSprite.Update(elapsedTime);
        }

        public override void UpdateSprite()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width * 2, Height * 2),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                ); 
        }
    }
}
