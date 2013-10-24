using System;
using System.Linq;
using System.Reflection;

namespace FortuneCookie.Aspects
{
    internal static class ReflectionHelper
    {
        public static bool HasCustomAttribute<T>(MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttributes(typeof(T), true).Length > 0;
        }

        public static T GetCustomAttribute<T>(MethodInfo methodInfo) where T : Attribute
        {
            return (T)methodInfo.GetCustomAttributes(typeof (T), true).FirstOrDefault();
        }
    }
}
