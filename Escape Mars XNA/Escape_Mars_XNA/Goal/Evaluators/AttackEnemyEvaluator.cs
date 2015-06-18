using System;
using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class AttackEnemyEvaluator:GoalEvaluator
    {
        public override double CalculateDesirability(MovingEntity owner)
        {
            throw new NotImplementedException();
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGoalAttackEnemy();
        }
    }
}
