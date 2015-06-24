﻿using System.Collections.Generic;
using System.Linq;

namespace Escape_Mars_XNA.Goal
{
    abstract class GoalComposite : Goal
    {
        public void AddSubgoal(Goal g)
        {
            if (Subgoals.Count != 0)
            {
                HaltAllSubgoals(this);
                
            }
            Subgoals.Push(g);
        }

        private void HaltAllSubgoals(Goal goal)
        {
            foreach (var subgoal in goal.Subgoals.Where(subgoal => subgoal.Status == Sts.Active))
            {
                subgoal.Status = Sts.Halted;
                subgoal.OnHalt();
                HaltAllSubgoals(subgoal);
            }
        }

        public Sts ProcessSubgoals()
        {
            if (Subgoals.Count == 0)
            {
                return Sts.Completed;
            }

            // Remove all completed and failed goals from the front
            // of the subgoal stack
            var first = Subgoals.Peek();
            while (Subgoals.Count != 0 && (first.IsCompleted() || first.HasFailed()))
            {
                first.Terminate();
                Subgoals.Pop();
                if (Subgoals.Count == 0)
                {
                    break;
                }
                first = Subgoals.Peek();
            }

            // If any subgoals remain, process the one at the front
            // of the list
            if (Subgoals.Count != 0)
            {
                first = Subgoals.Peek();
                // Grab the status of the frontmost subgoal
                var subgoalStatus = first.Process();

                // We have to test for the special case where the
                // frontmost subgoal reports "completed" and the subgoal
                // list contains additional goals.
                // When this is the case, to ensure the parent keeps 
                // processing its subgoal list, the "active" status is
                // returned
                if (subgoalStatus == Sts.Completed && Subgoals.Count > 1)
                {
                    return Sts.Active;
                }
                return subgoalStatus;
            }
            // no more subgoals to process - return "completed"
            return Sts.Completed;
        }

        // Removes all the subgoals
        public void RemoveAllSubgoals()
        {
            // Terminate all the subgoals
            foreach (var subgoal in Subgoals)
            {
                subgoal.Terminate();
            }
            // Reinitialize the structure
            Subgoals = new Stack<Goal>();
        }
    }
}
