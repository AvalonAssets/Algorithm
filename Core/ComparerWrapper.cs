using System;
using System.Collections.Generic;

namespace AvalonAssets.Core
{
    public class ComparerWrapper<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _compareFunc;

        public ComparerWrapper(Func<T, T, int> compareFunc)
        {
            _compareFunc = compareFunc;
        }

        public int Compare(T x, T y)
        {
            return _compareFunc(x, y);
        }
    }
}