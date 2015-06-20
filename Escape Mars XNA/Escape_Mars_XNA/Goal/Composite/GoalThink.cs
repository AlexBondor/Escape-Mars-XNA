﻿using System;
using System.Collections.Generic;
using System.Net;
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

            Evaluators.Add(new ExploreGoalEvaluator());
            Evaluators.Add(new AttackEnemyEvaluator());
            Evaluators.Add(new GetHealthPackEvaluator());
            Evaluators.Add(new GetAmmoGoalEvaluator());
            Evaluators.Add(new GetRocketPartEvaluator());
        }

        public override void Activate()
        {
            Status = Sts.Active;
            Arbitrate();
        }

        public override Sts Process()
        {
            ActivateIfInactive();

            Console.WriteLine("caca " + Subgoals.Count);
            var subgoalStatus = ProcessSubgoals();

            if (subgoalStatus == Sts.Completed || subgoalStatus == Sts.Failed)
            {
                Status = Sts.Inactive;
            }

            return Status;
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
                if (Subgoals.Count != 0)
                {
                    var first = Subgoals.Peek();
                    if (first.GetType().ToString().Contains("GoalExplore") && best.GetType().ToString().Contains("ExploreGoalEvaluator"))
                    {
                        return;
                    }  
                }
                
                best.SetGoal(Owner);
            }
        }

        public void AddExploreGoal()
        {
            AddSubgoal(new GoalExplore(Owner));   
        }

        public void AddGetHealthPackGoal()
        {
            AddSubgoal(new GoalGetHealthPack(Owner));
        }

        public void AddGetRocketPartGoal()
        {
            
        }

        public void AddGetAmmoGoal()
        {
            
        }

        public void AddAttackEnemyGoal()
        {
            
        }
    }
}
