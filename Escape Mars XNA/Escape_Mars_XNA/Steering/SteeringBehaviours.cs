using System;
using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Steering
{
    class SteeringBehaviours
    {
        public enum Dcl
        {
            Slow = 3,
            Normal = 2,
            Fast = 1
        };

        private Vector2 _lastHidingSpot = Vector2.Zero;

        private readonly MovingEntity _owner;

        public SteeringBehaviours(MovingEntity owner)
        {
            _owner = owner;
        }

        // Steers the agen in such a way it decelerates onto the target
        // position
        public Vector2 Arrive(Vector2 posTarget, Dcl deceleration)
        {
            var toTarget = posTarget - _owner.Position;

            // Calculate the distance to target position
            double dist = toTarget.Length();

            if (dist < 1)
            {
                _owner.Velocity = Vector2.Zero;
                return Vector2.Zero;
            }

            // Because Deceleration is enumerated as an int,
            // this value is required to provide fine tweaking
            // of the deceleration
            const double decelerationTweaker = 1;

            // Calculate the speed required to reach the target
            // given the desired deceleration
            var speed = dist / (double)deceleration * decelerationTweaker;

            // Make sure the velocity does not exceed the max
            speed = speed < _owner.MaxSpeed ? speed : _owner.MaxSpeed;

            // From here proceed just like Seek except we don't need
            // to normalize the toTarget vector because we have already
            // gone to the trouble of calculating its length: dist
            var desiredVelocity = Vector2Helper.ScalarMul(toTarget, speed / dist);

            return desiredVelocity - _owner.Velocity;
        }

        // Returns a force that directs an agent toward
        // a target position
        public Vector2 Seek(Vector2 posTarget)
        {
            var deltaPos = posTarget - _owner.Position;

            if (deltaPos.LengthSquared() < 1)
            {
                _owner.Velocity = Vector2.Zero;
                return Vector2.Zero;
            }

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

            if (_lastHidingSpot != bestHidingSpot)
            {
                _lastHidingSpot = bestHidingSpot;
                _owner.PathPlanning.CreatePath(_owner.Position, bestHidingSpot);
            }

            // If no suitable obstacle found then evade the target
            // else Arrive to the closest hiding spot
            return Math.Abs(distToClosest - double.MaxValue) < double.Epsilon ? Evade(target) : Seek(_owner.SteeringPosition);
        }

        public Vector2 Evade(MovingEntity pursuer)
        {
            var toPursuer = pursuer.Position - _owner.Position;

            // The look-ahead time is proportional to the distance
            // between the pursuer and the evader; and is inversely
            // proportional to the sum of the agents' velocities
            var lookAheadTime = toPursuer.Length() / (_owner.MaxSpeed + pursuer.MaxSpeed);

            // Now flee away from the predicted future position of
            // the pursuer
            return Flee(Vector2Helper.ScalarMul(pursuer.Position + pursuer.Velocity, lookAheadTime));
        }

        // Return a hiding position
        private static Vector2 GetHidingPosition(Vector2 posTarget, Vector2 posOb, double radius)
        {
            // Calculate how far away the agent is to be from the
            // chosen obstacle's bounding radius
            const double distanceFromBoundary = 100.0;

            var distAway = radius + distanceFromBoundary;

            // Calculate the heading toward the object from target
            var toOb = Vector2Helper.Normalize(posOb - posTarget);

            // Scale it to size and add to the obstacle's position
            // to get the hiding spot
            return (Vector2Helper.ScalarMul(toOb, distAway)) + posOb;
        }

        public Vector2 Explore(Vector2 posTarget)
        {
            if (!(Vector2Helper.DistanceSq(posTarget, _owner.Position) < 5))
            {
                return Seek(posTarget);
            }
            _owner.SteeringPosition = GetExploringPosition();
            _owner.CreatePathTo(_owner.SteeringPosition);

            return Vector2.Zero;
        }

        private Vector2 GetExploringPosition()
        {
            var currentPosition = _owner.Position;

            // ReSharper disable PossibleLossOfFraction
            return currentPosition.X < Vector2Helper.WindowWidth / 2 ? NextExploringPosition(currentPosition.Y < Vector2Helper.WindowHeight / 2 ? 0 : 3) : NextExploringPosition(currentPosition.Y < Vector2Helper.WindowHeight / 2 ? 1 : 2);
        }

        private Vector2 NextExploringPosition(int quadrant)
        {
            quadrant = ++quadrant % 4; 
            // Get active nodes from map
            var map = _owner.World.MapGraph;

            switch (quadrant)
            {
                case 0:
                    return ValidExploringNode(
                        0,
                        map.Rows % 2 == 0 ? (map.Rows - 1) / 2 : map.Rows / 2,
                        0,
                        map.Cols % 2 == 0 ? (map.Cols - 1) / 2 : map.Cols / 2);
                case 1:
                    return ValidExploringNode(
                        0,
                        map.Rows % 2 == 0 ? (map.Rows - 1) / 2 : map.Rows / 2,
                        map.Cols % 2 == 0 ? (map.Cols - 1) / 2 : map.Cols / 2,
                        map.Cols - 1);
                case 2:
                    return ValidExploringNode(
                        map.Rows % 2 == 0 ? (map.Rows - 1) / 2 : map.Rows / 2,
                        map.Rows - 1,
                        map.Cols % 2 == 0 ? (map.Cols - 1) / 2 : map.Cols / 2,
                        map.Cols - 1);
                case 3:
                    return ValidExploringNode(
                        map.Rows % 2 == 0 ? (map.Rows - 1) / 2 : map.Rows / 2,
                        map.Rows - 1,
                        0,
                        map.Cols % 2 == 0 ? (map.Cols - 1) / 2 : map.Cols / 2);
            }
            return _owner.Position;
        }

        private Vector2 ValidExploringNode(int rowLeft, int rowRight, int colLeft, int colRight)
        {
            var nodes = _owner.World.MapGraph.Nodes.Where(n => n.Active).ToArray();

            var rdn = new Random();

            GraphNode temp;

            while (true)
            {
                var row = rdn.Next(rowLeft, rowRight);
                var col = rdn.Next(colLeft, colRight);
                try
                {
                    temp = nodes.First(n => n.Row == row && n.Col == col);
                    return temp.Position;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
