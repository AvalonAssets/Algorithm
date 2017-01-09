namespace AvalonAssets.Core.Data.Queue
{
    /// <summary>
    ///     Represents an oject with priority.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IPriority<out T>
    {
        /// <summary>
        ///     Priority of the object. The lower value, the higher priority.
        /// </summary>
        int Priority { get; }

        /// <summary>
        ///     Value.
        /// </summary>
        T Value { get; }
    }
}