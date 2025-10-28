<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SearchOrders.aspx.cs" Inherits="Admin_SearchOrders" %>

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
                numberOfMonths: 1,
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected)
                }
            });
            $("#txtToDate").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    $("#txtFromDate").datepicker("option", "maxDate", selected)
                }
            });
  $("#txtFromDatePr").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    $("#txtToDatePr").datepicker("option", "minDate", selected)
                }
            });
            $("#txtToDatePr").datepicker({
                numberOfMonths: 1,
                onSelect: function (selected) {
                    $("#txtFromDatePr").datepicker("option", "maxDate", selected)
                }
            });
            GetSearchorders();
        });

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
        setTimeout(function(){ $('td:nth-child(4),th:nth-child(4)').hide();; }, 3000);
    </script>

     <style type="text/css">
    .overlay {
    background-color:#EFEFEF;
    position: fixed;
    width: 100%;
    height: 100%;
    z-index: 1000;
    top: 0px;
    left: 0px;
    opacity: .5; /* in FireFox */ 
    filter: alpha(opacity=50); /* in IE */
}
    .pleaseWaitText{
        position:absolute;
        top:200px;
        left:50%;
        margin-left:-100px;
        font-size:18px;
        font-weight:800;
        color:red;
    }
 </style>

    <script type="text/javascript">
        function PrintInvoice() {
            window.location = '../Invoice.aspx?ID=' + getParameterByName("transId");
        }
    </script>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Orders</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="lblOrdrNo">Order No.</label>
                        <div class="form_input">
                            <input id="txtOrderNo" class="hasDatepicker" type="text" />
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblSuccessStatus">Success Status</label>
                        <div class="form_input">
                            <div id="ddlSuccessStatus">
                                <select id="ddlSuccess">
                                    <option value="-1">Please Select</option>
                                    <option value="0">Successful</option>
                                    <option value="1">UnSuccessful</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </li>
                 <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblPaymentMode">Payment Mode</label>
                        <div class="form_input">
                            <div id="ddlPaymentMode">
                                <select  id="ddlPaymentModes">
                                    <option value="">Please Select</option>
                                    <option value="Cash On Delivery">Cash On Delivery</option>
                                    <option value="Net Banking/Debit/Credit Card">Citrus</option>
                                    <option value="Paytm">Paytm</option>
                                </select>
                            </div>
                        </div>
                    </div>
                     <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblOrdrStatus">Order Status</label>
                        <div class="form_input">
                            <div id="drpOrderStatus">
                                <select  id="drpOrderStatuss">
                                    <option value="-2">Please Select</option>
                                    <option value="-2">All</option>
                                    <option value="0">Order Pending</option>
                                    <option value="6">Autorized (Ready to ship)</option>
                                    <option value="8">Waiting for Pickup</option>
                                    <option value="9">Dispatched</option>
                                    <option value="3">Delivered</option>
                                    <option value="1">Cancelled</option>
                                    <option value="5">Refund</option>
                                    <option value="4">Refund</option>
                                </select>
                            </div>
                        </div>
                    </div>
                </li>
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
                 <li style="border-top: none;">
                    <div class="two_colfields">
                        <label id="lblShipmentId">Shipment Id</label>
                        <div class="form_input">
                            <input id="txtShipmentId" name="ShipmentId" class="hasDatepicker" type="text" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label id="lblCourierName">Courier</label>
                        <div class="form_input">
                            <select id="ddlCourier" name="CourierName">
                                <option value="0">Select</option>
                                <option value="1">DTDC</option>
                                <option value="2">DotZot</option>
                            </select>
                        </div>
                    </div>
                </li>             
            </ul>
        </div>
        <div style="text-align: center; font-size: 18px; font-weight: bold;">(or)</div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="lblMobile">Mobile No</label>
                        <div class="form_input">
                            <input id="txtMobile" class="hasDatepicker" type="text" />
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <div style="text-align: center; font-size: 18px; font-weight: bold;">(or)</div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="lblProductName">Product Name</label>
                        <div class="form_input">
                            <input id="txtProductName" class="hasDatepicker" type="text" />
                        </div>
                    </div>
 <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblFrom">From</label>
                        <div class="form_input">
                            <input id="txtFromDatePr" name="filter_date_start" type="text" />

                        </div>
                    </div>
                    <div class="two_colfields">
                        <label><span id="ctl00_ctl00_Main_Main_spmTo">To</span></label>
                        <div class="form_input">
                            <input name="filter_date_end" id="txtToDatePr" type="text" />
                        </div>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <input name="btnSearch" value="Search" onclick="GetSearchorders()" class="button_small greyishBtn" type="button" />
                <span>
                    <input name="Button2" value="Clear All" class="button_small greyishBtn" type="submit" /></span>
                <div class="clear"></div>
            </div>
        </div>
    </div>
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
                <input type="button" onclick="toExcel()" style="display: inline-block; margin-left:0px; padding:3px 2px 3px 2px;float:right;margin:0px 20px 0 0;" value="Export to Excel"/>
                <%--<input type="button" onclick="DownloadExcel()" style="display: inline-block; margin-left:20px; padding:3px 2px 3px 2px;float:right;margin:20px 20px 0 0;" value="Export to Tally"/>--%>
                <asp:Button Text="Export To Tally" OnClick="ExportExcel" runat="server" />
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

