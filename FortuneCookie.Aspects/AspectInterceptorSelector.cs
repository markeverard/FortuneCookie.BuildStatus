using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Attributes;
using FortuneCookie.Aspects.Caching;
using FortuneCookie.Aspects.Diagnostics;
using FortuneCookie.Aspects.Logging;
using FortuneCookie.Aspects.Serialization;

namespace FortuneCookie.Aspects
{
    internal class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo methodInfo, IInterceptor[] interceptors)
        {
            IEnumerable<IInterceptor> localInterceptors = interceptors;

            if (!ReflectionHelper.HasCustomAttribute<MethodCacheAttribute>(methodInfo))
                localInterceptors = localInterceptors.Where(i => !(i is MethodCacheInterceptor));

            if (!ReflectionHelper.HasCustomAttribute<MethodLogAttribute>(methodInfo))
                localInterceptors = localInterceptors.Where(i => !(i is MethodLogInterceptor));

            if (!ReflectionHelper.HasCustomAttribute<MethodXmlSerializeAttribute>(methodInfo))
                localInterceptors = localInterceptors.Where(i => !(i is MethodXmlSerializeInterceptor));

            if (!ReflectionHelper.HasCustomAttribute<MethodDiagnosticsAttribute>(methodInfo))
                localInterceptors = localInterceptors.Where(i => !(i is MethodDiagnosticsInterceptor));

            if (!ReflectionHelper.HasCustomAttribute<MiniProfilerAttribute>(methodInfo))
                localInterceptors = localInterceptors.Where(i => !(i is MiniProfilerInterceptor));

            return localInterceptors.ToArray();
        }
    }
}