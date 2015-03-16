<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_Default" %>

<!doctype html public "-//w3c//dtd xhtml 1.0 transitional//en" "http://www.w3.org/tr/xhtml1/dtd/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >


<head runat="server">
<link rel="shortcut icon" href="favicon.ico"/>
<link href="Style/css-all.css" rel="stylesheet" type="text/css" />
<link href="Style/login.css" rel="stylesheet" type="text/css" />

    <title>电动汽车充换电监控系统</title>

    <style type="text/css">
        .auto-style1 {
            height: 100px;
            font-size: 24px;
        }

        .btn3_mouseout {
            float: left;
            background: url(images/login_btn.jpg);
        }

        .btn3_mouseover {
            float: left;
            background: url(images/login_btnover.jpg);
        }

    </style>
</head>
    <script type="text/javascript">
        var ServerDBIP = "121.40.88.117";
        var ServerClientVer = "1.0.0.9";

        function switchwnd(index) {
            //alert("VdaSwitchForm index:" + index);
            XDSDK.VdaSwitchForm(index);
        }

        function initocx(us, ps) {
            //alert("VdaInitialization " );
            try {
                //alert("3");
                var loginstr = "<Ret><RetMsg><ErrorCode>0</ErrorCode><Description></Description></RetMsg><RetInfo></RetInfo></Ret>";

                var loginret = XDSDK.VdaInitialization(ServerDBIP, us, ps);
                //alert(loginret);
                if (loginret == loginstr) {
                    //设置两个cookie 
                    document.cookie = "userPass=" + ps;
                    document.cookie = "userName=" + us;
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (e)
            {
                //alert("4");

                document.getElementById("xdsdkform").style.height = 0;
                document.getElementById("versionckeck").innerHTML = "<li>控件未加载</li>"
                    + "<li>首次使用请先<a href='./ocx/iesetup.exe'>下载IE设置工具iesetup.exe</a></li>"
                    + "<li>然后再<a href='./ocx/XD_CDZ.exe'>下载控件XD_CDZ.exe</a> 解压后运行regcom.bat</li>"
                return false;
            }
        }
        document.onkeydown = function (event) {
            e = event ? event : (window.event ? window.event : null);
            if (e.keyCode == 13) {
                //执行的方法 
                Button_logoin();
            }
        }
        function Button_logoin() {
            var username = document.getElementById("username").value
            var mypassword = document.getElementById("mypassword").value
            if (username.length == 0 || mypassword.length == 0) {
                alert("用户名或密码不能为空。");
                return false;
            }
            else {
                document.getElementById("login").style.display = "none";
                document.getElementById("mainpage").style.display = "";
                if (checkVersion()) {
                    if (!initocx(username, mypassword))
                    {
                        document.getElementById("login").style.display = "";
                        document.getElementById("mainpage").style.display = "none";
                        alert("用户名或密码错误。");

                    }
                }
                else {
                    document.getElementById("xdsdkform").style.height = 0;
                }
            }
        }

        function getCookie(valname) {
            //获取cookie字符串 
            var strCookie = document.cookie;
            //将多cookie切割为多个名/值对 
            var arrCookie = strCookie.split("; ");
            var userId;
            //遍历cookie数组，处理每个cookie对 
            for (var i = 0; i < arrCookie.length; i++) {
                var arr = arrCookie[i].split("=");
                //找到名称为userId的cookie，并返回它的值 
                if (valname == arr[0]) {
                    userId = arr[1];
                    break;
                }
            }
            return userId;
        }

        function checkVersion() {
            var version = "<Ret><RetMsg><ErrorCode>0</ErrorCode><Description></Description></RetMsg><RetInfo>" + ServerClientVer + "</RetInfo></Ret>";

            var ocxversiion = ""
            try {
                //alert("1");
                ocxversiion = XDSDK.VdaGetVersion();
                //alert(ocxversiion +"\n"+version);
                if (ocxversiion < version) {
                //alert("1.1");
                document.getElementById("XDSDK").style.height = 0;
                    document.getElementById("versionckeck").innerHTML = "<li>控件版本不匹配</li>"
                        + "<li><a href='./ocx/XD_CDZ.exe'>请下载最新控件XD_CDZ.exe</a> 解压后运行regcom.bat，并重启浏览器</li>"
return false;
                }
                return true;
            } catch (e) {
                //alert("2"+e);
                document.getElementById("xdsdkform").style.height = 0;
                document.getElementById("versionckeck").innerHTML = "<li>控件未加载</li>"
                    + "<li>首次使用请先<a href='./ocx/iesetup.exe'>下载IE设置工具iesetup.exe</a></li>"
                    + "<li>然后再<a href='./ocx/XD_CDZ.exe'>下载控件XD_CDZ.exe</a> 解压后运行regcom.bat</li>"
                return false;
            }



        }
    </script>
<body >
        <div id="login" >

		 <div id="center">
			  <div id="center_middle" >
			       <div id="user"><input type="text" ID="username" runat="server" Style="position: relative; width: 130px;height:22px; top: 0px; left: 0px;"/></div>
				   <div id="password"><input type="Password" ID="mypassword" runat="server" Style="position: relative; width: 130px;height:22px; top: 0px; left: 0px;" /></div>
			  
			  </div>
			  <div id="center_right">
				   <div id="btn">
                       <input type="button" ID="Button1"  onclick="return Button_logoin()" 
                           value="" Style = "Width:70px; Height:65px;" class="btn3_mouseout" onmouseover="this.className='btn3_mouseover'"  onmouseout="this.className='btn3_mouseout'"/>&nbsp;
                       </div>
			  		<div id="msg"><asp:Label ID="message" runat="server" ForeColor="Red"> </asp:Label></div>

			  </div>
		 </div>

	</div>

    <div id="mainpage" style="display:none">
    <table  align="center" width="100%">
        <tr>
            <td align="left" width="100%">
                <table width="1035">
                    <tr>
                        <td>
                            <div id="top">
                                <div style="position:absolute;  margin-left:920px;  margin-top:47px;"><a class="logout" href="default.aspx" target="_self">注销</a></div>
                                <div class="logo">
                                    <img alt="电动汽车智能充换电服务网络运营监控系统" src="images/head.jpg" border="0" class="home" /></a>
                                </div>

                                <div id="menu">
                                    <div class="lc"></div>
                                    <div class="cc" id="mymenu" runat="server">
                                        <ul>
                                            <li class="nobg"><a onclick="switchwnd(0)" href="#">网站首页</a></li>
                                            <li><a onclick="switchwnd(3)" href="#">充电桩管理</a></li>
                                            <li><a onclick="switchwnd(2)" href="#">智能卡管理</a></li>
                                            <li><a onclick="switchwnd(4)" href="#">综合查询</a></li>
                                            <li><a onclick="switchwnd(1)" href="#">系统管理</a></li>
                                            <li><a>服务支持</a></li>
                                            <li><a>关于我们</a></li>
                                        </ul>
                                    </div>
                                    <div class="rc"></div>
                                </div>
                            </div>

                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="versionckeck"></div>
                <div id="xdsdkform">
                <object id="XDSDK" classid="clsid:8F72B3C0-0DEF-4995-9D9C-A7108CDD21E1" width="100%"
                        height="800"></object>
                    </div>
            </td>
        </tr>
    </table>
    </div>
</body>
</html>
