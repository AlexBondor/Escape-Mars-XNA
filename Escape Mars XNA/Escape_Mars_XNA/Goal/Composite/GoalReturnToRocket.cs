using System;
using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalReturnToRocket:GoalComposite
    {
        public GoalReturnToRocket(MovingEntity owner)
        {
            Type = Typ.ReturnToRocket;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;
            AddSubgoal(new GoalFollowPath(Owner, Owner.World.Rocket.Position));
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            var subgoalStatus = ProcessSubgoals();

            if (subgoalStatus == Sts.Completed || subgoalStatus == Sts.Failed)
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
