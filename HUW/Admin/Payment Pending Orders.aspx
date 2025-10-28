<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Payment Pending Orders.aspx.cs" Inherits="Admin_Payment_Pending_Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="Orders_files/default.js"></script>
    <script type="text/javascript" src="Orders_files/OrderSearchoptions.js"></script>
    <script type="text/javascript" src="Orders_files/jquery-jtemplates_uncompressed.js">
</script><script type="text/javascript" src="Orders_files/codepress.js"></script>
<script type="text/javascript" src="Orders_files/jquery-1.js"></script>
<script type="text/javascript" src="Orders_files/jquery-ui-1.js">
</script><link rel="stylesheet" type="text/css" href="Orders_files/demo_table_jui.css">
<link rel="stylesheet" type="text/css" href="Orders_files/jquery_006.css">
<script type="text/javascript" src="Orders_files/html5.js">
</script><script type="text/javascript" src="Orders_files/jquery_006.js">
</script><script type="text/javascript" src="Orders_files/jquery_007.js">
</script><script type="text/javascript" src="Orders_files/form_elements.js">
</script><script type="text/javascript" src="Orders_files/jquery_008.js">
</script><script type="text/javascript" src="Orders_files/jquery_009.js">
</script><script type="text/javascript" src="Orders_files/jquery.js">
</script><script type="text/javascript" src="Orders_files/main.js">
</script><script type="text/javascript" src="Orders_files/jquery_004.js">
</script><script type="text/javascript" src="Orders_files/jquery_003.js">
</script><script type="text/javascript" src="Orders_files/ckeditor.js">
</script><style>.cke{visibility:hidden;}</style><script type="text/javascript" src="Orders_files/swfobject.js">
</script><script type="text/javascript" src="Orders_files/jquery_005.js">
</script><script type="text/javascript" src="Orders_files/validation.js">
</script><script type="text/javascript" src="Orders_files/jquery_010.js">
</script><script type="text/javascript" src="Orders_files/jquery-impromptu.js">
</script><script type="text/javascript" src="Orders_files/kendo.js">
</script>
    <script type="text/javascript" language="javascript">
        var isnonmjmerchant = "False";
        var objDF = 'dd-MMM-yyyy';
        var objDS = '-';
        var objCurSymbol = '';
        var tempX, tempY;
        var Retransactionid;
        var DeliveryOption;
        var TotalPrice;
        var OrderData;
        var globRowIndex;
        var globRowIndex;

        document.onmousemove = getMouseXY;
        function getMouseXY(e) {

            if (window.navigator.appName == 'Microsoft Internet Explorer') {
                tempX = event.screenX;
                tempY = event.screenY;
            }
            else {  // grab the x-y pos.s if browser is NS
                tempX = e.pageX;
                tempY = e.pageY;
            }
            return;
        }

        function CancelOrder(CancelRETid) {
            openCancelOrder();
            document.getElementById('ctl00_ctl00_Main_Main_CancelOrder1_hdnOrderId').value = CancelRETid;
        }

        function CancelProgress() {
        }

        function CancelSuccess() {
            CallUpdatePanel();
            var CancelRETid = document.getElementById('ctl00_ctl00_Main_Main_CancelOrder1_hdnOrderId').value;
            document.getElementById('ctl00_ctl00_Main_Main_PanMsg').innerHTML = "<span class='iconsweet'>=</span>Order No. '" + CancelRETid + "'  has been Cancelled successfully.";
            document.getElementById('ctl00_ctl00_Main_Main_PanMsg').style.display = 'block';
        }

        function CallUpdatePanel() {

            __doPostBack('ctl00$ctl00$Main$Main$HyperLink1', '');
            $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + globRowIndex + ') span:[id$=lblImageload]').attr('innerHTML', '');

        }

        function SaveBankDetails(e, type) {
            $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + globRowIndex + ') span:[id$=lblImageload]').attr('innerHTML', '');



            var flag = false;
            if (type == "enter") {
                if (e.keyCode == 13) {
                    flag = true;
                }
            }
            else
                flag = true;
            if (flag == true) {
                //Making Checkdd Validations False
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkdddate').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkno').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankbranch').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtamount').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtCCdate').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_CustmchkddDateValidation').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_CustmCCDateValidation').enabled = false;
                //Making Bank Transfer Validations True
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtdate').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankrefno').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtbankbranch').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtamount').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_CustmBanktransDateValidation').enabled = true;

                if (Page_ClientValidate("BankTransfer") == true) {

                    var BTTransactionId;
                    var BTdate;
                    var BTType;
                    var BTRefno;
                    var BTBranch;
                    var BTAmount;
                    var Dlvoption;
                    BTTransactionId = Retransactionid;
                    Dlvoption = DeliveryOption;
                    BTdate = ConvertToStandardDate(document.getElementById('ctl00_ctl00_Main_Main_txtobtdate').value, objDF, objDS);
                    BTType = 'BT';
                    BTRefno = document.getElementById('ctl00_ctl00_Main_Main_txtbankrefno').value;
                    BTBranch = document.getElementById('ctl00_ctl00_Main_Main_txtobtbankbranch').value;
                    BTAmount = document.getElementById('ctl00_ctl00_Main_Main_txtobtamount').value;
                    changeOrderstatus(BTTransactionId, BTType, BTdate, BTRefno, BTBranch, BTAmount, Dlvoption);
                    closebanktransfer();
                    CheckDate();

                }
            }
        }

        function CheckDate() {
            $('input[id*=rfvtxtFromDate]').removeAttr('disabled')
            $('input[id*=rfvtxtToDate]').removeAttr('disabled')
            $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + globRowIndex + ') span:[id$=lblImageload]').attr('innerHTML', '');
        }

        function SaveChkDDDetails(e, type)//for taking values
        {
            var flag1 = false;
            if (type == "enter") {
                if (e.keyCode == 13) {
                    flag1 = true;
                }
            }
            else
                flag1 = true;
            if (flag1 == true) {
                //Making Bank and credit card Validations False
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtdate').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankrefno').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtbankbranch').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtamount').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_CustmBanktransDateValidation').enabled = false;
                document.getElementById('ctl00_ctl00_Main_Main_CustmCCDateValidation').enabled = false;
                //Networkorder validations false-shahid-3-8-2010

                //Making CheckDD Validations True
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkdddate').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkno').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankbranch').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_rfvtxtamount').enabled = true;
                document.getElementById('ctl00_ctl00_Main_Main_CustmchkddDateValidation').enabled = true;
                if (Page_ClientValidate("CheckDD") == true) {
                    var ARETransactionId;
                    var Itype;
                    var InstrumentalType;
                    var Tdate;
                    var Tcheckno;
                    var Branch;
                    var Amount;
                    var Delvoption;

                    ARETransactionId = Retransactionid;
                    Delvoption = DeliveryOption;
                    InstrumentalType = document.getElementById('ctl00_ctl00_Main_Main_drpinstrumenttype').value;
                    Tdate = ConvertToStandardDate(document.getElementById('ctl00_ctl00_Main_Main_txtchkdddate').value, objDF, objDS);
                    Tcheckno = document.getElementById('ctl00_ctl00_Main_Main_txtchkno').value;
                    Branch = document.getElementById('ctl00_ctl00_Main_Main_txtbankbranch').value;
                    Amount = document.getElementById('ctl00_ctl00_Main_Main_txtamount').value;
                    changeOrderstatus(ARETransactionId, InstrumentalType, Tdate, Tcheckno, Branch, Amount, Delvoption);

                    closechkDD();
                    CheckDate();
                }
            }
        }

        function changeOrderstatus(ARETransactionId, InstrumentalType, Tdate, Tcheckno, Branch, Amount, Delivoption)//ajax call
        {
            $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + (globRowIndex + 1) + ') span:[id$=lblImageload]').attr('innerHTML', '<img src="../images/indicator.gif" />');
            $.ajax({
                type: "POST",
                url: "UpdatePopUp",
                data: "{'RETransactionId':'" + ARETransactionId + "','RETransactionType':'" + InstrumentalType + "','PaymentDate':'" + Tdate + "','ChequeRefNo':'" + Tcheckno + "','DrawnOn':'" + Branch + "','Amount':'" + Amount + "','DeliveryOption':'" + Delivoption + "'}",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Content-type",
                           "application/json; charset=utf-8");
                },
                dataType: "json",
                success: function (msg) {
                    DisplayMessage(msg, ARETransactionId, Delivoption);
                    CallUpdatePanel();
                }

            });

        }

        function DisplayMessage(msg, ARETransactionId, Delivoption) {
            $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + globRowIndex + ') span:[id$=lblImageload]').attr('innerHTML', '');
            if (msg == 'false') {
                document.getElementById('ctl00_ctl00_Main_Main_PanErrMsg').innerHTML = "<span class='iconsweet'>X</span>Due to less stock availability your Order No " + ARETransactionId + " is unable to authorize .";
                document.getElementById('ctl00_ctl00_Main_Main_PanErrMsg').style.display = 'block';
                document.getElementById('ctl00_ctl00_Main_Main_PanMsg').style.display = 'none';

            }
            else {

                if (Delivoption == 'pickup') {
                    document.getElementById('ctl00_ctl00_Main_Main_PanMsg').innerHTML = "<span class='iconsweet'>=</span>Order No. '" + ARETransactionId + "' has been authorized successfully and moved to Ready For Packaging List.";
                }
                else {
                    document.getElementById('ctl00_ctl00_Main_Main_PanMsg').innerHTML = "<span class='iconsweet'>=</span>Order No. '" + ARETransactionId + "' has been authorized successfully.";
                }

                document.getElementById('ctl00_ctl00_Main_Main_PanMsg').style.display = 'block';
                document.getElementById('ctl00_ctl00_Main_Main_PanErrMsg').style.display = 'none';
            }
        }

        function openchkDD(Rtid, DeliveryOpt, OrderDate1, RowIndex) {
            globRowIndex = RowIndex;
            Retransactionid = Rtid;
            DeliveryOption = DeliveryOpt;
            OrderData = OrderDate1;

            document.getElementById('ctl00_ctl00_Main_Main_drpinstrumenttype').value = "ALL";
            document.getElementById('ctl00_ctl00_Main_Main_txtchkdddate').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtchkno').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtbankbranch').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtamount').value = "";
            comclick("divchkdd");
            document.getElementById('drpcheckouttype').style.visibility = "hidden";
        }

        function ChkDDdateValidation(source, arguments) {
            var dtSeperator = '-';
            var dtformat = 'dd MMM yyyy';
            var Todate = document.getElementById('ctl00_ctl00_Main_Main_txtchkdddate').value;
            var fromDate = OrderData;
            var valid = validateDatesNew(fromDate, Todate, dtSeperator, dtformat);
            if (valid == true) {
                arguments.IsValid = true;
            } else {
                arguments.IsValid = false;
            }
        }

        function closechkDD() {
            var id = "ctl00_ctl00_Main_Main_divchkdd";
            $("#" + id).dialog("close");
            document.getElementById('drpcheckouttype').style.visibility = "visible";
        }

        function openbanktransfer(Rtid, DeliveryOpt, OrderDate1, RowIndex) {
            globRowIndex = RowIndex;
            Retransactionid = Rtid;
            OrderData = OrderDate1;
            DeliveryOption = DeliveryOpt;

            document.getElementById('ctl00_ctl00_Main_Main_txtobtdate').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtbankrefno').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtobtbankbranch').value = "";
            document.getElementById('ctl00_ctl00_Main_Main_txtobtamount').value = "";
            comclick("divbanktransfer");
            document.getElementById('drpcheckouttype').style.visibility = "hidden";
        }
        function BankTransdateValidation(source, arguments) {
            var dtSeperator = '-';
            var dtformat = 'dd MMM yyyy';
            var Todate = document.getElementById('ctl00_ctl00_Main_Main_txtobtdate').value;
            var fromDate = OrderData;
            var valid = validateDatesNew(fromDate, Todate, dtSeperator, dtformat);
            if (valid == true) {
                arguments.IsValid = true;
            } else {
                arguments.IsValid = false;
            }
        }

        function closebanktransfer() {

            var id = "ctl00_ctl00_Main_Main_divbanktransfer";
            $("#" + id).dialog("close");
            document.getElementById('drpcheckouttype').style.visibility = "visible"
        }

        function closecreditcard() {
            document.getElementById('ctl00_ctl00_Main_Main_txtNewConvText').value = '';
            document.getElementById('ctl00_ctl00_Main_Main_txtCCdate').value = '';
            var id = "ctl00_ctl00_Main_Main_divcreditcard";
            $("#" + id).dialog("close");
            document.getElementById('drpcheckouttype').style.visibility = "visible";
        }
        $('#ctl00_ctl00_Main_Main_divPendingOrders tr:eq(' + globRowIndex + ') span:[id$=lblImageload]').attr('innerHTML', '');

        function chkLength(txt) {
            chkCharsRem(txt.id);
        }

        function chkCharsRem(txtSMS) {
            var txtMsg = document.getElementById(txtSMS);
            var txtCh = document.getElementById('ctl00_ctl00_Main_Main_txtTempCharsRem');
            if (txtMsg.value.length >= 500) {
                txtMsg.value = txtMsg.value.substring(0, 500);
            }
            txtCh.value = txtMsg.value.length;
        }

        function Savecc(e, type) {
            var validated = Page_ClientValidate('CreditCard');
            if (validated == false) {
                return false;
            }
            var flag1 = false;
            if (type == "enter") {
                if (e.keyCode == 13) {
                    flag1 = true;
                }
            }
            else
                flag1 = true;
            if (flag1 == true) {
                if (document.getElementById('ctl00_ctl00_Main_Main_txtNewConvText').value.length > 0) {
                    var OrderNo = Retransactionid;
                    var ConvText = document.getElementById('ctl00_ctl00_Main_Main_txtNewConvText').value;
                    var ConvDate = document.getElementById('ctl00_ctl00_Main_Main_txtCCdate').value;
                    var ConvType = "any";
                    var ConvLogID = document.getElementById('ctl00_ctl00_Main_Main_txtConvLogID').value;
                    var convtId = "";

                    if (document.getElementById('ctl00_ctl00_Main_Main_txtConvLogID').value > 0) {
                        convtId = ConvLogID;
                    }
                    else {
                        convtId = 0;
                    }
                    $(document.getElementById(OrderNo)).addClass('loading');
                    $.ajax(
                {
                    type: "POST",
                    url: "UpdatePopup",
                    data: "{'Insert':'Insert','ConvLogID':'" + convtId + "','ConvText':'" + ConvText + "','ConvDate':'" + ConvDate + "','OrderNo':'" + OrderNo + "','ConvType':'" + ConvType + "'}",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("Content-type", "application/json; charset=utf-8");
                    },
                    dataType: "json",
                    success: function (msg) {
                        document.getElementById('ctl00_ctl00_Main_Main_txtNewConvText').value = "";
                        document.getElementById('ctl00_ctl00_Main_Main_txtConvLogID').value = "";
                    }
                });
                    var ARETransactionId;
                    var Itype;
                    var InstrumentalType;
                    var Tdate;
                    var Tcheckno;
                    var Branch;
                    var Amount;
                    var Delvoption;

                    ARETransactionId = Retransactionid;
                    Delvoption = DeliveryOption;
                    InstrumentalType = 'CC';
                    Tdate = ConvertToStandardDate(document.getElementById('ctl00_ctl00_Main_Main_txtCCdate').value, objDF, objDS);
                    Tcheckno = '';
                    Branch = '';
                    Amount = 0;
                    changeOrderstatus(ARETransactionId, InstrumentalType, Tdate, Tcheckno, Branch, Amount, Delvoption);
                    var id = "ctl00_ctl00_Main_Main_divcreditcard";
                    $("#" + id).dialog("close");
                }
                else {
                    alert('Please enter comments.');
                    return false;
                }
                return false;
            }
        }

        function getdivCurrency() {
            var p = document.getElementById('divCurrency');
            if (p != null)
                document.getElementById('divCurrency').innerHTML = "Order Value";
        }

        function CCdateValidation(source, arguments) {
            var dtSeperator = '-';
            var dtformat = 'dd MMM yyyy';
            var Todate = document.getElementById('ctl00_ctl00_Main_Main_txtCCdate').value;
            var fromDate = OrderData;
            var valid = validateDatesNew(fromDate, Todate, dtSeperator, dtformat);
            if (valid == true) {
                arguments.IsValid = true;
            } else {
                arguments.IsValid = false;
            }
        }
    </script>

<link href="Orders_files/jquery_004.css" rel="stylesheet" type="text/css"><link href="Orders_files/kendo_002.css" rel="stylesheet" type="text/css"><link href="Orders_files/kendo.css" rel="stylesheet" type="text/css"><link href="Orders_files/reset.css" rel="stylesheet" type="text/css"><link href="Orders_files/main.css" rel="stylesheet" type="text/css"><link href="Orders_files/typography.css" rel="stylesheet" type="text/css"><link href="Orders_files/tipsy.css" rel="stylesheet" type="text/css"><link href="Orders_files/jquery_005.css" rel="stylesheet" type="text/css"><link href="Orders_files/jquery_002.css" rel="stylesheet" type="text/css"><link href="Orders_files/fullcalendar.css" rel="stylesheet" type="text/css"><link href="Orders_files/bootstrap.css" rel="stylesheet" type="text/css"><link href="Orders_files/highlight.css" rel="stylesheet" type="text/css"><link href="Orders_files/uploadify.css" rel="stylesheet" type="text/css"><link href="Orders_files/jquery.css" type="text/css" rel="stylesheet"><link href="Orders_files/jquery_003.css" type="text/css" rel="stylesheet"><link href="Orders_files/WebResource.css" type="text/css" rel="stylesheet"></head>

