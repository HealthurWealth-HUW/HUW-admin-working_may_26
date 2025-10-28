<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="HelpRequests.aspx.cs" Inherits="Admin_HelpRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="/Styles/dataTables.tableTools.css" rel="stylesheet" />


    <script type="text/javascript" src="Orders_files/jquery_006.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
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


    <script type="text/javascript" src="/Scripts/js/jquery.dataTables.min.js"></script>

    <script type="text/javascript" src="Scripts/js/dataTables.tableTools.js"></script>
    <script type="text/javascript" src="js/jquery.table2excel.js"></script>


    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Need Help Comments</h5>
        </div>
        <div class="widget_body clearfix">
            <div class="space" style="height: 20px;"></div>
              <div class="col-md-3 form-group">
                <div class="">
                    <label class="col-md-7">Help ID:</label>
                    <div class="col-md-5">
                        <span class=""><strong style="font-weight: 700;font-size:18px;color: #105298;" id="helpId" runat="server">342</strong></span>
                    </div>
                </div>
            </div>
            <div class="col-md-4 form-group">
                <div class="">
                    <label class="col-md-6 text-right">Contact Number:</label>
                    <div class="col-md-6">
                        <span class="" style="color: #da3b80;"><strong style="font-weight: 700;font-size:18px" id="phone" runat="server"></strong></span>
                    </div>
                </div>
            </div>

          

            <div class="col-md-5 form-group">
                <div class="">
                    <label class="col-md-6 text-right">Person Name:</label>
                    <div class="col-md-6">
                        <span class=""><strong style="font-weight: 700" id="personName" runat="server"></strong></span>
                    </div>
                </div>
            </div>

            <div class="col-md-6 form-group">
                <div class="">
                    <label class="col-md-3">Email ID:</label>
                    <div class="col-md-8">
                        <span class=""><strong style="font-weight: 700" id="personEmail" runat="server"></strong></span>
                    </div>
                </div>
            </div>

             <div class="col-md-6">
                <div class="">
                    <label class="col-md-5 text-right">Change Status:</label>
                    <div class="col-md-7">
                        <asp:DropDownList CssClass="form-control input-sm" ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" runat="server">
                            <asp:ListItem Value="1">Pending</asp:ListItem>
                            <asp:ListItem Value="2">Closed</asp:ListItem>
                        </asp:DropDownList>
                        <%--<select class="form-control input-sm" id="ddlStatus" runat="server">
                            <option value="1">Pending</option>
                            <option value="2">Closed</option>
                        </select>--%>
                    </div>
                </div>
            </div>
            <div class="col-xs-12"><hr /></div>
            <%--<div class="space" style="height: 30px;"></div>--%>
            <div class="col-md-12 form-group">
                <div class="">
                    <%--<label class="col-md-2">Comments:</label>--%>
                    <div class="col-md-12">
                        <h4 class="" style="font-size: 18px;">Your Comment(s)</h4>
                        <p>&nbsp;</p>
                        <div class="bg-box">
                            <textarea class="form-control" placeholder="Comment here..." id="txComment" runat="server"></textarea>
                            <asp:Button id="btnComment" class="btn-orange" runat="server" OnClick="btnComment_Click" Text="Comment" />
                        </div>
                       <%-- <p>&nbsp;</p>
                        <div class="text-right"><a href="javascript:;" class="text-black rply">Reply</a></div>
                        <br />
                         <div class="rpl" style="display:none;">
                        <textarea class="form-control" placeholder="Reply here..."></textarea>
                    </div>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-12 form-group">
                <div class="col-md-12" id="divComment" runat="server">
                    <h4 class="" style="font-size: 18px;">Replies</h4>
                    <p>&nbsp;</p>
                    <%--<div class="bg-box clearfix">
                        <p class="text-right" style="font-size: 12px;color: #16579c;font-weight: bold; margin-bottom:0"><i class="fa fa-calendar"></i> 26th June 2018</p>
                       <div class="col-md-2 text-center">
                            <img src="http://www.myiconfinder.com/uploads/iconsets/256-256-5644a46a81b2b9ab1557a2d7064680f5-user.png" class="img-responsive" alt="image" />
                            John
                        </div>
                        <div class="col-md-10 row">
                            
                            Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.
                             
                        </div>

                    </div>
                                       
                    <br />                   
                    <div class="bg-box clearfix">
                        <div class="col-md-2 text-center">
                            <img src="http://www.myiconfinder.com/uploads/iconsets/256-256-5644a46a81b2b9ab1557a2d7064680f5-user.png" class="img-responsive" alt="image" />
                            John
                        </div>
                        <span class="col-md-10 row">Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.</span>
                    </div>                   
                   --%>
                </div>
            </div>
        </div>




        <%--<table cellpadding="0" cellspacing="0" border="0" width="100%">
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
                    <td style="width: 40%; text-align: right; padding-right: 10px;">
                        <label id="lblRecivedby">Comments: </label>
                    </td>
                    <td style="text-align: left; padding-left: 10px;">
                       <%-- <input name="txtHelpComments" id="txtHelpComments" type="text" style="width: 186px;" />--%
                        <textarea id="txtHelpComments" class="form-control" name="txtHelpComments" style="width: 186px;"></textarea>
                    </td>
                </tr>
            </table>--%>
        <%--<div id="divHelpComments">
            </div>
            <div class="action_bar text_right">
                <input value="Ship" onclick='HelpComments()' id="Submit1" class="button_small greyishBtn" type="button" />
                <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
            </div>--%>
    </div>
    
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script type="text/javascript">
        $('.rpl').hide();
        $('.rply').click(function () {
            $('.rpl').fadeToggle();
        });
       
    </script>
    <style>
        .form-group {
            margin-bottom: 30px;
        }

        .bg-box {
            background: #f1f1f1;
            padding: 10px;
            font-size: 13px;
            line-height: 17px;
        }

        .text-black {
            color: #222 !important;
        }

        .mt-5 {
            margin-top: 5px;
        }
    </style>
</asp:Content>

