using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Algorithm.Geometry;
using AvalonAssets.Utility;

namespace AvalonAssets.Algorithm.Grid.Hex
{
    /// <summary>
    ///     It implements many functionalities used for <see cref="IHexCoordinate" />.
    /// </summary>
    public static class HexCoordinateExtensions
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new <see cref="IHexCoordinate" /> from current position with given <paramref name="direction" />
        ///         and <paramref name="distance" />.
        ///     </para>
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="direction">Direction of the new <see cref="IHexCoordinate" /> from <paramref name="coordinate" />.</param>
        /// <param name="distance">Distance from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <returns>Requested <see cref="IHexCoordinate" /></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <para><paramref name="distance" /> &lt; 0.</para>
        ///     <para><paramref name="direction" /> out of range.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> is null.</exception>
        public static IHexCoordinate Neighbor(this IHexCoordinate coordinate, HexDirection direction, int distance = 1)
        {
            if (distance == 0)
                return coordinate;
            if (distance < 0)
                throw new ArgumentOutOfRangeException("distance");
            var qOffset = 0;
            var rOffset = 0;
            switch (direction)
            {
                case HexDirection.A:
                    qOffset = 1;
                    break;
                case HexDirection.B:
                    rOffset = 1;
                    break;
                case HexDirection.C:
                    qOffset = -1;
                    rOffset = 1;
                    break;
                case HexDirection.D:
                    qOffset = -1;
                    break;
                case HexDirection.E:
                    rOffset = -1;
                    break;
                case HexDirection.F:
                    qOffset = 1;
                    rOffset = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
            return coordinate.OffsetDistance(qOffset, rOffset, distance);
        }

        /// <summary>
        ///     <para>
        ///         Initializes a new <see cref="IHexCoordinate" /> from current position with given <paramref name="direction" />
        ///         and <paramref name="distance" />.
        ///         Diagonal of <see cref="HexDirection.A" /> is the other neighbor of <see cref="HexDirection.A" /> and
        ///         <see cref="HexDirection.B" />.
        ///     </para>
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="direction">Direction of the new <see cref="IHexCoordinate" /> from <paramref name="coordinate" />.</param>
        /// <param name="distance">Distance from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <returns>Requested <see cref="IHexCoordinate" /></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <para><paramref name="distance" /> &lt; 0.</para>
        ///     <para><paramref name="direction" /> out of range.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> is null.</exception>
        public static IHexCoordinate Diagonal(this IHexCoordinate coordinate, HexDirection direction, int distance = 1)
        {
            if (distance == 0)
                return coordinate;
            if (distance < 0)
                throw new ArgumentOutOfRangeException("distance");
            int qOffset;
            int rOffset;
            switch (direction)
            {
                case HexDirection.A:
                    qOffset = 1;
                    rOffset = 1;
                    break;
                case HexDirection.B:
                    qOffset = -1;
                    rOffset = 2;
                    break;
                case HexDirection.C:
                    qOffset = -2;
                    rOffset = 1;
                    break;
                case HexDirection.D:
                    qOffset = -1;
                    rOffset = -1;
                    break;
                case HexDirection.E:
                    qOffset = 1;
                    rOffset = -2;
                    break;
                case HexDirection.F:
                    qOffset = 2;
                    rOffset = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
            return coordinate.OffsetDistance(qOffset, rOffset, distance);
        }

        private static IHexCoordinate OffsetDistance(this IAxialCoordinate coordinate, int qOffset, int rOffset,
            int distance)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            qOffset *= distance;
            rOffset *= distance;
            return HexCoordinates.Axial(coordinate.Q + qOffset, coordinate.R + rOffset);
        }

        /// <summary>
        ///     <para>
        ///         Converts <paramref name="target" /> to a relative <see cref="IRingHexCoordinate" /> with respect to
        ///         <paramref name="coordinate" />.
        ///     </para>
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="target"><see cref="IHexCoordinate" /> to be converted.</param>
        /// <returns><see cref="IRingHexCoordinate" /> with respect to <paramref name="coordinate" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> or <paramref name="target" /> is null.</exception>
        public static IRingHexCoordinate ToRing(this IHexCoordinate coordinate, IHexCoordinate target)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (target == null)
                throw new ArgumentNullException("target");
            var different = target.Subtract(coordinate);
            var xOffset = Math.Abs(different.X);
            var yOffset = Math.Abs(different.Y);
            var zOffset = Math.Abs(different.Z);
            int directionIndex;
            if (yOffset >= xOffset)
                if (zOffset >= yOffset)
                    directionIndex = different.Z > 0 ? 3 : 0;
                else
                    directionIndex = different.Y > 0 ? 5 : 2;
            else if (xOffset >= zOffset)
                directionIndex = different.X > 0 ? 1 : 4;
            else
                directionIndex = different.Z > 0 ? 3 : 0;
            var radius = (xOffset + yOffset + zOffset)/2;
            // Direction start from E
            var direction = EnumUtils.Values<HexDirection>().Shift(-2).ToList();
            // Find the counter-clockwise corner tile.
            var corner = coordinate.Add(direction[directionIndex].Distance(radius));
            different = target.Subtract(corner);
            xOffset = Math.Abs(different.X);
            yOffset = Math.Abs(different.Y);
            zOffset = Math.Abs(different.Z);
            var excess = (xOffset + yOffset + zOffset)/2;

            // Our tile is at the corner's index, plus this excess.
            var index = radius*directionIndex + excess;
            var ringSize = 6*radius;
            index = (index + ringSize - (radius + 1)/2)%ringSize;
            return HexCoordinates.Ring(coordinate, radius, index);
        }