<form name="aspnetForm" method="post" action="PendingOrders" onsubmit="javascript:return WebForm_OnSubmit();" id="aspnetForm">
<div>
<input name="__EVENTTARGET" id="__EVENTTARGET" value="" type="hidden">
<input name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" type="hidden">
<input name="__LASTFOCUS" id="__LASTFOCUS" value="" type="hidden">
<input name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwUKLTg3MDg3NjEwMQ8WAh4KTWVyY2hhbnRJZAUkMGNmOWZkZDItMTQ1Yi00MTRiLTkyMTItY2VhMWY1NmEzODY0FgJmDw8WAh4MU2VsZWN0ZWRMaW5rZWQWAmYPZBYEAgIPZBYCAgEPFgIeBFRleHQF6hg8bGluayByZWw9J2ljb24nIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwLy9JbWFnZXMvTWFydGphY2svRmF2SWNvbi5pY28nIHR5cGU9J2ltYWdlL3gtaWNvbicgLz48bGluayByZWw9J3Nob3J0Y3V0IGljb24nIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwLy9JbWFnZXMvTWFydGphY2svRmF2SWNvbi5pY28nIHR5cGU9J2ltYWdlL3gtaWNvbicgLz48c2NyaXB0IHR5cGU9J3RleHQvamF2YXNjcmlwdCcgc3JjPSdodHRwOi8vc2NyaXB0Lm1hcnRqYWNraG9zdGluZy5jb20vWm9uZTAxL2NwLWpzL0pRdWVyeS9qcXVlcnktanRlbXBsYXRlc191bmNvbXByZXNzZWQuanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9Jy9hc3BuZXRfY2xpZW50L2NvZGVwcmVzcy9jb2RlcHJlc3MuanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2pxdWVyeS0xLjcuMi5taW4uanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2pxdWVyeS11aS0xLjguMjEuY3VzdG9tLm1pbi5qcyc+PC9zY3JpcHQ+PGxpbmsgcmVsPSdzdHlsZXNoZWV0JyB0eXBlPSd0ZXh0L2NzcycgaHJlZj0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvanFfdGFibGVzL2RlbW9fdGFibGVfanVpLmNzcyc+PC9saW5rPjxsaW5rIHJlbD0nc3R5bGVzaGVldCcgdHlwZT0ndGV4dC9jc3MnIGhyZWY9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2ZhbmN5Ym94L2pxdWVyeS5mYW5jeWJveC0xLjMuNC5jc3MnPjwvbGluaz48c2NyaXB0IHR5cGU9J3RleHQvamF2YXNjcmlwdCcgc3JjPSdodHRwOi8vc2NyaXB0Lm1hcnRqYWNraG9zdGluZy5jb20vWm9uZTAxL2NwLWpzL0xpYi9odG1sNS5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvanF1ZXJ5LnRpcHN5LmpzJz48L3NjcmlwdD48c2NyaXB0IHR5cGU9J3RleHQvamF2YXNjcmlwdCcgc3JjPSdodHRwOi8vc2NyaXB0Lm1hcnRqYWNraG9zdGluZy5jb20vWm9uZTAxL2NwLWpzL0xpYi9qcXVlcnkuYXV0b2dyb3d0ZXh0YXJlYS5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvZm9ybV9lbGVtZW50cy5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvZmFuY3lib3gvanF1ZXJ5LmZhbmN5Ym94LTEuMy40LnBhY2suanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2ZhbmN5Ym94L2pxdWVyeS5tb3VzZXdoZWVsLTMuMC40LnBhY2suanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2pxX3RhYmxlcy9qcXVlcnkuZGF0YVRhYmxlcy5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvbWFpbi5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvanF1ZXJ5LmVhc2luZy4xLjMuanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvTGliL2NsX2VkaXRvci9qcXVlcnkuY2xlZGl0b3IubWluLmpzJz48L3NjcmlwdD48c2NyaXB0IHR5cGU9J3RleHQvamF2YXNjcmlwdCcgc3JjPSdodHRwOi8vc2NyaXB0Lm1hcnRqYWNraG9zdGluZy5jb20vWm9uZTAxL2NwLWpzL2Zja2VkaXRvci9ja2VkaXRvci5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvdXBsb2FkaWZ5L3N3Zm9iamVjdC5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9MaWIvdXBsb2FkaWZ5L2pxdWVyeS51cGxvYWRpZnkudjIuMS40Lm1pbi5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9KUXVlcnkvdmFsaWRhdGlvbi5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9KUXVlcnkvanF1ZXJ5LmF1dG9jb21wbGV0ZS5qcyc+PC9zY3JpcHQ+PHNjcmlwdCB0eXBlPSd0ZXh0L2phdmFzY3JpcHQnIHNyYz0naHR0cDovL3NjcmlwdC5tYXJ0amFja2hvc3RpbmcuY29tL1pvbmUwMS9jcC1qcy9KUXVlcnkvanF1ZXJ5LWltcHJvbXB0dS4xLjUuanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0ndGV4dC9qYXZhc2NyaXB0JyBzcmM9J2h0dHA6Ly9zY3JpcHQubWFydGphY2tob3N0aW5nLmNvbS9ab25lMDEvY3AtanMvSlF1ZXJ5L2tlbmRvLndlYi5taW4uanMnPjwvc2NyaXB0PjxzY3JpcHQgdHlwZT0idGV4dC9qYXZhc2NyaXB0Ij5pZiAodHlwZW9mIE1hcnRKYWNrID09ICd1bmRlZmluZWQnKSB7TWFydEphY2sgPSB7fTt9TWFydEphY2suQ29uZmlnID0geyAnTWVyY2hhbnRJZCc6ICcwY2Y5ZmRkMi0xNDViLTQxNGItOTIxMi1jZWExZjU2YTM4NjQnIH07PC9zY3JpcHQ+ZAIED2QWCgIBDxYCHgNzcmMFQWh0dHA6Ly9jc3MubWFydGphY2tob3N0aW5nLmNvbS9jcF90aGVtZXN6b25lMDIvZHcvaW1hZ2VzL2xvZ28ucG5nZAIDDw8WAh8CBRhXZWxjb21lIElicmFoaW0gTW9oYW1tZWRkZAIGDxYCHwJkZAIIDxYCHgRocmVmBR5odHRwOi8vd3d3LmxldmVsMi5tYXJ0amFjay5jb21kAg0PZBYIZg8WAh8CBdoJPGxpIGlkPSdsaW5rXzFfRGFzaGJvYXJkICYgU2V0dXAnIGNsYXNzPSduYXZfMScgPjxhIG9uY2xpY2s9Z2V0Y2hpbGRsaW5rKHRoaXMpIGlkPSdsaW5rX0Rhc2hib2FyZCAmIFNldHVwJyBydW5hdD0nc2VydmVyJyBocmVmPScjJz5EYXNoYm9hcmQgJiBTZXR1cDwvYT48L2xpPjxsaSBpZD0nbGlua18yX09yZGVycyAmIExlYWRzJyBjbGFzcz0nbmF2XzIgYWN0aXZlJyA+PGEgb25jbGljaz1nZXRjaGlsZGxpbmsodGhpcykgaWQ9J2xpbmtfT3JkZXJzICYgTGVhZHMnIHJ1bmF0PSdzZXJ2ZXInIGhyZWY9JyMnPk9yZGVycyAmIExlYWRzPC9hPjwvbGk+PGxpIGlkPSdsaW5rXzNfUHJvZHVjdHMnIGNsYXNzPSduYXZfMycgPjxhIG9uY2xpY2s9Z2V0Y2hpbGRsaW5rKHRoaXMpIGlkPSdsaW5rX1Byb2R1Y3RzJyBydW5hdD0nc2VydmVyJyBocmVmPScjJz5Qcm9kdWN0czwvYT48L2xpPjxsaSBpZD0nbGlua181X0Rlc2lnbiAmIENvbnRlbnQnIGNsYXNzPSduYXZfNScgPjxhIG9uY2xpY2s9Z2V0Y2hpbGRsaW5rKHRoaXMpIGlkPSdsaW5rX0Rlc2lnbiAmIENvbnRlbnQnIHJ1bmF0PSdzZXJ2ZXInIGhyZWY9JyMnPkRlc2lnbiAmIENvbnRlbnQ8L2E+PC9saT48bGkgaWQ9J2xpbmtfNl9NYXJrZXRpbmcnIGNsYXNzPSduYXZfNicgPjxhIG9uY2xpY2s9Z2V0Y2hpbGRsaW5rKHRoaXMpIGlkPSdsaW5rX01hcmtldGluZycgcnVuYXQ9J3NlcnZlcicgaHJlZj0nIyc+TWFya2V0aW5nPC9hPjwvbGk+PGxpIGlkPSdsaW5rXzdfU2V0dGluZ3MnIGNsYXNzPSduYXZfNycgPjxhIG9uY2xpY2s9Z2V0Y2hpbGRsaW5rKHRoaXMpIGlkPSdsaW5rX1NldHRpbmdzJyBydW5hdD0nc2VydmVyJyBocmVmPScjJz5TZXR0aW5nczwvYT48L2xpPjxsaSBpZD0nbGlua184X0N1c3RvbWVycycgY2xhc3M9J25hdl84JyA+PGEgb25jbGljaz1nZXRjaGlsZGxpbmsodGhpcykgaWQ9J2xpbmtfQ3VzdG9tZXJzJyBydW5hdD0nc2VydmVyJyBocmVmPScjJz5DdXN0b21lcnM8L2E+PC9saT48bGkgaWQ9J2xpbmtfOV9lWGNoYW5nZScgY2xhc3M9J25hdl85JyA+PGEgb25jbGljaz1nZXRjaGlsZGxpbmsodGhpcykgaWQ9J2xpbmtfZVhjaGFuZ2UnIHJ1bmF0PSdzZXJ2ZXInIGhyZWY9JyMnPmVYY2hhbmdlPC9hPjwvbGk+PGxpIGlkPSdsaW5rXzEwX0FwcHMnIGNsYXNzPSduYXZfMTAnID48YSBvbmNsaWNrPWdldGNoaWxkbGluayh0aGlzKSBpZD0nbGlua19BcHBzJyBydW5hdD0nc2VydmVyJyBocmVmPScjJz5BcHBzPC9hPjwvbGk+ZAIFDxYCHwIF47IBPHVsIGlkPSdEYXNoYm9hcmQgJiBTZXR1cCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvRGVmYXVsdCc+PHNwYW4+PC9zcGFuPjxpbnM+RGFzaCBCb2FyZDwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGFzaGJvYXJkICYgU2V0dXAnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMzMnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlSW5mb3JtYXRpb24vR2V0dGluZ1N0YXJ0ZWQ/c291cmNlPUdTJz48c3Bhbj48L3NwYW4+PGlucz5HZXR0aW5nIFN0YXJ0ZWQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rhc2hib2FyZCAmIFNldHVwJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTI1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TdG9yZUluZm9ybWF0aW9uL1N0b3JlQWRkcmVzcz9zb3VyY2U9R1MnPjxzcGFuPjwvc3Bhbj48aW5zPlN0b3JlIEluZm9ybWF0aW9uPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEYXNoYm9hcmQgJiBTZXR1cCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEyNicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTGF5b3V0L1NldFRlbXBsYXRlP3NvdXJjZT1HUyc+PHNwYW4+PC9zcGFuPjxpbnM+U2VsZWN0IFRlbXBsYXRlPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEYXNoYm9hcmQgJiBTZXR1cCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEyNycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU3RvcmVTZXR0aW5nL0NoZWNrT3V0T3B0aW9ucz9zb3VyY2U9R1MnPjxzcGFuPjwvc3Bhbj48aW5zPkNoZWNrb3V0IE9wdGlvbnM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rhc2hib2FyZCAmIFNldHVwJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTI4JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TdG9yZVNldHRpbmcvV1NDaGVja091dE9wdGlvbj9zb3VyY2U9R1MnPjxzcGFuPjwvc3Bhbj48aW5zPkNhbGwgZm9yIEFjdGlvbnM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rhc2hib2FyZCAmIFNldHVwJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTMwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvQWRkQ3VzdG9tQ2F0ZWdvcnk/c291cmNlPUdTJz48c3Bhbj48L3NwYW4+PGlucz5DYXRlZ29yeSBNYW5hZ2VtZW50PC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEYXNoYm9hcmQgJiBTZXR1cCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEzMScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvQ2F0bG9nL2FkZHByb2R1Y3Q/UHJvZHVjdFR5cGU9UCZzb3VyY2U9R1MnPjxzcGFuPjwvc3Bhbj48aW5zPkFkZCBOZXcgUHJvZHVjdDwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGFzaGJvYXJkICYgU2V0dXAnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMzInIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0xheW91dC9kbmQvZG5kcGFnZT9wYWdlaWQ9Mjk4NTgnPjxzcGFuPjwvc3Bhbj48aW5zPkRlc2lnbiBIb21lIFBhZ2U8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rhc2hib2FyZCAmIFNldHVwJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTQ1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9MYXlvdXQvRG9tYWluJz48c3Bhbj48L3NwYW4+PGlucz5Cb29rIERvbWFpbjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGFzaGJvYXJkICYgU2V0dXAnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsyMzgnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1NoaXBwaW5nUHJvZmlsZS9TaGlwcGluZ0NvZGVQcm9maWxlJz48c3Bhbj48L3NwYW4+PGlucz5TaGlwcGluZyBQcm9maWxlPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEYXNoYm9hcmQgJiBTZXR1cCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT48L3VsPjx1bCBpZD0nT3JkZXJzICYgTGVhZHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpIGNsYXNzPSdhY3RpdmUnID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L1BlbmRpbmdPcmRlcnMnPjxzcGFuPjwvc3Bhbj48aW5zPlBheW1lbnQgUGVuZGluZzwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nT3JkZXJzICYgTGVhZHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms0JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvU2hpcHBpbmdPcmRlcnMnPjxzcGFuPjwvc3Bhbj48aW5zPkF1dGhvcml6ZWQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J09yZGVycyAmIExlYWRzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rNScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L0NvbXBsZXRlZE9yZGVycz9vcmRTdGF0dXM9Uyc+PHNwYW4+PC9zcGFuPjxpbnM+V2FpdGluZyBmb3IgUGlja3VwPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdPcmRlcnMgJiBMZWFkcycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazI3NScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L2Rpc3BhdGNoZWRvcmRlcnM/b3JkU3RhdHVzPVMnPjxzcGFuPjwvc3Bhbj48aW5zPkRpc3BhdGNoZWQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J09yZGVycyAmIExlYWRzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rNicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L0NvbXBsZXRlZE9yZGVycz9vcmRTdGF0dXM9RCc+PHNwYW4+PC9zcGFuPjxpbnM+RGVsaXZlcmVkPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdPcmRlcnMgJiBMZWFkcycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazcnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL09yZGVyTWFuYWdlbWVudC9PcmRlckZpbHRyYXRpb24nPjxzcGFuPjwvc3Bhbj48aW5zPlNlYXJjaCBPcmRlcjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nT3JkZXJzICYgTGVhZHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmszJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvU3RvY2tQZW5kaW5nT3JkZXJzJz48c3Bhbj48L3NwYW4+PGlucz5TdG9jayBQZW5kaW5nPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdPcmRlcnMgJiBMZWFkcycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvb3JkZXJyZXR1cm5zJz48c3Bhbj48L3NwYW4+PGlucz5PcmRlciBSZXR1cm5zPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdPcmRlcnMgJiBMZWFkcycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazI4NCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L0NhbmNlbGxlZE9yZGVycyc+PHNwYW4+PC9zcGFuPjxpbnM+Q2FuY2VsbGVkIE9yZGVyczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nT3JkZXJzICYgTGVhZHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L0NyZWF0ZW9yZGVyJz48c3Bhbj48L3NwYW4+PGlucz5DcmVhdGUgT3JkZXI8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J09yZGVycyAmIExlYWRzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMjgzJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvRnVsbGZpbGxtZW50SXNzdWVPcmRlcnMnPjxzcGFuPjwvc3Bhbj48aW5zPk9yZGVycyB3aXRoIElzc3VlczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nT3JkZXJzICYgTGVhZHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTGVhZE1hbmFnZW1lbnQvTGVhZERhc2hib2FyZCc+PHNwYW4+PC9zcGFuPjxpbnM+TGVhZCBEYXNoYm9hcmQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J09yZGVycyAmIExlYWRzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTMnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0xlYWRNYW5hZ2VtZW50L0xlYWRSZXBvcnQnPjxzcGFuPjwvc3Bhbj48aW5zPkxlYWQgUmVwb3J0PC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdPcmRlcnMgJiBMZWFkcycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGxhYmVsICBpZD0nTGluazI2Mic+PHNwYW4+PC9zcGFuPjxpbnM+UmVwb3J0czwvaW5zPjwvbGFiZWw+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J09yZGVycyAmIExlYWRzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PGxpPjxhIGlkPSdTdWJMaW5rMjYyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvamFzcGVycmVwb3J0LmFzcHg/dHlwZT1Qcm9kdWN0ICc+PHNwYW4+PC9zcGFuPjxpbnM+UHJvZHVjdCBMaXN0IFJlcG9ydDwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazI4NScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvT3JkZXJNYW5hZ2VtZW50L0Rvd25sb2FkUmVwb3J0cyAnPjxzcGFuPjwvc3Bhbj48aW5zPlNoaXBwaW5nIE1hbmlmZXN0PC9pbnM+PC9hPjwvbGk+PC91bD48L2xpPjwvdWw+PHVsIGlkPSdQcm9kdWN0cycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTQnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0NhdGxvZy9BZGRDdXN0b21DYXRlZ29yeT9zb3VyY2U9UkdTJz48c3Bhbj48L3NwYW4+PGlucz5DYXRlZ29yeSBNYW5hZ2VtZW50PC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdQcm9kdWN0cycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazE1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvYWRkcHJvZHVjdD9Qcm9kdWN0VHlwZT1QJz48c3Bhbj48L3NwYW4+PGlucz5BZGQgTmV3IFByb2R1Y3Q8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1Byb2R1Y3RzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTYnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0NhdGxvZy9Qcm9kdWN0TGlzdD9Qcm9kdWN0VHlwZT1QJz48c3Bhbj48L3NwYW4+PGlucz5Qcm9kdWN0IExpc3Q8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1Byb2R1Y3RzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rMjA1Jz48c3Bhbj48L3NwYW4+PGlucz5NYXN0ZXJzPC9pbnM+PC9sYWJlbD48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nUHJvZHVjdHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48bGk+PGEgaWQ9J1N1YkxpbmsyMDUnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL2NhdGxvZy9Qcm9kdWN0c1RhZy5hc3B4ICc+PHNwYW4+PC9zcGFuPjxpbnM+UHJvZHVjdCBUYWdzPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjAzJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9jYXRsb2cvQWRkQnJhbmRzICc+PHNwYW4+PC9zcGFuPjxpbnM+QnJhbmRzPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjInIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL2NhdGxvZy9BZGRWYXJpYW50cyAnPjxzcGFuPjwvc3Bhbj48aW5zPkJ1bGsgVmFyaWFudCBQcm9wZXJ0aWVzPC9pbnM+PC9hPjwvbGk+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rMjUnPjxzcGFuPjwvc3Bhbj48aW5zPkJ1bGsgVG9vbHM8L2lucz48L2xhYmVsPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdQcm9kdWN0cycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjxsaT48YSBpZD0nU3ViTGluazI1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvQnVsa0V4Y2VsRG93bmxvYWRTdW1tYXJ5LmFzcHggJz48c3Bhbj48L3NwYW4+PGlucz5CdWxrIFByb2R1Y3QgRG93bmxvYWQ8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsyNicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvQ2F0bG9nL0FkZEJ1bGtQcm9kdWN0ICc+PHNwYW4+PC9zcGFuPjxpbnM+QnVsayBQcm9kdWN0IFVwbG9hZDwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazI3JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvUHJvZHVjdEF0dHJpYnV0ZXMgJz48c3Bhbj48L3NwYW4+PGlucz5CdWxrIEF0dHJpYnV0ZSBVcGxvYWQ8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsxMzQnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0NhdGxvZy9Eb3dubG9hZERlZmF1bHREYXRhICc+PHNwYW4+PC9zcGFuPjxpbnM+RGVmYXVsdCBEYXRhPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjk2JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9jYXRsb2cvQ2F0ZWdvcnlBdHRyaWJ1dGVzICc+PHNwYW4+PC9zcGFuPjxpbnM+QnVsayBDYXRlZ29yeSBBdHRyaWJ1dGU8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsyOTYnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL2NhdGxvZy9DYXRlZ29yeUF0dHJpYnV0ZXMgJz48c3Bhbj48L3NwYW4+PGlucz5CdWxrIENhdGVnb3J5IEF0dHJpYnV0ZTwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazMwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvQnVsa0FkZGl0aW9uYWxDb250ZW50VXBsb2FkICc+PHNwYW4+PC9zcGFuPjxpbnM+QnVsayBJbWFnZXJ5IENvbnRlbnQ8L2lucz48L2E+PC9saT48L3VsPjwvbGk+PC91bD48dWwgaWQ9J0Rlc2lnbiAmIENvbnRlbnQnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazQyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9MYXlvdXQvU2V0TWFzdGVyTGF5b3V0P3NvdXJjZT1SR0wnPjxzcGFuPjwvc3Bhbj48aW5zPlNlbGVjdCBMYXlvdXQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rlc2lnbiAmIENvbnRlbnQnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms0MycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTGF5b3V0L1NldFRoZW1lTmV3P3NvdXJjZT1SR0wnPjxzcGFuPjwvc3Bhbj48aW5zPlNlbGVjdCBUaGVtZXM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rlc2lnbiAmIENvbnRlbnQnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms0NCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTGF5b3V0L2RuZC9kbmRwYWdlP3BhZ2VpZD0yOTg1OCc+PHNwYW4+PC9zcGFuPjxpbnM+RGVzaWduIEhvbWUgUGFnZTwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGVzaWduICYgQ29udGVudCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazQ1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9MYXlvdXQvRGVzaWduU3RhbmRhcmRQYWdlcyc+PHNwYW4+PC9zcGFuPjxpbnM+RGVzaWduIFdlYiBQYWdlczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGVzaWduICYgQ29udGVudCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazQ2JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9MYXlvdXQvU2V0SGVhZGVySW1hZ2UnPjxzcGFuPjwvc3Bhbj48aW5zPkRlc2lnbiBIZWFkZXI8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rlc2lnbiAmIENvbnRlbnQnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms0NycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvbGF5b3V0L05hdmlnYXRpb25zJz48c3Bhbj48L3NwYW4+PGlucz5EZXNpZ24gTmF2aWdhdGlvbjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGVzaWduICYgQ29udGVudCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazQ4JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9MYXlvdXQvU2V0Rm9vdGVyRGV0YWlscyc+PHNwYW4+PC9zcGFuPjxpbnM+RGVzaWduIEZvb3RlcjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGVzaWduICYgQ29udGVudCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazUwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9DYXRsb2cvRmlsZU1hbmFnZW1lbnRWMyc+PHNwYW4+PC9zcGFuPjxpbnM+RmlsZSBNYW5hZ2VyPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEZXNpZ24gJiBDb250ZW50JyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rNTEnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0xheW91dC9MaXN0T2ZGb3JtQ3JlYXRpb24nPjxzcGFuPjwvc3Bhbj48aW5zPkFkLWhvYyBGb3JtczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nRGVzaWduICYgQ29udGVudCcgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazUyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9sYXlvdXQvQ3VzdG9tU2NyaXB0P3NvdXJjZT1SR0wnPjxzcGFuPjwvc3Bhbj48aW5zPkN1c3RvbSBTY3JpcHQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0Rlc2lnbiAmIENvbnRlbnQnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsyMDAnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlU2V0dGluZy9EZXNpZ25UaGVtZT9zb3VyY2U9UkdMJz48c3Bhbj48L3NwYW4+PGlucz5DU1MgRWRpdG9yPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdEZXNpZ24gJiBDb250ZW50JyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPjwvdWw+PHVsIGlkPSdNYXJrZXRpbmcnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazUzJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9Wb3VjaGVycy9MaXN0Vm91Y2hlckNhbXBhaWduP3R5cGU9RFYnPjxzcGFuPjwvc3Bhbj48aW5zPkRpc2NvdW50IFZvdWNoZXJzPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdNYXJrZXRpbmcnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms1NScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTWFpbGxpbmdMaXN0L01haWxsaW5nbGlzdCc+PHNwYW4+PC9zcGFuPjxpbnM+TWFpbGluZyBMaXN0PC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdNYXJrZXRpbmcnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms1NicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTWFya2V0aW5nU3luZGljYXRpb24vTWFuYWdlQWZmaWxpYXRlTmV0d29yayc+PHNwYW4+PC9zcGFuPjxpbnM+QWZmaWxpYXRlczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nTWFya2V0aW5nJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rNTcnPjxzcGFuPjwvc3Bhbj48aW5zPlNNUyBNYW5hZ2VtZW50PC9pbnM+PC9sYWJlbD48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nTWFya2V0aW5nJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PGxpPjxhIGlkPSdTdWJMaW5rNTcnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1Ntc01nbXQvU01TRGFzaGJvYXJkLmFzcHggJz48c3Bhbj48L3NwYW4+PGlucz5TTVMgRGFzaGJvYXJkPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rNTgnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1Ntc01nbXQvU01TU2VuZGVyU2V0dXAgJz48c3Bhbj48L3NwYW4+PGlucz5TZW5kZXIgSUQgU2V0dXA8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1Ykxpbms1OScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU21zTWdtdC9TTVNQcmVmZXJlbmNlcyAnPjxzcGFuPjwvc3Bhbj48aW5zPlNNUyBQcmVmZXJlbmNlczwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazYwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TbXNNZ210L1NNU1JlcG9ydHMgJz48c3Bhbj48L3NwYW4+PGlucz5SZXBvcnRzPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rNjEnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1Ntc01nbXQvUHVyc2hhc2VTbXNDcmVkaXRzICc+PHNwYW4+PC9zcGFuPjxpbnM+QnV5IENyZWRpdHM8L2lucz48L2E+PC9saT48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbms2NScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTXlBY2NvdW50L0dvb2dsZUFuYWx5dGljc01hbmFnZW1lbnQnPjxzcGFuPjwvc3Bhbj48aW5zPldlYiBBbmFseXRpY3M8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J01hcmtldGluZycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazY3JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9VUkxSZWRpcmVjdC9VUkxSZWRpcmVjdCc+PHNwYW4+PC9zcGFuPjxpbnM+SFRUUCBSZWRpcmVjdGlvbjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nTWFya2V0aW5nJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rNjgnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0NvbnRyb2xzL1Bvc3RNYW5hZ2VtZW50Jz48c3Bhbj48L3NwYW4+PGlucz5Qb3N0IE1hbmFnZW1lbnQ8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J01hcmtldGluZycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT48L3VsPjx1bCBpZD0nU2V0dGluZ3MnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpICA+PGxhYmVsICBpZD0nTGluazc3Jz48c3Bhbj48L3NwYW4+PGlucz5TdG9yZSBJbmZvcm1hdGlvbjwvaW5zPjwvbGFiZWw+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PGxpPjxhIGlkPSdTdWJMaW5rNzcnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlSW5mb3JtYXRpb24vU3RvcmVBZGRyZXNzLmFzcHg/c291cmNlPVJHTCAnPjxzcGFuPjwvc3Bhbj48aW5zPkFkZHJlc3M8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1Ykxpbms4MCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU3RvcmVJbmZvcm1hdGlvbi9DdXN0b21lclN1cHBvcnQgJz48c3Bhbj48L3NwYW4+PGlucz5DdXN0b21lciBTdXBwb3J0PC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rODEnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0NhdGxvZy9TdG9ja0xvY2F0aW9ucyAnPjxzcGFuPjwvc3Bhbj48aW5zPkxvY2F0aW9uczwvaW5zPjwvYT48L2xpPjwvdWw+PC9saT4gPGxpICA+PGxhYmVsICBpZD0nTGluazkyJz48c3Bhbj48L3NwYW4+PGlucz5UYXggUHJvZmlsZTwvaW5zPjwvbGFiZWw+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PGxpPjxhIGlkPSdTdWJMaW5rOTInIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1NoaXBwaW5nUHJvZmlsZS9UQVhEYXNoYm9hcmQuYXNweCAnPjxzcGFuPjwvc3Bhbj48aW5zPlRBWCBEYXNoYm9hcmQ8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1Ykxpbms5MycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU2hpcHBpbmdQcm9maWxlL0FkZFRheENhdGVnb3J5ICc+PHNwYW4+PC9zcGFuPjxpbnM+QWRkIFRheCBDYXRlZ29yeTwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazk0JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TaGlwcGluZ1Byb2ZpbGUvQ3JlYXRlU2hpcHBpbmdDb2Rlcz9Db2RlSWQ9MCYmQ29kZVR5cGU9VFggJz48c3Bhbj48L3NwYW4+PGlucz5BZGQgVGF4IENvZGU8L2lucz48L2E+PC9saT48L3VsPjwvbGk+IDxsaSAgPjxsYWJlbCAgaWQ9J0xpbms5Nyc+PHNwYW4+PC9zcGFuPjxpbnM+QXBwbGljYXRpb24gU2V0dGluZ3M8L2lucz48L2xhYmVsPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdTZXR0aW5ncycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjxsaT48YSBpZD0nU3ViTGluazk3JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TdG9yZVNldHRpbmcvT3RoZXJDb250cm9scy5hc3B4ICc+PHNwYW4+PC9zcGFuPjxpbnM+U3RvcmUgU2V0dGluZ3M8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsxMDcnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlSW5mb3JtYXRpb24vUmVnaXN0cmF0aW9uX2FuZF9VcF9mcm9udExvZ2luU2V0dGluZ3MgJz48c3Bhbj48L3NwYW4+PGlucz5SZWdpc3RyYXRpb24gU2V0dGluZ3M8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsxMDAnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlU2V0dGluZy9jaGVja291dHNldHRpbmcgJz48c3Bhbj48L3NwYW4+PGlucz5DaGVja291dCBTZXR0aW5nczwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazEwMScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU3RvcmVTZXR0aW5nL0ltYWdlT3B0aW9ucyAnPjxzcGFuPjwvc3Bhbj48aW5zPkltYWdlIFNldHRpbmdzPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMTAyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TdG9yZUluZm9ybWF0aW9uL0VtYWlsQ29uZmlndXJhdGlvbiAnPjxzcGFuPjwvc3Bhbj48aW5zPlNNVFAgQ29uZmlndXJhdGlvbjwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazEwMycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU3RvcmVJbmZvcm1hdGlvbi9NYXBwaW5nQ291bnRyaWVzICc+PHNwYW4+PC9zcGFuPjxpbnM+TWFwcGluZyBDb3VudHJpZXM8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsxMDUnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlU2V0dGluZy9GYWNlYm9va0FQSVNldHVwICc+PHNwYW4+PC9zcGFuPjxpbnM+U29jaWFsIE5ldHdvcmtpbmcgU2l0ZXMgQVBJIFNldHVwPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMTA2JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9PcmRlck1hbmFnZW1lbnQvQ3VzdG9taXplT3JkU3RhdHVzICc+PHNwYW4+PC9zcGFuPjxpbnM+Q3VzdG9taXplIE9yZGVyIFN0YXR1czwvaW5zPjwvYT48L2xpPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazgyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9TdG9yZVNldHRpbmcvQ2hlY2tPdXRPcHRpb25zP3NvdXJjZT1SR0wnPjxzcGFuPjwvc3Bhbj48aW5zPkNoZWNrb3V0IE9wdGlvbnM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rODMnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1N0b3JlU2V0dGluZy9XU0NoZWNrT3V0T3B0aW9uP3NvdXJjZT1SR0wnPjxzcGFuPjwvc3Bhbj48aW5zPkNhbGwgZm9yIEFjdGlvbnM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTA4JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9NeUFjY291bnQvQWNjb3VudERldGFpbHMnPjxzcGFuPjwvc3Bhbj48aW5zPkFjY291bnQgRGV0YWlsczwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nU2V0dGluZ3MnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMDknIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL015QWNjb3VudC9NYWlsTWFuYWdlcic+PHNwYW4+PC9zcGFuPjxpbnM+RW1haWwgVGVtcGxhdGVzPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdTZXR0aW5ncycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazI3NCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU2hpcHBpbmdQcm9maWxlL1NoaXBwaW5nQ29kZVByb2ZpbGUnPjxzcGFuPjwvc3Bhbj48aW5zPlNoaXBwaW5nIFByb2ZpbGU8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rODgnPjxzcGFuPjwvc3Bhbj48aW5zPkxvZ2lzdGljPC9pbnM+PC9sYWJlbD48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nU2V0dGluZ3MnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48bGk+PGEgaWQ9J1N1Ykxpbms4OCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU2hpcHBpbmdQcm9maWxlL0FyYW1leEludGVncmF0aW9uLmFzcHggJz48c3Bhbj48L3NwYW4+PGlucz5BcmFtZXggSW50ZWdyYXRpb248L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1Ykxpbms5MCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU2hpcHBpbmdQcm9maWxlL0JsdWVEYXJ0SW50ZWdyYXRpb24gJz48c3Bhbj48L3NwYW4+PGlucz5CbHVlRGFydCBJbnRlZ3JhdGlvbjwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazI4MicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvU2hpcHBpbmdQcm9maWxlL0RURENJbnRlZ3JhdGlvbiAnPjxzcGFuPjwvc3Bhbj48aW5zPkRUREMgSW50ZWdyYXRpb248L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsyMTUnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1NoaXBwaW5nUHJvZmlsZS9NYW5hZ2VQaW5Db2RlTGlzdCAnPjxzcGFuPjwvc3Bhbj48aW5zPkxvZ2lzdGljIFNldHRpbmdzPC9pbnM+PC9hPjwvbGk+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rOTYnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL015QWNjb3VudC9FbWFpbFNldHVwJz48c3Bhbj48L3NwYW4+PGlucz5FbWFpbCBTZXR1cDwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nU2V0dGluZ3MnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMTMnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL1RyYWluaW5nL01hcnRqYWNrVHJhaW5pbmdWaWRlbyc+PHNwYW4+PC9zcGFuPjxpbnM+SGVscCBWaWRlb3M8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J1NldHRpbmdzJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMjAxJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9UcmFpbmluZy9UcmFpbmluZ0NhbGVuZGFyJz48c3Bhbj48L3NwYW4+PGlucz5UcmFpbmluZyBDYWxlbmRhcjwvaW5zPjwvYT48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nU2V0dGluZ3MnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+PC91bD48dWwgaWQ9J0N1c3RvbWVycycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTE0JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9NeUFjY291bnQvTWVtYmVyU2hpcERhc2hCb2FyZCc+PHNwYW4+PC9zcGFuPjxpbnM+TWVtYmVyIFByb2ZpbGU8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0N1c3RvbWVycycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazExNicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTXlBY2NvdW50L1NhdmVkQ2FydExpc3QnPjxzcGFuPjwvc3Bhbj48aW5zPkFiYW5kb25lZCBDYXJ0PC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdDdXN0b21lcnMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+IDxsaSAgPjxhIG9uY2xpY2s9Z2V0YWN0aXZlbGluayh0aGlzKSAgaWQ9J0xpbmsxMTcnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL015QWNjb3VudC9jcHdpc2hsaXN0Jz48c3Bhbj48L3NwYW4+PGlucz5XaXNobGlzdCBTZWFyY2g8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0N1c3RvbWVycycgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT48L3VsPjx1bCBpZD0nZVhjaGFuZ2UnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEyMScgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvRXhjaGFuZ2UvRXhjaGFuZ2VQcm9maWxlJz48c3Bhbj48L3NwYW4+PGlucz5Qcm9maWxlPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdlWGNoYW5nZScgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEyMicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvRXhjaGFuZ2UvU2VsbGVyTmV0d29yayc+PHNwYW4+PC9zcGFuPjxpbnM+QXNzb2NpYXRlZCBQdWJsaXNoZXJzPC9pbnM+PC9hPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdlWGNoYW5nZScgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjwvdWw+PC9saT4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazEyNCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvRXhjaGFuZ2UvRXhjaGFuZ2VSZXBvcnQnPjxzcGFuPjwvc3Bhbj48aW5zPmVYY2hhbmdlIFJlcG9ydHM8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J2VYY2hhbmdlJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rMTM5Jz48c3Bhbj48L3NwYW4+PGlucz5CdWxrIFRvb2xzPC9pbnM+PC9sYWJlbD48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nZVhjaGFuZ2UnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48bGk+PGEgaWQ9J1N1YkxpbmsxMzknIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0V4Y2hhbmdlL0FkZEJ1bGtFeGNoYW5nZVByb2R1Y3QuYXNweCAnPjxzcGFuPjwvc3Bhbj48aW5zPlByb2R1Y3QgVXBsb2FkPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMTQyJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9FeGNoYW5nZS9Qcm9kdWN0QXR0cmlidXRlc19FeGNoYW5nZSAnPjxzcGFuPjwvc3Bhbj48aW5zPkF0dHJpYnV0ZSBVcGxvYWQ8L2lucz48L2E+PC9saT48bGk+PGEgaWQ9J1N1YkxpbmsxNDAnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL0V4Y2hhbmdlL0FaRmlsZUdlbmVyYXRpb24gJz48c3Bhbj48L3NwYW4+PGlucz5KdW5nbGVlLmNvbSBTZXR1cDwvaW5zPjwvYT48L2xpPjxsaT48YSBpZD0nU3ViTGluazE2NycgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvRXhjaGFuZ2UvQnVsa1ByaWNlTGlzdFVwbG9hZCAnPjxzcGFuPjwvc3Bhbj48aW5zPlByaWNlIExpc3QgVXBsb2FkPC9pbnM+PC9hPjwvbGk+PC91bD48L2xpPiA8bGkgID48YSBvbmNsaWNrPWdldGFjdGl2ZWxpbmsodGhpcykgIGlkPSdMaW5rMTY5JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9FeGNoYW5nZS9QcmljZUxpc3QnPjxzcGFuPjwvc3Bhbj48aW5zPlByaWNlIExpc3Q8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J2VYY2hhbmdlJyBzdHlsZT0nZGlzcGxheTpub25lJyA+PC91bD48L2xpPiA8bGkgID48bGFiZWwgIGlkPSdMaW5rMjU2Jz48c3Bhbj48L3NwYW4+PGlucz5BbWF6b24gQXBwPC9pbnM+PC9sYWJlbD48ZGl2IGNsYXNzPSduYXZfZW1wdHlkaXYnPjwvZGl2Pjx1bCBpZD0nZVhjaGFuZ2UnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48bGk+PGEgaWQ9J1N1YkxpbmsyNTYnIGhyZWY9J2h0dHA6Ly93d3cubWFydGphY2suY29tL2NwL015QWNjb3VudC9BcHBzQ29uZmlndXJhdGlvbi5hc3B4P2FwcGlkPTE1NiAnPjxzcGFuPjwvc3Bhbj48aW5zPk92ZXJ2aWV3PC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjU0JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9FeGNoYW5nZS9BbWF6b25BY2NvdW50U2V0dXAgJz48c3Bhbj48L3NwYW4+PGlucz5BY2NvdW50IFNldHVwPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjU1JyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9FeGNoYW5nZS9BbWF6b25TdGFydExpc3RpbmcgJz48c3Bhbj48L3NwYW4+PGlucz5TdGFydCBMaXN0aW5nPC9pbnM+PC9hPjwvbGk+PGxpPjxhIGlkPSdTdWJMaW5rMjYwJyBocmVmPSdodHRwOi8vd3d3Lm1hcnRqYWNrLmNvbS9jcC9FeGNoYW5nZS9MaXN0aW5nU2V0dGluZyAnPjxzcGFuPjwvc3Bhbj48aW5zPkxpc3RpbmcgU2V0dGluZzwvaW5zPjwvYT48L2xpPjwvdWw+PC9saT4gPGxpICA+PGxhYmVsICBpZD0nTGluazI4OCc+PHNwYW4+PC9zcGFuPjxpbnM+TWFuYWdlIFByb21vdGlvbnM8L2lucz48L2xhYmVsPjxkaXYgY2xhc3M9J25hdl9lbXB0eWRpdic+PC9kaXY+PHVsIGlkPSdlWGNoYW5nZScgc3R5bGU9J2Rpc3BsYXk6bm9uZScgPjxsaT48YSBpZD0nU3ViTGluazI4OCcgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvUnVsZUVuZ2luZS9Qcm9tb3Rpb24vQ2FydFByb21vdGlvbkxpc3QgJz48c3Bhbj48L3NwYW4+PGlucz5NYXJrZXRQbGFjZSBQcm9tb3Rpb248L2lucz48L2E+PC9saT48L3VsPjwvbGk+PC91bD48dWwgaWQ9J0FwcHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID4gPGxpICA+PGEgb25jbGljaz1nZXRhY3RpdmVsaW5rKHRoaXMpICBpZD0nTGluazExMicgaHJlZj0naHR0cDovL3d3dy5tYXJ0amFjay5jb20vY3AvTXlBY2NvdW50L0FkZC1vbnNDb25maWd1cmF0aW9uJz48c3Bhbj48L3NwYW4+PGlucz5BcHAgU3RvcmU8L2lucz48L2E+PGRpdiBjbGFzcz0nbmF2X2VtcHR5ZGl2Jz48L2Rpdj48dWwgaWQ9J0FwcHMnIHN0eWxlPSdkaXNwbGF5Om5vbmUnID48L3VsPjwvbGk+PC91bD5kAgYPZBYCAgEPZBYGZg8WAh4JaW5uZXJodG1sBQ9QYXltZW50IFBlbmRpbmdkAgEPFgIfBWVkAgIPFgIeBXZhbHVlBQJ7fWQCBw9kFg5mD2QWAgIBDxYCHwUFFlBheW1lbnQgUGVuZGluZyBPcmRlcnNkAgIPFgIfBQWDBFBheW1lbnQgUGVuZGluZyBPcmRlcnMgYXJlIHRoZSBvcmRlcnMgZm9yIHdoaWNoIHRoZSBwYXltZW50IGhhcyBub3QgYmVlbiByZWNlaXZlZC4gVGhlIG9yZGVycyBwbGFjZWQgdGhyb3VnaCAnQ2hlcXVlL0RELyBCYW5rIHRyYW5zZmVyLyBzb21lIGNhc2VzIG9mIHVuc3VjY2Vzc2Z1bCBDcmVkaXQgQ2FyZCBvciBEZWJpdCBDYXJkIG9yIE5ldCBCYW5raW5nIHRyYW5zYWN0aW9ucycgYXJlIGF1dG9tYXRpY2FsbHkgbGlzdGVkIGhlcmUgYW5kIG5lZWQgdG8gYmUgYXV0aG9yaXplZCBhZnRlciBwYXltZW50IGNvbmZpcm1hdGlvbiBmcm9tIGJhbmsuIFlvdSBjYW4gc29ydCBvdXQgdGhlc2Ugb3JkZXJzIHRocm91Z2ggbXVsdGlwbGUgc2VhcmNoIGNyaXRlcmlhIHRoYXQgYXJlIHByb3ZpZGVkIGhlcmUuIFRoZSBvcmRlcnMgcGxhY2VkIGFnYWluc3QgeW91ciBwcm9kdWN0cyBhdCBjb25uZWN0ZWQgbWFya2V0cGxhY2VzIGNhbiBiZSBmb3VuZCB0aHJvdWdoIOKAmE1hcmtldHBsYWNlIE9yZGVyc+KAmSB0YWIuZAIHD2QWAmYPZBYGAgEPZBYCAgEPFgIfBQUXTXVsdGlwbGUgU2VhcmNoIE9wdGlvbnNkAgcPZBYQZg8WAh4HVmlzaWJsZWgWBAIDDxYCHwdoZAIHDxBkEBUBDk5vIFN0b3JlIEFkZGVkFQErMGNmOWZkZDItMTQ1Yi00MTRiLTkyMTItY2VhMWY1NmEzODY0LCBBTExOVxQrAwFnFgFmZAICDxYCHwUFBEZyb21kAgQPDxYIHgJUVGUeAlRWBQFWHgJURQUBRR4CVFIFASpkZAIFDxYCHwUFAlRvZAIHDw8WCB8IZR8JBQFWHwoFAUUfCwUBKmRkAgoPFgIfB2gWBAIBDxYCHwUFEkRpc3BhdGNoIERhdGUgRnJvbWQCBw8WAh8FBQJUb2QCCw9kFgICAQ8WAh8HaBYEAgEPFgIfBQUIQ291cmllciBkAgMPEGRkFgBkAgwPFgIfB2gWBAIBDxYCHwUFD0Rpc3BhdGNoIFN0YXR1c2QCAw8QZGQWAGQCCQ9kFgICAQ9kFgYCAQ8PZBYCHgVzdHlsZQUNZGlzcGxheTpub25lO2QCAw88KwARAgAPFgQeC18hRGF0YUJvdW5kZx4LXyFJdGVtQ291bnQCBGQBEBYDAgMCCAIJFgM8KwAFAQAWAh8HaDwrAAUBABYCHwdnPCsABQEAFgIfB2cWAwIGAgYCBhYCZg9kFgoCAQ9kFhRmD2QWAgIBDw8WAh8CBQg2NzMgZGF5c2RkAgEPZBYCAgEPDxYEHwIFBjEwODk0OR4LTmF2aWdhdGVVcmwFQH4vT3JkZXJNYW5hZ2VtZW50L09yZGVyRGV0YWlscz9Tb3VyY2U9UE8mJlJFVHJhbnNhY3Rpb25JZD0xMDg5NDlkZAICD2QWAgIBDw8WAh8CBRcxMC1GZWItMjAxMiAwNDoxMToyNCBQTWRkAgQPZBYCAgEPDxYCHwIFCUNoZXF1ZS9ERGRkAgUPZBYCAgEPDxYCHwIFhwI8c3BhbiBjbGFzcz0nUXVhbnRpdHlUaXBzeVNwYW4nIHRpdGxlPSc8ZGl2IGNsYXNzPSJhZGRyZXNzZGV0YWlscyI+PGRpdiBjbGFzcz0icF9ubSI+VElUQU4gTkVCVUxBIDEwMTVETDAxVklERU9DT04gVjE3NTUgRHVhbCBIYW5kc2V0c1NhcmVlIDc5NUplYW5zIDc4N1RJVEFOIFRFQ0hOT0xPR1kgMTE4NlNMMDE8L2Rpdj48ZGl2IGNsYXNzPSJwX2NvdW50Ij48bGFiZWw+T3JkZXJlZDo8L2xhYmVsPjxzcGFuPjg8L3NwYW4+PC9kaXY+PC9kaXY+Jz44PC9zcGFuPmRkAgYPZBYCAgEPDxYCHwIFKTxzcGFuIGNsYXNzPSdXZWJSdXBlZSc+UnMuIDwvc3Bhbj4gMzQsODAwZGQCBw9kFgICAQ8PFgIfAgX5ATxzcGFuIGNsYXNzPSdRdWFudGl0eVRpcHN5U3BhbicgdGl0bGU9JzxkaXYgY2xhc3M9ImFkZHJlc3NkZXRhaWxzIj48ZGl2IGNsYXNzPSJwX25tIj5CYWxhamkgUmFqZXdhcjwvZGl2PjxwPkh5ZDwvYnI+PC9icj5IeWRlcmFiYWQ8L2JyPkFuZGhyYSBQcmFkZXNoPC9icj5JbmRpYS01MDAwMzA8L3A+PHA+OTEtOTk2NjgwMzg1OTwvcD48cD5iYWxhamkucmFqZXdhckBlcmVhc29uaW5nYy5vbTwvcD4nPkJhbGFqaSBSYWpld2FyPC9zcGFuPmRkAggPZBYKAgMPDxYCHwIFCUF1dGhvcml6ZRYCHgdPbkNsaWNrBStvcGVuY2hrREQoJzEwODk0OScsJ3NoaXAnLCcxMC1GZWItMjAxMicsMCk7ZAIHDw8WAh8CBQJDRGRkAgkPDxYCHwIFBjEwODk0OWRkAgsPDxYCHwIFFDIvMTAvMjAxMiA0OjExOjI0IFBNZGQCDQ8PFgIfAgUEc2hpcGRkAgkPZBYCAgMPDxYCHwIFBkNhbmNlbBYCHxAFFkNhbmNlbE9yZGVyKCcxMDg5NDknKTtkAgoPZBYCAgEPDxYCHwIFATFkZAICD2QWFGYPZBYCAgEPDxYCHwIFCTExNDIgZGF5c2RkAgEPZBYCAgEPDxYEHwIFBTI2MjI5Hw8FP34vT3JkZXJNYW5hZ2VtZW50L09yZGVyRGV0YWlscz9Tb3VyY2U9UE8mJlJFVHJhbnNhY3Rpb25JZD0yNjIyOWRkAgIPZBYCAgEPDxYCHwIFFzI5LU9jdC0yMDEwIDExOjM1OjAyIEFNZGQCBA9kFgICAQ8PFgIfAgUOT25saW5lIFBheW1lbnRkZAIFD2QWAgIBDw8WAh8CBcQBPHNwYW4gY2xhc3M9J1F1YW50aXR5VGlwc3lTcGFuJyB0aXRsZT0nPGRpdiBjbGFzcz0iYWRkcmVzc2RldGFpbHMiPjxkaXYgY2xhc3M9InBfbm0iPlRJVEFOIFRFQ0hOT0xPR1kgMTE4NlNMMDI8L2Rpdj48ZGl2IGNsYXNzPSJwX2NvdW50Ij48bGFiZWw+T3JkZXJlZDo8L2xhYmVsPjxzcGFuPjE8L3NwYW4+PC9kaXY+PC9kaXY+Jz4xPC9zcGFuPmRkAgYPZBYCAgEPDxYCHwIFKDxzcGFuIGNsYXNzPSdXZWJSdXBlZSc+UnMuIDwvc3Bhbj4gNyw2MDBkZAIHD2QWAgIBDw8WAh8CBZoCPHNwYW4gY2xhc3M9J1F1YW50aXR5VGlwc3lTcGFuJyB0aXRsZT0nPGRpdiBjbGFzcz0iYWRkcmVzc2RldGFpbHMiPjxkaXYgY2xhc3M9InBfbm0iPlNhaWR1bHUgR2FkYXJpPC9kaXY+PHA+UGxvdCBOby4gMTI0NCwgUm9hZCBObyAzNiw8L2JyPk1haW4gUm9hZCwgSnVibGllZSBIaWxsczwvYnI+SHlkZXJhYmFkPC9icj5BbmRocmEgUHJhZGVzaDwvYnI+SW5kaWEtNTAwMDMzPC9wPjxwPjwvcD48cD5nYWRhci5zYWlkdWx1QGVyZWFzb25pbmcuY29tPC9wPic+U2FpZHVsdSBHYWRhcmk8L3NwYW4+ZGQCCA9kFgoCAw8PFgIfAgUJQXV0aG9yaXplFgIfEAU6SW5zZXJ0Q29udmVyc2F0aW9uKCcyNjIyOScsJ3NoaXAnLCc3NjAwJywnMjktT2N0LTIwMTAnLDEpO2QCBw8PFgIfAgUCQ0NkZAIJDw8WAh8CBQUyNjIyOWRkAgsPDxYCHwIFFjEwLzI5LzIwMTAgMTE6MzU6MDIgQU1kZAINDw8WAh8CBQRzaGlwZGQCCQ9kFgICAw8PFgIfAgUGQ2FuY2VsFgIfEAUVQ2FuY2VsT3JkZXIoJzI2MjI5Jyk7ZAIKD2QWAgIBDw8WAh8CBQExZGQCAw9kFhRmD2QWAgIBDw8WAh8CBQkxMTQyIGRheXNkZAIBD2QWAgIBDw8WBB8CBQUyNjIyMh8PBT9+L09yZGVyTWFuYWdlbWVudC9PcmRlckRldGFpbHM/U291cmNlPVBPJiZSRVRyYW5zYWN0aW9uSWQ9MjYyMjJkZAICD2QWAgIBDw8WAh8CBRcyOS1PY3QtMjAxMCAxMToxOTowMSBBTWRkAgQPZBYCAgEPDxYCHwIFDk9ubGluZSBQYXltZW50ZGQCBQ9kFgICAQ8PFgIfAgXEATxzcGFuIGNsYXNzPSdRdWFudGl0eVRpcHN5U3BhbicgdGl0bGU9JzxkaXYgY2xhc3M9ImFkZHJlc3NkZXRhaWxzIj48ZGl2IGNsYXNzPSJwX25tIj5USVRBTiBURUNITk9MT0dZIDExODZTTDAyPC9kaXY+PGRpdiBjbGFzcz0icF9jb3VudCI+PGxhYmVsPk9yZGVyZWQ6PC9sYWJlbD48c3Bhbj4xPC9zcGFuPjwvZGl2PjwvZGl2Pic+MTwvc3Bhbj5kZAIGD2QWAgIBDw8WAh8CBSg8c3BhbiBjbGFzcz0nV2ViUnVwZWUnPlJzLiA8L3NwYW4+IDcsNjAwZGQCBw9kFgICAQ8PFgIfAgWaAjxzcGFuIGNsYXNzPSdRdWFudGl0eVRpcHN5U3BhbicgdGl0bGU9JzxkaXYgY2xhc3M9ImFkZHJlc3NkZXRhaWxzIj48ZGl2IGNsYXNzPSJwX25tIj5TYWlkdWx1IEdhZGFyaTwvZGl2PjxwPlBsb3QgTm8uIDEyNDQsIFJvYWQgTm8gMzYsPC9icj5NYWluIFJvYWQsIEp1YmxpZWUgSGlsbHM8L2JyPkh5ZGVyYWJhZDwvYnI+QW5kaHJhIFByYWRlc2g8L2JyPkluZGlhLTUwMDAzMzwvcD48cD48L3A+PHA+Z2FkYXIuc2FpZHVsdUBlcmVhc29uaW5nLmNvbTwvcD4nPlNhaWR1bHUgR2FkYXJpPC9zcGFuPmRkAggPZBYKAgMPDxYCHwIFCUF1dGhvcml6ZRYCHxAFOkluc2VydENvbnZlcnNhdGlvbignMjYyMjInLCdzaGlwJywnNzYwMCcsJzI5LU9jdC0yMDEwJywyKTtkAgcPDxYCHwIFAkNDZGQCCQ8PFgIfAgUFMjYyMjJkZAILDw8WAh8CBRYxMC8yOS8yMDEwIDExOjE5OjAxIEFNZGQCDQ8PFgIfAgUEc2hpcGRkAgkPZBYCAgMPDxYCHwIFBkNhbmNlbBYCHxAFFUNhbmNlbE9yZGVyKCcyNjIyMicpO2QCCg9kFgICAQ8PFgIfAgUBMWRkAgQPZBYUZg9kFgICAQ8PFgIfAgUJMTE0MiBkYXlzZGQCAQ9kFgICAQ8PFgQfAgUFMjYyMjEfDwU/fi9PcmRlck1hbmFnZW1lbnQvT3JkZXJEZXRhaWxzP1NvdXJjZT1QTyYmUkVUcmFuc2FjdGlvbklkPTI2MjIxZGQCAg9kFgICAQ8PFgIfAgUXMjktT2N0LTIwMTAgMTE6MTg6MDMgQU1kZAIED2QWAgIBDw8WAh8CBQ5PbmxpbmUgUGF5bWVudGRkAgUPZBYCAgEPDxYCHwIFxAE8c3BhbiBjbGFzcz0nUXVhbnRpdHlUaXBzeVNwYW4nIHRpdGxlPSc8ZGl2IGNsYXNzPSJhZGRyZXNzZGV0YWlscyI+PGRpdiBjbGFzcz0icF9ubSI+VElUQU4gVEVDSE5PTE9HWSAxMTg2U0wwMjwvZGl2PjxkaXYgY2xhc3M9InBfY291bnQiPjxsYWJlbD5PcmRlcmVkOjwvbGFiZWw+PHNwYW4+MTwvc3Bhbj48L2Rpdj48L2Rpdj4nPjE8L3NwYW4+ZGQCBg9kFgICAQ8PFgIfAgUoPHNwYW4gY2xhc3M9J1dlYlJ1cGVlJz5Scy4gPC9zcGFuPiA3LDYwMGRkAgcPZBYCAgEPDxYCHwIFmgI8c3BhbiBjbGFzcz0nUXVhbnRpdHlUaXBzeVNwYW4nIHRpdGxlPSc8ZGl2IGNsYXNzPSJhZGRyZXNzZGV0YWlscyI+PGRpdiBjbGFzcz0icF9ubSI+U2FpZHVsdSBHYWRhcmk8L2Rpdj48cD5QbG90IE5vLiAxMjQ0LCBSb2FkIE5vIDM2LDwvYnI+TWFpbiBSb2FkLCBKdWJsaWVlIEhpbGxzPC9icj5IeWRlcmFiYWQ8L2JyPkFuZGhyYSBQcmFkZXNoPC9icj5JbmRpYS01MDAwMzM8L3A+PHA+PC9wPjxwPmdhZGFyLnNhaWR1bHVAZXJlYXNvbmluZy5jb208L3A+Jz5TYWlkdWx1IEdhZGFyaTwvc3Bhbj5kZAIID2QWCgIDDw8WAh8CBQlBdXRob3JpemUWAh8QBTpJbnNlcnRDb252ZXJzYXRpb24oJzI2MjIxJywnc2hpcCcsJzc2MDAnLCcyOS1PY3QtMjAxMCcsMyk7ZAIHDw8WAh8CBQJDQ2RkAgkPDxYCHwIFBTI2MjIxZGQCCw8PFgIfAgUWMTAvMjkvMjAxMCAxMToxODowMyBBTWRkAg0PDxYCHwIFBHNoaXBkZAIJD2QWAgIDDw8WAh8CBQZDYW5jZWwWAh8QBRVDYW5jZWxPcmRlcignMjYyMjEnKTtkAgoPZBYCAgEPDxYCHwIFATFkZAIFDw8WAh8HaGRkAgUPZBYQAgEPEGRkFgECAmQCAw8PFgIfB2hkFggCAQ8PFgIfAmVkZAIFDw8WAh8CBQExZGQCCQ8PFgIfAgUBNGRkAg0PDxYCHwIFATRkZAIFDw8WAh8CBQExZGQCCQ8PFgIfAgUBMWRkAgwPFgIfB2gWEgIBDw8WBB4IQ3NzQ2xhc3MFC3BhZ2VfbmF2YnRuHgRfIVNCAgJkZAIDDw8WBB8RBQtwYWdlX25hdmJ0bh8SAgJkZAIFDw8WBh8RBQtwYWdlX2FjdGl2ZR8CBQExHxICAmRkAgcPDxYEHwJlHwdoZGQCCQ8PFgQfAmUfB2hkZAILDw8WBB8CZR8HaGRkAg0PDxYEHwJlHwdoZGQCDw8PFgYfEQUZcGFnZV9uYXZidG4gcGFnZV9kaXNhYmxlZB4HRW5hYmxlZGgfEgICZGQCEQ8PFgYfEQUZcGFnZV9uYXZidG4gcGFnZV9kaXNhYmxlZB8TaB8SAgJkZAIODxYCHgVWYWx1ZQUBMWQCEA8WAh8UBQE0ZAISDxYCHxQFAjUwZAIJDxYCHgV0aXRsZQUWQWRkaXRpb25hbCBJbmZvcm1hdGlvbhYMAgEPFgIfBQUPSW5zdHJ1bWVudCBUeXBlZAIFDxYCHwUFBERhdGVkAgkPDxYIHwgFCzE0LURlYy0yMDEzHwkFAVYfCgUBRR8LBQEqZGQCEw8WAh8FBQpDaGVxdWUgTm8uZAIbDxYCHwUFC0JhbmsvQnJhbmNoZAIjDxYCHwUFBkFtb3VudGQCCg8WAh8VBRZBZGRpdGlvbmFsIEluZm9ybWF0aW9uFgoCAQ8WAh8FBQREYXRlZAINDw8WCB8IZR8JBQFWHwoFAUUfCwUBKmRkAg8PFgIfBQUMQmFuayBSZWYgTm8uZAIXDxYCHwUFC0JhbmsvQnJhbmNoZAIfDxYCHwUFBkFtb3VudGQCCw8WAh8VBRZBZGRpdGlvbmFsIEluZm9ybWF0aW9uFgICDw8PFggfCGUfCQUBVh8KBQFFHwsFASpkZAIMDxYCHxUFDENhbmNlbCBPcmRlcmQYAgUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgQFHmN0bDAwJGN0bDAwJExvZ2luU3RhdHVzMSRjdGwwMQUeY3RsMDAkY3RsMDAkTG9naW5TdGF0dXMxJGN0bDAzBTJjdGwwMCRjdGwwMCRNYWluJE1haW4kQ2FuY2VsT3JkZXIxJGNoa1Nob3dDb21tZW50cwU1Y3RsMDAkY3RsMDAkTWFpbiRNYWluJENhbmNlbE9yZGVyMSRjaGtSZXZlcnRJbnZlbnRvcnkFJmN0bDAwJGN0bDAwJE1haW4kTWFpbiRncmRQZW5kaW5nT3JkZXJzDzwrAAwDBhUBD1JFVHJhbnNhY3Rpb25JZAcUKwAEFCsAASgpWVN5c3RlbS5JbnQ2NCwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5BjEwODk0ORQrAAEoKwQFMjYyMjkUKwABKCsEBTI2MjIyFCsAASgrBAUyNjIyMQgCAWQ=" type="hidden">
</div>

