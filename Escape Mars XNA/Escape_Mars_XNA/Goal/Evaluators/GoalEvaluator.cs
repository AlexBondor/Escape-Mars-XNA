using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    abstract class GoalEvaluator
    {
        public abstract double CalculateDesirability(MovingEntity owner);

        public abstract void SetGoal(MovingEntity owner);
    }
}
