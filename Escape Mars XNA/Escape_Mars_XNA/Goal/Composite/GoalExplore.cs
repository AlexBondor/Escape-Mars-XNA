using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

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
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            Owner.Behaviour = GameConfig.Bvr.Explore;

            return Status;
        }

        public override void Terminate()
        {
            Owner.Behaviour = GameConfig.Bvr.Idle;
        }
    }
}
