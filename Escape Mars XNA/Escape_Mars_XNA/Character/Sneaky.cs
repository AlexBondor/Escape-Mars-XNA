using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Escape_Mars_XNA.Character
{
    class Sneaky : MovingEntity
    {
        public Sneaky(Vector2 position)
        {
            // Sprite dimensions
            Width = 64;
            Height = 64;

            // Object properties
            MaxSpeed = 50;
            Mass = 0.1;

            // Set the initial position of the Robot
            Position = position;

            // Set sprite no. of columns
            AnimatedSprite = new AnimatedSprite(16, 10) { Animate = true };
                                    
            SteeringBehaviour = new SteeringBehaviours(this);

            PathPlanning = new PathPlanning(this, World.MapGraph);
        }

        public override void Update(double elapsedTime)
        {
            var whatever = SteeringBehaviour.Hide(World.Robot, World.MapGraph.GetObstacles());

            GoTo(HidingSpot);

            if (Arrived)
            {
                AnimatedSprite.Move(AnimatedSprite.Direction.Down);
                return; 
            }

            PathPlanning.Update();

            // Calculate the combined force from each steering behavior
            // in the vehicles' list
            var steeringForce = Arriving ? SteeringBehaviour.Arrive(SeekablePosition, SteeringBehaviours.Deceleration.Fast) : SteeringBehaviour.Seek(SeekablePosition);

            // Acceletartion = Force / Mass;
            var acceleration = Vector2Helper.ScalarDiv(steeringForce, Mass);

            // Update velocity
            Velocity += Vector2Helper.ScalarMul(acceleration, elapsedTime);

            // Make sure vehicle does not exceed maximum velocity
            Vector2Helper.Truncate(Velocity, MaxSpeed);

            // Update the position
            Position += Vector2Helper.ScalarMul(Velocity, elapsedTime);

            // Update the heading if the vehicle has a velocity greater
            // than a very small value
            if (Velocity.LengthSquared() > 0.00000001)
            {
                Heading = Vector2Helper.Normalize(Velocity);

                Side = Vector2Helper.Perp(Heading);
            }

            // Treat the screan as a toroid
            Position = Vector2Helper.WrapAround(Position);

            // Update sprite 
            UpdateSprite();
            AnimatedSprite.Update(elapsedTime);
        }

        public override void UpdateSprite()
        {
            if (Heading.X > 0 && Heading.Y < 0.45 && Heading.Y > -0.45 && Direction != AnimatedSprite.Direction.Right)
            {
                Direction = AnimatedSprite.Direction.Right;
                AnimatedSprite.Move(AnimatedSprite.Direction.Right);
                return;
            }
            if (Heading.X < 0 && Heading.Y < 0.45 && Heading.Y > -0.45 && Direction != AnimatedSprite.Direction.Left)
            {
                Direction = AnimatedSprite.Direction.Left;
                AnimatedSprite.Move(AnimatedSprite.Direction.Left);
                return;
            }
            if (Heading.Y > 0 && Heading.X < 0.45 && Heading.X > -0.45 && Direction != AnimatedSprite.Direction.Down)
            {
                Direction = AnimatedSprite.Direction.Down;
                AnimatedSprite.Move(AnimatedSprite.Direction.Down);
                return;
            }
            if (Heading.Y < 0 && Heading.X < 0.45 && Heading.X > -0.45 && Direction != AnimatedSprite.Direction.Up)
            {
                Direction = AnimatedSprite.Direction.Up;
                AnimatedSprite.Move(AnimatedSprite.Direction.Up);
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                AnimatedSprite.Texture,
                new Rectangle((int)Position.X - Width / 2, (int)Position.Y - Height / 2, Width, Height),
                new Rectangle(AnimatedSprite.CurrentCol * Width, AnimatedSprite.CurrentRow * Height, Width, Height),
                Color.White
                );
        }
    }
}
