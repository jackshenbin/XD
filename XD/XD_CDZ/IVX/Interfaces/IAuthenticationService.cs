using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;

namespace BOCOM.IVX.Interfaces
{
    public interface IAuthenticationService
    {
        bool Login(string userName, string password, string ip, ushort port, uint timeout, uint userData);

        bool Logout();

        string GetLoginUserRoleStr();

        BOCOM.DataModel.E_VDA_USER_ROLE_TYPE GetLoginUserRole();

    }
        
}
