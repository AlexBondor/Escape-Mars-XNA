using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    class ReturnToRocketEvaluator:GoalEvaluator
    {
        public override double CalculateDesirability(MovingEntity owner)
        {
            if (owner.ItemType == BaseGameEntity.Itm.Laika)
            {
                if (owner.World.Robot != null && !owner.World.Robot.HasEnemy)
                {
                    return 1;
                }
            }
            return 0;
        }

        public override void SetGoal(MovingEntity owner)
        {
            owner.Brain.AddReturnToRocketGoal();
        }
    }
}
