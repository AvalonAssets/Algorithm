namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         Wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" /> with weak reference.
    ///     </para>
    /// </summary>
    public class LambdaEventHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="IEventHandler" /> with <paramref name="subscriber" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <returns>New instance of <see cref="IEventHandler" />.</returns>
        public IEventHandler Create(ISubscriber subscriber)
        {
            return new LambdaEventHandler(subscriber);
        }
    }
}