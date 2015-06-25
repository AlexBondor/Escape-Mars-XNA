using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class GetAmmoGoalEvaluator:GoalEvaluator
    {
        public GetAmmoGoalEvaluator()
        {
            Type = Evl.GetAmmo;
        }

        public override double CalculateDesirability(MovingEntity owner)
        {
            var distance = EntityFeature.DistanceToItem(owner, BaseGameEntity.Itm.Ammo);

            if (Math.Abs(distance - 1) < double.Epsilon)
            {
                return 0;
            }

            const double tweaker = 0.03;

            var health = EntityFeature.Health(owner);

            var weaponStrength = EntityFeature.WeaponStrength(owner);

            var desirability = (tweaker*health*(1 - weaponStrength))/distance;

            return Vector2Helper.Clamp(desirability, 0, 1);
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGetAmmoGoal();
        }
    }
}
