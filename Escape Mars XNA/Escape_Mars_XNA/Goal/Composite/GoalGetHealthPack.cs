using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalGetHealthPack:GoalComposite
    {
        private Vector2 _healthPackPosition;

        public GoalGetHealthPack(MovingEntity owner)
        {
            Type = Typ.GetHealthPack;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            var closest = Owner.World.GetClosestItemTypePosition(Owner.Position, EntityFeature.Itm.HealthPack);

            if (Vector2Helper.DistanceSq(closest, new Vector2(float.MaxValue, float.MaxValue)) < 1)
            {
                Status = Sts.Failed;
            }

            _healthPackPosition = closest;

            AddSubgoal(new GoalFollowPath(Owner, _healthPackPosition));
        }

        public override Sts Process()
        {
            if (Status == Sts.Halted)
            {
                Activate();
            }

            // Otherwise continue
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
            Owner.Health += EntityFeature.HealthPackPoints;
            Owner.RemoveItemFromPosition(_healthPackPosition);
        }
    }
}
