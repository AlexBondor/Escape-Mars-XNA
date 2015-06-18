using Escape_Mars_XNA.Path;

namespace Escape_Mars_XNA.Helper
{
    public class PriorityQueueElement
    {
        public double Cost { get; set; }
        public GraphNode Node { get; set; }


        public PriorityQueueElement(double cost, GraphNode node)
        {
            Cost = cost;
            Node = node;
        }
    }
}