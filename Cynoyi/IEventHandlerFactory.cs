namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     Wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" />.
    /// </summary>
    /// <seealso cref="IEventAggregator" />
    public interface IEventHandlerFactory
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IEventHandler" /> with <paramref name="subscriber" />.
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <returns>New instance of <see cref="IEventHandler" />.</returns>
        IEventHandler Create(ISubscriber subscriber);
    }
}