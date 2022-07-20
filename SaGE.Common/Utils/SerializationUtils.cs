using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SaGE.Common.Utils
{
    public class SerializationUtils
    {
        public static string ObjectToString(object instanc, string separator, ObjectToStringTypes type)
        {
            System.Reflection.FieldInfo[] fi = instanc.GetType().GetFields();
            string output = string.Empty;
            if (type == ObjectToStringTypes.Properties || type == ObjectToStringTypes.PropertiesAndFields)
            {
                System.Reflection.PropertyInfo[] properties = instanc.GetType().GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    System.Reflection.PropertyInfo property = properties[i];
                    try
                    {
                        string text = output;
                        output = string.Concat(new string[]
						{
							text, 
							property.Name, 
							":", 
							property.GetValue(instanc, null).ToString(), 
							separator
						});
                    }
                    catch
                    {
                        output = output + property.Name + ": n/a" + separator;
                    }
                }
            }
            if (type == ObjectToStringTypes.Fields || type == ObjectToStringTypes.PropertiesAndFields)
            {
                System.Reflection.FieldInfo[] array = fi;
                for (int i = 0; i < array.Length; i++)
                {
                    System.Reflection.FieldInfo field = array[i];
                    try
                    {
                        output = string.Concat(new string[]
						{
							output, 
							field.Name, 
							": ", 
							field.GetValue(instanc).ToString(), 
							separator
						});
                    }
                    catch
                    {
                        output = output + field.Name + ": n/a" + separator;
                    }
                }
            }
            return output;
        }

        public static bool SerializeObject(object instance, string fileName, bool binarySerialization)
        {
            bool retVal = true;
            if (!binarySerialization)
            {
                XmlWriter writer = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(instance.GetType());
                    System.IO.Stream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = "  ";
                    settings.NewLineChars = "\r\n";
                    settings.OmitXmlDeclaration = true;


                    writer = XmlWriter.Create(fs, settings);
                    //writer.Formatting = Formatting.Indented;
                    //writer.IndentChar = ' ';
                    //writer.Indentation = 3;
                    serializer.Serialize(writer, instance);
                }
                catch (System.Exception ex)
                {
                    Debug.Write("SerializeObject failed with : " + ex.Message, "West Wind");
                    retVal = false;
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            else
            {
                System.IO.Stream fs = null;
                try
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer2 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                    serializer2.Serialize(fs, instance);
                }
                catch
                {
                    retVal = false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return retVal;
        }

        public static bool SerializeObject(object instance, XmlTextWriter writer, bool throwExceptions)
        {
            bool retVal = true;
            try
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());

                writer.Formatting = Formatting.Indented;
                writer.IndentChar = ' ';
                writer.Indentation = 3;
                serializer.Serialize(writer, instance);
            }
            catch (System.Exception ex)
            {
                if (throwExceptions)
                {
                    throw;
                }
                Debug.Write("SerializeObject failed with : " + ex.Message + "\r\n" + ((ex.InnerException != null) ? ex.InnerException.Message : ""), "West Wind");
                retVal = false;
            }
            return retVal;
        }

        public static bool SerializeObject(object instance, out string xmlResultString)
        {
            return SerializationUtils.SerializeObject(instance, out xmlResultString, false);
        }

        public static bool SerializeObject(object instance, out string xmlResultString, bool throwExceptions)
        {
            xmlResultString = string.Empty;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, new System.Text.UTF8Encoding());
            bool result;
            if (!SerializationUtils.SerializeObject(instance, writer, throwExceptions))
            {
                ms.Close();
                result = false;
            }
            else
            {
                xmlResultString = System.Text.Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                ms.Close();
                writer.Close();
                result = true;
            }
            return result;
        }

        public static bool SerializeObject(object instance, out byte[] resultBuffer)
        {
            bool retVal = true;
            System.IO.MemoryStream ms = null;
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ms = new System.IO.MemoryStream();
                serializer.Serialize(ms, instance);
            }
            catch
            {
                retVal = false;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
            resultBuffer = ms.ToArray();
            return retVal;
        }
        public static object DeSerializeObject(string fileName, System.Type objectType, bool binarySerialization)
        {
            return SerializationUtils.DeSerializeObject(fileName, objectType, binarySerialization, false);
        }
        public static object DeSerializeObject(string fileName, System.Type objectType, bool binarySerialization, bool throwExceptions)
        {
            object instance = null;
            object result;
            if (!binarySerialization)
            {
                XmlReader reader = null;
                System.IO.FileStream fs = null;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(objectType);
                    fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                    reader = new XmlTextReader(fs);
                    instance = serializer.Deserialize(reader);
                }
                catch (System.Exception ex)
                {
                    if (throwExceptions)
                    {
                        throw;
                    }
                    string message = ex.Message;
                    result = null;
                    return result;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            else
            {
                System.IO.FileStream fs = null;
                try
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer2 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
                    instance = serializer2.Deserialize(fs);
                }
                catch
                {
                    result = null;
                    return result;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            result = instance;
            return result;
        }
        public static object DeSerializeObject(XmlReader reader, System.Type objectType)
        {
            XmlSerializer serializer = new XmlSerializer(objectType);
            object Instance = serializer.Deserialize(reader);
            reader.Close();
            return Instance;
        }
        public static object DeSerializeObject(string xml, System.Type objectType)
        {
            XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);
            return SerializationUtils.DeSerializeObject(reader, objectType);
        }
        public static object DeSerializeObject(byte[] buffer, System.Type objectType)
        {
            System.IO.MemoryStream ms = null;
            object Instance = null;
            object result;
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ms = new System.IO.MemoryStream(buffer);
                Instance = serializer.Deserialize(ms);
            }
            catch
            {
                result = null;
                return result;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }
            result = Instance;
            return result;
        }
    }
}
