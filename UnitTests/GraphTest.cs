using Microsoft.VisualStudio.TestTools.UnitTesting;
using Escape_Mars_XNA.Path;
using Microsoft.Xna.Framework;

namespace UnitTests
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void GetNode()
        {
            var graph = new Graph(5, 5);

            graph.GetNodeByPosition(new Vector2(-10, -10));
        }
    }
}
