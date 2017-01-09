using System.Collections.Generic;
using AvalonAssets.Core.Data.Queue;
using AvalonAssets.Rojy.Graph;

namespace AvalonAssets.Rojy
{
    /// <summary>
    ///     Path finding algorithm for <see cref="IGraph{TNode}"/> and <see cref="IValueGraph{TNode,TValue}"/>.
    /// </summary>
    public static class PathFinding
    {
        /// <summary>
        ///     Returns the distance between <paramref name="node" /> and <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="goal">Goal node.</param>
        /// <param name="node">Current node.</param>
        /// <returns>Distance between <paramref name="node" /> and <paramref name="goal" /></returns>
        public delegate int Heuristic<in T>(T goal, T node);

        /// <summary>
        ///     Creates path from visit history.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="from">History of visit.</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref name="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        private static IEnumerable<T> CreatePath<T>(IDictionary<T, T> from, T start, T goal)
        {
            if (!from.ContainsKey(goal))
                return new List<T>();
            var current = goal;
            var path = new List<T> {current};
            while (!current.Equals(start))
            {
                current = from[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        #region Weight

        /// <summary>
        ///     Using Dijkstra's Algorithm to find path with given <see cref="IValueGraph{TNode,TValue}" />
        ///     from <paramref name="start" /> to <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref name="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> DijkstraAlgorithm<T>(this IValueGraph<T, int> graph, T start, T goal)
        {
            if(graph.InDegree(goal) == 0 || graph.OutDegree(start) == 0)
                return new List<T>();
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            var weights = new Dictionary<T, int> {{start, 0}};
            while (!frontiers.IsEmpty)
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in graph.Neighbors(frontier))
                {
                    var newCost = weights[frontier] + graph.EdgeValue(frontier, neighbor);
                    if (weights.ContainsKey(neighbor) && newCost >= weights[neighbor]) continue;
                    weights[neighbor] = newCost;
                    frontiers.Enqueue(newCost, neighbor);
                    from[neighbor] = frontier;
                }
            }
            return CreatePath(from, start, goal);
        }

        /// <summary>
        ///     Using A* Algorithm to find path with given <see cref="IValueGraph{TNode,TValue}" />
        ///     from <paramref name="start" /> to <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Weighted graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref name="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> AStarAlgorithm<T>(this IValueGraph<T, int> graph, T start, T goal,
            Heuristic<T> heuristic)
        {
            if (graph.InDegree(goal) == 0 || graph.OutDegree(start) == 0)
                return new List<T>();
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            var weights = new Dictionary<T, int> {{start, 0}};
            while (!frontiers.IsEmpty)
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in graph.Neighbors(frontier))
                {
                    var newCost = weights[frontier] + graph.EdgeValue(frontier, neighbor);
                    if (weights.ContainsKey(neighbor) && newCost >= weights[neighbor]) continue;
                    weights[neighbor] = newCost;
                    frontiers.Enqueue(newCost + heuristic(goal, neighbor), neighbor);
                    from[neighbor] = frontier;
                }
            }
            return CreatePath(from, start, goal);
        }

        #endregion

        #region Non-weight

        /// <summary>
        ///     Using Breadth First Search to find path with given <see cref="IGraph{T}" />
        ///     from <paramref name="start" /> to <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref name="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        public static IEnumerable<T> BreadthFirstSearch<T>(this IGraph<T> graph, T start, T goal)
        {
            if (graph.InDegree(goal) == 0 || graph.OutDegree(start) == 0)
                return new List<T>();
            var frontiers = new Queue<T>();
            frontiers.Enqueue(start);
            var from = new Dictionary<T, T>();
            from.Add(start, default(T));
            while (frontiers.Count > 0)
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in graph.Neighbors(frontier))
                {
                    if (from.ContainsKey(neighbor)) continue;
                    frontiers.Enqueue(neighbor);
                    from.Add(neighbor, frontier);
                }
            }
            return CreatePath(from, start, goal);
        }

        /// <summary>
        ///     Using Heuristic Search to find path with given <see cref="IGraph{T}" />
        ///     from <paramref name="start" /> to <paramref name="goal" />.
        /// </summary>
        /// <typeparam name="T">Graph node.</typeparam>
        /// <param name="graph">Graph</param>
        /// <param name="start">Start of the path.</param>
        /// <param name="goal">End of the path.</param>
        /// <param name="heuristic">Heuristic function.</param>
        /// <returns>
        ///     Path from <paramref name="start" /> to <paramref name="goal" />.
        ///     Empty if no path can be found.
        /// </returns>
        /// <seealso cref="Heuristic{T}" />
        public static IEnumerable<T> HeuristicSearch<T>(this IGraph<T> graph, T start, T goal, Heuristic<T> heuristic)
        {
            if (graph.InDegree(goal) == 0 || graph.OutDegree(start) == 0)
                return new List<T>();
            var frontiers = new PriorityQueue<T>();
            frontiers.Enqueue(0, start);
            var from = new Dictionary<T, T> {{start, default(T)}};
            while (!frontiers.IsEmpty)
            {
                var frontier = frontiers.Dequeue();
                if (start.Equals(goal))
                    break;
                foreach (var neighbor in graph.Neighbors(frontier))
                {
                    if (from.ContainsKey(neighbor)) continue;
                    frontiers.Enqueue(heuristic(goal, neighbor), neighbor);
                    from.Add(neighbor, frontier);
                }
            }
            return CreatePath(from, start, goal);
        }

        #endregion
    }
}