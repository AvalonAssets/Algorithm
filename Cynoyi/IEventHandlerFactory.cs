namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         Wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" />.
    ///     </para>
    /// </summary>
    /// <seealso cref="IEventAggregator" />
    public interface IEventHandlerFactory
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="IEventHandler" /> with <paramref name="subscriber" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <returns>New instance of <see cref="IEventHandler" />.</returns>
        IEventHandler Create(ISubscriber subscriber);
    }
}