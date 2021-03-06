﻿using System.Collections.Generic;

namespace AvalonAssets.Core
{
    public static class ComparerExtensions
    {
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return new ComparerWrapper<T>((x, y) => comparer.Compare(y, x));
        }
    }
}