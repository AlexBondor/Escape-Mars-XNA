using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal
{
    abstract class Goal
    {
        public enum Typ
        {
            Think = 0,
            TraverseEdge = 1,
            FollowPath = 2,
            GetItem = 3
        }

        public enum Sts
        {
            Inactive = 0,
            Active = 1,
            Completed = 2,
            Failed = 3
        }

        protected MovingEntity Owner;

        public Sts Status { get; set; }

        public Typ Type { get; set; }

        public abstract void Activate();

        public abstract Sts Process();

        public abstract void Terminate();

        public bool IsActive()
        {
            return Status == Sts.Active;
        }

        public bool IsInactive()
        {
            return Status == Sts.Inactive;
        }

        public bool IsCompleted()
        {
            return Status == Sts.Completed;
        }

        public bool HasFailed()
        {
            return Status == Sts.Failed;
        }

        public Typ GetGoalType()
        {
            return Type;
        }

        protected void ActivateIfInactive()
        {
            if (IsInactive())
            {
                Activate();
            }
        }
    }
}
