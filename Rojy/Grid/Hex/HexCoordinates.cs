using System;

namespace AvalonAssets.Rojy.Grid.Hex
{
    /// <summary>
    ///     Initializes <see cref="IHexCoordinate" /> and <see cref="IRingHexCoordinate" />.
    /// </summary>
    public static class HexCoordinates
    {
        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="IHexCoordinate" /> with cube coordinate.
        ///     </para>
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <param name="z">Z.</param>
        /// <returns>New <see cref="IHexCoordinate" />.</returns>
        /// <exception cref="ArgumentException"><paramref name="x" /> + <paramref name="y" /> + <paramref name="z" /> != 0</exception>
        public static IHexCoordinate Cube(int x, int y, int z)
        {
            return new HexCoordinate(x, y, z);
        }

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="IHexCoordinate" /> with axial coordinate.
        ///     </para>
        /// </summary>
        /// <param name="q">Q.</param>
        /// <param name="r">R.</param>
        /// <returns>New <see cref="IHexCoordinate" />.</returns>
        public static IHexCoordinate Axial(int q, int r)
        {
            return new HexCoordinate(q, r);
        }

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="IRingHexCoordinate" />.
        ///     </para>
        /// </summary>
        /// <param name="center">Center of <see cref="IRingHexCoordinate" />.</param>
        /// <param name="radius">Radius from <paramref name="center" />.</param>
        /// <param name="index">
        ///     Index start from 0. Count from the top of <paramref name="center" />.
        ///     If there are two, take the one on right.
        /// </param>
        /// <returns>New <see cref="IRingHexCoordinate" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="center" /> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> or <paramref name="index" /> &lt; 0</exception>
        public static IRingHexCoordinate Ring(IHexCoordinate center, int radius, int index)
        {
            return new RingHexCoordinate(center, radius, index);
        }
    }
}