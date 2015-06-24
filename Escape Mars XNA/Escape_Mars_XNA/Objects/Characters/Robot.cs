using System;
using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Objects.Characters
{
    class Robot : MovingEntity
    {
        private double _elapsedTime;

        private bool _displayBrainStack;

        private int _brainStackLevel = 2;

        public Robot(Vector2 position)
        {
            ItemType = EntityFeature.Itm.Robot;

            // Sprite dimensions
            Width = 64;
            Height = 64;
            CollisionBox = new Rectangle(0, 0, Width, Height);

            // Object properties
            MaxSpeed = 80;
            Mass = 0.1;

            // Set the initial position of the Robot
            Position = position;

            CanPickUp = true;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(4, 5) { Animate = true };

            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);

            Brain = new GoalThink(this);

            Brain.Activate();
        }

        // Compute the new values for the object vecotr
        public override void Update(double elapsedTime)
        {
            _elapsedTime += elapsedTime;

            UpdatePathPlanning();

            Brain.Process();

            UpdatePhysics(elapsedTime);

            // Update sprite 
            UpdateSprite(elapsedTime);

            NegotiateBullets();
        }

        public override void UpdateGraphDrawing()
        {
            World.UpdateGraph(PathPlanning.GetAStar());
        }

        public override void ShootBullet()
        {
            if (_elapsedTime < 0.5) return;
            TurnToTarget();
            Ammo--;
            Brain.Arbitrate();
            _elapsedTime = 0;
            var bulletPosition = Position;
            var targetPosition = Position;
            var bulletRange = GameConfig.BulletRange;
            switch (Direction)
            {
                case AnimatedSprite.Direction.Right:
                    bulletPosition.X += (Width / 6);
                    bulletPosition.Y -= (Height / 2 + Height / 4);
                    targetPosition = bulletPosition;
                    targetPosition.X += bulletRange;
                    break;
                case AnimatedSprite.Direction.Left:
                    bulletPosition.X -= (Width / 2);
                    bulletPosition.Y -= (Height / 2 + Height / 4);
                    targetPosition = bulletPosition;
                    targetPosition.X -= bulletRange;
                    break;
                case AnimatedSprite.Direction.Down:
                    bulletPosition.X -= (Width / 2);
                    bulletPosition.Y -= (Height / 2 + Height / 4);
                    targetPosition = bulletPosition;
                    targetPosition.Y += bulletRange;
                    break;
                case AnimatedSprite.Direction.Up:
                    bulletPosition.X += (Width / 14);
                    bulletPosition.Y -= (Height);
                    targetPosition = bulletPosition;
                    targetPosition.Y -= bulletRange;
                    break;

            }
            var bullet = AddBullet(bulletPosition);
            bullet.SteeringPosition = targetPosition;
        }

        private void TurnToTarget()
        {
            var deltaLR = Position.X - Enemy.Position.X;
            var deltaUD = Position.Y - Enemy.Position.Y;
            if (Math.Abs(deltaLR).CompareTo(Math.Abs(deltaUD)) < 0)
            {
                if (deltaUD < 0)
                {
                    Direction = AnimatedSprite.Direction.Down;
                    AnimatedSprite.Move(AnimatedSprite.Direction.Down);
                    return;
                }
                Direction = AnimatedSprite.Direction.Up;
                AnimatedSprite.Move(AnimatedSprite.Direction.Up);
            }
            else
            {
                if (deltaLR < 0)
                {
                    Direction = AnimatedSprite.Direction.Right;
                    AnimatedSprite.Move(AnimatedSprite.Direction.Right);
                    return;
                }
                Direction = AnimatedSprite.Direction.Left;
                AnimatedSprite.Move(AnimatedSprite.Direction.Left);
            }
        }

        // Draw the required portion of the sprite
        public override void Draw(SpriteBatch spriteBatch)
        {
            CollisionBox.X = (int)Position.X - Width / 2;
            CollisionBox.Y = (int)Position.Y - Height;
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                CollisionBox,
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );
            DrawHealthAndAmmo(spriteBatch);
            DrawBrainStack(spriteBatch);
        }

        private void DrawBrainStack(SpriteBatch spriteBatch)
        {
            if (!_displayBrainStack) return;
     
            var startPos = new Vector2(Position.X + Width / 2, Position.Y - Height / 2 - 20);

            DrawSubgoalStack(spriteBatch, Brain, startPos, 0);

        }

        private float DrawSubgoalStack(SpriteBatch spriteBatch, Goal.Goal goal, Vector2 position, int level)
        {
            var spaces = "";
            for (var i = 0; i < level; i++)
            {
                spaces = spaces.Insert(spaces.Length, "  ");
            }

            var nextPos = position;
            if (level <= _brainStackLevel)
            {
                spriteBatch.DrawString(AnimatedSprite.Font, spaces + level + "_" + goal.Type, position, Color.White);

                nextPos.Y += 10;
            }

            foreach (var subgoal in goal.Subgoals.Reverse())
            {
                nextPos.Y = DrawSubgoalStack(spriteBatch, subgoal, nextPos, level + 1);
            }
            return nextPos.Y;
        }

        public override void ShowBrainStack(bool display, int brainStackLevel)
        {
            _displayBrainStack = display;
            _brainStackLevel = brainStackLevel;
        }
    }
}