<script type="text/javascript">
//<![CDATA[
    var theForm = document.forms['aspnetForm'];
    if (!theForm) {
        theForm = document.aspnetForm;
    }
    function __doPostBack(eventTarget, eventArgument) {
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
            theForm.__EVENTTARGET.value = eventTarget;
            theForm.__EVENTARGUMENT.value = eventArgument;
            theForm.submit();
        }
    }
//]]>
</script>


<script src="Orders_files/WebResource.js" type="text/javascript"></script>

<script type="text/javascript" src="Orders_files/PopCalendar.js"></script><script type="text/javascript" src="Orders_files/PopCalendarFunctions.js"></script>
<script type="text/javascript">    var PopCal_enUS_DaysShort = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];</script>
<script type="text/javascript">    var PopCal_enUS_MonthsShort = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']; var PopCal_enUS_MonthsFull = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];</script>
<span id="ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError" dir="ltr" style="display:none;position:absolute;top:0px;left:0px;white-space:nowrap;z-index:+1000;color:red;" controltovalidate="txtAllOrdersFrom" display="Dynamic" validationgroup="btng" evaluationfunction="__PopCalCustomValidatorEvaluateIsValid" clientvalidationfunction="__PopCalValidateOnSubmit"></span>
<script src="Orders_files/ScriptResource.js" type="text/javascript"></script>
<span id="ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError" dir="ltr" style="display:none;position:absolute;top:0px;left:0px;white-space:nowrap;z-index:+1000;color:red;" controltovalidate="txtAllOrdersTo" display="Dynamic" validationgroup="btng" evaluationfunction="__PopCalCustomValidatorEvaluateIsValid" clientvalidationfunction="__PopCalValidateOnSubmit"></span>
<span id="ctl00_ctl00_Main_Main_PopCalendar1_MessageError" dir="ltr" style="display:none;position:absolute;top:0px;left:0px;white-space:nowrap;z-index:+1000;color:red;" controltovalidate="ctl00_ctl00_Main_Main_txtchkdddate" display="Dynamic" evaluationfunction="__PopCalCustomValidatorEvaluateIsValid" clientvalidationfunction="__PopCalValidateOnSubmit"></span>
<span id="ctl00_ctl00_Main_Main_PopCalendar2_MessageError" dir="ltr" style="display:none;position:absolute;top:0px;left:0px;white-space:nowrap;z-index:+1000;color:red;" controltovalidate="ctl00_ctl00_Main_Main_txtobtdate" display="Dynamic" evaluationfunction="__PopCalCustomValidatorEvaluateIsValid" clientvalidationfunction="__PopCalValidateOnSubmit"></span>
<span id="ctl00_ctl00_Main_Main_PopCalendar3_MessageError" dir="ltr" style="display:none;position:absolute;top:0px;left:0px;white-space:nowrap;z-index:+1000;color:red;" controltovalidate="ctl00_ctl00_Main_Main_txtCCdate" display="Dynamic" evaluationfunction="__PopCalCustomValidatorEvaluateIsValid" clientvalidationfunction="__PopCalValidateOnSubmit"></span>
<script src="Orders_files/ScriptResource_002.js" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_003.js" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_003.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_007.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_006.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_005.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_004.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource_002.axd" type="text/javascript"></script>
<script src="Orders_files/ScriptResource.axd" type="text/javascript"></script>
<script type="text/javascript">
//<![CDATA[
    function WebForm_OnSubmit() {
        if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) return false;
        return true;
    }
