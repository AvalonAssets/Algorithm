using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly Dictionary<Type, Action<object, object>> _supportedHandlers;
        private readonly WeakReference _weakReference;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        ///     </para>
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        /// <remarks>
        ///     <para>
        ///         It is not recommend to use this directly. You should use <see cref="LambdaEventHandler" /> instead.
        ///     </para>
        /// </remarks>
        public LambdaEventHandler(ISubscriber subscriber)
        {
            _weakReference = new WeakReference(subscriber);
            _supportedHandlers = new Dictionary<Type, Action<object, object>>();
            // Gets all the ISubscriber<T> interface
            var interfaces = subscriber.GetType().GetInterfaces()
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                _supportedHandlers[type] = CreateLambda(type);
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
            var type = Types.FirstOrDefault(t => t.IsAssignableFrom(messageType));
            if (type != null)
                _supportedHandlers[type](_weakReference.Target, message);
            return true;
        }


        private static Action<object, object> CreateLambda(Type type)
        {
            var genericType = typeof(ISubscriber<>).MakeGenericType(type);
            var method = genericType.GetMethod("Receive", BindingFlags.Instance | BindingFlags.Public);
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var messageExpression = Expression.Parameter(typeof(object), "message");
            var instanceCastingExpression = Expression.Convert(instanceExpression, genericType);
            var messageCastingExpression = Expression.Convert(messageExpression, type);
            var invokeExpression = Expression.Call(instanceCastingExpression, method, messageCastingExpression);
            var lambda = Expression.Lambda<Action<object, object>>(invokeExpression, instanceExpression, messageExpression).Compile();
            return lambda;
        }
    }
}