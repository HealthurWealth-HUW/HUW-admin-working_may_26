<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SalesReport.aspx.cs" Inherits="Admin_SalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />


    <script type="text/javascript">
        function addDays(theDate, days) {
            return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);
        }
        $(document).ready(function () {
            var FromDate = '';
            var Todate = '';
            var Status = 0;
            var Days = 1;
            var Range = GetParameterValues('Range');
            var From = GetParameterValues('From');

            function GetParameterValues(param) {
                var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < url.length; i++) {
                    var urlparam = url[i].split('=');
                    if (urlparam[0] == param) {
                        return urlparam[1];
                    }
                }
            }
            if (Range != undefined) {

                if (Range == 'Today') {
                    var dNow = new Date();
                    var utcdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    FromDate = utcdate;
                    var utctomorrowdate = (dNow.getMonth() + 1) + '/' + (dNow.getDate() + Days) + '/' + dNow.getFullYear();
                    Todate = utctomorrowdate;
                }
                if (Range == 'Week') {
                    var dNow = new Date();
                    var utcdate = (dNow.getMonth() + 1) + '/' + (dNow.getDate() + Days) + '/' + dNow.getFullYear();
                    Todate = utcdate;
                    var firstday = new Date(dNow.setDate((dNow.getDate() + 1) - dNow.getDay()));
                    dNow = firstday;
                    var utcweekdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    FromDate = utcweekdate;
                }
                if (Range == 'Month') {
                    var dNow = new Date();
                    var utcdate = (dNow.getMonth() + 1) + '/' + (dNow.getDate() + Days) + '/' + dNow.getFullYear();
                    Todate = utcdate;
                    var utcMonthdate = (dNow.getMonth() + 1) + '/' + 1 + '/' + dNow.getFullYear();
                    FromDate = utcMonthdate;
                }

                $('#ContentPlaceHolder1_txtFromDate').val(FromDate);
                $('#ContentPlaceHolder1_txtToDate').val(Todate);
                Status = $('#drpOrderStatuss').val();

                SalesReport(FromDate, Todate, 2);
            }
            else if (From != undefined) {
                FromDate = GetParameterValues('From');
                Todate = GetParameterValues('To');
                Status = GetParameterValues('Status');
                $('#ContentPlaceHolder1_txtFromDate').val(FromDate);
                $('#ContentPlaceHolder1_txtToDate').val(Todate);
                SalesReport(FromDate, Todate, Status);
            }
            else {
                GetSalesReport();
            }
        });
    </script>

    <style type="text/css">
        .dataTables_wrapper{
            margin-top: 15px;
        }
        .overview .overview_card, .overview_num, .overview_text, .overview .overview_card .overview_top{
            cursor:default;
        }
        .overview .overview_card .overview_top div p{
            padding-left:10px;
        }
        .dataTables_length label select{
            border: solid 1px #ccc;
    padding: 15px 8px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    background: #f8f8f8;
    border-radius: 3px;
    color: #666;
    box-shadow: inset 0 1px 0 0px #fff;
    -webkit-box-shadow: inset 0 1px 0 0px #fff;
        }
       .dataTables_wrapper .dataTables_filter input{
           border: solid 1px #ccc;
    padding: 15px 8px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    border-radius: 3px;
    color: #666;
    box-shadow: inset 0 1px 0 0px #fff;
    -webkit-box-shadow: inset 0 1px 0 0px #fff;
       }
        .search-btn{
            background: #d5b937;
    text-shadow: none;
    color: #fff;
        }
        .green{
                background: #047751;
    color: #fff;
    text-shadow: none;
        }
        .overview .overview_card{
            background: rgb(7 135 142);
        }
        .dataTables_wrapper .dataTables_filter{
           position: relative;
    top: 17px;
    right: 0;
    width: auto;
        }
        .form_fields_container li .two_colfields div.form_input input{
                border: solid 1px #ccc;
    padding: 8px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    background: #f8f8f8;
    border-radius: 3px;
    color: #666;
    box-shadow: inset 0 1px 0 0px #fff;
    -webkit-box-shadow: inset 0 1px 0 0px #fff;
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
    </style>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Sales Report</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">

                <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblFrom">From</label>
                        <div class="form_input">
                            <input id="txtFromDate" runat="server" name="filter_date_start" type="date" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="ctl00_ctl00_Main_Main_spmTo">To</span></label>
                        <div class="form_input">
                            <input name="filter_date_end" runat="server" id="txtToDate" type="date" />
                        </div>
                    </div>

                </li>
            </ul>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblOrdrStatus">Order Status</label>
                        <div class="form_input">
                            <div id="drpOrderStatus">
                                <select id="drpOrderStatuss" class="form-control dropdownclass" runat="server">
                                    <option value="2">Please Select</option>
                                    <option value="2">All</option>
                                    <option value="6">Autorized (Ready to ship)</option>
                                    <option value="8">Waiting for Pickup</option>
                                    <option value="9">Dispatched</option>
                                    <option value="3">Delivered</option>
                                    <option value="1">Cancelled</option>
                                    <option value="5">Returns</option>

                                </select>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <input name="btnSearch" value="Search" onclick="GetSalesReport()" class="button_small greyishBtn search-btn" type="button" />
                <asp:Button ID="btnDownload" runat="server" Text="Download Report" CssClass="button_small greyishBtn green" OnClick="btnDownload_Click"  />

            </div>
        </div>
    </div>
    
    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Sales Report</label>
                </h1>
            </div>
            <!--Quick Actions-->

            <div id="divMsg" class="msgbar msg_Success hide_onC" style="display: none;">
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
                    <div id="ctl00_ctl00_Main_Main_divloadingOrders">
                        <center>
                            <img id="imgLoad" alt="" src="Orders_files/progress-indicator.gif">
                        </center>
                    </div>
                </div>
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <div id="divPaymentOrdersGrd"></div>
                </div>

            </div>
            <div class="clear">
            </div>
            <div class="widget">
            </div>


        </div>
    </div>
    <link rel="stylesheet" href="css/jquery-ui.css">

    <script src="js/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#txtFromDate,#txtToDate").datepicker({
                dateFormat: 'mm/dd/yy'
            });
        });
        function getsales() {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "Salesreport.aspx/LoadDashboard?fromdate=2023-01-01&todate=2023-05-05",
                data: "{}",
                dataType: "json",
                success: function () {

                }
            });
        }
    </script>
    <style>
        .dropdownclass{
    border: solid 1px #ccc;
    padding: 8px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    background: #f8f8f8;
    border-radius: 3px;
    color: #666;
    box-shadow: inset 0 1px 0 0px #fff;
    -webkit-box-shadow: inset 0 1px 0 0px #fff;
}
    </style>
</asp:Content>

