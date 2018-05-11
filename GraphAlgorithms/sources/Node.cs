using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphAlgorithms
{
    public class Node<CONTENT>
    {
        public readonly CONTENT Content;
        protected readonly List<Node<CONTENT>> Neighbors;
        public int NeighborsCount => Neighbors.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CS201GraphAlgorithms.Node`1"/> class.
        /// </summary>
        /// <param name="content">The embedded payload of for this node.</param>
        public Node(CONTENT content)
        {
            Content = content;
            Neighbors = new List<Node<CONTENT>>();
        }

        /// <summary>
        /// Adds this edge if it doesn't already exist.
        /// MUST not already exist
        /// </summary>
        /// <param name="to">Node.</param>
        public virtual void AddDirectedEdge(Node<CONTENT> to) {
            if(HasNeighbor(to)) {
                throw new Exception("Duplicate edge");
            }
            Neighbors.Add(to);
        }

        /// <summary>
        /// Removes the edge.
        /// Edge MUST already exist
        /// </summary>
        /// <param name="node">Node.</param>
        public virtual void RemoveDirectedEdge(Node<CONTENT> node) {
            if (!HasNeighbor(node))
            {
                throw new KeyNotFoundException("Edge not found");
            }
            Neighbors.Remove(node);
        }

        /// <summary>
        /// Find a neighbor by its content.
        ///
        /// It is not a good idea to combine HasNeighbor+Find in a single utility since it hides its true cost
        /// public bool HasNeighbor(CONTENT neighbor) => Find(neighbor) != null;
        /// </summary>
        /// <returns>The found node</returns>
        /// <param name="content">Content.</param>
        public Node<CONTENT> Find(CONTENT content) => Neighbors.FirstOrDefault(node =>
                                                                               EqualityComparer<CONTENT>.Default.Equals(
                                                                                                        node.Content,
                                                                                                        content));

        /// <summary>
        /// Does the present node have this other node as a neighbor.
        /// </summary>
        /// <returns><c>true</c>, if "node" is a neighbor, <c>false</c> otherwise.</returns>
        /// <param name="node">This is the neighbor you are looking for.</param>
        public bool HasNeighbor(Node<CONTENT> node) => Neighbors.Contains(node);

        /// <summary>
        /// Index the of a given neighbor.
        /// </summary>
        /// <returns>The index of neighbor, which is assumed to exist</returns>
        /// <param name="node">Node.</param>
        protected int IndexOfNeighbor(Node<CONTENT> node) => Neighbors.IndexOf(node);

        /// <summary>
        /// Iterator
        /// </summary>
        /// <param name="action">Action.</param>
        public void ForEachNeighbor(Action<Node<CONTENT>> action)
        {
            foreach(var node in Neighbors) {
                action(node);
            }
        }
    }
}
