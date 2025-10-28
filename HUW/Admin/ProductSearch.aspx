<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="ProductSearch.aspx.cs" Inherits="Admin_ProductSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <script src="js/jquery.table2excel.js"></script>
    <script>
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

        $.getJSON(uri,  { Prefix: countryName })
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
    $(document).mouseup(function (e) {
        var container = $(".languages");

        // if the target of the click isn't the container nor a descendant of the container
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            container.hide();
        }
    });
    </script>
    <script language="javascript" type="text/javascript">
        function CheckAllDataViewCheckBoxes(checkVal) {
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i];
                if (elm.type == 'checkbox') {
                    elm.checked = checkVal;
                    if (checkVal == true) {
                        $('#' + elm.id + '').parents('span').addClass('checked');
                    }
                    else {
                        $('#' + elm.id + '').parents('span').removeClass('checked');
                    }
                }
            }
        }

    </script>

    <script type="text/javascript">
        $( document ).ready(function() {
            var Range = GetParameterValues('Range');

            function GetParameterValues(param) {
                var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < url.length; i++) {
                    var urlparam = url[i].split('=');
                    if (urlparam[0] == param) {
                        return urlparam[1];
                    }
                }
            }
            
            if(Range=="Active")
            {
                $('#ddlProdcutStatus').val(0);
                makeSearch();
            }
            else if(Range=="InActive")
            {
                $('#ddlProdcutStatus').val(1);
                makeSearch();
            }
            else if(Range=="TopSelling")
            {
                $('#ddlProdcutStatus').val(2);
                makeSearch();
            }
            else if (Range=="Outofstock")
            {
                $('#ddlProdcutStatus').val(0);
                $('#ddlQunatitysearch').val(1);
                makeSearch();
            }
            else
            {
                GetProductSearchLst();
            }
            
            var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
            alert(decodeURIComponent(keyValue) ? decodeURIComponent(keyValue[2]) : null);
            return decodeURIComponent(keyValue) ? decodeURIComponent(keyValue[2]) : null;

        });
    </script>

    <style type="text/css">
