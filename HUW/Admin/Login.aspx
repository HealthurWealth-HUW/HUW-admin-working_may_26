<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>

<head>

    <%--<link href="../sc/validator.css" rel="stylesheet" type="text/css" />
    <script src="../skin/frontend/default/megashop-pink/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../sc/jquery.validator-0.3.6.min.js" type="text/javascript"></script>
    --%>
    <title>HUW</title>
    <!-- favicon icon -->
    <link rel="shortcut icon" href="https://www.healthurwealth.com//assets/images/favicon.png" type="image/x-icon" />
    <!-- start: MAIN CSS -->
    <link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/fonts/style.css" />
    <link rel="stylesheet" href="assets/css/main.css" />
    <link rel="stylesheet" href="assets/css/main-responsive.css" />
    <link rel="stylesheet" href="assets/plugins/iCheck/skins/all.css" />
    <link rel="stylesheet" href="assets/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css" />
    <link rel="stylesheet" href="assets/plugins/perfect-scrollbar/src/perfect-scrollbar.css" />
    <link rel="stylesheet" href="assets/css/theme_light.css" id="skin_color" />
    <!--[if IE 7]>
		<link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome-ie7.min.css">
		<![endif]-->
    <!-- end: MAIN CSS -->
</head>


<body class="login example1">
    <div class="main-login col-lg-4 col-md-4 col-md-offset-4 col-sm-6 col-sm-offset-3">
        <div class="logo">
            <img src="https://www.healthurwealth.com//assets/images/logo.png" alt="Logo image" />
        </div>


        <div class="">
            <h3 class="text-center" style="margin-top: 10px;">Sign in to your account</h3>
            <form class="form-login" runat="server">
                <p class="text-center">
                    <asp:Label ID="lblMsg" runat="server" />
                </p>

                <fieldset>
                    <div class="form-group">
                        <span class="input-icon">
                            <asp:TextBox ID="txtUserName" CssClass="form-control" Style="height: 35px; border-radius: 5px;" runat="server" />
                            <i class="fa fa-user"></i></span>
                    </div>

                    <div class="form-group form-actions">
                        <span class="input-icon">
                            <asp:TextBox ID="txtPwd" CssClass="form-control" Style="height: 35px;" runat="server" TextMode="Password" validate="form" require="Please enter your desired password." regular="Invalid password, the password must be 6 characters or longer." validExpress=".{6,}" />
                            <i class="fa fa-lock"></i></span>
                    </div>

                    <div class="form-actions">
                        <a href="Admin_ForgotPassword.aspx">I forgot my password</a>
                        <asp:Button ID="btnLogIn" class="btn btn-bricky pull-right" Text="Login" runat="server" OnClick="btnLogIn_Click" />
                    </div>
                </fieldset>

            </form>
        </div>

        <%--<div class="copyright">
				2013 &copy;<a href="http://www.zoninnovative.com"> Zon Innovative IT Solutions Pvt. Ltd.</a>
			</div>--%>
            <p>&nbsp;</p>
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
    <%--</script>--%>
</body>
</html>



