using System.Collections.Generic;

namespace AvalonAssets.Core.Data.Queue
{
    /// <summary>
    ///     <para>
    ///         Default <see cref="IComparer{T}" /> used by <see cref="PriorityQueue{T}" />.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public class PriorityComparer<T> : IComparer<IPriority<T>>
    {
        public int Compare(IPriority<T> x, IPriority<T> y)
        {
            return x.Priority.CompareTo(y.Priority);
        }
    }
}