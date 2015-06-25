using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class AttackEnemyEvaluator:GoalEvaluator
    {
        public AttackEnemyEvaluator()
        {
            Type = Evl.AttackEnemy;
        }

        public override double CalculateDesirability(MovingEntity owner)
        {
            var desirability = 0.0;

            // Only do the calculation if there is a target present
            if (owner.HasEnemy)
            {
                const double tweaker = 1.0;

                var health = EntityFeature.Health(owner);
                var weaponStrength = EntityFeature.WeaponStrength(owner);

                desirability = tweaker*health*weaponStrength;
            }
            return desirability;
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddAttackEnemyGoal();
        }
    }
}
