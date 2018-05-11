using NUnit.Framework;
using System;
namespace GraphAlgorithms.test
{
    // A Mock Char WeightedNode containing a single character
    using CharNode = Node<Char>;

    [TestFixture()]
    public class GraphTest
    {
        private Graph<Char> MockGraph()
        {
            var graph = new Graph<Char>();

            var a = new CharNode('A');
            var b = new CharNode('B');
            var c = new CharNode('C');
            var d = new CharNode('D');
            var e = new CharNode('E');
            var g = new CharNode('G');

            graph.AddNode(a);
            graph.AddNode(b);
            graph.AddNode(c);
            graph.AddNode(d);
            graph.AddNode(e);
            graph.AddNode(g);

            graph.AddUndirectedEdge(a, b);
            graph.AddUndirectedEdge(a, e);
            graph.AddUndirectedEdge(b, c);
            graph.AddUndirectedEdge(b, d);
            graph.AddUndirectedEdge(c, d);
            graph.AddUndirectedEdge(c, e);
            graph.AddUndirectedEdge(d, e);

            return graph;
        }

        [Test()]
        public void TestNodesAreConnected()
        {
            var graph = MockGraph();

            var c = graph.Find('C');
            Assert.NotNull(c);
            Assert.True(c.HasNeighbor(graph.Find('B')));
            Assert.True(c.HasNeighbor(graph.Find('D')));
            Assert.True(c.HasNeighbor(graph.Find('E')));
            Assert.False(c.HasNeighbor(graph.Find('A')));
            Assert.False(c.HasNeighbor(graph.Find('G')));
        }

        [Test()]
        public void TestNodesAreInGraph()
        {
            var graph = MockGraph();

            // Equivalent
            Assert.IsTrue(graph.Contains(graph.Find('A')));
            Assert.IsTrue(graph.Contains(graph.Find('B')));
            Assert.IsTrue(graph.Contains(graph.Find('C')));
            Assert.IsTrue(graph.Contains(graph.Find('D')));
            Assert.IsTrue(graph.Contains(graph.Find('E')));
            Assert.IsTrue(graph.Contains(graph.Find('G')));

            Assert.IsFalse(graph.Contains(graph.Find('H')));

            // Similar
            Assert.IsFalse(graph.Contains(new CharNode('C')));
        }

        [Test()]
        public void TestUndirectedUnion()
        {
            var addition = new Graph<Char>();

            var a = new CharNode('A');
            var c = new CharNode('C');
            var f = new CharNode('F');
            var h = new CharNode('H');

            addition.AddNode(a);
            addition.AddNode(c);
            addition.AddNode(f);
            addition.AddNode(h);

            addition.AddUndirectedEdge(a, c);
            addition.AddUndirectedEdge(a, h);
            addition.AddUndirectedEdge(c, h);

            var graph = MockGraph();
            graph.UndirectedUnion(addition);

            var bfs = "";
            graph.BFS(node => bfs += node.Content);
            Assert.AreEqual("ABECHDGF", bfs);

            var edges = graph.UndirectedEdges();
            var traversal = $"{edges.Length} edges";
            foreach (var edge in edges)
            {
                traversal += $", {edge.Item1.Content.ToString()}-{edge.Item2.Content.ToString()}";
            }
            var expected = "10 edges, A-B, A-E, A-C, A-H, B-C, B-D, C-D, C-E, C-H, D-E";
            Assert.AreEqual(expected, traversal);
        }

        [Test()]
        public void TestBFS()
        {
            var graph = MockGraph();
            var traversal = "";
            graph.BFS(node => traversal += node.Content);

            Assert.AreEqual("ABECDG", traversal);
        }

        [Test()]
        public void TestDFS()
        {
            var graph = MockGraph();

            var traversalA = "";
            var a = graph.Find('A');
            graph.DFS(root: a, action: node => traversalA += node.Content);
            Assert.AreEqual("ABCDE", traversalA);

            var traversalG = "";
            var g = graph.Find('G');
            graph.DFS(root: g, action: node => traversalG += node.Content);
            Assert.AreEqual("G", traversalG);
        }

        [Test()]
        public void TestRemovingNode()
        {
            var graph = MockGraph();
            var a = graph.Find('A');
            var b = graph.Find('B');
            var e = graph.Find('E');

            Assert.AreEqual(2, a.NeighborsCount);
            Assert.AreEqual(3, b.NeighborsCount);
            Assert.AreEqual(3, e.NeighborsCount);

            graph.RemoveNode(a);

            Assert.IsNull(graph.Find('a'));

            Assert.AreEqual(2, b.NeighborsCount);
            Assert.AreEqual(2, e.NeighborsCount);

            var traversal = "";
            graph.BFS(node => traversal += node.Content);
            Assert.AreEqual("BCDEG", traversal);
        }

        [Test()]
        public void TestChangingEdges()
        {
            var graph = MockGraph();
            var g = graph.Find('G') as CharNode;
            graph.AddUndirectedEdge(graph.Find('A') as CharNode, g);

            var d = graph.Find('D') as CharNode;
            graph.RemoveUndirectedEdge(d, graph.Find('B') as CharNode);
            graph.RemoveUndirectedEdge(d, graph.Find('C') as CharNode);
            graph.RemoveUndirectedEdge(d, graph.Find('E') as CharNode);

            Assert.AreEqual(0, d.NeighborsCount);

            var traversal = "";
            graph.BFS(node => traversal += node.Content);
            Assert.AreEqual("ABEGCD", traversal);
        }

        [Test()]
        public void TestUndirectedEdges()
        {
            var graph = MockGraph();
            var edges = graph.UndirectedEdges();
            var traversal = $"{edges.Length} edges";
            foreach (var edge in edges)
            {
                traversal += $", {edge.Item1.Content.ToString()}-{edge.Item2.Content.ToString()}";
            }
            var expected = "7 edges, A-B, A-E, B-C, B-D, C-D, C-E, D-E";
            Assert.AreEqual(expected, traversal);
        }
    }
}
