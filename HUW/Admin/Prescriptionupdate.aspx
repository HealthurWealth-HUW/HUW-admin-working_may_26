<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminMaster.master" CodeFile="Prescriptionupdate.aspx.cs" Inherits="Admin_Prescriptionupdate" %>

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

    <!-- magnific popup gallery -->
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Orders_files/jquery.magnific-popup.js"></script>

    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />
    <!-- Font awesome icons -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />

    <link href="Orders_files/magnific-popup.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <style>
        #All_product-list {
            z-index: 999999;          
            width: 400px;
            top: 60px;
            position: absolute;
            left: 0px;
        }

        #All_product-list ul {
            padding-left: 0;
            border: 1px solid #ddd;
            height: 160px;
            overflow: scroll;
                background: #fff;
        }

        #All_product-list ul li {
            padding: 10px;
            border-bottom: 1px dotted #ddd;
            font-size: 12px;
        }
    </style>
    <script src="js/custom.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var getUrlParameters = getUrlParameter("transid");
            GetProductOverviewforpres(getUrlParameters);
            getdetailsforpres(getUrlParameters);
        });
        function getUrlParameter(transid) {

            var sPageURL = window.location.search.substring(1),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === transid) {
                    return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
                }
            }
        };

    </script>
    <script type="text/javascript">
        tinyMCE.init({
            // General options
            mode: "textareas",
            theme: "advanced",
            plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,autosave,visualblocks",

            // Theme options
            theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
            theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
            theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
            theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft,visualblocks",
            theme_advanced_toolbar_location: "top",
            theme_advanced_toolbar_align: "left",
            theme_advanced_statusbar_location: "bottom",
            theme_advanced_resizing: true,

            // Example content CSS (should be your site CSS)
            content_css: "css/content.css",

            // Drop lists for link/image/media/template dialogs
            template_external_list_url: "lists/template_list.js",
            external_link_list_url: "lists/link_list.js",
            external_image_list_url: "lists/image_list.js",
            media_external_list_url: "lists/media_list.js",

            // Style formats
            style_formats: [
                { title: 'Bold text', inline: 'b' },
                { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
                { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
                { title: 'Example 1', inline: 'span', classes: 'example1' },
                { title: 'Example 2', inline: 'span', classes: 'example2' },
                { title: 'Table styles' },
                { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
            ],

            // Replace values for the template plugin
            //        template_replace_values: {
            //            username: "Some User",
            //            staffid: "991234"
            //        }
        });
    </script>
    <div id="divOrdersOverviewGrd"></div>
    <!-- space -->
    <div class="col-lg-12 clearfix">
        <p>&nbsp;</p>
    </div>
    <!-- space -->
    <!-- prescription image -->
    <div class="widget clearfix mt-2">
        <div class="widget_title">
            <span id="spnBillAddress" class="iconsweet">r</span>
            <h5>Prescription images
            </h5>
        </div>
        <div class="row popup-gallery pl-2" id="getimageshere">

            <%--                 <div class="col-lg-4">
                    <a href="https://images.pexels.com/photos/159211/headache-pain-pills-medication-159211.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500">
                        <img src="https://images.pexels.com/photos/159211/headache-pain-pills-medication-159211.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500" alt="prescription image" class="img-thumbnail" />
                    </a>
                </div>--%>
        </div>

    </div>
    <div class="widget clearfix mt-2">
        <div class="widget_title">
            <span id="something" class="iconsweet">r</span>
            <h5>details
            </h5>
        </div>
        <div class="row popup-gallery pl-2" id="getdetailshere">
        </div>

    </div>

    <!-- add product section -->
    <div class="widget_body clearfix">
        <p>&nbsp;</p>
        <!-- bootstrap form -->
        <div class="form-row">
            <div class="form-group col-md-5">
                <label for="inputEmail4" class="font-weight-bold"><strong>Name:</strong></label>

                <%--<input type="email" class="form-control mt-2" id="inputEmail4" placeholder="Email">--%>
                <%--                <asp:TextBox CssClass="form-control form-control-lg mt-2" runat="server" ClientIDMode="Static" ID="txtProductName" onkeyup="SearchAllProductsforprescription(t'<%= txtProductName.ClientID%>')" validate="form1" require="This field is required" placeholder="Product Title"/>--%>
                <label>
                    <span class="cp_mandatory_field">*</span>
                </label>
                <div>
                    <span class="oneThree" style="position: relative;">
                        <input name="ctl00$ContentPlaceHolder1$txtProductName" class="form-control form-control-lg mt-2" type="text" id="txtProductName" onkeyup="SearchAllProducts()" validate="form1" require="This field is required" placeholder="Product Title" style="width: 240px;">
                    </span>
                    <div id="last_Product_loader">
                    </div>
                </div>
                <div id="All_product-list">
                </div>
            </div>
            <div class="form-group col-md-5">
                <label for="inputPassword4" class="font-weight-bold"><strong>Quantity:</strong></label>
                <%--   <input type="password" class="form-control mt-2" id="inputPassword4" placeholder="Password">--%>
                <asp:TextBox CssClass="form-control form-control-lg mt-2" ClientIDMode="Static" runat="server" ID="txtquantity" require="This field is required" placeholder="Quantity" />
            </div>
            <div class="form-group col-md-2">
                <label class="col-md-12" for="inputPassword4">&nbsp;</label>
                <%--<button type="submit" class="btn btn-primary mt-2">Add Product</button>--%>
                <input type="button" value="Add Product" onclick="addproducts();" class="btn btn-primary mt-2" />
            </div>
        </div>



        <%--        <label>
            <label>
                Product Name
            </label>
            <span class="cp_mandatory_field">*</span>
        </label>
        <div>
            <span class="oneThree">
                <input name="ctl00$ContentPlaceHolder1$txtProductName" type="text" id="txtProductName" onkeyup="SearchAllProducts()" validate="form1" require="This field is required" placeholder="Here Product Title" style="width: 240px;">
            </span>
            <div id="last_Product_loader">
            </div>
        </div>
        <div id="All_product-list" style="position: absolute; margin-top: 37px; margin-left: 120px; z-index: 999999; background: #fff; width: 400px;">
        </div>--%>




        <!-- add product button -->
        <%--<div class="text_right one_colfields">--%>
        <%--<input name="btnSearch" value="Search" onclick="GetSearchorders()" class="button_small greyishBtn" type="button">            --%>
        <%--   <input type="button" value="Add Product" onclick="addproducts();" class="button_small greyishBtn" style="margin-right: 55px;" />--%>
        <div class="clear"></div>
        <!-- grid table -->
        <div id="grdview" style="width: 97%;" class="mt-3"></div>
        <!-- grid table section -->
        <!-- total product cost -->
        <label id="prdtcost"></label>
        <!-- total product cost ends  -->
        <%-- </div>--%>
    </div>
    <!-- add product section ends -->
    <div class="row"></div>
    <!-- promo code section -->
    <div class="widget_body clearfix mb-5">
        <p>&nbsp;</p>
        <div class="form-row">
            <div class="form-group col-md-3">
                <label for="inputEmail4" class="font-weight-bold"><strong>Promocode:</strong></label>
                <input type="text" name="note" id="txtCouponCode" class="form-control mt-2" />
            </div>
            <div class="form-group col-md-9">
                <label for="inputEmail4">&nbsp;</label>
                <input type="button" onclick="AddCouponCode()" class="btn btn-primary mt-4" value="Submit" />&nbsp;&nbsp;
                        <asp:Button Text="Procced to buy" runat="server" ID="btnbuy" CssClass="btn btn-success mt-4 mr-4 float-right" OnClick="btnbuy_Click" />
            </div>
        </div>
    </div>
    <!-- promo code section ends -->
    <label id="txtmessage" hidden=""></label>


    <%--   <asp:TextBox runat="server" ClientIDMode="Static" ID="txtProductName" onkeyup="SearchAllProductsforprescription(t'<%= txtProductName.ClientID%>')" validate="form1" require="This field is required" placeholder="Here Product Title" Style="width: 120px" />--%>


    <%-- <asp:TextBox Style="width: 120px" ClientIDMode="Static" runat="server" ID="txtquantity" require="This field is required" placeholder="Here Quantity" />--%>
    <%-- <label>
            <label>
                Cost:
            </label>
        <asp:TextBox style="width:120px" runat="server" ID="TextBox1" require="This field is required" placeholder="Here Quantity"/>
        </label>--%>

    <%--<div id="addrows"></div>--%>
    <%--<div id="buy">buy</div>--%>


    <%--    <script>
        function addrow() {
            var i = 1;
            var str = '<br /><br /><br /><label><label>Name</label><span class="cp_mandatory_field">*</span></label><asp:TextBox runat="server" style="width:120px" ID="txtProductName1"  onkeyup="SearchAllProducts()" validate="form1" require="This field is required" placeholder="Here Product Title"  />';
            str += '<label><label>Quantity:</label><span class="cp_mandatory_field">*</span></label><asp:TextBox ID="txtquantity1" style="width:120px" runat="server" require="This field is required" placeholder="Here Quantity" /><label><label>Cost:</label><asp:TextBox style="width:120px" runat="server" ID="new" require="This field is required" placeholder="Here Quantity"/></label><label ></label><input type="button" onclick="addrow();" value="add row" />'
            $('#addrows').append(str);
        }
    </script>--%>

    <script>
        $(document).ready(function () { getimages(); });
        function getimages() {
            let searchParams = new URLSearchParams(window.location.search);
            let param = searchParams.get('transid');
            //alert(param);
            $.ajax({
                url: '../api/Master/getimagesforpres?id=' + param,
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    $('#getimageshere').append(data.Result);
                },
            });
        }
        function getdetailsforpres() {
            let searchParams = new URLSearchParams(window.location.search);
            let param = searchParams.get('transid');
            //alert(param);
            $.ajax({
                url: '../api/Master/getdetailsforpres?id=' + param,
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    $('#getdetailshere').append(data.Result);
                },
            });
        }

        function addproducts() {
            var text = $('#txtProductName').val();
            var quan = $('#txtquantity').val();
            $('#grdview').empty();
            $.ajax({
                url: '../api/Master/addproducts?text=' + text + '&quan=' + quan,
                type: 'post',
                dataType: 'json',
                success: function (data) {


                    $('#grdview').append(data.Result);
                    $('#txtProductName').val('');
                    $('#txtquantity').val('');
                },


            });
        }
        function removeproduct(prdctname) {

            $('#grdview').empty();
            $.ajax({
                url: '../api/Master/removeproduct?prdctname=' + prdctname,
                type: 'post',
                dataType: 'json',
                success: function (data) {


                    $('#grdview').append(data.Result);
                    $('#txtProductName').val('');
                    $('#txtquantity').val('');
                },


            });
        }
        function AddCouponCode() {
            var coupon = $('#txtCouponCode').val();
            $.ajax({
                url: '../api/Master/AddCouponCode?CouponCode=' + coupon,
                type: 'POST',
                dataType: 'json',
                success: function (data) {
                    //$('#txtmessage').show();
                    //$('#txtmessage').val(data.Message);
                    alert(data.Message);
                },
                error: function (x, y, z) {

                }
            });
        }

        // popup gallery
        $(document).ready(function () {
            $('.popup-gallery').magnificPopup({
                delegate: 'a',
                type: 'image',
                tLoading: 'Loading image #%curr%...',
                mainClass: 'mfp-img-mobile',
                gallery: {
                    enabled: true,
                    navigateByImgClick: true,
                    preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
                }
            });
        });

    </script>
    <style>
        .textb {
            width: 120px;
        }
    </style>
</asp:Content>
