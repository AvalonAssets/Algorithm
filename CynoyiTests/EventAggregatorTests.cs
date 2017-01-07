using AvalonAssets.Cynoyi;
using NUnit.Framework;

namespace AvalonAssets.CynoyiTests
{
    [TestFixture]
    public class EventAggregatorTests : ISubscriber<int>
    {
        [SetUp]
        public void Setup()
        {
            _value = 0;
        }

        private int _value;

        void ISubscriber<int>.Receive(int message)
        {
            _value = message;
        }

        [Test]
        public void LambdaTest()
        {
            Test(new EventAggregatorBuilder().SetEventHandlerFactory(new LambdaEventHandlerFactory()).Build());
        }

        [Test]
        public void ReflectionTest()
        {
            Test(new EventAggregatorBuilder().SetEventHandlerFactory(new ReflectEventHandlerFactory()).Build());
        }
        
        private void Test(IEventAggregator aggregator)
        {
            aggregator.Publish(1);
            Assert.AreEqual(0, _value);
            aggregator.Subscribe(this);
            aggregator.Publish(2);
            Assert.AreEqual(2, _value);
            aggregator.Unsubscribe(this);
            aggregator.Publish(3);
            Assert.AreEqual(2, _value);
        }
    }
}