namespace AvalonAssets.Rojy.Graph
{
    /// <summary>
    ///     Mutable value graph interface.
    /// </summary>
    /// <typeparam name="TNode">Node Type.</typeparam>
    /// <typeparam name="TValue">Edge value type.</typeparam>
    public interface IMutableValueGraph<TNode, TValue> : IValueGraph<TNode, TValue>
    {
        /// <summary>
        ///     Adds a node to graph.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>Is the graph changes.</returns>
        bool AddNode(TNode node);

        /// <summary>
        ///     Adds a edge to graph with value.
        /// </summary>
        /// <param name="nodeU">Node.</param>
        /// <param name="nodeV">Node.</param>
        /// <param name="value">Edge value.</param>
        /// <returns>Is the graph changes.</returns>
        bool PutEdge(TNode nodeU, TNode nodeV, TValue value);

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