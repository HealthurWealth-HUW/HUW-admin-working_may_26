<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_ForgotPassword.aspx.cs" Inherits="Admin_Admin_ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"/>

<head>
  		<title>HUW</title>
		<!-- start: MAIN CSS -->
		<link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen"/>
		<link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome.min.css"/>
		<link rel="stylesheet" href="assets/fonts/style.css"/>
		<link rel="stylesheet" href="assets/css/main.css"/>
		<link rel="stylesheet" href="assets/css/main-responsive.css"/>
		<link rel="stylesheet" href="assets/plugins/iCheck/skins/all.css"/>
		<link rel="stylesheet" href="assets/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css"/>
		<link rel="stylesheet" href="assets/plugins/perfect-scrollbar/src/perfect-scrollbar.css"/>
		<link rel="stylesheet" href="assets/css/theme_light.css" id="skin_color"/>
		<!--[if IE 7]>
		<link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome-ie7.min.css">
		<![endif]-->
		<!-- end: MAIN CSS -->
	</head>

<body class="login example1">

<script type="text/javascript">
    function ForgotPwdSubmit() {
         $('#forgotten').submit();
           }
</script>

		<div class="main-login col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3">
			<div class="logo">
            <img src="../skin/frontend/default/megashop-pink/images/logo.png"/>
			</div>
			
			<div class="box-login">
				<h3>Forget Password?</h3>
				<form class="form-forgot" runat="server">
                  <p>
                <asp:Label ID="lblErrormessage" runat="server" Text=""></asp:Label>
                </p>
				  <fieldset>
						<div class="form-group">
							<span class="input-icon">
                            <input id="EmailId" type="text" name="email" value="" class="form-control" placeholder="Email" runat="server" />
								<i class="fa fa-envelope"></i> </span>
						</div>

					    <div class="form-actions">
                        <asp:Button ID="btnBack" class="btn btn-bricky pull-left"   Text="Back"   runat="server" onclick="btnBack_Click"/>
                        <asp:Button ID="btnForgotSubmit" class="btn btn-bricky pull-right"   Text="Submit"  
                                runat="server" onclick="btnForgotSubmit_Click" OnClientClick="javascript:ForgotPwdSubmit()" />  
     				</div>
					</fieldset>
				</form>
			</div>	
			
            <%--<div class="copyright">
				2013 &copy;<a href="http://www.zoninnovative.com"> Zon Innovative IT Solutions Pvt. Ltd.</a>
			</div>--%>
			
		</div>
		<!-- start: MAIN JAVASCRIPTS -->
		<!--[if lt IE 9]>
		<script src="assets/plugins/respond.min.js"></script>
		<script src="assets/plugins/excanvas.min.js"></script>
		<![endif]-->
		<script type="text/javascript" src="../../../ajax.googleapis.com/ajax/libs/jquery/2.0.3/jquery.min.js"></script>
		<script type="text/javascript" src="assets/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js"></script>
		<script type="text/javascript" src="assets/plugins/bootstrap/js/bootstrap.min.js"></script>
		<script type="text/javascript" src="assets/plugins/blockUI/jquery.blockUI.js"></script>
		<script type="text/javascript" src="assets/plugins/iCheck/jquery.icheck.min.js"></script>
		<script type="text/javascript" src="assets/plugins/perfect-scrollbar/src/jquery.mousewheel.js"></script>
		<script type="text/javascript" src="assets/plugins/perfect-scrollbar/src/perfect-scrollbar.js"></script>
		<script type="text/javascript" src="assets/plugins/less/less-1.5.0.min.js"></script>
		<script type="text/javascript" src="assets/plugins/jquery-cookie/jquery.cookie.js"></script>
		<script type="text/javascript" src="assets/plugins/bootstrap-colorpalette/js/bootstrap-colorpalette.js"></script>
		<script type="text/javascript" src="assets/js/main.js"></script>
		<!-- end: MAIN JAVASCRIPTS -->
		<!-- start: JAVASCRIPTS REQUIRED FOR THIS PAGE ONLY -->
		<script type="text/javascript" src="assets/plugins/jquery-validation/dist/jquery.validate.min.js"></script>
		<script type="text/javascript" src="assets/js/login.js"></script>
		<!-- end: JAVASCRIPTS REQUIRED FOR THIS PAGE ONLY -->
		<script type="text/javascript">
		    jQuery(document).ready(function () {
		        Main.init();
		        Login.init();
		    });
		</script>
</body>