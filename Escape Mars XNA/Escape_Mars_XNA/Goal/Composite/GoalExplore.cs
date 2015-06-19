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
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            Owner.Behaviour = MovingEntity.Bvr.Explore;

            return Status;
        }

        public override void Terminate()
        {
            throw new System.NotImplementedException();
        }
    }
}
