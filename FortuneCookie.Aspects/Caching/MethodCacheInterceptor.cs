using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using FortuneCookie.Aspects.Attributes;
using FortuneCookie.Aspects.Helper;

namespace FortuneCookie.Aspects.Caching
{
    internal class MethodCacheInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.ReturnParameter == null || invocation.Method.ReturnParameter.ParameterType == typeof(void))
            {
                invocation.Proceed();
                return;
            }

            var attribute = ReflectionHelper.GetCustomAttribute<MethodCacheAttribute>(invocation.Method);
            
            string key = GetCacheKey(invocation);
            object value = CacheHelper.GetFromCache(key, attribute);

            if (value != null)
            {
                invocation.ReturnValue = value;
                Debug.WriteLine(string.Format("> {0} returned cached value from {1} : key {2}", invocation.Method.Name, attribute.CacheLocation.ToString(), key));
            }
            else
            {
                invocation.Proceed();
                Debug.WriteLine(string.Format("> {0} called and did not return from cache with key {1}", invocation.Method.Name, key));
                value = invocation.ReturnValue;

                if (value != null)
                    CacheHelper.AddToCache(key, value, attribute);
            }
        }

        private string GetCacheKey(IInvocation invocation)
        {
            var sb = new StringBuilder();
            sb.Append(invocation.Method.Name);
            sb.Append(";");

            ParameterInfo[] parameters = InvocationHelper.MethodParameters(invocation);
            
            foreach (var parameter in parameters)
            {
                var objectValue = invocation.GetArgumentValue(parameter.Position);
                string valueHash = EqualityHelper.ValueTypeHashCode(objectValue);
                sb.Append(valueHash);
            }

            return sb.ToString();
            //return InvocationHelper.InvocationNameAndArguments(invocation);
        }
    }
}