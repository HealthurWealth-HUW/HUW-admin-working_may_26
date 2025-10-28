<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="MyTransactions.aspx.cs" Inherits="Admin_MyTransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <title>Transaction Inquiry Details</title>
    <link href="Citrus/css/default.css" rel="stylesheet" type="text/css" />
  

    <div id="page-header">
        <div class="page-wrap">
            <div class="logo-wrapper">
                <a href="/citruspay-admin-site/">
                    <img height="32" width="81" src="Citrus/images/logo_citrus.png" alt="Citrus" />
                </a>
            </div>
        </div>
    </div>
    <div id="page-client-logo">
        &#160;</div>
    <div id="page-wrapper">
        <div class="box-white">
            <div class="page-content">
                <!-- content goes here -->
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
                               Reference Record No:</label>
                            <input type="text" name="merchantTxnID" size="64" class="text" maxlength="64" value=""/></li>
                        <li>
                            <label width="125px;">
                                &nbsp;</label>
                            <input type="button" class="btn-orange" name="btnSub" value="Search" onclick="JavaScript:onClk_Submit();" /></li>
                    </ul>
                </div>
                </form>
                <%
                    if (String.Compare("Submit", Request["search"]) == 0)
                    {
                        string key = "3eae2a218bda60db701aef08a2065c8ef1fa618a";
                        CitrusPay.MerchantKit.Infrastructure.CitruspayConstants.merchantKey = key;
                        string merchantId = "NC2CO87YYOPJZ35P79RY";
                        string merchantTxnId = Request["merchantTxnID"];
                        System.Collections.Generic.IDictionary<string, object> enquiryData = new System.Collections.Generic.Dictionary<string, object>();
                        enquiryData.Add("merchantAccessKey", merchantId);
                        enquiryData.Add("transactionId", merchantTxnId);
                        enquiryData.Add("bankName", "ABC BANK");

                        CitrusPay.MerchantKit.Services.CitrusPayEnquiryService enquiryService =
                                                             new CitrusPay.MerchantKit.Services.CitrusPayEnquiryService();
                        CitrusPay.MerchantKit.Entities.EnquiryCollection enquiryResult =
                                                             enquiryService.Create(enquiryData);

                        System.Diagnostics.Debug.WriteLine("PGSearchResponse received from payment gateway:"
                                                                                            + enquiryResult.RespMsg);
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
                            System.Collections.Generic.List<CitrusPay.MerchantKit.Entities.Enquiry> enqList = enquiryResult.Enquiry;
                        if (enqList != null && enqList.Count != 0)
                        {
                            foreach (CitrusPay.MerchantKit.Entities.Enquiry enquiry in enqList)
                            {				 
                        %>
                        <li>
                            <div class="tbl-col col-1">
                                <%
                Response.Write(enquiry.RespCode);
                                %>
                            </div>
                            <div class="tbl-col col-2">
                                <%
                Response.Write(enquiry.RespMsg);
                                %>
                            </div>
                            <div class="tbl-col col-3">
                                <%
                Response.Write(enquiry.TxnId == null ? "" : enquiry.TxnId);
                                %>
                            </div>
                            <div class="tbl-col col-4">
                                <%
                Response.Write(enquiry.PgTxnId == null ? "" : enquiry.PgTxnId);
                                %>
                            </div>
                            <div class="tbl-col col-5">
                                <%
                Response.Write(enquiry.AuthIdCode == null ? "" : enquiry.AuthIdCode);
                                %>
                            </div>
                            <div class="tbl-col col-6">
                                <%
                Response.Write(enquiry.RRN == null ? "" : enquiry.RRN);
                                %>
                            </div>
                            <div class="tbl-col col-11">
                                <%
                Response.Write(enquiry.Amount == null ? "" : enquiry.Amount);
                                %>
                            </div>
                            <div class="tbl-col col-11">
                                <%
                Response.Write(enquiry.TxnType == null ? "" : enquiry.TxnType);
                                %>
                            </div>
                            <div class="tbl-col col-11">
                                <%
                Response.Write(enquiry.TxnDateTime == null ? "" : enquiry.TxnDateTime.Substring(0, 10));
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
                                Response.Write(enquiryResult.RespCode);
                            %>
                        </div>
                        <div class="tbl-col col-2">
                            <%
                                Response.Write(enquiryResult.RespMsg);
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
                        var mrtTxnId = document.searchForm.merchantTxnID.value;

                        if (mrtId.length > 0 && mrtTxnId.length > 0) {
                            document.searchForm.search.value = "Submit";
                            document.searchForm.method = "POST";
                            document.searchForm.submit();
                        } else {
                            if (mrtId.length <= 0) {
                                alert("Please Enter Merchant Access Key");
                                return;
                            }

                            if (mrtTxnId.length <= 0) {
                                alert("Please Enter Merchant Transaction No");
                                return;
                            }
                            return;

                        }
                    }
                </script>
                <!-- end content -->
            </div>
        </div>
    </div>
    <div>
        <div>
           <%-- Copyrights © 2012 Citrus.--%></div>
    </div>

</asp:Content>

