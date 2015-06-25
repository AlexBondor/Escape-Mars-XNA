using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Path;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalReturnRocketPart:GoalComposite
    {
        private BaseGameEntity _rocketPart;

        public GoalReturnRocketPart(MovingEntity owner, BaseGameEntity rocketPart)
        {
            Type = Typ.ReturnRocketPart;
            Owner = owner;
            _rocketPart = rocketPart;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            _rocketPart.PickedUp = true;
            _rocketPart.FollowMe(Owner);

            AddSubgoal(new GoalFollowPath(Owner, Owner.World.Rocket.Position));
        }

        public override Sts Process()
        {
            if (Status == Sts.Halted)
            {
                Activate();
            }

            ActivateIfInactive();
            
            var subgoalStatus = ProcessSubgoals();

            if (subgoalStatus != Sts.Completed && subgoalStatus != Sts.Failed) return Status;

            Status = Sts.Completed;
            Owner.World.ObjectsToBeRemoved.Add(_rocketPart);
            Owner.World.RocketPartsCount--;
            if (Owner.World.RocketPartsCount == 0)
            {
                Owner.World.Paused = true;
                Owner.World.GameWon();
            }

            return Status;
        }

        public override void OnHalt()
        {
            RemoveAllSubgoals();

            _rocketPart.PickedUp = false;

            var node = _rocketPart.World.MapGraph.GetNodeByPosition(_rocketPart.Position);

            _rocketPart.Position = node.Position;
        }

        public override void Terminate()
        {
        }
    }
}
