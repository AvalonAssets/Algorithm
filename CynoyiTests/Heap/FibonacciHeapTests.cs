using AvalonAssets.Algorithm.Injection;
using AvalonAssets.DataStructure.Heap;
using NUnit.Framework;

namespace AvalonAssetsTests.DataStructure.Heap
{
    [TestFixture]
    public class FibonacciHeapTests : HeapTest
    {
        [OneTimeSetUp]
        public override void Initialize()
        {
            base.Initialize();
            Container.RegisterType<IHeap<int>, FibonacciHeap<int>>();
        }
    }
}