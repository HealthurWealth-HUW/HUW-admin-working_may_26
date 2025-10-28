<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="Admin_OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <style type="text/css">
        .plus, .minus {
            position: absolute;
            right: 30px;
            top: 0;
            margin-top: 30px;
            padding: 5px;
            color: #fff;
            border-radius: 50%;
            background: #38bbc4;
        }
    </style>
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
        //$(document).ready(function () {
        //    setTimeout(function () {
        //        $(".min").click(function () {
        //            $(".widget_body.max").slideToggle();
        //        });
        //        $(".min").css({ "cursor": "pointer" });
        //    }, 3000);
        //});
        function myfunction() {
            if ($('#ContentPlaceHolder1_ddlCancel').val().trim() == 1) {
                $("#Cancelthisordert").show();
                $("#Returnorder").hide();
            }
            if ($('#ContentPlaceHolder1_ddlCancel').val().trim() == 2) {
                $("#Returnorder").show();
                $("#Cancelthisordert").hide();
            }
            if ($('#ContentPlaceHolder1_ddlCancel').val().trim() == 0) {
                $("#Returnorder").hide();
                $("#Cancelthisordert").hide();
            }
        }
        function updatedoctor(transid) {
            $.ajax({
                url: '../api/Master/Updatedoctorname?transid=' + transid + '&doctorname=' + $('#txtdoctorname').val(),
                type: 'post',
                success: function (data) {
                    window.location.reload();
                }
            })
        }
        function PopupDataCancel() {
            vpb_show_login_box2();
        }

        function Refund(dis) {
            var AWBID = $('#txtAwbnumber').val();
            var refundreason = $('#txtrefund').val();
            var PickUPID;
            var checkedVals = getQueryVariable('transId')
            //var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            //var IDS = checkedVals.join(",");
            vpb_hide_popup_boxes();

            $.ajax({
                contentType: "application/json; charset=utf-8",

                url: '../api/Master/Refundorder?IDS=' + checkedVals + '&ShipmentID=' + AWBID + '&Reason=' + refundreason,
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        //PickUPID = data.Result.ID;
                        //$('#divMsg').html("Refund successfully created.").show();
                        //alert(PickUPID);
                        alert('Refund successfully created.')
                        window.location.href = 'OrderDetails.aspx?transId=' + checkedVals;
                        // window.location.href = 'ManiFest.aspx';
                    }
                    else {
                        PickupID = "";
                        $('#divMsg').html("Pickup successfully created.").show();

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
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#vpb_order_pop_up_box').hide();
            var trnsId = getParameterByName("transId");
            GetProductOverview(trnsId);


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
        function PrintMedicineInvoice() {
            window.location = '../MedicineInvoice.aspx?ID=' + getParameterByName("transId");
        }
        function PrintInvoice() {
            window.location = '../Invoice.aspx?ID=' + getParameterByName("transId");
        }

        function MakeOrderSuccess() {
            var trnsId = getParameterByName("transId");
            popupOrderSuccess(trnsId);
        }

        function MakePendingtoSuccess() {
            var payMode = $('#ddlPaymentMode').val();
            var trnsId = getParameterByName("transId");
            PendingtoSuccessOrder(trnsId, $('#txtPGTxnID').val(), $('#txtOTP').val(), payMode);
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
            /** add **/
            padding-left: 0;
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
            /*width: 25%;*/
            /** added **/
            width: 22%;
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
                left: -52%;
                margin-left: 2px;
                position: absolute;
                top: 15px;
                width: 91%;
            }

            .divprocess #progressbar li:first-child:after {
                content: none;
            }

            .divprocess #progressbar li.active:before, #progressbar li.active:after {
                background: none repeat scroll 0 0 #27ae60;
                color: white;
            }

        #right #isvisible {
            display: flex;
        }
    </style>


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
            <div id="content_wrap">

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
            <div id="CBcoupnid" runat="server" style="float: right"></div>
            <div id="wallettransactionsdiv" runat="server">
                <asp:GridView ID="wallettransactionsgrid" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <%--<asp:BoundField DataField="CashbackID" HeaderText="CashbackID" />--%>
                        <asp:BoundField DataField="orderid" HeaderText="Order Number" />
                        <asp:BoundField DataField="date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Date" />
                        <%--<asp:BoundField DataField="totalAmount" HeaderText="total amount" />--%>
                        <asp:BoundField DataField="credit" HeaderText="credit" />
                        <asp:BoundField DataField="debit" HeaderText="debit" />
                        <asp:BoundField DataField="balance" HeaderText="balance" />
                        <%--<asp:BoundField DataField="Userid" HeaderText="userid" />--%>
                        <asp:BoundField DataField="PGTxnid" HeaderText="PGTXNId" />
                        <asp:BoundField DataField="Messages" HeaderText="Message" />
                    </Columns>
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#008000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </div>
            <!-- Block 2 -->
            <div class='one_two_wrap fl_right' style="width: 100%">
                <div class='widget'>
                    <div class='widget_title'>
                        <span id='spnOrderComment' class='iconsweet'>r</span><h5>Post Comment</h5>
                    </div>
                    <div class='widget_body'>
                        <div class='content_pad'>
                            <textarea id="txtcomment" rows="5" style="width: 100%" placeholder="Comment Here"></textarea>
                            <br />
                            <br />
                            <input type="button" class="button_small greyishBtn" id="btnComment" name="btnComment" value="Comment" onclick="JavaScript: OrderComment();" />
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

        >


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
    <div id="vpb_pop_up_background2"></div>
    <div id="vpb_login_pop_up_box2" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Cancel/Refund</span>

            <div class="widget">
                <div class="widget_body">


                    <div class="action_bar text_right">
                        <div style="width: 300px; float: left;" align="left">
                            <div class="input-box">
                                <select name='Select' id='ddlCancel' runat="server" onchange="myfunction()" style="width: 260px;" required="Please enter your Country.">
                                    <option value="0">Please Select</option>
                                    <option value='1'>Cancel </option>
                                    <option value='2'>Return </option>
                                </select>
                            </div>
                        </div>
                        <div id="Returnorder" hidden="hidden">
                            <label id="lblawb">Return AWB Number</label>
                            <input name="txtAwbnumber" id="txtAwbnumber" style="width: 186px;" type="text" /><br />
                            <label id="lblreason">Reason for Return</label>
                            <input name="txtrefund" id="txtrefund" style="width: 186px;" type="text" />
                            <input value="Return" onclick='Refund(this)' id="" class="button_small greyishBtn" type="button" />
                        </div>
                        <div id="Cancelthisordert" hidden="hidden">
                            <input value="CancelOrder" onclick='CancelOrder()' id="" class="button_small greyishBtn" type="button" />

                        </div>
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="" class="button_small greyishBtn" type="button" />

                    </div>
                </div>
            </div>
        </div>


        <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
        <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>

    <script language="javascript">
        $(".min").click(function () {
            setTimeout(function () {
                if ($('.widget_body.max').not(':visible')) {
                    //alert();
                    $(".minus").hide();
                    $(".plus").show();
                }
            }, 2900);
        });
        $(document).ready(function () {

            $('#load').show();
            $("#load").attr("src", "http://ecomexpress.in/tracking/?awb_field=986683898");

            $(".widget_body.max").hide();
            $(".min").click(function () {
                $(".widget_body.max").slideToggle();
                $(".plus").hide();
                $(".minus").show();
            });
        });
        function onGetTransDetails_Submit() {
            window.open("PGStatus.aspx?transId=" + getParameterByName("transId"), '_blank');
        }

        function OrderComment() {
            var Comment = $('#txtcomment').val();
            var trnsId = getParameterByName("transId");

            $.ajax({
                url: '../api/Master/CommentOrder?OrderID=' + trnsId + '&Comment=' + Comment,
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        window.location.href = "OrderDetails.aspx?transId=" + trnsId;
                    }
                    if (data.Status == "NoData") {
                        alert('Comment Not Updated');
                    }
                }
            });
        }
    </script>
    <%-- <script>
        $(document).ready(function () {
            var trnsId = getParameterByName("transId");
            $.ajax({
                url: 'https://secure.paytm.in/oltp/HANDLER_INTERNAL/TXNSTATUS?JsonData={"MID":"SonalE04048888945700","ORDERID":"' + trnsId + '"}',
                type: 'get',
                dataType: 'json',
                success: function (data) {
                }
            });
        });
    </script>--%>
</asp:Content>

