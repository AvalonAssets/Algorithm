namespace AvalonAssets.Rojy.Graph
{
    /// <summary>
    ///     Mutable graph interface.
    /// </summary>
    /// <typeparam name="TNode">Node Type.</typeparam>
    public interface IMutableGraph<TNode> : IGraph<TNode>
    {
        /// <summary>
        ///     Adds a node to graph.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Is the graph changes.</returns>
        bool AddNode(TNode node);

        /// <summary>
        ///     Adds a edge to graph.
        /// </summary>
        /// <param name="nodeU">Node.</param>
        /// <param name="nodeV">Node.</param>
        /// <returns>Is the graph changes.</returns>
        bool PutEdge(TNode nodeU, TNode nodeV);

        /// <summary>
        ///     Removes a node from graph.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Is the graph changes.</returns>
        bool RemoveNode(TNode node);

        /// <summary>
        ///     Removes a edge from graph.
        /// </summary>
        /// <param name="nodeU">Node.</param>
        /// <param name="nodeV">Node.</param>
        /// <returns>Is the graph changes.</returns>
        bool RemoveEdge(TNode nodeU, TNode nodeV);
    }
}