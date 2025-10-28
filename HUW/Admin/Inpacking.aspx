<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master"  AutoEventWireup="true" CodeFile="Inpacking.aspx.cs" Inherits="Admin_Inpacking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <script src="js/custom.js"></script>
    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <%--<script src="js/SelectedRowstoExcel.js"></script>--%>
    <script src="js/jquery.table2excel.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            Getinpacking();
        });

        function ExporttoExcel() {
            $("#divPaymentOrdersGrd").table2excel({
                name: "Worksheet Name",
                filename: "Order_Process" //do not include extension
            });
        }
        
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
        function PopupData() {
            vpb_show_login_box();
        }
    </script>

    <script type="text/javascript">
        function DownloadPDF() {
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            var IDS = checkedVals.join(",");
            $('#divMsg').html("PDF is processing. Please Wait...").show();
            vpb_hide_popup_boxes();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/DownlaodPdf?IDS=' + IDS ,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        window.open(data.Result, '_blank');
                        //window.location.href= data.Result;
                        // window.location.href = 'ManiFest.aspx';
                    }
                }
            });
            //alert($('input[class=Check][type=checkbox]:checked').length);
            //$('input[class=Check][type=checkbox]:checked').each(function () {
            //    var pid = PickUPID;
            //    UpdateWaitingForPickOrder($(this).val(), pid);
            //});
        }
        function CreatePickUP(dis) {
            var ShipmentID = $('#txtShipmentID').val();
            var PickUP = $('#txtPickUPID').val();
            var PickUPID;
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            var IDS = checkedVals.join(",");
            $('#divMsg').html("Pickup is processing. Please Wait...").show();
            vpb_hide_popup_boxes();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/CreatePickup?IDS=' + IDS + '&ShipmentID=' + ShipmentID + '&PickUPID=' + PickUP,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        PickUPID = data.Result.ID;
                        $('#divMsg').html("Pickup successfully created.").show();
                        GetWaitingForPickOrder();
                        // window.location.href = 'ManiFest.aspx';
                    }
                    else {
                        PickupID = "";
                        $('#divMsg').html("Pickup successfully created.").show();
                        GetWaitingForPickOrder();
                        //window.location.href = 'ManiFest.aspx';
                    }
                    vpb_hide_popup_boxes();
                }
            });
            //alert($('input[class=Check][type=checkbox]:checked').length);
            //$('input[class=Check][type=checkbox]:checked').each(function () {
            //    var pid = PickUPID;
            //    UpdateWaitingForPickOrder($(this).val(), pid);
            //});
        }
        function Advancedpickup() {
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
             var IDS = checkedVals.join(",");
              $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/Advancedpickup?IDS=' + IDS,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        PickUPID = data.Result.ID;
                        $('#divMsg').html("RPickup successfully created.").show();
                        GetWaitingForPickOrder();
                        // window.location.href = 'ManiFest.aspx';
                    }
                    else {
                        PickupID = "";
                        $('#divMsg').html("Pickup successfully created.").show();
                        GetWaitingForPickOrder();
                        //window.location.href = 'ManiFest.aspx';
                    }
                    vpb_hide_popup_boxes();
                }
            });
            var IDS = checkedVals.join(",");
        }

        function DownloadEcomExcel() {
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/DownloadEcomExcel?OrderIDs=' + checkedVals,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    $('#divEcomExcel').html();
                    $('#divEcomExcel').append(data.Result.m_StringValue);
                    $("#divEcomExcel").table2excel({
                        //exclude: ".noExl",
                        name: "Worksheet Name",
                        filename: "Ecom Orders" //do not include extension
                    });
                }
            });
        }
        function DownloadDelhiveryExcel() {
            var d = new Date();
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/DownloadDelhiveryExcel?OrderIDs=' + checkedVals,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    $('#divEcomExcel').html();
                    $('#divEcomExcel').append(data.Result.m_StringValue);
                    $("#divEcomExcel").table2excel({
                        //exclude: ".noExl",
                        name: "Worksheet Name",
                        //fileExtension: ".xlsx",
                        fileext:".xlsx",
                        filename: d.getDate() + "_HUW_DELHIVERY" //do not include extension
                    });
                }
            });
        }
        function Movettoauthorized() {
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            var IDS = checkedVals.join(",");
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/Movettoauthorized?OrderIDs=' + checkedVals,
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    GetWaitingForPickOrder();
                    
                }
            });
        }
        function AssignDelhiveryAWBS () {
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
             var IDS = checkedVals.join(",");
              $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/AssignDelhiveryAWBS?IDS=' + IDS,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        
                        $('#divMsg').html("Awb numbers assigned successfully.").show();
                        GetWaitingForPickOrder();
                        // window.location.href = 'ManiFest.aspx';
                    }
                    else {
                        PickupID = "";
                        $('#divMsg').html("Awb numbers assigned successfully.").show();
                        GetWaitingForPickOrder();
                        //window.location.href = 'ManiFest.aspx';
                    }
                    vpb_hide_popup_boxes();
                }
            });
            var IDS = checkedVals.join(",");
        }
    </script>


    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">In Packing</label>
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
                    <input type="hidden" id="lblPtrnsId" />
                    <label id="lblSelectedboxes"></label>
                    <input style='display: inline-block;' type='button' onclick='ExporttoExcel()' class='button_small greyishBtn fl_right' value='Export to Excel' />
                    <input style='display: inline-block;' type='button' onclick='Movettoauthorized()' class='button_small greyishBtn fl_right' value='Move back to Authorized' />

                    <asp:Label runat="server" ID="Label3" ForeColor="Red" Font-Bold="true">AWB UnUsed NUMBERS:</asp:Label>
                    COD
                    <asp:Label runat="server" ID="lblAWBCOD" ForeColor="Green" Font-Bold="true" Font-Underline="true"></asp:Label>
                    PPD
                    <asp:Label runat="server" ID="lblAWBPPD" ForeColor="Green" Font-Bold="true" Font-Underline="true"></asp:Label>
                    <div id="divPaymentOrdersGrd"></div>

                </div>

            </div>
            <div class="clear">
            </div>
            <div class="widget">
            </div>


        </div>
    </div>

    <div id="divEcomExcel" class="hide" style="display: none"></div>

    <div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Waiting for Pickup</span>

            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPickUP">
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label1">Shipment ID:</label></td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input name="txtShipmentID" id="txtShipmentID" type="text" style="width: 186px;" /><br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label2">PickUP ID:</label></td>
                            <td style="text-align: left; padding-left: 10px;">

                                <input name="txtPickUPID" id="txtPickUPID" type="text" style="width: 186px;" />
                            </td>
                        </tr>

                    </table>

                    <div class="action_bar text_right">

                        <input value="Ship" onclick='CreatePickUP(this)' id="Submit1" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>


        <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
        <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>

</asp:Content>


