using System;

namespace AvalonAssets.Rojy.Graph
{
    /// <summary>
    ///     Graph interface with edge value.
    /// </summary>
    /// <typeparam name="TNode">Node Type.</typeparam>
    /// <typeparam name="TValue">Edge value type.</typeparam>
    public interface IValueGraph<TNode, TValue> : IGraph<TNode>
    {
        /// <summary>
        ///     Returns the value of given edge.
        /// </summary>
        /// <param name="nodeU">Node.</param>
        /// <param name="nodeV">Node.</param>
        /// <returns>Edge value.</returns>
        /// <exception cref="ArgumentException">
        ///     Node does not exists.
        /// </exception>
        TValue EdgeValue(TNode nodeU, TNode nodeV);

        /// <summary>
        ///     Returns the value of given edge. If the node cannot be found, return default value instead.
        /// </summary>
        /// <param name="nodeU">Node.</param>
        /// <param name="nodeV">Node.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Edge value.</returns>
        TValue EdgeValueOrDefault(TNode nodeU, TNode nodeV, TValue defaultValue);
    }
}