using System.Collections.Generic;

namespace AvalonAssets.Core.Data.Queue
{
    /// <summary>
    ///     Default <see cref="IComparer{T}" /> used by <see cref="PriorityQueue{T}" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityComparer<T> : IComparer<IPriority<T>>
    {
        public int Compare(IPriority<T> x, IPriority<T> y)
        {
            return x.Priority.CompareTo(y.Priority);
        }
    }
}