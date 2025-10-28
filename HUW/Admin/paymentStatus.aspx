<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="paymentStatus.aspx.cs" Inherits="Admin_paymentStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <div>
        <table>
            <tr>
                <td>Payment Status :
                </td>
                <td>
                    <select id='ddlstatus' class='uniform_pager_list'>
                        <option value='0'>Select</option>
                        <option value='1'>Success</option>
                        <option value='2'>Cancel</option>
                        <option value='3'>Pending</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" value="Search" onclick="GetPaymentStaus('0');" /></td>
            </tr>
        </table>
        <div class="one_wrap fl_left">
            <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display: none;">
                <div id="ctl00_ctl00_Main_Main_divloadingOrders">
                    <center>
                        <img id="imgLoad" alt="" src="Orders_files/progress-indicator.gif">
                    </center>
                </div>
            </div>
            <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                <div id="divPaymentOrdersGrd"></div>
            </div>

        </div>
    </div>
</asp:Content>

