using System.Diagnostics;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Attributes;
using log4net;

namespace FortuneCookie.Aspects.Logging
{
    internal class MethodLogInterceptor : IInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Intercept(IInvocation invocation)
        {
            MethodLogAttribute attribute = ReflectionHelper.GetCustomAttribute<MethodLogAttribute>(invocation.Method);
            LoggingLevel loggingLevel = attribute.LoggingLevel;

            string invocationMessage = Helper.InvocationHelper.InvocationNameAndArguments(invocation);
            Debug.WriteLine(invocationMessage);
            WriteLogEntry(invocationMessage, loggingLevel);
            
            invocation.Proceed();

            string returnValue = string.Format("returns: {0}", invocation.ReturnValue);
            Debug.WriteLine(returnValue);
            WriteLogEntry(returnValue, loggingLevel);
        }


        private static void WriteLogEntry(string message, LoggingLevel loggingLevel)
        {
            switch (loggingLevel)
            {
                case LoggingLevel.Debug:
                    Logger.Debug(message);
                    break;
                case LoggingLevel.Error:
                    Logger.Error(message);
                    break;
                case LoggingLevel.Fatal:
                    Logger.Fatal(message);
                    break;
                case LoggingLevel.Info:
                    Logger.Info(message);
                    break;
                case LoggingLevel.Warn:
                    Logger.Warn(message);
                    break;
            }
        }
    }
}