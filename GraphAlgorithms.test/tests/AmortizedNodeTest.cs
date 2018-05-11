using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Int WeightedNode containing a simple integer
    using IntNode = AmortizedNode<int>;

    [TestFixture()]
    public class AmortizedNodeTest
    {
        [Test()]
        public void TestSingleValue()
        {
            var root = new IntNode(42);
            var neighbor = new IntNode(21);
            root.AddDirectedEdge(neighbor, 10);

            Assert.AreEqual(10, root.Cost(neighbor));
        }

        [Test()]
        public void TestAverageValue()
        {
            var root = new IntNode(42);
            var neighbor = new IntNode(21);

            // Single
            root.AddDirectedEdge(neighbor, 10);
            Assert.AreEqual(10, root.Cost(neighbor));

            // Flood
            for (int i = 0; i < root.QueueSize; i++)
            {
                root.UpdateDirectedEdge(neighbor, 20);
            }
            Assert.AreEqual(20, root.Cost(neighbor));

            // Suite
            for (int i = 1; i <= root.QueueSize; i++)
            {
                root.UpdateDirectedEdge(neighbor, i);
            }
            var Σ = (1 + root.QueueSize) / 2;
            Assert.AreEqual(Σ, root.Cost(neighbor));

        }
    }
}
