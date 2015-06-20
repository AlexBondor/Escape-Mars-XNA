using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalExplore:GoalComposite
    {
        public GoalExplore(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            Owner.Behaviour = MovingEntity.Bvr.Explore;
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            return Status;
        }

        public override void Terminate()
        {
            Owner.Behaviour = MovingEntity.Bvr.Idle;
        }
    }
}
