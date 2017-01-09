using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AvalonAssets.Cynoyi
{
    /// <summary>
    ///     Implementation of <see cref="IEventHandler" />. Uses weak reference to hold the reference to subscriber.
    /// </summary>
    internal class LambdaEventHandler : AbstractEventHandler
    {
        private readonly Dictionary<Type, Action<object, object>> _supportedHandlers;

        /// <summary>
        ///     Initializes a new instance of <see cref="EventAggregator" /> with <see cref="IEventHandlerFactory" />.
        /// </summary>
        /// <param name="subscriber">Object that want to subscribe.</param>
        public LambdaEventHandler(ISubscriber subscriber) : base(subscriber)
        {
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
        ///     Gets All the <see cref="Type" /> that can handle by <see cref="IEventHandler" />.
        /// </summary>
        /// <returns>All type the <see cref="IEventHandler" /> that can handle.</returns>
        public override IEnumerable<Type> Types => _supportedHandlers.Keys;

        protected override void HandleMessage(Type handlerType, object target, object message)
        {
            _supportedHandlers[handlerType](target, message);
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
            var lambda =
                Expression.Lambda<Action<object, object>>(invokeExpression, instanceExpression, messageExpression)
                    .Compile();
            return lambda;
        }
    }
}