        /// <summary>
        ///     <para>
        ///         Calculates distance between two <see cref="IHexCoordinate" />, <paramref name="start" /> and
        ///         <paramref name="destination" />.
        ///     </para>
        /// </summary>
        /// <param name="start">Start <see cref="IHexCoordinate" />.</param>
        /// <param name="destination">Destination <see cref="IHexCoordinate" />.</param>
        /// <returns>Distance between <paramref name="start" /> and <paramref name="destination" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="start" /> or <paramref name="destination" /> is null.</exception>
        public static int Distance(this IHexCoordinate start, IHexCoordinate destination)
        {
            if (start == null)
                throw new ArgumentNullException("start");
            if (destination == null)
                throw new ArgumentNullException("destination");
            return (Math.Abs(start.X - destination.X) +
                    Math.Abs(start.Y - destination.Y) +
                    Math.Abs(start.Z - destination.Z))/2;
        }

        /// <summary>
        ///     <para>
        ///         Returns all <see cref="IHexCoordinate" /> within <paramref name="radius" /> and <paramref name="coordinate" />
        ///         as center.
        ///     </para>
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="radius">Radius from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <returns>All <see cref="IHexCoordinate" /> within <paramref name="radius" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> is null.</exception>
        public static IEnumerable<IHexCoordinate> Range(this IHexCoordinate coordinate, int radius)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (radius == 0)
            {
                yield return coordinate;
                yield break;
            }
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            for (var dx = -radius; dx <= radius; dx++)
                for (var dy = Math.Max(-radius, -dx - radius); dy <= Math.Min(radius, -dx + radius); dy++)
                    yield return HexCoordinates.Cube(dx, dy, -dx - dy).Add(coordinate);
        }

        /// <summary>
        ///     Returns true if there is nothing blocking between <paramref name="coordinate" /> and <paramref name="target" />.
        ///     Do not use this to find the field of view.
        ///     Uses <see cref="FieldOfView" /> to have better performance.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="target"><see cref="IHexCoordinate" /> to be checked.</param>
        /// <param name="isOpaque">Returns if <see cref="IHexCoordinate" /> is opaque.</param>
        /// <returns>True if there is nothing blocking between <paramref name="coordinate" /> and <paramref name="target" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> or <paramref name="isOpaque" /> is null.</exception>
        public static bool Visible(this IHexCoordinate coordinate, IHexCoordinate target,
            Func<IHexCoordinate, bool> isOpaque)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (isOpaque == null)
                throw new ArgumentNullException("isOpaque");
            return !coordinate.Line(target).Any(isOpaque);
        }

        /// <summary>
        ///     <para>
        ///         Plots a approximate line between  <paramref name="start" /> and <paramref name="destination" />.
        ///     </para>
        /// </summary>
        /// <param name="start">Start <see cref="IHexCoordinate" />.</param>
        /// <param name="destination">Destination <see cref="IHexCoordinate" />.</param>
        /// <returns>All <see cref="IHexCoordinate" /> pass through <paramref name="start" /> and <paramref name="destination" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="start" /> or <paramref name="destination" /> is null.</exception>
        public static IEnumerable<IHexCoordinate> Line(this IHexCoordinate start, IHexCoordinate destination)
        {
            var distance = start.Distance(destination);
            for (var i = 0; i <= distance; i++)
            {
                yield return Round(LinearInterpolation(start.X, destination.X, distance, i),
                    LinearInterpolation(start.Y, destination.Y, distance, i),
                    LinearInterpolation(start.Z, destination.Z, distance, i));
            }
        }

        private static double LinearInterpolation(int pA, int pB, int n, int i)
        {
            return pA + (pB - pA)*1.0/n*i;
        }

        private static IHexCoordinate Round(double x, double y, double z)
        {
            var rX = (int) Math.Round(x);
            var rY = (int) Math.Round(y);
            var rZ = (int) Math.Round(z);
            var xDiff = Math.Abs(rX - x);
            var yDiff = Math.Abs(rY - y);
            var zDiff = Math.Abs(rZ - z);
            if (xDiff > yDiff && xDiff > yDiff)
                rX = -rY - rZ;
            else if (yDiff > zDiff)
                rY = -rX - rZ;
            else
                rZ = -rX - rY;
            return HexCoordinates.Cube(rX, rY, rZ);
        }

        /// <summary>
        ///     Initializes a new unit <see cref="IHexCoordinate" /> with given <paramref name="direction" />  and
        ///     <paramref name="distance" />.
        /// </summary>
        /// <param name="direction">Direction from (0, 0).</param>
        /// <param name="distance">Distance from (0, 0).</param>
        /// <returns>Unit <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <para><paramref name="distance" /> &lt; 0.</para>
        ///     <para><paramref name="direction" /> out of range.</para>
        /// </exception>
        public static IHexCoordinate Distance(this HexDirection direction, int distance)
        {
            return HexCoordinates.Axial(0, 0).Neighbor(direction, distance);
        }

        /// <summary>
        ///     Returns all the <see cref="IHexCoordinate" /> that can be reached by given <paramref name="steps" /> from
        ///     <paramref name="coordinate" />.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="steps">Maximum steps can be used. 0 returns itself.</param>
        /// <param name="isOpaque">Returns if <see cref="IHexCoordinate" /> is opaque.</param>
        /// <returns>All reachable <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="steps" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> or <paramref name="isOpaque" /> is null.</exception>
        public static IEnumerable<IHexCoordinate> Reachable(this IHexCoordinate coordinate, int steps,
            Func<IHexCoordinate, bool> isOpaque)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (isOpaque == null)
                throw new ArgumentNullException("isOpaque");
            if (steps < 0)
                throw new ArgumentOutOfRangeException("steps");
            if (steps == 0)
            {
                yield return coordinate;
                yield break;
            }
            var visited = new HashSet<IHexCoordinate> {coordinate};
            var fringes = new List<IHexCoordinate> {coordinate};
            for (var i = 1; i <= steps; i++)
            {
                var newFringes = new List<IHexCoordinate>();
                foreach (var currentCoordinate in fringes)
                    foreach (var direction in EnumUtils.Values<HexDirection>())
                    {
                        var neighbor = currentCoordinate.Neighbor(direction);
                        if (visited.Contains(neighbor) || isOpaque(neighbor)) continue;
                        visited.Add(neighbor);
                        newFringes.Add(neighbor);
                        yield return neighbor;
                    }
                fringes = newFringes;
            }
        }

        /// <summary>
        ///     Plots a ring of <see cref="IHexCoordinate" /> with given <paramref name="radius" /> and
        ///     <paramref name="coordinate" /> as center.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="radius">Radius from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <returns>A ring of <see cref="IHexCoordinate" />.</returns>
        /// <returns>All reachable <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" />  is null.</exception>
        public static IEnumerable<IHexCoordinate> Ring(this IHexCoordinate coordinate, int radius)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            if (radius == 0)
            {
                yield return coordinate;
                yield break;
            }
            var current = coordinate.Add(HexDirection.E.Distance(radius));
            var directions = EnumUtils.Values<HexDirection>().ToList();
            foreach (var direction in directions)
                for (var i = 0; i < radius; i++)
                {
                    yield return current;
                    current = current.Neighbor(direction);
                }
        }

        /// <summary>
        ///     Plots a spiral ring of <see cref="IHexCoordinate" /> with given <paramref name="radius" /> and
        ///     <paramref name="coordinate" /> as center from inside to outside.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="radius">Radius from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <returns><see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" />  is null.</exception>
        public static IEnumerable<IHexCoordinate> Spiral(this IHexCoordinate coordinate, int radius)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius");
            if (radius == 0)
            {
                yield return coordinate;
                yield break;
            }
            yield return coordinate;
            for (var i = 1; i <= radius; i++)
                foreach (var current in coordinate.Ring(i))
                    yield return current;
        }

        /// <summary>
        ///     Field Of View.
        ///     Returns All visiable <see cref="IHexCoordinate" /> within <paramref name="radius" />.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="radius">Radius from <paramref name="coordinate" />. 0 returns <paramref name="coordinate" />.</param>
        /// <param name="isOpaque">Returns if <see cref="IHexCoordinate" /> is opaque.</param>
        /// <returns>All visiable <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> or <paramref name="isOpaque" /> is null.</exception>
        public static IEnumerable<IHexCoordinate> FieldOfView(this IHexCoordinate coordinate, int radius,
            Func<IHexCoordinate, bool> isOpaque)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (isOpaque == null)
                throw new ArgumentNullException("isOpaque");
            if (radius < 0)
                throw new ArgumentOutOfRangeException();
            if (isOpaque(coordinate))
                yield break;
            yield return coordinate;
            if (radius == 0)
                yield break;
            var shadowCast = new ShadowCast();
            for (var ringIndex = 1; ringIndex <= radius; ringIndex++)
            {
                var isEven = ringIndex%2 == 0;
                var slide = (double) 360/(6*ringIndex);
                for (var hexIndex = 0; hexIndex < ringIndex*6; hexIndex++)
                {
                    var ring = new RingHexCoordinate(coordinate, ringIndex, hexIndex);
                    var current = ring.ConvertTo();
                    var minAngle = hexIndex*slide;
                    if (isEven)
                        minAngle -= slide/2;
                    var maxAngle = minAngle + slide;
                    var center = (maxAngle + minAngle)/2.0;
                    // Ignore if the center of the hex cannot be seen
                    if (shadowCast.Hide(center))
                        continue;
                    yield return current;
                    // Add to shadow if the hex is opaque
                    if (isOpaque(current))
                        shadowCast.AddShadow(minAngle, maxAngle);
                }
            }
        }

        /// <summary>
        ///     Returns all the neighbors of <paramref name="coordinate" />.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <returns>Neighbors of <paramref name="coordinate" /></returns>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> is null.</exception>
        public static IEnumerable<IHexCoordinate> AllNeighbors(this IHexCoordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            return EnumUtils.Values<HexDirection>().Select(direction => coordinate.Neighbor(direction));
        }

        /// <summary>
        ///     Use this as a center to rotate other <see cref="HexCoordinate" />.
        /// </summary>
        /// <param name="coordinate">Current <see cref="IHexCoordinate" />.</param>
        /// <param name="target"><see cref="IHexCoordinate" /> to be rotated.</param>
        /// <param name="times">Number of 60°. Positive for clockwise.</param>
        /// <returns>Rotated <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="times" /> &lt; 0. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="coordinate" /> or <paramref name="target" /> is null.</exception>
        public static IHexCoordinate Rotate(this IHexCoordinate coordinate, IHexCoordinate target, int times)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");
            if (target == null)
                throw new ArgumentNullException("target");
            if (times < 0)
                throw new ArgumentOutOfRangeException("times");
            times %= 6;
            if (times == 0)
                return target;
            var vectorPx = target.X - coordinate.X;
            var vectorPy = target.Y - coordinate.Y;
            var vectorPz = target.Z - coordinate.Z;
            var isOdd = times%2 == 1;
            times %= 3;
            var isPositive = times > 0;
            times = Math.Abs(times);
            for (var i = 0; i < times; i++)
            {
                int temp;
                if (isPositive)
                {
                    temp = vectorPx;
                    vectorPx = vectorPz;
                    vectorPz = vectorPy;
                    vectorPy = temp;
                }
                else
                {
                    temp = vectorPx;
                    vectorPx = vectorPy;
                    vectorPy = vectorPz;
                    vectorPz = temp;
                }
            }
            if (!isOdd)
                return HexCoordinates.Cube(vectorPx + coordinate.X, vectorPy + coordinate.Y, vectorPz + coordinate.Z);
            vectorPx = -vectorPx;
            vectorPy = -vectorPy;
            vectorPz = -vectorPz;
            return HexCoordinates.Cube(vectorPx + coordinate.X, vectorPy + coordinate.Y, vectorPz + coordinate.Z);
        }

        /// <summary>
        ///     Returns the sum of two <see cref="IHexCoordinate" />.
        /// </summary>
        /// <param name="left"><see cref="IHexCoordinate" />.</param>
        /// <param name="right"><see cref="IHexCoordinate" />.</param>
        /// <returns>Sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left" /> or <paramref name="right" /> is null.</exception>
        public static IHexCoordinate Add(this IHexCoordinate left, IHexCoordinate right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return HexCoordinates.Axial(left.Q + right.Q, left.R + right.R);
        }

        /// <summary>
        ///     Returns the difference of two <see cref="IHexCoordinate" />.
        /// </summary>
        /// <param name="left"><see cref="IHexCoordinate" />.</param>
        /// <param name="right"><see cref="IHexCoordinate" />.</param>
        /// <returns>Difference of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left" /> or <paramref name="right" /> is null.</exception>
        public static IHexCoordinate Subtract(this IHexCoordinate left, IHexCoordinate right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");
            return HexCoordinates.Axial(left.Q - right.Q, left.R - right.R);
        }

        /// <summary>
        ///     Returns the product of two <see cref="IHexCoordinate" />.
        /// </summary>
        /// <param name="left"><see cref="IHexCoordinate" />.</param>
        /// <param name="right"><see cref="IHexCoordinate" />.</param>
        /// <returns>Product of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left" /> is null.</exception>
        public static IHexCoordinate Time(this IHexCoordinate left, int right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            return HexCoordinates.Axial(left.Q*right, left.R*right);
        }

        /// <summary>
        ///     Returns the quotient of two <see cref="IHexCoordinate" />.
        /// </summary>
        /// <param name="left"><see cref="IHexCoordinate" />.</param>
        /// <param name="right"><see cref="IHexCoordinate" />.</param>
        /// <returns>Quotient of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left" /> is null.</exception>
        public static IHexCoordinate Divide(this IHexCoordinate left, int right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            return HexCoordinates.Axial(left.Q/right, left.R/right);
        }
    }
}