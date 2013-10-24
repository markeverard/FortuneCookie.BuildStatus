using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace FortuneCookie.Aspects
{
    internal class ProxyGenerationHook : IProxyGenerationHook
    {
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return true;
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public void MethodsInspected()
        {
        }

        public override bool Equals(object obj)
        {
            return ((obj != null) && (obj.GetType() == typeof(ProxyGenerationHook)));
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}