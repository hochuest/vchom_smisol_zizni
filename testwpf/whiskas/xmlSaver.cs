using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace testwpf.whiskas
{
   static class XmlSaver
   {
      public async static void Save<T>(string fileName, T obj)
      {
         var serializer = new XmlSerializer(typeof(T));
         using (var stream = File.Create(fileName))
         {
            await Task.Factory.StartNew(() => { serializer.Serialize(stream, obj); });
         }
      }

      public static object Read(string fileName, System.Type type )
      {
         var serializer = new XmlSerializer(type);
         using (var stream = File.OpenRead(fileName))
         {
            return (object)(serializer.Deserialize(stream));
         }
      }
   }
}
