using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Steering
{
    class SteeringBehaviours
    {
        public enum Deceleration
        {
            Slow = 3,
            Normal = 2,
            Fast = 1
        };

        private readonly MovingEntity _owner;

        public SteeringBehaviours(MovingEntity owner)
        {
            _owner = owner;
        }

        // Steers the agen in such a way it decelerates onto the target
        // position
        public Vector2 Arrive(Vector2 posTarget, Deceleration deceleration)
        {
            var toTarget = posTarget - _owner.Position;

            // Calculate the distance to target position
            double dist = toTarget.Length();

            if (dist < 1)
            {
                _owner.Arrived = true;
                _owner.Velocity = Vector2.Zero;
                return Vector2.Zero;
            }

            // Because Deceleration is enumerated as an int,
            // this value is required to provide fine tweaking
            // of the deceleration
            const double decelerationTweaker = 1;

            // Calculate the speed required to reach the target
            // given the desired deceleration
            var speed = dist/(double) deceleration*decelerationTweaker;
                
            // Make sure the velocity does not exceed the max
            speed = speed < _owner.MaxSpeed ? speed : _owner.MaxSpeed;

            // From here proceed just like Seek except we don't need
            // to normalize the toTarget vector because we have already
            // gone to the trouble of calculating its length: dist
            var desiredVelocity = Vector2Helper.ScalarMul(toTarget, speed/dist);

            return desiredVelocity - _owner.Velocity;
        }

        // Returns a force that directs an agent toward
        // a target position
        public Vector2 Seek(Vector2 posTarget)
        {
            var deltaPos = posTarget - _owner.Position;
            
            if (deltaPos.Length() <= double.Epsilon) return Vector2.Zero;

            deltaPos.Normalize();

            var desiredVelocity = Vector2Helper.ScalarMul(deltaPos, _owner.MaxSpeed);
            
            return desiredVelocity - _owner.Velocity;
        }

        // Opposite of seek
        public Vector2 Flee(Vector2 posTarget)
        {
            var deltaPos = _owner.Position - posTarget;

            if ((_owner.Position - posTarget).Length() > 100) return Vector2.Zero;

            deltaPos.Normalize();

            var desiredVelocity = Vector2Helper.ScalarMul(deltaPos, _owner.MaxSpeed);

            return desiredVelocity - _owner.Velocity;
        }

        public Vector2 Hide(MovingEntity target, GraphNode[] obstacles)
        {
            var distToClosest = double.MaxValue;

            var bestHidingSpot = new Vector2();

            foreach (var obstacle in obstacles)
            {
                // Calculate the position of the hiding spot for this
                // obstacle
                var hidingSpot = GetHidingPosition(target.Position, obstacle.Position, obstacle.Width);

                // Work in distance squared space to find the closest 
                // hiding spot to the agent
                var dist = Vector2Helper.DistanceSq(hidingSpot, _owner.Position);

                if (!(dist < distToClosest)) continue;

                distToClosest = dist;

                bestHidingSpot = hidingSpot;
            }

            _owner.HidingSpot = bestHidingSpot;
 
            // If no suitable obstacle found then evade the target
            // else Arrive to the closest hiding spot
            return Math.Abs(distToClosest - double.MaxValue) < double.Epsilon ? Evade(target) : Arrive(bestHidingSpot, Deceleration.Fast);
        }

        private Vector2 Evade(MovingEntity pursuer)
        {
            var toPursuer = pursuer.Position - _owner.Position;

            // The look-ahead time is proportional to the distance
            // between the pursuer and the evader; and is inversely
            // proportional to the sum of the agents' velocities
            var lookAheadTime = toPursuer.Length()/(_owner.MaxSpeed + pursuer.MaxSpeed);

            // Now flee away from the predicted future position of
            // the pursuer
            return Flee(Vector2Helper.ScalarMul(pursuer.Position + pursuer.Velocity, lookAheadTime));
        }

        // Return a hiding position
        private static Vector2 GetHidingPosition(Vector2 posTarget, Vector2 posOb, double radius)
        {
            // Calculate how far away the agent is to be from the
            // chosen obstacle's bounding radius
            const double distanceFromBoundary = 30.0;

            var distAway = radius + distanceFromBoundary;
        
            // Calculate the heading toward the object from target
            var toOb = Vector2Helper.Normalize(posOb - posTarget);

            // Scale it to size and add to the obstacle's position
            // to get the hiding spot
            return (Vector2Helper.ScalarMul(toOb, distAway)) + posOb;
        }
    }
}
