<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Employeelist.aspx.cs" Inherits="Admin_Employeelist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
       <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />
    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script type="text/javascript" src="js/SelectedRowstoExcel.js"></script>
    <script src="js/custom.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            GetEmployeelist();
          });
          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     
     <div id="divempGrd"></div>
                
    </asp:Content>