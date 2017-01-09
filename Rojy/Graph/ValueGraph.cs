using System;
using System.Collections.Generic;

namespace AvalonAssets.Rojy.Graph
{
    public class ValueGraph<TNode, TValue> : Graph<TNode>, IMutableValueGraph<TNode, TValue>
    {
        private readonly IDictionary<EndPointPair<TNode>, TValue> _edgeValues;

        internal ValueGraph(bool allowSelfLoops, bool isDirected) :
            base(allowSelfLoops, isDirected)
        {
            _edgeValues = new Dictionary<EndPointPair<TNode>, TValue>();
        }

        public virtual TValue EdgeValue(TNode nodeU, TNode nodeV)
        {
            var key = Pair(nodeU, nodeV);
            if (!_edgeValues.ContainsKey(key))
                throw new ArgumentException("Node does not exists.");
            return _edgeValues[key];
        }

        public virtual TValue EdgeValueOrDefault(TNode nodeU, TNode nodeV, TValue defaultValue)
        {
            try
            {
                return EdgeValue(nodeU, nodeV);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        public bool PutEdge(TNode nodeU, TNode nodeV, TValue value)
        {
            if (!base.PutEdge(nodeU, nodeV))
                return false;
            _edgeValues[Pair(nodeU, nodeV)] = value;
            return true;
        }

        /// <summary>
        ///     This is a invalid operation in value graph.
        /// </summary>
        /// <param name="nodeU"></param>
        /// <param name="nodeV"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        public override bool PutEdge(TNode nodeU, TNode nodeV)
        {
            throw new InvalidOperationException();
        }
    }
}