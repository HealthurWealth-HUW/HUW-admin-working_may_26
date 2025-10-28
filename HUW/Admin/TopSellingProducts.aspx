<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="TopSellingProducts.aspx.cs" Inherits="Admin_TopSellingProducts" %>
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
    <script language="javascript" type="text/javascript">
function myFunction() {
  var input, filter, table, tr, td, i, txtValue;
  input = document.getElementById("myInput");
  filter = input.value.toUpperCase();
  table = document.getElementById("tbl");
  tr = table.getElementsByTagName("tr");
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
            //alert(50);
            var isPostBackObject = document.getElementById('isPostBack');
            if (isPostBackObject != null) {
                $('#noofprods').val(50);
                $('#noofdayspast').val(90);
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
            else if (Range=="Outofstock")
            {
                $('#ddlProdcutStatus').val(0);
                $('#ddlQunatitysearch').val(1);
                makeSearch();
            }
            else
            {
                GetProductSearchLstfortopselling();
            }
            
            var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
            alert(decodeURIComponent(keyValue) ? decodeURIComponent(keyValue[2]) : null);
            return decodeURIComponent(keyValue) ? decodeURIComponent(keyValue[2]) : null;
            $('#ididid').css({ "class": "sorting_asc" });
            $('#soldqtyid').css({ "class": "sorting" });
//            $('#tbl').DataTable({
//"ordering": [[ 6, "desc" ]]
//});
//$('.dataTables_length').addClass('bs-select');

        });
    </script>

    <style type="text/css">
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
 </style>

    <script type="text/javascript">
        function makeSearch() {
            GetProductSearchLstfortopselling();
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
                                        <li id="onehide" style="display:none">
                                            <label class="search_label" id="lblPrdctID">Product ID </label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductId" name="txtProductId" />
                                            </div>
                                        </li>
                                        <li id="twohide" style="display:none">
                                            <label class="search_label" id="lblstoresku">Product Name</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="txtProductName" name="txtProductName" />
                                            </div>
                                        </li>
                                        <li>
                                            <label class="search_label" id="noofprodsheader">No Of Products</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="noofprods" name="txtnoofprods" />
                                            </div>
                                        </li>
                                        <li>
                                            <label class="search_label" id="noofdayspastheader">No of past days</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="noofdayspast" name="txtnoofpastdays" />
                                            </div>
                                        </li>
<%--                                        <li>
                                        </li>--%>
                                        <li id="threehide" style="display:none">
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

                                        <li runat="server" id="fourhide" style="display:none" >
                                            <div class="two_colfields">
                                                <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblByBrand" >Category</label>
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
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                        <li runat="server" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lisupplier" style="display:none">
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
                                        <li>
                                        <label class="search_label" id="searchingwithintable">filter</label>
                                            <div class="form_input fl_left">
                                                <input type="text" id="myInput" onkeyup="myFunction()" title="Type in a name"/>
                                            </div>
                                        </li>

                                        <div class="globalaction_bar text_right">
                                            <span style="color: Red; float: left; visibility: hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                                            <input type="submit" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch"/>
                                        </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>



            <div id="ctl00_ctl00_Main_Main_UpdatePanel1">

                <div id="ProductLst">
                </div>
            </div>
        </div>
    </form>
</asp:Content>


