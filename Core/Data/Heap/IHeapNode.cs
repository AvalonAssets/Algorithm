namespace AvalonAssets.Core.Data.Heap
{
    /// <summary>
    ///     Represents a node in heap.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IHeapNode<T>
    {
        T Value { get; set; }
    }
}