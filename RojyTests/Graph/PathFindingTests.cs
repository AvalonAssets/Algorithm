using System;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Rojy;
using AvalonAssets.Rojy.Graph;
using NUnit.Framework;

namespace AvalonAssets.RojyTests.Graph
{
    [TestFixture]
    public class PathFindingTests
    {
        [Test]
        public void AStarAlgorithmTest()
        {
            var valueGraph = Graph<int>.Undirected<int>();
            const int nodeA = 1;
            const int nodeB = 2;
            const int nodeC = 0;
            const int nodeD = 3;
            const int nodeE = 4;
            const int nodeF = 5;
            const int nodeG = 7;
            valueGraph.AddNode(nodeA);
            valueGraph.AddNode(nodeB);
            valueGraph.AddNode(nodeC);
            valueGraph.AddNode(nodeD);
            valueGraph.AddNode(nodeE);
            valueGraph.AddNode(nodeF);
            valueGraph.AddNode(nodeG);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            valueGraph.PutEdge(nodeA, nodeB, 1);
            valueGraph.PutEdge(nodeA, nodeC, 1);
            valueGraph.PutEdge(nodeB, nodeA, 1);
            valueGraph.PutEdge(nodeB, nodeD, 1);
            valueGraph.PutEdge(nodeC, nodeF, 1);
            valueGraph.PutEdge(nodeC, nodeF, 1);
            valueGraph.PutEdge(nodeD, nodeB, 1);
            valueGraph.PutEdge(nodeD, nodeE, 1);
            valueGraph.PutEdge(nodeE, nodeD, 1);
            valueGraph.PutEdge(nodeE, nodeF, 1);
            valueGraph.PutEdge(nodeF, nodeE, 1);
            valueGraph.PutEdge(nodeF, nodeC, 1);
            valueGraph.PutEdge(nodeF, nodeG, 1);
            PathFinding.Heuristic<int> method = (a, b) => 1;
            var path = valueGraph.AStarAlgorithm(nodeA, nodeG, method).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA, nodeC, nodeF, nodeG};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        [Test]
        public void BreadthFirstSearchTest()
        {
            var graph = Graph<int>.Undirected();
            const int nodeA = 1;
            const int nodeB = 2;
            const int nodeC = 3;
            const int nodeD = 4;
            const int nodeE = 5;
            const int nodeF = 6;
            const int nodeG = 7;
            graph.AddNode(nodeA);
            graph.AddNode(nodeB);
            graph.AddNode(nodeC);
            graph.AddNode(nodeD);
            graph.AddNode(nodeE);
            graph.AddNode(nodeF);
            graph.AddNode(nodeG);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            graph.PutEdge(nodeA, nodeB);
            graph.PutEdge(nodeA, nodeC);
            graph.PutEdge(nodeB, nodeA);
            graph.PutEdge(nodeB, nodeD);
            graph.PutEdge(nodeC, nodeF);
            graph.PutEdge(nodeC, nodeF);
            graph.PutEdge(nodeD, nodeB);
            graph.PutEdge(nodeD, nodeE);
            graph.PutEdge(nodeE, nodeD);
            graph.PutEdge(nodeE, nodeF);
            graph.PutEdge(nodeF, nodeE);
            graph.PutEdge(nodeF, nodeC);
            graph.PutEdge(nodeF, nodeG);
            var path = graph.BreadthFirstSearch(nodeA, nodeG).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA, nodeC, nodeF, nodeG};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        [Test]
        public void DijkstraAlgorithmTest()
        {
            var valueGraph = Graph<int>.Undirected<int>();
            const int nodeA = 1;
            const int nodeB = 2;
            const int nodeC = 0;
            const int nodeD = 3;
            const int nodeE = 4;
            const int nodeF = 5;
            const int nodeG = 7;
            valueGraph.AddNode(nodeA);
            valueGraph.AddNode(nodeB);
            valueGraph.AddNode(nodeC);
            valueGraph.AddNode(nodeD);
            valueGraph.AddNode(nodeE);
            valueGraph.AddNode(nodeF);
            valueGraph.AddNode(nodeG);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            valueGraph.PutEdge(nodeA, nodeB, 1);
            valueGraph.PutEdge(nodeA, nodeC, 1);
            valueGraph.PutEdge(nodeB, nodeA, 1);
            valueGraph.PutEdge(nodeB, nodeD, 1);
            valueGraph.PutEdge(nodeC, nodeF, 1);
            valueGraph.PutEdge(nodeC, nodeF, 1);
            valueGraph.PutEdge(nodeD, nodeB, 1);
            valueGraph.PutEdge(nodeD, nodeE, 1);
            valueGraph.PutEdge(nodeE, nodeD, 1);
            valueGraph.PutEdge(nodeE, nodeF, 1);
            valueGraph.PutEdge(nodeF, nodeE, 1);
            valueGraph.PutEdge(nodeF, nodeC, 1);
            valueGraph.PutEdge(nodeF, nodeG, 1);
            var path = valueGraph.DijkstraAlgorithm(nodeA, nodeG).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA, nodeC, nodeF, nodeG};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }

        [Test]
        public void HeuristicSearchTest()
        {
            var graph = Graph<int>.Undirected();
            const int nodeA = 1;
            const int nodeB = 2;
            const int nodeC = 3;
            const int nodeD = 4;
            const int nodeE = 5;
            const int nodeF = 6;
            const int nodeG = 7;
            graph.AddNode(nodeA);
            graph.AddNode(nodeB);
            graph.AddNode(nodeC);
            graph.AddNode(nodeD);
            graph.AddNode(nodeE);
            graph.AddNode(nodeF);
            graph.AddNode(nodeG);
            /**
             * D - E - F - G
             * |       |
             * B - A - C
             */
            graph.PutEdge(nodeA, nodeB);
            graph.PutEdge(nodeA, nodeC);
            graph.PutEdge(nodeB, nodeA);
            graph.PutEdge(nodeB, nodeD);
            graph.PutEdge(nodeC, nodeF);
            graph.PutEdge(nodeC, nodeF);
            graph.PutEdge(nodeD, nodeB);
            graph.PutEdge(nodeD, nodeE);
            graph.PutEdge(nodeE, nodeD);
            graph.PutEdge(nodeE, nodeF);
            graph.PutEdge(nodeF, nodeE);
            graph.PutEdge(nodeF, nodeC);
            graph.PutEdge(nodeF, nodeG);
            PathFinding.Heuristic<int> method = (a, b) =>
            {
                switch (b)
                {
                    case 1:
                        return 3;
                    case 2:
                        return 4;
                    case 3:
                        return 2;
                    case 4:
                        return 3;
                    case 5:
                        return 2;
                    case 6:
                        return 1;
                    case 7:
                        return 0;
                    default:
                        return -1;
                }
            };
            var path = graph.HeuristicSearch(nodeA, nodeG, method).ToList();
            Console.WriteLine(string.Join(",", path));
            var result = new List<int> {nodeA, nodeC, nodeF, nodeG};
            Console.WriteLine(string.Join(",", result));
            Assert.IsTrue(result.SequenceEqual(path));
        }
    }
}