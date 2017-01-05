namespace AvalonAssets.Core.Data.Queue
{
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