<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Changepassword.aspx.cs" Inherits="Admin_Changepassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Change Password</b></label>
            </h5>
        </div>
    </div>
    <div id="filters" class="widget_body clearfix" style="padding-top: 25px; padding-bottom: 30px; padding-left: 20px;">
        <asp:Label ID="errLabel" runat="server" Visible="false"></asp:Label>
        <div class="offset-lg-3">
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label">Old Password:</label>
                    <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtOldpassword" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RegularExpressionValidator1" ControlToValidate="txtOldpassword" runat="server" ErrorMessage="Please enter Old password"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label">New Password:</label>
                    <asp:TextBox TextMode="Password" ID="txtNewpassword" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RegularExpressionValidator2" ControlToValidate="txtNewpassword" runat="server" ErrorMessage="Please enter New password"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label">Confirm Password:</label>
                    <asp:TextBox TextMode="Password" ID="txtConfirmnewpassword" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RegularExpressionValidator3" ControlToValidate="txtConfirmnewpassword" runat="server" ErrorMessage="Please enter Confirm password"></asp:RequiredFieldValidator>
                </div>
            </div>
             <div class="col-lg-6">
            <div class="form-group">
                <asp:Button runat="server" CssClass="btn btn-primary mt-2" ID="btnchangepassword" Text="Submit" OnClick="btnchangepassword_Click" />
            </div>
        </div>
        </div>
        <%--  <label>Old Password</label>
            <asp:TextBox TextMode="Password" ID="txtOldpassword" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RegularExpressionValidator1" ControlToValidate="txtOldpassword" runat="server" ErrorMessage="Please enter Old password"></asp:RequiredFieldValidator>
             <label>New Password</label>
            <asp:TextBox TextMode="Password" ID="txtNewpassword" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RegularExpressionValidator2" ControlToValidate="txtNewpassword" runat="server" ErrorMessage="Please enter New password"></asp:RequiredFieldValidator>
             <label>Confirm Password</label>
            <asp:TextBox TextMode="Password" ID="txtConfirmnewpassword" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RegularExpressionValidator3" ControlToValidate="txtConfirmnewpassword" runat="server" ErrorMessage="Please enter Confirm password"></asp:RequiredFieldValidator>--%>
       
    </div>
</asp:Content>
