using System;
using System.Linq;
using System.Threading;

namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     A array implementation of <see cref="IObjectPool{T}" /> implementation with predefined pool size limit.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Given a <see cref="IObjectFactory{T}" />, this class will maintain a simple pool of instances.
    ///         A finite number of idle instances is enforced, but when the pool is empty, new instances are created to support
    ///         the new load.
    ///     </para>
    ///     <para>
    ///         Hence this class places no limit on the number of active instances created by the pool.
    ///         This pool is thread-safe.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of IObjectPool held in this pool</typeparam>
    public class ArrayObjectPool<T> : AbstractObjectPool<T> where T : class
    {
        private const int DefaultPoolSize = 20;
        private readonly IObjectFactory<T> _factory;
        private readonly T[] _items; // Slow cache
        private T _firstItem; // For quick access

        /// <summary>
        ///     Create a new <see cref="ArrayObjectPool{T}" /> using the specified factory to create new instances.
        /// </summary>
        /// <remarks>
        ///     It limits the maximum number of idle instances to default value.
        /// </remarks>
        /// <param name="factory">The <see cref="IObjectFactory{T}" /> used to populate the pool.</param>
        public ArrayObjectPool(IObjectFactory<T> factory) : this(factory, DefaultPoolSize)
        {
        }

        /// <summary>
        ///     Create a new <see cref="ArrayObjectPool{T}" /> using the specified factory to create new instances.
        /// </summary>
        /// <remarks>
        ///     It limits the maximum number of idle instances to <paramref name="poolSize" />.
        /// </remarks>
        /// <param name="factory">The <see cref="IObjectFactory{T}" /> used to populate the pool.</param>
        /// <param name="poolSize">Maximum size of the pool.</param>
        public ArrayObjectPool(IObjectFactory<T> factory, int poolSize)
            : base(PoolExceptionHandleOption.Swallow)
        {
            // Argument validation
            if (factory == null)
                throw new ArgumentNullException("factory");
            if (poolSize <= 0)
                throw new ArgumentOutOfRangeException("poolSize");
            _factory = factory;
            _items = new T[poolSize - 1];
        }

        /// <summary>
        ///     Allocate an object from the pool.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If there are idle instances available in the pool, the first element is activated, and
        ///         return to the client.
        ///         If there are no idle instances available, <see cref="IObjectFactory{T}.Make" /> is invoked to create a
        ///         new instance.
        ///     </para>
        ///     <para>
        ///         All instances are <see cref="IObjectFactory{T}.Activate" /> before being returned to the client. If the
        ///         instances fail the activation, it will be destoryed.
        ///     </para>
        ///     <para>
        ///         Exceptions throws by <see cref="IObjectFactory{T}.Activate" /> or
        ///         <see cref="IObjectFactory{T}.Destory" /> instances are silently swallowed.
        ///     </para>
        /// </remarks>
        /// <returns>An instance from the pool.</returns>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Make" /> or
        ///     <see cref="IObjectFactory{T}.Make" /> throws an exception.
        /// </exception>
        public override T Allocate()
        {
            var obj = _firstItem;
            // Try to get first item
            if (obj == null || obj != Interlocked.CompareExchange(ref _firstItem, null, obj))
                obj = SlowAllocate(); // Failed to get from first item, use slower method instead.
            else
            {
                // Success to get from first item,
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
                    // Failed to get from first item, use slower method instead.
                    obj = SlowAllocate();
                }
            }
            return obj;
        }

        /// <summary>
        ///     Frees an instance to the pool, put it in the pool after successful deactivation.
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
                    return;
                }
                return;
            }

            // Intentionally not using interlocked here. 
            // In a worst case scenario two objects may be stored into same slot.
            // It is very unlikely to happen and will only mean that one of the objects will get collected.
            if (_firstItem == null)
                _firstItem = obj;
            else
                SlowFree(obj); // Use slower method if first item is occupied.
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
            return _items.Count(s => s != null) + (_firstItem == null ? 0 : 1);
        }

        /// <summary>
        ///     Clears any objects sitting idle in the pool, releasing any associated resources.
        /// </summary>
        /// <remarks>
        ///     Idle objects cleared must be <see cref="IObjectFactory{T}.Destory" />. If new objects is cached during
        ///     clearing, it may not be clear.
        /// </remarks>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Destory" /> throws an exception.
        /// </exception>
        public override void Clear()
        {
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                var obj = items[i];
                if (obj == null) continue;
                // Try to get i item
                if (obj != Interlocked.CompareExchange(ref items[i], null, obj)) continue;
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

        /// <summary>
        ///     Allocate an object from the pool using linear search.
        /// </summary>
        /// <remarks>
        ///     <see cref="SlowFree" /> will try to store recycled objects close to the start thus statistically reducing how far
        ///     will typically be searched.
        /// </remarks>
        /// <returns>An instance from the pool.</returns>
        private T SlowAllocate()
        {
            T obj;
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                obj = items[i];
                if (obj == null) continue;
                // Try to get i item
                if (obj != Interlocked.CompareExchange(ref items[i], null, obj)) continue;
                try
                {
                    // Activate object
                    _factory.Activate(obj);
                    return obj;
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
            }
            // Failed to get cached object, create a new one
            obj = _factory.Make();
            if (obj == null)
                throw new Exception("Factory failed to create a object.");
            return obj;
        }

        /// <summary>
        ///     Frees an instance to the pool, put it in the pool after successful deactivation.
        /// </summary>
        /// <remarks>
        ///     <see cref="SlowAllocate" /> will try to get recycled objects close to the start thus statistically reducing how far
        ///     will typically be searched.
        /// </remarks>
        /// <param name="obj"></param>
        private void SlowFree(T obj)
        {
            var items = _items;
            for (var i = 0; i < items.Length; i++)
            {
                // Intentionally not using interlocked here. 
                // In a worst case scenario two objects may be stored into same slot.
                // It is very unlikely to happen and will only mean that one of the objects will get collected.
                if (items[i] != null) continue;
                items[i] = obj;
                break;
            }
        }
    }
}