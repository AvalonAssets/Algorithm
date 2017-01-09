using System;
using System.Linq;
using AvalonAssets.Core.Utility;

namespace AvalonAssets.Rojy.Grid.Hex
{
    internal class RingHexCoordinate : IRingHexCoordinate
    {
        private readonly IHexCoordinate _center;

        public RingHexCoordinate(IHexCoordinate center, int radius, int index)
        {
            if (center == null)
                throw new ArgumentNullException(nameof(center));
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            Radius = radius;
            Index = index;
            _center = center;
        }

        public IHexCoordinate ConvertTo()
        {
            if (Radius == 0)
                return _center;
            var ringSize = 6*Radius;
            var tweakedIndex = (Index + (Radius + 1)/2)%ringSize;
            var directionIndex = tweakedIndex/Radius;
            var direction = EnumUtils.Values<HexDirection>().Shift(-2).ToList();
            // Compute the ring index of the corner tile at the end of this spoke:
            var cornerIndex = directionIndex*Radius;
            // Compute how much further we still need to go:
            var excess = tweakedIndex - cornerIndex;
            return _center.Add(direction[directionIndex].Distance(Radius))
                .Add(direction[(directionIndex + 2)%6].Distance(excess));
        }

        public int Index { get; }
        public int Radius { get; }

        public override bool Equals(object obj)
        {
            var coord = obj as IHexCoordinate;
            if (coord != null)
                return Equals(coord);
            var ring = obj as IRingHexCoordinate;
            return ring != null && Equals(ring);
        }

        public bool Equals(IRingHexCoordinate obj)
        {
            return Equals(ConvertTo(), obj.ConvertTo());
        }

        public bool Equals(IHexCoordinate obj)
        {
            return Equals(ConvertTo(), obj);
        }

        public override int GetHashCode()
        {
            return ConvertTo().GetHashCode();
        }
    }
}