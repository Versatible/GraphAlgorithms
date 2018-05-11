using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphAlgorithms
{
    /// <summary>
    /// Erp node: Interpolation on egde costs
    /// </summary>
    public class AmortizedNode<CONTENT> : WeightedNode<CONTENT>
    {
        public int QueueSize => 10;

        protected readonly List<Queue<int>> amortizedWeights; // O(n), must be migrated to a O(1) data structure

        public AmortizedNode(CONTENT content) : base(content)
        {
            amortizedWeights = new List<Queue<int>>();
        }

        public override void AddDirectedEdge(WeightedNode<CONTENT> to, int cost)
        {
            base.AddDirectedEdge(to, cost);
            var queue = new Queue<int>();
            queue.Enqueue(cost);
            amortizedWeights.Add(queue);
        }

        public override void UpdateDirectedEdge(WeightedNode<CONTENT> to, int cost)
        {
            var indexOfNeighbor = IndexOfNeighbor(to);
            Debug.Assert(indexOfNeighbor >= 0);
            if(indexOfNeighbor >= 0) {
                var queue = amortizedWeights[indexOfNeighbor];
                queue.Enqueue(cost);
                if(queue.Count > QueueSize) {
                    queue.Dequeue();
                }
                base.UpdateDirectedEdge(to, (int)queue.Average());
            }
        }

        public Queue<int> AmortizedWeights(AmortizedNode<CONTENT> neighbor)
        {
            var indexOfNeighbor = IndexOfNeighbor(neighbor);
            Debug.Assert(indexOfNeighbor >= 0);
            return amortizedWeights[indexOfNeighbor];
        }

        public override void RemoveDirectedEdge(Node<CONTENT> from)
        {
            var indexOfNeighbor = IndexOfNeighbor(from);
            base.RemoveDirectedEdge(from);
            amortizedWeights.RemoveAt(indexOfNeighbor);
        }
    }
}
