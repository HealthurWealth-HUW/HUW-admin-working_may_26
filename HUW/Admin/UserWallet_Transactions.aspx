<%@ Page Title="UserWallet_Transactions" Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="UserWallet_Transactions.aspx.cs" Inherits="Admin_UserWallet_Transactions" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.11/jquery-ui.min.js"></script>
        <script src="js/custom.js"></script>

</script>

                <script>
        $(document).ready(function () {

            GetCashbackTransactionRecords();
        });
    </script>
        <script type="text/javascript">

        function disablepaging() {
            //$('#tbl').dataTable({
            //    "bProcessing": true,
            //    "sAutoWidth": false,
            //    "bDestroy": true,
            //    "sPaginationType": "bootstrap", // full_numbers
            //    "iDisplayStart ": 10,
            //    "iDisplayLength": 10,
            //    "bPaginate": false, //hide pagination
            //    "bFilter": false, //hide Search bar
            //    "bInfo": false, // hide showing entries
            //})
            //$(window).scrollTop(0);
        }
    </script>
    <script type="text/javascript">
        var disablecalledbool = false;
        function Datefunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var xfromdate, xtodate, table, tr, td, i, xfromtableValue, xtotablevalue;
            var gg = $("#fromdatepicker").val();
            var ff = $("#todatepicker").val();
            if ($("#fromdatepicker").val() == "") {
                var fromdate = "01/01/1800";
            }
            else {
                var fromdate = new Date(Date.parse($("#fromdatepicker").val()));
            }
            if ($("#todatepicker").val() == "") {
                var todate = "01/01/2200";
            }
            else {
                var todate = new Date(Date.parse($("#todatepicker").val()));
            }
            //filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");
            // Loop through all table rows, and hide those who don't match the search query
            fromdate = (fromdate.getMonth() + 1) + '/' + fromdate.getDate() + '/' + fromdate.getFullYear();
            todate = (todate.getMonth() + 1) + '/' + todate.getDate() + '/' + todate.getFullYear();
            var fromdateobj = new Date(fromdate);
            var todateobj = new Date(todate);
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[2];
                if (td) {
                    var tableValue = new Date(Date.parse(td.textContent || td.innerText));
                    //var totablevalue = new Date(Date.parse(td.textContent || td.innerText));
                    tableValue = (tableValue.getMonth() + 1) + '/' + tableValue.getDate() + '/' + tableValue.getFullYear();
                    //totablevalue = (totablevalue.getMonth() + 1) + '/' + totablevalue.getDate() + '/' + totablevalue.getFullYear();
                    var tablevalueobj = new Date(tableValue);
                    if (fromdateobj < tablevalueobj && todateobj > tablevalueobj) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>
    <script>
        var disablecalledbool = false;
        function orderidfilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("orderidfilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");
            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[1];
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>
    <script>
        var disablecalledbool = false;
        function Messagefilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("Messagefilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[9];
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>
    <script>
        var disablecalledbool = false;
        function prodnamefilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("productnamefilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[4];
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>
    <script>
        var disablecalledbool = false;
        function PGTXNIdfilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("PGTXNIdfilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[8];
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>

    <script>
        $(function () {
            $("#fromdatepicker").datepicker();
        });
    </script>
    <script>
        $(function () {
            $("#todatepicker").datepicker();
        });
    </script>

</asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

            <div id="filters" class="widget_body clearfix" style="padding-top: 25px; padding-bottom: 30px;padding-left:20px;">
        <div class="form-row">
            <div id="realtimefiltersdiv" class="box content">
                <div class="col-lg-5">
                    <div class="form-group">
                        <label class="control-label">OrderID:</label>
                        <input type="text" class="form-control" id="orderidfilter" onkeyup="orderidfilterfunc()" />
                    </div>
                </div>
                <div class="col-lg-1">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                    </div>
                </div>
                <div class="col-lg-5">
                    <div class="form-group">
                        <label class="control-label">PGTXNId:</label>
                        <input type="text" class="form-control" id="PGTXNIdfilter" onkeyup="PGTXNIdfilterfunc()" />
                    </div>
                </div>
                <div class="col-lg-1">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                    </div>
                </div>
                <div class="col-lg-5">
                    <div class="form-group">
                        <label class="control-label">Message:</label>
                        <input type="text" class="form-control" id="Messagefilter" onkeyup="Messagefilterfunc()" />
                    </div>
                </div>
                <div class="col-lg-1">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                    </div>
                </div>
                </div>
                <div id="differentdiv" class="box content">
                    <div class="col-lg-5">
                        <div class="form-group">
                            <label class="control-label">From Date:</label>
                            <input type="text" class="form-control" id="fromdatepicker" />
                        </div>
                    </div>
                    <div class="col-lg-1">
                        <div class="form-group">
                            <label class="control-label">&nbsp;</label>
                        </div>
                    </div>
                    <div class="col-lg-5">
                        <div class="form-group">
                            <label class="control-label">Todate</label>
                            <input type="text" class="form-control" id="todatepicker" />
                        </div>
                    </div>
                    <div class="col-lg-11 text-right">
                        <div class="form-group">
                            <label class="control-label"></label>
                            <input type="button" class="btn btn-primary mt-2" id="datefilterbutton" value="Search" onclick="Datefunc();" />
                        </div>
                    </div>
                </div>
        </div>
    </div>

                        <div id="tablehere"></div>



    <div id="PrdctList">

        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
            <div id="pager">
            </div>
        </div>
    </div>




        </asp:Content>
