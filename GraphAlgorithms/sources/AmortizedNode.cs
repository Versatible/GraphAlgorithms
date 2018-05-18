using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphAlgorithms
{
    /// <summary>
    /// Erp node: Interpolation on egde costs
    /// </summary>
    public class AmortizedNode<CONTENT> : WeightedNode<CONTENT> where CONTENT : IEquatable<CONTENT>
    {
        public int QueueSize => 10;

        protected readonly List<Queue<double>> amortizedCosts; // O(n), must be migrated to a O(1) data structure

        public AmortizedNode(CONTENT content) : base(content)
        {
            amortizedCosts = new List<Queue<double>>();
        }

        public override void AddDirectedEdge(WeightedNode<CONTENT> to, double cost)
        {
            base.AddDirectedEdge(to, cost);
            var queue = new Queue<double>();
            queue.Enqueue(cost);
            amortizedCosts.Add(queue);
        }

        public override void UpdateDirectedEdge(WeightedNode<CONTENT> to, double cost)
        {
            var indexOfNeighbor = IndexOfNeighbor(to);
            Debug.Assert(indexOfNeighbor >= 0);
            if(indexOfNeighbor >= 0) {
                var queue = amortizedCosts[indexOfNeighbor];
                queue.Enqueue(cost);
                if(queue.Count > QueueSize) {
                    queue.Dequeue();
                }
                base.UpdateDirectedEdge(to, (int)queue.Average());
            }
        }

        public Queue<double> AmortizedCosts(AmortizedNode<CONTENT> neighbor)
        {
            var indexOfNeighbor = IndexOfNeighbor(neighbor);
            Debug.Assert(indexOfNeighbor >= 0);
            return amortizedCosts[indexOfNeighbor];
        }

        public override void RemoveDirectedEdge(Node<CONTENT> node)
        {
            var indexOfNeighbor = IndexOfNeighbor(node);
            base.RemoveDirectedEdge(node);
            amortizedCosts.RemoveAt(indexOfNeighbor);
        }
    }
}
