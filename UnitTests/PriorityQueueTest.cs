using Microsoft.VisualStudio.TestTools.UnitTesting;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;

namespace UnitTests
{
    [TestClass]
    public class PriorityQueueTest
    {
        [TestMethod]
        public void EnqueueDequeue()
        {
            var pq = new PriorityQueue(10);

            var t = new GraphNode(1, new Vector2(0, 1));
            pq.Enqueue(new PriorityQueueElement(3, t));

            t = new GraphNode(1, new Vector2(0, 2));
            pq.Enqueue(new PriorityQueueElement(2, t));

            t = new GraphNode(1, new Vector2(0, 3));
            pq.Enqueue(new PriorityQueueElement(1, t));

            var e = pq.Dequeue();

            Assert.AreEqual(e.Cost, 1);
        }
    }
}
