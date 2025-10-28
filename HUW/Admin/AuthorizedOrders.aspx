<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AuthorizedOrders.aspx.cs" Inherits="Admin_AuthorizedOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <%--<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">--%>

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/SelectedRowstoExcel.js"></script>

    <script type="text/javascript">
        debugger
        $(document).ready(function () {
            GetAuthorizedOrders();
        });

        function ExporttoExcel() {
            debugger
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
        function CreateShippment() {
            $('input[class=Check][type=checkbox]:checked').each(function () {
                var Courier = $("#ddlBlkShipperName option:selected").text();
                if (Courier != null || Courier != "" || Courier != undefined) {
                    var CourierName = "";
                    if (Courier == "Ecom Express") {
                        $('#divMsg').html("Shipment is Processing Please wait......").show();
                        ShipmentOrder($(this).val(), Courier);
                    }
                    else {
                        CourierName = Courier;
                        $('#divMsg').html("Shipment is Processing Please wait......").show();
                        ShipmentOrder($(this).val(), Courier);
                    }                                        
                }
            });
            GetAuthorizedOrders();
            vpb_hide_popup_boxes();
        }
    </script>
    <script type="text/javascript">
        function ShipperName() {
            if ($("#ddlBlkShipperName").val() == "4") {
                $('#txtBlkShipperName').show();
                $("#lblRecivedby").show();
            }
            else {
                $('#txtBlkShipperName').hide();
                $("#lblRecivedby").hide();
            }

        }
        function GetComments(transid) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/Getcomments?transid=' + transid,
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            $('#divComments').html(data.Result);
            vpb_show_Comment_box();

        },
        error:
        {
            //Show error message
        }
    });
}
    </script>


    <script type="text/javascript">
        function PopupAndClearControlsData() {
            $("#ddlBlkShipperName").val(0);
            $("#lblRecivedby").hide();
            $('#txtBlkShipperName').hide();
            vpb_show_login_box();
        }
    </script>

    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Authorized Orders</label>
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
                    <label id="lblSelectedboxes"></label>
                    <input style='display: inline-block;' type='button' onclick='ExporttoExcel()' class='button_small greyishBtn fl_right' value='Export to Excel' />

                    <div id="divPaymentOrdersGrd"></div>
                </div>

            </div>
            <div class="clear">
            </div>
            <div class="widget">
            </div>


        </div>
    </div>

    <div id="divComments">

    </div>
    <div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Waiting for Pickup</span>

            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label6">Courier/Shippers Name</label></td>


                            <td style="text-align: left; padding-left: 10px;">
                                <select style="opacity: 10;" name="ddlBlkShipperName" id="ddlBlkShipperName" onchange="ShipperName()">
                                    <option value="0">Select</option>
                                    <option value="1">Ecom Express</option>
                                    <option value="9">Delhivery</option>
                                    <option value="2">Aramex</option>
                                    <option value="3">DTDC</option>
                                    <option value="4">Personal Service</option>
                                    <option value="5">Other</option>
                                    <option value="6">India Post</option>
                                    <option value="7">Fedex</option>
                                    <option value="8">Dotzot</option>
                                </select></td>


                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="lblRecivedby">Received By </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">

                                <input name="txtBlkShipperName" id="txtBlkShipperName" type="text" style="width: 186px;" />
                            </td>
                        </tr>
                    </table>

                    <div class="action_bar text_right">
                        <input value="Ship" onclick='CreateShippment()' id="Submit1" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>


        <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
        <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>
    <link rel="stylesheet" href="css/jquery-ui.css">

    <script src="js/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#txtBlkShipDate").datepicker({
                dateFormat: 'dd-mm-yy'
            });
        });
    </script>
</asp:Content>


