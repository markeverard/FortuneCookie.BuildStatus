using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace FortuneCookie.Aspects.Caching
{
    public class CacheKeyHelper
    {
        private string GetCacheKey(string methodName, object requestObject)
        {
            var sb = new StringBuilder();
            sb.Append(methodName);
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