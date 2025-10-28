<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="OfflineSales.aspx.cs" Inherits="Admin_OfflineSales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
        });
        </script>
  <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <table id="list">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pager">
        </div>
    <div id="divinsertstatus" class="msgbar msg_Success hide_onC"></div>
    <table>
        
        <tr>
              <td>Offline InvoiceID</td><td><asp:TextBox ID="txtOfflineInvoiceID" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
        <tr>
              <td>SubProduct Id</td><td><asp:TextBox ID="txtSubProductId" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>Product Id</td><td><asp:TextBox ID="txtProductId" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>Quantity</td><td><asp:TextBox ID="txtQuantity" ClientIDMode="Static"  runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>Total price</td><td><asp:TextBox ID="txtTotalprice" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <tr><td>User Name</td><td><asp:TextBox ID="txtbuyername" ClientIDMode="Static"  runat="server"></asp:TextBox></td></tr>
              <td>Phonenumber</td><td><asp:TextBox ID="txtPhonenum" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>AddressLine</td><td><asp:TextBox ID="txtAddress" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
                <tr><td>Sold Date</td><td><asp:TextBox ID="txtsolddate" ClientIDMode="Static"  runat="server"></asp:TextBox></td></tr>
         <tr>
              <td>IsActive</td><td><asp:TextBox ID="txtIsactive" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
        <tr><td></td><td><input type="button" id="btnsubmit" value="Submit" onclick="Insertofflinesales()"/></td></tr>
    </table>
</asp:Content>