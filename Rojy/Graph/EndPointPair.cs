using System;
using System.Collections;
using System.Collections.Generic;

namespace AvalonAssets.Rojy.Graph
{
    /// <summary>
    ///     Represents a connection between two nodes.
    /// </summary>
    /// <typeparam name="TNode">Node type.</typeparam>
    public abstract class EndPointPair<TNode> : IEnumerable<TNode>
    {
        protected readonly TNode N;
        protected readonly TNode U;

        /// <summary>
        ///     Use <see cref="Undirected" /> or <see cref="Directed" /> to initialize instead.
        /// </summary>
        /// <param name="u">Node.</param>
        /// <param name="n">Node.</param>
        protected EndPointPair(TNode u, TNode n)
        {
            U = u;
            N = n;
        }

        public virtual IEnumerator<TNode> GetEnumerator()
        {
            yield return U;
            yield return N;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns true if <paramref name="node" /> is in this connection.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>If <paramref name="node" /> is in this connection.</returns>
        public bool Contains(TNode node)
        {
            return Equals(N, node) || Equals(U, node);
        }

        /// <summary>
        ///     Returns another node on this connection.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Another node.</returns>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="node" /> does not exists in this connection.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="node" />  is null.</exception>
        public TNode Neighbor(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (node.Equals(U))
                return N;
            if (node.Equals(N))
                return U;
            throw new InvalidOperationException("node does not exist in this edge.");
        }

        /// <summary>
        ///     Creates a directed connection where the order matters.
        /// </summary>
        /// <param name="u">Start node.</param>
        /// <param name="n">End node.</param>
        /// <returns>Directed connection.</returns>
        public static EndPointPair<TNode> Directed(TNode u, TNode n)
        {
            return new DirectedPair(u, n);
        }

        /// <summary>
        ///     Creates a directed connection where the order does not matter.
        /// </summary>
        /// <param name="u">Node.</param>
        /// <param name="n">Node.</param>
        /// <returns>Undirected connection.</returns>
        public static EndPointPair<TNode> Undirected(TNode u, TNode n)
        {
            return new UndirectedPair(u, n);
        }

        public override string ToString()
        {
            return $"EndPointPair[{U}, {N}]";
        }

        private class DirectedPair : EndPointPair<TNode>
        {
            public DirectedPair(TNode u, TNode n) : base(u, n)
            {
            }

            public override bool Equals(object obj)
            {
                var pair = obj as DirectedPair;
                return pair != null && Equals(U, pair.U) && Equals(N, pair.N);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashN = EqualityComparer<TNode>.Default.GetHashCode(N);
                    var hashU = EqualityComparer<TNode>.Default.GetHashCode(U);
                    return hashN*397 ^ hashU;
                }
            }
        }

        private class UndirectedPair : EndPointPair<TNode>
        {
            public UndirectedPair(TNode u, TNode n) : base(u, n)
            {
            }

            public override bool Equals(object obj)
            {
                var pair = obj as EndPointPair<TNode>;
                return pair != null && Contains(pair.U) && Contains(pair.N) && pair.Contains(U) && pair.Contains(N);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashN = EqualityComparer<TNode>.Default.GetHashCode(N);
                    var hashU = EqualityComparer<TNode>.Default.GetHashCode(U);
                    return hashN*hashU + hashN + hashU;
                }
            }
        }
    }
}