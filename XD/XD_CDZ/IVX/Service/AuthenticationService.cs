using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Protocol;
using BOCOM.IVX.Framework;
using System.ComponentModel.Composition;
using BOCOM.IVX.Interfaces;
using BOCOM.DataModel;
using BOCOM.IVX.Common;



namespace BOCOM.IVX.Service
{
    //[Export(typeof(IAuthenticationService))]
    public class AuthenticationService //: IAuthenticationService
    {
        //private object OnUserLogginCompleted(VAServerMsg msg, object[] args)
        //{
        //    int e = 0;// ICASProtocol.Vda_GetLoginResult();
        //    E_LOGIN_RESULT loginResult = (E_LOGIN_RESULT)e;

        //    return loginResult;
        //}
        public UserInfo CurrUser { get { return Framework.Container.Instance.IVXProtocol.GetLoginUserInfo(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="timeout"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public bool Login(string userName, string password, string ip, ushort port, uint timeout, uint userData)
        {
            bool bRet = true;
            bRet = Framework.Container.Instance.IVXProtocol.Login(ip, 9001, userName, password, timeout, userData);

            if (bRet)
            {
                LoginToken token = new LoginToken(ip, userName, password);
                Framework.Container.Instance.EvtAggregator.GetEvent<UserLoginEvent>().Publish(token);
                ////UserInfo user = Framework.Container.Instance.VDAConfigService.GetUserByName(userName);
                //Framework.Environment.CurrentUser = user;
            }
            return bRet;
        }

        public bool Logout()
        {
            bool bRet = true;
            try
            {
                if (Framework.Environment.IsLoggedIn)
                {
                    try
                    {
                        bRet = Framework.Container.Instance.IVXProtocol.Logout();
                    }
                    catch (SDKCallException ex)
                    { }
                }

                Framework.Container.Instance.IVXProtocol.Cleanup();
                Framework.Container.Instance.EvtAggregator.GetEvent<UserLogOutEvent>().Publish(null); ;
            }
            catch (SDKCallException ex)
            {

            }
            return bRet;
        }
        
        public E_VDA_USER_ROLE_TYPE GetLoginUserRole()
        {
            return  Framework.Container.Instance.IVXProtocol.GetLoginUserType();
        }

        public string GetLoginUserRoleStr()
        {
            E_VDA_USER_ROLE_TYPE type = Framework.Container.Instance.IVXProtocol.GetLoginUserType();
            string role = "";
            switch(type)
            {
                case E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_ADMIN:
                    role = "管理员";
                    break;
                case E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_LEADER:
                    role = "组长";
                    break;
                case E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_NORMAL:
                    role = "普通用户";
                    break;
                case E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_SUPPER:
                    role = "超级管理员";
                    break;
            }
            return role;
        }
    }
}
