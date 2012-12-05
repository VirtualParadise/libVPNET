using System;
using System.IO;
using System.Xml.Serialization;

namespace VPNetExamples.Common
{
    public sealed class SerializationHelpers
    {
        public static T Deserialize<T>(FileInfo fi) where T : new()
        {
            if (!fi.Exists)
                return new T();
            var sr = new StreamReader(fi.FullName);
            string message = sr.ReadToEnd();
            sr.Close();
            return Deserialize<T>(message, false);
        }

        public static T Deserialize<T>(string message, bool appendXmlHeader)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (appendXmlHeader)
                message = "<?xml version=\"1.0\"?>" + message;
            using (var mem = new MemoryStream())
            {
                byte[] a = System.Text.Encoding.Unicode.GetBytes(message);
                mem.Write(a, 0, a.Length);
                mem.Position = 0;
                var ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(mem);
            }
        }

        public static void Serialize<T>(T objectToSerialize, FileInfo fi) where T : class
        {
            var message = Serialize(objectToSerialize);
            var sw = new StreamWriter(fi.FullName);
            sw.Write(message);
            sw.Close();
        }

        public static string Serialize<T>(T objectToSerialize) where T : class
        {
            if (objectToSerialize == null) throw new ArgumentNullException("objectToSerialize");

            using (var mem = new MemoryStream())
            {
                var x = new XmlSerializer(objectToSerialize.GetType());
                x.Serialize(mem, objectToSerialize);
                var array = new byte[mem.Length];
                mem.Position = 0;
                mem.Read(array, 0, (int)mem.Length);
                return System.Text.Encoding.UTF8.GetString(array);
            }
        }
    }
}
