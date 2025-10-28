<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_RecordMaster" CodeFile="RecordMaster.aspx.cs" %>

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
     <script type="text/javascript">
        $(document).ready(function () {
            //  alert();
            //$('#tblContact').dataTable({ });

           
        });

    </script>
    <style>
        table {
            font-family: arial, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

        td, th {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        tr:nth-child(even) {
            background-color: #dddddd;
        }
        .form_fields_container > li > div label{
                min-width: 100px;
    margin: 5px 0 5px 0;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!--news code-->
    <div class="widget">
        <div class="widget_title ">

            <h5>Delivery List by Month</h5>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="lblName">Document Name</label>
                        <div class="form_input">
                            <asp:DropDownList ID="ddlDocumnetMonth" runat="server" CssClass="form-control" 
                                OnSelectedIndexChanged="ddlDocumnetMonth_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="lblMobile">Pincode</span></label>
                        <div class="form_input">
                            <%-- <input name="txtMobile" id="txtMobile" type="text">--%>

                            <asp:DropDownList ID="ddlpincode" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="ddlpincode_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </div>
                    </div>
                </li>
            </ul>
           
        </div>
       <%-- <div class="widget_body">
            <div class="action_bar text_right">
                <input name="btnSearch" value="Search" onclick="GetUsers()" class="button_small greyishBtn" type="button">
                <span>
                    <input name="Button2" value="Clear All" class="button_small greyishBtn" type="submit"></span>
                <div class="clear"></div>
            </div>
        </div>--%>
    </div>
    <!-- new code ends -->


    <div id="ctl00_ctl00_Main_Main_dvCheckPageControl" runat="server">
       <%-- <div class="col-xs-12">
            <asp:Label ID="lblDocmonth" runat="server" Text="Document Name"></asp:Label>
            >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblPin" runat="server" Text="Pin Code"></asp:Label>

            
        </div>--%>
        <%--Price & Availability Information--%>
        <div class="widget">
            <div class="widget_title">
                <h5>
                    Delivery Invoice List
                </h5>
            </div>
            <div class="widget_body">

                <div class="row">
                    <div class="col-xs-12">
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnUpload" Text="Upload file"
                OnClick="btnUpload_Click" Style="float: right" />
                        <p>&nbsp;</p><p>&nbsp;</p>
                        <table id="tblContact">
                            <thead>
                                <tr>
                                    <th>Order id</th>
                                    <th>Waybill No</th>
                                    <th>Package Type</th>
                                    <th>Status</th>
                                    <th>Amount</th>
                                    <th>Dpin code</th>
                                    <th>Item shipped</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptDetailsTbl" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="LblPARTYTYPE" Text='<%#Eval("Order_id") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="LblPARTYNAME" Text='<%#Eval("waybill_no") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="LblCITY" Text='<%#Eval("package_type") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="LblCONTACTNO" Text='<%#Eval("status") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label1" Text='<%#Eval("Sum") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label2" Text='<%#Eval("D_Pin") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="LblPARTYADDRESS" Text='<%#Eval("Item_Shipped") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>

                                <tr>
                                    <td>
                                        <p>Total Sum</p>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="Lblsum"></asp:Label></td>
                                     <td colspan="6"></td>
                                </tr>
                            </tfoot>
                        </table>
                        <!-- /.box -->
                    </div>
                    <!-- /.col -->
                </div>

            </div>
        </div>
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
</asp:Content>
