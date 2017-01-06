using System;
using System.Linq;

namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         <see cref="EventHandlerExtensions" /> provides extension methods for <see cref="IEventHandler" />.
    ///     </para>
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        ///     <para>
        ///         Handles <paramref name="message" /> of type <typeparamref name="T" />.
        ///     </para>
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        /// <seealso cref="IEventHandler.Handle" />
        public static bool Handle<T>(this IEventHandler handler, T message)
        {
            return handler.Handle(typeof(T), message);
        }

        /// <summary>
        ///     <para>
        ///         Checks if <paramref name="handler" /> can handle message of <typeparamref name="T" />.
        ///     </para>
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <returns>True if it can handle <typeparamref name="T" />.</returns>
        /// <seealso cref="CanHandle" />
        public static bool CanHandle<T>(this IEventHandler handler)
        {
            return handler.CanHandle(typeof(T));
        }

        /// <summary>
        ///     <para>
        ///         Checks if <paramref name="handler" /> can handle message of <paramref name="messageType" />.
        ///     </para>
        /// </summary>
        /// <param name="handler">Handler.</param>
        /// <param name="messageType">Message type.</param>
        /// <returns>True if it can handle <paramref name="messageType" />.</returns>
        /// <seealso cref="CanHandle{T}" />
        public static bool CanHandle(this IEventHandler handler, Type messageType)
        {
            return handler.Types.Contains(messageType);
        }
    }
}