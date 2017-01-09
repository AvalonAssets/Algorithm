using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Core.Data.Heap;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Heap
{
    [TestFixture]
    public class BinomialHeapTests : HeapTest
    {
        public override IHeap<int> CreateHeap(bool isMin)
        {
            return new BinomialHeap<int>(GetComparer(isMin));
        }

        public void MergeTest(IHeap<int> heap, bool isMin)
        {
            var newList = RandomList().ToList();
            var tmpLst = new List<int>(TestList);
            tmpLst.AddRange(newList);
            tmpLst.Sort();
            if (!isMin)
                tmpLst.Reverse();
            var newHeap = CreateHeap(isMin) as BinomialHeap<int>;
            Assert.NotNull(newHeap);
            foreach (var num in newList)
                newHeap.Insert(num);
            var binaryHeap = heap as BinomialHeap<int>;
            Assert.IsNotNull(binaryHeap);
            binaryHeap.Merge(newHeap);
            var heapList = GetHeapList(isMin).ToList();
            Console.WriteLine("isMin:" + isMin);
            Console.WriteLine("Expect:" + string.Join(", ", tmpLst));
            Console.WriteLine("Result:" + string.Join(", ", heapList));
            Assert.IsTrue(tmpLst.SequenceEqual(heapList));
        }

        [Test]
        public void MergeTest()
        {
            MergeTest(MinHeap, true);
            MergeTest(MaxHeap, false);
        }
    }
}