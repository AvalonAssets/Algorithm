using System;
using System.Linq.Expressions;
using System.Threading;

namespace AvalonAssets.Core
{
    public class Lazy<T>
    {
        private readonly Func<T> _valueFactory;
        private Boxed _boxedValue;

        public Lazy()
        {
            var type = typeof(T);
            if (!type.IsValueType && type.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException("T is not value type and does not have a default constructor.");
            _valueFactory = Expression.Lambda<Func<T>>(Expression.New(type)).Compile();
        }

        public Lazy(Func<T> factoryMethod)
        {
            _valueFactory = factoryMethod;
        }

        public T Value
        {
            get
            {
                if (_boxedValue == null)
                    Interlocked.CompareExchange(ref _boxedValue, new Boxed(_valueFactory()), null);
                return _boxedValue.Value;
            }
        }

        private class Boxed
        {
            internal readonly T Value;

            internal Boxed(T value)
            {
                Value = value;
            }
        }
    }
}