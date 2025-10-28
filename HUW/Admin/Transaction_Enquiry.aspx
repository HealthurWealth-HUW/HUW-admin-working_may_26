<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Transaction_Enquiry.aspx.cs" Inherits="Admin_Transaction_Enquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      
    <title>Transaction Enquiry Details</title>
    
    <link href="css/default.css" rel="stylesheet" type="text/css">
</head>
<body>
    <div>
         <input type="hidden" id="search" name="search" value="" />
        <ul class="form-wrapper add-merchant clearfix">

            <li class="clearfix">
                <label width="125px;">
                    Merchant Txn ID:</label>
                <%--<asp:TextBox ID="txtmerchantTxnsID" runat="server" Text="merchantTxnsID"></asp:TextBox>--%>
                <input type="text" name="merchantTxnID" id="mchTxnID" size="64" class="text" maxlength="64" value=""></li>
            <li>
                <label width="125px;">
                    &nbsp;</label>
                <input type="button" class="btn-orange" name="btnSub" value="Search" onclick="JavaScript: onGetEnqDetails_Submit();"></li>
        </ul>
    </div>

    <script language="javascript">
        function onGetEnqDetails_Submit() {
            var mrtId = "EYHOAN8NXQICMP879LJU";
            var mrtTxnId = document.getElementById("mchTxnID").value;
            var txnStatus = 0;
            if (mrtId.length > 0 && mrtTxnId.length > 0) {
                document.getElementById("search").value = "Submit";

                $("#searchForm").attr('method', 'POST');
                $('#searchForm').submit();

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

    <%
                    if (String.Compare("Submit", Request["search"]) == 0)
                    {
                        string key = "82830f42afff529394431dc0987a89e17b6aa99a";
                        CitrusPay.MerchantKit.Infrastructure.CitruspayConstants.merchantKey = key;
                        string merchantId = "EYHOAN8NXQICMP879LJU";
                        string merchantTxnId = Request["merchantTxnID"];
                        string txnStatus = Request["0"];
                        System.Collections.Generic.IDictionary<string, object> enquiryData = new System.Collections.Generic.Dictionary<string, object>();
                        enquiryData.Add("merchantAccessKey", merchantId);
                        enquiryData.Add("transactionId", merchantTxnId);
                        enquiryData.Add("txnStatus", txnStatus);

                        CitrusPay.MerchantKit.Services.CitrusPayEnquiryService enquiryService =
                                                             new CitrusPay.MerchantKit.Services.CitrusPayEnquiryService();
                        CitrusPay.MerchantKit.Entities.EnquiryCollection enquiryResult =
                                                             enquiryService.Create(enquiryData);

                        System.Diagnostics.Debug.WriteLine("PGSearchResponse received from payment gateway:"
                                                                                            + enquiryResult.RespMsg);
    %>

    <div>
<br />
        <h1><b><u>Transaction History</u></b></h1>
        <ul class="form-wrapper add-merchant clearfix">

            <%
                            System.Collections.Generic.List<CitrusPay.MerchantKit.Entities.Enquiry> enqList = enquiryResult.Enquiry;
                        if (enqList != null && enqList.Count != 0)
                        {                        
                         			 
            %>
            <li class="clearfix">
                <label width="125px;">
                    Merchant Txn ID :</label>
                <input type="hidden" name="MerchantID" id="MerchantID" size="16" class="text" maxlength="64" value="EYHOAN8NXQICMP879LJU" />
                <input type="text" name="merchantTxnID" id="merchantTxnID" size="64" class="text" maxlength="64" readonly="readonly" value="<% = merchantTxnId== null ? ""  : merchantTxnId %>" />
            </li>

            <li class="clearfix">
                <label width="125px;">
                    Enter Root Sys Ref No :</label>

                <input type="text" name="RootSysRefNum" id="RootSysRefNum" size="64" class="text" maxlength="64" readonly="readonly" value="<% = enqList[0].PgTxnId == null ? "" : enqList[0].PgTxnId %>" />

            </li>
            <li class="clearfix">
                <label width="125px;">
                    Enter Root RRN No :</label>
                <input type="text" name="RootPRefNum" id="RootPRefNum" size="64" class="text" maxlength="64" readonly="readonly" value="<% = enqList[0].RRN == null ? "" : enqList[0].RRN %>" />

            </li>
            <li class="clearfix">
                <label width="125px;">
                    Enter Root Auth Code:</label>
                <input type="text" name="RootAuthID" id="RootAuthID" size="64" class="text" maxlength="64" readonly="readonly" value="<% = enqList[0].AuthIdCode == null ? "" : enqList[0].AuthIdCode %>" />
            </li>
            <li class="clearfix">
                <label width="125px;">
                    Enter Curr Code :</label>
                <input type="text" name="CurrCode" id="CurrCode" size="64" class="text" maxlength="64" readonly="readonly" value="INR">
            </li>
            <li class="clearfix">
                <label width="125px;">
                    Enter the Amount:</label>
                <input type="text" name="Amount" id="Amount" size="64" class="text" maxlength="64" readonly="readonly" value="<% = enqList[0].Amount == null ? "" : enqList[0].Amount %>" /></li>
        </ul>
        <%
            
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
      </form>
                <%
                    }
                %>
    </body>
</asp:Content>

