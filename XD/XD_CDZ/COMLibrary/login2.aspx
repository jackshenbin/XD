<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login2.aspx.cs" Inherits="login2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>用户登录</title>
<link href="images/login.css" rel="stylesheet" type="text/css" />
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


<body>
<form id="form" runat="server">
    <div id="login">

		 
		 <div id="center">
			  <div id="center_middle">
			       <div id="user"><asp:TextBox ID="username" runat="server" Style="position: relative; width: 130px;height:22px;"></asp:TextBox></div>
				   <div id="password"><asp:TextBox ID="mypassword" runat="server" Style="position: relative; width: 130px;height:22px; top: 0px; left: 0px;" TextMode="Password"></asp:TextBox></div>
			  
			  </div>
			  <div id="center_right">
				   <div id="btn">
                       <asp:Button ID="Button1" runat="server" OnClick="Button_logoin" 
                           Text="" Width="70px" Height="65px" class="btn3_mouseout" onmouseover="this.className='btn3_mouseover'"  onmouseout="this.className='btn3_mouseout'"/>&nbsp;
                       </div>
			  		<div id="msg"><asp:Label ID="message" runat="server" ForeColor="Red"> </asp:Label></div>

			  </div>
		 </div>

	</div>
	</form>
</body>
</html>

