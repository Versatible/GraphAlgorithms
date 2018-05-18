using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphAlgorithms
{
    public class Graph<CONTENT> where CONTENT : IEquatable<CONTENT>
    {
        public readonly HashSet<Node<CONTENT>> NodeSet;
        public int Count => NodeSet.Count;

        public Graph()
        {
            NodeSet = new HashSet<Node<CONTENT>>();
        }

        /// <summary>
        /// Creates a new Node for this type of Graph
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="content">Content.</param>
        public virtual Node<CONTENT> Node(CONTENT content) => new Node<CONTENT>(content);

        public void AddNode(Node<CONTENT> node) => NodeSet.Add(node);

        public bool Contains(Node<CONTENT> node) => NodeSet.Contains(node);

        protected virtual void AddDirectedEdge(Node<CONTENT> from, Node<CONTENT> to)
        {
            from.AddDirectedEdge(to);
        }

        public virtual void AddUndirectedEdge(Node<CONTENT> between, Node<CONTENT> and)
        {
            AddDirectedEdge(between, and);
            AddDirectedEdge(and, between);
        }

        public void RemoveUndirectedEdge(Node<CONTENT> between, Node<CONTENT> and)
        {
            between.RemoveDirectedEdge(and);
            and.RemoveDirectedEdge(between);
        }

        public bool RemoveNode(Node<CONTENT> node)
        {
            Debug.Assert(null != node);
            Debug.Assert(NodeSet.Contains(node));

            if (null == node || !NodeSet.Contains(node))
            {
                return false;
            }

            // otherwise, the node was found
            NodeSet.Remove(node);

            // This is optimized as we assume that edges are always bidirectional
            node.ForEachNeighbor(neighbor => neighbor.RemoveDirectedEdge(node));

            // For directed edges, must enumerate through each node in the nodeSet,
            // removing edges to this node
            //foreach (WeightedNode<CONTENT> otherNode in nodeSet)
            //{
            //    if (otherNode.Neighbors.ContainsKey(node))
            //    {
            //        otherNode.Neighbors.Remove(node);
            //    }
            //}

            return true;
        }

        /// <summary>
        /// Find the specified node in the graph, by value, not node.
        /// It is not a good idea to combine Contains+Find in a single utility since it hides its true cost
        /// public bool Contains(CONTENT value) => Find(value) != null;
        /// </summary>
        /// <returns>The found node by content, or null</returns>
        /// <param name="content">Value.</param>
        public Node<CONTENT> Find(CONTENT content)
        {
            return NodeSet.FirstOrDefault(node => node.Content.Equals(content));
        }

        /// <summary>
        /// Batches the creation of nodes
        /// </summary>
        /// <param name="contents">An enumerable of content</param>
        public void BatchNodes(IEnumerable<CONTENT> contents)
        {
            foreach (var content in contents)
            {
                AddNode(Node(content));
            }
        }

        /// <summary>
        /// Join 2 graphs with undirected edges
        /// </summary>
        /// <returns>The union.</returns>
        /// <param name="with">With.</param>
        public virtual void UndirectedUnion(Graph<CONTENT> with)
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
                var node = Find(edge.Item1.Content);
                var neighbor = Find(edge.Item2.Content);
                if (!node.HasNeighbor(neighbor))
                {
                    AddUndirectedEdge(between: node, and: neighbor);
                }
            }
        }

        #region Traversal
        /// <summary>
        /// Breadth First Search
        /// </summary>
        /// <param name="action">an Action accepting 1 parameter CONTENT</param>
        public void BFS(Action<Node<CONTENT>> action)
        {
            var visited = new HashSet<Node<CONTENT>>();
            var notVisited = new Node<CONTENT>[Count];
            NodeSet.CopyTo(notVisited);

            // This is a disjoint set, so visit each vertice
            foreach (var start in NodeSet)
            {
                if (!visited.Contains(start))
                {
                    var queue = new Queue<Node<CONTENT>>();
                    visited.Add(start);
                    action(start);
                    queue.Enqueue(start);

                    while (queue.Count != 0)
                    {
                        Node<CONTENT> node = queue.Dequeue();
                        node.ForEachNeighbor(neighbor =>
                        {
                            if (!visited.Contains(neighbor))
                            {
                                visited.Add(neighbor);
                                action(neighbor);
                                queue.Enqueue(neighbor);
                            }
                        });
                    }
                }
            }
        }

        private enum VertexState
        {
            White, Gray, Black
        }

        public void DFS(Node<CONTENT> root, Action<Node<CONTENT>> action)
        {
            var state = new Dictionary<Node<CONTENT>, VertexState>();
            RunDFS(root, state, action);
        }

        private void RunDFS(Node<CONTENT> node, Dictionary<Node<CONTENT>, VertexState> state, Action<Node<CONTENT>> action)
        {
            state[node] = VertexState.Gray;
            action(node);
            node.ForEachNeighbor((neighbor) =>
            {
                if (!state.ContainsKey(neighbor)) //  VertexState.White
                {
                    RunDFS(neighbor, state, action);
                }
            });
            state[node] = VertexState.Black;
        }

        // Edges are connection between nodes
        public virtual Tuple<Node<CONTENT>, Node<CONTENT>>[] UndirectedEdges()
        {
            var edgesSet = new HashSet<Tuple<Node<CONTENT>, Node<CONTENT>>>();

            foreach (var node in NodeSet)
            {
                node.ForEachNeighbor(neighbor =>
                {
                    var oneWay = new Tuple<Node<CONTENT>, Node<CONTENT>>(node, neighbor);
                    var theOther = new Tuple<Node<CONTENT>, Node<CONTENT>>(neighbor, node);
                    if (!edgesSet.Contains(oneWay) && !edgesSet.Contains(theOther))
                    {
                        edgesSet.Add(oneWay);
                    }
                });
            }
            return edgesSet.ToArray();
        }
        #endregion Traversal

        #region paths
        public List<Node<CONTENT>> ShortestPath(Node<CONTENT> from, Node<CONTENT> destination)
        {
            return Path(destination, Dijkstra(from, destination));
        }

        /// <summary>
        /// Calculate every shortest path to every node, optionally stopping when reaching destination
        /// </summary>
        /// <returns>The dijkstra.</returns>
        /// <param name="from">From.</param>
        /// <param name="destination">destination.</param>
        public Dictionary<Node<CONTENT>, Node<CONTENT>> Dijkstra(Node<CONTENT> from, Node<CONTENT> destination)
        {
            var infinity = double.MaxValue;
            var previous = new Dictionary<Node<CONTENT>, Node<CONTENT>>();
            var distances = new Dictionary<Node<CONTENT>, double>();
            var nodes = NodeSet.ToList();

            // All distances but «from» to ∞
            foreach (var node in NodeSet)
                distances[node] = infinity;
            distances[from] = 0;

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => ((distances[x] == distances[y])
                           ? 0
                            : ((distances[x] < distances[y])
                              ? -1
                               : 1)));

                var smallest = nodes.First();
                nodes.Remove(smallest);

                if ((smallest == destination) || (distances[smallest] == infinity))
                    break;

                smallest.ForEachNeighbor((Node<CONTENT> neighbor) =>
                {
                    var distance = smallest.Cost(neighbor);
                    var alt = distances[smallest] + distance;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = smallest;
                    }
                });
            }

            return previous;
        }

        /// <summary>
        /// Shortest path to the specified destination, from a dictionary of paths previously computed by Dijkstra
        /// </summary>
        /// <returns>The shortest path to the destination</returns>
        /// <param name="destination">Destination.</param>
        /// <param name="paths">Paths previously calculated by Dijkstra</param>
        public List<Node<CONTENT>> Path(Node<CONTENT> destination, Dictionary<Node<CONTENT>, Node<CONTENT>> paths)
        {
            var path = new List<Node<CONTENT>>();
            var via = destination;
            while (paths.ContainsKey(via))
            {
                path.Add(via);
                via = paths[via];
            }
            return path;
        }

        #endregion paths
    }
}
