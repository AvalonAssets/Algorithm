using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Rojy.Graph;
using NUnit.Framework;

// ReSharper disable CoVariantArrayConversion

namespace AvalonAssets.RojyTests.Graph
{
    [TestFixture]
    public class GraphTests
    {
        [SetUp]
        public void Setup()
        {
            _mutableGraphs = new[]
            {
                Graph<int>.Undirected(),
                Graph<int>.Directed(),
                Graph<int>.Undirected(true),
                Graph<int>.Directed(true)
            };
            _mutableValueGraphs = new[]
            {
                Graph<int>.Undirected<int>(),
                Graph<int>.Directed<int>(),
                Graph<int>.Undirected<int>(true),
                Graph<int>.Directed<int>(true)
            };
        }

        private static readonly Random Random = new Random();
        private const int UnDir = 0, Dir = 1, UnDirAlw = 2, DirAlw = 3;
        private IMutableGraph<int>[] _mutableGraphs;
        private IMutableValueGraph<int, int>[] _mutableValueGraphs;

        private static void NodeTest<T>(IEnumerable<T> nodes, params IGraph<T>[] graphs)
        {
            var nodeList = nodes.ToList();
            foreach (var graph in graphs)
            {
                CollectionAssert.AreEquivalent(nodeList, graph.Nodes);
            }
        }

        private static IList<int> SetUpEdge(IMutableGraph<int>[] graphs)
        {
            var nodes = Random.UniqueNumberList(5);
            AddNodes(graphs, nodes);
            foreach (var graph in graphs)
            {
                graph.PutEdge(nodes[0], nodes[1]);
                graph.PutEdge(nodes[2], nodes[1]);
                graph.PutEdge(nodes[2], nodes[3]);
                graph.PutEdge(nodes[3], nodes[2]);
            }
            return nodes;
        }

        private static IList<int> SetUpEdge(IMutableValueGraph<int, int>[] graphs)
        {
            var nodes = Random.UniqueNumberList(5);
            AddNodes(graphs, nodes);
            foreach (var graph in graphs)
            {
                graph.PutEdge(nodes[0], nodes[1], 1);
                graph.PutEdge(nodes[2], nodes[1], 2);
                graph.PutEdge(nodes[2], nodes[3], 2);
                graph.PutEdge(nodes[3], nodes[2], 3);
            }
            return nodes;
        }

        private static void AddNodes<T>(IMutableGraph<T>[] graphs, IEnumerable<T> nodes)
        {
            foreach (var node in nodes)
            {
                foreach (var graph in graphs)
                {
                    graph.AddNode(node);
                }
            }
        }

        private static void AddNodes<T, TValue>(IMutableValueGraph<T, TValue>[] graphs, IEnumerable<T> nodes)
        {
            foreach (var node in nodes)
            {
                foreach (var graph in graphs)
                {
                    graph.AddNode(node);
                }
            }
        }

        private static void InDegreeTest(IEnumerable<IGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                Assert.AreEqual(graph.IsDirected ? 0 : 1, graph.InDegree(nodesList[0]));
                Assert.AreEqual(2, graph.InDegree(nodesList[1]));
                Assert.AreEqual(graph.IsDirected ? 1 : 2, graph.InDegree(nodesList[2]));
                Assert.AreEqual(1, graph.InDegree(nodesList[3]));
                Assert.AreEqual(0, graph.InDegree(nodesList[4]));
            }
        }

