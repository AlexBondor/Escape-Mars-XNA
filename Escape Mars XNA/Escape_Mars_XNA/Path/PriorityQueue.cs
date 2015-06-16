using System;
using System.Collections.Generic;
using System.Linq;

namespace Escape_Mars_XNA.Path
{
    public class PriorityQueue
    {
        private List<PriorityQueueElement> _queue; 

        public PriorityQueue(int numNodes)
        {
            _queue = new List<PriorityQueueElement>(numNodes);
        }

        // Enqueue a new element
        // Sort the new queue
        public void Enqueue(PriorityQueueElement element)
        {
            _queue.Add(element);
        }

        // Return and remove the first element of the queue
        public PriorityQueueElement Dequeue()
        {
            _queue = _queue.OrderBy(e => e.Cost).ToList();

            var first = _queue[0];

            _queue.Remove(first);

            return (first);
        }

        public void ChangePriority(double cost, GraphNode node)
        {
            var el = _queue.Find(e => e.Node.Index == node.Index);
            
            if (el == null) return;
            
            el.Cost = cost;
        }

        public bool IsEmpty()
        {
            return !_queue.Any();
        }
    }
}
