<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Excell2.aspx.cs" Inherits="Admin_Excell2" MasterPageFile="~/Admin/AdminMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="RichTextEditor/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/themes/advanced/skins/default/ui.css" rel="stylesheet" type="text/css" />
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/plugins/inlinepopups/skins/clearlooks2/window.css" rel="stylesheet" type="text/css" />


    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" /> 

    <script src="js/custom.js" type="text/javascript"></script>
    <script src="Orders_files/bootstrap-filestyle.min.js"></script>
    <style>
        table th{
            color:#000 !important;
            font-weight:700;
            white-space:pre;
            padding:10px;
        }
        div.selector, div.selector select{
            width:100% !important;
        }
        .group-span-filestyle{    
            color: #333;
    background-color: #17599d;
    border-color: #ccc;
            display: inline-block;
    padding: 0px 12px;
    margin-bottom: 0;
    font-size: 14px;
    font-weight: 400;
    line-height: 1.42857143;
    text-align: center;
    white-space: nowrap;
    vertical-align: middle;
    -ms-touch-action: manipulation;
    touch-action: manipulation;
    cursor: pointer;
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    background-image: none;
    border: 1px solid transparent;
    border-radius: 4px;
        }
        .group-span-filestyle span.buttonText{
            color:#fff;
        }
    </style>
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
    <script>
        function AddAWB() {
            alert();
            debugger;
            $.ajax({
                type: 'GET',
                url: '../api/Master/AddAWB',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                async: false,
                success: function (data) {
                    //alert("Hi");
                    alert(data.Result);
                },
            });
        }
    </script>
    <script>
        function AddAWB() {
            alert();
            debugger;
            $.ajax({
                type: 'GET',
                url: '../api/Master/AddAWB',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                async: false,
                success: function (data) {
                    alert("Hi");
                    alert(data.Result);
                },
            });
        }
    </script>
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
    <script>
        // Add the following code if you want the name of the file appear on select
        $(".custom-file-input").on("change", function () {
            var fileName = $(this).val().split("\\").pop();
            $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
        });
       $(document).ready(function () {  
        $('#ContentPlaceHolder1_New_Data').DataTable({ "paging": false, "ordering": false, "searching": false });  
    });  
    </script>

    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Orders</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body p-3">
            <div class="form-group pt-2">
                <label class="control-label">AWB File</label>
                <asp:FileUpload ID="fileAWB" class="custom-file-input filestyle" validate="form1" require="This field is required" name="fileAWB" runat="server" data-buttonText="Select a File" />
               <%-- <div class="custom-file mt-2">
                    <asp:FileUpload ID="fileAWB" class="custom-file-input" validate="form1" require="This field is required" name="fileAWB" runat="server" />
                    <label class="custom-file-label" for="customFile"></label>
                </div>--%>
            </div>
            <div class="form-group">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </div>
            <%-- <ul class="form_fields_container">
                <li id="ctl00_ctl00_Main_Main_litsmallcontainer">
                    <label>AWB File</label>

                    <div class="form_input">
                        <span class="oneThree"></span>
                    </div>
                </li>
            </ul>--%>
            <%--  <div class="action_bar text_right">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </div>--%>
            <%-- <div class="action_bar text_right">
                 <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click"  Text="AWBUploadExcelFormat"/>* In this PaymentMode COD give the 1 if PPD give 2 and OrderId Default 0.
            </div>--%>
            <div class="alert alert-success in alert-dismissible text-center" id="successmessage" runat="server">               
               <label id="lblMessage" runat="server" style="color: forestgreen;"  oninput="showdata()"></label>
            </div>
           
        </div>
        <div id="filters" runat="server" class="form-row p-3">
            <%--            <label>order id:</label><input id="order_id_search" type="text" runat="server" />
            <asp:Button ID="search_filter_button" runat="server" OnClick="filter_method" Text="search" />--%>
            <div class="form-group col-lg-6">
                <label class="control-label mb-2">Order ID:</label>
                <asp:TextBox type="text" class="textboxess form-control" ID="OrderID_txt" runat="server" />
            </div>
            <div class="form-group col-lg-6">
                <label class="control-label mb-2">Status:</label>
                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Recieved" Value="Paid"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group col-lg-6">
                <label class="control-label mb-2">From Date:</label>
                <asp:TextBox type="text" class="datepicker form-control" ID="From_Date_txt" runat="server" AutoCompleteType="None" />
            </div>
            <div class="form-group col-lg-6">
                <label class="control-label mb-2">To Date:</label>
                <asp:TextBox type="text" class="datepicker form-control" ID="To_Date_txt" runat="server" AutoCompleteType="None" />
            </div>

            <div class="form-group col-lg-12">
                <label class="control-label"></label>
                <asp:Button ID="show_pending_records_all" CssClass="btn btn-primary" runat="server" OnClick="pending_records_bind" Text="Search" />&nbsp;&nbsp;
                <%--<label class="text-danger">Will take some to load once pressed</label>--%>
                <input type="button" class="btn btn-secondary" onclick="cleartextboxes()" value="Reset" />
            </div>
        </div>
        <%--  <label>Order ID:</label><asp:TextBox type="text" class="textboxess" ID="OrderID_txt" runat="server" /><br />
            <label>From Date:</label><asp:TextBox type="text" class="datepicker" ID="From_Date_txt" runat="server" AutoCompleteType="None" /><br />
            <label>To Date:</label><asp:TextBox type="text" class="datepicker" ID="To_Date_txt" runat="server" AutoCompleteType="None" /><br />--%>

        <%--  <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                <asp:ListItem Text="Recieved" Value="Paid"></asp:ListItem>
            </asp:DropDownList><br />--%>




        <%--            <asp:Button ID="show_pending_records_all" runat="server" OnClick="pending_records_bind" Text="search" /><label style="color: red">will take some to load once pressed</label>
            <input type="button" onclick="cleartextboxes()" value="Clear filter" />--%>
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
            <asp:GridView ID="New_Data" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="4" CssClass="dt-responsive border" Width="100%" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="AWB_number" HeaderText="AWB Number"/>
                    <asp:BoundField DataField="Order_Number" HeaderText="Order Number" />
                    <asp:BoundField DataField="Pickup_Date" HeaderText="Pickup Date" />
                    <asp:BoundField DataField="Origin" HeaderText="Origin" />
                    <asp:BoundField DataField="Shipper" HeaderText="Shipper" />
                    <asp:BoundField DataField="Consignee" HeaderText="Consignee" />
                    <asp:BoundField DataField="COD_Due" HeaderText="COD Due" />
                    <asp:BoundField DataField="Remitted_Amount" HeaderText="Remitted Amount" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" />
                    <asp:BoundField DataField="Dest_Centre" HeaderText="Dest Center" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Del_Date" HeaderText="Del Date" />
                    <asp:BoundField DataField="Payment_Ref_and_Date" HeaderText="Payment Ref and Date" />
                    <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name" />
                    <asp:BoundField DataField="Bank_Ref" HeaderText="Bank Ref" />
                </Columns>
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle BackColor="#e5e5e5" Font-Bold="True" ForeColor="Black" />
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
            <asp:GridView ID="Existing_Data" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="4" CssClass="dt-responsive border" Width="100%" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="AWB_number" HeaderText="AWB Number"/>
                    <asp:BoundField DataField="Order_Number" HeaderText="Order Number"/>
                    <asp:BoundField DataField="Pickup_Date" HeaderText="Pickup Date" />
                    <asp:BoundField DataField="Origin" HeaderText="Origin" />
                    <asp:BoundField DataField="Shipper" HeaderText="Shipper" />
                    <asp:BoundField DataField="Consignee" HeaderText="Consignee" />
                    <asp:BoundField DataField="COD_Due" HeaderText="COD Due" />
                    <asp:BoundField DataField="Remitted_Amount" HeaderText="Remitted Amount" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" />
                    <asp:BoundField DataField="Dest_Centre" HeaderText="Dest Center" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="Del_Date" HeaderText="Del Date" />
                    <asp:BoundField DataField="Payment_Ref_and_Date" HeaderText="Payment Ref and Date" />
                    <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name" />
                    <asp:BoundField DataField="Bank_Ref" HeaderText="Bank Ref" />
                </Columns>
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#e5e5e5 " Font-Bold="True" ForeColor="White" />
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
    <div id></div>
    </div>
</asp:Content>
