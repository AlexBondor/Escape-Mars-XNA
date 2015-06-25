using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Escape_Mars_XNA.Steering;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalExplore:GoalComposite
    {
        public GoalExplore(MovingEntity owner)
        {
            Type = Typ.Explore;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            Owner.Behaviour = SteeringBehaviours.Bvr.Explore;
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            return Status;
        }

        public override void Terminate()
        {
            Owner.Behaviour = SteeringBehaviours.Bvr.Idle;
        }
    }
}
