<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Prescription pending.aspx.cs" Inherits="Admin_Prescription_pending" %>

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
    <title>Pending</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            pendingrecords();
        });
        //]]>
    </script>

    <%-- <div>
        <table>
            <tr>
                <td>Product ID :
                </td>
                <td>
                    <input type="text" id="txtProductID" class="form-control" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" value="Search" onclick="" /></td>
            </tr>
        </table>
    </div>--%>
    <div class="widget">
        <div class='widget_title'>           
            <h5>
                <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Pending Prescriptions </b></label>
            </h5>
        </div>
    </div>
    <div id="pending">

        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
            <div id="pager">
            </div>
        </div>
    </div>

</asp:Content>
