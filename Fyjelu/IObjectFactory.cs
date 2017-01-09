using System;

namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     An interface defining life-cycle methods for instances to be served by an <see cref="IObjectPool{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of objects held in this pool.</typeparam>
    /// <seealso cref="IObjectPool{T}" />
    public interface IObjectFactory<T> where T : class
    {
        /// <summary>
        ///     Activate an instance to be returned by the <see cref="IObjectPool{T}" />.
        /// </summary>
        /// <param name="obj">The instance to be activated.</param>
        /// <exception cref="Exception">
        ///     If there is a problem activating <paramref name="obj" />, this exception may be
        ///     swallowed by the pool.
        /// </exception>
        void Activate(T obj);

        /// <summary>
        ///     Deactivate an instance to be returned to the idle <see cref="IObjectPool{T}" />.
        /// </summary>
        /// <param name="obj">The instance to be deactivated.</param>
        /// <exception cref="Exception">
        ///     If there is a problem deactivating <paramref name="obj" />, this exception may be
        ///     swallowed by the pool.
        /// </exception>
        void Deactivate(T obj);

        /// <summary>
        ///     Destroys an instance no longer needed by the <see cref="IObjectPool{T}" />.
        /// </summary>
        /// <param name="obj">The instance to be destroyed.</param>
        /// <exception cref="Exception">It should be avoided as it may be swallowed by the pool implementation.</exception>
        void Destory(T obj);

        /// <summary>
        ///     Creates an instance that can be served by the <see cref="IObjectPool{T}" />.
        /// </summary>
        /// <remarks>
        ///     Instances returned from this method should be in the same state as if they had been <see cref="Activate" />.
        ///     They will not be activated before being served by the <see cref="IObjectPool{T}" />.
        /// </remarks>
        /// <returns>An instance that can be served by the <see cref="IObjectPool{T}" />.</returns>
        /// <exception cref="Exception">
        ///     If there is a problem creating a new instance, this will be propagated to the code
        ///     requesting an object.
        /// </exception>
        T Make();
    }
}