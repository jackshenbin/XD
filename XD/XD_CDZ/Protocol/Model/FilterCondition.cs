using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Protocol.Model
{
    public class FilterCondition
    {
        private Dictionary<string, object> m_Settings;

        public object this[string key] 
        {
            get
            {
                object obj = null;

                if (m_Settings.ContainsKey(key))
                {
                    obj = m_Settings[key];
                }

                return obj;
            }
            set
            {
                if (!m_Settings.ContainsKey(key))
                {
                    m_Settings.Add(key, value);
                }
                else
                {
                    m_Settings[key] = value;
                }
            }
        }

        public FilterCondition()
        {
            m_Settings = new Dictionary<string, object>();
        }
    }
}
