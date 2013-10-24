using System.Diagnostics;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Attributes;

using log4net;
using StackExchange.Profiling;

namespace FortuneCookie.Aspects.Diagnostics
{
    internal class MiniProfilerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var profiler = MiniProfiler.Current;
            using (profiler.Step(invocation.Method.Name))
            {
                invocation.Proceed();
            }  
        }
    }
}