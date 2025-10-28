<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="TransactionsDates.aspx.cs" Inherits="Admin_TransactionsDates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <link rel="stylesheet" href="templates/mirror/stylesheet3cc5.css?v=1.6" type="text/css" />
    <link rel="stylesheet" href="templates/mirror/instyles.css" type="text/css" />
    <link href="templates/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="templates/js/jquery-1.6.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="templates/js/jquery.validationEngine-en.js" type="text/javascript" charset="utf-8"></script>
    <script src="templates/js/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
    <script src="templates/js/jquery-ui.js" type="text/javascript"></script>
    <%-- <script type="text/javascript">
          $(function () {
              $("#Txtfromdate").datepicker();
          });
    </script>
     <script type="text/javascript">
         $(function () {
             $("#Txttodate").datepicker();
         });
    </script>--%>
    
    <title>Transaction Inquiry Details</title>
    <link href="Citrus/css/default.css" rel="stylesheet" type="text/css" />

    <%--<div id="page-header">
        <div class="page-wrap">
            <div class="logo-wrapper">
                <a href="">
                    <img height="32" width="81" src="#" alt="Citrus" />
                </a>
            </div>
        </div>
    </div>--%>
    <div id="page-client-logo">
        &#160;</div>
    <div id="page-wrapper">
        <form name="searchForm">
        <input type="hidden" name="search" value="">
        <div class="paddling-lef:5px;">
            <h2>
                Transaction Inquiry</h2>
        </div>
        <div>
            <ul class="form-wrapper add-merchant clearfix">
                <%-- <li class="clearfix">
                            <label width="125px;">
                                Merchant Access Key :</label>
                            <input type="text" name="merchantId" size="16" class="text" maxlength="64" value=""></li>--%>
                <li class="clearfix">
                    <label width="125px;">
                        From Date:</label>
                    <input type="text" id="txnStartDate" name="txnStartDate" size="64" class="text" maxlength="64"
                        value=""></li>
                <li class="">
                    <label>
                        To Date:</label>
                    <input type="text" id="txnEndDate" name="txnEndDate" size="64" class="text" maxlength="64"
                        value="">
                </li>
                <li>
                    <label width="125px;">
                        &nbsp;</label>
                    <input type="button" class="btn-orange" name="btnSub" value="Search" onclick="JavaScript:onClk_Submit();"></li>
            </ul>
        </div>
        </form>
        <%
              if (String.Compare("Submit", Request["search"]) == 0)
            {
                String key = "3eae2a218bda60db701aef08a2065c8ef1fa618a";
                CitrusPay.MerchantKit.Infrastructure.CitruspayConstants.merchantKey = key;
                string merchantId = "NC2CO87YYOPJZ35P79RY";
                string txnStartDate = Request["txnStartDate"];
                string txnEndDate = Request["txnEndDate"];
                System.Collections.Generic.IDictionary<string, object> transactionSearchData = new System.Collections.Generic.Dictionary<string, object>(); ;                  
                transactionSearchData.Add("merchantAccessKey", merchantId);
                transactionSearchData.Add("txnStartDate", txnStartDate);
                transactionSearchData.Add("txnEndDate", txnEndDate);
                transactionSearchData.Add("bankName", "ABC BANK");

                CitrusPay.MerchantKit.Services.CitrusPayTransactionSearchService transactionSearchService =
                                        new CitrusPay.MerchantKit.Services.CitrusPayTransactionSearchService();
                CitrusPay.MerchantKit.Entities.TransactionSearchCollection trans =
                                        transactionSearchService.Create(transactionSearchData);

                System.Diagnostics.Debug.WriteLine("Response Code: " + trans.RespCode);
                System.Diagnostics.Debug.WriteLine("Response Message : " + trans.RespMsg);
                int count = trans.Transaction == null ? 0 : trans.Transaction.Count;
                System.Diagnostics.Debug.WriteLine("No of Transactions : " + count);
        %>
        <div>
            <h3>
                Transaction History</h3>
            <ul class="tbl-wrapper clearfix" id="chkoutPageUserPramList">
                <li class="tbl-header">
                    <div class="tbl-col col-1">
                        Response Code</div>
                    <div class="tbl-col col-2">
                        Response Message</div>
                    <div class="tbl-col col-3">
                        Txn Id</div>
                    <div class="tbl-col col-4">
                        Epg Txn Id</div>
                    <div class="tbl-col col-5">
                        AuthIdCode:</div>
                    <div class="tbl-col col-6">
                        Issuer Ref. No.</div>
                    <div class="tbl-col col-11">
                        Txn Amount</div>
                    <div class="tbl-col col-11">
                        Txn Type</div>
                    <div class="tbl-col col-11">
                        Txn Date</div>
                </li>
                <%
                        if (trans.Transaction != null && trans.Transaction.Count != 0)
                        {
                            foreach (CitrusPay.MerchantKit.Entities.TransactionSearch transaction in trans.Transaction)
                            {
                            %>
                            <li>
                                <div class="tbl-col col-11">
                                    <%
                                            Response.Write(transaction.RespCode);
                                    %>
                                </div>
                                <div class="tbl-col col-4">
                                    <%
                                            Response.Write(transaction.RespMsg == null ? "" : transaction.RespMsg);
                                    %>
                                </div>
                                <div class="tbl-col col-4">
                                    <%
                                        Response.Write(transaction.MerchantTxnId == null ? "" : transaction.MerchantTxnId);
                                    %>
                                </div>
                                <div class="tbl-col col-4">
                                    <%
                                            Response.Write(transaction.TxnId == null ? "" : transaction.TxnId);
                                    %>
                                </div>
                                <div class="tbl-col col-4">
                                    <%
                                            Response.Write(transaction.PgTxnId == null ? "" : transaction.PgTxnId);
                                    %>
                                </div>
                                <div class="tbl-col col-5">
                                    <%
                                            Response.Write(transaction.AuthIdCode == null ? "" : transaction.AuthIdCode);
                                    %>
                                </div>
                                <div class="tbl-col col-3">
                                    <%
                                            Response.Write(transaction.RRN == null ? "" : transaction.RRN);
                                    %>
                                </div>
                                <div class="tbl-col col-7">
                                    <%
                                            Response.Write(transaction.TxnType == null ? "" : transaction.TxnType);
                                    %>
                                </div>
                                <div class="tbl-col col-11">
                                    <%
                                            Response.Write(transaction.Amount == null ? "" : transaction.Amount);
                                    %>
                                </div>
                                <div class="tbl-col col-7">
                                    <%
                                            Response.Write(transaction.TxnDateTime == null
                                                                ? ""
                                                                : transaction.TxnDateTime.Substring(0, 10));
                                    %>
                    </div>
                </li>
                <%
}
                        }
                        else
                        {	%>
                <div class="tbl-col col-1">
                    <%
                        Response.Write(trans.RespCode);
                    %>
                </div>
                <div class="tbl-col col-2">
                    <%
                        Response.Write(trans.RespMsg == null ? "" : trans.RespMsg);
                    %>
                </div>
                <%	
}
                %>
            </ul>
        </div>
        <%
            }
        %>
        <script language="javascript" type="text/javascript">
            function onClk_Submit() {
                var mrtId = "NC2CO87YYOPJZ35P79RY";
                var frmDate = document.searchForm.txnStartDate.value;
                var todate = document.searchForm.txnEndDate.value;

                if (mrtId.length > 0 && frmDate.length > 0) {
                    document.searchForm.search.value = "Submit";
                    document.searchForm.method = "POST";
                    document.searchForm.submit();
                } else {
                    if (frmDate.length <= 0) {
                        alert("Please Enter  From Key");
                        return;
                    }

                    if (todate.length <= 0) {
                        alert("Please Enter Transaction From Date");
                        return;
                    }
                    return;

                }
            }
        </script>
    </div>
</asp:Content>

