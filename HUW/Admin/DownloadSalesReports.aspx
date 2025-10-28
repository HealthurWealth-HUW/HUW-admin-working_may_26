<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="DownloadSalesReports.aspx.cs" Inherits="Admin_DownloadSalesReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
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
    <script src="/Scripts/js/jquery.dataTables.min.js"></script>
    <link href="/Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/Scripts/js/dataTables.tableTools.js"></script>
    <link href="/Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/jquery.table2excel.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.js"></script>

    <%--  <link href='https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/ui-lightness/jquery-ui.css' rel='stylesheet'>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js" ></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js" ></script>--%>


    <script type="text/javascript">
        $(document).ready(function () {
            $(".hasDatepicker").datepicker();
            $("#ContentPlaceHolder1_txtFromDate").datepicker({
                numberOfMonths: 1
            });
            $("#ContentPlaceHolder1_txtToDate").datepicker({
                numberOfMonths: 1
            });
        });

    </script>
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
    </style>

    <form>
        <div id="divMsg">
        </div>

        <div id="divPrdctSrch">
            <div id="Searchtabs" class="">

                <div class='one_wrap ' id='divSearchResult' style=''>
                    <div id='divSearchgrids' class='widget' style=''>
                        <div class='widget_title'>
                            <span style="font-family: iconSweets;">}</span>
                            <h5>
                                <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Download Sales Report </b></label>
                            </h5>
                        </div>

                        <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                            <div class="search_contentpad">

                                <div class="margin_10" id="margin_10">
                                    <ul class="form_fields_container">
                                        <li>
                                            <label class="search_label" id="lblPrdctID">From Date </label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtFromDate" runat="server" class="hasDatepicker" placeholder="yyyy-mm-dd" name="txtFromDate" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
                                            </div>
                                        </li>
                                        <li>
                                            <label class="search_label" id="lblstoresku">Todate</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtToDate" runat="server" class="hasDatepicker" placeholder="yyyy-mm-dd" name="txtToDate" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>

                                            </div>
                                        </li>
                                    </ul>
                                    <div class="globalaction_bar text_right">
                                        <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                        <asp:Button ID="btnDownload" runat="server" Text="Button" OnClick="btnDownload_Click" />

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

                <div id="ProductLst">
                </div>
            </div>
        </div>
    </form>



</asp:Content>

