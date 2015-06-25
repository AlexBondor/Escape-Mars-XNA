using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Steering;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Atomic
{
    class GoalSeekToPosition:Goal
    {
        private readonly Vector2 _steeringPosition;

        public GoalSeekToPosition(MovingEntity owner, Vector2 steeringPosition)
        {
            Type = Typ.SeekToPosition;
            Owner = owner;
            // Position to seek to
            _steeringPosition = steeringPosition;
        }

        public override void Activate()
        {
            Status = Sts.Active;
            // Set entity behaviour to seeking
            Owner.Behaviour = SteeringBehaviours.Bvr.Seek;
            // Set the entity's steering position
            Owner.SteeringPosition = _steeringPosition;
        }

        public override Sts Process()
        {
            // If status is inactive, call Activate() and set status
            // to active
            ActivateIfInactive();

            if (Status == Sts.Halted)
            {
                Activate();
            }

            // If entity arrived at the specified point
            if (Vector2Helper.DistanceSq(Owner.Position, _steeringPosition) < 5)
            {
                Status = Sts.Completed;
            }

            return Status;
        }

        public override void Terminate()
        {
        }
    }
}
