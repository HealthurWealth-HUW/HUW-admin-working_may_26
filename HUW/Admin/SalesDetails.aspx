<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SalesDetails.aspx.cs" Inherits="Admin_SalesDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <title>UsersInfo</title>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#date-start").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#date-end").datepicker("option", "minDate", selected)
                }
            });
            $("#date-end").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#date-start").datepicker("option", "maxDate", selected)
                }
            });
            $("#date-end").datepicker("setDate", new Date());
            var currentDate = new Date();
            (currentDate).setDate(currentDate.getDate() - 30);
            $("#date-start").datepicker("setDate", currentDate);
            MonthlyWiseUserProductTransactionsGrid();
        });

    </script>
    <table class="form">

        <tr>
            <td style="width: 250px;">Order ID:
            </td>
            <td style="width: 250px;">
                <input type="text" name="txtOrderID" id="txtOrderID" size="12" />
            </td>
            <td style="text-align: left;">
                <%--<input type="submit" value="Filter" onclick="javascript: MonthlyWiseUserProductTransactionsGrid();" />--%>
                <a href="#" onclick="javascript:MonthlyWiseUserProductTransactionsGrid();">Filter</a>
            </td>
        </tr>
        <tr>
            <td style="width: 250px;">User Mail ID:
            </td>
            <td style="width: 250px;">
                <input type="text" name="txtUserMailID" id="txtUserMailID" size="12" />
            </td>
            <td style="text-align: left;">
                <a href="#" onclick="javascript:MonthlyWiseUserProductTransactionsGrid();">Filter</a>
            </td>
        </tr>
        <tr>
            <td>Start Date:<input type="text" name="filter_date_start" id="date-start" size="12" /></td>
            <td>End Date :<input type="text" name="filter_date_end" id="date-end" size="12" /></td>
            <td style="text-align: left;">
                <a href="#" onclick="javascript:MonthlyWiseUserProductTransactionsGrid();">Filter</a>
            </td>
        </tr>
    </table>
    <table id="list">
        <tr>
            <td />
        </tr>
    </table>
    <div id="pager">
    </div>
</asp:Content>

