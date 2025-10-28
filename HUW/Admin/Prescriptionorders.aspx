<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster1.master"  CodeFile="Prescriptionorders.aspx.cs" Inherits="Admin_Prescriptionorders" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script type="text/javascript">
    //<![CDATA[
    jQuery(document).ready(function () {
        pendingrecordsnew();
    });
    function getfiltereddata() {

        $.ajax({
            datatype: 'json',
            url: '../api/Master/Getpendinglistnew?status=' + $('#ContentPlaceHolder1_DDlstatus').val(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    var prddata = PendingDataNew(data.Result);
                    $('#pager').html(prddata);
                    $rep_datatable = $('#tbl').dataTable();
                    $("#pending").show();
                }
            }
        });

    }
        //]]>
</script>

   <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='Label1'><b>&nbsp;&nbsp;&nbsp;&nbsp;Pending Orders</b></label>
            </h5>
        </div>
    </div>

    <div class="col-lg-12">
        <div class="col-lg-6 offset-lg-3">
            <div class="form-group mt-3">
                <label class="control-label">Status:</label>
                    <asp:dropdownlist id="DDlstatus" runat="server" onchange="updatestatus(this)" CssClass="form-control">
                    <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Approve" Value="2"></asp:ListItem>
                     <asp:ListItem Text="Ready for pickup" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Dispatched" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Delivered" Value="5"></asp:ListItem>
                    <asp:ListItem Text="Reject" Value="6"></asp:ListItem>
                </asp:dropdownlist>
               </div>
               <div class="form-group">
                <label class="control-label"></label>
                    <input type="button" value="Search" class="btn btn-primary mt-2" onclick="getfiltereddata()" /></td>
          </div>
</div>
</div>

<div class="widget">
    <div class='widget_title'>
        <h5>
            <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Pending Prescriptions </b></label>
        </h5>
    </div>
</div>
<div id="pending" class="widget col-lg-12">

    <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
        <div id="pager">
        </div>
    </div>
</div>
    </asp:Content>
