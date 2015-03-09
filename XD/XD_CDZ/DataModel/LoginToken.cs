using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.DataModel
{
    public class LoginToken
    {
         public string ServerIP{get; set;}
         public string UserName { get; set; }
         public string Password { get; set; }
         public int UserID { get; set; }
         public int UserType { get; set; }

         public LoginToken(int userID,string serverIP, string userName, string password,int type)
         {
             UserID = userID;
             ServerIP = serverIP;
             UserName = userName;
             Password = password;
             UserType = type;
         }
    }
}
