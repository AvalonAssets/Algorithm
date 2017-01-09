using System;

namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     A base implementation of <see cref="IObjectFactory{T}" />.
    ///     All operations defined here are essentially no operation.
    /// </summary>
    /// <typeparam name="T">The type of objects held in this pool.</typeparam>
    /// <seealso cref="IObjectPool{T}" />
    public abstract class AbstractObjectFactory<T> : IObjectFactory<T> where T : class
    {
        /// <summary>
        ///     Not supported in this base implementation.
        /// </summary>
        /// <param name="obj">Ignored.</param>
        /// <exception cref="Exception">
        ///     If there is a problem activating <paramref name="obj" />, this exception may be swallowed
        ///     by the pool.
        /// </exception>
        public virtual void Activate(T obj)
        {
        }

        /// <summary>
        ///     Not supported in this base implementation.
        /// </summary>
        /// <param name="obj">Ignored.</param>
        /// <exception cref="Exception">
        ///     If there is a problem deactivating <paramref name="obj" />, this exception may be swallowed
        ///     by the pool.
        /// </exception>
        public virtual void Deactivate(T obj)
        {
        }

        /// <summary>
        ///     Not supported in this base implementation.
        /// </summary>
        /// <param name="obj">Ignored.</param>
        /// <exception cref="Exception">It should be avoided as it may be swallowed by the pool implementation.</exception>
        public virtual void Destory(T obj)
        {
        }

        /// <summary>
        ///     Creates an instance that can be served by the <see cref="IObjectPool{T}" />.
        /// </summary>
        /// <seealso cref="IObjectFactory{T}.Make" />
        /// <returns>An instance that can be served by the <see cref="IObjectPool{T}" />.</returns>
        /// <exception cref="Exception">
        ///     If there is a problem creating a new instance, this will be propagated to the code
        ///     requesting an object.
        /// </exception>
        public abstract T Make();
    }
}