using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Core.Data.Heap;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Heap
{
    [TestFixture]
    public class BinaryHeapTests : HeapTest
    {
        public override IHeap<int> CreateHeap(bool isMin)
        {
            return new BinaryHeap<int>(GetComparer(isMin));
        }

        public void MergeTest(IHeap<int> heap, bool isMin)
        {
            var newList = RandomList().ToList();
            var tmpLst = new List<int>(TestList);
            tmpLst.AddRange(newList);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var newHeap = CreateHeap(isMin) as BinaryHeap<int>;
            Assert.NotNull(newHeap);
            foreach (var num in newList)
                newHeap.Insert(num);
            var binaryHeap = heap as BinaryHeap<int>;
            Assert.IsNotNull(binaryHeap);
            binaryHeap.Merge(newHeap);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        public void BuildTest(IHeap<int> heap, bool isMin)
        {
            var tmpLst = new List<int>(TestList);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var newHeap = new BinaryHeap<int>(GetComparer(isMin), TestList);
            var heapList = new List<int>();
            while (!newHeap.IsEmpty)
                heapList.Add(newHeap.Extract().Value);
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        [Test]
        public void BuildTest()
        {
            BuildTest(MinHeap, true);
            BuildTest(MaxHeap, false);
        }

        [Test]
        public void MergeTest()
        {
            MergeTest(MinHeap, true);
            MergeTest(MaxHeap, false);
        }
    }
}