using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class GetRocketPartEvaluator:GoalEvaluator
    {
        public GetRocketPartEvaluator()
        {
            Type = Evl.GetRocketPart;
        }

        public override double CalculateDesirability(MovingEntity owner)
        {
            if (!owner.CanPickUp || owner.World.RocketPartsCount == 0)
            {
                return 0;
            }

            // If it has no enemy than you go for the rocket part
            if (!owner.HasEnemy)
            {
                return 1;
            }
            const double tweaker = 0.05;

            var health = EntityFeature.Health(owner);
            var weaponStrength = EntityFeature.WeaponStrength(owner);
            var distance = EntityFeature.DistanceToItem(owner, BaseGameEntity.Itm.RocketPart);

            var desirability = tweaker * health * weaponStrength / distance;

            return Vector2Helper.Clamp(desirability, 0, 1);
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGetRocketPartGoal();
        }
    }
}
