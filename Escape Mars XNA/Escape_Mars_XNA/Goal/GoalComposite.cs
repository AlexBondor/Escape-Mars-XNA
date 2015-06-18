using System.Collections.Generic;

namespace Escape_Mars_XNA.Goal
{
    abstract class GoalComposite : Goal
    {
        public Stack<Goal> Subgoals = new Stack<Goal>();

        public void AddSubgoal(Goal g)
        {
            Subgoals.Push(g);
        }

        public Sts ProcessSubgoals()
        {
            // Remove all completed and failed goals from the front
            // of the subgoal stack
            var first = Subgoals.Peek();
            while (Subgoals.Count != 0 && (first.IsCompleted() || first.HasFailed()))
            {
                first.Terminate();
                Subgoals.Pop();
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