//]]>
</script>

        


<a id="ToggleSecNavBtn" class="btn_hidemenu" onclick="ToggleSecNav(this);return false;" href="#" style="display: block;"></a>


<div id="dreamworks_container" class="">
<nav id="primary_nav">

<input name="ctl00$ctl00$Main$hdngroupname" id="ctl00_ctl00_Main_hdngroupname" value="Orders &amp; Leads" type="hidden">
</nav>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        var masterid = document.getElementById('ctl00_ctl00_Main_hdngroupname').value;
        $('#secondary_nav').find('ul').each(function () {
            if ($(this).attr('id') == masterid) {
                document.getElementById('ctl00_ctl00_Main_lblgroupname').innerHTML = masterid;
                $(this).css('display', 'block');
            } else {
                $(this).css('display', 'none');
            }
        });


        if ($('#secondary_nav2').find('ul').children('li').length > 0) {
            $('#dreamworks_container').addClass('hidemenupanel right_nav');
            $('#secondary_nav2').css('display', 'block');
            $('#ToggleSecNavBtn').addClass('btn_showmenu');
            $('#ToggleSecNavBtn').css('display', 'block');
        }
        else {
            $('#secondary_nav2').hide();
            $('#secondary_nav').css('display', 'block');
            $('#ToggleSecNavBtn').css('display', 'block');
            $('#dreamworks_container').removeClass('hidemenupanel');
            $('#ToggleSecNavBtn').removeClass('btn_showmenu');
        }


    });

    function ToggleSecNav(El) {
        if ($(El).hasClass('btn_showmenu')) {
            if ($('#secondary_nav2').find('ul').children('li').length > 0) {
                $('#secondary_nav').css('display', 'block');
                $('#dreamworks_container').removeClass('hidemenupanel right_nav');
                $('#secondary_nav2').css('display', 'none');
                $('#ToggleSecNavBtn').removeClass('btn_showmenu');
            }
            else {
                $('#secondary_nav').css('display', 'block');
                $('#dreamworks_container').removeClass('hidemenupanel right_nav');
                $('#secondary_nav2').css('display', 'none');
                $('#ToggleSecNavBtn').removeClass('btn_showmenu');
            }
        }
        else {
            $('#secondary_nav').css('display', 'none');
            if ($('#secondary_nav2').find('ul').children('li').length > 0) {
                $('#dreamworks_container').addClass('hidemenupanel right_nav');
                $('#secondary_nav2').css('display', 'block');
            }
            else {
                $('#dreamworks_container').removeClass('right_nav');
                $('#dreamworks_container').addClass('hidemenupanel');
                $('#secondary_nav2').css('display', 'none');
            }
            $('#ToggleSecNavBtn').addClass('btn_hidemenu btn_showmenu');
        }
    }

    function getchildlink(masterlink) {
        $("li").removeClass(' active');
        $($(masterlink).parent()).addClass(' active');
        var linkid = $($(masterlink).parent()).attr('id');
        var id = linkid.split("_")[2];
        document.getElementById('ctl00_ctl00_Main_lblgroupname').innerHTML = id;
        $('#secondary_nav').css('display', 'block');
        $('#dreamworks_container').removeClass('hidemenupanel right_nav');
        $('#secondary_nav2').css('display', 'none');
        $('#ToggleSecNavBtn').removeClass('btn_showmenu');
        $('#secondary_nav').find('ul').each(function () {
            if ($(this).attr('id') == id) {
                $(this).css('display', 'block');
            } else {
                $(this).css('display', 'none');
            }
        });

    }
    function getactivelink(ourlink) {
        $("li[class='active']").removeClass('active');
        $($(ourlink).parent()).addClass('active');
    }
