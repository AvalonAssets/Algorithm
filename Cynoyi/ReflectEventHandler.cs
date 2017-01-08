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
    internal class ReflectEventHandler : AbstractEventHandler
    {
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <remarks>
        ///     <para>
        ///         It is not recommend to use this directly. You should use <see cref="ReflectEventHandlerFactory" /> instead.
        ///     </para>
        /// </remarks>
        public ReflectEventHandler(ISubscriber subscriber) : base(subscriber)
        {
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
        ///         Gets All the <see cref="Type" /> that can handle by <see cref="IEventHandler" />.
        ///     </para>
        /// </summary>
        /// <returns>All type the <see cref="IEventHandler" /> that can handle.</returns>
        public override IEnumerable<Type> Types => _supportedHandlers.Keys;

        protected override void HandleMessage(Type handlerType, object target, object message)
        {
            _supportedHandlers[handlerType].Invoke(target, new[] {message});
        }
    }
}