using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Common
{
    public class LoginException : Exception
    {
        public LoginException(string msg) :
            base(msg)
        {

        }
    }
}
