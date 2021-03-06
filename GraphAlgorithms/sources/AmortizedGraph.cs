﻿using System;

namespace GraphAlgorithms
{
    /// <summary>
    /// Interpolation graph
    /// </summary>
    public class AmortizedGraph<CONTENT> : WeightedGraph<CONTENT> where CONTENT : IEquatable<CONTENT>
    {
        public AmortizedGraph() : base()
        {
        }

        public AmortizedGraph(UpdateEdgePolicy updateEdgePolicy) : base(updateEdgePolicy)
        {
        }

        public override Node<CONTENT> Node(CONTENT content) => new AmortizedNode<CONTENT>(content);
    }
}
