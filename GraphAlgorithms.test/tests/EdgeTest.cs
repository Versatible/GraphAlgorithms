using NUnit.Framework;
using System;
namespace GraphAlgorithms.test
{
    // A Mock Int WeightedNode containing a simple integer
    using IntNode = Node<int>;

    [TestFixture()]
    public class EdgeTest
    {
        [Test()]
        public void TestSameOrderEquality()
        {
            var edge1 = new Edge<int>(from: new IntNode(21), to: new IntNode(42));
            var edge2 = new Edge<int>(from: new IntNode(21), to: new IntNode(42));

            Assert.AreEqual(edge1, edge2);
        }

        [Test()]
        public void TestInverseOrderInequality()
        {
            var edge1 = new Edge<int>(from: new IntNode(21), to: new IntNode(42));
            var edge2 = new Edge<int>(from: new IntNode(42), to: new IntNode(21));

            Assert.AreNotEqual(edge1, edge2);
        }

        [Test()]
        public void TestEqualityDifferentWeights()
        {
            var edge1 = new Edge<int>(from: new IntNode(21), to: new IntNode(42));
            var edge2 = new Edge<int>(from: new IntNode(21), to: new IntNode(42), weight: 13.0d);

            Assert.AreEqual(edge1, edge2);
        }

        [Test()]
        public void TestInequality()
        {
            var edge1 = new Edge<int>(from: new IntNode(21), to: new IntNode(22));
            var edge2 = new Edge<int>(from: new IntNode(21), to: new IntNode(42));

            Assert.AreNotEqual(edge1, edge2);
        }
    }
}
