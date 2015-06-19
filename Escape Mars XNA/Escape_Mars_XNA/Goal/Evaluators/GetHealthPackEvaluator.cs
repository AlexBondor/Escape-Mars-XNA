using System;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class GetHealthPackEvaluator : GoalEvaluator
    {
        public override double CalculateDesirability(MovingEntity owner)
        {
            // First grab the distance tot the closest instance of
            // a health item
            var distance = EntityFeature.DistanceToItem(owner, EntityFeature.Itm.HealthPack);

            // If distance feature is rated with a value of 1 it means
            // that the item is either not present on the map or far
            // away to be worth considering, therefore the desirability
            // is 0
            if (Math.Abs(distance - 1) < double.Epsilon)
            {
                return 0;
            }

            const double tweaker = 0.2;

            var desirability = tweaker * (1 - EntityFeature.Health(owner)) / distance;
            
            return Vector2Helper.Clamp(desirability, 0, 1);
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGetHealthPackGoal();
        }
    }
}
