using System;
using System.Collections;
using System.Collections.Generic;

namespace AvalonAssets.Core.Data.Heap
{
    /// <summary>
    ///     Binomial Heap implemenation of <see cref="IHeap{T}" />.
    ///     <seealso cref="IHeap{T}" />
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class BinomialHeap<T> : IHeap<T>
    {
        private readonly IComparer<T> _comparer;
        private Node<T> _head;

        public BinomialHeap(IComparer<T> comparer)
        {
            _head = null;
            _comparer = comparer;
        }

        private BinomialHeap(Node<T> node, IComparer<T> comparer)
        {
            _head = node;
            _comparer = comparer;
        }

        public IHeapNode<T> Insert(T element)
        {
            var node = new Node<T>(element);
            _head = Union(new BinomialHeap<T>(node, _comparer));
            return node;
        }

        public IHeapNode<T> ExtractMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            var min = _head;
            Node<T> minPrev = null;
            var next = min.Sibling;
            var nextPrev = min;
            while (next != null)
            {
                if (Compare(next, min) < 0)
                {
                    min = next;
                    minPrev = nextPrev;
                }
                nextPrev = next;
                next = next.Sibling;
            }
            RemoveTreeRoot(min, minPrev);
            return min;
        }

        public bool IsEmpty => _head == null;
        public int Size => IsEmpty ? 0 : _head.Count();

        public IHeapNode<T> GetMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            var min = _head;
            var next = min.Sibling;
            while (next != null)
            {
                if (Compare(next, min) < 0)
                    min = next;
                next = next.Sibling;
            }
            return min;
        }

        public void DecreaseKey(IHeapNode<T> element)
        {
            var node = element as Node<T>;
            if (node == null)
                throw new ArgumentException();
            UpHeap(node, false);
        }

        public void Delete(IHeapNode<T> element)
        {
            var node = element as Node<T>;
            if (node == null)
                throw new ArgumentException();
            node = UpHeap(node, true);
            if (_head == node)
                RemoveTreeRoot(node, null);
            else
            {
                var prev = _head;
                while (Compare(prev.Sibling, node) != 0)
                {
                    prev = prev.Sibling;
                }
                RemoveTreeRoot(node, prev);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var list = GetNodesValue(_head);
            list.Sort(_comparer);
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int Compare(IHeapNode<T> left, IHeapNode<T> right)
        {
            return _comparer.Compare(left.Value, right.Value);
        }

        private Node<T> UpHeap(Node<T> node, bool toRoot)
        {
            var parent = node.Parent;
            while (parent != null && (toRoot || Compare(node, parent) < 0))
            {
                var temp = node.Value;
                node.Value = parent.Value;
                parent.Value = temp;
                node = parent;
                parent = parent.Parent;
            }
            return node;
        }

        private void RemoveTreeRoot(Node<T> root, Node<T> prev)
        {
            if (root == _head)
                _head = root.Sibling;
            else
                prev.Sibling = root.Sibling;
            Node<T> newHead = null;
            var child = root.Child;
            while (child != null)
            {
                var next = child.Sibling;
                child.Sibling = newHead;
                child.Parent = null;
                newHead = child;
                child = next;
            }
            var newHeap = new BinomialHeap<T>(newHead, _comparer);
            _head = Union(newHeap);
        }

        private static void LinkTree(Node<T> minNodeTree, Node<T> other)
        {
            other.Parent = minNodeTree;
            other.Sibling = minNodeTree.Child;
            minNodeTree.Child = other;
            minNodeTree.Degree++;
        }

        /// <summary>
        ///     Combine the heap with another to form a valid binomial heap.
        /// </summary>
        /// <param name="heap">Another binomial heap.</param>
        public void Merge(BinomialHeap<T> heap)
        {
            _head = Union(heap);
        }

        private Node<T> Union(BinomialHeap<T> heap)
        {
            var newHead = Union(this, heap);
            _head = null;
            heap._head = null;
            if (newHead == null)
                return null;
            Node<T> prev = null;
            var curr = newHead;
            var next = newHead.Sibling;
            while (next != null)
            {
                if (curr.Degree != next.Degree || (next.Sibling != null && next.Sibling.Degree == curr.Degree))
                {
                    prev = curr;
                    curr = next;
                }
                else
                {
                    if (Compare(curr, next) < 0)
                    {
                        curr.Sibling = next.Sibling;
                        LinkTree(curr, next);
                    }
                    else
                    {
                        if (prev == null)
                            newHead = next;
                        else
                            prev.Sibling = next;
                        LinkTree(next, curr);
                        curr = next;
                    }
                }
                next = curr.Sibling;
            }
            return newHead;
        }

        private static Node<T> Union(BinomialHeap<T> heap1, BinomialHeap<T> heap2)
        {
            if (heap1._head == null)
                return heap2._head;
            if (heap2._head == null)
                return heap1._head;
            Node<T> head;
            var heap1Next = heap1._head;
            var heap2Next = heap2._head;
            if (heap1._head.Degree <= heap2._head.Degree)
            {
                head = heap1._head;
                heap1Next = heap1Next.Sibling;
            }
            else
            {
                head = heap2._head;
                heap2Next = heap2Next.Sibling;
            }
            var tail = head;
            while (heap1Next != null && heap2Next != null)
            {
                if (heap1Next.Degree <= heap2Next.Degree)
                {
                    tail.Sibling = heap1Next;
                    heap1Next = heap1Next.Sibling;
                }
                else
                {
                    tail.Sibling = heap2Next;
                    heap2Next = heap2Next.Sibling;
                }
                tail = tail.Sibling;
            }
            tail.Sibling = heap1Next ?? heap2Next;
            return head;
        }

        private static List<T> GetNodesValue(Node<T> node)
        {
            var list = new List<T>();
            if (node == null)
                return list;
            list.Add(node.Value);
            list.AddRange(GetNodesValue(node.Sibling));
            list.AddRange(GetNodesValue(node.Child));
            return list;
        }

        private class Node<TValue> : IHeapNode<TValue>
        {
            public Node(TValue value)
            {
                Value = value;
                Degree = 0;
                Parent = null;
                Child = null;
                Sibling = null;
            }

            public int Degree { get; set; }
            public Node<TValue> Parent { get; set; }
            public Node<TValue> Child { get; set; }
            public Node<TValue> Sibling { get; set; }
            public TValue Value { get; set; }

            public int Count()
            {
                var count = 1;
                if (Child != null)
                    count += Child.Count();
                if (Sibling != null)
                    count += Sibling.Count();
                return count;
            }
        }
    }
}