using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class ExploreGoalEvaluator:GoalEvaluator
    {
        public ExploreGoalEvaluator()
        {
            Type = Evl.Explore;
            SingleGoalInstance = true;
        }

        public override double CalculateDesirability(MovingEntity owner)
        {
            const double desirability = 0.05;

            return desirability;
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddExploreGoal();
        }
    }
}
