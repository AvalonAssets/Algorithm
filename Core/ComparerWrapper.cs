using System;
using System.Collections.Generic;

namespace AvalonAssets.Core
{
    public class ComparerWrapper<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparerWrapper(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
    }
}