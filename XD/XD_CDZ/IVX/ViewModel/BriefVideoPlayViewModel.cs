using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.ViewModel
{
    public class BriefVideoPlayViewModel
    {
        private IntPtr m_windowHandle;

        public BriefVideoPlayViewModel(IntPtr windowHandle)
        {
            m_windowHandle = windowHandle;
        }
    }
}
