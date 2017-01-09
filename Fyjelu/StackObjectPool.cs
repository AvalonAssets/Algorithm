using System;
using System.Collections.Generic;

namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     A simple, Stack-based <see cref="IObjectPool{T}" /> implementation.
    /// </summary>
    /// <remarks>
    ///     Given a <see cref="IObjectFactory{T}" />, this class will maintain a simple pool of instances.
    ///     A finite number of idle instances is enforced, but when the pool is empty, new instances are created to support the
    ///     new load.
    ///     Hence this class places no limit on the number of active instances created by the pool.
    /// </remarks>
    /// <typeparam name="T">The type of objects held in this pool.</typeparam>
    public class StackObjectPool<T> : AbstractObjectPool<T> where T : class
    {
        private const int DefaultPoolSize = 4;
        private const int DefaultMaxPoolSize = 20;
        private readonly IObjectFactory<T> _factory;
        private readonly int _maxPoolSize;
        private readonly Stack<T> _pool;

        /// <summary>
        ///     Create a new <see cref="StackObjectPool{T}" /> using the specified factory to create new instances.
        /// </summary>
        /// <remarks>
        ///     It limits the maximum number of idle instances to default value.
        /// </remarks>
        /// <param name="factory">The <see cref="IObjectFactory{T}" /> used to populate the pool.</param>
        public StackObjectPool(IObjectFactory<T> factory)
            : this(factory, DefaultMaxPoolSize, DefaultPoolSize)
        {
        }

        /// <summary>
        ///     Create a new <see cref="StackObjectPool{T}" /> using the specified factory to create new instances.
        /// </summary>
        /// <remarks>
        ///     It limits the maximum number of idle instances to <paramref name="maxPoolSize" />.
        /// </remarks>
        /// <param name="factory">The <see cref="IObjectFactory{T}" /> used to populate the pool.</param>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        public StackObjectPool(IObjectFactory<T> factory, int maxPoolSize)
            : this(factory, maxPoolSize, DefaultPoolSize)
        {
        }

        /// <summary>
        ///     Create a new <see cref="StackObjectPool{T}" /> using the specified factory to create new instances.
        /// </summary>
        /// <remarks>
        ///     It limits the maximum number of idle instances to <paramref name="maxPoolSize" />,
        ///     and initially allocating a container capable of containing at least <paramref name="initialPoolSize" /> instances.
        ///     The pool is not pre-populated. The <paramref name="initialPoolSize" /> parameter just determines the initial size
        ///     of the underlying container,
        ///     which can increase beyond this value if <paramref name="maxPoolSize" /> > <paramref name="initialPoolSize" />.
        /// </remarks>
        /// <param name="factory">The <see cref="IObjectFactory{T}" /> used to populate the pool.</param>
        /// <param name="maxPoolSize">Maximum size of the pool.</param>
        /// <param name="initialPoolSize">Initial size of the pool.</param>
        public StackObjectPool(IObjectFactory<T> factory, int maxPoolSize, int initialPoolSize)
            : base(PoolExceptionHandleOption.Swallow)
        {
            // Argument validation
            if (factory == null)
                throw new ArgumentNullException("factory");
            if (maxPoolSize <= 0)
                throw new ArgumentOutOfRangeException("maxPoolSize");
            if (initialPoolSize <= 0 || initialPoolSize > maxPoolSize)
                throw new ArgumentOutOfRangeException("initialPoolSize");
            _factory = factory;
            _pool = new Stack<T>(initialPoolSize);
            _maxPoolSize = maxPoolSize;
        }

        /// <summary>
        ///     Allocate an object from the pool.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If there are idle instances available on the stack, the top element of the stack is popped to activate, and
        ///         return to the client.
        ///         If there are no idle instances available, <see cref="IObjectFactory{T}.Make" /> is invoked to create a
        ///         new instance.
        ///     </para>
        ///     <para>
        ///         All instances are <see cref="IObjectFactory{T}.Activate" /> before being returned to the client.
        ///     </para>
        /// </remarks>
        /// <returns>An instance from the pool.</returns>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Make" /> or
        ///     <see cref="IObjectFactory{T}.Make" /> throws an exception.
        /// </exception>
        public override T Allocate()
        {
            T obj = null;
            while (obj == null)
            {
                // Try to get cached object from stack
                if (_pool.Count > 0)
                {
                    obj = _pool.Pop();
                    try
                    {
                        // Activate object
                        _factory.Activate(obj);
                    }
                    catch (Exception activateException)
                    {
                        // Failed activation
                        // Check the exception rethrow setting
                        CheckExceptionRethrow(activateException);
                        try
                        {
                            // Destroy object
                            _factory.Destory(obj);
                        }
                        catch (Exception destoryException)
                        {
                            // Failed to destroy
                            // Check the exception rethrow setting
                            CheckExceptionRethrow(destoryException);
                        }
                    }
                    finally
                    {
                        obj = null;
                    }
                }
                else
                {
                    // Failed to get cached object, create a new one
                    obj = _factory.Make();
                    if (obj == null)
                        throw new Exception("Factory failed to create a object.");
                }
            }
            return obj;
        }

        /// <summary>
        ///     Frees an instance to the pool, pushing it on top of the idle instance stack after successful deactivation.
        /// </summary>
        /// <param name="obj">A <see cref="Allocate" /> instance to be disposed.</param>
        /// <remarks>
        ///     <para>
        ///         The returning instance is destroyed if deactivation throws an exception, or the stack is already full.
        ///     </para>
        ///     <para>
        ///         Exceptions throws by <see cref="IObjectFactory{T}.Deactivate" /> or
        ///         <see cref="IObjectFactory{T}.Destory" /> instances are silently swallowed.
        ///     </para>
        /// </remarks>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Deactivate" /> or
        ///     <see cref="IObjectFactory{T}.Destory" /> throws an exception.
        /// </exception>
        public override void Free(T obj)
        {
            var shouldDestroy = _pool.Count >= _maxPoolSize; // Check if the pool is full.
            try
            {
                // Deactivate object
                _factory.Deactivate(obj);
            }
            catch (Exception deactivateException)
            {
                // Failed deactivation
                // Check the exception rethrow setting
                CheckExceptionRethrow(deactivateException);
                shouldDestroy = true; // Destory object that fail to deactivate.
            }
            if (shouldDestroy)
                try
                {
                    // Destroy object
                    _factory.Destory(obj);
                }
                catch (Exception destoryException)
                {
                    // Failed to destroy
                    // Check the exception rethrow setting
                    CheckExceptionRethrow(destoryException);
                }
            else
                _pool.Push(obj); // Put back to stack
        }

        /// <summary>
        ///     Return the number of instances currently in this pool.
        /// </summary>
        /// <remarks>
        ///     This may be considered an approximation of the number of objects that can be borrowed without creating any new
        ///     instances.
        /// </remarks>
        /// <returns>The number of instances currently in this pool.</returns>
        public override int GetCacheSize()
        {
            return _pool.Count;
        }

        /// <summary>
        ///     Clears any objects sitting idle in the pool, releasing any associated resources.
        /// </summary>
        /// <remarks>Idle objects cleared must be <see cref="IObjectFactory{T}.Destory" />.</remarks>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Destory" /> throws an exception.
        /// </exception>
        public override void Clear()
        {
            while (_pool.Count > 0)
            {
                var obj = _pool.Pop();
                try
                {
                    // Destroy object
                    _factory.Destory(obj);
                }
                catch (Exception destoryException)
                {
                    // Failed to destroy
                    // Check the exception rethrow setting
                    CheckExceptionRethrow(destoryException);
                }
            }
        }
    }
}