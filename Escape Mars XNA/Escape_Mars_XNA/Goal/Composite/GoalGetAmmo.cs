using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalGetAmmo:GoalComposite
    {
         private Vector2 _ammoPosition;

        public GoalGetAmmo(MovingEntity owner)
        {
            Type = Typ.GetAmmo;
            Owner = owner;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            var closest = Owner.World.GetClosestItemTypePosition(Owner.Position, BaseGameEntity.Itm.Ammo);

            if (Vector2Helper.DistanceSq(closest, new Vector2(float.MaxValue, float.MaxValue)) < 1)
            {
                Status = Sts.Failed;
            }

            _ammoPosition = closest;

            AddSubgoal(new GoalFollowPath(Owner, _ammoPosition));
        }

        public override Sts Process()
        {
            if (Status == Sts.Halted)
            {
                RemoveAllSubgoals();
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
            Owner.Ammo += GameConfig.AmmoPoints;
            Owner.RemoveItemOfTypeFromPosition(_ammoPosition, BaseGameEntity.Itm.Ammo);
        }
    }
}
