<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_CountryInfo" CodeFile="CountryInfo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <title>Countries</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            CountryGrid();
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

