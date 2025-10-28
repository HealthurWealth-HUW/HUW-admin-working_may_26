<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Admin_Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script src="../Validation/tabs.js" type="text/javascript"></script>
     <script src="../Validation/tabs.js" type="text/javascript"></script>
    <link href="../Validation/stylesheet.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
     <script type="text/javascript" src="../catalog/view/theme/leisure/js/custom.js"></script>
     <script type="text/javascript" src="../catalog/view/theme/leisure/js/jquery.flexslider.js" ></script>
<script type="text/javascript" src="../catalog/view/theme/leisure/js/jquery.jcarousel.js"></script>

     <script type="text/javascript" >
         $(document).ready(function () {

             $.blockUI();

             setTimeout(function () {
                 $.unblockUI({

                 });
             }, 3000);
         });
        
</script>

<script type="text/javascript">
    function ChangeTransctionStatus() {
        var status = $("#ddlStatus").val();
        jQuery.support.cors = true;
        $.ajax({
            url: '../PersonService.asmx/UpdategePymentStatus?TransactionId=' + getParameterByName("ID") + '&StatusVal=' + status,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data.d.Status == "Success") {
                      $("#lblUpdateStatus").html('<div><p>Status Updated Successfully.</p></div>');
                      FillUserTransctionDetails();
                }
            },
            error: function (x, y, z) {
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');
            }
        });

    }
   
</script>


      <script type="text/javascript">
    function GetOrderStatus() {
        jQuery.support.cors = true;
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../PersonService.asmx/GetOrderStatusList',
            type: "GET",
            data: "{}",
            dataType: "json",
            success: function (data) {
                $.each(data.d, function (key, value) {
                    FillUserTransctionDetails();
                    $("#ddlStatus").append($("<option></option>").val(value.Key).html(value.Value));                   
                });
            },
            error: function (x, y, z) {
                //alert(x + '\n' + y + '\n' + z);
            }
        });

    }

   
</script>


      <script type="text/javascript">
    function PrintInvoice() {
        window.location = '../Invoice.aspx?ID=' + getParameterByName("ID");
    }
