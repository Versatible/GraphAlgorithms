using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Int WeightedNode containing a simple integer
    using IntNode = Node<int>;

    [TestFixture()]
    public class UndirectedEdgeTest
    {
        [Test()]
        public void TestSameOrderEquality()
        {
            var edge1 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42));
            var edge2 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42));

            Assert.AreEqual(edge1, edge2);
        }

        [Test()]
        public void TestEqualityDifferentWeights()
        {
            var edge1 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42));
            var edge2 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42), weight: 13.0d);

            Assert.AreEqual(edge1, edge2);
        }

        [Test()]
        public void TestInequality()
        {
            var edge1 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(22));
            var edge2 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42));

            Assert.AreNotEqual(edge1, edge2);
        }

        [Test()]
        public void TestInverseOrderEquality()
        {
            var edge1 = new UndirectedEdge<int>(between: new IntNode(21), and: new IntNode(42));
            var edge2 = new UndirectedEdge<int>(between: new IntNode(42), and: new IntNode(21));

            Assert.AreEqual(edge1, edge2);
        }
    }
}
