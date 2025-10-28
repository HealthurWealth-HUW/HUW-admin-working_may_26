<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCoupon.aspx.cs" Inherits="Admin_AddCoupon" MasterPageFile="~/Admin/AdminMaster.master" %>

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
    background-color:#EFEFEF;
    position: fixed;
    width: 100%;
    height: 100%;
    z-index: 1000;
    top: 0px;
    left: 0px;
    opacity: .5; /* in FireFox */ 
    filter: alpha(opacity=50); /* in IE */
}
    .pleaseWaitText{
        position:absolute;
        top:200px;
        left:50%;
        margin-left:-100px;
        font-size:18px;
        font-weight:800;
        color:red;
    }
 </style>

    <script type="text/javascript">
        function PrintInvoice() {
            window.location = '../Invoice.aspx?ID=' + getParameterByName("transId");
        }
    </script>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Orders</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="lblCouponCode">Coupon Code</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtCouponCode" runat="server" />
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblCartAmount">Min. Cart Amount</label>
                        <div class="form_input">
                            <div id="ddlCartStatus">
                                <asp:TextBox ID="txtCartAMount" runat="server" />
                            </div>
                        </div>
                    </div>
                </li>      
                 <li>
                    <div class="two_colfields">
                        <label id="lblValidFrom">Valid From</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtValidfrom" runat="server" />
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblValidTo">Valid To</label>
                        <div class="form_input">
                            <div id="lblValidTo">
                                <asp:TextBox ID="txtValidTo" runat="server" />
                            </div>
                        </div>
                    </div>
                </li>      
                 <li>
                    <div class="two_colfields">
                        <label id="lblDiscountType">Discount Type</label>
                        <div class="form_input">
                            <asp:DropDownList ID="ddlDiscountType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDiscountType_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Amount</asp:ListItem>
                                <asp:ListItem Value="2">Percentage</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="two_colfields" id="divAmount" runat="server" visible="false">
                        <label id="ctl00_ctl00_Main_Main_lblAmount">Discount Amount</label>
                       <div class="form_input">
                            <div id="lblAmount">
                                <asp:TextBox ID="txtAmount" runat="server" />
                            </div>
                        </div>
                    </div>

                     <div class="two_colfields" id="divPercentage" runat="server" visible="false">
                        <label id="ctl00_ctl00_Main_Main_lblPercentage">Discount Percentage</label>
                       <div class="form_input">
                            <div id="lblPercentage">
                                <asp:TextBox ID="txtPercentage" runat="server" />
                            </div>
                        </div>
                    </div>
                </li>      

                <li>
                    <div class="two_colfields">
                        <label id="lblUsage">Max. Total No of Times Usage</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtUsage" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblUserUsage">Max. No Of Time Usage/User</label>
                       <div class="form_input">
                            <div id="lblUserUsage">
                                <asp:TextBox ID="txtUserUsage" runat="server" />
                            </div>
                        </div>
                    </div>                    
                </li>                   
                <li>
                    <div>
                        <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"  Text="Submit"/>           
            </div>
        </div>        
    </div>
</asp:Content>

