using System;

namespace FortuneCookie.Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MiniProfilerAttribute : Attribute
    {
        
    }
}