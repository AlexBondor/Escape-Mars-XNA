using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Entity
{
    abstract class BaseGameEntity
    {
        // Width of sprite
        public int Width;

        // Height of sprite;
        public int Height;

        // A vector for position
        public Vector2 Position;

        // Instance of world object so that it can access any other
        // obstacles or characters
        public World World;

        // Sprite used for animating object movement
        public AnimatedSprite AnimatedSprite;

        // Direction of animated sprite
        public AnimatedSprite.Direction Direction = AnimatedSprite.Direction.Down;

        // Every entity has a unique identifying number
        public int Id { get; private set; }

        // This is the nex valid id. Each time a BaseGameEntity is
        // instantiated this value is updated
        private static int _nextValidId;

        protected BaseGameEntity()
        {
            Id = _nextValidId;
            _nextValidId++;

            // Default values for width and height
            Width = 64;
            Height = 64;

            World = World.Instance();
        }

        public abstract void Update(double elapsedTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
