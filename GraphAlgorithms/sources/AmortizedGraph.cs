namespace GraphAlgorithms
{
    /// <summary>
    /// Interpolation graph
    /// </summary>
    public class AmortizedGraph<CONTENT> : WeightedGraph<CONTENT>
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
