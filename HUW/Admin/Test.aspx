<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" Inherits="Admin_Test" CodeFile="Test.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>Demonstration how use jqGrid to call WFC service</title>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.5/themes/redmond/jquery-ui.css" />
        <link rel="stylesheet" type="text/css" href="http://www.ok-soft-gmbh.com/jqGrid/jquery.jqGrid-3.8/css/ui.jqgrid.css" />
        <script src="~/Scripts/jquery-1.7.1.js" type="text/javascript"></script>
        <script type="text/javascript" src="http://www.ok-soft-gmbh.com/jqGrid/jquery.jqGrid-3.8/js/i18n/grid.locale-en.js"></script>
        <script type="text/javascript" src="http://www.ok-soft-gmbh.com/jqGrid/jquery.jqGrid-3.8/js/jquery.jqGrid.min.js"></script>
        <script type="text/javascript" src="http://www.ok-soft-gmbh.com/jqGrid/json2.js"></script>
        <script src="~/Scripts/jquery-ui-1.8.20.min.js" type="text/javascript"></script>
        <script src="~/Scripts/Common.js" type="text/javascript"></script>
        <%-- ReSharper disable UnusedParameter --%>
        <script type="text/javascript">

            //<![CDATA[
            jQuery(document).ready(function () {
                $("#list").jqGrid({
                    datatype: 'json',
                    url: '/Customers/GetList',
                    jsonReader: { repeatitems: false },
                    loadui: "block",
                    key: "CustomerID",
                    mtype: 'GET',
                    rowNum: 5,
                    autosize: true,
                    rowList: [5, 10, 20, 30],
                    viewrecords: true,
                    colNames: ['CustomerID', 'CustomerName', 'LastName', 'EmailID', 'Mobile', 'Address', 'Village','ZipCode'],
                    colModel: [
                    { name: 'CustomerID', index: 'CustomerID', width: 100, editoptions: { readonly: true, size: 10} },
                    { name: 'CustomerName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'LastName', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'EmailID', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'Mobile', index: 'Mobile', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'Address', index: 'Address', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'Village', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'ZipCode', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
                    pager: '#pager',
                    sortname: 'TakenOn',
                    sortorder: 'asc',
                    height: "100%",
                    width: "100%",
                    prmNames: { nd: null, search: null }, // we switch of data caching on the server
                    // and not use _search parameter
                    caption: 'Customer Records'
                });

                $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'CustomerID');
                return value;
            }
            }, url: "/Customers/Edit", closeOnEscape: true, reloadAfterSubmit: true,
                closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                    if (response.responseText == "Success") {
                        jQuery("#success").show();
                        jQuery("#success").html("Record successfully updated");
                        jQuery("#success").fadeOut(6000);
                        return [true, response.responseText];
                    }
                    else {
                        return [false, response.responseText];
                    }
                }
            },
            { url: "/Customers/Create", closeOnEscape: true, reloadAfterSubmit: true,
                closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                    if (response.responseText == "Success") {
                        jQuery("#success").show();
                        jQuery("#success").html("Record successfully saved");
                        jQuery("#success").fadeOut(6000);
                        return [true, response.responseText];
                    }
                    else {
                        return [false, response.responseText];
                    }
                }
            },
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'CustomerID');
                    return value;
                }
            }, url: "/Customers/Delete", closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                if (response.responseText == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
            }

            );
            });

            //]]>
        </script>
        <%-- ReSharper restore UnusedParameter --%>
    </head>
    <body>
        <table id="list">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pager">
        </div>
    </body>
    </html>
</asp:Content>
