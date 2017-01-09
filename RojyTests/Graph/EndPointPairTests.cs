using System;
using System.Collections.Generic;
using AvalonAssets.Rojy.Graph;
using NUnit.Framework;

namespace AvalonAssets.RojyTests.Graph
{
    [TestFixture]
    public class EndPointPairTests
    {
        private EndPointPair<int> _undirectedPair, _directedPair;
        private IList<int> _testNum1, _testNum2;

        [OneTimeSetUp]
        public void Initialize()
        {
            _testNum1 = CreateNumList();
            _testNum2 = CreateNumList();
            _undirectedPair = EndPointPair<int>.Undirected(_testNum1[0], _testNum1[1]);
            _directedPair = EndPointPair<int>.Directed(_testNum2[0], _testNum2[1]);
        }

        private static IList<int> CreateNumList()
        {
            var rand = new Random();
            var num1 = rand.Next();
            var num2 = rand.Next();
            while (num2 == num1)
            {
                num2 = rand.Next();
            }
            return new List<int> {num1, num2};
        }

        [Test]
        public void ContainsTest()
        {
            Assert.True(_undirectedPair.Contains(_testNum1[0]));
            Assert.True(_undirectedPair.Contains(_testNum1[1]));
            Assert.True(_directedPair.Contains(_testNum2[0]));
            Assert.True(_directedPair.Contains(_testNum2[1]));
            var rand = new Random();
            var testNum = rand.Next();
            while (_testNum1.Contains(testNum))
            {
                testNum = rand.Next();
            }
            Assert.False(_undirectedPair.Contains(testNum));
            while (_testNum2.Contains(testNum))
            {
                testNum = rand.Next();
            }
            Assert.False(_directedPair.Contains(testNum));
        }

        [Test]
        public void EqualsTest()
        {
            Assert.True(_undirectedPair.Equals(EndPointPair<int>.Undirected(_testNum1[0], _testNum1[1])));
            Assert.True(_undirectedPair.Equals(EndPointPair<int>.Undirected(_testNum1[1], _testNum1[0])));
            Assert.True(_directedPair.Equals(EndPointPair<int>.Directed(_testNum2[0], _testNum2[1])));
            Assert.False(_directedPair.Equals(EndPointPair<int>.Directed(_testNum2[1], _testNum2[0])));
        }

        [Test]
        public void GetEnumeratorTest()
        {
            CollectionAssert.AreEqual(_undirectedPair, _testNum1);
            CollectionAssert.AreEqual(_directedPair, _testNum2);
        }

        [Test]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(_undirectedPair.GetHashCode(),
                EndPointPair<int>.Undirected(_testNum1[1], _testNum1[0]).GetHashCode());
            Assert.AreNotEqual(_directedPair.GetHashCode(),
                EndPointPair<int>.Undirected(_testNum2[1], _testNum2[0]).GetHashCode());
        }

        [Test]
        public void NeighborTest()
        {
            Assert.AreEqual(_undirectedPair.Neighbor(_testNum1[0]), _testNum1[1]);
            Assert.AreEqual(_undirectedPair.Neighbor(_testNum1[1]), _testNum1[0]);
            Assert.AreEqual(_directedPair.Neighbor(_testNum2[0]), _testNum2[1]);
            Assert.AreEqual(_directedPair.Neighbor(_testNum2[1]), _testNum2[0]);
        }
    }
}