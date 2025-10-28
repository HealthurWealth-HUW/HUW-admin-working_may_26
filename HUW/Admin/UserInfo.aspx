<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_UserInfo" CodeFile="UserInfo.aspx.cs" %>

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
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/dataTables.bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>

    <style type="text/css">
       #tblContact_wrapper{
          overflow-x: scroll;
       }
       #tblContact tbody td:nth-child(3),  #tblContact thead tr  th:nth-child(3){
          max-width: 250px !important;
    word-break: break-all;
       }
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
    <%--<script type="text/javascript" src="JS/jquery-1.10.2.min.js"></script>--%>
     <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            //  alert();
            setTimeout(function () {
            $('#tblContact').dataTable({
            });
        }, 2000);
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
    <div class="widget">
        <div class="widget_title ">
            <%--<span class="iconsweet">r</span>--%>
            <h5>Users List</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="lblName">Name</label>
                        <div class="form_input">
                            <input id="txtName" name="txtMobile" type="text" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="lblMobile">Mobile</span></label>
                        <div class="form_input">
                            <input name="txtMobile" id="txtMobile" type="text" />
                        </div>
                    </div>
                </li>
            </ul>
            <ul class="form_fields_container">
                <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="lblEmail">Email</label>
                        <div class="form_input">
                            <input id="txtEmail" name="txtEmail" type="text" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="lblOrderid">Order id</span></label>
                        <div class="form_input">
                            <input name="txtorderid" id="txtorderid" type="text" />
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div class="widget_body">
            <div class="action_bar text_right">
                <input name="btnSearch" value="Search" onclick="GetUsers()" class="button_small greyishBtn" type="button" />
                <span>
                    <input name="Button2" value="Clear All" class="button_small greyishBtn" type="submit" /></span>
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <div id="divPrdctList">
        <div id="cp_placeholder">
            <!--Activity Stats-->
      <%--      <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Users List</label>
                </h1>
            </div>--%>
            <!--Quick Actions-->
            <div id="divMsg" class="msgbar msg_Success hide_onC" style="display: none;">
            </div>
            <div id="divexport" class="dataTable">
            </div>
            <div>
                <div id="ctl00_ctl00_Main_Main_PageMessage" class="msgbar msg_Success hide_onC" style="display: none;">
                    <span class="iconsweet">=</span>
                    <label id="ctl00_ctl00_Main_Main_lblSuccess">
                    </label>
                </div>
                <div style="display: none;" id="ctl00_ctl00_Main_Main_PanMsg" class="cp_success">
                </div>
            </div>
            <!--One_Three-->
            <div class="one_wrap fl_left">
                <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display: none;">
                </div>
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <div id="divUserSearchGrd">
                         <div id="pager">
            </div>
                    </div>
                </div>
            </div>
            <div class="widget">
            </div>
        </div>
    </div>
    <div id="myModal" class="modal fade" role="dialog">
        <!-- Modal content -->
        <%-- <div class="modal-content">
            <span class="close">&times;</span>
            <div id="OrderBody" class="modal-body">
            </div>
        </div>--%>
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"></h4>

                </div>
                <br />
                <div>
                    <div class="col-sm-7">
                        <p>
                            Name: 
                            <span id="lblName1"></span>
                        </p>
                        <p>
                            Email:<span id="lblEmail1"></span>
                        </p>
                        <p>
                            Address:  
                            <span id="lblAddress1"></span>
                        </p>
                    </div>
                    <div class="col-sm-5">
                        <p>
                            Mobile: <span id="lblMobile1"></span>
                        </p>
                        <p>
                            Password: <span id="lblPassword1"></span>
                        </p>
                        <p>
                            Status: <span id="lblStatus1"></span>
                        </p>

                    </div>
                </div>

                <div id="OrderBody" class="modal-body">
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
