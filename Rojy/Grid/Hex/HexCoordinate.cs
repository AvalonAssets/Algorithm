using System;

namespace AvalonAssets.Rojy.Grid.Hex
{
    internal class HexCoordinate : IHexCoordinate
    {
        internal HexCoordinate(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new ArgumentException("Invalid coordinates.");
            Q = x;
            R = z;
        }

        internal HexCoordinate(int q, int r)
        {
            Q = q;
            R = r;
        }

        public int X => Q;
        public int Y => -Q - R;
        public int Z => R;
        public int Q { get; }
        public int R { get; }

        public override int GetHashCode()
        {
            return HashUtils.IntegerHash(X, Y);
        }

        public override bool Equals(object obj)
        {
            var coord = obj as IHexCoordinate;
            if (coord != null)
                return Equals(coord);
            var ring = obj as IRingHexCoordinate;
            return ring != null && Equals(ring);
        }

        public bool Equals(IHexCoordinate obj)
        {
            return obj.Q == Q && obj.R == R;
        }

        public bool Equals(IRingHexCoordinate obj)
        {
            return Equals(obj.ConvertTo());
        }

        public override string ToString()
        {
            return $"X, Y, Z: {X}, {Y}, {Z}";
        }
    }
}