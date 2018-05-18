using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphAlgorithms
{
    public class WeightedNode<CONTENT> : Node<CONTENT> where CONTENT : IEquatable<CONTENT>
    {
        protected readonly List<double> Costs; // O(n), must be migrated to a O(1) data structure

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CS201GraphAlgorithms.WeightedNode`1"/> class.
        /// </summary>
        /// <param name="content">The embedded payload of for this node.</param>
        public WeightedNode(CONTENT content) : base(content)
        {
            Costs = new List<double>();
        }

        /// <summary>
        /// Cost of that edge, to the specified neighbor.
        /// Neighbor MUST exist
        /// </summary>
        /// <returns>Edge weight.</returns>
        /// <param name="neighbor">Neighbor.</param>
        public override double Cost(Node<CONTENT> neighbor)
        {
            if (!HasNeighbor(neighbor))
            {
                throw new KeyNotFoundException("Edge not found");
            }
            return Costs[IndexOfNeighbor(neighbor)];
        }

        /// <summary>
        /// Adds the directed edge.
        /// MUST not already exist
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cost">Cost.</param>
        public virtual void AddDirectedEdge(WeightedNode<CONTENT> to, double cost)
        {
            base.AddDirectedEdge(to);
            Costs.Add(cost);
        }

        public virtual void UpdateDirectedEdge(WeightedNode<CONTENT> to, double cost)
        {
            var indexOfNeighbor = IndexOfNeighbor(to);
            Debug.Assert(indexOfNeighbor >= 0);
            if (indexOfNeighbor >= 0)
            {
                Costs[indexOfNeighbor] = cost;
            }
        }

        public override void RemoveDirectedEdge(Node<CONTENT> node)
        {
            var indexOfNeighbor = IndexOfNeighbor(node);
            base.RemoveDirectedEdge(node);
            Costs.RemoveAt(indexOfNeighbor);
        }

        /// <summary>
        /// Enumerates through the nodes and their weight.
        /// </summary>
        /// <param name="action">Action.</param>
        public void ForEachNeighborAndCost(Action<KeyValuePair<Node<CONTENT>, double>> action)
        {
            for (int i = 0; i < NeighborsCount; i++)
            {
                var nodeAndWeight = new KeyValuePair<Node<CONTENT>, double>(key: Neighbors[i],
                                                                            value: Costs[i]);
                action(nodeAndWeight);
            }
        }
    }
}
