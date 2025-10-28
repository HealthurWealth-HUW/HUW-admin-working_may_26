<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_RemittanceFileUpload" CodeFile="RemittanceFileUpload.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <%-- <link href="https://cdn.datatables.net/1.10.13/css/dataTables.bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/responsive/2.1.1/css/responsive.bootstrap.min.css" />--%>


    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>--%>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
    <!-- start: MAIN CSS -->
    <link href="assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />
    <link rel="stylesheet" href="assets/plugins/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="assets/fonts/style.css" />
    <link rel="stylesheet" href="assets/css/main.css" />
    <link rel="stylesheet" href="assets/css/main-responsive.css" />
    <link rel="stylesheet" href="assets/plugins/iCheck/skins/all.css" />
    <link rel="stylesheet" href="assets/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css" />
    <link rel="stylesheet" href="assets/plugins/perfect-scrollbar/src/perfect-scrollbar.css" />

    <link rel="stylesheet" type="text/css" href="CSS/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="CSS/dataTables.bootstrap.css">
    <link rel="stylesheet" type="text/css" href="CSS/Style.css">
    <link rel="stylesheet" href="http://cdn.datatables.net/1.11.4/css/jquery.dataTables.min.css" />

    <script type="text/javascript" src="JS/jquery.dataTables.js"></script>
    <script type="text/javascript" src="JS/dataTables.bootstrap.js"></script>
    <script type="text/javascript" src="http://cdn.datatables.net/1.11.4/js/jquery.dataTables.min.js"></script>
    <style>
        .form_fields_container {
            padding-left: 10px;
        }

            .form_fields_container li {
                display: flex;
            }

        .search_contentpad {
            border: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="widget">
        <div class="widget_title ">
            <h5>Remittance file upload</h5>
        </div>
        <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

            <div class="search_contentpad">

                <div class="margin_10" id="margin_10">
                    <ul class="form_fields_container">
                        <li>
<label>Enter Id:</label>&nbsp;&nbsp;&nbsp;
                            <asp:TextBox placeholder="Enter Remittance Id" runat="server" ID="txtRemId" CssClass="form-control"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                            <asp:FileUpload runat="server" ID="fupload" CssClass="form-control" />&nbsp;&nbsp;&nbsp;
 <li>
                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Upload Delhivery File" OnClick="Button1_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Upload ShipRocket File"  OnClick="Button2_Click"/>

                        </li> 
                      
                    </ul>
                </div>
            </div>
        </div>
    </div>




  
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
</asp:Content>
