<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Remitting_Automated.aspx.cs" Inherits="Admin_Remitting_Automated" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery-1.7.1.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


<asp:FileUpload ID="FileUpload1" runat="server" />

<asp:Button ID="btnUpload" runat="server" Text="Upload"

            OnClick="btnUpload_Click" />

<br />

<asp:Label ID="Label1" runat="server" Text="Has Header ?" />

<asp:RadioButtonList ID="rbHDR" runat="server">

    <asp:ListItem Text = "Yes" Value = "Yes" Selected = "True" >

    </asp:ListItem>

    <asp:ListItem Text = "No" Value = "No"></asp:ListItem>

</asp:RadioButtonList>

<asp:GridView ID="GridView1" runat="server"

OnPageIndexChanging = "PageIndexChanging" AllowPaging = "true">

</asp:GridView>

</asp:Content>

