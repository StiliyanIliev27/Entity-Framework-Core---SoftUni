using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new(rootName);
            XmlSerializer xmlSerializer = new(typeof(T), xmlRootAttribute);

            StringReader stringReader = new(inputXml);
            T deserializedDtos = (T)xmlSerializer.Deserialize(stringReader)!;

            return deserializedDtos;
        }

        //May not be used.
        //Syntax sugar
        public IEnumerable<T> DeserializeCollection<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRootAttribute = new(rootName);
            XmlSerializer xmlSerializer = new(typeof(T[]), xmlRootAttribute);

            StringReader stringReader = new(inputXml);
            T[] deserializedDtos = (T[])xmlSerializer.Deserialize(stringReader)!;

            return deserializedDtos;
        }

        public string Serialize<T>(T obj, string rootName)
        {
            StringBuilder sb = new();

            XmlRootAttribute xmlRootAttribute = new(rootName);
            XmlSerializer xmlSerializer = new(typeof(T), xmlRootAttribute);

            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new(sb);
            xmlSerializer.Serialize(stringWriter, obj, namespaces);

            return sb.ToString().TrimEnd();
        }

        public string SerializeCollection<T>(T[] obj, string rootName)
        {
            StringBuilder sb = new();

            XmlRootAttribute xmlRootAttribute = new(rootName);
            XmlSerializer xmlSerializer = new(typeof(T[]), xmlRootAttribute);

            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);

            using StringWriter stringWriter = new(sb);
            xmlSerializer.Serialize(stringWriter, obj, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
