namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     Marker interface.
    /// </summary>
    /// <remarks>
    ///     Do not use this. Use <see cref="ISubscriber{T}" /> instead.
    /// </remarks>
    public interface ISubscriber
    {
    }

    
    /// <summary>
    ///     Subscribes to message type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <seealso cref="IEventAggregator" />
    /// <seealso cref="IEventHandlerFactory" />
    public interface ISubscriber<in T> : ISubscriber
    {
        /// <summary>
        ///     Receives publish messages from <see cref="IEventAggregator" />.
        /// </summary>
        /// <param name="message">Message received.</param>
        void Receive(T message);
    }
}