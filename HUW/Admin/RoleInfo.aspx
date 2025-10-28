<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_RoleInfo" CodeFile="RoleInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
     <title>Role Info</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            RoleGrid();
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

