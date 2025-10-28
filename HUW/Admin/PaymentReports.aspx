<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/AdminMaster.master"  CodeFile="PaymentReports.aspx.cs" Inherits="Admin_PaymentReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>PaymentReports</title>
    <script type="text/javascript">
        $(document).ready(function () {
            PaymentReports();
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
</asp:Content>
