using System;
using Escape_Mars_XNA.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace UnitTests
{
    [TestClass]
    public class Vector2HelperTest
    {
        [TestMethod]
        public void ScalarOperations()
        {
            var vector = new Vector2(0, 0);

            vector = Vector2Helper.ScalarAdd(vector, 1);
            Assert.AreEqual(vector, new Vector2(1, 1));

            vector = Vector2Helper.ScalarSub(vector, -2);
            Assert.AreEqual(vector, new Vector2(3, 3));

            vector = Vector2Helper.ScalarMul(vector, 3);
            Assert.AreEqual(vector, new Vector2(9, 9));

            vector = Vector2Helper.ScalarDiv(vector, 9);
            Assert.AreEqual(vector, new Vector2(1, 1));
        }

        [TestMethod]
        public void Truncate()
        {
            var vector = new Vector2(4, 4);

            vector = Vector2Helper.Truncate(vector, 4);
            Assert.IsTrue(vector.Length() - Math.Sqrt(32) < double.Epsilon);

            vector = Vector2Helper.Truncate(vector, 2);
            Assert.IsTrue(vector.Length() - Math.Sqrt(8) < double.Epsilon);
        }
    }
}
