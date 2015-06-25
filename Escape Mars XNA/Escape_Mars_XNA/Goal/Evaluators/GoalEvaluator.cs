using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Helper;

namespace Escape_Mars_XNA.Goal.Evaluators
{
    abstract class GoalEvaluator
    {
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
                case Evl.AttackEnemy:
                    return Goal.Typ.AttackEnemy;
                case Evl.GetAmmo:
                    return Goal.Typ.GetAmmo;
                case Evl.GetRocketPart:
                    return Goal.Typ.GetRocketPart;
            }
            return Goal.Typ.NotSet;
        }
    }
}
