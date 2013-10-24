using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Castle.DynamicProxy;
using log4net;

namespace FortuneCookie.Aspects.Serialization
{
    internal class MethodXmlSerializeInterceptor : IInterceptor
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //TODO Proper implementation and testing - this is a work in progress
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            Type returnType = invocation.Method.ReturnType;

            var serializedObject = SerializeObject(invocation.ReturnValue, returnType);
            Debug.WriteLine(serializedObject);
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The object as a string
        /// </returns>
        private string SerializeObject(object obj, Type type)
        {
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                XmlSerializer xs = new XmlSerializer(type);
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    xs.Serialize(xmlTextWriter, obj);
                    memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
                }

                return Utf8ByteArrayToString(memoryStream.ToArray());
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return string.Empty;
            }
            finally
            {
                memoryStream.Close();
            }
        }

        /// <summary>
        /// Gets a string from the specified character array
        /// Ghostdoc: UTF8s the byte array to string.
        /// </summary>
        /// <param name="characters">The characters.</param>
        /// <returns>A string from the specified byte array</returns>
        private string Utf8ByteArrayToString(byte[] characters)
        {
            return Encoding.UTF8.GetString(characters);
        }
    }
}