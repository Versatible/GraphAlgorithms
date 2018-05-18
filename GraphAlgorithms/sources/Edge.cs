using System;

namespace GraphAlgorithms
{
    public struct Edge<CONTENT> : IComparable, IEquatable<Edge<CONTENT>> where CONTENT : IEquatable<CONTENT>
    {
        public readonly Node<CONTENT> From;
        public readonly Node<CONTENT> To;
        public readonly double Weight;

        public Edge(Node<CONTENT> from, Node<CONTENT> to, double weight = 1.0d)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            var otherWeight = ((Edge<CONTENT>)obj).Weight;
            return ((Weight == otherWeight)
                   ? 0
                    : ((Weight < otherWeight)
                      ? -1
                       : 1));
        }
        #endregion IComparable

        #region IEquatable
        public bool Equals(Edge<CONTENT> other)
        {
            return From.Content.Equals(other.From.Content) && To.Content.Equals(other.To.Content);
        }
        #endregion IEquatable
    }

}
