using System;

namespace FortuneCookie.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MethodDiagnosticsAttribute : Attribute
    {
        public static bool DefaultLogAsInfo = true;
        private bool? _defaultLogAsInfo;
       
        public bool LogAsInfo
        {
            get
            {
                return _defaultLogAsInfo.HasValue ? _defaultLogAsInfo.Value : DefaultLogAsInfo;
            }
            set { _defaultLogAsInfo = value; }
        }
    }
}