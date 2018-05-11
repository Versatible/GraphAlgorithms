using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Int WeightedNode containing a simple integer
    using IntNode = Node<int>;

    [TestFixture()]
    public class NodeTest
    {
        [Test()]
        public void TestAPI()
        {
            var root = new IntNode(42);
            Assert.AreEqual(42, root.Content);
            Assert.AreEqual(0, root.NeighborsCount);
            Assert.IsNull(root.Find(42));
            Assert.IsFalse(root.HasNeighbor(root));

            var neighbor = new IntNode(21);
            root.AddDirectedEdge(neighbor);
            Assert.AreEqual(1, root.NeighborsCount);

            Assert.IsTrue(root.HasNeighbor(neighbor));

            var found = root.Find(21);
            Assert.AreSame(found, neighbor);

            var neighbors = ">";
            root.ForEachNeighbor(node =>
            {
                neighbors += $" {node.Content.ToString()}";
            });
            Assert.AreEqual("> 21", neighbors);

            root.RemoveDirectedEdge(neighbor);
            Assert.AreEqual(0, root.NeighborsCount);
            Assert.IsFalse(root.HasNeighbor(neighbor));
            Assert.IsNull(root.Find(21));
        }

        [Test()]
        public void TestCountDifferentNeighbor()
        {
            var root = new IntNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntNode(i);
                root.AddDirectedEdge(neighbor);
            }
            Assert.AreEqual(10, root.NeighborsCount);
        }

        [Test()]
        public void TestIdentityNeighbor()
        {
            var root = new IntNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntNode(i);
                root.AddDirectedEdge(neighbor);
                Assert.True(root.HasNeighbor(neighbor));
            }
        }

        [Test()]
        public void TestDifferentNeighbor()
        {
            var root = new IntNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntNode(i);
                root.AddDirectedEdge(neighbor);
                var differentNeighbor = new IntNode(i);
                Assert.False(root.HasNeighbor(differentNeighbor));
            }
        }

        [Test()]
        public void TestNeighborByValue()
        {
            var root = new IntNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntNode(i);
                root.AddDirectedEdge(neighbor);
                Assert.True(root.HasNeighbor(root.Find(i)));
            }
        }
    }
}
