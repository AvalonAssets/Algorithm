using System;
using System.Collections.Generic;

namespace AvalonAssets.Rojy.Grid.Hex
{
    public class Region
    {
        public readonly IHexCoordinate Center;
        public readonly int Radius;

        public Region(IHexCoordinate center, int radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        ///     Returns all the intersect <see cref="IHexCoordinate" /> with other <see cref="Region" />.
        /// </summary>
        /// <param name="regions"></param>
        /// <returns>Intersect <see cref="IHexCoordinate" /></returns>
        public IEnumerable<IHexCoordinate> Intersection(params Region[] regions)
        {
            if (Radius < 0)
                throw new ArgumentOutOfRangeException();
            var minX = Center.X - Radius;
            var maxX = Center.X + Radius;
            var minY = Center.Y - Radius;
            var maxY = Center.Y + Radius;
            var minZ = Center.Z - Radius;
            var maxZ = Center.Z + Radius;
            foreach (var pair in regions)
            {
                minX = Math.Max(minX, pair.Center.X - pair.Radius);
                maxX = Math.Min(maxX, pair.Center.X + pair.Radius);
                minY = Math.Max(minY, pair.Center.Y - pair.Radius);
                maxY = Math.Min(maxY, pair.Center.Y + pair.Radius);
                minZ = Math.Max(minZ, pair.Center.Z - pair.Radius);
                maxZ = Math.Min(maxZ, pair.Center.Z + pair.Radius);
            }
            for (var x = minX; x <= maxX; x++)
                for (var y = Math.Max(minY, -x - maxZ); y <= Math.Min(maxY, -x - minZ); y++)
                    yield return HexCoordinates.Cube(x, y, -x - y);
        }

        /// <summary>
        ///     Returns all <see cref="IHexCoordinate" /> within the region.
        /// </summary>
        /// <returns><see cref="IHexCoordinate" /> inside the region.</returns>
        public IEnumerable<IHexCoordinate> All()
        {
            return Center.Range(Radius);
        }

        public override bool Equals(object obj)
        {
            if (Radius == 0)
            {
                var coord = obj as IHexCoordinate;
                if (coord != null)
                    return Center.Equals(coord);
            }
            var ring = obj as Region;
            return ring != null && Equals(ring);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Center?.GetHashCode() ?? 0)*397) ^ Radius;
            }
        }

        public bool Equals(Region obj)
        {
            return Center.Equals(obj.Center) && Radius.Equals(obj.Radius);
        }
    }
}