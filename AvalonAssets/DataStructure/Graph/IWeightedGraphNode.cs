﻿using System.Collections.Generic;

namespace AvalonAssets.DataStructure.Graph
{
    /// <summary>
    ///     Simple weighted graph node.
    /// </summary>
    public interface IWeightedGraphNode<T> where T : IWeightedGraphNode<T>
    {
        /// <summary>
        ///     Get the weight from curent node to neighbor node.
        /// </summary>
        /// <param name="neighbor">Neighbor of this node.</param>
        /// <returns>Required weight.</returns>
        int GetWeight(T neighbor);

        /// <summary>
        ///     Get the neighbor of current node.
        /// </summary>
        /// <returns>Neighbor of this node.</returns>
        IEnumerable<T> GetNeighbors();
    }
}