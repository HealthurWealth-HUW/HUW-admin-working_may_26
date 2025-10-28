<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SareeStatus.aspx.cs" Inherits="Admin_SareeStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
  
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Validation/stylesheet.css" rel="stylesheet" type="text/css" />
   
     <script language="javascript" type="text/javascript">

         function GetProductStatus() {

             jQuery.support.cors = true;
             $.ajax({
                 contentType: "application/json; charset=utf-8",
                 url: '../api/Master/GetproductStatus?ProductId=' + $("#txtPid").val(),
                 type: 'GET',
                 dataType: 'json',
                 success: function (data) {

                     ProductResponse(data);
                     function clearconsole() {
                         console.log(window.console);
                         if (window.console || window.console.firebug) {
                             console.clear();
                         }
                     }
                 },
                 error: function (x, y, z) {
                     $('.success, .warning, .attention, .information, .error').remove();
                     $('.error').fadeIn('slow');
                     function clearconsole() {
                         console.log(window.console);
                         if (window.console || window.console.firebug) {
                             console.clear();
                         }
                     }
                 }

             });
         }





         function ProductResponse(products) {

             var orderhtml = OrderTable(products);
             $("#divProductTable").html(orderhtml);
             $("#lblStatus").html(ProductStatusAsString(products.IsSold));
         }
    


    </script>

    <script type="text/javascript">
        function ProductStatusAsString(status) {
            return status == "0" ? "Pending" : (status == "1" ? "Delivered" : (status == "2" ? "Delivery Failed" : "Amount Transaction Failed"));

        }
        function OrderTable(products) {
            var strHtmltr = "";

            var strHtml = " <table class='list'><thead><tr><td class='left'>Product ID</td><td class='left'>ProductName</td>";
            strHtml = strHtml + "<td class='right'>product Cost</td><td class='right'>Status</td></tr>";
            strHtml = strHtml + " </thead><tbody>"
            strHtml = strHtml + "<tr><td class='left' ><img  style='height:70px;width:50px;' src='" + products.ProductImgUrl.replace("~/", "") + "' alt='" + products.ProductName + "'> <br /><small>" + products.ProductId + "</small>";
            strHtml = strHtml + "</td><td class='left'>" + products.ProductName + "</td>";
            strHtml = strHtml + " <td class='right'>" + products.ProductCost + "</td>";
            strHtml = strHtml + " <td class='right'>  <label id='lblStatus'></label></td></tr>";
            strHtml= strHtml +   " </tbody><tbody id='Tbody1'></table>";

            return strHtml; 

        }
    </script>
      
  
   <table ><tr>>
   <td> 
   <label>Product Id</label>
   </td>
   <td><input type="text" id="txtPid" /></td></tr>
   <tr><td></td><td><input type="button"  value="Verify" id="btnVerify" onclick="GetProductStatus()" /></td></tr>
   </table>
   <br />
   <br />
   <div id="divProductTable"></div>
    
  
</asp:Content>
