<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProductReviews.aspx.cs" Inherits="Admin_ProductReviews" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <script src="../Scripts/js/custom.js" type="text/javascript"></script>



<script type="text/javascript">
    $(document).ready(function () {
        GetProductReviewsList();
    });
</script>
<div id="content">
<h1>Reviews Info</h1>
<div class="padding20">
<br />
<div id="list" class="cart-info">

</div>

</div>
</div>
</asp:Content>


