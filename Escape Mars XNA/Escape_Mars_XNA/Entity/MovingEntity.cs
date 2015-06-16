using Escape_Mars_XNA.Path;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Entity
{
    abstract class MovingEntity : BaseGameEntity
    {
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

        public Vector2 SeekablePosition { get; set; }

        public Vector2 HidingSpot { get; set; }

        // Steering behaviour
        public SteeringBehaviours SteeringBehaviour { get; set; }

        // Path planning
        public PathPlanning PathPlanning { get; set; }

        public bool Arriving;

        public bool Arrived;

        public void GoTo(Vector2 to)
        {
            PathPlanning.CreatePath(Position, to);
        }
    }
}
