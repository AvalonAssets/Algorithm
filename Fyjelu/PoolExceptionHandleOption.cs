namespace AvalonAssets.Fyjelu
{
    /// <summary>
    ///     Exception handle option used by <see cref="IObjectPool{T}" />.
    /// </summary>
    public enum PoolExceptionHandleOption
    {
        /// <summary>
        ///     All the exception are thrown to caller.
        /// </summary>
        Throw,

        /// <summary>
        ///     All the exception are swallow unless the factory failed to allocate a new instance.
        /// </summary>
        Swallow
    }
}