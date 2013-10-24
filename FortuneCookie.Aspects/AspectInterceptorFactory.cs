using System;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Caching;
using FortuneCookie.Aspects.Diagnostics;
using FortuneCookie.Aspects.Logging;
using FortuneCookie.Aspects.Serialization;

namespace FortuneCookie.Aspects
{
    public class AspectInterceptorFactory
    {
        private readonly ProxyGenerator _generator;
        private readonly ProxyGenerationOptions _options;
        private readonly IInterceptor[] _interceptors;
        private readonly IInterceptorSelector _selector; 

        public AspectInterceptorFactory()
        {
            _generator = new ProxyGenerator();
            _selector = new AspectInterceptorSelector();
            _options = new ProxyGenerationOptions(new ProxyGenerationHook()) { Selector = _selector };
            _interceptors = new IInterceptor[] { new MethodDiagnosticsInterceptor(), new MethodXmlSerializeInterceptor(), new MethodLogInterceptor(), new MiniProfilerInterceptor(), new MethodCacheInterceptor() };
        }

        public virtual T CreateInstance<T>()
        {
            return CreateInstance<T>(new object[] {});
        }

        public virtual T CreateInstance<T>(object[] constructorArguments)
        {
            return (T)_generator.CreateClassProxy(typeof(T), new Type[] { }, _options, constructorArguments, _interceptors);   
        }
    } 
}