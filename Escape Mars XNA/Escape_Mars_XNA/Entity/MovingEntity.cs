using Escape_Mars_XNA.Goal.Composite;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Entity
{
    abstract class MovingEntity : BaseGameEntity
    {
        // Every character starts with full Ammo
        public int Ammo = EntityFeature.MaxAmmo;

        public enum Bvr
        {
            Seek = 0,
            Arrive = 1,
            Flee = 2,
            Evade = 3,
            Hide = 4,
            Explore = 5,
            Idle = 6
        };

        // A vector for velocity
        public Vector2 Velocity;

        // A normalized vector pointing in the direction the entity is heading.
        public Vector2 Heading;

        // A vector perpendicular to the heading vector
        public Vector2 Side;

        // This entitys' mass
        public double Mass;

        // The maximum speed at which this entity may travel.
        public double MaxSpeed;

        // The maximum force this entity can produce to power itself
        // (think rockets and thrust)
        public double MaxForce;

        // The maximum rate (radians per second) at which this vehicle can rotate
        public double MaxTurnRate;

        public Bvr Behaviour { get; set; }
        
        // General Position used by Steering Behaviour
        // Set this for the Seek, Arrive, Flee or other
        // steering behaviours
        public Vector2 SteeringPosition { get; set; }

        // The enemy is used to Hide/Flee from.. or Attack
        public MovingEntity Enemy { get; set; }

        public bool HasEnemy { get; set; }

        public bool CanPickUp { get; set; }

        // Used by the Hide function to find hiding spots
        public GraphNode[] Obstacles { get; set; }
        public SteeringBehaviours.Dcl Deceleration { get; set; }

        public GoalThink Brain { get; set; }

        // Steering behaviour
        public SteeringBehaviours SteeringBehaviour { get; set; }

        // Path planning
        public PathPlanning PathPlanning { get; set; }

        public void CreatePathTo(Vector2 to)
        {
            PathPlanning.CreatePath(Position, to);
        }

        // Update the path planning
        protected void UpdatePathPlanning()
        {
            PathPlanning.Update();
        }

        // Update direction of the sprite and also get the frame corresponding
        // to current elapsed time
        protected void UpdateSprite(double elapsedTime)
        {
            var prevDir = Direction;
            //Console.WriteLine(Direction + " --- " + Heading);
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

            AnimatedSprite.Update(elapsedTime);
        }
    
        // Update the physics of the game object
        protected void UpdatePhysics(double elapsedTime)
        {
            // Calculate the combined force from each steering behavior
            // in the vehicles' list
            Vector2 steeringForce;
            switch (Behaviour)
            {
                case Bvr.Seek:
                    steeringForce = SteeringBehaviour.Seek(SteeringPosition);
                    break;
                case Bvr.Arrive:
                    steeringForce = SteeringBehaviour.Arrive(SteeringPosition, Deceleration);
                    break;
                case Bvr.Flee:
                    steeringForce = SteeringBehaviour.Flee(SteeringPosition);
                    break;
                case Bvr.Evade:
                    steeringForce = SteeringBehaviour.Evade(Enemy);
                    break;
                case Bvr.Hide:
                    steeringForce = SteeringBehaviour.Hide(Enemy, Obstacles);
                    break;
                case Bvr.Explore:
                    steeringForce = SteeringBehaviour.Explore(SteeringPosition);
                    break;
                case Bvr.Idle:
                    return;
                default:
                    return;
            };

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
        }
    }
}
