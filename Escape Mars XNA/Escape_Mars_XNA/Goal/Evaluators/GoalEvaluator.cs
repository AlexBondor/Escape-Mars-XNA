using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    abstract class GoalEvaluator
    {
        public bool SingleGoalInstance { get; set; }

        public enum Evl
        {
            AttackEnemy = 0,
            Explore = 1,
            GetAmmo = 2,
            GetHealthPack = 3,
            GetRocketPart = 4
        }

        public Evl Type { get; protected set; }

        public abstract double CalculateDesirability(MovingEntity owner);

        public abstract void SetGoal(MovingEntity owner);

        public Goal.Typ GetCorrespondentGoalType(Evl type)
        {
            switch (type)
            {
                case Evl.Explore:
                    return Goal.Typ.Explore;
                case Evl.GetHealthPack:
                    return Goal.Typ.GetHealthPack;
            }
            return Goal.Typ.NotSet;
        }
    }
}
