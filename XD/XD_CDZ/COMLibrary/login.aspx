<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>用户登录</title>
<link href="style/login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
<!--
        .btn3_mouseout {
         float:left;background:url(images/login_btn.jpg)
        } 
        .btn3_mouseover {
         float:left;background:url(images/login_btnover.jpg)
        }
-->
</style>
</head>
    <script type="text/javascript">
        function Button_logoin() {
            var username = document.getElementById("username").value
            var mypassword = document.getElementById("mypassword").value
            if (username.length == 0 || mypassword.length == 0)
            {
                alert("用户名或密码不能为空。");
                return false;
            }
        }
    </script>

<body>
<form id="form" action="default.aspx" method="get">
    <div id="login">

		 <div id="center">
			  <div id="center_middle" >
			       <div id="user"><input type="text" ID="username" runat="server" Style="position: relative; width: 130px;height:22px; top: 0px; left: 0px;"/></div>
				   <div id="password"><input type="Password" ID="mypassword" runat="server" Style="position: relative; width: 130px;height:22px; top: 0px; left: 0px;" /></div>
			  
			  </div>
			  <div id="center_right">
				   <div id="btn">
                       <input type="submit" ID="Button1"  onclick="return Button_logoin()" 
                           value="" Style = "Width:70px; Height:65px;" class="btn3_mouseout" onmouseover="this.className='btn3_mouseover'"  onmouseout="this.className='btn3_mouseout'"/>&nbsp;
                       </div>
			  		<div id="msg"><asp:Label ID="message" runat="server" ForeColor="Red"> </asp:Label></div>

			  </div>
		 </div>

	</div>
	</form>
</body>
</html>

