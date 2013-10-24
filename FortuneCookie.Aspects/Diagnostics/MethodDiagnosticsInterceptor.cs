using System.Diagnostics;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Attributes;
using log4net;

namespace FortuneCookie.Aspects.Diagnostics
{
    internal class MethodDiagnosticsInterceptor : IInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public void Intercept(IInvocation invocation)
        {
            var attribute = ReflectionHelper.GetCustomAttribute<MethodDiagnosticsAttribute>(invocation.Method);
            bool logAsInfo = attribute.LogAsInfo;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            invocation.Proceed();
            stopwatch.Stop();

            
            string timeTaken = string.Format("{0} : {1}ms", invocation.Method.Name, stopwatch.ElapsedMilliseconds);
            
            if (logAsInfo)
                Logger.Info(timeTaken);
            
            Debug.WriteLine(timeTaken);
        }

    }
}