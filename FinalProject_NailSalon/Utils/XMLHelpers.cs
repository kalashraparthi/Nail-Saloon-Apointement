using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FinalProject_NailSalon
{
    class XMLHelpers
    {
        private static NailSalonXMLDAO nailSalonXMLDAOInstance;
        public static void ReadFromXML(string path)
        {
            TextReader textReader = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(NailSalonXMLDAO));

                textReader = new StreamReader(path);
                nailSalonXMLDAOInstance = (NailSalonXMLDAO)xmlSerializer.Deserialize(textReader);
            } catch(Exception e)
            {
                textReader.Close();
                throw new Exception("Failed to loading data file."+ e.InnerException.Message);
            } finally
            {
                textReader.Close();
            }

        }

        public static void WriteToXML(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NailSalonXMLDAO));
            TextWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, nailSalonXMLDAOInstance);

            writer.Close();
        }

        public static NailSalonXMLDAO Instance()
        {
            if(nailSalonXMLDAOInstance == null)
            {
                return new NailSalonXMLDAO();
            }

            return nailSalonXMLDAOInstance;
        }
    }
}
