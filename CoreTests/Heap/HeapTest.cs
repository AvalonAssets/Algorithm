using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AvalonAssets.Core.Data.Heap;
using AvalonAssets.Core.Utility;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Heap
{
    [TestFixture]
    public abstract class HeapTest
    {
        [SetUp]
        public void SetUp()
        {
            foreach (var num in RandomList())
                TestList.Add(num);
            MinHeap = CreateHeap(true);
            foreach (var element in TestList)
                MinHeap.Insert(element);
            MaxHeap = CreateHeap(false);
            foreach (var element in TestList)
                MaxHeap.Insert(element);
        }

        [TearDown]
        public void ClearUp()
        {
            TestList.Clear();
        }

        private IComparer<int> _minComparer;
        private IComparer<int> _maxComparer;

        [OneTimeSetUp]
        public virtual void Initialize()
        {
            _minComparer = Comparer<int>.Default;
            _maxComparer = Comparer<int>.Default.Reverse();
        }

        public const int Range = 100;
        protected readonly List<int> TestList = new List<int>();
        protected IHeap<int> MaxHeap;
        protected IHeap<int> MinHeap;
        public abstract IHeap<int> CreateHeap(bool isMin);
        private readonly Random _random = new Random();
        protected IContainer Container;

        protected int RandomNumber()
        {
            return _random.Next(-Range, Range);
        }

        protected IEnumerable<int> RandomList()
        {
            var total = _random.Next(5, 20);
            for (var i = 0; i < total; i++)
                yield return RandomNumber();
        }

        public void InsertTest(IHeap<int> heap, bool isMin)
        {
            var newList = RandomList().ToList();
            var tmpLst = new List<int>(TestList);
            tmpLst.AddRange(newList);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            foreach (var num in newList)
                heap.Insert(num);
            var heapList = new List<int>();
            while (!heap.IsEmpty)
                heapList.Add(heap.Extract().Value);
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        public void ExtractMinTest(IHeap<int> heap, bool isMin)
        {
            var tmpLst = new List<int>(TestList);
            Console.WriteLine("isMin:" + isMin);
            var expected = isMin ? tmpLst.Min() : tmpLst.Max();
            var result = heap.Extract().Value;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
            expected = tmpLst.Count - 1;
            result = heap.Count;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }

        public void GetMinTest(IHeap<int> heap, bool isMin)
        {
            var tmpLst = new List<int>(TestList);
            Console.WriteLine("isMin:" + isMin);
            var expected = isMin ? tmpLst.Min() : tmpLst.Max();
            var result = heap.Get().Value;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }

        public void SizeTest(IHeap<int> heap)
        {
            var tmpLst = new List<int>(TestList);
            var expected = tmpLst.Count;
            var result = heap.Count;
            Console.WriteLine("Expect:" + expected + " Result:" + result);
            Assert.AreEqual(expected, result);
        }

        public void DecreaseKeyTest(IHeap<int> heap, bool isMin)
        {
            var num = RandomNumber();
            var tmpLst = new List<int>(TestList) {num};
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var node = heap.Insert(isMin ? num + 1 : num - 1);
            node.Value = num;
            heap.DecreaseKey(node);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        public void DeleteTest(IHeap<int> heap, bool isMin)
        {
            var num = RandomNumber();
            var tmpLst = new List<int>(TestList) {num};
            tmpLst.Remove(num);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var node = heap.Insert(num);
            heap.Delete(node);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        public void EnumerableTest(IHeap<int> heap, bool isMin)
        {
            var num = RandomNumber();
            var tmpLst = new List<int>(TestList) { num };
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            heap.Insert(num);
            CollectionAssert.AreEqual(tmpLst, heap.ToList());
        }

        protected IEnumerable<int> GetHeapList(bool isMin)
        {
            if (isMin)
                while (!MinHeap.IsEmpty)
                    yield return MinHeap.Extract().Value;
            else
                while (!MaxHeap.IsEmpty)
                    yield return MaxHeap.Extract().Value;
        }

        protected IComparer<int> GetComparer(bool isMin)
        {
            return isMin ? _minComparer : _maxComparer;
        }

        [Test]
        public void EnumerableTest()
        {
            EnumerableTest(MinHeap, true);
            EnumerableTest(MaxHeap, false);
        }

        [Test]
        public void DecreaseKeyTest()
        {
            DecreaseKeyTest(MinHeap, true);
            DecreaseKeyTest(MaxHeap, false);
        }

        [Test]
        public void DeleteTest()
        {
            DeleteTest(MinHeap, true);
            DeleteTest(MaxHeap, false);
        }

        [Test]
        public void ExtractMinTest()
        {
            ExtractMinTest(MinHeap, true);
            ExtractMinTest(MaxHeap, false);
        }

        [Test]
        public void GetMinTest()
        {
            GetMinTest(MinHeap, true);
            GetMinTest(MaxHeap, false);
        }

        [Test]
        public void InsertTest()
        {
            InsertTest(MinHeap, true);
            InsertTest(MaxHeap, false);
        }

        [Test]
        public void SizeTest()
        {
            SizeTest(MinHeap);
            SizeTest(MaxHeap);
        }
    }
}