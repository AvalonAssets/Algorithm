using System;
using System.Collections.Generic;

namespace AvalonAssets.Cynoyi
{
    internal abstract class AbstractEventHandler : IEventHandler
    {
        private readonly WeakReference _weakReference;

        protected AbstractEventHandler(ISubscriber subscriber)
        {
            _weakReference = new WeakReference(subscriber);
        }

        public bool Alive => _weakReference.Target != null;
        public abstract IEnumerable<Type> Types { get; }

        /// <summary>
        ///     Check if <paramref name="instance" /> equals to its reference object.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <returns>True if this handler is wraping <paramref name="instance" />.</returns>
        public bool Matches(object instance)
        {
            return _weakReference.Target == instance;
        }

        /// <summary>
        ///     Handles <paramref name="message" /> of type <paramref name="messageType" />.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        public bool Handle(Type messageType, object message)
        {
            if (!Alive)
                return false;
            foreach (var type in Types)
            {
                if (type.IsAssignableFrom(messageType))
                    HandleMessage(type, _weakReference.Target, message);
            }
            return true;
        }

        protected abstract void HandleMessage(Type handlerType, object target, object message);
    }
}