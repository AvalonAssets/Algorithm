using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Rojy.Graph
{
    public class Graph<TNode> : IMutableGraph<TNode>
    {
        protected readonly IDictionary<TNode, GraphConnection<TNode>> Connections;

        protected Graph(bool allowSelfLoops, bool isDirected)
        {
            AllowSelfLoops = allowSelfLoops;
            IsDirected = isDirected;
            Connections = new Dictionary<TNode, GraphConnection<TNode>>();
        }

        public bool AllowSelfLoops { get; }

        public virtual IEnumerable<EndPointPair<TNode>> Edges
        {
            get
            {
                return Connections.SelectMany(pair => pair.Value.Successors(),
                    (pair, successor) => Pair(pair.Key, successor));
            }
        }

        public bool IsDirected { get; }

        public virtual int Degree(TNode node)
        {
            if (IsDirected)
            {
                var predecessors = Predecessors(node).ToList();
                var successors = Successors(node).ToList();
                return predecessors.Count + successors.Count -
                       (predecessors.Contains(node) && successors.Contains(node) ? 1 : 0);
            }
            var neighbors = Neighbors(node).ToList();
            var selfCount = AllowSelfLoops && neighbors.Contains(node) ? 1 : 0;
            return neighbors.Count + selfCount;
        }

        public virtual int InDegree(TNode node)
        {
            return IsDirected ? Predecessors(node).Count() : Degree(node);
        }

        public virtual int OutDegree(TNode node)
        {
            return IsDirected ? Successors(node).Count() : Degree(node);
        }

        public IEnumerable<TNode> Nodes => Connections.Keys;

        public IEnumerable<TNode> Neighbors(TNode node)
        {
            return !Connections.ContainsKey(node) ? new List<TNode>() : Connections[node].Neighbors();
        }

        public IEnumerable<TNode> Predecessors(TNode node)
        {
            return !Connections.ContainsKey(node) ? new List<TNode>() : Connections[node].Predecessors();
        }

        public IEnumerable<TNode> Successors(TNode node)
        {
            return !Connections.ContainsKey(node) ? new List<TNode>() : Connections[node].Successors();
        }

        public virtual bool AddNode(TNode node)
        {
            if (Connections.ContainsKey(node))
                return false;
            Connections.Add(node, new GraphConnection<TNode>());
            return true;
        }

        public virtual bool PutEdge(TNode nodeU, TNode nodeV)
        {
            if (!Connections.ContainsKey(nodeU) || !Connections.ContainsKey(nodeV))
                return false;
            if (Equals(nodeU, nodeV) && !AllowSelfLoops)
                throw new InvalidOperationException("Self loop is not allowed.");
            Connections[nodeU].AddSuccessor(nodeV);
            Connections[nodeV].AddPredecessor(nodeU);
            if (IsDirected) return true;
            Connections[nodeU].AddPredecessor(nodeV);
            Connections[nodeV].AddSuccessor(nodeU);
            return true;
        }

        public virtual bool RemoveNode(TNode node)
        {
            if (!Connections.ContainsKey(node))
                return false;
            foreach (var connection in Connections.Values)
            {
                connection.RemovePredecessor(node);
                connection.RemoveSuccessor(node);
            }
            Connections.Remove(node);
            return true;
        }

        public virtual bool RemoveEdge(TNode nodeU, TNode nodeV)
        {
            if (!Connections.ContainsKey(nodeU) || !Connections.ContainsKey(nodeV))
                return false;
            var removed = Connections[nodeU].RemoveSuccessor(nodeV);
            removed = Connections[nodeV].RemovePredecessor(nodeU) || removed;
            if (IsDirected) return removed;
            removed = Connections[nodeU].RemovePredecessor(nodeV) || removed;
            removed = Connections[nodeV].RemoveSuccessor(nodeU) || removed;
            return removed;
        }

        public static IMutableGraph<TNode> Directed(bool allowSelfLoops = false)
        {
            return new Graph<TNode>(allowSelfLoops, true);
        }

        public static IMutableGraph<TNode> Undirected(bool allowSelfLoops = false)
        {
            return new Graph<TNode>(allowSelfLoops, false);
        }

        public static IMutableValueGraph<TNode, TValue> Directed<TValue>(bool allowSelfLoops = false)
        {
            return new ValueGraph<TNode, TValue>(allowSelfLoops, true);
        }

        public static IMutableValueGraph<TNode, TValue> Undirected<TValue>(bool allowSelfLoops = false)
        {
            return new ValueGraph<TNode, TValue>(allowSelfLoops, false);
        }

        public override string ToString()
        {
            return $"Graph[IsDirected:{IsDirected}, AllowSelfLoops:{AllowSelfLoops}, Nodes:{Nodes}, Edges:{Edges}]";
        }
        
        protected virtual EndPointPair<TNode> Pair(TNode nodeU, TNode nodeV)
        {
            return IsDirected
                ? EndPointPair<TNode>.Directed(nodeU, nodeV)
                : EndPointPair<TNode>.Undirected(nodeU, nodeV);
        }
    }
}