using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FortuneCookie.Aspects.Helper
{
    /// <summary>
    /// Helper class to calculate equality of reference objects using reflection (as opposed to overriding Equals and GetHashCode)
    /// </summary>
    public static class EqualityHelper
    {
        private const BindingFlags _bindingFlags = BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// returns a string representing the current value of a reference object - does not include collections in the hash.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ValueTypeHashCode(object obj)
        {
            Type objType = obj.GetType();

            FieldInfo[] fields = objType.GetFields(_bindingFlags);

            fields = ConcatInheritedFields(fields, objType);

            var sb = new StringBuilder();

            foreach (var field in fields)
            {
                var fieldValue = field.GetValue(obj);
                if (fieldValue == null)
                    continue;

                var inheritsFromValueType = FieldTypeIsValueTypeOrString(field.FieldType);


                if (inheritsFromValueType)
                {
                    sb.Append(fieldValue);
                    sb.Append(";");
                }
                else
                {
                    sb.Append(ValueTypeHashCode(fieldValue));
                }
            }

            return sb.ToString();
        }

        private static FieldInfo[] ConcatInheritedFields(FieldInfo[] fields, Type objType, int maxRecursionDepth = 0)
        {
            if (maxRecursionDepth > 2)
                return fields;
            
            Type baseType = objType.BaseType;

            if (baseType == null)
                return fields;

            FieldInfo[] inheritedFields = baseType.GetFields(_bindingFlags);
            fields = fields.Concat(inheritedFields).ToArray();
            return ConcatInheritedFields(fields, baseType, maxRecursionDepth + 1);
        }

        /// <summary>
        /// Is the property type a Value type or a string
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns></returns>
        internal static bool FieldTypeIsValueTypeOrString(Type fieldType)
        {
            return fieldType.IsSubclassOf(typeof(ValueType))
                || fieldType.IsAssignableFrom(typeof(string));
        }


        /// <summary>
        /// Determines whether reference objects have equal values via reflection - does not include collections in the calculation.
        /// </summary>
        /// <param name="a">object</param>
        /// <param name="b">object to compare</param>
        /// <returns>
        ///   <c>true</c> if [is value type equal] [the specified a]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueTypeEqual(object a, object b)
        {
            if (a == null || b == null)
                return false;

            if (a.GetType() != b.GetType())
                return false;

            return ValueTypeHashCode(a) == ValueTypeHashCode(b);
        }
    
        
    }
}