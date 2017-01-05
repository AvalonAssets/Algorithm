namespace AvalonAssets.Core.Data.Heap
{
    public interface IHeap<T>
    {
        /// <summary>
        ///     Returns if the heap is empty.
        /// </summary>
        /// <returns>Heap is empty or not.</returns>
        bool IsEmpty { get; }

        /// <summary>
        ///     Returns number of values inside the heap.
        ///     Use <see cref="IsEmpty" /> instead if you want to check for empty.
        /// </summary>
        /// <returns>Size of the heap.</returns>
        int Size { get; }

        /// <summary>
        ///     Inserts a new value.
        /// </summary>
        /// <param name="element">Value to be inserted.</param>
        /// <returns>Node representation of the value.</returns>
        IHeapNode<T> Insert(T element);

        /// <summary>
        ///     Returns and removes the minimum value. Minimum value means root value.
        ///     If it is a max heap, it return the largest value.
        /// </summary>
        /// <returns> Minimum value.</returns>
        IHeapNode<T> ExtractMin();

        /// <summary>
        ///     Returns the minimum value. Minimum value means root value.
        ///     If it is a max heap, it return the largest value.
        /// </summary>
        IHeapNode<T> GetMin();

        /// <summary>
        ///     Decreases an existing key to some value.
        ///     Decreases key means decrease the order in the heap.
        ///     If it is a max heap, increase value instead.
        ///     Use in order way will result in unexpected effects.
        /// </summary>
        void DecreaseKey(IHeapNode<T> element);

        /// <summary>
        ///     Removes the given node from the heap.
        /// </summary>
        /// <param name="element">Node to be removed.</param>
        void Delete(IHeapNode<T> element);
    }
}