<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Updateemployee.aspx.cs" Inherits="Admin_Updateemployee" %>

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
        }

        table tbody td{border:0}
        table tbody tr{float:left;min-width:185px}
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
                                <b>&nbsp;&nbsp;&nbsp;&nbsp;Update Employee</b>
                            </label>
                        </h5>
                    </div>
                    <label id="lblerror" runat="server" style="color: red; padding: 10px 0;"></label>
                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                        <div class="search_contentpad">

                            <div class="" id="">
                                <ul class="form_fields_container">
                                    <li>
                                        <label class="search_label" id="lblPrdctID">First Name </label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtfirst" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Last Name</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtsecond" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblPrdctID">Email ID</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtemail" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Password</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtpass" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">MobileNo</label>
                                        <div class="form_input fl_left">
                                            <asp:TextBox ID="txtmbl" runat="server" required></asp:TextBox>
                                        </div>
                                    </li>
                                    <li>
                                        <label class="search_label" id="lblstoresku">Is Active</label>
                                        <div class="form_input fl_left">
                                            <asp:CheckBox ID="txtCheckBox" runat="server" Checked="true" />
                                        </div>
                                    </li>
                                   


                                    
                                </ul>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
     
        <div id="" class="">
            <div class="one_wrap" >
                <div id="" class="widget" style="">
                    <div class="widget_title">
                        <span style="font-family: iconSweets;"></span>
                        <h5>
                            <label id="ctl00_ctl00_Main_Main_lblProductList">
                                <b>&nbsp;&nbsp;&nbsp;&nbsp;Permission Restrictions</b>
                            </label>
                        </h5>
                    </div>
                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom">

                        <div class="search_contentpad">

                            <div class="margin_10" id="margin_10">
                                <ul class="form_fields_container">
                                     <li>
                                        <asp:CheckBoxList ID="chkaccess" runat="server"></asp:CheckBoxList>
                                    </li>
                                    </ul>
                                <div class="globalaction_bar text_right">
                                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="">*</span>
                                        <asp:Button  ID ="btnSavedetails" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClick="btnSavedetails_Click1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                     </div>
                 </div>
             </div>
    <%-- <asp:Label ID="lblfirst" runat="server" Text="Firstname"></asp:Label>
    <asp:TextBox ID="txtfirst" runat="server" required></asp:TextBox>
        <asp:Label ID="lblsecond" runat="server" Text="Lastname"></asp:Label>
    <asp:TextBox ID="txtsecond" runat="server" required></asp:TextBox>
    <asp:Label ID="lblemail" runat="server" Text="EmailID"></asp:Label>
    <asp:TextBox ID="txtemail" runat="server" required></asp:TextBox>
        <asp:Label ID="lblpass" runat="server" Text="Password"></asp:Label>
    <asp:TextBox ID="txtpass" runat="server" required ></asp:TextBox>
     <asp:Label ID="lblmbl" runat="server" Text="MobileNo"></asp:Label>
    <asp:TextBox ID="txtmbl" runat="server" required></asp:TextBox>
    <asp:Label ID="chklabel" runat="server" Text="IsActive"></asp:Label>
    <asp:CheckBox ID="txtCheckBox" runat="server" Checked="true" />
        <asp:CheckBoxList ID="chkaccess" runat="server" checked="true"></asp:CheckBoxList>

     <asp:Button ID="btnSavedetails" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClick="btnSavedetails_Click1"/>
    --%>
</asp:Content>
