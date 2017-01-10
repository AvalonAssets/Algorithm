using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Core.Data.Heap
{
    /// <summary>
    ///     Fibonacci Heap implemenation of <see cref="IHeap{T}" />.
    ///     <seealso cref="IHeap{T}" />
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class FibonacciHeap<T> : IHeap<T>
    {
        private readonly IComparer<T> _comparer;
        private Node<T> _minNode;

        public FibonacciHeap(IComparer<T> comparer)
        {
            _minNode = null;
            Count = 0;
            _comparer = comparer;
        }

        public IHeapNode<T> Insert(T element)
        {
            var node = new Node<T>(element);
            _minNode = MergeLists(_minNode, node);
            Count++;
            return node;
        }

        public IHeapNode<T> Extract()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            var extractedMin = _minNode;
            if (extractedMin.Child != null)
            {
                var child = extractedMin.Child;
                do
                {
                    child.Parent = null;
                    child = child.Next;
                } while (child != extractedMin.Child);
            }
            var nextInRootList = extractedMin.Next == extractedMin ? null : extractedMin.Next;
            RemoveNodeFromList(extractedMin);
            Count--;
            _minNode = MergeLists(nextInRootList, extractedMin.Child);
            if (nextInRootList == null) return extractedMin;
            _minNode = nextInRootList;
            Consolidate();
            return extractedMin;
        }

        public bool IsEmpty => Count == 0;
        public int Count { get; private set; }

        public IHeapNode<T> Get()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            return _minNode;
        }

        public void DecreaseKey(IHeapNode<T> element)
        {
            var node = element as Node<T>;
            if (node == null)
                throw new ArgumentException();
            var parent = node.Parent;
            if (parent != null && Compare(node, parent) < 0)
            {
                Cut(node, parent);
                CascadingCut(parent);
            }
            if (Compare(node, _minNode) < 0)
                _minNode = node;
        }

        public void Delete(IHeapNode<T> element)
        {
            var node = element as Node<T>;
            if (node == null)
                throw new ArgumentException();
            var parent = node.Parent;
            if (parent != null)
            {
                Cut(node, parent);
                CascadingCut(parent);
            }
            _minNode = node;
            Extract();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var list = GetRootTrees().Select(n => n.Value).ToList();
            list.Sort(_comparer);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Combine the heap with another to form a valid Fibonacci heap.
        /// </summary>
        /// <param name="heap">Another Fibonacci heap.</param>
        public void Merge(FibonacciHeap<T> heap)
        {
            _minNode = MergeLists(_minNode, heap._minNode);
            Count += heap.Count;
        }

        private void CascadingCut(Node<T> node)
        {
            while (true)
            {
                var parent = node.Parent;
                if (parent == null) return;
                if (node.IsMarked)
                {
                    Cut(node, parent);
                    node = parent;
                    continue;
                }
                node.IsMarked = true;
                break;
            }
        }

        private void Cut(Node<T> node, Node<T> parent)
        {
            parent.Degree--;
            parent.Child = node.Next == node ? null : node.Next;
            RemoveNodeFromList(node);
            MergeLists(_minNode, node);
            node.IsMarked = false;
        }

        private void Consolidate()
        {
            if (_minNode == null)
                return;
            var aux = new List<Node<T>>();
            var items = GetRootTrees();
            foreach (var current in items)
            {
                var top = current;
                while (aux.Count <= top.Degree + 1)
                    aux.Add(null);
                while (aux[top.Degree] != null)
                {
                    if (Compare(aux[top.Degree], top) < 0)
                    {
                        var temp = top;
                        top = aux[top.Degree];
                        aux[top.Degree] = temp;
                    }
                    LinkHeaps(aux[top.Degree], top);
                    aux[top.Degree] = null;
                    top.Degree++;
                }
                while (aux.Count <= top.Degree + 1)
                    aux.Add(null);
                aux[top.Degree] = top;
            }
            _minNode = null;
            foreach (var t in aux)
            {
                if (t == null) continue;
                t.Next = t;
                t.Prev = t;
                _minNode = MergeLists(_minNode, t);
            }
        }

        private IEnumerable<Node<T>> GetRootTrees()
        {
            var items = new Queue<Node<T>>();
            var current = _minNode;
            do
            {
                items.Enqueue(current);
                current = current.Next;
            } while (_minNode != current);
            return items;
        }

        private void LinkHeaps(Node<T> max, Node<T> min)
        {
            RemoveNodeFromList(max);
            min.Child = MergeLists(max, min.Child);
            max.Parent = min;
            max.IsMarked = false;
        }

        private static void RemoveNodeFromList(Node<T> node)
        {
            var prev = node.Prev;
            var next = node.Next;
            prev.Next = next;
            next.Prev = prev;
            node.Next = node;
            node.Prev = node;
        }

        private int Compare(IHeapNode<T> left, IHeapNode<T> right)
        {
            return _comparer.Compare(left.Value, right.Value);
        }

        private Node<T> MergeLists(Node<T> a, Node<T> b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;
            var temp = a.Next;
            a.Next = b.Next;
            a.Next.Prev = a;
            b.Next = temp;
            b.Next.Prev = b;
            return Compare(a, b) < 0 ? a : b;
        }

        private class Node<TValue> : IHeapNode<TValue>
        {
            public Node(TValue value)
            {
                Value = value;
                Next = this;
                Prev = this;
                Parent = null;
                Child = null;
                IsMarked = false;
                Degree = 0;
            }

            public int Degree { get; set; }
            public Node<TValue> Parent { get; set; }
            public Node<TValue> Child { get; set; }
            public Node<TValue> Prev { get; set; }
            public Node<TValue> Next { get; set; }
            public bool IsMarked { get; set; }
            public TValue Value { get; set; }
        }
    }
}