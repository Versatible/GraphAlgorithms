using System;
using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Char WeightedNode containing a single character
    using CharWeightedNode = WeightedNode<Char>;

    [TestFixture()]
    public class WeightedGraphTest
    {
        private WeightedGraph<Char> MockWeightedGraph()
        {
            var graph = new WeightedGraph<Char>();

            var a = new CharWeightedNode('A');
            var b = new CharWeightedNode('B');
            var c = new CharWeightedNode('C');
            var d = new CharWeightedNode('D');
            var e = new CharWeightedNode('E');
            var g = new CharWeightedNode('G');

            graph.AddNode(a);
            graph.AddNode(b);
            graph.AddNode(c);
            graph.AddNode(d);
            graph.AddNode(e);
            graph.AddNode(g);

            graph.AddUndirectedEdge(a, b, 20);
            graph.AddUndirectedEdge(a, e, 8);
            graph.AddUndirectedEdge(b, c, 18);
            graph.AddUndirectedEdge(b, d, 21);
            graph.AddUndirectedEdge(c, d, 15);
            graph.AddUndirectedEdge(c, e, 30);
            graph.AddUndirectedEdge(d, e, 12);

            return graph;
        }

        [Test()]
        public void TestChangingEdges()
        {
            var graph = MockWeightedGraph();
            var g = graph.Find('G') as CharWeightedNode;
            graph.AddUndirectedEdge(graph.Find('A') as CharWeightedNode, g, 12);

            var d = graph.Find('D') as CharWeightedNode;
            graph.RemoveUndirectedEdge(d, graph.Find('B') as CharWeightedNode);
            graph.RemoveUndirectedEdge(d, graph.Find('C') as CharWeightedNode);
            graph.RemoveUndirectedEdge(d, graph.Find('E') as CharWeightedNode);

            Assert.AreEqual(0, d.NeighborsCount);

            var actual = "";
            graph.BFS(node => actual += node.Content);
            Assert.AreEqual("ABEGCD", actual);
        }

        [Test()]
        public void TestMultipleEdgesChanges()
        {
            var graph = MockWeightedGraph();
            var a = graph.Find('A') as CharWeightedNode;
            var b = graph.Find('B') as CharWeightedNode;
            var c = graph.Find('C') as CharWeightedNode;
            var d = graph.Find('D') as CharWeightedNode;
            var e = graph.Find('E') as CharWeightedNode;
            var g = graph.Find('G') as CharWeightedNode;

            for (int cost = 10; cost < 20; cost++)
            {
                graph.UpdateUndirectedEdge(between: a, and: b, cost: cost);
                graph.UpdateUndirectedEdge(between: a, and: e, cost: cost);

                graph.UpdateUndirectedEdge(between: b, and: a, cost: cost);
                graph.UpdateUndirectedEdge(between: b, and: c, cost: cost);
                graph.UpdateUndirectedEdge(between: b, and: d, cost: cost);

                graph.UpdateUndirectedEdge(between: c, and: b, cost: cost);
                graph.UpdateUndirectedEdge(between: c, and: d, cost: cost);
                graph.UpdateUndirectedEdge(between: c, and: e, cost: cost);

                graph.UpdateUndirectedEdge(between: d, and: b, cost: cost);
                graph.UpdateUndirectedEdge(between: d, and: c, cost: cost);
                graph.UpdateUndirectedEdge(between: d, and: e, cost: cost);

                graph.UpdateUndirectedEdge(between: e, and: c, cost: cost);
                graph.UpdateUndirectedEdge(between: e, and: d, cost: cost);
                graph.UpdateUndirectedEdge(between: e, and: a, cost: cost);
            }

            var actual = "";
            graph.BFS(node => actual += node.Content);
            Assert.AreEqual("ABECDG", actual);
        }


        [Test()]
        public void TestUndirectedUnion()
        {
            var addition = new WeightedGraph<Char>();

            var a = new CharWeightedNode('A');
            var c = new CharWeightedNode('C');
            var f = new CharWeightedNode('F');
            var h = new CharWeightedNode('H');

            addition.AddNode(a);
            addition.AddNode(c);
            addition.AddNode(f);
            addition.AddNode(h);

            addition.AddUndirectedEdge(a, c, 11);
            addition.AddUndirectedEdge(a, h, 7);
            addition.AddUndirectedEdge(c, h, 14);

            var graph = MockWeightedGraph();
            graph.UndirectedUnion(addition);

            var bfs = "";
            graph.BFS(node => bfs += node.Content);
            Assert.AreEqual("ABECHDGF", bfs);

            var edges = graph.UndirectedEdges();
            var actual = $"{edges.Length} edges";
            foreach (var edge in edges)
            {
                actual += $", {edge.Item1.Content.ToString()}-{edge.Item3.ToString()}-{edge.Item2.Content.ToString()}";
            }
            var expected = "10 edges, A-20-B, A-8-E, A-11-C, A-7-H, B-18-C, B-21-D, C-15-D, C-30-E, C-14-H, D-12-E";
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void TestUndirectedEdges()
        {
            var graph = MockWeightedGraph();
            var edges = graph.UndirectedEdges();
            var actual = $"{edges.Length} edges";
            foreach (var edge in edges)
            {
                actual += $", {edge.Item1.Content.ToString()}-{edge.Item3.ToString()}-{edge.Item2.Content.ToString()}";
            }
            var expected = "7 edges, A-20-B, A-8-E, B-18-C, B-21-D, C-15-D, C-30-E, D-12-E";
            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public void TestShortestPath()
        {
            var graph = MockWeightedGraph();
            var a = graph.Find('A');
            var c = graph.Find('C');

            var actual = "";
            var shortestPath = graph.ShortestPath(a, c);
            foreach (var node in shortestPath)
            {
                actual += node.Content;
            }

            var expected = "CDE";
            Assert.AreEqual(expected, actual);
        }

        #region Edge Update Policies (not exhaustive)
        [Test()]
        public void TestEveryEdge()
        {
            var graph = new WeightedGraph<Char>((oldEdge, newEdge) => true);

            var a = new CharWeightedNode('A');
            var b = new CharWeightedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(20, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(6, a.Cost(b));
        }

        [Test()]
        public void TestNoEdge()
        {
            var graph = new WeightedGraph<Char>((oldEdge, newEdge) => false);

            var a = new CharWeightedNode('A');
            var b = new CharWeightedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(10, a.Cost(b));
        }

        [Test()]
        public void TestContractingEdges()
        {
            var graph = new WeightedGraph<Char>((oldEdge, newEdge) => oldEdge > newEdge);

            var a = new CharWeightedNode('A');
            var b = new CharWeightedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(6, a.Cost(b));
        }

        [Test()]
        public void TestExpandingEdges()
        {
            var graph = new WeightedGraph<Char>((oldEdge, newEdge) => newEdge > oldEdge);

            var a = new CharWeightedNode('A');
            var b = new CharWeightedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(20, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(20, a.Cost(b));
        }
        #endregion
    }
}
