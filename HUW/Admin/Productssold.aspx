<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Productssold.aspx.cs" Inherits="Admin_Productssold" MasterPageFile="~/Admin/AdminMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        .overlay {
            background-color: #EFEFEF;
            position: fixed;
            width: 100%;
            height: 100%;
            z-index: 1000;
            top: 0px;
            left: 0px;
            opacity: .5; /* in FireFox */
            filter: alpha(opacity=50); /* in IE */
        }

        .pleaseWaitText {
            position: absolute;
            top: 200px;
            left: 50%;
            margin-left: -100px;
            font-size: 18px;
            font-weight: 800;
            color: red;
        }

        .languages {
            max-height: 250px;
            position: absolute;
            width: 90%;
            /*left:60px;*/
            z-index: 10000;
            background: #fff;
            overflow: auto;
            list-style: none;
            margin-left: 1px;
            /*border: 1px solid #ddd;*/
        }

        .languages2 {
            width: 23%;
            left: 60px;
        }

        .languages li:first-child {
            border-top: 1px solid #f1f1f1;
        }

        .languages li {
            margin-left: -26px;
            border-left: 0 !important;
            padding: 5px 0 5px 8px;
            border-bottom: 1px solid #f1f1f1;
            border-left: 1px solid #ddd;
        }

            .languages li a {
                font-size: 14px !important;
            }

        .form-group {
            margin-bottom: 15px;
        }

        .control-label {
            font-weight: 600;
            padding-bottom: 5px;
        }

        div.selector {
            width: 100%;
        }

            div.selector span {
                width: 100%;
                padding-left: 8px;
            }
            input[type=date]{
             width: 80% !important;
    border: solid 1px #ccc;
    padding: 6px;
    font-family: Arial, Helvetica, sans-serif;
    font-size: 12px;
    background: #f8f8f8;
    border-radius: 3px;
    color: #666;
    box-shadow: inset 0 1px 0 0px #fff;
    -webkit-box-shadow: inset 0 1px 0 0px #fff;
            }
            #ProductLst{
                    padding-top: 48px;
    margin-top: 18px;
            }
            #ProductLst table tr th{
              background: #12549a;
    padding: 10px;
    border-right: 1px solid #fff;
    color: #fff;
            }
    </style>

    <script type="text/javascript">
        function makeSearch() {
            GetProductsoldLst();
            return false;
        }
        var textbox;
        var selectValue;

        $(function () {
            textbox = $("#txtProductName");
            selectValue = $('ul#selectedValue');

            textbox.on("input", function () {
                if (textbox.val().length == 0) {

                    $('.languages').hide();
                }
                if (textbox.val().length > 3) {
                    getAutoComplete(textbox.val());

                    if ($('#txtProductName').val() == "") {
                        $('.languages').hide();
                    }
                    else
                        $('.languages').show();
                    $('.languages').css("max-height", "219px");
                }

            });

        });
        function getAutoComplete(countryName) {
            var uri = "../api/Master/Autocompleteforproductname";

            $.getJSON(uri, { Prefix: countryName })
                .done(function (data) {
                    selectValue.html("");
                    var count = 0;

                    $.each(data.Result, function (key, item) {
                        //$('<option>').text(item).appendTo('#selectedValue');
                        var li = $("<li onclick=\"setText('" + item + "') \"/>").addClass('ui-menu-item').attr('role', 'menuitem')
                            .html("<a href='Javascript:;' >" + item + "</a>")
                            .appendTo(selectValue);

                        count++;
                    });
                });
        }
        function setText(text, onlytext) {
            textbox.val(text);
            getAutoComplete(text);
            $('#Diagnosis').val(onlytext);
            $('.languages').hide();
        }
    </script>
    <script type="text/javascript">
        function GetProductsoldLst() {
            $.ajax({
                url: '../api/Master/GetProductsoldLst?Fromdate=' + $('#txtfromdate').val() + '&Todate=' + $('#txttodate').val() + '&Productid=' + $('#txtProductId').val() + '&Productname=' + $('#txtProductName').val(),
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#ProductLst').html(data.Result);
                }
            });

        }


    </script>
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
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
    <script src="/Scripts/js/jquery.dataTables.min.js"></script>
    <link href="/Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/Scripts/js/dataTables.tableTools.js"></script>
    <link href="/Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery.table2excel.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        //$(document).ready(function () {
        //       //$("#txtFromDate").datepicker({
        //       //    numberOfMonths: 2,
        //       //    onSelect: function (selected) {
        //       //$("#txtFromDate").datepicker()
        //       //    }
        //       //});

        //    $("#txtFromDate").datepicker();
        //       $("#txtToDate").datepicker({
        //           numberOfMonths: 2,
        //           onSelect: function (selected) {
        //       $("#txtToDate").datepicker()
        //               }
        //           });
        //   });
        $(function () {
            $("#txtfromdate").datepicker();
        });
    </script>
    <script>
        function toExcel() {
            $("#table2excel").table2excel({
                exclude: ".noExl",
                name: "Products Sold list"
            });
        }
    //        $(function () {
    //            $("#excel-download").click(function () {
    //                alert();
    //                $("#table2excel").table2excel({
    //                    exclude: ".noExl",
    //                    name: "Products Sold list"
    //                });
    //            });
    //});
    </script>
    <div id="divMsg">
    </div>
    <div id="divPrdctSrch">
        <div id="Searchtabs" class="">

            <div class='one_wrap ' id='divSearchResult' style=''>
                <div id='divSearchgrids' class='widget' style=''>
                    <div class='widget_title'>
                        <span style="font-family: iconSweets;">}</span>
                        <h5>
                            <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Product Search </b></label>
                        </h5>
                    </div>

                    <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                        <div class="search_contentpad">

                            <div class="margin_10 widget_body" id="margin_10">
                                <ul class="form_fields_container">
                                    <li>
                                        <div class="two_colfields">
                                            <label class="search_label" id="lblPrdctID">Product ID </label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductId" name="txtProductId" />
                                            </div>
                                        </div>
                                        <div class="two_colfields">
                                            <label class="search_label" id="lblPrdctname">Product Name</label>

                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductName" name="txtProductname" />
                                                <ul id="selectedValue" class="languages"></ul>

                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="two_colfields">
                                            <label class="search_label" id="">From Date </label>

                                            <div class="form_input fl_left">
                                                <input type="date" id="txtfromdate" name="" class="hasDatepicker" />
                                            </div>
                                        </div>
                                        <div class="two_colfields">
                                            <label class="search_label" id="">To Date </label>
                                            <div class="form_input fl_left">
                                                <input type="date" id="txttodate" name="" class="hasDatepicker" />
                                            </div>
                                        </div>


                                    </li>
                                </ul>


                                <div class="globalaction_bar text_right">
                                    <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                    <input type="button" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch">
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>


    <div id="divPrdctList">
        
        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
            <input type='button' onclick='toExcel()' id="excel-download" style='display: inline-block;margin-left:0px;padding: 8px 6px;float:right;border: none;outline: none;margin:0px 20px 0 0;background: green;color: #fff;border-radius: 2px;' value='Export to Excel'>
            <div id="ProductLst">
                
            </div>
        </div>
    </div>
</asp:Content>