</script>

  


    <div id="cp_placeholder">
        
    <script type="text/javascript">
        /*added by shahid script for update panel progress 4-aug-2010*/
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())
                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            if (postBackElement.id == 'ctl00_ctl00_Main_Main_lnkGetNetWOrders')
                document.getElementById("ctl00_ctl00_Main_Main_updProgress").style.display = 'block';
            else if (postBackElement.id == 'ctl00_ctl00_Main_Main_lnkGetOrders')
                document.getElementById("ctl00_ctl00_Main_Main_updProgress").style.display = 'block';
        }
        function EndRequest(sender, args) {
            if (postBackElement.id == 'ctl00_ctl00_Main_Main_lnkGetNetWOrders')
                document.getElementById("ctl00_ctl00_Main_Main_updProgress").style.display = 'none';
            else if (postBackElement.id == 'ctl00_ctl00_Main_Main_lnkGetOrders')
                document.getElementById("ctl00_ctl00_Main_Main_updProgress").style.display = 'none';
            if (document.getElementById("ctl00_ctl00_Main_Main_grdPendingOrders") == null) {
                document.getElementById('ctl00_ctl00_Main_Main_btnExport').style.display = 'none';
                document.getElementById('ctl00_ctl00_Main_Main_divbtn').style.display = 'none';
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_btnExport').style.display = 'block';
                document.getElementById('ctl00_ctl00_Main_Main_divbtn').style.display = 'block';
            }

            if (args.get_error() != undefined) {
                var msg = args.get_error().message.replace("Sys.WebForms.PageRequestManagerServerErrorException: ", "");
                // Show the custom error. 
                // Here you can be creative and do whatever you want
                // with the exception (i.e. call a modalpopup and show 
                // a nicer error window). I will simply use 'alert'
                alert(msg);
                // Let the framework know that the error is handled, 
                //  so it doesn't throw the JavaScript alert.
                args.set_errorHandled(true);
            }


        }

        function InsertConversation(Rtid, DeliveryOpt, TP, OrderDate1, RowIndex) {
            globRowIndex = RowIndex;
            Retransactionid = Rtid;
            DeliveryOption = DeliveryOpt;
            OrderData = OrderDate1;
            TotalPrice = TP;
            comclick("divcreditcard");
            var txtTmpChar = document.getElementById('ctl00_ctl00_Main_Main_txtNewConvText');
            chkLength(txtTmpChar);
        }
    
    </script>
 
    
    
    <div id="ctl00_ctl00_Main_Main_PanMsg" class="msgbar msg_Success hide_onC" style="display: none;">
    </div>
    <div id="ctl00_ctl00_Main_Main_PanErrMsg" class="msgbar msg_Error hide_onC" style="display: none;">
    </div>
     <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display:none;">
	
            <div id="theprogress">
               <center>
                <img id="ctl00_ctl00_Main_Main_imgWaitIcon" src="Orders_files/progress-indicator.gif" style="border-width:0px;" align="absmiddle">
               </center>
            </div>
        
