<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Refund.aspx.cs" Inherits="Admin_Refund" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 
    
    <title>Transaction Inquiry Details</title>
    
    <link href="css/default.css" rel="stylesheet" type="text/css">
</head>
<body>
    <script type="text/javascript">
        $(document).ready(function () {
            var trnsId = getParameterByName("transId");
            $('#merchantTxnsID').val(trnsId);
        });
    </script>
    <div id="page-client-logo">
        &#160;</div>
    <div id="page-wrapper" style="width:730px;">
        <div class="box-white">
            <div class="page-content">
                <!-- content goes here -->
               
                <input type="hidden" id="search" name="search" value="" />
                
                <div class="paddling-lef:5px;">
                    <h2>
                        Refund</h2>
                </div>
                <div>
                    <ul class="form-wrapper add-merchant clearfix">
                       
                        <li class="clearfix">
                            <label width="125px;">
                                Merchant Txn ID:</label>
                                <%--<asp:TextBox ID="txtmerchantTxnsID" runat="server" Text="merchantTxnsID"></asp:TextBox>--%>
                            <input type="text" name="merchantTxnsID" id="merchantTxnsID" size="64" class="text" maxlength="64" value=""></li>
                        <li>
                            <label width="125px;">
                                &nbsp;</label>
                            <input type="button" class="btn-orange" name="btnSub" value="Search" onclick="JavaScript:onGetTransDetails_Submit();"></li>
                    </ul>
                </div>
           

                <%
                    if (String.Compare("Submit", Request["search"]) == 0)
                    {
                        string key = "82830f42afff529394431dc0987a89e17b6aa99a";
                        CitrusPay.MerchantKit.Infrastructure.CitruspayConstants.merchantKey = key;
                        string merchantId = "EYHOAN8NXQICMP879LJU";
                        string merchantTxnId = Request["merchantTxnsID"];
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

               
                  <input type="hidden" name="actionchanged" value="" id="actionchanged" />
                <div>
                    <h3>
                        Transaction History</h3>
                                      <ul class="form-wrapper add-merchant clearfix">
                        
                        <%
                            System.Collections.Generic.List<CitrusPay.MerchantKit.Entities.Enquiry> enqList = enquiryResult.Enquiry;
                        if (enqList != null && enqList.Count != 0)
                        {
                            
                         			 
                        %>
                         <li class="clearfix">
                            <label width="125px;">
                                Merchant Txn ID :</label>
                               <%-- <input type="hidden" name="merchantTxnID" size="16" class="text" maxlength="64" id="merchantTxnID" value="" />--%>
           <%--                   <input type="text" class="text" maxlength="64" name="merchantTxnID" value="<% = merchantTxnId== null ? ""  : merchantTxnId %>" />--%>
                     <input type="hidden" name="MerchantID" id="MerchantID" size="16" class="text" maxlength="64" value="EYHOAN8NXQICMP879LJU" />
                              <input type="text" name="merchantTxnID" id="merchantTxnID" size="64" class="text" maxlength="64" value="<% = merchantTxnId== null ? ""  : merchantTxnId %>" />
                           </li>
                        
                          <li class="clearfix">
                            <label width="125px;">
                                Enter Root Sys Ref No :</label>
                                 
                              <input type="text" name="RootSysRefNum" id="RootSysRefNum" size="64" class="text" maxlength="64" value="<% = enqList[0].PgTxnId == null ? "" : enqList[0].PgTxnId %>" />
                              
                           </li>
                             <li class="clearfix">
                            <label width="125px;">
                           
                                Enter Root RRN No :</label>
                                   <input type="text" name="RootPRefNum" id="RootPRefNum" size="64" class="text" maxlength="64" value="<% = enqList[0].RRN == null ? "" : enqList[0].RRN %>" />
                                
                       </li>
                               <li class="clearfix">
                            <label width="125px;">
                                Enter Root Auth Code:</label>
                                 <input type="text" name="RootAuthID" id="RootAuthID" size="64" class="text" maxlength="64" value="<% = enqList[0].AuthIdCode == null ? "" : enqList[0].AuthIdCode %>" />
                               </li>
                             <li class="clearfix">
                            <label width="125px;">
                                Enter Curr Code :</label>
                                 <input type="text" name="CurrCode" id="CurrCode" size="64" class="text" maxlength="64" value="INR">
                                
                          </li>
                           <li class="clearfix">
                            <label width="125px;">
                                Enter the Amount:</label>
                         <input type="text" name="Amount" id="Amount" size="64" class="text" maxlength="64" value="<% = enqList[0].Amount == null ? "" : enqList[0].Amount %>" /></li>
                           
                            <li class="clearfix">
                            <label width="125px;">
                                Message Type:</label><label>Refund</label>
                            <input type="hidden" name="MessageType" id="MessageType" value="R" />
                        </li>
                      
                         <li>
                            <label width="125px;">
                                &nbsp;</label>
                              <%--  <asp:TextBox ID="btnSubmit" runat="server" Text="Submit" onclientclick="JavaScript:onClk_Submit();"></asp:TextBox>--%>
                            <input type="button" class="btn-orange" name="btnSub" value="Submit" onclick="JavaScript:onClk_Submit();"></li>
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
                <script language="javascript">
                    function onGetTransDetails_Submit() {
                        var mrtId = "EYHOAN8NXQICMP879LJU";
                        var mrtTxnId = document.getElementById("merchantTxnsID").value;                   
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
                <!-- end content -->
                  <%
                    if (String.Compare("Submit", Request["actionchanged"]) == 0)
                    {
                        string key = "82830f42afff529394431dc0987a89e17b6aa99a";
                        CitrusPay.MerchantKit.Infrastructure.CitruspayConstants.merchantKey = key;
                        System.Collections.Generic.IDictionary<string, object> refundData = new System.Collections.Generic.Dictionary<string, object>(); 
                        refundData.Add("merchantAccessKey", Request["MerchantID"]);
                        refundData.Add("transactionId", Request["MerchantTxnID"]);
                        refundData.Add("pgTxnId", Request["RootSysRefNum"]);
                        refundData.Add("RRN", Request["RootPRefNum"]);
                        refundData.Add("authIdCode", Request["RootAuthID"]);
                        refundData.Add("currencyCode", Request["CurrCode"]);
                        refundData.Add("txnType", Request["MessageType"]);
                        refundData.Add("amount", Request["Amount"]);
                        refundData.Add("bankName", "ABC BANK");
                        CitrusPay.MerchantKit.Services.CitrusPayRefundService refundService = new CitrusPay.MerchantKit.Services.CitrusPayRefundService();
                        CitrusPay.MerchantKit.Entities.Refund refund = refundService.Create(refundData);

                        DAL.db_Zon_HuwEntities Entity = new DAL.db_Zon_HuwEntities();
                        if (refund.RespMsg == "Refund successful")
                        {
                            long TxnID=Convert.ToInt64(refund.MerchantTxnId);
                            Utility.PaymentTransaction t = Entity.PaymentTransactions.Where(x => x.PaymentTransactionId == TxnID).First();
                            // OrderReturnAction not using any where thats why we are using for Refund Status in Payment Transactions by Teja
                            t.OrdersReturnAction = "Refund";
                            t.Pickup = false;
                            t.Dispatched = false;
                            t.Delivered = false;
                            t.OrderCurrentStatus = 4;
                            Entity.SaveChanges();
                        }
                %>
                <div>
                    <h3>
                        Transaction Refund Response</h3>
                    <ul class="tbl-wrapper clearfix" id="chkoutPageUserPramList">
						<li class="tbl-header">
							<div class="tbl-col col-1">Response Code</div>
							<div class="tbl-col col-2">Response Message</div>
							<div class="tbl-col col-3">Txn Id</div>
							<div class="tbl-col col-4">Epg Txn Id</div>
							<div class="tbl-col col-5">AuthIdCode:</div>
							<div class="tbl-col col-6">Issuer Ref. No.</div>
							<div class="tbl-col col-7">Txn Amount</div>
						</li>

						<li>
							<div class="tbl-col col-1">
								<%
										Response.Write(refund.RespCode);
									%>
							</div>
							<div class="tbl-col col-2">
								<%
										Response.Write(refund.RespMsg);
									%>
							</div>
							<div class="tbl-col col-3">
								<%
										Response.Write(refund.TxnId == null ? "" : refund.TxnId);
									%>
							</div>
							<div class="tbl-col col-4">
								<%
										Response.Write(refund.PgTxnId == null ? "" : refund.PgTxnId);
									%>
							</div>
							<div class="tbl-col col-5">
								<%
										Response.Write(refund.AuthIdCode == null ? "" : refund.AuthIdCode);
									%>
							</div>
							<div class="tbl-col col-6">
								<%
										Response.Write(refund.RRN == null ? "" : refund.RRN);
									%>
							</div>
							<div class="tbl-col col-7">
								<%
										Response.Write(refund.Amount == null ? "" : refund.Amount);
								%>
							</div>
						</li>
					</ul>
                </div>
                <%
                        }
                %>
                <script language="javascript">
                    function onClk_Submit() {
                        var mrtId = document.getElementById("MerchantID").value;
                        var mrtTxnId =document.getElementById("merchantTxnID").value ;
                        var TxnRefNo = document.getElementById("RootSysRefNum").value;
                        var prevTxnRefNo =document.getElementById("RootPRefNum").value;
                        var authId = document.getElementById("RootAuthID").value;
                        var currcode = document.getElementById("CurrCode").value;
                        var amt = document.getElementById("Amount").value;

                        if (mrtId.length > 0 && mrtTxnId.length > 0
									&& TxnRefNo.length > 0
									&& prevTxnRefNo.length > 0
									&& authId.length > 0 && currcode.length > 0
									&& amt.length > 0) {

                            document.getElementById("actionchanged").value = "Submit";

                            $("#searchForm").attr('method', 'POST');
                            $('#searchForm').submit();

                        } else {
                            alert("field is mandatory");
                            return;
                        }
                    }
                </script>
            </div>
        </div>
    </div>
     
</body>

</asp:Content>