</script>

      <script type="text/javascript">
          function OrderTable(categories) {

              var strHtmltr = "";

              var count = 0;
              for (categorie in categories) {
                  if (categories.hasOwnProperty(categorie)) {

                      strHtmltr = strHtmltr + "<tr><td class='left' ><img  style='height:70px;width:50px;' src='" + categories[count].Products[0].ProductsGalleries[0].ImgUrl.replace("~/", "") + "' alt='" + categories[count].Products[0].ProductName + "'> <br /><small>" + categories[count].Products[0].ProductId + "</small>";
                      strHtmltr = strHtmltr + "</td><td class='left'>" + categories[count].Products[0].ProductName + "</td>";
                      strHtmltr = strHtmltr + " <td class='right'>" + categories[0].Quantity + "</td>";
                      strHtmltr = strHtmltr + "<td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[count].Products[0].ProductCost * categories[count].PaymentTransactionDetails[0].CurrencyValue).toFixed(2) + "</td>";
                      strHtmltr = strHtmltr + " <td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[count].Products[0].ProductCost * categories[count].PaymentTransactionDetails[0].CurrencyValue).toFixed(2) + "</td></tr>";

                      count++;
                  }
              }

              var strHtml = " <table class='list'><thead><tr><td class='left'>Product ID</td><td class='left'>ProductName</td><td class='right'>Quantity</td>";
              strHtml = strHtml + "<td class='right'>Unit Price</td><td class='right'>Total</td></tr>";
              strHtml = strHtml + " </thead><tbody>" + strHtmltr + " </tbody><tbody id='Tbody1'>";
              strHtml = strHtml + " <tr><td colspan='4' class='right'>ServiceTax:</td><td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[0].PaymentTransactionDetails[0].ServiceTax).toFixed(2) + "</td></tr></tbody>";
              strHtml = strHtml + "<tbody id='Tbody2'> <tr><td colspan='4' class='right'>ShippingCharges:</td><td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[0].PaymentTransactionDetails[0].ShippingCharges).toFixed(2) + "</td></tr>";
              strHtml = strHtml + "<tr><td colspan='4' class='right'>OtherCharges:</td><td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[0].PaymentTransactionDetails[0].OtherCharges).toFixed(2) + "</td></tr>";
              strHtml = strHtml + "<tr><td colspan='4' class='right'>VAT:</td><td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + parseFloat(categories[0].PaymentTransactionDetails[0].VAT).toFixed(2) + "</td></tr></tbody>";
              strHtml = strHtml + "<tbody id='Tbody3'><tr><td colspan='4' class='right'>Total:</td><td class='right'>" + categories[0].PaymentTransactionDetails[0].CurrencySymbol + " " + parseFloat(categories[0].PaymentTransactionDetails[0].TxnAmount).toFixed(2) + "</td></tr></tbody></table>";

              return strHtml; //$("<div></div>").html(); 
          }
         
      </script>

      <script type="text/javascript" language="javascript">

              jQuery(document).ready(function () {
                  GetOrderStatus();
                  FillUserTransctionDetails();

              }); 
      </script>
       
      <script type="text/javascript">
             function FillUserTransctionDetails() {
                 jQuery.support.cors = true;
                 $.ajax({
                     url: '../api/Master/GetOrderDetls?TransactionId=' + getParameterByName("ID"),
                     type: 'GET',
                     dataType: 'json',
                     success: function (data) {
                         ProductResponse(data);
                     },
                     error: function (x, y, z) {
                         alert(x + '\n' + y + '\n' + z);
                     }
                 });

             }
             function PaymentStatusAsString(status) {
                 return status == "0" ? "Un-Authorized" : (status == "SUCCESS" ? "SUCCESS" : (status == "2" ? "Fail" : "Transaction Fail"));
             }

             function OrderstatusAsString(status) {
                 alert(status);
                 return status == "0" ? "Authorized" : (status == "1" ? "Cancelled" : (status == "3" ? "Delivered" : (status == "9" ? "Dispatched" : (status == "8" ? "Waiting for Pickup" : "Pending"))));
             }
             function ProductResponse(categories) {                 
                  
                 $("#ddlStatus").val(PaymentStatusAsString(categories[0].PaymentTransactionDetails[0].PaymentStatus));                
                 $("#lblCustomer").html(categories[0].User[0].RoleName);
                 $("#lblOrderID").html(categories[0].PaymentTransactionDetails[0].PaymentTransactionId);
                 $("#lblEmailId").html(categories[0].User[0].EmailId);
                 $("#lblMobileNumber").html(categories[0].User[0].MobileNo);
                 $("#lblPTotal").html(categories[0].PaymentTransactionDetails[0].TxnAmount);
                 $("#lblFirstName").html(categories[0].User[0].FirstName);
                 $("#lblLastName").html(categories[0].User[0].LastName);
                 $("#lblQuntity").html(categories[0].Quantity);
                 $("#lblOrderStatus").html(OrderstatusAsString(categories[0].PaymentTransactionDetails[0].OrderCurrentStatus));
                 $("#lblTranscationStatus").html(PaymentStatusAsString(categories[0].PaymentTransactionDetails[0].TxnStatus));
                 $("#lblCreateDate").html(categories[0].PaymentTransactionDetails[0].CreatedOn);
                 $("#lblModifiedDate").html(categories[0].PaymentTransactionDetails[0].UpdatedOn);
                 $("#lblOrderCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 //$("#lblIPAddress").html(categories[0].product.ProductId);
                 //      $("#lblUserAgent").html(categories[0].product.ProductId);

                 $("#Baddress").html(categories[0].BillingAddress[0].StreetAddress1 + " , " + categories[0].BillingAddress[0].StreetAddress2 + "," + categories[0].BillingAddress[0].LandMark);
                 $("#lblBFirstName").html(categories[0].User[0].FirstName);
                 $("#lblBLastName").html(categories[0].User[0].LastName);
                 $("#lblBState").html(categories[0].BillingAddress[0].StateName);
                 $("#lblBCity").html(categories[0].BillingAddress[0].City);
                 $("#lblBPostcode").html(categories[0].BillingAddress[0].PinCode);
                 $("#lblBPCity").html(categories[0].BillingAddress[0].City);
                 $("#lblBillingState").html(categories[0].BillingAddress[0].StateName);
                 $("#lblBillingCountry").html(categories[0].Country[0].Country);

                 $("#Saddress").html(categories[0].ShipingAddress[0].StreetAddress1 + "," + categories[0].ShipingAddress[0].StreetAddress2 + "," + categories[0].ShipingAddress[0].LandMark);
                 $("#lblSFirstName").html(categories[0].User[0].FirstName);
                 $("#lbBSlLastName").html(categories[0].User[0].LastName);
                 $("#lblSPostcode").html(categories[0].ShipingAddress[0].PinCode);
                 $("#lblSCity").html(categories[0].ShipingAddress[0].City);
                 $("#lblShippingState").html(categories[0].ShipingAddress[0].StateName);
                 $("#lblSMobileNumber").html(categories[0].User[0].MobileNo);
                 $("#lblSCountry").html(categories[0].Country[0].Country);

                 $("#lblCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 $("#lblTCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 $("#lblTotalCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 var orderhtml = OrderTable(categories);
                 $("#divProductTable").html(orderhtml);

                 $("#lblCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 $("#lblTCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 $("#lblTotalCurrency").html(categories[0].PaymentTransactionDetails[0].CurrencySymbol);
                 var orderhtml = OrderTable(categories);
                 $("#divProductTable").html(orderhtml);



             }
         
         </script>


        <div id="tagline">
            <div style="clear: both;">
            </div>
        </div>
       
       
        <div class="box">
          <div class="heading">
      <h1><img src="../catalog/view/theme/leisure/image/order.png" alt="" /> Orders</h1>&nbsp;&nbsp;&nbsp;
      <b> <label  style="color:Green;"  ID = "lblUpdateStatus" ></label></b>
       <br />
       </div>
       <div class="box">
       <table>
      <tr>       
      <%-- <td> Status :</td>
     <td >
      <asp:DropDownList ID = "ddlStatus" ClientIDMode="Static" runat="server"></asp:DropDownList> 
      </td>
      <td >
         <input type="button"  id="btnUpdate" name="Update" value="Update" onclick = "ChangeTransctionStatus()" />
         </td>--%>
                                      
         <td style="align:right; ">           
      <div class="buttons"> <a onclick="PrintInvoice()" class="button"> Print Invoice</a>&nbsp;&nbsp;&nbsp;&nbsp;
      <%--<a onclick="location = 'MonthlyWiseProductionTransaction.aspx';" class="button">Cancel</a>--%></div>
     </td>
     </tr>
   </table>
  
    </div>
   
           <div class="content">
      <div class="vtabs" style="padding-top:0px;"><a href="#tab-order">Order Details</a>
                <a href="#tab-Billing">Billing Address</a>
               <%-- <a href="#tab-shipping">Shipping Details</a>--%>
                <a href="#tab-product">Products</a>
                <a href="#tab-history">Order History</a>
              </div>
                <div id="tab-order" class="vtabs-content">
                        <table class="form">
                    
                            <tr>
                                <td>
                                    Order ID:
                                </td>
                                <td>
                                  
                                <label id = "lblOrderID" ></label>
                               
                                  </td>
                            </tr>
                            <tr>
                                <td>
                                    Role:
                                </td>
                                <td>
                                  <label id = "lblCustomer" ></label>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    E-Mail:
                                </td>
                                <td>
                                  <label id = "lblEmailId" ></label>
                                
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    MobileNumber:
                                </td>
                                <td>
                                  <label id = "lblMobileNumber" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Amount:
                                </td>
                                <td>
                                 <label id = "lblOrderCurrency" ></label>
                                  <label id = "lblPTotal" ></label>
                                 
                                </td>
                            </tr>
                            
                              <tr>
                                <td>
                                    Transcation Status:
                                </td>
                                <td id="Td1">
                                 
                                       <label id = "lblTranscationStatus" ></label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Order Status:
                                </td>
                                <td id="order-status">
                                  <label id = "lblOrderStatus" ></label>
                                  
                                </td>
                            </tr>
                           <%-- <tr>
                                <td>
                                    IP Address:
                                </td>
                                <td>
                                 <%-- <label id = "Label5" ></label>
                                    <asp:Label ID="lblIPAddress" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    User Agent:
                                </td>
                                <td>
                                 <%-- <label id = "lblUserAgent" ></label>
                                      <asp:Label ID="lblUserAgent" runat="server"></asp:Label>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    Date Added:
                                </td>
                                <td>
                                  <label id = "lblCreateDate" ></label>
                                 
                                </td>
                            </tr>
             
                        </table>
                    </div>
                    <div id="tab-Billing" class="vtabs-content">
                        <div class="heading">
                            Billing Address</div>
                        <table class="form">
                            <tr>
                                <td>
                                    First Name:
                                </td>
                                <td>
                                  <label id = "lblBFirstName" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Last Name:
                                </td>
                                <td>
                                  <label id = "lblBLastName" ></label>
                                
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address :
                                </td>
                                <td>
                                    <div id="Baddress"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City:
                                </td>
                                <td>
                                  <label id = "lblBCity" ></label>
                                 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Postcode:
                                </td>
                                <td>
                                  <label id = "lblBPostcode" ></label>
                                  
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Region / State:
                                </td>
                                <td>
                                  <label id = "lblBillingState" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Country:
                                </td>
                                <td>
                                  <label id = "lblBillingCountry" ></label>
                                 
                                </td>
                            </tr>
                        </table>
                        <label>
                            Shipping Address</label>
                        <table class="form">
                            <tr>
                                <td>
                                    First Name:
                                </td>
                                <td>
                                  <label id = "lblSFirstName" ></label>
                               
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Last Name:
                                </td>
                                <td>
                                  <label id = "lblSlLastName" ></label>
                                 
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Address 1:
                                </td>
                                <td>
                                 
                                  <div id="Saddress"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City:
                                </td>
                                <td>
                                  <label id = "lblSCity" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Postcode:
                                </td>
                                <td>
                                  <label id = "lblSPostcode" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Region / State:
                                </td>
                                <td>
                                  <label id = "lblShippingState" ></label>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Country:
                                </td>
                                <td>
                                  <label id = "lblSCountry" ></label>
                                   
                                </td>
                            </tr>
                        </table>
                    </div>
<%--
                <div id="tab-shipping" class="vtabs-content">
               
                </div>--%>

                <div id="tab-product" class="vtabs-content">
                    <div id="history"></div>
                    <div id="divProductTable"></div>
        
                  </div>
                  <div id="tab-history" class="vtabs-content">
                <div id="Div1" class="vtabs-content">
                    <div id="Div2"></div>
        <table class="form">
          <tr>
            <td>Order Status:</td>
            <td>
            <%--<select name="order_status_id">
                                                <option value="7">Cancelled</option>
                                                                <option value="9">Cancelled Reversal</option>
                                                                <option value="13">Chargeback</option>
                                                                <option value="5">Complete</option>
                                                                <option value="8">Denied</option>
                                                                <option value="14">Expired</option>
                                                                <option value="10">Failed</option>
                                                                <option value="1" selected="selected">Pending</option>
                                                                <option value="15">Processed</option>
                                                                <option value="2">Processing</option>
                                                                <option value="11">Refunded</option>
                                                                <option value="12">Reversed</option>
                                                                <option value="3">Shipped</option>
                                                                <option value="16">Voided</option>
                                              </select>--%>
                                              </td>
          </tr>
          <tr>
            <td>Notify Customer:</td>
            <td><input type="checkbox" name="notify" value="1" /></td>
          </tr>
          <tr>
            <td>Comment:</td>
            <td><textarea name="comment" cols="40" rows="8" style="width: 99%"></textarea>
              <div style="margin-top: 10px; text-align: right;"><a id="button-history" class="button">Add History</a></div></td>
          </tr>
        </table>
                </div>
                </div>





                  
                  
                  
                </div>

            </div>
     
        </div>
        </div>
        </div>
  
  <%--      <script src="Validation/script.js" type="text/javascript"></script>--%>

 
   <script type="text/javascript">
       $('.vtabs a').tabs();
</script>
</asp:Content>

