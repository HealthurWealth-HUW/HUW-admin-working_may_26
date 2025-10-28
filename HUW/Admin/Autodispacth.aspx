<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Autodispacth.aspx.cs" Inherits="Admin_Autodispacth" MasterPageFile="~/Admin/AdminMaster.master" %>

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
    <script>
        //function AddAWB() {
        //    alert();
        //    debugger;
        //    $.ajax({
        //        type: 'GET',
        //        url: '../api/Master/AddAWB',
        //        contentType: "application/json; charset=utf-8",
        //        dataType: 'json',
        //        async: false,
        //        success: function (data) {
        //            alert("Hi");
        //            alert(data.Result);
        //        },
        //    });
        //}
    </script>
    <div class="widget">
        <div class="widget_title ">
            <span class="iconsweet">r</span>
            <h5>Search Orders</h5>
            <input type="hidden" id="lblPtrnsId" />
            <div id="editor"></div>
        </div>
        <div class="widget_body">
            <label id="lblMessage" runat="server"></label>
            <ul class="form_fields_container">
                <li id="ctl00_ctl00_Main_Main_litsmallcontainer">
                    <label>
                        AWB File</label>

                    <div class="form_input">
                        <span class="oneThree">
                            <asp:FileUpload ID="fileAWB" validate="form1" require="This field is required" name="fileAWB" Style="height: 25px;" runat="server" />
                        </span>
                    </div>
                </li>
            </ul>
            <div class="action_bar text_right">
                <%--<input type="button" value="Add" onclick="AddAWB()">--%>
                 <asp:Button ID="btnSubmit" runat="server"  Text="Submit" OnClick="btnSubmit_Click"/>
            </div>
             <div class="action_bar text_right">
                 <%--<asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click"  Text="AWBUploadExcelFormat"/>--%>* 
            </div>
        </div>
    </div>
</asp:Content>

