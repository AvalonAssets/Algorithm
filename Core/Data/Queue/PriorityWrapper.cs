namespace AvalonAssets.Core.Data.Queue
{
    /// <summary>
    ///     Simple implementation of <see cref="IPriority{T}" />
    /// </summary>
    /// <typeparam name="T">Type of the Value.</typeparam>
    public class PriorityWrapper<T> : IPriority<T>
    {
        /// <summary>
        ///     Create a new <see cref="PriorityWrapper{T}" />.
        /// </summary>
        /// <param name="priority">Priority of the Value. The lower value, the higher priority.</param>
        /// <param name="value">Value.</param>
        public PriorityWrapper(int priority, T value)
        {
            Priority = priority;
            Value = value;
        }

        /// <summary>
        ///     Priority of the Value. The lower value, the higher priority.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        ///     Value.
        /// </summary>
        public T Value { get; }
    }
}