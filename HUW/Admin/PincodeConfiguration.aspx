<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="PincodeConfiguration.aspx.cs" Inherits="Admin_Pincode_Configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            PinCodeGrid();
            GetStates();
        });
        //]]>
        </script>
  <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <table id="list">
        </table>
        <div id="pager">
        </div>
    <div id="divinsertstatus" class="msgbar msg_Success hide_onC"></div>
    <table>
        <tr>
              <td>States</td><td><asp:DropDownList ID="ddlstate" ClientIDMode="Static" runat="server"></asp:DropDownList></td>
            </tr>
         <tr>
              <td>District</td><td><asp:TextBox ID="txtDistrict" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>City</td><td><asp:TextBox ID="txtCity" ClientIDMode="Static"  runat="server"></asp:TextBox></td>
            </tr>
         <tr>
              <td>Location</td><td><asp:TextBox ID="txtLocation" ClientIDMode="Static" runat="server"></asp:TextBox></td>
            </tr>
        <tr><td>PinCode</td><td><asp:TextBox ID="txtpincode" ClientIDMode="Static"  runat="server"></asp:TextBox></td></tr>
                <tr><td>ControlingStation</td><td><asp:TextBox ID="txtControlingStation" ClientIDMode="Static"  runat="server"></asp:TextBox></td></tr>
        <tr><td>COD</td><td><asp:TextBox ID="txtcod" ClientIDMode="Static"  runat="server"></asp:TextBox></td></tr>
        <tr><td>ShippingAmount</td><td><asp:TextBox ClientIDMode="Static" ID="txtShippingAmount" runat="server"></asp:TextBox></td></tr>      
        <tr><td></td><td><input type="button" id="btnsubmit" value="Submit" onclick="Insertpin()"/></td></tr>
    </table>
</asp:Content>