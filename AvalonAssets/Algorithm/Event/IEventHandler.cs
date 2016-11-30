﻿using System;

namespace AvalonAssets.Algorithm.Event
{
    public interface IEventHandler
    {
        /// <summary>
        ///     Returns true if object is not GC.
        /// </summary>
        bool Alive { get; }

        /// <summary>
        ///     Returns true if this handler is wraping <paramref name="instance" />.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <returns>True if this handler is wraping <paramref name="instance" />.</returns>
        bool Matches(object instance);

        /// <summary>
        ///     Handles <paramref name="message" /> of type <paramref name="messageType" />.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        bool Handle(Type messageType, object message);

        /// <summary>
        ///     Returns true if it can handle <paramref name="messageType" />.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <returns>True if it can handle <paramref name="messageType" />.</returns>
        bool CanHandle(Type messageType);
    }
}