.load {
    position: absolute;
    text-align:center;
    top: 0;
    padding-top: 10%;
    left: 0;
    z-index: 100000;
    font-weight: 500;
    width: 100%;
    height: 100%;
    background: hsla(0,0%,100%,.8);
}
    .overlay {
    background-color:#EFEFEF;
    position: fixed;
    width: 100%;
    height: 100%;
    z-index: 1000;
    top: 0px;
    left: 0px;
    opacity: .5; /* in FireFox */ 
    filter: alpha(opacity=50); /* in IE */
}
    .pleaseWaitText{
        position:absolute;
        top:200px;
        left:50%;
        margin-left:-100px;
        font-size:18px;
        font-weight:800;
        color:red;
    }
    .languages {
    max-height: 250px;
    position: absolute;
    width:90%;
    /*left:60px;*/
    z-index: 10000;
    background: #fff;
    overflow:visible;
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
.languages li{
   margin-left: -40px;
    padding: 5px 0 5px 8px;
    border-bottom: 1px solid #f1f1f1;
    border-left: 1px solid #ddd;
}
.languages li a{
    font-size:14px !important;
}
 </style>

    <script type="text/javascript">
        function makeSearch() {
            GetProductSearchLst();
            return false;
        }
    </script>

    <script type="text/javascript">
        jQuery().ready(function () {
            $('#Searchtabs').tabs({ selected: 0 });
            ShowHideContentPad('1');
            $("#divstoresearch").hide();
            $("#storesearchcontent").html('');
            $("#divSellers").dialog({
                modal: true,
                width: 650,
                autoOpen: false,
                position: 'center',
                draggable: false
            });
        });

        function bindSuppliers() {
            $("#divSellers").dialog("open");
            $("#btnSellerSubmit").unbind("click").click(function (e) {
                e.preventDefault();
                SellerPopup.submit();
                if (SellerPopup.selectedMid != "") {
                    $("[ID$=_txtSupplier]").val(SellerPopup.businessName);
                    $("[ID$=hdnSupplierMid]").val(SellerPopup.selectedMid);
                    $("#divSellers").dialog("close");
                }
            });
            $("#btnSellerCancel").click(function () {
                $("#divSellers").dialog("close");
            });
        }

        function ShowHideContentPad(status) {        
            var isstatus = $("#margin_10").css("display");
            if (status != '0') {
                if (isstatus == 'none') {              
                    $("#margin_10").show();
                    $('#btnshowsearch').attr('class', 'btn_search hidesearch');
                    $("#divstoresearch").hide();
                }
                else
                {              
                    $("#margin_10").hide();
                    $('#btnshowsearch').attr('class', 'btn_search showsearch');
                    if (ProductList.showSearchribbon) {                   
                        $("#divstoresearch").show();
                    }
                }           
            }
        }

        function ShowHideContentPad_market(status) {
            var isstatus1 = $("#margin_101").css("display");

            if (status != '0') {
                if (isstatus1 == 'none') {
                    $("#margin_101").show();
                    $('#btnshowsearch1').attr('class', 'btn_search hidesearch');
                }
                else {
                    $("#margin_101").hide();
                    $('#btnshowsearch1').attr('class', 'btn_search showsearch');                
                }
            }
        }

        function CtrlValidateCatBrandSupp(source, arguments) {
            arguments.IsValid = false;
            var drpCtrlCategory = $('#ctl00_ctl00_Main_Main_SelectProductUserControl_drpCtrlCategory');
            var drpCtrlBrand = $('#ctl00_ctl00_Main_Main_SelectProductUserControl_drpCtrlBrand');
        
            var ddlExchange = $("#ctl00_ctl00_Main_Main_SelectProductUserControl_ddlExchange");
            var ddlExCategory = $("#ctl00_ctl00_Main_Main_SelectProductUserControl_ddlExchangeCategory");
            var hasText = $("[id$=txtCtrlName]").val().length &gt; 0 | $("[id$=txtMinPrice]").val().length &gt; 0 | $("[id$=txtMaxPrice]").val().length &gt; 0;
            var IsSelected = drpCtrlCategory.attr("selectedIndex") != 0 | drpCtrlBrand.attr("selectedIndex") != 0 | $("[id$=ddlLocations]").attr("selectedIndex") != 0 | $("[id$=ddlProductTags]").attr("selectedIndex") != 0 | $("[id$=ddlLocations]").attr("selectedIndex") != 0 | $("[id$=ddlAvailablility]").attr("selectedIndex") != 0;

            try {
                if (hasText | IsSelected) {
                    arguments.IsValid = true;
                }
                else if (ddlExchange.length != 0) {
                    if (ddlExchange.attr("selectedIndex") != 0 | ddlExCategory.attr("selectedIndex") != 0) {
                        arguments.IsValid = true;
                    }
                }
                else {
                    arguments.IsValid = false;
                }
            }
            catch (e) {

            }
        }
        //Allows only numeric values
        function OnlyNumeric(evt) {
            var chCode = evt.keyCode ? evt.keyCode : evt.charCode ? evt.charCode : evt.which;

            if (chCode &gt;= 48 &amp;&amp; chCode &lt;= 57 ||
                     chCode == 46 || chCode == 8 || chCode == 9) {
                         return true;
                     }
        else
                return false;
        }
        //Show hide Exchange Category  drop down
        function showOrHideExCategory() {
            //        alert($("#ddlExchange.ClientId").val());
            if ($("[id$=ddlExchange]").val() == "I") {
                $("#liExCatogory").show();
                $("#Excategoryli").show();
            }
            else {
                $("#liExCatogory").hide();
                $("#Excategoryli").hide();
                $("[id$=ddlExchangeCategory]").attr("selectedIndex", 0);
            }
        }
        function MarketplaceTab(ele) {
            var current = $(ele).text();
            if (current == 'SKUs') {
                $("#btnAll").removeClass('active');
                $("#btnSku").addClass('active');
                $("#divSku").show();
                $("#divAll").hide();
            }
            else {
                $("#btnSku").removeClass('active');
                $("#btnAll").addClass('active');
                $("#divSku").hide();
                $("#divAll").show();
            }
        }
    </script>
<script type="text/javascript">
        $(".load").show();
    </script>
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
                                <label id='ctl00_ctl00_Main_Main_lblProductList'><b>&nbsp;&nbsp;&nbsp;&nbsp;Product Search </b></label>
                            </h5>
                        </div>

                        <div class="tab_content tab-panel ui-tabs-panel ui-widget-content ui-corner-bottom" id="StoreTab">

                            <div class="search_contentpad">

                                <div class="margin_10" id="margin_10">
                                    <ul class="form_fields_container">
                                        <li>
                                            <label class="search_label" id="lblPrdctID">Product ID </label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductId" name="txtProductId" />
                                            </div>
                                        </li>
                                        <li>
                                            <label class="search_label" id="lblstoresku">Product Name</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductName" autocomplete="off" name="txtProductName" />
                                <ul id="selectedValue" class="languages"></ul>

                                            </div>
                                        </li>
                                        <li>
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblByCategory">Super Category</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_drpCtrlCategory">
                                                        <asp:DropDownList ID="ddlSuperCatogeries" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlSuperCatogeries_SelectedIndexChanged" Style="-moz-user-select: none;">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblCtrlProductTags">Brand</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlProductTags">
                                                        <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlBrand" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                        </li>

                                        <li>
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblByBrand">Category</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_drpCtrlBrand">
                                                        <asp:DropDownList ID="ddlCategory" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblAvailability">Product Status</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlAvailablility">
                                                        <asp:DropDownList runat="server" ID="ddlProdcutStatus" ClientIDMode="Static">
                                                            <asp:ListItem Value="-1">Select</asp:ListItem>
                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                            <asp:ListItem Value="1">In Active</asp:ListItem>
                                                            <asp:ListItem Value="2">Top Selling Product</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                        <li id="ctl00_ctl00_Main_Main_SelectProductUserControl_lisupplier">
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblExchangeText">Sub Category</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlExchange">
                                                        <asp:DropDownList ID="ddlSubCategory" runat="server" AutoPostBack="true" ClientIDMode="Static">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblQuantity">Available Quantity</label>
                                                <div class="form_input fl_left">
                                                    <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlQuantity">
                                                        <asp:DropDownList ID="ddlQunatitysearch" runat="server" ClientIDMode="Static">
                                                            <asp:ListItem Value="0">Select</asp:ListItem>
                                                            <asp:ListItem Value="1">0 - Out of Stock</asp:ListItem>
                                                            <asp:ListItem Value="2">1-5</asp:ListItem>
                                                            <asp:ListItem Value="3">5 and above</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>

                                        <div class="globalaction_bar text_right">
                                            <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                            <input type="submit" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch">
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

                <div id="ProductLst" style="position:relative;">
 <%--loading div--%>
                 <div class="load text-center">
                        <img src="/images/ajax_loader.gif" alt="loading icon" width="11%" />
                        <p class="text-center" style="padding-top:10px;">Please wait data is loading...</p>
                    </div>
                <%--loading div ends--%>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

