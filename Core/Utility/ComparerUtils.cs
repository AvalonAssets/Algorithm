using System.Collections.Generic;

namespace AvalonAssets.Core.Utility
{
    public static class ComparerUtils
    {
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return new ComparerReverser<T>(comparer);
        }
    }
}