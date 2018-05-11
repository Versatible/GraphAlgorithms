using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GraphAlgorithms
{
    public class WeightedNode<CONTENT> : Node<CONTENT>
    {
        protected readonly List<int> Weights; // O(n), must be migrated to a O(1) data structure

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CS201GraphAlgorithms.WeightedNode`1"/> class.
        /// </summary>
        /// <param name="content">The embedded payload of for this node.</param>
        public WeightedNode(CONTENT content) : base(content)
        {
            Weights = new List<int>();
        }

        /// <summary>
        /// Cost of that edge, to the specified neighbor.
        /// Neighbor MUST exist
        /// </summary>
        /// <returns>Edge weight.</returns>
        /// <param name="neighbor">Neighbor.</param>
        public int Cost(Node<CONTENT> neighbor)
        {
            if(!HasNeighbor(neighbor)) {
                throw new KeyNotFoundException("Edge not found");
            }
            return Weights[IndexOfNeighbor(neighbor)];
        }

        /// <summary>
        /// Adds the directed edge.
        /// MUST not already exist
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="cost">Cost.</param>
        public virtual void AddDirectedEdge(WeightedNode<CONTENT> to, int cost)
        {
            base.AddDirectedEdge(to);
            Weights.Add(cost);
        }

        public virtual void UpdateDirectedEdge(WeightedNode<CONTENT> to, int cost)
        {
            var indexOfNeighbor = IndexOfNeighbor(to);
            Debug.Assert(indexOfNeighbor >= 0);
            if (indexOfNeighbor >= 0)
            {
                Weights[indexOfNeighbor] = cost;
            }
        }

        public override void RemoveDirectedEdge(Node<CONTENT> from)
        {
            var indexOfNeighbor = IndexOfNeighbor(from);
            base.RemoveDirectedEdge(from);
            Weights.RemoveAt(indexOfNeighbor);
        }

        /// <summary>
        /// Enumerates through the nodes and their weight.
        /// </summary>
        /// <param name="action">Action.</param>
        public void ForEachNeighborAndCost(Action<KeyValuePair<Node<CONTENT>, int>> action)
        {
            for (int i = 0; i < NeighborsCount; i++) {
                var nodeAndWeight = new KeyValuePair<Node<CONTENT>, int>(key:  Neighbors[i],
                                                                         value: Weights[i]);
                action(nodeAndWeight);
            }
        }
    }
}
