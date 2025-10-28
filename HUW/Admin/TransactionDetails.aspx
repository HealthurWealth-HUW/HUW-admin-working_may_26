<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master"   CodeFile="TransactionDetails.aspx.cs" Inherits="Admin_TransactionDetails" %> 


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>Edit Products</title>
         <script type="text/javascript">
             //<![CDATA[
             jQuery(document).ready(function () {
                 Transactiondetails();
             });
             //]]>
        </script>
  
         <table id="list">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pager">
        </div>
</asp:Content>
