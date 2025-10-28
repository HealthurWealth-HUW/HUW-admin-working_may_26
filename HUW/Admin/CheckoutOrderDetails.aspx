<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckoutOrderDetails.aspx.cs" MasterPageFile="~/Admin/AdminMaster.master" Inherits="Admin_CheckoutOrderDetails" %>

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
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <script src="js/custom.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#vpb_order_pop_up_box').hide();
            var trnsId = getParameterByName("transId");
            GetCheckoutProductOverview(trnsId);
        });
    </script>
    <script type="text/javascript">
        function ValidateEditAddress() {
            var valdteCheck;
            valdteCheck = validate('customValidatorEditAddress');
            if (valdteCheck == true)
                EditUserAddress();
        }
    </script>
    <script type="text/javascript">
        function PrintInvoice() {
           // window.location = '../Invoice.aspx?ID=' + getParameterByName("transId");
        }

        function MakeOrderSuccess() {
            var trnsId = getParameterByName("transId");
            popupCheckoutOrderSuccess(trnsId);
        }
        function CloseOrder() {
            var trnsId = getParameterByName("transId");
            CheckoutOrderClose(trnsId);
        }
        function MakePendingtoSuccess() {
            var payMode = $('#ddlPaymentMode').val();
            var trnsId = getParameterByName("transId");
            CheckouttoSuccessOrder(trnsId, $('#txtPGTxnID').val(), $('#txtOTP').val(), payMode);
        }
    </script>

    <style type="text/css">
        @import "http://fonts.googleapis.com/css?family=Montserrat";

        #tooltip label {
            margin-left: 10px;
            display: none;
            border: solid 1px #7DA7D9;
            padding: 5px;
            background-color: #ffffff;
            position: absolute;
        }

        .divprocess #progressbar {
            counter-reset: step;
            margin-bottom: 10px;
            overflow: hidden;
        }

        .divprocess a {
            line-height: 2;
        }

        .divprocess #progressbar li {
            color: red;
            float: left;
            font-size: 10px;
            list-style-type: none;
            position: relative;
            text-transform: uppercase;
            width: 25%;
            text-align: center;
        }

            .divprocess #progressbar li:before {
                content: counter(step);
                counter-increment: step;
                width: 30px;
                line-height: 30px;
                display: block;
                font-size: 20px;
                color: #fff;
                background: #dddddd;
                border-radius: 3px;
                margin: 0 auto;
            }

            .divprocess #progressbar li:after {
                background: none repeat scroll 0 0 red;
                content: "";
                height: 3px;
                left: -50%;
                margin-left: 2px;
                position: absolute;
                top: 15px;
                width: 100%;
            }

            .divprocess #progressbar li:first-child:after {
                content: none;
            }

            .divprocess #progressbar li.active:before, #progressbar li.active:after {
                background: none repeat scroll 0 0 #27ae60;
                color: white;
            }
    </style>

    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Order Details</label>
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
                            <img id="imgLoad" alt="" src="Orders_files/progress-indicator.gif" />
                        </center>
                    </div>
                </div>
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <input type="hidden" id="lblPtrnsId" />

                    <div id="divOrdersOverviewGrd"></div>
                </div>

            </div>
            <div class="clear">
            </div>
            <div class="widget">
            </div>

