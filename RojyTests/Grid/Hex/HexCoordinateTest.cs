using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Core.Utility;
using AvalonAssets.Rojy.Grid.Hex;
using NUnit.Framework;

namespace AvalonAssets.RojyTests.Grid.Hex
{
    [TestFixture]
    public class HexCoordinateTest
    {
        protected IHexCoordinate Center;

        [OneTimeSetUp]
        public void Initialize()
        {
            Center = HexCoordinates.Cube(1, 1, -2);
        }

        [Test]
        public void DiagonalTest()
        {
            var expected = new List<IHexCoordinate>
            {
                HexCoordinates.Cube(3, 0, -3),
                HexCoordinates.Cube(2, -1, -1),
                HexCoordinates.Cube(0, 0, 0),
                HexCoordinates.Cube(-1, 2, -1),
                HexCoordinates.Cube(0, 3, -3),
                HexCoordinates.Cube(2, 2, -4)
            };
            var result =
                EnumUtils.Values<HexDirection>().Select(direction => Center.Diagonal(direction)).ToList();
            CollectionAssert.AreEquivalent(expected, result);
        }

        [Test]
        public void DistanceTest()
        {
            Assert.AreEqual(2, Center.Distance(HexCoordinates.Cube(1, -1, 0)));
        }

        [Test]
        public void FieldOfViewTest()
        {
            Func<IHexCoordinate, bool> isBlock = c =>
                c.Equals(HexCoordinates.Cube(0, 0, 0)) ||
                c.Equals(HexCoordinates.Cube(1, 0, -1)) ||
                c.Equals(HexCoordinates.Cube(2, 0, -2)) ||
                c.Equals(HexCoordinates.Cube(0, 2, -2));
            var field = Center.FieldOfView(5, isBlock).ToList();
            Assert.False(field.Contains(HexCoordinates.Cube(0, 0, 0)));
            Assert.False(field.Contains(HexCoordinates.Cube(-1, -2, 3)));
            Assert.False(field.Contains(HexCoordinates.Cube(2, -1, -1)));
            Assert.False(field.Contains(HexCoordinates.Cube(3, 0, -3)));
            Assert.True(field.Contains(HexCoordinates.Cube(1, 2, -3)));
            Assert.True(field.Contains(HexCoordinates.Cube(4, 0, -4)));
        }

        [Test]
        public void FromCubeTest()
        {
            Assert.Throws<ArgumentException>(() => { HexCoordinates.Cube(1, 1, 1); });
        }

        [Test]
        public void FromRingTest()
        {
            Assert.AreEqual(HexCoordinates.Cube(1, -1, 0), HexCoordinates.Ring(Center, 2, 5));
            Assert.AreEqual(HexCoordinates.Cube(4, -2, -2), HexCoordinates.Ring(Center, 3, 4));
        }

        [Test]
        public void LineTest()
        {
            var expected = new List<IHexCoordinate>
            {
                Center,
                HexCoordinates.Cube(1, 0, -1),
                HexCoordinates.Cube(0, 0, 0),
                HexCoordinates.Cube(0, -1, 1)
            };
            CollectionAssert.AreEqual(expected, Center.Line(HexCoordinates.Cube(0, -1, 1)));
        }

        [Test]
        public void NeighborTest()
        {
            Assert.AreEqual(HexCoordinates.Cube(2, 0, -2), Center.Neighbor(HexDirection.A));
            var neighbor =
                EnumUtils.Values<HexDirection>().Select(direction => Center.Neighbor(direction)).ToList();
            CollectionAssert.AreEquivalent(neighbor, Center.AllNeighbors());
            neighbor = new List<IHexCoordinate>
            {
                HexCoordinates.Cube(2, 0, -2),
                HexCoordinates.Cube(1, 0, -1),
                HexCoordinates.Cube(0, 1, -1),
                HexCoordinates.Cube(0, 2, -2),
                HexCoordinates.Cube(1, 2, -3),
                HexCoordinates.Cube(2, 1, -3)
            };
            CollectionAssert.AreEquivalent(neighbor, Center.AllNeighbors());
        }

        [Test]
        public void RangeTest()
        {
            var expected = Center.AllNeighbors().ToList();
            expected.Add(Center);
            CollectionAssert.AreEquivalent(expected, Center.Range(1));
            CollectionAssert.AreEquivalent(new List<IHexCoordinate> {Center}, Center.Range(0));
        }

        [Test]
        public void ReachableTest()
        {
            var expected = new List<IHexCoordinate>
            {
                HexCoordinates.Cube(0, 1, -1),
                HexCoordinates.Cube(0, 2, -2),
                HexCoordinates.Cube(1, 2, -3),
                HexCoordinates.Cube(2, 1, -3)
            };
            Func<IHexCoordinate, bool> isBlock = c =>
                c.Equals(HexCoordinates.Cube(0, 0, 0)) ||
                c.Equals(HexCoordinates.Cube(1, 0, -1)) ||
                c.Equals(HexCoordinates.Cube(2, 0, -2));
            CollectionAssert.AreEquivalent(expected, Center.Reachable(1, isBlock));
        }

        [Test]
        public void RingOfTest()
        {
            Assert.AreEqual(HexCoordinates.Ring(Center, 3, 9), Center.ToRing(HexCoordinates.Cube(-1, 0, 1)));
            Assert.AreEqual(HexCoordinates.Ring(Center, 3, 9), Center.ToRing(HexCoordinates.Cube(-1, 0, 1)));
            Assert.AreEqual(HexCoordinates.Ring(Center, 2, 3),
                Center.ToRing(HexCoordinates.Ring(Center, 2, 3).ConvertTo()));
        }

        [Test]
        public void RingTest()
        {
            var expected = Center.Range(1).ToList();
            expected.Remove(Center);
            CollectionAssert.AreEquivalent(expected, Center.Ring(1));
            CollectionAssert.AreEquivalent(Center.AllNeighbors(), Center.Ring(1));
            const int size = 3;
            var result = new List<IHexCoordinate>();
            for (var i = 0; i <= size; i++)
                result.AddRange(Center.Ring(i));
            CollectionAssert.AreEquivalent(Center.Range(size), result);
        }

        [Test]
        public void RotateTest()
        {
            Assert.AreEqual(HexCoordinates.Cube(-2, 4, -2), Center.Rotate(HexCoordinates.Cube(4, -2, -2), 3));
        }

        [Test]
        public void SpiralTest()
        {
            const int size = 3;
            var expected = new List<IHexCoordinate>();
            for (var i = 0; i <= size; i++)
                expected.AddRange(Center.Ring(i));
            CollectionAssert.AreEqual(expected, Center.Spiral(size));
        }

        [Test]
        public void VisibleTest()
        {
            Func<IHexCoordinate, bool> isBlock = c => c.Equals(HexCoordinates.Cube(0, 0, 0));
            Assert.True(Center.Visible(HexCoordinates.Cube(1, -1, 0), isBlock));
            Assert.False(Center.Visible(HexCoordinates.Cube(-1, 0, 1), isBlock));
        }
    }
}