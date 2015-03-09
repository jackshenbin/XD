using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Model
{
    public class LogDetailCat
    {
        #region Fields

        private int m_Id;
        private int m_catId;
        private string m_Name;

        #endregion

        #region Properties

        public int CatId
        {
            get { return m_catId; }
        }
        
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        #endregion

        #region Constructors

        public LogDetailCat(int id, int catId, string name)
        {
            this.m_Id = id;
            this.m_catId = catId;
            this.m_Name = name;
        }

        #endregion

        public override string ToString()
        {
            return this.m_Name;
        }

        public static int Compare(LogDetailCat detailCat1, LogDetailCat detailCat2)
        {
            return string.Compare(detailCat1.Name, detailCat2.Name);
        }
    }
}
