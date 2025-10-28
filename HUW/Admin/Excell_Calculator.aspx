<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="Excell_Calculator.aspx.cs" Inherits="Admin_Excell_Calculator" %>

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


    <input type="file" id="excelfile" />  
   <input type="button" id="viewfile" value="Export To Table" onclick="ExportToTable()" />  
      <br />  
      <br />  
   <table id="exceltable">  
</table>


    <script src="jquery-1.10.2.min.js" type="text/javascript"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.7.7/xlsx.core.min.js"></script>  
<script src="https://cdnjs.cloudflare.com/ajax/libs/xls/0.7.4-a/xls.core.min.js"></script>
<script>
    function ExportToTable() {  
     var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;  
     /*Checks whether the file is a valid excel file*/  
     if (regex.test($("#excelfile").val().toLowerCase())) {  
         var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/  
         if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {  
             xlsxflag = true;  
         }  
         /*Checks whether the browser supports HTML5*/  
         if (typeof (FileReader) != "undefined") {  
             var reader = new FileReader();  
             reader.onload = function (e) {  
                 var data = e.target.result;  
                 /*Converts the excel data in to object*/  
                 if (xlsxflag) {  
                     var workbook = XLSX.read(data, { type: 'binary' });  
                 }  
                 else {  
                     var workbook = XLS.read(data, { type: 'binary' });  
                 }  
                 /*Gets all the sheetnames of excel in to a variable*/  
                 var sheet_name_list = workbook.SheetNames;  
  
                 var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/  
                 sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/  
                     /*Convert the cell value to Json*/  
                     if (xlsxflag) {  
                         var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);  
                     }  
                     else {  
                         var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);  
                     }  
                     if (exceljson.length > 0 && cnt == 0) {  
                         BindTable(exceljson, '#exceltable');  
                         cnt++;  
                     }  
                 });  
                 $('#exceltable').show();  
             }  
             if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/  
                 reader.readAsArrayBuffer($("#excelfile")[0].files[0]);  
             }  
             else {  
                 reader.readAsBinaryString($("#excelfile")[0].files[0]);  
             }  
         }  
         else {  
             alert("Sorry! Your browser does not support HTML5!");  
         }  
     }  
     else {  
         alert("Please upload a valid Excel file!");  
     }  
    } 
    function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/  
     var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/  
     for (var i = 0; i < jsondata.length; i++) {  
         var row$ = $('<tr/>');  
         for (var colIndex = 0; colIndex < columns.length; colIndex++) {  
             var cellValue = jsondata[i][columns[colIndex]];  
             if (cellValue == null)  
                 cellValue = "";  
             row$.append($('<td/>').html(cellValue));  
         }  
         $(tableid).append(row$);  
     }  
 }  
 function BindTableHeader(jsondata, tableid) {/*Function used to get all column names from JSON and bind the html table header*/  
     var columnSet = [];  
     var headerTr$ = $('<tr/>');  
     for (var i = 0; i < jsondata.length; i++) {  
         var rowHash = jsondata[i];  
         for (var key in rowHash) {  
             if (rowHash.hasOwnProperty(key)) {  
                 if ($.inArray(key, columnSet) == -1) {/*Adding each unique column names to a variable array*/  
                     columnSet.push(key);  
                     headerTr$.append($('<th/>').html(key));  
                 }  
             }  
         }  
     }  
     $(tableid).append(headerTr$);  
     return columnSet;  
    }  


    setTimeout(function () { trythis() }, 12000);


    function Send_Excel_To_Server() {
        if ($('#exceltable').length > 0) {
            var table=$("#exceltable tr:gt(0)").each(function () {
                var this_row = $(this);
                var productId = $.trim(this_row.find('td:eq(0)').html());//td:eq(0) means first td of this row
                var product = $.trim(this_row.find('td:eq(1)').html())
                var Quantity = $.trim(this_row.find('td:eq(2)').html())

            });
            alert(JSON.stringify(table));
        }
    }
    function trythis()
    {
        var x = new Array(1000);

        for (var i = 0; i < x.length; i++)
        {
            x[i] = new Array(1000);
        }
        var oTable = document.getElementById('exceltable');
        var rowLength = oTable.rows.length;  
        for (i = 0; i < rowLength; i++)
        {
            var oCells = oTable.rows.item(i).cells;
            var cellLength = oCells.length;
            for (var j = 0; j < cellLength; j++)
            {
                var cellVal = oCells.item(j).innerHTML;
                x[i][j] = cellVal;
            }
        }
        sendarrayviaajax(x);
    }
    function sendarrayviaajax(x)
    {
        //alert(x[0][0]);

        var obj = convert(x);
        var thetable = JSON.stringify(obj);
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../api/Master/Remiting_Customers_ajax',
            type: 'POST',
            data: {arrr:  thetable},
            dataType: 'json',
            success: function ()
            {

            }
        });
    }
    function convert(data) {
    return Array.isArray(data)
        ? data.reduce( (obj, el, i) => (el && (obj[i] = convert(el)), obj), {} )
        : data;
}

</script>

</asp:Content>
