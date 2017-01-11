using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         Implementation of <see cref="IEventAggregator" />.
    ///     </para>
    /// </summary>
    internal class EventAggregator : IEventAggregator
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;
        // Registered event handlers
        private readonly ConcurrentDictionary<Type, HashSet<IEventHandler>> _eventHandlers;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        ///     </para>
        /// </summary>
        /// <param name="eventHandlerFactory">Initializes new <see cref="IEventHandler" /> for <see cref="EventAggregator" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="eventHandlerFactory" />is null.</exception>
        public EventAggregator(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _eventHandlers = new ConcurrentDictionary<Type, HashSet<IEventHandler>>();
        }

        /// <summary>
        ///     <para>
        ///         <paramref name="subscriber" /> subscribes to <see cref="ISubscriber{T}" /> that it implemented.
        ///         For example, if it implemented <see cref="ISubscriber{T}" /> of <see cref="string" />.
        ///         It will receives any published messages that is <see cref="string" /> or its subclass.
        ///     </para>
        ///     <para>
        ///         If <paramref name="subscriber" /> does not implement <see cref="ISubscriber{T}" />  or
        ///         it has already subscribe will be ignored.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that implements <see cref="ISubscriber{T}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" />is null.</exception>
        public void Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            lock (_eventHandlers)
            {
                var handler = _eventHandlerFactory.Create(subscriber);
                // Registers if there is atleast one type
                if (!handler.Types.Any())
                    return;
                var checkAdded = false;
                foreach (var type in handler.Types)
                {
                    if (!_eventHandlers.ContainsKey(type))
                        _eventHandlers.AddOrUpdate(type, new HashSet<IEventHandler>(), (key, value) => value);
                    var typeSet = _eventHandlers[type];
                    // Does not allow double subscribe
                    if (!checkAdded && typeSet.Any(h => h.Matches(subscriber)))
                        throw new ArgumentException($"{subscriber} subscribed twice.");
                    checkAdded = true;
                    typeSet.Add(handler);
                }
            }
        }

        /// <summary>
        ///     <para>
        ///         <paramref name="subscriber" /> unsubscribes from all <see cref="ISubscriber{T}" />.
        ///         If <paramref name="subscriber" /> does not subscribe, it will be ignored.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that already subscribes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" />is null.</exception>
        public void Unsubscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            var handler = _eventHandlerFactory.Create(subscriber);
            if (!handler.Types.Any())
                return;
            foreach (var type in handler.Types)
            {
                if (!_eventHandlers.ContainsKey(type))
                    continue;
                var typeSet = _eventHandlers[type];
                typeSet.RemoveWhere(h => h.Matches(subscriber));
            }
        }

        /// <summary>
        ///     <para>
        ///         Publishs a <paramref name="message" /> to all the registered <see cref="ISubscriber{T}" /> of
        ///         <typeparamref name="T" /> or its super class.
        ///     </para>
        ///     <para>
        ///         The receive order is not guarantee.
        ///     </para>
        /// </summary>
        /// <param name="message">Message to be published.</param>
        public void Publish<T>(T message)
        {
            var messageType = typeof(T);
            IEventHandler[] toNotify;
            if (!_eventHandlers.ContainsKey(messageType))
                return;
            toNotify = _eventHandlers[messageType].Where(h => h.CanHandle(messageType)).ToArray();
            // Publishes message
            var dead = toNotify.Where(h => !h.Handle(messageType, message)).ToList();
            if (!dead.Any()) return;
            // Clean up
            foreach (var handler in dead)
            foreach (var type in handler.Types)
                _eventHandlers[type].Remove(handler);
        }
    }
}