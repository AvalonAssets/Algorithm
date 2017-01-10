# Event Aggregator
## Introduction
Event aggregator is used for event boardcasting and decoupling of publishers and subscribers so they do not need to know each other.
Consider observer pattern, subscribers need to know who they are subscribing to. For example, if *A* wants to observe changes of *B*, *A* must know *B*.
This leads to some problems. Increasing number of observers and observees scales up complexity quickly.

Another problem is subscribers do not know who to observe. For example, *Police* wants to get notice when *Citizen* reports *Crime*.
However, it is impossible for every *Police* to oberve every *Citizen* and wait for them to reports *Crime* because if there is a new *Citizen* every *Police* have to observe it.
This can be solve by letting *Citizen* reports to *Police Station* intead and *Police* only need to observe *Police Station*. *Police Station* is event aggregator.

Event aggregator centralizes the event publishing. Publishers and subscribers never know each other. Thus, even one of them changes, it will not affect the other one.

## Getting Started
After understanding the reasons to use event aggregator, let's get started.

To use event aggregator, add `ISubscriber<T>` to the subscriber class where `T` is the event type you want to observe. You have to implement `ISubscriber<T>.Receive` to handle the event you received. It is recommend to implement it **explicitly** to avoid name conflict and calling it directly.

> `ISubscriber<T>` receives all events of type T and its super class.

```csharp
public class CustomSubscriber : ISubscriber<int>
{
    void ISubscriber<int>.Receive(int message)
    {
        Console.WriteLine("I received " + message);
    }
}
```
### Initialization
Then, you have to create a `IEventAggregator`.

```csharp
var eventAggregator = new EventAggregatorBuilder().Build();
```

### Subscribe
Add your `ISubscriber<T>` to `IEventAggregator`.

```csharp
var subscriber = new CustomSubscriber();
eventAggregator.Subscribe(subscriber);
```

### Publish
After that whenever the `eventAggregator` is called by `IEventAggregator.Publish`, `subscriber` will receive the event. To publish a event,

```csharp
eventAggregator.Publish(3);
```

### Unsubscribe
You can also unsubscribe when you do not want to receive any events anymore.

```csharp
eventAggregator.Unsubscribe(subscriber);
```

## Advanced
### Customization
Customizing `EventAggregator` requires `IEventHandlerFactory` and `IEventHandler`. By default, it uses `ReflectEventHandlerFactory`. You can also implement your `IEventAggregator` instead.

```csharp
var eventAggregator = new EventAggregatorBuilder().SetEventHandlerFactory(new CustomEventHandlerFactory()).Build();
```
