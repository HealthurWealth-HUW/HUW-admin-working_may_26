<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="FeaturesCategoryinfo.aspx.cs" Inherits="Admin_FeaturesCategoryinfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"/>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>Features Category</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            FeaturesCategoryGrid();
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

