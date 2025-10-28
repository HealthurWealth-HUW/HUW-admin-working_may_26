<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Getorders.aspx.cs" Inherits="Admin_Getorders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_007.js"></script>
    <script type="text/javascript" src="Orders_files/form_elements.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_008.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_009.js"></script>
    <script type="text/javascript" src="Orders_files/jquery.js"></script>
    <script type="text/javascript" src="Orders_files/main.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_004.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_003.js"></script>
    <script type="text/javascript" src="Orders_files/ckeditor.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_005.js"></script>
    <script type="text/javascript" src="../Scripts/js/jquery.jqGrid.min.js"></script>

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <script src="/Scripts/js/jquery.dataTables.min.js"></script>
    <link href="/Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/Scripts/js/dataTables.tableTools.js"></script>
    <link href="/Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery.table2excel.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromDate").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected)
                }
            });
            $("#txtToDate").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#txtFromDate").datepicker("option", "maxDate", selected)
                }
            });
            GetSearchorders();
        });
        function GetSearchorders() {

            $.ajax({
                url: '../api/Master/Getorderslist?fromdate=' + getParameterByName('fromdate') + '&todate=' + getParameterByName('todate') + '&Productid=' + getParameterByName('Productid'),
                dataType: 'json',
                success: function (data) {
                    $("#divPaymentSearchOrdersGrd").empty();
                    $('#divPaymentSearchOrdersGrd').append(data.Result);
                }
            });

        }
        function getParameterByName(name, url = window.location.href) {
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
    </script>
    <script type="text/javascript">
        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $('#btnSearch').click();
            }

        });
    </script>
    <script>//to hide the qty row in the table
        setTimeout(function () { $('td:nth-child(4),th:nth-child(4)').hide();; }, 3000);
    </script>

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
    </style>

    <script type="text/javascript">
        function PrintInvoice() {
            window.location = '../Invoice.aspx?ID=' + getParameterByName("transId");
        }
    </script>
    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Search Orders</label>
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
                </div>
                <br />
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <div id="divPaymentSearchOrdersGrd"></div>
                </div>
            </div>
            <div class="clear text_right">
                <button type="button" class="button_small greyishBtn" id="nxtOrders" onclick="GetNextOrders()">Next 20 Records</button>
            </div>
            <div class="widget">
            </div>
        </div>
    </div>
</asp:Content>