</div>

   
	
                <div class="widget">
                     
                    <div class="widget_body">
                                                                        
                            <div id="ctl00_ctl00_Main_Main_divGrid" class="tab-grdpanel">  
                                <div class="widget marg-0"> 
                                    <div class="widget_title">
                                        <span class="iconsweet">r</span><h5>Available Orders</h5>
                                    </div>                  
                                    <div id="ctl00_ctl00_Main_Main_divPendingOrders">
                                <a id="ctl00_ctl00_Main_Main_HyperLink1" href='javascript:WebForm_DoPostBackWithOptions(new%20WebForm_PostBackOptions("ctl00$ctl00$Main$Main$HyperLink1",%20"",%20true,%20"",%20"",%20false,%20true))' style="display:none;">hi</a>
                                <div>
		<table rules="all" class="activity_datatable" id="ctl00_ctl00_Main_Main_grdPendingOrders" style="width:100%;border-collapse:collapse;" border="1" cellspacing="0">
			<tbody><tr>
				<th scope="col" style="width:70px;">Age</th><th scope="col" style="width:70px;">Order ID</th><th scope="col" style="width:151px;">Order Date</th><th scope="col" style="width:80px;">Checkout Type</th><th scope="col" style="width:60px;">Qty</th><th scope="col" style="width:120px;"><div id="divCurrency">Order Value</div></th><th scope="col" style="width:100px;">Customer Name</th><th scope="col" style="width:70px;">Authorize</th><th scope="col" style="width:50px;">Cancel Order</th>
			</tr><tr>
				<td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblAge">673 days</span>
                                            </td><td>
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_hypEdit" href="http://www.martjack.com/cp/OrderManagement/OrderDetails?Source=PO&amp;&amp;RETransactionId=108949">108949</a>
                                            </td><td>                                                
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_OrderDate">10-Feb-2012 04:11:24 PM</span>                                                                                                
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblCheckOut">Cheque/DD</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblTotalProducts"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;TITAN NEBULA 1015DL01VIDEOCON V1755 Dual HandsetsSaree 795Jeans 787TITAN TECHNOLOGY 1186SL01&lt;/div&gt;&lt;div class=&quot;p_count&quot;&gt;&lt;label&gt;Ordered:&lt;/label&gt;&lt;span&gt;8&lt;/span&gt;&lt;/div&gt;&lt;/div&gt;" class="QuantityTipsySpan">8</span></span>
                                            </td><td align="right">
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblTotalPrice"><span class="WebRupee">Rs. </span> 34,800</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblcustName"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;Balaji Rajewar&lt;/div&gt;&lt;p&gt;Hyd&lt;/br&gt;&lt;/br&gt;Hyderabad&lt;/br&gt;Andhra Pradesh&lt;/br&gt;India-500030&lt;/p&gt;&lt;p&gt;91-9966803859&lt;/p&gt;&lt;p&gt;balaji.rajewar@ereasoningc.om&lt;/p&gt;" class="QuantityTipsySpan">Balaji Rajewar</span></span>
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_hypauthorized" onclick="openchkDD('108949','ship','10-Feb-2012',0);">Authorize</a>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_lblImageload"></span>
                                                
                                                
                                                
                                                
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl02_hypCancel" onclick="CancelOrder('108949');">Cancel</a>
                                            </td>
			</tr><tr class="grd_alternetrow">
				<td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblAge">1142 days</span>
                                            </td><td>
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_hypEdit" href="http://www.martjack.com/cp/OrderManagement/OrderDetails?Source=PO&amp;&amp;RETransactionId=26229">26229</a>
                                            </td><td>                                                
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_OrderDate">29-Oct-2010 11:35:02 AM</span>                                                                                                
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblCheckOut">Online Payment</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblTotalProducts"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;TITAN TECHNOLOGY 1186SL02&lt;/div&gt;&lt;div class=&quot;p_count&quot;&gt;&lt;label&gt;Ordered:&lt;/label&gt;&lt;span&gt;1&lt;/span&gt;&lt;/div&gt;&lt;/div&gt;" class="QuantityTipsySpan">1</span></span>
                                            </td><td align="right">
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblTotalPrice"><span class="WebRupee">Rs. </span> 7,600</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblcustName"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;Saidulu Gadari&lt;/div&gt;&lt;p&gt;Plot No. 1244, Road No 36,&lt;/br&gt;Main Road, Jubliee Hills&lt;/br&gt;Hyderabad&lt;/br&gt;Andhra Pradesh&lt;/br&gt;India-500033&lt;/p&gt;&lt;p&gt;&lt;/p&gt;&lt;p&gt;gadar.saidulu@ereasoning.com&lt;/p&gt;" class="QuantityTipsySpan">Saidulu Gadari</span></span>
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_hypauthorized" onclick="InsertConversation('26229','ship','7600','29-Oct-2010',1);">Authorize</a>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_lblImageload"></span>
                                                
                                                
                                                
                                                
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl03_hypCancel" onclick="CancelOrder('26229');">Cancel</a>
                                            </td>
			</tr><tr>
				<td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblAge">1142 days</span>
                                            </td><td>
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_hypEdit" href="http://www.martjack.com/cp/OrderManagement/OrderDetails?Source=PO&amp;&amp;RETransactionId=26222">26222</a>
                                            </td><td>                                                
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_OrderDate">29-Oct-2010 11:19:01 AM</span>                                                                                                
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblCheckOut">Online Payment</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblTotalProducts"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;TITAN TECHNOLOGY 1186SL02&lt;/div&gt;&lt;div class=&quot;p_count&quot;&gt;&lt;label&gt;Ordered:&lt;/label&gt;&lt;span&gt;1&lt;/span&gt;&lt;/div&gt;&lt;/div&gt;" class="QuantityTipsySpan">1</span></span>
                                            </td><td align="right">
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblTotalPrice"><span class="WebRupee">Rs. </span> 7,600</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblcustName"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;Saidulu Gadari&lt;/div&gt;&lt;p&gt;Plot No. 1244, Road No 36,&lt;/br&gt;Main Road, Jubliee Hills&lt;/br&gt;Hyderabad&lt;/br&gt;Andhra Pradesh&lt;/br&gt;India-500033&lt;/p&gt;&lt;p&gt;&lt;/p&gt;&lt;p&gt;gadar.saidulu@ereasoning.com&lt;/p&gt;" class="QuantityTipsySpan">Saidulu Gadari</span></span>
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_hypauthorized" onclick="InsertConversation('26222','ship','7600','29-Oct-2010',2);">Authorize</a>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_lblImageload"></span>
                                                
                                                
                                                
                                                
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl04_hypCancel" onclick="CancelOrder('26222');">Cancel</a>
                                            </td>
			</tr><tr class="grd_alternetrow">
				<td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblAge">1142 days</span>
                                            </td><td>
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_hypEdit" href="http://www.martjack.com/cp/OrderManagement/OrderDetails?Source=PO&amp;&amp;RETransactionId=26221">26221</a>
                                            </td><td>                                                
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_OrderDate">29-Oct-2010 11:18:03 AM</span>                                                                                                
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblCheckOut">Online Payment</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblTotalProducts"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;TITAN TECHNOLOGY 1186SL02&lt;/div&gt;&lt;div class=&quot;p_count&quot;&gt;&lt;label&gt;Ordered:&lt;/label&gt;&lt;span&gt;1&lt;/span&gt;&lt;/div&gt;&lt;/div&gt;" class="QuantityTipsySpan">1</span></span>
                                            </td><td align="right">
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblTotalPrice"><span class="WebRupee">Rs. </span> 7,600</span>
                                            </td><td>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblcustName"><span original-title="&lt;div class=&quot;addressdetails&quot;&gt;&lt;div class=&quot;p_nm&quot;&gt;Saidulu Gadari&lt;/div&gt;&lt;p&gt;Plot No. 1244, Road No 36,&lt;/br&gt;Main Road, Jubliee Hills&lt;/br&gt;Hyderabad&lt;/br&gt;Andhra Pradesh&lt;/br&gt;India-500033&lt;/p&gt;&lt;p&gt;&lt;/p&gt;&lt;p&gt;gadar.saidulu@ereasoning.com&lt;/p&gt;" class="QuantityTipsySpan">Saidulu Gadari</span></span>
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_hypauthorized" onclick="InsertConversation('26221','ship','7600','29-Oct-2010',3);">Authorize</a>
                                                <span id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_lblImageload"></span>
                                                
                                                
                                                
                                                
                                            </td><td>
                                                
                                                <a id="ctl00_ctl00_Main_Main_grdPendingOrders_ctl05_hypCancel" onclick="CancelOrder('26221');">Cancel</a>
                                            </td>
			</tr>
		</tbody></table>
	</div>
                                
     <div class="cp_grdpager">
                	<div class="grdinfo">
                      <label>Records per page</label>
                      <div id="uniform-ctl00_ctl00_Main_Main_usrGridPaging1_ddlNoRows" class="selector"><span style="-moz-user-select: none;">50</span><select style="opacity: 0;" name="ctl00$ctl00$Main$Main$usrGridPaging1$ddlNoRows" onchange="javascript:setTimeout('__doPostBack(\'ctl00$ctl00$Main$Main$usrGridPaging1$ddlNoRows\',\'\')', 0)" id="ctl00_ctl00_Main_Main_usrGridPaging1_ddlNoRows" class="uniform_pager_list">
		<option value="10">10</option>
		<option value="25">25</option>
		<option selected="selected" value="50">50</option>
		<option value="75">75</option>
		<option value="100">100</option>

	</select></div> 
        
                        
                    </div>
                      <div class="grd_pages">
                      <label class="pages">
            Records:
            <span id="ctl00_ctl00_Main_Main_usrGridPaging1_lblPageNo">1</span>
            <span id="ctl00_ctl00_Main_Main_usrGridPaging1_lbl_of">of</span>
            <span id="ctl00_ctl00_Main_Main_usrGridPaging1_lblPageCount">1</span><span id="ctl00_ctl00_Main_Main_usrGridPaging1_lblPagestxt"> Pages</span></label>
              
             
<div id="dvHidden">
    
    
    
</div>
                      </div>
                      <div class="clear"></div>
                    </div>



                                <input name="ctl00$ctl00$Main$Main$hdnNWSearchOpt" id="ctl00_ctl00_Main_Main_hdnNWSearchOpt" type="hidden">
                            </div>                    
                                </div>
                            </div>
                        <div class="tab-grdpanel">
                            
</div>
                    </div>
                </div>
                <input name="ctl00$ctl00$Main$Main$inputFlagNW" id="ctl00_ctl00_Main_Main_inputFlagNW" type="hidden">
                <input name="ctl00$ctl00$Main$Main$inputFlagNW_temp" id="ctl00_ctl00_Main_Main_inputFlagNW_temp" type="hidden">
                <input name="ctl00$ctl00$Main$Main$inputFlag" id="ctl00_ctl00_Main_Main_inputFlag" value="NSearch" type="hidden">
                <input name="ctl00$ctl00$Main$Main$hdnchanell" id="ctl00_ctl00_Main_Main_hdnchanell" type="hidden">
                
                
                <img alt="" id="img441" src="Orders_files/designoption_009_13.jpg" onload="getdivCurrency();" height="1" width="1">
                <input name="ctl00$ctl00$Main$Main$hdnTodoSearchcriteria" id="ctl00_ctl00_Main_Main_hdnTodoSearchcriteria" value="[&quot;Order No&quot;,&quot;SKU&quot;,&quot;Email Id&quot;,&quot;Phone No&quot;,&quot;Coupon Code&quot;,&quot;Checkout Type&quot;]" type="hidden">
            
