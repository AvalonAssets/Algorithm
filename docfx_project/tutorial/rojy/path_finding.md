# Path Finding
# Introduction
This is a common usually technique in programming. `PathFinding` provide the commonly used implementations for `IGraph<TNode>` and `IValueGraph<TNode, TValue>`.

`IGraph<TNode>`:
* Breadth First Search
* Heuristic Search

`IValueGraph<TNode, TValue>`:
* Dijkstra's Algorithm
* A* Algorithm 

If you have a custom graph class, you can implement either `IGraph<TNode>` or `IValueGraph<TNode>`. It is recommend to implement the interface explicitly. Or you can use `Graph<TNode>` or ` ValueGraph<TNode, TValue>` instead.

## Getting Started
First, you have to construct your graph.

```csharp
/**
    * D - E - F - G
    * |       |
    * B - A - C
    */
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
```

Then, you can use the path finding.

```csharp
var path = graph.BreadthFirstSearch(nodeA, nodeG);
```