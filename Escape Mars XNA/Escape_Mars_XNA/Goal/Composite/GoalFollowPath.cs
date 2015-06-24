﻿using Escape_Mars_XNA.Entity;
using Escape_Mars_XNA.Goal.Atomic;
using Escape_Mars_XNA.Helper;
using Microsoft.Xna.Framework;

namespace Escape_Mars_XNA.Goal.Composite
{
    class GoalFollowPath:GoalComposite
    {
        private readonly Vector2 _steeringPosition;

        public GoalFollowPath(MovingEntity owner, Vector2 steeringPosition)
        {
            Type = Typ.FollowPath;
            Owner = owner;
            _steeringPosition = steeringPosition;
        }

        public override void Activate()
        {
            Status = Sts.Active;

            if (Subgoals.Count != 0)
            {
                RemoveAllSubgoals();
            }

            var succeded = Owner.PathPlanning.CreatePath(Owner.Position, _steeringPosition);

            if (!succeded)
            {
                Status = Sts.Failed;
                return;
            }

            var aStar = Owner.PathPlanning.GetAStar();

            var path = aStar.GetPath();

            path.Reverse();

            foreach (var edge in path)
            {
                AddSubgoal(new GoalSeekToPosition(Owner, Vector2Helper.ScalarAdd(edge.To.Position, edge.To.Width / 2)));
            }
        }

        public override Sts Process()
        {
            if (Status == Sts.Halted)
            {
                RemoveAllSubgoals();
                Activate();
            }

            if (Subgoals.Count == 1)
            {
                var i = Subgoals;
            }

            // Otherwise continue
            ActivateIfInactive();

            var subgoalStatus = ProcessSubgoals();
            
            if (subgoalStatus == Sts.Completed || subgoalStatus == Sts.Failed)
            {
                Status = Sts.Completed;
            }

            return Status;
        }

        public override void Terminate()
        {
        }
    }
}