</div>
        <div id="ctl00_ctl00_Main_Main_divbtn" class="action_bar text_right">
            <input name="ctl00$ctl00$Main$Main$btnExport" value="Export to Excel" onclick='javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ctl00$ctl00$Main$Main$btnExport", "", true, "", "", false, false))' id="ctl00_ctl00_Main_Main_btnExport" class="button_small greyishBtn  fl_right" type="submit">
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
        <div class="clear">
        </div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
    </div>
    <div class="clear">
    </div>
    <input name="ctl00$ctl00$Main$Main$hdntab" id="ctl00_ctl00_Main_Main_hdntab" type="hidden">
    <input name="ctl00$ctl00$Main$Main$hdnMerchantrole" id="ctl00_ctl00_Main_Main_hdnMerchantrole" value="M" type="hidden">
    <a id="ctl00_ctl00_Main_Main_lnkGetNetWOrders" href='javascript:WebForm_DoPostBackWithOptions(new%20WebForm_PostBackOptions("ctl00$ctl00$Main$Main$lnkGetNetWOrders",%20"",%20true,%20"",%20"",%20false,%20true))'></a>
    <a id="ctl00_ctl00_Main_Main_lnkGetOrders" href='javascript:WebForm_DoPostBackWithOptions(new%20WebForm_PostBackOptions("ctl00$ctl00$Main$Main$lnkGetOrders",%20"",%20true,%20"",%20"",%20false,%20true))'></a>
    <script type="text/javascript" language="javascript">
        var isajax = false;
        var IsPostBack = false;
        $(document).ready(function () {

            ApplyTipsy();
            ConstructSearch();
            var type = $("input[id*=inputFlagNW_temp]").val();
            if (type == "NW") {
                isajax = true;
                $("#tabs").tabs({ selected: 1 })
                $('#tabs').find('.tabs_holder').css('display', 'block');
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            getdivCurrency();
            $('div[name = divchkdd]').dialog("widget").wrap('<div ></div>');
            $('div[name = divchkdd]').dialog({
                modal: true,
                width: 580,
                autoOpen: false
            });
            $('div[name = divcreditcard]').dialog("widget").wrap('<div ></div>');
            $('div[name = divcreditcard]').dialog({
                modal: true,
                width: 512,
                autoOpen: false
            });
            $('div[name = divbanktransfer]').dialog("widget").wrap('<div ></div>');
            $('div[name = divbanktransfer]').dialog({
                modal: true,
                width: 580,
                autoOpen: false
            });
            $('div[name = divCancelOrder]').dialog("widget").wrap('<div ></div>');
            $('div[name = divCancelOrder]').dialog({
                modal: true,
                width: 534,
                autoOpen: false
            });
        });
        function ApplyTipsy() {
            $('.QuantityTipsySpan').tipsy({
                gravity: $.fn.tipsy.autoNS,
                fade: true,
                html: true
            });
        }
        function EndRequestHandler() {
            $("#tabs").tabs();
            $('#tabs').find('.tabs_holder').css('display', 'block');
            $(".uniform_pager_listselect,[Id*=drpcheckouttype],select[Id*=drpOrderStatus],select[Id*=drpStore],select[Id*=ddlNoRows]").uniform();
            var type = $("input[id*=inputFlagNW_temp]").val();
            if (type == "NW") {
                isajax = true;
                $("#tabs").tabs({ selected: 1 });
                $('#tabs').find('.tabs_holder').css('display', 'block');
            }
            //IsPostBack = true;
            SelectedSearchSummary();
            ConstructSearch();
            BindBrowseNode();
            OrderPeriod();
            BindStoreCategoryEvent();
            $('div[name = divCategory_BrowseNode]').dialog({
                modal: true,
                width: 470,
                autoOpen: false,
                position: 'top'
            });
            $('div[name = divCategory_BrowseNode]').dialog("widget").wrap('<div class="cp_light_blue"></div>');
            $('div[name = divCategory]').dialog({
                modal: true,
                width: 470,
                autoOpen: false,
                position: 'top'
            });
            $("#divSellers").dialog({
                modal: true,
                width: 650,
                autoOpen: false,
                position: 'center',
                draggable: false
            });
            $('div[name = divCategory]').dialog("widget").wrap('<div class="cp_light_blue"></div>');
            $('select[id*=drpOrderAge]').uniform();
            $('select[id*=ddlCustomerService]').uniform();
            $('select[id*=drpSeller]').uniform();
            ApplyTipsy();
        }


        function comclick(divid) {
            var id = "ctl00_ctl00_Main_Main_" + divid;
            $("#" + id + "").dialog("open");
            $("#" + id + "").dialog("option", "position", 'center');

        }

        //added changes for cancel order S
        function openCancelOrder() {
            HideRevert("P");
            var id = "ctl00_ctl00_Main_Main_divCancelOrder";
            $("#" + id + "").dialog("open");
            $("#" + id + "").dialog("option", "position", 'center');
            return false;
        }
        function CloseCancelOrder() {
            var id = "ctl00_ctl00_Main_Main_divCancelOrder";
            $("#" + id + "").dialog("close");
        }

        function BindOrders(type) {
            $("input[id*=hdntab]").val(type);
            $("input[id*=inputFlagNW_temp]").val(type);
            if (!isajax) {
                clearcontrols();
                $(arrSearchFields).each(function () {
                    this.Value = "";
                });
                IsPostBack = false;
                if (type == 'NW') {
                    __doPostBack('ctl00$ctl00$Main$Main$lnkGetNetWOrders', '')
                }
                else {
                    __doPostBack('ctl00$ctl00$Main$Main$lnkGetOrders', '');
                }
            } else {
                isajax = false;
            }
        }

        function getBrowseNode() {
            if (typeof selNodes != 'undefined') {
                var selcategory = $.map(selNodes, function (node) {
                    $('#ctl00_ctl00_Main_Main_sodMyorders_hdnbrowsenode').val(node.data.key.toString());
                    $("#txtBrowseNode").val(node.data.title.toString());
                    $("#divCategory").dialog("close");
                });
            }
        }

    </script>

    </div>
</div>

</section>
</div>

    

    <div id="ReloadTabDialog" title="information for reloading page" style="display: none; width: 70%">
        <div>
            <p class="padding_10">The current page is serving a stale 
version as the merchant account session used to generate page has 
expired. Please click OK to reload the tab.</p>
        </div>
    </div>


<script type="text/javascript">
//<![CDATA[
    var Page_Validators = new Array(document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError"), document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError"), document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation"), document.getElementById("ctl00_ctl00_Main_Main_PopCalendar1_MessageError"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtchkdddate"), document.getElementById("ctl00_ctl00_Main_Main_CustmchkddDateValidation"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtchkno"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtbankbranch"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtamount"), document.getElementById("ctl00_ctl00_Main_Main_revtxtamount"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtdate"), document.getElementById("ctl00_ctl00_Main_Main_CustmBanktransDateValidation"), document.getElementById("ctl00_ctl00_Main_Main_PopCalendar2_MessageError"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtbankrefno"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtbankbranch"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtamount"), document.getElementById("ctl00_ctl00_Main_Main_revtxtobtamount"), document.getElementById("ctl00_ctl00_Main_Main_rfvtxtCCdate"), document.getElementById("ctl00_ctl00_Main_Main_CustmCCDateValidation"), document.getElementById("ctl00_ctl00_Main_Main_PopCalendar3_MessageError"));
//]]>
</script>

<script type="text/javascript">
//<![CDATA[
    var ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError = document.all ? document.all["ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError"] : document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError");
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.controltovalidate = "txtAllOrdersFrom";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.errormessage = "";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.display = "Dynamic";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.validationGroup = "btng";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.evaluationfunction = "__PopCalCustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError.clientvalidationfunction = "__PopCalValidateOnSubmit";
    var ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError = document.all ? document.all["ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError"] : document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError");
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.controltovalidate = "txtAllOrdersTo";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.errormessage = "";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.display = "Dynamic";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.validationGroup = "btng";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.evaluationfunction = "__PopCalCustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError.clientvalidationfunction = "__PopCalValidateOnSubmit";
    var ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation = document.all ? document.all["ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation"] : document.getElementById("ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation");
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.controltovalidate = "txtAllOrdersTo";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.focusOnError = "t";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.errormessage = "To Date should be greater than or equal to From Date";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.display = "None";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.validationGroup = "btng";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.evaluationfunction = "CustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation.clientvalidationfunction = "dateValidationOrders";
    var ctl00_ctl00_Main_Main_PopCalendar1_MessageError = document.all ? document.all["ctl00_ctl00_Main_Main_PopCalendar1_MessageError"] : document.getElementById("ctl00_ctl00_Main_Main_PopCalendar1_MessageError");
    ctl00_ctl00_Main_Main_PopCalendar1_MessageError.controltovalidate = "ctl00_ctl00_Main_Main_txtchkdddate";
    ctl00_ctl00_Main_Main_PopCalendar1_MessageError.errormessage = "";
    ctl00_ctl00_Main_Main_PopCalendar1_MessageError.display = "Dynamic";
    ctl00_ctl00_Main_Main_PopCalendar1_MessageError.evaluationfunction = "__PopCalCustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_PopCalendar1_MessageError.clientvalidationfunction = "__PopCalValidateOnSubmit";
    var ctl00_ctl00_Main_Main_rfvtxtchkdddate = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtchkdddate"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtchkdddate");
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.controltovalidate = "ctl00_ctl00_Main_Main_txtchkdddate";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.errormessage = "Select Date";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtchkdddate.initialvalue = "";
    var ctl00_ctl00_Main_Main_CustmchkddDateValidation = document.all ? document.all["ctl00_ctl00_Main_Main_CustmchkddDateValidation"] : document.getElementById("ctl00_ctl00_Main_Main_CustmchkddDateValidation");
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.controltovalidate = "ctl00_ctl00_Main_Main_txtchkdddate";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.focusOnError = "t";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.errormessage = "Date Should be Greater than or Equal to Order Date";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.display = "None";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.evaluationfunction = "CustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_CustmchkddDateValidation.clientvalidationfunction = "ChkDDdateValidation";
    var ctl00_ctl00_Main_Main_rfvtxtchkno = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtchkno"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtchkno");
    ctl00_ctl00_Main_Main_rfvtxtchkno.controltovalidate = "ctl00_ctl00_Main_Main_txtchkno";
    ctl00_ctl00_Main_Main_rfvtxtchkno.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtchkno.errormessage = "Enter Check No";
    ctl00_ctl00_Main_Main_rfvtxtchkno.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtchkno.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_rfvtxtchkno.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtchkno.initialvalue = "";
    var ctl00_ctl00_Main_Main_rfvtxtbankbranch = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtbankbranch"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtbankbranch");
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.controltovalidate = "ctl00_ctl00_Main_Main_txtbankbranch";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.errormessage = "Enter Bank Branch";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtbankbranch.initialvalue = "";
    var ctl00_ctl00_Main_Main_rfvtxtamount = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtamount"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtamount");
    ctl00_ctl00_Main_Main_rfvtxtamount.controltovalidate = "ctl00_ctl00_Main_Main_txtamount";
    ctl00_ctl00_Main_Main_rfvtxtamount.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtamount.errormessage = "Enter Amount";
    ctl00_ctl00_Main_Main_rfvtxtamount.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtamount.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_rfvtxtamount.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtamount.initialvalue = "";
    var ctl00_ctl00_Main_Main_revtxtamount = document.all ? document.all["ctl00_ctl00_Main_Main_revtxtamount"] : document.getElementById("ctl00_ctl00_Main_Main_revtxtamount");
    ctl00_ctl00_Main_Main_revtxtamount.controltovalidate = "ctl00_ctl00_Main_Main_txtamount";
    ctl00_ctl00_Main_Main_revtxtamount.focusOnError = "t";
    ctl00_ctl00_Main_Main_revtxtamount.errormessage = "Invalid Amount";
    ctl00_ctl00_Main_Main_revtxtamount.display = "None";
    ctl00_ctl00_Main_Main_revtxtamount.validationGroup = "CheckDD";
    ctl00_ctl00_Main_Main_revtxtamount.evaluationfunction = "RegularExpressionValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_revtxtamount.validationexpression = "^[0-9.]{0,9}$";
    var ctl00_ctl00_Main_Main_rfvtxtobtdate = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtobtdate"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtdate");
    ctl00_ctl00_Main_Main_rfvtxtobtdate.controltovalidate = "ctl00_ctl00_Main_Main_txtobtdate";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.errormessage = "Select Date";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtobtdate.initialvalue = "";
    var ctl00_ctl00_Main_Main_CustmBanktransDateValidation = document.all ? document.all["ctl00_ctl00_Main_Main_CustmBanktransDateValidation"] : document.getElementById("ctl00_ctl00_Main_Main_CustmBanktransDateValidation");
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.controltovalidate = "ctl00_ctl00_Main_Main_txtobtdate";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.focusOnError = "t";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.errormessage = "Date Should be Greater than or Equal to Order Date";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.display = "None";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.evaluationfunction = "CustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_CustmBanktransDateValidation.clientvalidationfunction = "BankTransdateValidation";
    var ctl00_ctl00_Main_Main_PopCalendar2_MessageError = document.all ? document.all["ctl00_ctl00_Main_Main_PopCalendar2_MessageError"] : document.getElementById("ctl00_ctl00_Main_Main_PopCalendar2_MessageError");
    ctl00_ctl00_Main_Main_PopCalendar2_MessageError.controltovalidate = "ctl00_ctl00_Main_Main_txtobtdate";
    ctl00_ctl00_Main_Main_PopCalendar2_MessageError.errormessage = "";
    ctl00_ctl00_Main_Main_PopCalendar2_MessageError.display = "Dynamic";
    ctl00_ctl00_Main_Main_PopCalendar2_MessageError.evaluationfunction = "__PopCalCustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_PopCalendar2_MessageError.clientvalidationfunction = "__PopCalValidateOnSubmit";
    var ctl00_ctl00_Main_Main_rfvtxtbankrefno = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtbankrefno"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtbankrefno");
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.controltovalidate = "ctl00_ctl00_Main_Main_txtbankrefno";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.errormessage = "Enter Bank Ref No";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtbankrefno.initialvalue = "";
    var ctl00_ctl00_Main_Main_rfvtxtobtbankbranch = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtobtbankbranch"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtbankbranch");
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.controltovalidate = "ctl00_ctl00_Main_Main_txtobtbankbranch";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.errormessage = "Enter Bank Branch ";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtobtbankbranch.initialvalue = "";
    var ctl00_ctl00_Main_Main_rfvtxtobtamount = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtobtamount"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtobtamount");
    ctl00_ctl00_Main_Main_rfvtxtobtamount.controltovalidate = "ctl00_ctl00_Main_Main_txtobtamount";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.errormessage = "Enter Amount";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtobtamount.initialvalue = "";
    var ctl00_ctl00_Main_Main_revtxtobtamount = document.all ? document.all["ctl00_ctl00_Main_Main_revtxtobtamount"] : document.getElementById("ctl00_ctl00_Main_Main_revtxtobtamount");
    ctl00_ctl00_Main_Main_revtxtobtamount.controltovalidate = "ctl00_ctl00_Main_Main_txtobtamount";
    ctl00_ctl00_Main_Main_revtxtobtamount.focusOnError = "t";
    ctl00_ctl00_Main_Main_revtxtobtamount.errormessage = "Invalid Amount";
    ctl00_ctl00_Main_Main_revtxtobtamount.display = "None";
    ctl00_ctl00_Main_Main_revtxtobtamount.validationGroup = "BankTransfer";
    ctl00_ctl00_Main_Main_revtxtobtamount.evaluationfunction = "RegularExpressionValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_revtxtobtamount.validationexpression = "^[0-9.]{0,9}$";
    var ctl00_ctl00_Main_Main_rfvtxtCCdate = document.all ? document.all["ctl00_ctl00_Main_Main_rfvtxtCCdate"] : document.getElementById("ctl00_ctl00_Main_Main_rfvtxtCCdate");
    ctl00_ctl00_Main_Main_rfvtxtCCdate.controltovalidate = "ctl00_ctl00_Main_Main_txtCCdate";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.focusOnError = "t";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.errormessage = "Select Date";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.display = "None";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.validationGroup = "CreditCard";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.evaluationfunction = "RequiredFieldValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_rfvtxtCCdate.initialvalue = "";
    var ctl00_ctl00_Main_Main_CustmCCDateValidation = document.all ? document.all["ctl00_ctl00_Main_Main_CustmCCDateValidation"] : document.getElementById("ctl00_ctl00_Main_Main_CustmCCDateValidation");
    ctl00_ctl00_Main_Main_CustmCCDateValidation.controltovalidate = "ctl00_ctl00_Main_Main_txtCCdate";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.focusOnError = "t";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.errormessage = "Date Should be Greater than or Equal to Order Date";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.display = "None";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.validationGroup = "CreditCard";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.evaluationfunction = "CustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_CustmCCDateValidation.clientvalidationfunction = "CCdateValidation";
    var ctl00_ctl00_Main_Main_PopCalendar3_MessageError = document.all ? document.all["ctl00_ctl00_Main_Main_PopCalendar3_MessageError"] : document.getElementById("ctl00_ctl00_Main_Main_PopCalendar3_MessageError");
    ctl00_ctl00_Main_Main_PopCalendar3_MessageError.controltovalidate = "ctl00_ctl00_Main_Main_txtCCdate";
    ctl00_ctl00_Main_Main_PopCalendar3_MessageError.errormessage = "";
    ctl00_ctl00_Main_Main_PopCalendar3_MessageError.display = "Dynamic";
    ctl00_ctl00_Main_Main_PopCalendar3_MessageError.evaluationfunction = "__PopCalCustomValidatorEvaluateIsValid";
    ctl00_ctl00_Main_Main_PopCalendar3_MessageError.clientvalidationfunction = "__PopCalValidateOnSubmit";
//]]>
</script>


<script type="text/javascript"><!--
    if ((typeof (PopCalendar) == 'undefined') || (PopCalendar.majorVersion == null) || (PopCalendar.minorVersion == null))
        alert('Unable to find script library "/aspnet_client/system_web/4_0_30319/PopCalendar2005/PopCalendar.js".\n\nTry placing this file manually');
    else if ((PopCalendar.majorVersion < 8) || ((PopCalendar.majorVersion = 8) && (PopCalendar.minorVersion < 0.9)))
        alert('This page uses an incorrect version of PopCalendar.js.\n\nThe page expects version 8.0.9 or greater. The script library is ' + PopCalendar.majorVersion + '.' + PopCalendar.minorVersion + '.');
    else if ((typeof (PopCalendarFunctions) == 'undefined') || (PopCalendarFunctions.majorVersion == null) || (PopCalendarFunctions.minorVersion == null))
        alert('Unable to find script library "/aspnet_client/system_web/4_0_30319/PopCalendar2005/PopCalendarFunctions.js".\n\nTry placing this file manually');
    else if ((PopCalendarFunctions.majorVersion < 8) || ((PopCalendarFunctions.majorVersion = 8) && (PopCalendarFunctions.minorVersion < 1.2)))
        alert('This page uses an incorrect version of PopCalendarFunctions.js.\n\nThe page expects version 8.1.2 or greater. The script library is ' + PopCalendarFunctions.majorVersion + '.' + PopCalendarFunctions.minorVersion + '.');
