using System;
using FortuneCookie.Aspects.Logging;

namespace FortuneCookie.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodLogAttribute : Attribute
    {
        private LoggingLevel _cacheLocation = LoggingLevel.Debug;
        
        public LoggingLevel LoggingLevel
        {
            get { return _cacheLocation; }
            set { _cacheLocation = value; }

        }
    }
}