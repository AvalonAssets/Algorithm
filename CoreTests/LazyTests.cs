using System;
using NUnit.Framework;

namespace AvalonAssets.CoreTests
{
    [TestFixture]
    public class LazyTests
    {
        // ReSharper disable once ClassNeverInstantiated.Local
        private class DummyTestClass
        {
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class DummyTestClass2
        {
            private DummyTestClass2(int foo)
            {
            }
        }

        [Test]
        public void LazyClassTest()
        {
            Assert.DoesNotThrow(() =>
            {
                var lazy = new Core.Lazy<DummyTestClass>();
            });
            Assert.Throws<ArgumentException>(() =>
            {
                var lazy = new Core.Lazy<DummyTestClass2>();
            });
        }

        [Test]
        public void LazyValueTest()
        {
            var lazyValue = new Core.Lazy<int>();
            Assert.AreEqual(default(int), lazyValue.Value);
        }
    }
}