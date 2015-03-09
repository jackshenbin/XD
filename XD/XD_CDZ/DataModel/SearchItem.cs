using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class SearchItem : SearchParaBase
    {
        private bool m_IsFinished = false;

        public bool IsFinished
        {
            get
            {
                return m_IsFinished;
            }
            set
            {
                lock (this)
                {
                    if (m_IsFinished != value)
                    {
                        m_IsFinished = value;
                    }
                }
            }
        }

        public SearchResultSingleSummary ResultSummary { get; set; }

        public uint TaskUnitId { get; set; }

        public uint CameraId { get; set; }

    }
}
