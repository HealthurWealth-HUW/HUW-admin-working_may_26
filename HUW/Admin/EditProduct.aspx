<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="EditProduct.aspx.cs" Inherits="Admin_EditProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>Edit Products</title>
         <script type="text/javascript">
        //<![CDATA[
             jQuery(document).ready(function () {
                 EditProductGrid();
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
        
               <%--<table id="listProductFeatures">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pagerProductFeatures">
        </div>
         <table id="listProductSpecifications">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pagerProductSpecifications">
        </div>
         <table id="listProductsGallery">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pagerProductsGallery">
        </div>--%>
</asp:Content>

