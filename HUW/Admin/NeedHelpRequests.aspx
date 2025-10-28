<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="NeedHelpRequests.aspx.cs" Inherits="Admin_NeedHelpRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #ContentPlaceHolder1_gdvNeedHelp a{color:#000;text-decoration:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script src="../sc/jquery.min.js" type="text/javascript"></script>--%>



    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="/Styles/dataTables.tableTools.css" rel="stylesheet" />


    <script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_007.js"></script>
    <script type="text/javascript" src="Orders_files/form_elements.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_008.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_009.js"></script>
    <script type="text/javascript" src="Orders_files/jquery.js"></script>
    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Orders_files/main.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_004.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_003.js"></script>
    <script type="text/javascript" src="Orders_files/ckeditor.js"></script>
    <script type="text/javascript" src="Orders_files/jquery_005.js"></script>
    <script type="text/javascript" src="../Scripts/js/jquery.jqGrid.min.js"></script>


    <script type="text/javascript" src="/Scripts/js/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="Scripts/js/dataTables.tableTools.js"></script>
    <script type="text/javascript" src="js/jquery.table2excel.js"></script>


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
                        <label id="lblOrdrNo">Help Id</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtHelpId" runat="server" />
                        </div>
                    </div>
                    <div class="two_colfields">
                        <label id="lblCourierName">Help Status</label>
                        <div class="form_input">
                            <asp:DropDownList ID="ddlNeedStatus" runat="server" name="CourierName">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                                <asp:ListItem Value="1">Pending</asp:ListItem>
                                <asp:ListItem Value="2">Closed</asp:ListItem>
                            </asp:DropDownList>
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
                        <label id="lblMobile">Mobile</label>
                        <div class="form_input">
                            <asp:TextBox ID="txtMobile" runat="server" placeholder="Mobile No" />
                        </div>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <asp:Button ID="btnNeedHelp" runat="server" Text="Search" OnClick="btnNeedHelp_Click" CssClass="button_small greyishBtn" />
            </div>
        </div>
    </div>
    <%--<div id="content_wrap" >--%>
    <div class="widget">
        <div id="cp_placeholder">
            <!--Activity Stats-->
            <div id="activity_stats">
                <h1>
                    <label id="ctl00_ctl00_Main_Main_lblPageTitle">Need Help Requests</label>
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
                    <asp:GridView AllowPaging ="True" style="color:black" OnPageIndexChanging = "OnPaging" ID="gdvNeedHelp" CssClass="" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <%-- <asp:BoundField HeaderText="Help ID" DataField="Help_Id" />--%>
                            <asp:TemplateField HeaderText="Help ID">
                                <ItemTemplate>
                                    <asp:LinkButton Text="" Style="color: #222" PostBackUrl='<%# "~/Admin/HelpRequests.aspx?ID=" + Eval("Help_Id")+"&Name="+ Eval("Person_Name")+"&Email="+ Eval("Person_Email_Id")+"&Phone="+ Eval("Person_Contact")+"&Status="+ Eval("Status") %>' runat="server"><%# Eval("Help_Id") %></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Date" DataField="Help_Date" ItemStyle-Width="100" DataFormatString="{0:MMM dd yyyy}" />
                            <asp:BoundField HeaderText="Person Name" DataField="Person_Name" />
                            <asp:BoundField HeaderText="Contact" DataField="Person_Contact" />
                            <asp:BoundField HeaderText="Email ID" DataField="Person_Email_Id" />
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Status").ToString() == "1" ? "Pending" : "Closed" %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" Visible="false">
                                <ItemTemplate>
                                    <a id="lnkChangeStatus" style="color: blue" href="javascript:;" onclick="viewStatusPopup(<%# Eval("Help_Id") %>)">Change Status</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Need Help Comments</span>
            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label6">Help Status:</label></td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input type="hidden" id="hdnHelpId" />
                                <select style="opacity: 10;" name="ddlHelpStatus" id="ddlHelpStatus">
                                    <option value="0">Select</option>
                                    <option value="1">Pending</option>
                                    <option value="2">Closed</option>
                                </select></td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px; position: relative; top: -15px;">
                                <label id="lblRecivedby">Comments: </label>
                            </td>
                            <td style="text-align: left; padding-left: 10px;">
                                <%--<input name="txtHelpComments" id="txtHelpComments" type="text" style="width: 186px;" />--%>
                                <textarea name="txtHelpComments" id="txtHelpComments" style="width: 186px;"></textarea>
                            </td>
                        </tr>
                    </table>
                    <div id="divHelpComments">
                    </div>
                    <div class="action_bar text_right">
                        <input value="Ship" onclick='HelpComments()' id="Submit1" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        $('#vpb_show_help_box').hide();
        function vpb_show_help_box() {
            $("#vpb_pop_background").css({
                "opacity": "0.6"
            });
            $("#vpb_pop_background").fadeIn("slow");
            $("#vpb_help_pop_up_box").fadeIn('fast');
            window.scroll(0, 0);
        }

        function viewStatusPopup(helpId) {
            $('#hdnHelpId').val(helpId);
            vpb_show_help_box();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/getHelpComments?HelpId=' + helpId,
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        html = '';
                        $.each(data, function (index, item) {
                            html += '<p>' + item.Comment + ' --- ' + item.Comment_Date + '</p><br/>';
                        });
                        $('#divHelpComments').html(html);
                    }
                    vpb_show_login_box();
                }
            });
        }

        function HelpComments() {
            var helpId = $('#hdnHelpId').val();
            var comment = $('#txtHelpComments').val();
            var status = $('#ddlHelpStatus').val();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/NeedHelpComments?HelpId=' + helpId + '&Comments=' + comment + '&status=' + status,
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    location.reload();
                }
            });
        }
    </script>
    <style>
        table tr th {
            background: #12549a;
            padding: 10px;
            border-right: 1px solid #fff;
        }
    </style>
</asp:Content>

