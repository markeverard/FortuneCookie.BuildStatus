using System.Reflection;
using Castle.DynamicProxy;

namespace FortuneCookie.Aspects.Helper
{
    public static class InvocationHelper
    {
        /// <summary>
        /// Returns a list of parameter info objects used in the call to the invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        public static ParameterInfo[] MethodParameters(IInvocation invocation)
        {
            return invocation.Method.GetParameters();
        }

        /// <summary>
        /// Returns a string containing the invocations argument names and values.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        public static string InvocationNameAndArguments(IInvocation invocation)
        {
            string key = string.Format("{0}_{1}", invocation.TargetType.Name, invocation.Method.Name);
            ParameterInfo[] parameters = MethodParameters(invocation);

            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = parameters[i].Name;
                object argument = invocation.Arguments[i];
                key += string.Format("_{0}_{1}",
                                     parameterName, argument == null ? string.Empty : argument.ToString());
            }

            return key;
        }
    }
}