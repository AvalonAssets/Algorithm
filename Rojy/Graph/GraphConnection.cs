using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Rojy.Graph
{
    public class GraphConnection<TNode>
    {
        private readonly IDictionary<TNode, ConnectionType> _neighbors;

        public GraphConnection()
        {
            _neighbors = new Dictionary<TNode, ConnectionType>();
        }

        public IEnumerable<TNode> Predecessors()
        {
            return _neighbors.Where(n => (n.Value & ConnectionType.Predecessor) == ConnectionType.Predecessor)
                .Select(n => n.Key);
        }

        public bool IsPredecessors(TNode node)
        {
            return _neighbors.ContainsKey(node) && Contains(_neighbors[node], ConnectionType.Predecessor);
        }

        public IEnumerable<TNode> Successors()
        {
            return _neighbors.Where(n => (n.Value & ConnectionType.Successor) == ConnectionType.Successor)
                .Select(n => n.Key);
        }

        public bool IsSuccessors(TNode node)
        {
            return _neighbors.ContainsKey(node) && Contains(_neighbors[node], ConnectionType.Successor);
        }

        public IEnumerable<TNode> Neighbors()
        {
            return _neighbors.Keys;
        }

        public bool RemovePredecessor(TNode node)
        {
            return RemoveConnection(node, ConnectionType.Predecessor);
        }

        public bool RemoveSuccessor(TNode node)
        {
            return RemoveConnection(node, ConnectionType.Successor);
        }

        private bool RemoveConnection(TNode node, ConnectionType type)
        {
            if (!_neighbors.ContainsKey(node))
                return false;
            if (!Contains(_neighbors[node], type))
                return false;
            _neighbors[node] &= ~type;
            if (_neighbors[node] == ConnectionType.None)
                _neighbors.Remove(node);
            return true;
        }

        public void AddPredecessor(TNode node)
        {
            AddConnection(node, ConnectionType.Predecessor);
        }

        public void AddSuccessor(TNode node)
        {
            AddConnection(node, ConnectionType.Successor);
        }

        private void AddConnection(TNode node, ConnectionType type)
        {
            if (_neighbors.ContainsKey(node))
                _neighbors[node] |= type;
            else
                _neighbors[node] = type;
        }

        private static bool Contains(ConnectionType type, ConnectionType checkType)
        {
            return (type & checkType) == checkType;
        }

        [Flags]
        private enum ConnectionType : ushort
        {
            None = 0,
            Predecessor = 1 << 0,
            Successor = 1 << 1
        }
    }
}