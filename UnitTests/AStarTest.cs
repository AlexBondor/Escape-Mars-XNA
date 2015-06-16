using Escape_Mars_XNA.Path;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class AStarTest
    {
        [TestMethod]
        public void AStarSearch()
        {
            var graph = new Graph(20, 25);
            var start = graph.GetNodeByIndex(0);
            var end = graph.GetNodeByIndex(10);

            var aStar = new AStar(graph);

            aStar.Search(start, end);
        }
    }
}
