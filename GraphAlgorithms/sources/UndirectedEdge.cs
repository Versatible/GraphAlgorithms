using System;

namespace GraphAlgorithms
{
    public struct UndirectedEdge<CONTENT> : IComparable, IEquatable<UndirectedEdge<CONTENT>> where CONTENT : IEquatable<CONTENT>
    {
        public readonly Node<CONTENT> Between;
        public readonly Node<CONTENT> And;
        public readonly double Weight;

        public UndirectedEdge(Node<CONTENT> between, Node<CONTENT> and, double weight = 1.0d)
        {
            Between = between;
            And = and;
            Weight = weight;
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            var otherWeight = ((UndirectedEdge<CONTENT>)obj).Weight;
            return ((Weight == otherWeight)
                   ? 0
                    : ((Weight < otherWeight)
                      ? -1
                       : 1));
        }
        #endregion IComparable

        #region IEquatable
        public bool Equals(UndirectedEdge<CONTENT> other)
        {
            return (Between.Content.Equals(other.Between.Content) && And.Content.Equals(other.And.Content)) ||
                (Between.Content.Equals(other.And.Content) && And.Content.Equals(other.Between.Content));
        }
        #endregion IEquatable
    }
}
