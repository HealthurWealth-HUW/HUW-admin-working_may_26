<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="BusinessType.aspx.cs" Inherits="Admin_BusinessType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"/>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>Business Types</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {


            BusinessGrid();
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

