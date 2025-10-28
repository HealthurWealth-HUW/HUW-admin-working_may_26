<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Access.aspx.cs" Inherits="Admin_Access" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <style type="text/css">
        .form_fields_container {
            width: 88%;
    </style>
    <script type="text/javascript">
         //$(document).ready(function () {
         //     $('#ContentPlaceHolder1_con').click(function () {
         //        $('#step2').show();
         //    });
         //});

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="step1" runat="server">
        <div id="" class="">
            <div class="one_wrap " id="" style="">
                <div id="" class="widget" style="">
                    <div class="widget_title">
                        <span style="font-family: iconSweets;"></span>
                        <h5>
                            <label id="">
                                <b>&nbsp;&nbsp;&nbsp;&nbsp;Add Page</b>
                            </label>
                        </h5>
                    </div>

                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                        <div class="search_contentpad">

                            <div class="" id="">
                                <ul class="form_fields_container">
                                    <li>
                                        <label class="search_label" id="">Page Name</label>
                                         <div class="form_input fl_left">
                                          <asp:TextBox ID="txtname" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                      <li>
                                        <label class="search_label" id="">Page URL</label>
                                         <div class="form_input fl_left">
                                             <asp:TextBox ID="txtpageurl" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                      <li>
                                        <label class="search_label" id="">IsPage</label>
                                         <div class="form_input fl_left">
                                           <asp:CheckBox ID="CheckBox1" runat="server" />
                                        </div>
                                    </li>
                                      <div class="globalaction_bar text_right">
                                          <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                         <asp:Button ID="btnSavedetails" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClick="btnSavedetails_Click" />
                                        </div>
                                  
                                </ul>
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>
   <%-- <asp:Label ID="lblname" runat="server" Text="Pagename"></asp:Label>
    <asp:TextBox ID="txtname" runat="server" required></asp:TextBox>
    <asp:Label ID="lblpageurl" runat="server" Text="PageUrl"></asp:Label>
    <asp:TextBox ID="txtpageurl" runat="server" required></asp:TextBox>--%>
    <%-- <asp:DropDownList ID="ddList" runat="server">
    <asp:ListItem Value="">--Select Month--</asp:ListItem>
    <asp:ListItem Value="true">Page</asp:ListItem>
    <asp:ListItem Value="false">Method</asp:ListItem>
 
    </asp:DropDownList>--%>
   <%-- <asp:Label ID="Label1" runat="server" Text="IsPage"></asp:Label>
    <asp:CheckBox ID="CheckBox1" runat="server" />--%>
   


</asp:Content>