        private static void OutDegreeTest(IEnumerable<IGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                Assert.AreEqual(1, graph.OutDegree(nodesList[0]));
                Assert.AreEqual(graph.IsDirected ? 0 : 2, graph.OutDegree(nodesList[1]));
                Assert.AreEqual(2, graph.OutDegree(nodesList[2]));
                Assert.AreEqual(1, graph.OutDegree(nodesList[3]));
                Assert.AreEqual(0, graph.OutDegree(nodesList[4]));
            }
        }

        private static void NeighborsTest(IEnumerable<IGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                CollectionAssert.AreEquivalent(new List<int> {nodesList[1]}, graph.Neighbors(nodesList[0]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[0], nodesList[2]}, graph.Neighbors(nodesList[1]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[1], nodesList[3]}, graph.Neighbors(nodesList[2]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[2]}, graph.Neighbors(nodesList[3]));
                CollectionAssert.AreEquivalent(new List<int>(), graph.Neighbors(nodesList[4]));
            }
        }

        private static void PredecessorsTest(IEnumerable<IGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                CollectionAssert.AreEquivalent(
                    graph.IsDirected ? new List<int>() : new List<int> {nodesList[1]},
                    graph.Predecessors(nodesList[0]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[0], nodesList[2]},
                    graph.Predecessors(nodesList[1]));
                CollectionAssert.AreEquivalent(
                    graph.IsDirected ? new List<int> {nodesList[3]} : new List<int> {nodesList[1], nodesList[3]},
                    graph.Predecessors(nodesList[2]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[2]}, graph.Predecessors(nodesList[3]));
                CollectionAssert.AreEquivalent(new List<int>(), graph.Predecessors(nodesList[4]));
            }
        }

        private static void SuccessorsTest(IEnumerable<IGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                CollectionAssert.AreEquivalent(new List<int> {nodesList[1]}, graph.Successors(nodesList[0]));
                CollectionAssert.AreEquivalent(
                    graph.IsDirected ? new List<int>() : new List<int> {nodesList[0], nodesList[2]},
                    graph.Successors(nodesList[1]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[1], nodesList[3]},
                    graph.Successors(nodesList[2]));
                CollectionAssert.AreEquivalent(new List<int> {nodesList[2]}, graph.Successors(nodesList[3]));
                CollectionAssert.AreEquivalent(new List<int>(), graph.Successors(nodesList[4]));
            }
        }

        private static void RemoveNodeTest(IEnumerable<IMutableGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                var tempList = new List<int>(nodesList);
                foreach (var node in nodesList)
                {
                    tempList.Remove(node);
                    graph.RemoveNode(node);
                    CollectionAssert.AreEquivalent(tempList, graph.Nodes);
                }
            }
        }

        private static void RemoveEdgeTest(IEnumerable<IMutableGraph<int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                Assert.AreEqual(!graph.IsDirected, graph.RemoveEdge(nodesList[1], nodesList[0]));
                Assert.True(graph.RemoveEdge(nodesList[2], nodesList[3]));
                Assert.AreEqual(graph.IsDirected, graph.RemoveEdge(nodesList[3], nodesList[2]));
            }
        }

        private static void RemoveEdgeTest(IEnumerable<IMutableValueGraph<int, int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                Assert.AreEqual(!graph.IsDirected, graph.RemoveEdge(nodesList[1], nodesList[0]));
                Assert.True(graph.RemoveEdge(nodesList[2], nodesList[3]));
                Assert.AreEqual(graph.IsDirected, graph.RemoveEdge(nodesList[3], nodesList[2]));
            }
        }

        private static void RemoveNodeTest(IEnumerable<IMutableValueGraph<int, int>> graphs, IList<int> nodesList)
        {
            foreach (var graph in graphs)
            {
                var tempList = new List<int>(nodesList);
                foreach (var node in nodesList)
                {
                    tempList.Remove(node);
                    graph.RemoveNode(node);
                    CollectionAssert.AreEquivalent(tempList, graph.Nodes);
                }
            }
        }

        [Test]
        public void AddNodeTest()
        {
            var nodes = Random.UniqueNumberList();
            AddNodes(_mutableGraphs, nodes);
            AddNodes(_mutableValueGraphs, nodes);
            NodeTest(nodes, _mutableGraphs);
            NodeTest(nodes, _mutableValueGraphs);
        }

        [Test]
        public void DegreeTest()
        {
            var nodes = SetUpEdge(_mutableGraphs);
            foreach (var graph in _mutableGraphs)
            {
                Assert.AreEqual(0, graph.Degree(nodes[4]));
                Assert.AreEqual(1, graph.Degree(nodes[0]));
            }
            Assert.AreEqual(3, _mutableGraphs[Dir].Degree(nodes[2]));
            Assert.AreEqual(3, _mutableGraphs[DirAlw].Degree(nodes[2]));
            Assert.AreEqual(2, _mutableGraphs[Dir].Degree(nodes[3]));
            Assert.AreEqual(2, _mutableGraphs[DirAlw].Degree(nodes[3]));
            Assert.AreEqual(2, _mutableGraphs[UnDir].Degree(nodes[2]));
            Assert.AreEqual(2, _mutableGraphs[UnDirAlw].Degree(nodes[2]));
            Assert.AreEqual(1, _mutableGraphs[UnDir].Degree(nodes[3]));
            Assert.AreEqual(1, _mutableGraphs[UnDirAlw].Degree(nodes[3]));
            _mutableGraphs[DirAlw].PutEdge(nodes[2], nodes[2]);
            Assert.AreEqual(4, _mutableGraphs[DirAlw].Degree(nodes[2]));
            _mutableGraphs[UnDirAlw].PutEdge(nodes[3], nodes[3]);
            Assert.AreEqual(3, _mutableGraphs[UnDirAlw].Degree(nodes[3]));
            nodes = SetUpEdge(_mutableValueGraphs);
            foreach (var graph in _mutableValueGraphs)
            {
                Assert.AreEqual(0, graph.Degree(nodes[4]));
                Assert.AreEqual(1, graph.Degree(nodes[0]));
            }
            Assert.AreEqual(3, _mutableValueGraphs[Dir].Degree(nodes[2]));
            Assert.AreEqual(3, _mutableValueGraphs[DirAlw].Degree(nodes[2]));
            Assert.AreEqual(2, _mutableValueGraphs[Dir].Degree(nodes[3]));
            Assert.AreEqual(2, _mutableValueGraphs[DirAlw].Degree(nodes[3]));
            Assert.AreEqual(2, _mutableValueGraphs[UnDir].Degree(nodes[2]));
            Assert.AreEqual(2, _mutableValueGraphs[UnDirAlw].Degree(nodes[2]));
            Assert.AreEqual(1, _mutableValueGraphs[UnDir].Degree(nodes[3]));
            Assert.AreEqual(1, _mutableValueGraphs[UnDirAlw].Degree(nodes[3]));
            _mutableValueGraphs[DirAlw].PutEdge(nodes[2], nodes[2], 0);
            Assert.AreEqual(4, _mutableValueGraphs[DirAlw].Degree(nodes[2]));
            _mutableValueGraphs[UnDirAlw].PutEdge(nodes[3], nodes[3], 0);
            Assert.AreEqual(3, _mutableValueGraphs[UnDirAlw].Degree(nodes[3]));
        }

        [Test]
        public void InDegreeTest()
        {
            InDegreeTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            InDegreeTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void NeighborsTest()
        {
            NeighborsTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            NeighborsTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void OutDegreeTest()
        {
            OutDegreeTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            OutDegreeTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void PredecessorsTest()
        {
            PredecessorsTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            PredecessorsTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void RemoveEdgeTest()
        {
            RemoveEdgeTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            RemoveEdgeTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void RemoveNodeTest()
        {
            RemoveNodeTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            RemoveNodeTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }

        [Test]
        public void SuccessorsTest()
        {
            SuccessorsTest(_mutableGraphs, SetUpEdge(_mutableGraphs));
            SuccessorsTest(_mutableValueGraphs, SetUpEdge(_mutableValueGraphs));
        }
    }
}