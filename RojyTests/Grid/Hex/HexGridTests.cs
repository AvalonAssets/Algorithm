using System;
using System.Collections.Generic;
using AvalonAssets.Rojy;
using AvalonAssets.Rojy.Grid.Hex;
using NUnit.Framework;

namespace AvalonAssets.RojyTests.Grid.Hex
{
    [TestFixture]
    public class HexGridTests
    {
        [SetUp]
        public void SetUp()
        {
            _grid = new HexGrid<int>();
        }

        private HexGrid<int> _grid;
        private Random _random;

        [OneTimeSetUp]
        public void Initialize()
        {
            _random = new Random();
        }

        [Test]
        public void ContainsKeyTest()
        {
            var queue = _random.UniqueNumberQueue();
            var coord = HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue());
            _grid.Add(coord, queue.Dequeue());
            Assert.True(_grid.ContainsKey(coord));
            var coord2 = HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue());
            Assert.False(_grid.ContainsKey(coord2));
        }

        [Test]
        public void ContainsValueTest()
        {
            var queue = _random.UniqueNumberQueue();
            var value = queue.Dequeue();
            _grid.Add(HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue()), value);
            Assert.True(_grid.ContainsValue(value));
            Assert.False(_grid.ContainsValue(queue.Dequeue()));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            var queue = _random.UniqueNumberQueue(50);
            var pairList = new List<KeyValuePair<IHexCoordinate, int>>();
            for (var i = 0; i < 10; i++)
            {
                var coord = HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue());
                var value = queue.Dequeue();
                pairList.Add(new KeyValuePair<IHexCoordinate, int>(coord, value));
                _grid.Add(coord, value);
            }
            CollectionAssert.AreEquivalent(pairList, _grid);
        }

        [Test]
        public void HexGridTest()
        {
            _grid.Add(HexCoordinates.Cube(0, 0, 0), 0);
            _grid.Add(HexCoordinates.Cube(1, -1, 0), 0);
            _grid.Add(HexCoordinates.Cube(1, -2, 1), 0);
            _grid.Add(HexCoordinates.Cube(-1, 0, 1), 0);
            _grid.Add(HexCoordinates.Cube(-1, 1, 0), 0);
            _grid.Add(HexCoordinates.Cube(0, 1, -1), 0);
            _grid.Add(HexCoordinates.Cube(1, 0, -1), 0);
            _grid.Add(HexCoordinates.Cube(2, -3, 1), 0);
            var path = _grid.BreadthFirstSearch(HexCoordinates.Cube(0, 0, 0), HexCoordinates.Cube(2, -3, 1));
            var expected = new List<IHexCoordinate>
            {
                HexCoordinates.Cube(0, 0, 0),
                HexCoordinates.Cube(1, -1, 0),
                HexCoordinates.Cube(1, -2, 1),
                HexCoordinates.Cube(2, -3, 1)
            };
            CollectionAssert.AreEquivalent(expected, path);
        }

        [Test]
        public void TryGetValueTest()
        {
            var queue = _random.UniqueNumberQueue();
            var coord = HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue());
            var expect = queue.Dequeue();
            _grid.Add(coord, expect);
            int value;
            Assert.True(_grid.TryGetValue(coord, out value));
            Assert.AreEqual(expect, value);
            var coord2 = HexCoordinates.Axial(queue.Dequeue(), queue.Dequeue());
            Assert.False(_grid.TryGetValue(coord2, out value));
        }
    }
}