// --></script>
<script type="text/javascript">
//<![CDATA[

    var Page_ValidationActive = false;
    if (typeof (ValidatorOnLoad) == "function") {
        ValidatorOnLoad();
    }

    function ValidatorOnSubmit() {
        if (Page_ValidationActive) {
            return ValidatorCommonOnSubmit();
        }
        else {
            return true;
        }
    }
    Sys.Application.add_init(function () {
        $create(Sys.UI._UpdateProgress, { "associatedUpdatePanelId": "ctl00_ctl00_Main_Main_UpdatePanel1", "displayAfter": 1, "dynamicLayout": true }, null, null, $get("ctl00_ctl00_Main_Main_updProgress"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersFrom_MessageError'));
    }

    document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_PopCalendarAllOrdersTo_MessageError'));
    }

    document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_sodMyorders_ValidatorCalloutExtenderDrpMonTo_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_sodMyorders_ValidatorCalloutExtenderDrpMonTo", "popupPosition": 2, "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_sodMyorders_CustDateValidation"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_PopCalendar1_MessageError').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_PopCalendar1_MessageError'));
    }

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkdddate').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkdddate'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender5_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender5", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtchkdddate"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_CustmchkddDateValidation').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_CustmchkddDateValidation'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender12_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender12", "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_CustmchkddDateValidation"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkno').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtchkno'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender6_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender6", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtchkno"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankbranch').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankbranch'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender7_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender7", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtbankbranch"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtamount').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtamount'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender8_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender8", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtamount"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_revtxtamount').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_revtxtamount'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ajxrevtxtamount_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ajxrevtxtamount", "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_revtxtamount"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtdate').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtdate'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender3_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender3", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtobtdate"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_CustmBanktransDateValidation').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_CustmBanktransDateValidation'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender13_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender13", "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_CustmBanktransDateValidation"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_PopCalendar2_MessageError').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_PopCalendar2_MessageError'));
    }

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankrefno').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtbankrefno'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender2_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender2", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtbankrefno"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtbankbranch').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtbankbranch'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender4_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender4", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtobtbankbranch"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtamount').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtobtamount'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender1_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender1", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtobtamount"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_revtxtobtamount').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_revtxtobtamount'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender9_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender9", "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_revtxtobtamount"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_rfvtxtCCdate').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_rfvtxtCCdate'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender11_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender11", "warningIconImageUrl": "../images/ValidationImage.jpg" }, null, null, $get("ctl00_ctl00_Main_Main_rfvtxtCCdate"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_CustmCCDateValidation').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_CustmCCDateValidation'));
    }
    Sys.Application.add_init(function () {
        $create(Sys.Extended.UI.ValidatorCalloutBehavior, { "ClientStateFieldID": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender14_ClientState", "closeImageUrl": "../images/Close.jpg", "id": "ctl00_ctl00_Main_Main_ValidatorCalloutExtender14", "warningIconImageUrl": "../images/ValidationImage.jpg", "width": "250px" }, null, null, $get("ctl00_ctl00_Main_Main_CustmCCDateValidation"));
    });

    document.getElementById('ctl00_ctl00_Main_Main_PopCalendar3_MessageError').dispose = function () {
        Array.remove(Page_Validators, document.getElementById('ctl00_ctl00_Main_Main_PopCalendar3_MessageError'));
    }
//]]>
</script>
</form>  



<div id="fancybox-tmp"></div><div id="fancybox-loading"><div></div></div><div id="fancybox-overlay"></div><div id="fancybox-wrap"><div id="fancybox-outer"><div class="fancybox-bg" id="fancybox-bg-n"></div><div class="fancybox-bg" id="fancybox-bg-ne"></div><div class="fancybox-bg" id="fancybox-bg-e"></div><div class="fancybox-bg" id="fancybox-bg-se"></div><div class="fancybox-bg" id="fancybox-bg-s"></div><div class="fancybox-bg" id="fancybox-bg-sw"></div><div class="fancybox-bg" id="fancybox-bg-w"></div><div class="fancybox-bg" id="fancybox-bg-nw"></div><div id="fancybox-content"></div><a id="fancybox-close"></a><div id="fancybox-title"></div><a href="javascript:;" id="fancybox-left"><span class="fancy-ico" id="fancybox-left-ico"></span></a><a href="javascript:;" id="fancybox-right"><span class="fancy-ico" id="fancybox-right-ico"></span></a></div></div><div class="cp_light_blue"><div aria-labelledby="ui-dialog-title-divCategory" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;">
<div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span id="ui-dialog-title-divCategory" class="ui-dialog-title">&nbsp;</span><a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a></div><div class="ui-dialog-content ui-widget-content" id="divCategory" name="divCategory" style="text-align: center; overflow: hidden;">
        <div style="position: absolute; top: 50%; left: 30%;">
            <img src="Orders_files/ajax-loader.gif" id="lodingimage1" alt="Loading..." style="display: none;">
        </div>
 </div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div></div><div class="cp_light_blue"><div aria-labelledby="ui-dialog-title-divCategory_BrowseNode" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;"><div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span id="ui-dialog-title-divCategory_BrowseNode" class="ui-dialog-title">&nbsp;</span><a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a></div><div class="ui-dialog-content ui-widget-content" id="divCategory_BrowseNode" name="divCategory_BrowseNode" style="text-align: center; overflow: hidden;">
        <div style="position: absolute; top: 50%; left: 30%;">
            <img src="Orders_files/ajax-loader.gif" id="Img1" alt="Loading..." style="display: none;">
        </div>
 </div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div></div><div aria-labelledby="ui-dialog-title-divSellers" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;"><div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span id="ui-dialog-title-divSellers" class="ui-dialog-title">Select Seller</span><a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a></div><div class="ui-dialog-content ui-widget-content" id="divSellers" style="text-align: center; overflow: hidden;">
       <div>
           


<div style="" class="k-grid k-widget k-secondary" data-role="grid" id="gridSellers"><div style="padding-right: 17px;" class="k-grid-header"><div class="k-grid-header-wrap"><table role="grid"><colgroup><col style="width:40px"><col><col></colgroup><thead><tr><th class="k-header" role="columnheader" data-field=""></th><th data-role="sortable" class="k-header k-filterable" role="columnheader" data-field="SiteUrl" data-title="URL"><a tabindex="-1" class="k-grid-filter" href="#"><span class="k-icon k-filter"></span></a><a class="k-link" href="#">URL</a></th><th data-role="sortable" class="k-header k-filterable" role="columnheader" data-field="BusinessName" data-title="Business Name"><a tabindex="-1" class="k-grid-filter" href="#"><span class="k-icon k-filter"></span></a><a class="k-link" href="#">Business Name</a></th></tr></thead></table></div></div><div class="k-grid-content"><table role="grid"><colgroup><col style="width:40px"><col><col></colgroup><tbody></tbody></table></div><div style="" data-role="pager" class="k-pager-wrap k-grid-pager k-widget"><a tabindex="-1" data-page="1" href="#" title="Go to the first page" class="k-link k-state-disabled"><span class="k-icon k-i-seek-w">Go to the first page</span></a><a tabindex="-1" data-page="1" href="#" title="Go to the previous page" class="k-link k-state-disabled"><span class="k-icon k-i-arrow-w">Go to the previous page</span></a><ul class="k-pager-numbers k-reset"><li><span class="k-state-selected">0</span></li></ul><a tabindex="-1" data-page="0" href="#" title="Go to the next page" class="k-link k-state-disabled"><span class="k-icon k-i-arrow-e">Go to the next page</span></a><a tabindex="-1" data-page="0" href="#" title="Go to the last page" class="k-link k-state-disabled"><span class="k-icon k-i-seek-e">Go to the last page</span></a><span class="k-pager-info k-label">No items to display</span></div></div>
<div class="globalaction_bar text_right marg-0">
    <input id="btnSellerSubmit" class="button_small bluishBtn " value="Submit" type="button">
    <input id="btnSellerCancel" class="button_small greyishBtn  " value="Cancel" type="button">
</div>
        
       </div>
   </div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div>
   <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div>
   </div>
   <div aria-labelledby="ui-dialog-title-ctl00_ctl00_Main_Main_divchkdd" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;">
   <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix">
   <span id="ui-dialog-title-ctl00_ctl00_Main_Main_divchkdd" class="ui-dialog-title">Additional Information</span>
   <a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a></div>
   <div class="ui-dialog-content ui-widget-content" id="ctl00_ctl00_Main_Main_divchkdd" name="divchkdd" style="">
            <div class="widget_body">
                <ul class="form_fields_container">
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblInstrmtType">Instrument Type</label>
                        <div class="form_input">
                            <div id="uniform-ctl00_ctl00_Main_Main_drpinstrumenttype" class="selector"><span style="-moz-user-select: none;">Cheque</span><select style="opacity: 0;" name="ctl00$ctl00$Main$Main$drpinstrumenttype" id="ctl00_ctl00_Main_Main_drpinstrumenttype">
	<option selected="selected" value="Chk">Cheque</option>
	<option value="DD">DD</option>

</select></div>
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblDate">Date</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtchkdddate" value="14-Dec-2013" id="ctl00_ctl00_Main_Main_txtchkdddate" class="hasDatepicker" calendar="ctl00_ctl00_Main_Main_PopCalendar1" format="dd-mmm-yyyy" dir="ltr" autocomplete="off" onfocus="__PopCalSetFocus(this, event);" type="text">
                            <span id="ctl00_ctl00_Main_Main_PopCalendar1_Control" onclick='__PopCalShowCalendar("ctl00_ctl00_Main_Main_txtchkdddate",this)' style="cursor:pointer;"><img src="Orders_files/Calendar.gif" align="absmiddle" border="0"></span><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css"><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css">
                            <span class="cp_orderimg">
                                <span id="ctl00_ctl00_Main_Main_rfvtxtchkdddate" style="color:Red;display:none;"></span>
                                <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender5_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender5_ClientState" type="hidden">
                                <span id="ctl00_ctl00_Main_Main_CustmchkddDateValidation" style="color:Red;display:none;"></span>
                                <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender12_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender12_ClientState" type="hidden">
                            </span>
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblChqNo">Cheque No.</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtchkno" id="ctl00_ctl00_Main_Main_txtchkno" class="oneThree" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtchkno" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender6_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender6_ClientState" type="hidden">
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblBnkBrnch">Bank/Branch</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtbankbranch" id="ctl00_ctl00_Main_Main_txtbankbranch" class="oneThree" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtbankbranch" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender7_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender7_ClientState" type="hidden">
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblAmnt">Amount</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtamount" id="ctl00_ctl00_Main_Main_txtamount" class="oneThree" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtamount" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender8_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender8_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_revtxtamount" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ajxrevtxtamount_ClientState" id="ctl00_ctl00_Main_Main_ajxrevtxtamount_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_SpnChkddCurr" class="cp_graytext" style="margin-left: 5px;">
                            </span>
                        </div>
                    </li>
                </ul>
                <div class="action_bar text_right">
                    <input name="ctl00$ctl00$Main$Main$btnchkddsave" id="ctl00_ctl00_Main_Main_btnchkddsave" value="Save &amp; Authorize" class="button_small greyishBtn  fl_right" validationgroup="CheckDD" onclick="SaveChkDDDetails(event,'mouse');" type="button">
                    <input name="ctl00$ctl00$Main$Main$btnchkddclose" id="ctl00_ctl00_Main_Main_btnchkddclose" value="Close" class="button_small greyishBtn  fl_right" onclick="closechkDD();" type="button">
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div>
        <div aria-labelledby="ui-dialog-title-ctl00_ctl00_Main_Main_divcreditcard" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;">
        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span id="ui-dialog-title-ctl00_ctl00_Main_Main_divcreditcard" class="ui-dialog-title">Additional Information</span>
        <a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a>
        </div>
        <div class="ui-dialog-content ui-widget-content" id="ctl00_ctl00_Main_Main_divcreditcard" name="divcreditcard" style="">
            <div class="widget_body">
                <ul class="form_fields_container">
                    <li>
                        <div>
                            <input name="ctl00$ctl00$Main$Main$txtConvLogID" id="ctl00_ctl00_Main_Main_txtConvLogID" type="hidden"></div>
                       <label> <label id="ctl00_ctl00_Main_Main_lblLogConverDate">
                            Date</label><span class="cp_mandatory_field"> *</span></label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtCCdate" id="ctl00_ctl00_Main_Main_txtCCdate" class="hasDatepicker" calendar="ctl00_ctl00_Main_Main_PopCalendar3" format="dd-mmm-yyyy" dir="ltr" autocomplete="off" onfocus="__PopCalSetFocus(this, event);" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtCCdate" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender11_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender11_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_CustmCCDateValidation" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender14_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender14_ClientState" type="hidden">
                            <span class="cp_orderimg">
                                <span id="ctl00_ctl00_Main_Main_PopCalendar3_Control" onclick='__PopCalShowCalendar("ctl00_ctl00_Main_Main_txtCCdate",this)' style="cursor:pointer;"><img src="Orders_files/Calendar.gif" align="absmiddle" border="0"></span><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css"><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css">
                            </span>
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblLogConvrSummry">
                            Comments<span class="cp_mandatory_field"> *</span></label><div class="form_input">
                                <textarea name="ctl00$ctl00$Main$Main$txtNewConvText" rows="2" cols="20" id="ctl00_ctl00_Main_Main_txtNewConvText" onkeyup="chkLength(this);" style="height:50px;"></textarea></div>
                    </li>
                    <li>
                        <label>
                            Message Length (Max 500 Characters)
                        </label>
                        <div class="form_input">
                            <span class="oneThree">
                                <input name="ctl00$ctl00$Main$Main$txtTempCharsRem" readonly="readonly" id="ctl00_ctl00_Main_Main_txtTempCharsRem" type="text"></span></div>
                    </li>
                </ul>
                <div class="action_bar text_right">
                    <input name="ctl00$ctl00$Main$Main$btnSaveNew" value="Save" onclick='return Savecc();WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ctl00$ctl00$Main$Main$btnSaveNew", "", true, "CreditCard", "", false, false))' id="ctl00_ctl00_Main_Main_btnSaveNew" class="button_small greyishBtn  fl_rightn" type="submit">
                    <input value="Close" class="button_small greyishBtn  fl_right" onclick="return closecreditcard();" type="button">
                </div>
            </div>
        </div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div>
        <div aria-labelledby="ui-dialog-title-ctl00_ctl00_Main_Main_divbanktransfer" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;"><div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix">
     <h5>Available Orders</h5>
        <a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#">
        <span class="ui-icon ui-icon-closethick">close</span>
        </a>
        </div>
        <div class="ui-dialog-content ui-widget-content" id="ctl00_ctl00_Main_Main_divbanktransfer" name="divbanktransfer" style="">
            <div class="widget_body">
                <ul class="form_fields_container">
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblDate2">Date</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtobtdate" id="ctl00_ctl00_Main_Main_txtobtdate" class="hasDatepicker" calendar="ctl00_ctl00_Main_Main_PopCalendar2" format="dd-mmm-yyyy" dir="ltr" autocomplete="off" onfocus="__PopCalSetFocus(this, event);" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtobtdate" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender3_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender3_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_CustmBanktransDateValidation" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender13_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender13_ClientState" type="hidden">
                            <span class="cp_orderimg">
                                <span id="ctl00_ctl00_Main_Main_PopCalendar2_Control" onclick='__PopCalShowCalendar("ctl00_ctl00_Main_Main_txtobtdate",this)' style="cursor:pointer;"><img src="Orders_files/Calendar.gif" align="absmiddle" border="0"></span><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css"><link type="text/css" rel="stylesheet" href="Orders_files/Classic.css">
                            </span>
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblBnkRefNo">Bank Ref No.</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtbankrefno" id="ctl00_ctl00_Main_Main_txtbankrefno" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtbankrefno" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender2_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender2_ClientState" type="hidden">
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblBnkBrnch2">Bank/Branch</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtobtbankbranch" id="ctl00_ctl00_Main_Main_txtobtbankbranch" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtobtbankbranch" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender4_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender4_ClientState" type="hidden">
                        </div>
                    </li>
                    <li>
                        <label id="ctl00_ctl00_Main_Main_lblAmnt2">Amount</label>
                        <div class="form_input">
                            <input name="ctl00$ctl00$Main$Main$txtobtamount" id="ctl00_ctl00_Main_Main_txtobtamount" type="text">
                            <span id="ctl00_ctl00_Main_Main_rfvtxtobtamount" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender1_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender1_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_revtxtobtamount" style="color:Red;display:none;"></span>
                            <input name="ctl00$ctl00$Main$Main$ValidatorCalloutExtender9_ClientState" id="ctl00_ctl00_Main_Main_ValidatorCalloutExtender9_ClientState" type="hidden">
                            <span id="ctl00_ctl00_Main_Main_spnCurncySymble" class="cp_graytext" style="margin-left: 5px;">
                            </span>
                        </div>
                    </li>
                </ul>
                <div class="action_bar text_right">
                    <input name="ctl00$ctl00$Main$Main$btnobtsave" id="ctl00_ctl00_Main_Main_btnobtsave" title="Save &amp; Authorize" value="Save &amp; Authorize" class="button_small greyishBtn  fl_right" validationgroup="BankTransfer" onclick="SaveBankDetails(event,'mouse');" type="button">
                    <input name="ctl00$ctl00$Main$Main$btnobtclose" id="ctl00_ctl00_Main_Main_btnobtclose" value="Close" class="button_small greyishBtn  fl_right" onclick="closebanktransfer();" type="button">
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div>
        <div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div>
        <div aria-labelledby="ui-dialog-title-ctl00_ctl00_Main_Main_divCancelOrder" role="dialog" tabindex="-1" class="ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable" style="display: none; z-index: 1000; outline: 0px none; position: absolute;">
        <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix">
        <span id="ui-dialog-title-ctl00_ctl00_Main_Main_divCancelOrder" class="ui-dialog-title">Cancel Order</span>
        <a role="button" class="ui-dialog-titlebar-close ui-corner-all" href="#"><span class="ui-icon ui-icon-closethick">close</span></a>
        </div><div class="ui-dialog-content ui-widget-content" id="ctl00_ctl00_Main_Main_divCancelOrder" name="divCancelOrder" style="">
            

    <div class="widget">       
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <label id="ctl00_ctl00_Main_Main_CancelOrder1_Label1">
                        Cancel Comments
                    </label>
                    <div class="form_input">
                    <textarea name="ctl00$ctl00$Main$Main$CancelOrder1$txtComments" rows="5" cols="27" id="ctl00_ctl00_Main_Main_CancelOrder1_txtComments"></textarea>
                    </div>
                </li>
                <li>
                    <label id="ctl00_ctl00_Main_Main_CancelOrder1_lblShowComments">
                        Show the comments to end user
                    </label>
                    <div class="form_input">
                        <div id="uniform-ctl00_ctl00_Main_Main_CancelOrder1_chkShowComments" class="checker"><span><input style="opacity: 0;" id="ctl00_ctl00_Main_Main_CancelOrder1_chkShowComments" name="ctl00$ctl00$Main$Main$CancelOrder1$chkShowComments" type="checkbox"></span></div>
                    </div>
                </li>
                <li id="ctl00_ctl00_Main_Main_CancelOrder1_lirevert">
                 
                        <label id="ctl00_ctl00_Main_Main_CancelOrder1_lblRevert">
                            Revert Inventory
                        </label>
                   <div class="form_input">
                    <div id="uniform-ctl00_ctl00_Main_Main_CancelOrder1_chkRevertInventory" class="checker"><span><input style="opacity: 0;" id="ctl00_ctl00_Main_Main_CancelOrder1_chkRevertInventory" name="ctl00$ctl00$Main$Main$CancelOrder1$chkRevertInventory" type="checkbox"></span></div>
                        </div>
                </li>
            </ul>
        </div>
        <div class="action_bar text_right">
        <input value="Close" onclick="CloseCancelOrder();" class="button_small greyishBtn " type="button">
        <input name="ctl00$ctl00$Main$Main$CancelOrder1$btnOrdCancel" value="Cancel Order" onclick='CancelOrder_UserControl();WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions("ctl00$ctl00$Main$Main$CancelOrder1$btnOrdCancel", "", true, "", "", false, false))' id="ctl00_ctl00_Main_Main_CancelOrder1_btnOrdCancel" class="button_small greyishBtn " type="submit">
        <input name="ctl00$ctl00$Main$Main$CancelOrder1$hdnOrderId" id="ctl00_ctl00_Main_Main_CancelOrder1_hdnOrderId" type="hidden">
    </div>
    </div>
    


        </div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-n"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-e"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-s"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-w"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se ui-icon-grip-diagonal-se"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-sw"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-ne"></div><div style="z-index: 1000;" class="ui-resizable-handle ui-resizable-nw"></div></div></body>
</asp:Content>

    


