using System;

namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     A pooling interface.
    /// </summary>
    /// <remarks>
    ///     <see cref="IObjectPool{T}" /> defines a trivially simple pooling interface.
    /// </remarks>
    /// <typeparam name="T">The type of objects held in this pool.</typeparam>
    /// <seealso cref="IObjectFactory{T}" />
    public interface IObjectPool<T> where T : class
    {
        /// <summary>
        ///     Obtains an instance from the pool.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Instances returned from this method will have been either newly created with
        ///         <see cref="IObjectFactory{T}.Make" />
        ///         or will be a previously idle object and have been activated with
        ///         <see cref="IObjectFactory{T}.Activate" />.
        ///     </para>
        ///     <para>
        ///         By contract, clients <b>must</b> return the allocated instance using <see cref="Free" />, or a related method
        ///         as defined in an implementation or sub-interface.
        ///     </para>
        ///     <para>
        ///         The behaviour of this method when the pool has been exhausted is not strictly specified (although it may be
        ///         specified by implementations).
        ///         It is encouraged to throw a <see cref="InvalidOperationException" />.
        ///     </para>
        /// </remarks>
        /// <returns>An instance from this pool.</returns>
        /// <exception cref="InvalidOperationException">
        ///     When the pool is exhausted or will not return another instance.
        /// </exception>
        /// <exception cref="Exception">When <see cref="IObjectFactory{T}.Make" /> throws an exception.</exception>
        T Allocate();

        /// <summary>
        ///     Frees an instance to the pool.
        /// </summary>
        /// <remarks>
        ///     By contract, <paramref name="obj" /> <b>must</b> have been obtained using <see cref="Allocate" /> or a
        ///     related method as defined in an implementation or sub-interface.
        /// </remarks>
        /// <param name="obj">A <see cref="Allocate" /> instance to be disposed.</param>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Deactivate" /> or
        ///     <see cref="IObjectFactory{T}.Destory" /> throws an exception.
        /// </exception>
        void Free(T obj);

        /// <summary>
        ///     Set the pool how to handle the exception.
        /// </summary>
        /// <param name="option">How to handle exception.</param>
        void SetExceptionHandleOption(PoolExceptionHandleOption option);

        /// <summary>
        ///     Return the number of instances currently in this pool. Returns a negative value if this information is not
        ///     available.
        /// </summary>
        /// <remarks>
        ///     This may be considered an approximation of the number of objects that can be borrowed without creating any new
        ///     instances.
        /// </remarks>
        /// <returns>The number of instances currently in this pool or a negative value if unsupported.</returns>
        int GetCacheSize();

        /// <summary>
        ///     Clears any objects in the pool, releasing any associated resources.
        /// </summary>
        /// <remarks>Idle objects cleared must be <see cref="IObjectFactory{T}.Destory" />.</remarks>
        /// <exception cref="Exception">
        ///     When <see cref="IObjectFactory{T}.Destory" /> throws an exception.
        /// </exception>
        void Clear();
    }
}