using AvalonAssets.Core.Data.Heap;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Heap
{
    [TestFixture]
    public class FibonacciHeapTests : HeapTest
    {
        public override IHeap<int> CreateHeap(bool isMin)
        {
            return new FibonacciHeap<int>(GetComparer(isMin));
        }
    }
}