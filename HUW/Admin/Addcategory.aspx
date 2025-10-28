<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Addcategory.aspx.cs" Inherits="Admin_Addcategory" MasterPageFile="~/Admin/AdminMaster.master" %>

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
            <h5>Add Category</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li id="ctl00_ctl00_Main_Main_lblCat">
                                <label>
                                    Super Categories
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div id="ctl00_ctl00_Main_Main_dvStoreWithLabel">
                                <div id="ctl00_ctl00_Main_Main_dvStoreCategory">
                                    <span id="Span2" class="oneThree">
                                        <asp:DropDownList ID="ddlSuperCatogeries" Style="height: 25px;" runat="server" AutoPostBack="true"
                                            >
                                        </asp:DropDownList>
                                    </span>
                                </div>

                            </div>
                        </li>
                <li>
                    <div class="two_colfields">
                        <label id="lblCouponCode">CategoryName</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtCategoryname" runat="server" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtCategoryname" runat="server" ErrorMessage="Enter Category Name"></asp:RequiredFieldValidator>

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


