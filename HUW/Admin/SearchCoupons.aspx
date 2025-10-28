<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchCoupons.aspx.cs" Inherits="Admin_SearchCoupons" MasterPageFile="~/Admin/AdminMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="RichTextEditor/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/themes/advanced/skins/default/ui.css" rel="stylesheet" type="text/css" />
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/plugins/inlinepopups/skins/clearlooks2/window.css" rel="stylesheet" type="text/css" />
    <script src="js/custom.js" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_007.js"></script>
    <script type="text/javascript" src="Orders_files/form_elements.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_008.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_009.js"></script>
    <script type="text/javascript" src="Orders_files/jquery.js"></script>
    <script type="text/javascript" src="Orders_files/main.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_004.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_003.js"></script>
    <script type="text/javascript" src="Orders_files/ckeditor.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_005.js"></script>
    <script type="text/javascript" src="../Scripts/js/jquery.jqGrid.min.js"></script>

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery.table2excel.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromDate").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#txtToDate").datepicker("option", "minDate", selected)
                }
            });
            $("#txtToDate").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#txtFromDate").datepicker("option", "maxDate", selected)
                }
            });
            GetSearchorders();
        });

    </script>
    <script type="text/javascript">
        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $('#btnSearch').click();
            }

        });
    </script>

    <style type="text/css">
        .overlay {
            background-color: #EFEFEF;
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: 1000;
            top: 0px;
            left: 0px;
            opacity: .5; /* in FireFox */
            filter: alpha(opacity=50); /* in IE */
        }

        .pleaseWaitText {
            position: absolute;
            top: 200px;
            left: 50%;
            margin-left: -100px;
            font-size: 18px;
            font-weight: 800;
            color: red;
        }
    </style>

    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Coupons</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>


    </div>
    <div id="content_wrap">
        <div id="cp_placeholder">
            <!--One_Three-->
            <div class="one_wrap fl_left">
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <asp:GridView ID="gdvCoupons" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="gdvCoupons_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Coupon_Id" HeaderText="Id" />
                            <asp:BoundField DataField="Coupon_Code" HeaderText="Coupon Code" />
                            <asp:BoundField DataField="Min_Cart_Value" HeaderText="Min Value" />
                            <asp:BoundField DataField="Coupon_Percentage" HeaderText="Percentage" />
                            <asp:BoundField DataField="Coupon_Amount" HeaderText="Amount" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate><asp:LinkButton ID="lnkCouponEdit" runat="server" CommandName="EditCoupon" CommandArgument='<%# Eval("Coupon_Id") %>' ForeColor="Red">Edit</asp:LinkButton></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="widget">
            </div>
        </div>
    </div>

</asp:Content>

