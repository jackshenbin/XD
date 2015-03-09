using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol.Model;
using System.Xml;

namespace BOCOM.IVX.Service
{
    public class CarBrandInfoService
    {
        private readonly static string s_Folder = @"\resources\carbrand\";

        private readonly static string s_ConfigFile = @"\resources\carbrand\carstyle.xml";

        
        private XmlNode[] SortCarBrand(XmlNodeList elemList)
        {
            string name;
            SortedDictionary<string, XmlNode> dict = new SortedDictionary<string,XmlNode>();
            foreach (XmlNode node in elemList)
            {
                name = node["name"].InnerText;
                if (!string.IsNullOrEmpty(name) && !dict.ContainsKey(name))
                {
                    dict.Add(name, node);
                }
            }
            XmlNode[] nodes = new XmlNode[dict.Count];
            dict.Values.CopyTo(nodes, 0);
            
            return nodes;
        }

        public List<CarBrandInfo> GetAllCarBrandInfos()
        {
            List<CarBrandInfo> carBrandInfos = new List<CarBrandInfo>();

             XmlDocument doc = new XmlDocument();
            XmlReader reader = null;
            XmlReaderSettings settings = new XmlReaderSettings();

            settings.ValidationType = ValidationType.DTD;

            reader = XmlReader.Create(BOCOM.IVX.Framework.Environment.CurrentDirectory + s_ConfigFile, settings);

            doc.Load(reader);
            XmlNodeList elemList = doc.GetElementsByTagName("item");
            XmlNode[] sortedNodes = SortCarBrand(elemList);

            foreach (XmlNode node in sortedNodes)
            {
                System.IO.FileInfo f = new System.IO.FileInfo(BOCOM.IVX.Framework.Environment.CurrentDirectory + s_Folder + node["file"].InnerText);
                DevExpress.XtraBars.Ribbon.GalleryItem galleryItem2 = new DevExpress.XtraBars.Ribbon.GalleryItem();
                galleryItem2.Caption = node["name"].InnerText.ToString();
            }

            return carBrandInfos;
        }
    }
}
