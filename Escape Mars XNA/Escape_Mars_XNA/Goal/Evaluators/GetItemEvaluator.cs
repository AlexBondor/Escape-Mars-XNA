﻿using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class GetItemEvaluator:GoalEvaluator
    {
        public override double CalculateDesirability(MovingEntity owner)
        {
            throw new System.NotImplementedException();
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddGoalGetItem();
        }
    }
}
