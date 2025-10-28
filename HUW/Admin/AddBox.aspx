<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBox.aspx.cs" Inherits="Admin_AddBox" MasterPageFile="~/Admin/AdminMaster.master" %>

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
   <%-- <script type="text/javascript">
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

    </script>--%>
    <%--<script type="text/javascript">
        $(document).keypress(function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $('#btnSearch').click();
            }

        });
    </script>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            GetBox();
        });

        function ExporttoExcel() {
            $("#divPaymentOrdersGrd").table2excel({
                name: "Worksheet Name",
                filename: "Order_Process" //do not include extension
            });
        }

        function FillBoxDetails(BoxId) {
       $.ajax({
        url: '../api/Master/GetBoxDetails?Id=' + BoxId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#ContentPlaceHolder1_txtBoxName').val(data.BoxName);
            $('#ContentPlaceHolder1_txtLength').val(data.Lengths);
            $('#ContentPlaceHolder1_txtWidth').val(data.Width);
            $('#ContentPlaceHolder1_txtHeight').val(data.Height);
            $('#ContentPlaceHolder1_txthiddent').val(data.BoxId);
          
             }
         });
        }
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
            <h5>Add Box</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <asp:TextBox ID="txthiddent" type="hidden" runat="server"  />
                    <div class="two_colfields">
                        <label id="lblCouponCode">Box Name</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtBoxName" runat="server" />
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblCartAmount">Length</label>
                        <div class="form_input">
                            <div id="ddlCartStatus">
                                <asp:TextBox ID="txtLength" runat="server" />
                            </div>
                        </div>
                    </div>
                </li>      
                 <li>
                    <div class="two_colfields">
                        <label id="lblValidFrom">Height</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtHeight" runat="server" />
                        </div>
                    </div>
                    
                    <div class="two_colfields">
                        <label id="ctl00_ctl00_Main_Main_lblValidTo">Width</label>
                        <div class="form_input">
                            <div id="lblValidTo">
                                <asp:TextBox ID="txtWidth" runat="server" />
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
        
         <!--One_Three-->
            <div class="one_wrap fl_left">
                <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display: none;">
                    <div id="ctl00_ctl00_Main_Main_divloadingOrders">
                        <center>
                            <img id="imgLoad" alt="" src="Orders_files/progress-indicator.gif">
                        </center>
                    </div>
                </div>
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <input type="hidden" id="lblPtrnsId" />
                    <label id="lblSelectedboxes"></label>
                    <%--<input style='display: inline-block;' type='button' onclick='ExporttoExcel()' class='button_small greyishBtn fl_right' value='Export to Excel' />
                    <input style='display: inline-block;' type='button' onclick='Movettoauthorized()' class='button_small greyishBtn fl_right' value='Move back to Authorized' />--%>
                    <br /><br />
                    <asp:Label runat="server" ID="Label3" ForeColor="Red" Font-Bold="true">Packing Box List</asp:Label><br /><br />
                    <div id="divBoxGrid"></div>

                </div>

            </div>

    </div>
</asp:Content>

