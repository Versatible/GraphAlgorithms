using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Int WeightedNode containing a simple integer
    using IntWeightedNode = WeightedNode<int>;

    [TestFixture()]
    public class WeightedNodeTest
    {
        [Test()]
        public void TestAPI()
        {
            var root = new IntWeightedNode(42);
            Assert.AreEqual(42, root.Content);
            Assert.AreEqual(0, root.NeighborsCount);
            Assert.IsNull(root.Find(42));
            Assert.IsFalse(root.HasNeighbor(root));

            var neighbor = new IntWeightedNode(21);
            root.AddDirectedEdge(neighbor, 100);
            Assert.AreEqual(1, root.NeighborsCount);

            Assert.AreEqual(100, root.Cost(neighbor));
            Assert.IsTrue(root.HasNeighbor(neighbor));

            var found = root.Find(21);
            Assert.AreSame(found, neighbor);

            var neighborsAndWeights = ">";
            root.ForEachNeighborAndCost(pair =>
            {
                neighborsAndWeights +=
                    $" {(pair.Key as IntWeightedNode).Content.ToString()}" +
                    $":{pair.Value.ToString()}";
            });
            Assert.AreEqual("> 21:100", neighborsAndWeights);

            var neighbors = ">";
            root.ForEachNeighbor((node) =>
            {
                neighbors += $" {(node as IntWeightedNode).Content.ToString()}";
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
            var root = new IntWeightedNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntWeightedNode(i);
                root.AddDirectedEdge(neighbor, i);
            }
            Assert.AreEqual(10, root.NeighborsCount);
        }

        [Test()]
        public void TestIdentityNeighbor()
        {
            var root = new IntWeightedNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntWeightedNode(i);
                root.AddDirectedEdge(neighbor, i);
                Assert.True(root.HasNeighbor(neighbor));
            }
        }

        [Test()]
        public void TestDifferentNeighbor()
        {
            var root = new IntWeightedNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntWeightedNode(i);
                root.AddDirectedEdge(neighbor, i);
                var differentNeighbor = new IntWeightedNode(i);
                Assert.False(root.HasNeighbor(differentNeighbor));
            }
        }

        [Test()]
        public void TestNeighborByValue()
        {
            var root = new IntWeightedNode(0);
            for (int i = 1; i <= 10; i++)
            {
                var neighbor = new IntWeightedNode(i);
                root.AddDirectedEdge(neighbor, i);
                Assert.True(root.HasNeighbor(root.Find(i)));
            }
        }

        [Test()]
        public void TestOneEdgeWeight()
        {
            var root = new IntWeightedNode(0);
            var neighbor = new IntWeightedNode(2);
            root.AddDirectedEdge(neighbor, 10);
            Assert.AreEqual(10, root.Cost(neighbor));
        }


        [Test()]
        public void TestEdgesWeight()
        {
            var count = 10;
            var root = new IntWeightedNode(0);

            for (int cost = 100; cost < 100 + count; cost++)
            {
                var neighbor = new IntWeightedNode(cost); // Store cost in node
                root.AddDirectedEdge(neighbor, cost);
            }

            // The cost of each edge to neighbor was stored a the value of the IntWeightedNode, so they must match
            root.ForEachNeighborAndCost(pair =>
            {
                var intNode = pair.Key as IntWeightedNode;
                var cost = pair.Value;
                Assert.AreEqual(intNode.Content, cost);
            });
        }
    }
}
