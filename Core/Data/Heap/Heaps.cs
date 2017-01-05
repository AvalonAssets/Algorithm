using System.Collections.Generic;

namespace AvalonAssets.Core.Data.Heap
{
    public static class Heaps
    {
        public static IHeap<T> Default<T>(IComparer<T> comparer)
        {
            return new BinaryHeap<T>(comparer);
        }
    }
}