using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Atomic
{
    class GoalSeekToPosition:Goal
    {
        private readonly Vector2 _steeringPosition;

        public GoalSeekToPosition(MovingEntity owner, Vector2 steeringPosition)
        {
            Owner = owner;
            _steeringPosition = steeringPosition;
        }

        public override void Activate()
        {
            Status = Sts.Active;
            
            Owner.Behaviour = MovingEntity.Bvr.Seek;
            Owner.SteeringPosition = _steeringPosition;
        }

        public override Sts Process()
        {
            // If status is inactive, call Activate() and set status
            // to active
            ActivateIfInactive();

            if (Vector2Helper.DistanceSq(Owner.Position, _steeringPosition) < 20)
            {
                Status = Sts.Completed;
            }

            return Status;
        }

        public override void Terminate()
        {
            //Owner.Behaviour = MovingEntity.Bvr.Idle;
        }
    }
}
