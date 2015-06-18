using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Atomic
{
    class GoalSeekToPosition:Goal
    {
        public GoalSeekToPosition(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;
            Owner.Behaviour = MovingEntity.Bvr.Seek;
        }

        public override Sts Process()
        {
            // If status is inactive, call Activate() and set status
            // to active
            ActivateIfInactive();

            return Status;
        }

        public override void Terminate()
        {
            Owner.Behaviour = MovingEntity.Bvr.Idle;
        }
    }
}
