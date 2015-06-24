using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalGetRocketPart:GoalComposite
    {
        private BaseGameEntity _rocketPart;

        public GoalGetRocketPart(MovingEntity owner)
        {
            Type = Typ.GetRocketPart;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            var closest = Owner.World.GetClosestItemTypePosition(Owner.Position, EntityFeature.Itm.RocketPart);

            _rocketPart = Owner.World.Objects.First(o => o.Position == closest);

            if (_rocketPart == null)
            {
                Status = Sts.Failed;
            }
            else
            {
                RemoveAllSubgoals();
                AddSubgoal(new GoalFollowPath(Owner, _rocketPart.Position));
            }
        }

        public override Sts Process()
        {
            if (Status == Sts.Halted)
            {
                Activate();
            }

            // Otherwise continue
            ActivateIfInactive();

            var distanceSq = Vector2Helper.DistanceSq(Vector2Helper.ScalarSub(Owner.Position, _rocketPart.Width/2),
                _rocketPart.Position);

            if (distanceSq < 5 && !_rocketPart.PickedUp)
            {
                RemoveAllSubgoals();
                AddSubgoal(new GoalReturnRocketPart(Owner, _rocketPart));
            }

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
