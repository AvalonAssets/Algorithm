using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AvalonAssets.Cynoyi;
using NUnit.Framework;

namespace AvalonAssets.CynoyiTests
{
    [TestFixture]
    public class EventAggregatorTests : ISubscriber<int>
    {
        private int _value;

        void ISubscriber<int>.Receive(int message)
        {
            _value = message;
        }

        private class DummyObject : ISubscriber<int>
        {
            public void Receive(int message)
            {
            }
        }

        private class TestEventHandlerFactory : IEventHandlerFactory
        {
            public IEventHandler Create(ISubscriber subscriber)
            {
                return new TestEventHandler(subscriber);
            }
        }

        private class TestEventHandler : IEventHandler
        {
            private readonly Dictionary<Type, MethodInfo> _supportedHandlers;
            private readonly WeakReference _weakReference;

            public TestEventHandler(object subscriber)
            {
                _weakReference = new WeakReference(subscriber);
                _supportedHandlers = new Dictionary<Type, MethodInfo>();
                // Gets all the ISubscriber<T> interface
                var interfaces = subscriber.GetType().GetInterfaces()
                    .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
                foreach (var @interface in interfaces)
                {
                    var type = @interface.GetGenericArguments()[0];
                    var method = @interface.GetMethod("Receive");
                    _supportedHandlers[type] = method;
                }
            }

            public bool Alive => _weakReference.Target != null;
            public IEnumerable<Type> Types => _supportedHandlers.Keys;

            public bool Matches(object instance)
            {
                return _weakReference.Target == instance;
            }

            public bool Handle(Type messageType, object message)
            {
                if (!Alive)
                    return false;
                var loop = 10;
                var sw = new Stopwatch();
                sw.Start();
                for (var i = 0; i < loop; i++)
                {
                    _supportedHandlers[messageType].Invoke(_weakReference.Target, new[] {message});
                }
                sw.Stop();
                Console.WriteLine($"Elapsed={sw.Elapsed}");
                sw.Reset();
                var gtype = typeof(ISubscriber<>).MakeGenericType(messageType);
                var method = gtype.GetMethod("Receive", BindingFlags.Instance | BindingFlags.Public);
                var instExpr = Expression.Parameter(typeof(object), "instance");
                var paramExpr = Expression.Parameter(typeof(object), "message");
                var instCastExpr = Expression.Convert(instExpr, typeof(ISubscriber<>).MakeGenericType(messageType));
                var castExpr = Expression.Convert(paramExpr, messageType);
                var invokeExpr = Expression.Call(instCastExpr, method, castExpr);
                var ifExpr = Expression.IfThen(Expression.TypeIs(paramExpr, messageType), invokeExpr);
                var lambda = Expression.Lambda<Action<object, object>>(ifExpr, instExpr, paramExpr);
                var compiled = lambda.Compile();
                sw.Start();
                for (var i = 0; i < loop; i++)
                {
                    compiled(_weakReference.Target, message);
                }
                sw.Stop();
                Console.WriteLine($"Elapsed={sw.Elapsed}");
                return true;
            }
        }

        [Test]
        public void Test()
        {
            var aggregator = new EventAggregatorBuilder().SetEventHandlerFactory(new TestEventHandlerFactory()).Build();
            aggregator.Publish(1);
            Assert.AreEqual(0, _value);
            aggregator.Subscribe(this);
            aggregator.Publish(2);
            Assert.AreEqual(2, _value);
            aggregator.Unsubscribe(this);
            aggregator.Publish(3);
            Assert.AreEqual(2, _value);
        }

        [Test]
        public void Test2()
        {
            var aggregator = new EventAggregatorBuilder().Build();
            var list = new List<DummyObject>();
            for (var i = 0; i < 10000; i++)
            {
                var obj = new DummyObject();
                list.Add(obj);
                aggregator.Subscribe(obj);
            }
            var sw = new Stopwatch();
            sw.Start();
            aggregator.Publish(1);
            sw.Stop();
            Console.WriteLine($"Elapsed={sw.Elapsed}");
        }

        [Test]
        public void Test3()
        {
            var aggregator = new EventAggregatorBuilder().SetEventHandlerFactory(new TestEventHandlerFactory()).Build();
            var list = new List<DummyObject>();
            for (var i = 0; i < 10000; i++)
            {
                var obj = new DummyObject();
                list.Add(obj);
                aggregator.Subscribe(obj);
            }
            var sw = new Stopwatch();
            sw.Start();
            aggregator.Publish(1);
            sw.Stop();
            Console.WriteLine($"Elapsed={sw.Elapsed}");
        }
    }
}