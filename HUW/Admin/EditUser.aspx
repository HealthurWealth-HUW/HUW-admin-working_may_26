<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_EditUser" CodeFile="EditUser.aspx.cs" %>

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

    <style type="text/css">
        .overlay {
            background-color: #EFEFEF;
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: 1000;
            top: 0px;
            left: 0px;
            opacity: .5; /* in FireFox */
            filter: alpha(opacity=50); /* in IE */
        }

        .pleaseWaitText {
            position: absolute;
            top: 200px;
            left: 50%;
            margin-left: -100px;
            font-size: 18px;
            font-weight: 800;
            color: red;
        }

        .paging-nav {
            text-align: right;
            padding-top: 2px;
        }

            .paging-nav a {
                margin: auto 1px;
                text-decoration: none;
                display: inline-block;
                padding: 1px 7px;
                background: #91b9e6;
                color: white;
                border-radius: 3px;
            }

            .paging-nav .selected-page {
                background: #187ed5;
                font-weight: bold;
            }

        .paging-nav,
        #tableData {
            width: 400px;
            margin: 0 auto;
            font-family: Arial, sans-serif;
        }
    </style>



    <link rel="stylesheet" type="text/css" href="CSS/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="CSS/dataTables.bootstrap.css">
    <link rel="stylesheet" type="text/css" href="CSS/Style.css">
    <script type="text/javascript" src="JS/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="JS/jquery.dataTables.js"></script>
    <script type="text/javascript" src="JS/dataTables.bootstrap.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //  alert();
            $('#tblContact').dataTable({
                "iDisplayLength": 5,
                "lengthMenu": [5, 10, 25, 50, 100]
            });
            GetUsers();

            $('.close').click(function () {
                $('#myModal').hide();
            });
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#grdShippingOrders').pagination({ limit: 5 });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<div class="widget">
        <label>Username</label>
        <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
          <label>Mobile NUmber</label>
        <asp:TextBox ID="txtMobileNumber" runat="server"></asp:TextBox>
          <label>Alternate Mobile Number</label>
        <asp:TextBox ID="txtAMobileNumber" runat="server"></asp:TextBox>
          <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
       
    </div>--%>
    <div id="" class="">
            <div class="one_wrap " id="" style="">
                <div id="" class="widget" style="">
                    <div class="widget_title">
                        <%--<span style="font-family: iconSweets;"></span>--%>
                       
                    </div>
                    <label id="lblerror" runat="server" style="color: red; padding: 10px 0;"></label>
                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="">

                        <div class="search_contentpad">

                            <div class="" id="">
                                <ul class="form_fields_container">
                                    <li>
                                        <label class="search_label" id="lblPrdctID">User Name</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtUsername" runat="server" required></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet1">
                                            <div class="arrow"></div>
                                           
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Mobile NUmber</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtMobileNumber" runat="server" ></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet2">
                                            <div class="arrow"></div>
                                           
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblPrdctID">Alternate Number</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtAMobileNumber" runat="server" ></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet3">
                                            <div class="arrow"></div>
                                           
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Password</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtPassword" runat="server" required ></asp:TextBox>
                                        </div>
                                        <div class="toolti" id="tstreet4">
                                            <div class="arrow"></div>
                                            
                                        </div>
                                    </li>
                                  

                                    <div class="globalaction_bar text_right">
                                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="">*</span>
                                         <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Update" class="button_small greyishBtn" />
                                        <%--<asp:Button ID="con" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" OnClientClick="myfunction(); return false;" Text="Continue to Step 2" />--%>
                                    </div>
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
</asp:Content>
