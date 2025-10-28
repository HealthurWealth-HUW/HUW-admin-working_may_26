<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="transactions.aspx.cs" Inherits="Admin_transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <title>Tourism Times</title>
    <link rel="stylesheet" href="templates/mirror/stylesheet3cc5.css?v=1.6" type="text/css" />
    <link rel="stylesheet" href="templates/mirror/instyles.css" type="text/css" />
    <script type="text/javascript" src="templates/uploadify/jquery-1.4.2.min.js"></script>

    <link href="templates/css/template.css" rel="Stylesheet" type="text/css" />
    <link href="templates/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="templates/css/validationEngine.jquery.css" rel="Stylesheet" type="text/css" />
    <script src="templates/js/jquery-1.6.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="templates/js/jquery.validationEngine-en.js" type="text/javascript" charset="utf-8"></script>
    <script src="templates/js/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
    <%--<script src="js/jquery-1.9.1.js" type="text/javascript" charset="utf-8"></script>--%>
    <script src="templates/js/jquery-ui.js" type="text/javascript"></script>
     <link href="templates/css/default.css" rel="stylesheet" type="text/css">
   
    <div>
        <div id="">
        </div>
        <div id="">
            <%--<a href="#">
                <img src="#" style="text-align: left;" alt="" class="logo" />
            </a>--%>
            <%--<div id="navigation">
                <ul class="nav">
                </ul>
            </div>--%>
            <div class="page">
                <div class="">
                <form name="searchForm">
                <input type="hidden" name="search" value="">
                <div class="paddling-lef:5px;">
                    <h2  margin-top: "20px;" margin-left:"20px;">
                        Transaction Inquiry</h2>
                </div>
                <div>
                    <ul class="form-wrapper add-merchant clearfix">
                       <%-- <li class="clearfix">
                            <label width="125px;">
                                Merchant Access Key :</label>
                            <input type="text" name="merchantId" size="16" class="text" maxlength="64" value=""></li>--%>
                        <li class="clearfix">
                            <label width="125px;"  margin-top: "20px;" margin-left:"20px;">
                               Reference Record No:</label>
                            <input type="text" name="merchantTxnID" size="64" class="text" maxlength="64" value=""></li>
                        <li>
                            <label width="125px;">
                                &nbsp;</label>
                            <input type="button"   value="Search" onclick="onClk_Submit();" /></li>
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
                           <%-- <div class="tbl-col col-1">
                                Response Code</div>--%>
                            <div class="tbl-col col-2">
                                Response Message</div>
                            <%--<div class="tbl-col col-3">
                                Txn Id</div>--%>
                           <%-- <div class="tbl-col col-4">
                                Epg Txn Id</div>--%>
                           <%-- <div class="tbl-col col-5">
                                AuthIdCode:</div>--%>
                           <%-- <div class="tbl-col col-6">
                                Issuer Ref. No.</div>--%>
                            <div class="tbl-col col-11">
                                Txn Amount</div>
                            <%--<div class="tbl-col col-11">
                                Txn Type</div>--%>
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
                           <%-- <div class="tbl-col col-1">
                                <%
                Response.Write(enquiry.RespCode);
                                %>
                            </div>--%>
                            <div class="tbl-col col-2">
                                <%
                Response.Write(enquiry.RespMsg);
                                %>
                            </div>
                          <%--  <div class="tbl-col col-3">
                                <%
                Response.Write(enquiry.TxnId == null ? "" : enquiry.TxnId);
                                %>
                            </div>--%>
                            <%--<div class="tbl-col col-4">
                                <%
                Response.Write(enquiry.PgTxnId == null ? "" : enquiry.PgTxnId);
                                %>
                            </div>--%>
                           <%-- <div class="tbl-col col-5">
                                <%
                Response.Write(enquiry.AuthIdCode == null ? "" : enquiry.AuthIdCode);
                                %>
                            </div>--%>
                            <%--<div class="tbl-col col-6">
                                <%
                Response.Write(enquiry.RRN == null ? "" : enquiry.RRN);
                                %>
                            </div>--%>
                            <div class="tbl-col col-11">
                                <%
                Response.Write(enquiry.Amount == null ? "" : enquiry.Amount);
                                %>
                            </div>
                            <%--<div class="tbl-col col-11">
                                <%
                Response.Write(enquiry.TxnType == null ? "" : enquiry.TxnType);
                                %>
                            </div>--%>
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
                </div>
            </div>

            <div class="" style="padding-top: 40px; padding-left:300px;" >
                <%--<table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                        <td>
                        <p style="color: #FFFFFF; font-size: 15px; font-weight:bold;">
                                Customer Care : +91 9246282897
                            </p>
                            <p style="color: #FFFFFF; font-size: 12px;">
                                Copyright © 2013 Tourism Times. All rights reserved.</p>
                            
                        </td>
                    </tr>
                </table>--%>
            </div>
        </div>
    </div>
    
</asp:Content>

