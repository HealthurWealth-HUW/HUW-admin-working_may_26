<%@ Page Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Routinecustomers.aspx.cs" Inherits="Admin_Routinecustomers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <title>Edit Products</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            Routinecustomers();
        });
        //]]>
    </script>
    <script type="text/javascript">

        function disablepaging() {
            $('#tbl').dataTable({
                "bProcessing": true,
                "sAutoWidth": false,
                "bDestroy": true,
                "sPaginationType": "bootstrap", // full_numbers
                "iDisplayStart ": 10,
                "iDisplayLength": 10,
                "bPaginate": false, //hide pagination
                "bFilter": false, //hide Search bar
                "bInfo": false, // hide showing entries
            })
            $(window).scrollTop(0);
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
                td = tr[i].getElementsByTagName("td")[6];
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
    <script type="text/javascript">
        function Checkboxajaxcall(userid, productid, id) {
            var checkstatus;
            if ($("#" + id).prop("checked") == true) {
                checkstatus = true;
            }
            else if ($("#" + id).prop("checked") == false) {
                checkstatus = false;
            }
            $.ajax({
                datatype: 'json',
                url: '../api/Master/Routinecustomerscheckboxes?userid=' + userid + '&productid=' + productid + '&checkstatus=' + checkstatus,
                type: 'POST',
                dataType: 'json',
                success: function (data) {

                }
            });
        }
    </script>
    <script>
        var disablecalledbool = false;
        function namefilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("namefilter");
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
        function numfilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("mobilenumfilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[3];
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
        function Emailfilterfunc() {
            // Declare variables 
            if (disablecalledbool == false) {
                disablepaging();
                disablecalledbool = true;
            }
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("Emailfilter");
            filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");

            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[2];
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
    <%--    <script type="text/javascript">
        function Datefunc()
        {
            // Declare variables 
            var fromdate,todate,table, tr, td, i, fromtableValue,totablevalue;
            fromdate = new date(document.getElementById("fromdatepicker"));
            todate = new date(document.getElementById("todatepicker"));
            //filter = input.value.toUpperCase();
            table = document.getElementById("tbl");
            tr = table.getElementsByTagName("tr");
            // Loop through all table rows, and hide those who don't match the search query
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[5];
                if (td) {
                    fromtableValue = new date(td.textContent || td.innerText);
                    totablevalue = new date(td.textContent || td.innerText);

                    //if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    if (fromdate > fromtableValue && todate < totablevalue) {
                      tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
</script>--%>
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



    <%-- <div>
        <table>
            <tr>
                <td>Product ID :
                </td>
                <td>
                    <input type="text" id="txtProductID" class="form-control" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <input type="button" value="Search" onclick="GetProductID();" /></td>
            </tr>
        </table>
    </div>--%>
    <div class="widget">
        <div class='widget_title'>
            <h5>
                <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Routine Customers</b></label>
            </h5>
        </div>
    </div>
    <div id="filters" class="widget_body clearfix" style="padding-top: 25px; padding-bottom: 30px;padding-left:20px;">
        <div class="form-row">
            <div class="col-lg-5">
                <div class="form-group">
                    <label class="control-label">Name:</label>
                    <input type="text" class="form-control" id="namefilter" onkeyup="namefilterfunc()" />
                </div>
            </div>
             <div class="col-lg-1">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                </div>
            </div>
            <div class="col-lg-5">
                <div class="form-group">
                    <label class="control-label">Email ID:</label>
                    <input type="text" class="form-control" id="Emailfilter" onkeyup="Emailfilterfunc()" />
                </div>
            </div>
             <div class="col-lg-1">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                </div>
            </div>
            <div class="col-lg-5">
                <div class="form-group">
                    <label class="control-label">Mobile Number:</label>
                    <input type="text" class="form-control" id="mobilenumfilter" onkeyup="numfilterfunc()" />
                </div>
            </div>
             <div class="col-lg-1">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                </div>
            </div>
            <div class="col-lg-5">
                <div class="form-group">
                    <label class="control-label">Product Name:</label>
                    <input type="text" class="form-control" id="productnamefilter" onkeyup="prodnamefilterfunc()" />
                </div>
            </div>
             <div class="col-lg-1">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                </div>
            </div>
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
    <div id="PrdctList">

        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
            <div id="pager">
            </div>
        </div>
    </div>

</asp:Content>
