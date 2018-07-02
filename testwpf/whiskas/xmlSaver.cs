using System.Xml.Serialization;
using System.IO;

namespace testwpf.whiskas
{
   class XmlSaver
   {
      public static void Save(string filename, object obj)
      {
         XmlSerializer xs = new XmlSerializer(obj.GetType());
         Stream writer = new FileStream(filename, FileMode.Create);
         xs.Serialize(writer, obj);
         writer.Close();
      }

      public static object Read(string filename)
      {
         XmlSerializer xs = new XmlSerializer(typeof( object ));
         if ( File.Exists(filename) )
         {
            Stream reader = new FileStream(filename, FileMode.Open);
            object to = xs.Deserialize(reader);
            reader.Close();
            return to;
         }

         return null;
         
      }

   }
}
