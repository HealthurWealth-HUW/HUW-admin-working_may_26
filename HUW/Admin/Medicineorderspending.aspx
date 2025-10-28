<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Admin/AdminMaster.master" CodeFile="Medicineorderspending.aspx.cs" Inherits="Admin_Medicineorderspending" %>

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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/datepicker/1.0.10/datepicker.min.css" integrity="sha512-YdYyWQf8AS4WSB0WWdc3FbQ3Ypdm0QCWD2k4hgfqbQbRCJBEgX0iAegkl2S1Evma5ImaVXLBeUkIlP6hQ1eYKQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<script src="js/custom.js" type="text/javascript"></script>

<script type="text/javascript">
    //<![CDATA[
    jQuery(document).ready(function () {
        pendingrecordsnewmedicines();
         $('#btnDownload1').hide();
    });
    function getfiltereddata() {
        
        var ddl = $('#ContentPlaceHolder1_DDlstatus').val();
        if (ddl == 5) {
            $('#btnDownload1').show();
        }
        else {
            $('#btnDownload1').hide();
        };
        $.ajax({
            datatype: 'json',
            url: '../api/Master/Getpendinglistmedicinenew?status=' + $('#ContentPlaceHolder1_DDlstatus').val(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    var prddata = PendingDataNewMedicines(data.Result);
                    $('#pager').html(prddata);
                    $rep_datatable = $('#tbl').dataTable();
                    $("#pending").show();
                }
            }
        });

    }
        //]]>
</script>
    </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='Label1'><b>&nbsp;&nbsp;&nbsp;&nbsp;Medicine Pending Orders</b></label>
            </h5>
        </div>
    </div>
    
    <div class="col-lg-12">
            <div class="col-lg-6 offset-lg-3">
                <div class="form-group mt-3">
                    <label class="control-label">Status:</label>
                      <asp:dropdownlist id="DDlstatus" runat="server" CssClass="form-control" onchange="updatestatus(this)">
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
                    <input type="button" value="Search" onclick="getfiltereddata()"  class="btn btn-primary mt-2" id="datefilterbutton" />
                </div>
            </div>
             
            </div>
    <div id="btnDownload1">
     <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='Label1'><b>&nbsp;&nbsp;&nbsp;&nbsp;Download Report To Delivered Order Report</b></label>
            </h5>
        </div>
    </div>
    
    <div class="col-lg-12">
            <div class="col-lg-5 offset-lg-3">
                <div class="form-group mt-3">
                    <label class="control-label">From Date:</label>
                      <asp:TextBox runat="server" ID="txtfromdate" CssClass="datepicker"></asp:TextBox>
                </div>
            </div>
        <div class="col-lg-5 offset-lg-3">
                <div class="form-group mt-3">
                    <label class="control-label">To Date:</label>
                      <asp:TextBox runat="server" ID="txttodate" CssClass="datepicker"></asp:TextBox>
                </div>
            </div>
        <div class="col-lg-2 offset-lg-3">
                 <div class="form-group">
                    <label class="control-label"></label>
                     <asp:Button runat="server" class="btn btn-primary mt-2" ID="btnDownload" Text="Download" onclick="btnDownload_Click"></asp:Button>
                </div>
            </div>
             
            </div>
    </div>
<div>
<%--
        <table>
            <tr>
                <td>Status :
                </td>
                <td>
                  
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input /></td>
            </tr>
        </table>--%>
</div>

<div class="widget">
    <div class='widget_title'>
        <h5>
            <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Pending Prescriptions </b></label>
        </h5>
    </div>
</div>
<div id="pending" class="col-lg-12 widget">

    <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
        <div id="pager">
        </div>
    </div>
</div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/datepicker/1.0.10/datepicker.min.js" integrity="sha512-RCgrAvvoLpP7KVgTkTctrUdv7C6t7Un3p1iaoPr1++3pybCyCsCZZN7QEHMZTcJTmcJ7jzexTO+eFpHk4OCFAg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
   <script src="https://cdnjs.cloudflare.com/ajax/libs/datepicker/1.0.10/datepicker.common.min.js" integrity="sha512-mO702mHlRSyiikpZ/tE5PDAHi1FYjgjDkoT/yvK7DgE8o22bcq/fvby2tqF5G9tOJrnFakgBSuv/Oki5UMq1jA==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/datepicker/1.0.10/datepicker.esm.min.js" integrity="sha512-ra/RUbfkWg5FK8u7aDV6+vWA4zhc/oqW34Y00pHBJ/4EIXE0Wq7oAOnahMp66hQH48IMF5nIbrncIvJmSYurQw==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
   <script>
       $('.datepicker').datepicker({
    format: 'yyyy/MM/dd'
});
   </script>
    </asp:Content>

