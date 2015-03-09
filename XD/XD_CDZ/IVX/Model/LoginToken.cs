using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Model
{
    public class LoginToken
    {
         public string ServerIP{get; set;}
         public string UserName { get; set; }
         public string Password { get; set; }
         

         public LoginToken(string serverIP, string userName, string password)
         {
             ServerIP = serverIP;
             UserName = userName;
             Password = password;
         }
    }
}
