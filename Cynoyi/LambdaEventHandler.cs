using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     <para>
    ///         Implementation of <see cref="IEventHandler" />. Uses weak reference to hold the reference to subscriber.
    ///     </para>
    /// </summary>
    internal class LambdaEventHandler : IEventHandler
    {
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers;
        private readonly WeakReference _weakReference;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <remarks>
        ///     <para>
        ///         It is not recommend to use this directly. You should use <see cref="WeakEventHandlerFactory" /> instead.
        ///     </para>
        /// </remarks>
        public LambdaEventHandler(ISubscriber subscriber)
        {
            _weakReference = new WeakReference(subscriber);
            _supportedHandlers = new Dictionary<Type, MethodInfo>();
            // Gets all the ISubscriber<T> interface
            var interfaces = subscriber.GetType().GetInterfaces()
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                var method = @interface.GetMethod("Receive");
                _supportedHandlers[type] = method;
            }
        }

        /// <summary>
        ///     <para>
        ///         Checks if the object still available.
        ///     </para>
        /// </summary>
        /// <returns>True if object is not GC.</returns>
        public bool Alive => _weakReference.Target != null;

        /// <summary>
        ///     <para>
        ///         Gets All the <see cref="Type" /> that can handle by <see cref="IEventHandler" />.
        ///     </para>
        /// </summary>
        /// <returns>All type the <see cref="IEventHandler" /> that can handle.</returns>
        public IEnumerable<Type> Types => _supportedHandlers.Keys;

        /// <summary>
        ///     <para>
        ///         Check if <paramref name="instance" /> equals to its reference object.
        ///     </para>
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <returns>True if this handler is wraping <paramref name="instance" />.</returns>
        public bool Matches(object instance)
        {
            return _weakReference.Target == instance;
        }

        /// <summary>
        ///     <para>
        ///         Handles <paramref name="message" /> of type <paramref name="messageType" />.
        ///     </para>
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        public bool Handle(Type messageType, object message)
        {
            if (!Alive)
                return false;
            foreach (var pair in _supportedHandlers)
            {
                // Continue if it is not the type or its sub-type
                if (!pair.Key.IsAssignableFrom(messageType)) continue;
                // Returns result ignored.
                pair.Value.Invoke(_weakReference.Target, new[] {message});
            }
            return true;
        }

        private class LambdaWrapper
        {
            public LambdaWrapper(Type type, ISubscriber subscriber)
            {
                
            }
        }
    }
}