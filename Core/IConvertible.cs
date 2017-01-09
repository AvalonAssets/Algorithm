namespace AvalonAssets.Core
{
    /// <summary>
    ///     Represents a object that can convert to <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Convertible type.</typeparam>
    public interface IConvertible<out T>
    {
        T ConvertTo();
    }
}