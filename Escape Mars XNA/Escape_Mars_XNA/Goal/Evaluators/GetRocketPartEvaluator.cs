using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class GetRocketPartEvaluator:GoalEvaluator
    {
        public GetRocketPartEvaluator()
        {
            Type = Evl.GetRocketPart;
            SingleGoalInstance = false;
        }

        public override double CalculateDesirability(MovingEntity owner)
        {
            var desirability = 0.0;

            if (!owner.CanPickUp)
            {
                return 0;
            }

            // If it has no enemy than you go for the rocket part
            if (!owner.HasEnemy)
            {
                return 1;
            }
            const double tweaker = 1.0;

            var health = EntityFeature.Health(owner);
            var weaponStrength = EntityFeature.WeaponStrength(owner);
            var distance = EntityFeature.DistanceToItem(owner, EntityFeature.Itm.RocketPart);

            desirability = tweaker * health * weaponStrength / distance;

            return Vector2Helper.Clamp(desirability, 0, 1);
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGetRocketPartGoal();
        }
    }
}
