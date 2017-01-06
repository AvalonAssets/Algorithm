namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         Marker interface.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     <para>Do not use this. Use <see cref="ISubscriber{T}" /> instead.</para>
    /// </remarks>
    public interface ISubscriber
    {
    }

    /// <summary>
    ///     <para>
    ///         Subscribes to message type <typeparamref name="T" />.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <seealso cref="IEventAggregator" />
    /// <seealso cref="IEventHandlerFactory" />
    public interface ISubscriber<in T> : ISubscriber
    {
        /// <summary>
        ///     <para>
        ///         Receives publish messages from <see cref="IEventAggregator" />.
        ///     </para>
        /// </summary>
        /// <param name="message">Message received.</param>
        void Receive(T message);
    }
}