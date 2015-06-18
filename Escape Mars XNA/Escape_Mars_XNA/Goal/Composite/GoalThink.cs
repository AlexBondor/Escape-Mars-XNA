using System;
using System.Collections.Generic;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Evaluators;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalThink:GoalComposite
    {
        public List<GoalEvaluator> Evaluators = new List<GoalEvaluator>();
 
        public GoalThink(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override Sts Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        public void Arbitrate()
        {
            var highest = double.MinValue;

            GoalEvaluator best = null;

            foreach (var evaluator in Evaluators)
            {
                var score = evaluator.CalculateDesirability(Owner);
                if (score > highest)
                {
                    best = evaluator;
                    highest = score;
                }
            }

            if (best != null)
            {
                best.SetGoal(Owner);
            }
        }

        public void AddGoalExplore()
        {
            
        }

        public void AddGoalGetItem()
        {
            
        }

        public void AddGoalAttackEnemy()
        {
            
        }
    }
}
