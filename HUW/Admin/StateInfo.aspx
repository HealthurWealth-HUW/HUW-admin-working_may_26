<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_Default" CodeFile="StateInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>States</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            StatesGrid();
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

