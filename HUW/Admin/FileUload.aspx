<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_FileUload" CodeFile="FileUload.aspx.cs" %>

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
            <h5>Upload Invoice file </h5>
        </div>
        <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

            <div class="search_contentpad">

                <div class="margin_10" id="margin_10">
                    <ul class="form_fields_container">
                        <li>
                            <asp:FileUpload runat="server" ID="fupload" CssClass="form-control" />&nbsp;&nbsp;&nbsp;
                            <asp:TextBox runat="server" TextMode="Date" ID="exceluploadeddate" placeholder="Please Enter Excel Date" CssClass="form-control"  ></asp:TextBox>&nbsp;&nbsp;&nbsp;

                            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Save as Excel" OnClick="Button1_Click" />
                        </li>
                        <li>
                            <asp:FileUpload runat="server" ID="Pdfuload" CssClass="form-control" />&nbsp;&nbsp;&nbsp;
                            <asp:TextBox runat="server" TextMode="Date" ID="pdfuploadeddate" placeholder="Please Enter PDF Date" CssClass="form-control"  ></asp:TextBox>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnpdf" runat="server" Text="Save as PDF" CssClass="btn btn-primary" OnClick="btnpdf_Click" />
                        </li>


                        <%-- <div class="globalaction_bar text_right">
                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                        <input type="submit" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch">
                    </div>--%>
                    </ul>
                </div>
            </div>
        </div>
    </div>




    <div class="widget">
        <div class="widget_title">
            <h5>Invoice Uploaded List </h5>
        </div>
                        <p>&nbsp;</p>
        <asp:Button runat="server" ID="btnback" CssClass="btn btn-info" Text="Record List"  OnClick="btnback_Click" />
        <div class="widget_body">
            <div class="">
                <div class="col-xs-12">
                        <p>&nbsp;</p>

                        <%--<asp:Button runat="server" ID="btnback" CssClass="btn btn-info" Text="Record List" OnClick="btnback_Click" />--%>
                    <%--<div class="pull-right">
                        <p>&nbsp;</p>

                        <asp:Button runat="server" ID="btnback" CssClass="btn btn-info" Text="Record List" OnClick="btnback_Click" />
                    </div>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <h4>Excel Document List</h4>
                    <table>
                        <thead>
                            <tr>
                                <th>Document Name</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptDetailsTbl" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="LblPARTYTYPE" Text='<%#Eval("File_Name") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="Label1" Text='<%#Eval("File_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                        </tfoot>
                    </table>
                    <!-- /.box -->
                </div>

                <div class="col-xs-6">

                    <h4>PDF Document List</h4>
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>PDF Name</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptDetailsTbl1" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="Label2" Text='<%#Eval("Pdf_Name") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="Label3" Text='<%#Eval("Pdf_Date") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                        <tfoot>
                        </tfoot>
                    </table>
                    <!-- /.box -->
                </div>
                <!-- /.col -->
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
