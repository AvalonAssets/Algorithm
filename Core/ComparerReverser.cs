using System.Collections.Generic;

namespace AvalonAssets.Core
{
    public class ComparerReverser<T> : IComparer<T>
    {
        private readonly IComparer<T> _comparer;

        public ComparerReverser(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return _comparer.Compare(y, x);
        }
    }
}