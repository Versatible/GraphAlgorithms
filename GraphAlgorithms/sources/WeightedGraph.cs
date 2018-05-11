using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphAlgorithms
{
    /// <summary>
    /// Weighted graph.
    /// </summary>
    public class WeightedGraph<CONTENT> : Graph<CONTENT>
    {
        /// <summary>
        /// Boolean Edge comparator: returns true if the edge is to be updated, false otherwise
        /// </summary>
        public delegate bool UpdateEdgePolicy(int oldEdge, int newEdge);
        private readonly UpdateEdgePolicy updateEdgePolicy;

        public WeightedGraph() : base()
        {
            this.updateEdgePolicy = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CS201GraphAlgorithms.WeightedGraph`1"/> class.
        /// </summary>
        /// <param name="updateEdgePolicy">Edge policy.</param>
        public WeightedGraph(UpdateEdgePolicy updateEdgePolicy) : base()
        {
            Debug.Assert(null != updateEdgePolicy);
            this.updateEdgePolicy = updateEdgePolicy;
        }
 
        public override Node<CONTENT> Node(CONTENT content) => new WeightedNode<CONTENT>(content);

        protected override void AddDirectedEdge(Node<CONTENT> from, Node<CONTENT> to)
        {
            throw new NotSupportedException("WeightedGraph require cost");
        }

        public override void AddUndirectedEdge(Node<CONTENT> between, Node<CONTENT> and)
        {
            throw new NotSupportedException("WeightedGraph require cost");
        }

        protected virtual void AddDirectedEdge(WeightedNode<CONTENT> from, WeightedNode<CONTENT> to, int cost)
        {
            from.AddDirectedEdge(to, cost);
        }

        protected void UpdateDirectedEdge(WeightedNode<CONTENT> from, WeightedNode<CONTENT> to, int cost)
        {
            if (updateEdgePolicy != null)
            {
                var hasNeighbor = from.HasNeighbor(to);
                Debug.Assert(hasNeighbor);
                if (hasNeighbor)
                {
                    if (updateEdgePolicy(oldEdge: from.Cost(to), newEdge: cost))
                    {
                        from.UpdateDirectedEdge(to, cost);
                    }
                }
            }
            else
            {
                from.UpdateDirectedEdge(to, cost);
            }
        }

        public void AddUndirectedEdge(WeightedNode<CONTENT> between, WeightedNode<CONTENT> and, int cost)
        {
            AddDirectedEdge(between, and, cost);
            AddDirectedEdge(and, between, cost);
        }

        public void UpdateUndirectedEdge(WeightedNode<CONTENT> between, WeightedNode<CONTENT> and, int cost)
        {
            UpdateDirectedEdge(between, and, cost);
            UpdateDirectedEdge(and, between, cost);
        }

        public override void UndirectedUnion(Graph<CONTENT> with) {
            throw new NotSupportedException("WeightedGraph require cost");
        }

        /// <summary>
        /// Takes a series of nodes and their weight, and interconnect them in a graph where each node is
        /// linked to every opposite nodes
        /// </summary>
        /// <param name="enumerable">A Tuple with Item1:Nodes and Item2:weight </param>
        public void ExplodeEdges(IEnumerable<Tuple<WeightedNode<CONTENT>, double>> enumerable)
        {
            var dictionary = enumerable.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

            // Interconnect nodes
            while (dictionary.Count > 1)
            {
                var origin = dictionary.First();
                var nodeOrigin = origin.Key;
                var distanceToOrigin = origin.Value;
                dictionary.Remove(nodeOrigin);

                foreach (var destination in dictionary)
                {
                    var distance = (int)(distanceToOrigin + destination.Value);
                    AddUndirectedEdge(between: nodeOrigin, and: destination.Key, cost: distance);
                }
            }
        }

        /// <summary>
        /// Join 2 graphs with undirected edges
        /// </summary>
        /// <returns>The union.</returns>
        /// <param name="with">With.</param>
        public void UndirectedUnion(WeightedGraph<CONTENT> with)
        {
            // Add nodes to the graph
            var newNodes = new List<Node<CONTENT>>();
            with.BFS(node =>
            {
                if (null == Find(node.Content))
                {
                    AddNode(node);
                    newNodes.Add(node);
                }
            });

            var edges = with.UndirectedEdges();

            // Cleanup residual edges in the nodes we freshly inserted
            foreach (var newNode in newNodes)
            {
                var neighbors = new List<Node<CONTENT>>();
                newNode.ForEachNeighbor(neighbor => neighbors.Add(neighbor));
                foreach (var neighbor in neighbors)
                {
                    newNode.RemoveDirectedEdge(neighbor);
                }
            }

            foreach (var edge in edges)
            {
                var node = Find(edge.Item1.Content) as WeightedNode<CONTENT>;
                var neighbor = Find(edge.Item2.Content) as WeightedNode<CONTENT>;
                var cost = edge.Item3;
                if (node.HasNeighbor(neighbor)) {
                    UpdateUndirectedEdge(between: node, and: neighbor, cost:cost);
                } else {
                    AddUndirectedEdge(between: node, and: neighbor, cost:cost);
                }
            }
        }

        #region Traversal
        // Edges are connection between nodes, with a weight
        public new Tuple<WeightedNode<CONTENT>, WeightedNode<CONTENT>, int>[] UndirectedEdges()
        {
            var edgesSet = new HashSet<Tuple<WeightedNode<CONTENT>, WeightedNode<CONTENT>, int>>();

            foreach (var node in NodeSet)
            {
                (node as WeightedNode<CONTENT>).ForEachNeighborAndCost(pair =>
                {
                    var oneWay = new Tuple<WeightedNode<CONTENT>, WeightedNode<CONTENT>, int>
                        (node as WeightedNode<CONTENT>, pair.Key as WeightedNode<CONTENT>, pair.Value);
                    var theOther = new Tuple<WeightedNode<CONTENT>, WeightedNode<CONTENT>, int>
                        (pair.Key as WeightedNode<CONTENT>, node as WeightedNode<CONTENT>, pair.Value);
                    if (!edgesSet.Contains(oneWay) && !edgesSet.Contains(theOther))
                    {
                        edgesSet.Add(oneWay);
                    }
                });
            }
            return edgesSet.ToArray();
        }
        #endregion
    }
}
