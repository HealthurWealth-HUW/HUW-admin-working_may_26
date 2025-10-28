<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="NotifymeProducts.aspx.cs" Inherits="Admin_NotifymeProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Help</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <ul class="form_fields_container">
                <li>
                    <div class="two_colfields">
                        <label id="lblOrdrNo">Product Id</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtProductId" runat="server" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label id="lblEmailId">Email</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtEmail" runat="server" />
                        </div>
                    </div>
                </li>
                <li>
                    <div class="two_colfields">
                        <label id="lblName">Name</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtName" runat="server" placeholder="Name" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label id="lblMobile">ProductName</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtProduct" runat="server" placeholder="Product Name" />
                        </div>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <asp:Button ID="btnNotify" runat="server" Text="Search" OnClick="btnNotify_Click" CssClass="button_small greyishBtn" />
            </div>
        </div>
    </div>
   <%-- <div id="content_wrap">--%>
        <div class="widget">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Notify Me Products</label>
                </h1>
            </div>
            <!--Quick Actions-->
            <div id="divMsg" class="msgbar msg_Success hide_onC" style="display: none;">
            </div>
            <div>
                <div id="ctl00_ctl00_Main_Main_PageMessage" class="msgbar msg_Success hide_onC" style="display: none;">
                    <span class="iconsweet">=</span>
                    <label id="ctl00_ctl00_Main_Main_lblSuccess">
                    </label>
                </div>
                <div style="display: none;" id="ctl00_ctl00_Main_Main_PanMsg" class="cp_success">
                </div>
            </div>
            <!--One_Three-->
            <div class="one_wrap fl_left">
                <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display: none;">
                </div>
                <br />
                <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <div class="EU_TableScroll" id="showData" style="display: block">  
                    <asp:GridView ID="gdvNotifyme" CssClass="EU_DataTable" runat="server" AutoGenerateColumns="false" OnRowCommand="gdvNotifyme_RowCommand" RowStyle-Wrap="true">
                        <Columns>
                            <asp:BoundField HeaderText="Product ID" DataField="ProductId"  ItemStyle-Width="5%" />
                            <asp:BoundField HeaderText="Product Name" DataField="ProductName"  ItemStyle-Width="5%" />
                            <asp:BoundField HeaderText="User Name" DataField="UserName"  ItemStyle-Width="5%" />
                            <asp:BoundField HeaderText="Date" DataField="CreatedOn"  ItemStyle-Width="5%" />
                            <asp:BoundField HeaderText="Phone" DataField="MobileNumber"  ItemStyle-Width="5%" />
                            <asp:BoundField HeaderText="Email ID" DataField="EmailID"  ItemStyle-Width="5%" />
                            <asp:TemplateField HeaderText="NotiFy Here">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkNotifyme" runat="server" ForeColor="Blue" Text="Send Mail" CommandName="NotifyMe" CommandArgument='<%#Eval("ProductId")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                        </div>
                </div>
            </div>
        </div>
    </div>
     <style>
        table tr th{
            background:#12549a;
                padding: 10px;
                border-right: 1px solid #fff;
        }



        .EU_TableScroll  
{  
    max - height: 275 px;  
    overflow: auto;  
    border: 1 px solid# ccc;  
}  
.EU_DataTable  
{  
    border - collapse: collapse;  
    width: 100 % ;  
}  
.EU_DataTable tr th  
{  
    background - color: #3c454f;  
color: # ffffff;  
    padding: 10 px 5 px 10 px 5 px;  
    border: 1 px solid# cccccc;  
    font - family: Arial, Helvetica, sans - serif;  
    font - size: 12 px;  
    font - weight: normal;  
    text - transform: capitalize;  
}  
.EU_DataTable tr: nth - child(2 n + 2)  
{  
    background - color: #f3f4f5;  
}  
  
.EU_DataTable tr: nth - child(2 n + 1) td  
{  
        background - color: #d6dadf;  
        color: #454545;  
}  
.EU_DataTable tr td  
{  
padding: 5px 10px 5px 10px;  
color: # 454545;  
        font - family: Arial, Helvetica, sans - serif;  
        font - size: 11 px;  
        border: 1 px solid# cccccc;  
        vertical - align: middle;  
    }  
    .EU_DataTable tr td: first - child  
    {  
        text - align: center;  
    } 
    </style>

</asp:Content>

