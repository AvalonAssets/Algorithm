using System.Collections.Generic;

namespace AvalonAssets.Rojy.Graph
{
    /// <summary>
    ///     Basic graph interface.
    /// </summary>
    /// <typeparam name="TNode">ode type.</typeparam>
    public interface IGraph<TNode>
    {
        /// <summary>
        ///     Returns if self loops is allowed.
        /// </summary>
        bool AllowSelfLoops { get; }

        /// <summary>
        ///     Returns all the edges in the graph.
        /// </summary>
        IEnumerable<EndPointPair<TNode>> Edges { get; }

        /// <summary>
        ///     Returns if the graph is directed.
        /// </summary>
        bool IsDirected { get; }

        /// <summary>
        ///     Returns all the nodes in the graph.
        /// </summary>
        IEnumerable<TNode> Nodes { get; }

        /// <summary>
        ///     Get the neighbors of current node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Neighbor of <paramref name="node" />.</returns>
        IEnumerable<TNode> Neighbors(TNode node);

        /// <summary>
        ///     Returns the number of edges on <paramref name="node" />.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Number of edges.</returns>
        int Degree(TNode node);

        /// <summary>
        ///     Returns the number of incoming edges (i.e. <paramref name="node" /> is destination) on <paramref name="node" />.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Number of incoming edges.</returns>
        int InDegree(TNode node);

        /// <summary>
        ///     Returns the number of outgoing edges (i.e. <paramref name="node" /> is start) on <paramref name="node" />.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Number of outgoing edges.</returns>
        int OutDegree(TNode node);

        /// <summary>
        ///     Returns all the nodes that is connection by incoming edges (i.e. <paramref name="node" /> is destination) on
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Incoming nodes.</returns>
        IEnumerable<TNode> Predecessors(TNode node);

        /// <summary>
        ///     Returns all the nodes that is connection by outgoing edges (i.e. <paramref name="node" /> is start) on
        ///     <paramref name="node" />.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Outgoing nodes.</returns>
        IEnumerable<TNode> Successors(TNode node);
    }
}