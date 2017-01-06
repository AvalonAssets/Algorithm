using System;
using System.Collections.Generic;

namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         <see cref="IEventHandler" /> handles the reference between the object and the <see cref="Type" />.
    ///         It is used for <see cref="IEventAggregator" />.
    ///     </para>
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        ///     <para>
        ///         Checks if the object still available.
        ///     </para>
        /// </summary>
        /// <returns>True if object is not GC.</returns>
        bool Alive { get; }

        /// <summary>
        ///     <para>
        ///         Gets All the <see cref="Type" /> that can handle by <see cref="IEventHandler" />.
        ///     </para>
        /// </summary>
        /// <returns>All type the <see cref="IEventHandler" /> that can handle.</returns>
        IEnumerable<Type> Types { get; }

        /// <summary>
        ///     <para>
        ///         Check if <paramref name="instance" /> equals to its reference object.
        ///     </para>
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <returns>True if this handler is wraping <paramref name="instance" />.</returns>
        bool Matches(object instance);

        /// <summary>
        ///     <para>
        ///         Handles <paramref name="message" /> of type <paramref name="messageType" />.
        ///     </para>
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        bool Handle(Type messageType, object message);
    }
}