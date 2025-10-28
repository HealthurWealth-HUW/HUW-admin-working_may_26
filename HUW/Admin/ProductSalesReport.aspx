<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProductSalesReport.aspx.cs" Inherits="Admin_ProductSalesReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            var Range = GetParameterValues('Range');

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
                    var utctomorrowdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    Todate = utctomorrowdate;
                }
                if (Range == 'Week') {
                    var dNow = new Date();
                    var utcdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    Todate = utcdate;
                    var firstday = new Date(dNow.setDate((dNow.getDate() + 1) - dNow.getDay()));
                    dNow = firstday;
                    var utcweekdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    FromDate = utcweekdate;
                }
                if (Range == 'Month') {
                    var dNow = new Date();
                    var utcdate = (dNow.getMonth() + 1) + '/' + dNow.getDate() + '/' + dNow.getFullYear();
                    Todate = utcdate;
                    var utcMonthdate = (dNow.getMonth() + 1) + '/' + 1 + '/' + dNow.getFullYear();
                    FromDate = utcMonthdate;
                }

                $('#txtFromDate').val(FromDate);
                $('#txtToDate').val(Todate);

                ProductSalesReport(FromDate, Todate);
            }
            else {
                GetProductSalesReport();
            }
        });
    </script>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Product Sales Report</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">

                <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblFrom">From</label>
                        <div class="form_input">
                            <input id="txtFromDate" name="filter_date_start" type="text" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="ctl00_ctl00_Main_Main_spmTo">To</span></label>
                        <div class="form_input">
                            <input name="filter_date_end" id="txtToDate" type="text" />
                        </div>
                    </div>

                </li>
            </ul>
        </div>   
        
        <div class="widget_body">            
            <div class="action_bar text_right">
                <input name="btnSearch" value="Search" onclick="GetSalesReport()" class="button_small greyishBtn" type="button" />
            </div>
        </div>     
    </div>

    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Product Sales Report</label>
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
    </script>
</asp:Content>