<!--Order Status-->
            
            <div class='one_two_wrap fl_right' style="width: 100%" >
                <div class='widget'>
                    <div class='widget_title'>
                        <span class='iconsweet'>r</span><h5>Order Status</h5>
                    </div>
                    <div class='widget_body'>
                        <div class='content_pad'>
                            <select id="ddlOrderStatus">
                                    <option value="Open">Open</option>
                                    <option value="Closed">Closed</option>
                                </select>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Block 2 -->
            <div class='one_two_wrap fl_right' style="width: 100%" >
                <div class='widget'>
                    <div class='widget_title'>
                        <span id='spnOrderComment' class='iconsweet'>r</span><h5>Post Comment</h5>
                    </div>
                    <div class='widget_body'>
                        <div class='content_pad'>
                            <textarea id="txtcomment" rows="5" style="width: 100%" placeholder="Comment Here"></textarea>
                            <br />
                            <input type="button" class="btn-orange" id="btnComment" name="btnComment" value="Comment" onclick="JavaScript: OrderComment();" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Block 2 ends here-->

        </div>
    </div>


    <div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Delivered Orders</span>

            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label1">Shipment ID</label></td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input name="txtShipment" readonly="readonly" id="txtBlkOrderNo" style="width: 186px;" type="text" /></td>


                        </tr>


                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label4">Delivered Date </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input name="Delivered" value="" id="txtDeliveredDate" calendar="ctl00_ctl00_Main_Main_PopCalendar4" format="dd-mmm-yyyy" dir="ltr" autocomplete="off" onfocus="__PopCalSetFocus(this, event);" style="width: 186px;" type="text" />
                                <img src="Orders_files/Calendar.gif" align="absmiddle" border="0">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label6">Received By </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">

                                <input name="txtBlkShipperName" id="txtReceivedBy" type="text" style="width: 186px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label5">Comments  </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <div class="form_input">
                                    <textarea id="txtComments" cols="20" rows="2"></textarea>
                                </div>
                            </td>
                        </tr>
                    </table>

                    <div class="action_bar text_right">

                        <input value="Ship" onclick='UpdateAuthorizedOrders()' id="Submit1" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>


        <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
        <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>


    <div id="vpb_pop_up_background1"></div>
    <div id="vpb_login_pop_up_box1" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Make Payment Success from Admin</span>

            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="lblPaymentType">Payment Mode:</label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <select id="ddlPaymentMode">
                                    <option value="Select">Select</option>
                                    <option value="Paytm">Paytm</option>
                                    <option value="Cash On Delivery">Cash On Delivery</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label">PG Txn ID:</label></td>


                            <td style="text-align: left; padding-left: 10px;">
                                <input type="text" id="txtPGTxnID" placeholder="Enter PG Txn Id" />
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="lblRecivedby">Enter OTP: </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input name="txtOTP" id="txtOTP" placeholder="Enter OTP" type="text" style="width: 186px;" />
                            </td>
                        </tr>
                    </table>

                    <div class="action_bar text_right">
                        <input value="Confirm" onclick='MakePendingtoSuccess()' id="btnOrderSuccess" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="btnClose" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <center>
        <!-- Sign-up and Login Links Starts Here -->
        <br clear="all" />
        <!-- Sign-up and Login Links Ends Here -->
        <!-- Code Begins -->
        <div id="Div1">
        </div>
        <!-- General Pop-up Background -->
        <!-- Sign Up Box Starts Here -->
        <div id="vpb_signup_pop_up_box">
            <input type="hidden" id="addressId" />
            <div align="left" style="font-family: Verdana, Geneva, sans-serif; font-size: 16px; font-weight: bold;">
                Edit Address
            </div>
            <br clear="all">
            <div style="width: 100px; margin-left: 10px; float: left;" align="left">
                Address1:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <input type="text" id="txbEditShippingAddress1" value="" class="vpb_textAreaBoxInputs" validate="customValidatorEditAddress" require="Please enter your Address1.">
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; margin-left: 10px; float: left;" align="left">
                Address2:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <input type="text" id="txbEditShippingAddress2" value="" class="vpb_textAreaBoxInputs" validate="customValidatorEditAddress" require="Please enter your Address2." />
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; margin-left: 10px; float: left;" align="left">
                Nearest Landmark:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <input type="text" id="txbEditShippingLandmark" value="" class="vpb_textAreaBoxInputs" validate="customValidatorEditAddress" require="Please enter your Nearest Landmark.">
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; margin-left: 10px; float: left;" align="left">
                City:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <input type="text" id="txbEditShippingCity" value="" class="vpb_textAreaBoxInputs" validate="customValidatorEditAddress" require="Please enter your City.">
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; margin-left: 10px; float: left;" align="left">
                State:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <div class="input-box">
                    <select id='txbEditShippingState' style="width: 260px;" validate="customValidatorEditAddress" require="Please enter your State.">
                    </select>
                    <input name='country' type='hidden' value='India' />
                </div>
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; padding-top: 10px; margin-left: 10px; float: left;" align="left">
                Country:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <div class="input-box">
                    <select name='country' id='ddlEditShippingCountry' style="width: 260px;" validate="customValidatorEditAddress" require="Please enter your Country.">
                        <option value="">Please Select</option>
                        <option value='1'>India </option>
                    </select>
                </div>
            </div>
            <br clear="all">
            <br clear="all">
            <div style="width: 100px; padding-top: 10px; margin-left: 10px; float: left;" align="left">
                Zip/Postal Code:
            </div>
            <div style="width: 300px; float: left;" align="left">
                <input type="text" id="txbEditShippingPincode" maxlength="10" name="passs" value="" class="vpb_textAreaBoxInputs" validate="customValidatorEditAddress" require="Please enter your Postal Code.">
            </div>
            <br clear="all">
            <label id="lblresult" style="color: #FF0000"></label>
            <%--<asp:Label ID="lblresult" runat="server" ClientIDMode="Static" Text="jhyhg"></asp:Label>--%>
            <%--<input type="text" id="lblresult" value="lblresult" />--%>
            <br clear="all">
            <div style="width: 100px; padding-top: 10px; margin-left: 10px; float: left;" align="left">
                &nbsp;
            </div>
            <div style="width: 300px; float: left;" align="left">
                <a class="vpb_general_button" onclick="ValidateEditAddress()">Submit</a> <a class="vpb_general_button"
                    onclick="vpb_hide_popup_boxes();">Cancel</a>
            </div>
            <br clear="all">
            <br clear="all">
        </div>
        <!-- Sign Up Box Ends Here -->
        <!-- Code Ends -->
        <p style="margin-bottom: 500px;">
        </p>
    </center>

    <script language="javascript">
        function onGetTransDetails_Submit() {
            //window.open("PGStatus.aspx?transId=" + getParameterByName("transId"), '_blank');
        }

        function OrderComment() {
            var OrderStatus = $('#ddlOrderStatus').val();
            var Comment = $('#txtcomment').val();
            var trnsId = getParameterByName("transId");
            var MainTabUrl = "~/Admin/CheckoutSearchOrders.aspx";
            var childWindow = "";

            $.ajax({
                url: '../api/Master/CheckoutCommentOrder?OrderID=' + trnsId + '&Comment=' + Comment+'&OrderStatus='+OrderStatus,
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                    }
                    if (data.Status == "NoData") {
                        alert('Comment Not Updated');
                    }
                }
            });
	$.ajax({
                url: '../api/Master/CheckoutOrderStatus?OrderID=' + trnsId +'&OrderStatus='+OrderStatus,
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
			alert('Comment Updated');
                        window.top.close();
                        childWindow = window.focus(MainTabUrl);
                    }
                    if (data.Status == "NoData") {
                        alert('Comment Not Updated');
                    }
                }
            });
        }

    </script>
   <script>
        $(document).ready(function () {
            var trnsId = getParameterByName("transId");
            $.ajax({
                url: '../api/Master/GetStatusById?id=' + trnsId,
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Closed") {
                        $("div.content_pad select").val("Closed").change();
                    }

                }
            });
        });
    </script>
</asp:Content>
