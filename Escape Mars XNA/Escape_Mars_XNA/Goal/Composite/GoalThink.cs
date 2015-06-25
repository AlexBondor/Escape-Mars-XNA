using System;
using System.Collections.Generic;
using System.Linq;
using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Evaluators;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalThink:GoalComposite
    {
        public List<GoalEvaluator> Evaluators = new List<GoalEvaluator>();

        public GoalThink(MovingEntity owner)
        {
            Type = Typ.Think;
            Owner = owner;

            Evaluators.Add(new ExploreGoalEvaluator());
            Evaluators.Add(new AttackEnemyEvaluator());
            Evaluators.Add(new GetHealthPackEvaluator());
            Evaluators.Add(new GetAmmoGoalEvaluator());
            Evaluators.Add(new GetRocketPartEvaluator());
            Evaluators.Add(new ReturnToRocketEvaluator());
        }

        public override void Activate()
        {
            Status = Sts.Active;
            Arbitrate();
        }

        public override Sts Process()
        {
            ActivateIfInactive();

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
            ProcessSubgoals();

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
                if (!GoalExistsAlready(best))
                {
                    best.SetGoal(Owner);
                }
            }
        }

        private bool GoalExistsAlready(GoalEvaluator best)
        {
            return Subgoals.Any(goal => goal.Type == best.GetCorrespondentGoalType(best.Type));
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
            AddSubgoal(new GoalGetRocketPart(Owner));   
        }

        public void AddGetAmmoGoal()
        {
            AddSubgoal(new GoalGetAmmo(Owner));
        }

        public void AddAttackEnemyGoal()
        {
            AddSubgoal(new GoalAttackEnemy(Owner));
        }

        public void AddReturnToRocketGoal()
        {
            AddSubgoal(new GoalReturnToRocket(Owner));
        }
    }
}
