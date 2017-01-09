using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Rojy.Graph;

namespace AvalonAssets.Rojy.Grid.Hex
{
    public class HexGrid<T> : IGraph<IHexCoordinate>, IEnumerable<KeyValuePair<IHexCoordinate, T>>
    {
        private readonly Dictionary<IHexCoordinate, T> _grid;

        public HexGrid()
        {
            _grid = new Dictionary<IHexCoordinate, T>();
        }

        public T this[IHexCoordinate coordinate]
        {
            get { return _grid[coordinate]; }
            set { _grid[coordinate] = value; }
        }

        public int Count => _grid.Count;

        public Dictionary<IHexCoordinate, T>.KeyCollection Keys => _grid.Keys;

        public Dictionary<IHexCoordinate, T>.ValueCollection Values => _grid.Values;

        public void Add(IHexCoordinate coordinate, T value)
        {
            _grid.Add(coordinate, value);
        }

        public bool TryGetValue(IHexCoordinate coordinate, out T value)
        {
            return _grid.TryGetValue(coordinate, out value);
        }

        public bool ContainsKey(IHexCoordinate coordinate)
        {
            return _grid.ContainsKey(coordinate);
        }

        public bool ContainsValue(T value)
        {
            return _grid.ContainsValue(value);
        }

        #region IGraph

        bool IGraph<IHexCoordinate>.AllowSelfLoops => true;

        IEnumerable<EndPointPair<IHexCoordinate>> IGraph<IHexCoordinate>.Edges
        {
            get
            {
                var returnSet = new HashSet<EndPointPair<IHexCoordinate>>();
                foreach (var node in _grid.Keys)
                {
                    var negihbors = node.AllNeighbors().Where(n => _grid.ContainsKey(n)).ToList();
                    foreach (var negihbor in negihbors)
                    {
                        var pair = EndPointPair<IHexCoordinate>.Undirected(node, negihbor);
                        if (returnSet.Contains(pair)) continue;
                        returnSet.Add(pair);
                        yield return pair;
                    }
                }
            }
        }

        bool IGraph<IHexCoordinate>.IsDirected => false;

        IEnumerable<IHexCoordinate> IGraph<IHexCoordinate>.Nodes => _grid.Keys;

        IEnumerable<IHexCoordinate> IGraph<IHexCoordinate>.Neighbors(IHexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        int IGraph<IHexCoordinate>.Degree(IHexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        int IGraph<IHexCoordinate>.InDegree(IHexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        int IGraph<IHexCoordinate>.OutDegree(IHexCoordinate node)
        {
            return node.AllNeighbors().Count(n => _grid.ContainsKey(n));
        }

        IEnumerable<IHexCoordinate> IGraph<IHexCoordinate>.Predecessors(IHexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        IEnumerable<IHexCoordinate> IGraph<IHexCoordinate>.Successors(IHexCoordinate node)
        {
            return node.AllNeighbors().Where(n => _grid.ContainsKey(n));
        }

        #endregion

        #region IEnumerable

        public IEnumerator<KeyValuePair<IHexCoordinate, T>> GetEnumerator()
        {
            return _grid.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}