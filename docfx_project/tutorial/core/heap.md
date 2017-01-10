# Heap
*Core* provides some implementations structure of heap.

## Getting Started
First, you have to implement `IComparer<T>`. For `IComparable<T>`, you can use `Comparer<T>.Default`.

```csharp
var data = new  []{1, 2, 3, 7, 4, 5};
var minHeap = new BinaryHeap<int>(Comparer<int>.Default);
minHeap.Insert(2);
var minNode = minHeap.ExtractMin();
var minValue = minNode.Value; // return 1
```

## Time Complexity
The follow time complexities is assuming the heap is a min-heap. Max-heap have the same complexity for maximum instead of minimum

### Binary Heap
* Bulid heap: ***O(n)***
* Insert: ***O(log n)***
* Extract minimum: ***O(log n)***
* Find minimum: ***Θ(1)***
* Decrease key: ***O(log n)***
* Delete: ***O(log n)***

### Binomial Heap
* Bulid heap: ***O(n log n)***
* Insert: ***O(log n)***
* Extract minimum: ***Θ(log n)***
* Find minimum: ***O(log n)***
* Decrease key: ***Θ(log n)***
* Delete: ***Θ(log n)***

### Fibonacci Heap
* Bulid heap: ***Θ(n)***
* Insert: ***Θ(1)***
* Extract minimum: ***O(log n)***
* Find minimum: ***Θ(1)***
* Decrease key: ***Θ(1)***
* Delete: ***O(log n)***

## Comparer Wrapper
`ComparerWrapper<T>` provides a simple wrapper if you do not want to `IComparable<T>` for somme reasons. `ComparerWrapper<T>` can be created by `Comparsion<T>` where it is a delegate thats compare two values.

```csharp
var comparer = new ComparerWrapper<int>((x, y) => x - y);
```

## Reverse Comparer Order
To reverse the order, you can use `IComparer<T>.Reverse()`.

```csharp
var reversedComparer = comparer.Reverse();
```
