using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using MaterialDesignThemes.Wpf;
using System.Windows.Shapes;

namespace GCManagementApp.Helpers
{
    public class XmlGenericSerializer<T> where T : class
    {
        public static string Serialize(T obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true };
                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    xsSubmit.Serialize(writer, obj);
                    return sww.ToString();
                }
            }
        }

        public static T Deserialize(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(path))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T DeserializeFromStream(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
