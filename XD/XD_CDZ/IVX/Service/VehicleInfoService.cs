using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.DataModel;
using System.Xml;

namespace BOCOM.IVX.Service
{
    public class VehicleInfoService
    {
        private readonly static VehicleBrandInfo S_VEHICLEBRANDINFO_NOTSPECIFIED = new VehicleBrandInfo { ID = -1, Name = "不限" };
        private readonly static VehicleBrandInfo S_VEHICLEBRANDINFO_OTHER = new VehicleBrandInfo { ID = 1000, Name = "其它" };
        
        private List<VehicleBrandInfo> m_BrandInfos;

        private SortedDictionary<int, VehicleBrandInfo> m_DTID2BrandInfo;

        private void RetrieveBrandInfos()
        {
            m_DTID2BrandInfo = new SortedDictionary<int, VehicleBrandInfo>();
            XmlDocument doc = new XmlDocument();
      
            doc.LoadXml(Properties.Resources.carstyle);
            XmlNodeList elemList = doc.GetElementsByTagName("item");
            
            VehicleBrandInfo brandInfo;
            foreach (XmlNode node in elemList)
            {
                brandInfo = new VehicleBrandInfo();
                brandInfo.Name = node["name"].InnerText;
                brandInfo.ImageName = node["file"].InnerText;
                brandInfo.ID = int.Parse(node["id"].InnerText);

                if (!m_DTID2BrandInfo.ContainsKey(brandInfo.ID))
                {
                    m_DTID2BrandInfo.Add(brandInfo.ID, brandInfo);
                }
            }
        }
        
        private XmlNode[] SortCarBrand(XmlNodeList elemList)
        {
            string name;
            SortedDictionary<string, XmlNode> dict = new SortedDictionary<string, XmlNode>();
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

        public VehicleBrandInfo[] GetAllBrandInfos()
        {
            VehicleBrandInfo[] brandInfos = null;

            if (m_DTID2BrandInfo == null)
            {
                RetrieveBrandInfos();
            }

            if (m_DTID2BrandInfo != null)
            {
                brandInfos = new VehicleBrandInfo[m_DTID2BrandInfo.Count];
                m_DTID2BrandInfo.Values.CopyTo(brandInfos, 0);
            }

            return brandInfos;
        }

        public VehicleBrandInfo GetBrandInfo(int id)
        {
            if (id == -1)
            {
                return S_VEHICLEBRANDINFO_NOTSPECIFIED;
            }
            else if (id == 1000)
            {
                return S_VEHICLEBRANDINFO_OTHER;
            }

            VehicleBrandInfo brandInfo = null;

            if (m_DTID2BrandInfo == null)
            {
                RetrieveBrandInfos();
            }

            if (m_DTID2BrandInfo != null && m_DTID2BrandInfo.ContainsKey(id))
            {
                brandInfo = m_DTID2BrandInfo[id];
            }

            return brandInfo;
        }
    }
}
