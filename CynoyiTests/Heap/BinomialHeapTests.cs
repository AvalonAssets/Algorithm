using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm.Injection;
using AvalonAssets.DataStructure.Heap;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Heap
{
    [TestFixture]
    public class BinomialHeapTests : HeapTest
    {
        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<IHeap<int>, BinomialHeap<int>>();
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