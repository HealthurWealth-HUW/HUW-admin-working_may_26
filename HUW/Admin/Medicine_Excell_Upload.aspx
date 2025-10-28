<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Medicine_Excell_Upload.aspx.cs" Inherits="Admin_Medicine_Excell_Upload" MasterPageFile="~/Admin/AdminMaster.master" %>


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
        function cleartextboxes() {
            $(".textboxess").val("");
            $(".datepicker").val("");
        }
    </script>
    <script>
        $(function () {
            $(".datepicker").datepicker({ dateFormat: 'dd-mm-yy' });
        });
    </script>
    <%--    <script type="text/javascript" charset="utf-8">

        $(document).ready(function () {

            $("#Existing_Data").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable();


        });
    </script>--%>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Upload Medicines excell sheet</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li id="ctl00_ctl00_Main_Main_litsmallcontainer">
                    <label>
                        Med Excel File</label>

                    <div class="form_input">
                        <span class="oneThree">
                            <asp:FileUpload ID="fileAWB" validate="form1" require="This field is required" name="fileAWB" Style="height: 25px;" runat="server" />
                        </span>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </div>
            <%-- <div class="action_bar text_right">
                 <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click"  Text="AWBUploadExcelFormat"/>* In this PaymentMode COD give the 1 if PPD give 2 and OrderId Default 0.
            </div>--%>
            <div style="height: 35px">
                <label id="lblMessage" runat="server" style="color: forestgreen; font-size: 20px; margin-left: 34%;" oninput="showdata()"></label>
            </div>
        </div>
        <div hidden>
            <div id="filters" runat="server">


                <%--            <label>order id:</label><input id="order_id_search" type="text" runat="server" />
            <asp:Button ID="search_filter_button" runat="server" OnClick="filter_method" Text="search" />--%>

                <label>Order ID:</label><asp:TextBox type="text" class="textboxess" ID="OrderID_txt" runat="server" /><br />
                <label>From Date:</label><asp:TextBox type="text" class="datepicker" ID="From_Date_txt" runat="server" AutoCompleteType="None" /><br />
                <label>To Date:</label><asp:TextBox type="text" class="datepicker" ID="To_Date_txt" runat="server" AutoCompleteType="None" /><br />

                <asp:DropDownList ID="DropDownList1" runat="server">
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Recieved" Value="Paid"></asp:ListItem>
                </asp:DropDownList><br />
                <br />
                <br />
                <br />



                <asp:Button ID="show_pending_records_all" runat="server" OnClick="pending_records_bind" Text="search" /><label style="color: red">will take some to load once pressed</label>
                <input type="button" onclick="cleartextboxes()" value="Clear filter" />
            </div>
            <div id="pending_records_div" runat="server" style="padding-top: 40px; width: 100%; overflow: scroll; height: 300px">
                <div class="widget">
                    <div class="widget_title relative_position">
                        <span class="iconsweet">r</span>
                        <h5>
                            <label id="ctl00_ctl00_Main_Main_legSelProductt" runat="server">Pending Payment</label>
                        </h5>
                    </div>
                </div>



                <asp:GridView ID="pending_records_grid" runat="server" CellPadding="4" CssClass="dt-responsive" Width="100%" GridLines="None">

                    <Columns>
                        <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="table_04" HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle CssClass="table_02" HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PaymentTransactionId" HeaderText="PaymentTransactionId" ItemStyle-Width="150" />
                        <asp:BoundField DataField="UserId" HeaderText="UserId" ItemStyle-Width="150" />
                        <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="UpdatedOn" HeaderText="UpdatedOn" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="TxnRefNo" HeaderText="TxnRefNo" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="PGTxnId" HeaderText="PGTxnId" ItemStyle-Width="150" />
                        <asp:BoundField DataField="TxnStatus" HeaderText="TxnStatus" ItemStyle-Width="150" />
                        <asp:BoundField DataField="TxnMessage" HeaderText="TxnMessage" ItemStyle-Width="150" />
                        <asp:BoundField DataField="TxnAmount" HeaderText="TxnAmount" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="ServiceTax" HeaderText="ServiceTax" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="VAT" HeaderText="VAT" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="OtherCharges" HeaderText="OtherCharges" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="ShippingCharges" HeaderText="ShippingCharges" ItemStyle-Width="150" />
                        <asp:BoundField DataField="PaymentStatus" HeaderText="PaymentStatus" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="CurrencyCode" HeaderText="CurrencyCode" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="CurrencyValue" HeaderText="CurrencyValue" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="CurrencySymbol" HeaderText="CurrencySymbol" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="ShipmentId" HeaderText="ShipmentId" ItemStyle-Width="150" />
                        <asp:BoundField DataField="ShipmentDate" HeaderText="ShipmentDate" ItemStyle-Width="150" />
                        <asp:BoundField DataField="ShipmentType" HeaderText="ShipmentType" ItemStyle-Width="150" />
                        <asp:BoundField DataField="ShipmentURL" HeaderText="ShipmentURL" ItemStyle-Width="150" />
                        <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="Free_Product_ID" HeaderText="Free_Product_ID" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="Authorized" HeaderText="Authorized" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="Pickup" HeaderText="Pickup" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="PickupID" HeaderText="PickupID" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="PickupDate" HeaderText="PickupDate" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="Dispatched" HeaderText="Dispatched" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="DispatchedID" HeaderText="DispatchedID" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="DispatchedDate" HeaderText="DispatchedDate" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="Delivered" HeaderText="Delivered" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="DeliveredID" HeaderText="DeliveredID" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="DeliveredDate" HeaderText="DeliveredDate" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="CourierName" HeaderText="CourierName" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="OrderCurrentStatus" HeaderText="OrderCurrentStatus" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="ReceivedBy" HeaderText="ReceivedBy" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="Comments" HeaderText="Comments" ItemStyle-Width="150" />
                        <asp:BoundField DataField="OrdersReturnReason" HeaderText="OrdersReturnReason" ItemStyle-Width="150" />
                        <asp:BoundField DataField="OrdersReturnAction" HeaderText="OrdersReturnAction" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="AmountFromMyAccount" HeaderText="AmountFromMyAccount" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="PaymentMode" HeaderText="PaymentMode" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="Has_Promo_Code" HeaderText="Has_Promo_Code" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="Promo_Code_ID" HeaderText="Promo_Code_ID" ItemStyle-Width="150" />---%>
                        <asp:BoundField DataField="Promo_Code_Amount" HeaderText="Promo_Code_Amount" ItemStyle-Width="150" />
                        <%--<asp:BoundField DataField="Offer_Product_ID" HeaderText="Offer_Product_ID" ItemStyle-Width="150" />---%>
                        <%--<asp:BoundField DataField="IsPrescription" HeaderText="IsPrescription" ItemStyle-Width="150" />---%>
                    </Columns>
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#008000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>




            </div>
            <div id="uploader" runat="server">
                <div class="widget">
                    <div class="widget_title relative_position">
                        <span class="iconsweet">r</span>
                        <h5>
                            <label id="ctl00_ctl00_Main_Main_legSelProduct">New data</label>
                        </h5>
                    </div>
                </div>
                <div style="padding-top: 40px; width: 100%; overflow: scroll; height: 300px">
                    <asp:GridView ID="New_Data" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="4" CssClass="dt-responsive" Width="100%" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="AWB_number" HeaderText="AWB Number" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Order_Number" HeaderText="Order Number" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Pickup_Date" HeaderText="Pickup Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Origin" HeaderText="Origin" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Shipper" HeaderText="Shipper" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Consignee" HeaderText="Consignee" ItemStyle-Width="150" />
                            <asp:BoundField DataField="COD_Due" HeaderText="COD Due" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Remitted_Amount" HeaderText="Remitted Amount" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Balance" HeaderText="Balance" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Dest_Centre" HeaderText="Dest Center" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Del_Date" HeaderText="Del Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Payment_Ref_and_Date" HeaderText="Payment Ref and Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Bank_Ref" HeaderText="Bank Ref" ItemStyle-Width="150" />
                        </Columns>
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#008000" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
                <div class="widget">
                    <div class="widget_title relative_position">
                        <span class="iconsweet">r</span>
                        <h5>
                            <label id="ctl00_ctl00_Main_Main_legSelProductx">Exisiting data</label>
                        </h5>
                    </div>
                </div>
                <div style="padding-top: 40px; width: 100%; overflow: scroll; height: 300px">
                    <asp:GridView ID="Existing_Data" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="4" CssClass="dt-responsive" Width="100%" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="AWB_number" HeaderText="AWB Number" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Order_Number" HeaderText="Order Number" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Pickup_Date" HeaderText="Pickup Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Origin" HeaderText="Origin" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Shipper" HeaderText="Shipper" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Consignee" HeaderText="Consignee" ItemStyle-Width="150" />
                            <asp:BoundField DataField="COD_Due" HeaderText="COD Due" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Remitted_Amount" HeaderText="Remitted Amount" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Balance" HeaderText="Balance" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Dest_Centre" HeaderText="Dest Center" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Del_Date" HeaderText="Del Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Payment_Ref_and_Date" HeaderText="Payment Ref and Date" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Bank_Ref" HeaderText="Bank Ref" ItemStyle-Width="150" />
                        </Columns>
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#FF0000 " Font-Bold="True" ForeColor="White" />
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
        </div>
    </div>
</asp:Content>

