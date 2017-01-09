namespace AvalonAssets.Core
{
    public interface IConvertible<out T>
    {
        T ConvertTo();
    }
}