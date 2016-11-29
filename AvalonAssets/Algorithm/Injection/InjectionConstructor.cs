﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AvalonAssets.Algorithm.Injection.Exception;

namespace AvalonAssets.Algorithm.Injection
{
    /// <summary>
    ///     <see cref="IInjectionConstructor" /> using normal constructor.
    /// </summary>
    public class InjectionConstructor : IInjectionConstructor
    {
        private readonly ConstructorInfo _constructor;

        /// <summary>
        ///     Creates a <see cref="IInjectionConstructor" /> using normal constructor.
        /// </summary>
        /// <param name="constructor">Desire constructor.</param>
        public InjectionConstructor(ConstructorInfo constructor)
        {
            if (constructor == null)
                throw new ArgumentNullException("constructor");
            _constructor = constructor;
        }

        public object NewInstance(IContainer container, IDictionary<string, object> arguments)
        {
            var paramsInfoList = _constructor.GetParameters().ToList();
            var resolvedParams = new List<object>();
            foreach (var paramsInfo in paramsInfoList)
            {
                object value;
                // Uses given arguments if possible.
                if (arguments.ContainsKey(paramsInfo.Name))
                    value = arguments[paramsInfo.Name];
                else
                {
                    try
                    {
                        // Asks container to resolve type.
                        value = container.Resolve(paramsInfo.ParameterType);
                    }
                    catch (TypeNotRegisteredException)
                    {
                        // Falls back to defualt value if any.
                        if (paramsInfo.HasDefaultValue)
                            value = paramsInfo.RawDefaultValue;
                        else
                            throw;
                    }
                }
                resolvedParams.Add(value);
            }
            return _constructor.Invoke(resolvedParams.ToArray());
        }
    }
}