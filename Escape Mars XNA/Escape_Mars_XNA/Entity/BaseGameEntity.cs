﻿using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Entity
{
    abstract class BaseGameEntity
    {
        public enum Itm
        {
            RocketPart = 0,
            HealthPack = 1,
            Ammo = 2,
            Rocket = 3,
            Robot = 4,
            Sneaky = 5,
            Laika = 6,
            Bullet = 7,
            PoisonCloud = 8,
            NotSet = 9
        }

        // Width of sprite
        public int Width;

        // Height of sprite;
        public int Height;

        // Every character starts with default health value of 100
        public int Health = GameConfig.DefaultHeath;
                             
        // A vector for position
        public Vector2 Position;

        // Instance of world object so that it can access any other
        // obstacles or characters
        public World World;

        // Sprite used for animating object movement
        public AnimatedSprite AnimatedSprite;

        // Direction of animated sprite
        public AnimatedSprite.Direction Direction = GameConfig.DefaultDirection;

        // Every entity has a unique identifying number
        public int Id { get; private set; }

        public Itm ItemType { get; set; }
        
        public bool PickedUp { get; set; }

        // This is the nex valid id. Each time a BaseGameEntity is
        // instantiated this value is updated
        private static int _nextValidId;

        public Rectangle CollisionBox = new Rectangle();

        protected BaseGameEntity()
        {
            Id = _nextValidId;
            _nextValidId++;

            // Default values for width and height
            Width = GameConfig.DefaultBgeWidth;
            Height = GameConfig.DefaultHeath;

            World = World.Instance();

            ItemType = Itm.NotSet;
        }

        public abstract void Update(double elapsedTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void FollowMe(MovingEntity who)
        {
        }
    }
}
