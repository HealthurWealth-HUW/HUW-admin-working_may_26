<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="ApproveReviews.aspx.cs" Inherits="Admin_ApproveReviews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<title>Pincodes</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            GetProductReviews();
            GetProduct();
            //AdminIsApproveReview(ReviewId);
        });
        //]]>
        </script>
  <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <div id="tblreviews"></div>
        <div id="pager">
        </div>
<%--    <div id="divinsertstatus" class="msgbar msg_Success hide_onC"></div>--%>

</asp:Content>
