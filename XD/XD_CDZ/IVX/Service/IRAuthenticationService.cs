using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BOCOM.IVX.Interfaces;
using System.ComponentModel.Composition;
using BOCOM.IVX.Protocol;
using System.Diagnostics;

namespace BOCOM.IVX.Service
{
    //[Export("IRAuthenticationService", typeof(IAuthenticationService))]
    public class IRAuthenticationService// : IAuthenticationService
    {
        //[Import (typeof(IAuthenticationService))]
        private AuthenticationService m_AuthenticationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException">传入参数格式异常</exception>
        /// <returns></returns>
        public bool Login(string userName, string password, string ip, ushort port, uint timeout, uint userData)
        {
            bool bRet = false;

            if (IR_SDK.IR_SetLoginCmdInfo(userName))
            {
                int loginType = 2; // Framework.Environment.LoginType;			//登录类型（直连服务器or管理服务器）
                string serverIP = Framework.Environment.ServerIP;		//IP地址
                string user = IR_SDK.IR_GetIDFromAuthToken(Framework.Environment.IR_APP_ID, Framework.Environment.IR_APP_KEY);	//用户名
                // string userName = null;
                //if (string.IsNullOrEmpty(userName))
                //{
                //    userName = "jim";
                //}
                if (!string.IsNullOrEmpty(userName))
                {
                    password = "3edcVFR$";	//高级权限通用密码
                    bRet = m_AuthenticationService.Login(userName, password, ip, port, timeout, userData);
                    if (bRet)
                    {
                        IR_SDK.StartHeartbeat();
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }

            return bRet;
        }

        public bool Logout()
        {
            bool bRet = true;

            IR_SDK.StopHeartbeat();

            return bRet;
        }

        public string GetLoginUserRoleStr()
        {
            return "普通用户";
        }
        public BOCOM.DataModel.E_VDA_USER_ROLE_TYPE GetLoginUserRole()
        {
            return BOCOM.DataModel.E_VDA_USER_ROLE_TYPE.E_ROLE_TYPE_NORMAL;
        }
    }
}
