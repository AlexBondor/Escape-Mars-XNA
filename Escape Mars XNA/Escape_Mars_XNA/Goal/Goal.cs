using System.Collections.Generic;
using Escape_Mars_XNA.Entity;

namespace Escape_Mars_XNA.Goal
{
    abstract class Goal
    {
        public Stack<Goal> Subgoals = new Stack<Goal>();

        public enum Typ
        {
            Think = 0,
            SeekToPosition = 1,
            FollowPath = 2,
            GetHealthPack = 3,
            Explore = 4,
            NotSet = 5
        }

        public enum Sts
        {
            Inactive = 0,
            Active = 1,
            Completed = 2,
            Failed = 3,
            Halted = 4
        }

        protected MovingEntity Owner;

        public Sts Status { get; set; }

        public Typ Type { get; protected set; }

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

        protected void ActivateIfInactive()
        {
            if (IsInactive())
            {
                Activate();
            }
        }
    }
}
