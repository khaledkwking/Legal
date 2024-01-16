using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Utilities
{
   public  class ReadFromXML
    {
        public static string ReadXMLByName(string strInput, string strXmlPath)
        {
            XDocument ReadXML = XDocument.Load(strXmlPath);
            var XMLElement = from Req in ReadXML.Descendants(strInput)
                             select Req;

            string strUrl = XMLElement.FirstOrDefault().FirstNode.ToString();

            return strUrl;
        }

    }
}
