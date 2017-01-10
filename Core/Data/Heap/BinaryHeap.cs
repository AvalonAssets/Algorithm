using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Core.Data.Heap
{
    /// <summary>
    ///     Binary Heap implemenation of <see cref="IHeap{T}" />.
    ///     <seealso cref="IHeap{T}" />
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class BinaryHeap<T> : IHeap<T>
    {
        private readonly IComparer<T> _comparer;
        private readonly List<Node<T>> _heap;

        public BinaryHeap(IComparer<T> comparer, IEnumerable<T> elements = null)
        {
            _heap = new List<Node<T>>();
            _comparer = comparer;
            if (elements == null) return;
            foreach (var element in elements)
            {
                _heap.Add(new Node<T>(element));
            }
            Build();
        }

        public IHeapNode<T> Insert(T element)
        {
            var node = new Node<T>(element);
            _heap.Add(node);
            UpHeap(Count - 1);
            return node;
        }

        public IHeapNode<T> Extract()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            var min = _heap[0];
            var last = Count - 1;
            _heap[0] = _heap[last];
            _heap.RemoveAt(last);
            DownHeap(0);
            return min;
        }

        public IHeapNode<T> Get()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Empty heap");
            return _heap[0];
        }

        public int Count => _heap.Count;
        public bool IsEmpty => Count == 0;

        public void DecreaseKey(IHeapNode<T> element)
        {
            var node = element as Node<T>;
            if (node == null)
                throw new ArgumentException();
            var index = _heap.IndexOf(node);
            if (index >= 0)
                UpHeap(index);
        }

        public void Delete(IHeapNode<T> element)
        {
            var index = _heap.FindLastIndex(e => e.Equals(element));
            if (index == 0 && Count == 1)
            {
                _heap.RemoveAt(index);
                return;
            }
            if (index == Count - 1)
            {
                _heap.RemoveAt(index);
                return;
            }
            Swap(index, Count - 1);
            _heap.RemoveAt(Count - 1);
            var parent = Parent(index);
            if (index != 0 && Compare(_heap[index], _heap[parent]) < 0)
                UpHeap(index);
            else
                DownHeap(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var valueList = _heap.Select(n => n.Value).ToList();
            valueList.Sort(_comparer);
            return valueList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static int Parent(int index)
        {
            return (index - 1)/2;
        }

        private static int LeftChild(int index)
        {
            return 2*index + 1;
        }

        private static int RightChild(int index)
        {
            return 2*index + 2;
        }

        private void Swap(int i, int j)
        {
            var tmp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = tmp;
        }

        private int Compare(IHeapNode<T> left, IHeapNode<T> right)
        {
            return _comparer.Compare(left.Value, right.Value);
        }

        private void UpHeap(int position)
        {
            while (position != 0 && Compare(_heap[position], _heap[Parent(position)]) < 0)
            {
                Swap(position, Parent(position));
                position = Parent(position);
            }
        }

        private void DownHeap(int position)
        {
            while (true)
            {
                var left = LeftChild(position);
                var right = RightChild(position);
                var smallest = position;
                if (left < Count && Compare(_heap[left], _heap[smallest]) < 0)
                    smallest = left;
                if (right < Count && Compare(_heap[right], _heap[smallest]) < 0)
                    smallest = right;
                if (smallest == position)
                    break;
                Swap(smallest, position);
                position = smallest;
            }
        }

        /// <summary>
        ///     Combine the heap with another to form a valid binary heap.
        /// </summary>
        /// <param name="heap">Another binary heap.</param>
        public void Merge(BinaryHeap<T> heap)
        {
            _heap.AddRange(heap._heap);
            Build();
        }

        private void Build()
        {
            for (var i = Count/2; i >= 0; i--)
                DownHeap(i);
        }

        private class Node<TValue> : IHeapNode<TValue>
        {
            public Node(TValue value)
            {
                Value = value;
            }

            public TValue Value { get; set; }
        }
    }
}