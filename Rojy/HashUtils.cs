namespace AvalonAssets.Rojy
{
    internal static class HashUtils
    {
        // Reference: http://stackoverflow.com/a/13871379/3673259
        //            http://szudzik.com/ElegantPairing.pdf
        public static int IntegerHash(int x, int y)
        {
            unchecked
            {
                var a = (ulong) (x >= 0 ? 2*(long) x : -2*(long) x - 1);
                var b = (ulong) (y >= 0 ? 2*(long) y : -2*(long) y - 1);
                var c = (long) ((a >= b ? a*a + a + b : a + b*b)/2);
                return (int) (x < 0 && y < 0 || x >= 0 && y >= 0 ? c : -c - 1);
            }
        }
    }
}