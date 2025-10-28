<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Couponslist.aspx.cs" Inherits="Admin_Couponslist" %>
   <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <title>Edit Coupons</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            GetCouponslist();
        });
        //]]>
    </script>

    

    <div id="CoupList">

        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
            <div id="pager">
            </div>
        </div>
    </div>

</asp:Content>