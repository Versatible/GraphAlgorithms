using System;
using NUnit.Framework;

namespace GraphAlgorithms.test
{
    // A Mock Char AmortizedNode containing a single character
    using CharAmortizedNode = AmortizedNode<Char>;

    [TestFixture()]
    public class AmortizedGraphTest
    {
        private AmortizedGraph<Char> MockAmortizedGraph()
        {
            var graph = new AmortizedGraph<Char>();

            var a = new CharAmortizedNode('A');
            var b = new CharAmortizedNode('B');
            var c = new CharAmortizedNode('C');
            var d = new CharAmortizedNode('D');
            var e = new CharAmortizedNode('E');
            var g = new CharAmortizedNode('G');

            graph.AddNode(a);
            graph.AddNode(b);
            graph.AddNode(c);
            graph.AddNode(d);
            graph.AddNode(e);
            graph.AddNode(g);

            graph.AddUndirectedEdge(a, b, 20);
            graph.AddUndirectedEdge(a, e, 18);
            graph.AddUndirectedEdge(b, c, 10);
            graph.AddUndirectedEdge(b, d, 21);
            graph.AddUndirectedEdge(c, d, 15);
            graph.AddUndirectedEdge(c, e, 30);
            graph.AddUndirectedEdge(d, e, 17);

            return graph;
        }

        [Test()]
        public void TestAPI()
        {
            var graph = MockAmortizedGraph();
            Assert.AreEqual(6, graph.Count);
        }

        #region Edge Update Policies (not exhaustive)
        [Test()]
        public void TestEveryEdge()
        {
            var graph = new AmortizedGraph<Char>((oldEdge, newEdge) => true);

            var a = new CharAmortizedNode('A');
            var b = new CharAmortizedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(15, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(12, a.Cost(b));
        }

        [Test()]
        public void TestNoEdge()
        {
            var graph = new AmortizedGraph<Char>((oldEdge, newEdge) => false);

            var a = new CharAmortizedNode('A');
            var b = new CharAmortizedNode('B');

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
            var graph = new AmortizedGraph<Char>((oldEdge, newEdge) => oldEdge > newEdge);

            var a = new CharAmortizedNode('A');
            var b = new CharAmortizedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(8, a.Cost(b));
        }

        [Test()]
        public void TestExpandingEdges()
        {
            var graph = new AmortizedGraph<Char>((oldEdge, newEdge) => newEdge > oldEdge);

            var a = new CharAmortizedNode('A');
            var b = new CharAmortizedNode('B');

            graph.AddNode(a);
            graph.AddNode(b);

            graph.AddUndirectedEdge(a, b, 10);
            Assert.AreEqual(10, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 20);
            Assert.AreEqual(15, a.Cost(b));

            graph.UpdateUndirectedEdge(a, b, 6);
            Assert.AreEqual(15, a.Cost(b));
        }
        #endregion
    }
}
