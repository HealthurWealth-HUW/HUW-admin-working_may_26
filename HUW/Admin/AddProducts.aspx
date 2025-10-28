<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AddProducts.aspx.cs" Inherits="Admin_AddProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- bootstrap -->
    <link rel="stylesheet" href="Orders_files/bootstrap.min.css" />
    <script type="text/javascript" src="RichTextEditor/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/themes/advanced/skins/default/ui.css" rel="stylesheet" type="text/css" />
    <link href="RichTextEditor/tinymce/jscripts/tiny_mce/plugins/inlinepopups/skins/clearlooks2/window.css" rel="stylesheet" type="text/css" />
    <script src="js/custom.js" type="text/javascript"></script>
    <script src="js/jquery.datatables.js" type="text/javascript"></script>
    <script src="js/jquery.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/custom.js" type="text/javascript"></script>
    <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />
    <%--<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>--%>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />


    <link href="../sc/validator.css" rel="stylesheet" type="text/css" />
    <script src="../sc/jquery.min.js" type="text/javascript"></script>
    <script src="../sc/jquery.validator-0.3.6.min.js" type="text/javascript"></script>
    <link href="../Styles/css/stylesheet/stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">  
        $(document).ready(function () {
            $("#bestbeforedate").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#bestbeforedate").datepicker("option", "dateFormat", "yy-mm-dd")
                }
            });
        });
        function Getmonths(d) {
            var months;
            var d1 = new Date(Date.now());
            var d2 = new Date(d);
            months = (d2.getFullYear() - d1.getFullYear()) * 12;
            months -= d1.getMonth();
            months += d2.getMonth();
            //return months <= 0 ? 0 : months;
            //var from = d.split("-");
            ////alert(from[1])
            ////var f = new Date(from[2], from[1], from[0]);
            //var n = from[1];
            $('#ContentPlaceHolder1_Best_Before_Date').val(months <= 0 ? 0 : months);
        }
            
        function ShowPreview(input) {
            if (input.files && input.files[0]) {
                var ImageDir = new FileReader();
                ImageDir.onload = function (e) {
                    $('#ContentPlaceHolder1_impPrev').show();
                    $('#ContentPlaceHolder1_impPrev').attr('src', e.target.result);
                }
                ImageDir.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <script>
        $(document).ready(function () {

            // This will automatically grab the 'title' attribute and replace
            // the regular browser tooltips for all <a> elements with a title attribute!
            $('a[title]').qtip();

        });
    </script>
    <script type="text/javascript">  
        function ShowPreview1(input) {
            if (input.files && input.files[0]) {
                var ImageDir = new FileReader();
                ImageDir.onload = function (e) {
                    $('#ContentPlaceHolder1_impPrev2').show();
                    $('#ContentPlaceHolder1_impPrev2').attr('src', e.target.result);
                }
                ImageDir.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <script type="text/javascript">
        function Check() {
            var chkPassport = document.getElementById("btnCheck");
            if (chkPassport.checked) {
                $('#liMRP').hide();
                $('#lidiscount').hide();
                $('#lisubproducts').show();
            } else {
                $('#liMRP').show();
                $('#lidiscount').show();
                $('#lisubproducts').hide();
            }
        }
    </script>
    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            $("#divGallery").hide();
            $("#divSizeGuide").hide();

        });
    </script>
    <script type="text/javascript">
        function CheckboxGallery(dis) {
            $("#divGallery").show();

            $('input[class=Check][type=checkbox]').click(function () {


                var checked = $(this).is(':checked');
                $('input[class=Check][type=checkbox]').each(function () {
                    this.checked = false;
                    $("#divGallery").hide();
                });
                if (checked) {
                    this.checked = true;
                    $("#divGallery").show();

                }
            });

        }
        function chkSizeGuides(dis) {
            $("#divSizeGuide").show();
            $('#chkSizeGuide').click(function () {
                var checked = $(this).is(':checked');
                $('input[class=Check][type=checkbox]').each(function () {
                    this.checked = false;
                    $("#divSizeGuide").hide();
                });
                if (checked) {
                    this.checked = true;
                    $("#divSizeGuide").show();

                }
            });
        }

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

    <script type="text/javascript">
        // SubProductCostCaluclations().empty;
        SubProductCostCaluclations();
        DiscountCaluclations();
        // CostCaluclations().empty;
        CostCaluclations();
    </script>

    <script type="text/javascript">
        $(function () {

            $('#txtProductName').keyup(function () {
                var yourInput = $(this).val();
                re = /[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi;
                var isSplChar = re.test(yourInput);
                if (isSplChar) {
                    var no_spl_char = yourInput.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
                    $(this).val(no_spl_char);
                }
            });

        });

        function ProductQuantityCalculations() {

            // $("#txtSubProductQuantity").attr("readonly", true);
            //   var v1=document.getElementById("txtSubProductQuantity");
            //    v1.setAttribute("readOnly","true");
            // document.getElementById('txtSubProductQuantity').a
            // $("#txtSubProductQuantity").enableSelection = false;
        }

    </script>
    <style>
        .box-list table tbody td {
            border-top: 1px solid #e6e6e6;
            display: inline-flex;
            margin-right: 20px;
            min-width: 160px;
        }
    </style>

    <%--<form>--%>
    <div id="ctl00_ctl00_Main_Main_dvCheckPageControl" runat="server">
        <div id="activity_stats">
            <h1>
                <label id="ctl00_ctl00_Main_Main_lblPageTitle">Add Product</label>
            </h1>
        </div>
        <asp:Label ID="lblErrorCatch" runat="server"></asp:Label>
        <div>
            <%--class="one_wrap fl_left"--%>
            <%--Product Details--%>
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label id="ctl00_ctl00_Main_Main_legSelProduct">Product Details</label>
                    </h5>
                </div>

                <div class="widget_body">
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                    <%--<asp:UpdatePanel ID="udpCategory" runat="server" UpdateMode="Conditional">--%>
                    <ul class="form_fields_container">

                        <li id="ctl00_ctl00_Main_Main_lblCat">
                                <label>
                                    Super Categories
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div id="ctl00_ctl00_Main_Main_dvStoreWithLabel">
                                <div id="ctl00_ctl00_Main_Main_dvStoreCategory">
                                    <span id="Span2" class="oneThree">
                                        <asp:DropDownList ID="ddlSuperCatogeries" Style="height: 25px;" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSuperCatogeries_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </span>
                                </div>

                            </div>
                        </li>

                        <li>
                                <label>
                                    Categories
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div>
                                <span class="oneThree">
                                    <asp:DropDownList ID="ddlCategory" Style="height: 25px; width: 123px;" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </li>

                        <li>
                                <label>
                                    Sub Categories
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div>
                                <span class="oneThree">
                                    <asp:DropDownList ID="ddlSubCategory" Style="height: 25px; width: 123px;" runat="server">
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </li>

                        <li>
                                <label>
                                    Product Name
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ClientIDMode="Static" ID="txtProductName" onkeyup="SearchAllProducts()" validate="form1" require="This field is required" placeholder="Here Product Title" Style="width: 240px;" />
                                </span>
                                <div id="last_Product_loader">
                                </div>
                            </div>
                            <div id="All_product-list" style="position: absolute; margin-top: 37px; margin-left: 120px; z-index: 999999; background: #fff; width: 400px;">
                            </div>
                        </li>

                        <li>
                                <label>
                                    Product Brand
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="txtProductBrand" onkeyup="SearchAllBrands()" ClientIDMode="Static" validate="form1" require="This field is required" Style="width: 240px;" placeholder="Here Product Brand" />
                                </span>
                                <div id="last_Brand_loader">
                                </div>
                            </div>
                            <div id="All_Brand-list" style="position: absolute; margin-top: 37px; margin-left: 120px; z-index: 999999; background: #fff; width: 400px;">
                            </div>
                        </li>
                    </ul>
                    <%--    </asp:UpdatePanel>--%>
                    <br>
                </div>
            </div>

            <%--Product Description--%>
            <div id="ctl00_ctl00_Main_Main_divPrdDes" class="widget">
                <div class="widget_title">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label id="ctl00_ctl00_Main_Main_spnPrDesc">Product Description</label>
                    </h5>
                </div>
                <div id="ctl00_ctl00_Main_Main_PanDesc">

                    <div class="widget_body">

                        <ul class="form_fields_container" style="margin-left: -20px;">
                            <li class="multilinefield">
                                <label id="ctl00_ctl00_Main_Main_lblShortDescription">Short Description:</label>
                            </li>
                            <div class="form_input">
                                <div id="container1">
                                    <textarea id="elm1" name="elm1" rows="15" runat="server" cols="80" style="width: 80%">               
                                     </textarea>
                                </div>
                            </div>

                            <li>
                                <li class="multilinefield">
                                    <label id="ctl00_ctl00_Main_Main_lblLongDescription">Long Description:</label>
                                </li>
                                <div class="form_input">
                                    <div id="container2">
                                        <asp:TextBox runat="server" ID="txtProductDescription" TextMode="MultiLine" />
                                    </div>
                                </div>
                            </li>

                            <li>
                                <li class="multilinefield">
                                    <label id="ctl00_ctl00_Main_Main_lblFeatures">Features:</label>
                                </li>
                                <div class="form_input">
                                    <div id="container3">
                                        <textarea id='ha' name="txtFeatures" runat="server" clientidmode="Static"></textarea>
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>

                </div>
            </div>

            <%--Price & Availability Information--%>
            <div class="widget">
                <div class="widget_title">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label id="ctl00_ctl00_Main_Main_legBasicInfo">Price & Availability Information</label>
                    </h5>
                </div>
                <div class="widget_body">
                    <%--<label id="Label2">Product Cost & Discount Details</label>--%>
                    <ul class="form_fields_container">

                        <li>
                            <asp:CheckBox ID="btnCheck" runat="server" ClientIDMode="Static" onclick="Check()" />
                            Have SubProducts
                        </li>

                        <li id="liMRP">
                            <div id="divMrpPrice">
                                <div class="two_colfields">
                                    <label id="lblPrdctMRP">MRP </label>
                                    <div class="form_input">
                                        <asp:TextBox runat="server" validate="form1" require="This field is required" placeholder="Product Cost" ClientIDMode="Static" ID="txtProductCost" onkeypress="return isNumber(event)" Width="150px"
                                            onkeyup="CostCaluclations();" />
                                        <span class="cp_formNote">
                                            <span id="spnCurSymbol1">(In <span class="WebRupee">Rs. </span>)</span></span>
                                        <span id="revtxtMRP" style="color: Red; display: none;"></span>
                                        <span id="cvMRP" style="color: Red; display: none;"></span>
                                        <input name="txtMRP_ClientState" id="txtMRP_ClientState" type="hidden">
                                    </div>
                                </div>
                                <div class="two_colfields">
                                <span>
                                    <label>Product Quantity</label>
                                </span>
                                <label class="form_input">
                                    <asp:TextBox runat="server" ID="txtProductQuantity" validate="form1" require="This field is required" ClientIDMode="Static" Width="150px" onkeypress="return isNumber(event)" />
                                    <span class="cp_mandatory_field">*</span>
                                </label>
                            </div>
                            </div>
                        </li>

                        <li id="lidiscount">
                            <div class="two_colfields">
                                <label id="lblDiscount">Discount</label>
                                <div class="form_input">
                                    <asp:TextBox runat="server" placeholder="Product Discount" ClientIDMode="Static" ID="txtProductDiscount" Width="120px" onkeypress="return isNumber(event)"
                                        onkeyup="CostCaluclations();" />
                                    <span id="cfvtxtRRP" style="color: Red; display: none;"></span>
                                    <span id="rgvExpValtxtRRP" style="color: Red; display: none;"></span>
                                    <input name="ValCallExttxtRRP_ClientState" id="ValCallExttxtRRP_ClientState" type="hidden">
                                    <input name="cfvtxtRRP_ClientState" id="cfvtxtRRP_ClientState" type="hidden">
                                    <span class="cp_formNote">(In % ) </span>
                                </div>
                            </div>
                            <div class="two_colfields">
                                <label id="ctl00_ctl00_Main_Main_spnWebPrice">Product Cost</label>
                                <div class="form_input">
                                    <asp:TextBox runat="server" validate="form1" require="This field is required" ClientIDMode="Static" ID="txtProductCostAfterDiscount" onkeypress="return isNumber(event)"
                                        Width="120px" onkeyup="DiscountCaluclations();" />
                                    <span class="cp_formNote">
                                        <span id="spnCurSymbol2">(In <span class="WebRupee">Rs. </span>)</span></span>
                                    <span id="revtxtWebPrice" style="color: Red; display: none;"></span>
                                    <span id="cfvtxtWebPrice" style="color: Red; display: none;"></span>

                                    <input name="revtxtWebPrice_ClientState" id="revtxtWebPrice_ClientState" type="hidden">
                                    <input name="cfvtxtWebPrice_ClientState" id="cfvtxtWebPrice_ClientState" type="hidden">
                                </div>
                            </div>
                            <div class="two_colfields">
                                <label id="">GST</label>
                                <div class="form_input">
                                    <asp:DropDownList ID="DDLgst" Style="height: 25px; width: 123px;" runat="server">
                                    </asp:DropDownList>
                                </div>
                                
                            </div>
                            
                            
                        </li>
                        <li class="multilinefield" id="lisubproducts" style="display: none">
                            <label id="lblSubPrdcts">Sub Products :</label>
                            <div class="form_input">
                                <asp:UpdatePanel ID="udpSubProducts" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="GVSubProducts" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" ShowFooter="True"
                                            OnRowDataBound="GVSubProducts_RowDataBound">
                                            <EmptyDataTemplate>
                                                <asp:Button ID="btnAddSubProducts" runat="server" Text="Add New" class="button_small greyishBtn"
                                                    OnClick="btnAddSubProducts_Click" OnClientClick="ClickAddQuantity(this)" />
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="SPName">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" ID="txtSPName" Text='<%#Eval("SPName") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" onkeypress="return isNumber(event)" ID="txtSubProductQuantity" Text='<%#Eval("Quantity") %>' ClientIDMode="Static" onkeyup="SubProductCostCaluclations();"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Original Cost">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" onkeypress="return isNumber(event)" ID="txtSubProductOriginalCost" ClientIDMode="Static"
                                                            Text='<%#Eval("ProductOriginalCost") %>' onkeyup="SubProductCostCaluclations();"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount(%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" onkeypress="return isNumber(event)" ID="txtSubProductDiscountPercentage" ClientIDMode="Static" Text='<%#Eval("ProductDiscountPercentage") %>' onkeyup="SubProductCostCaluclations();"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Final Cost">
                                                    <ItemTemplate>
                                                        <asp:TextBox ReadOnly="true" runat="server" Style="height: 25px;" ID="txtSubProductProductCost" Text='<%#Eval("ProductCost") %>' ClientIDMode="Static"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" Text="Delete" ID="btnDeleteSubProducts" class="button_small greyishBtn"
                                                            OnClick="btnDeleteSubProducts_Click" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddSubProducts" runat="server" Text="Add New" class="button_small greyishBtn"
                                                            OnClick="btnAddSubProducts_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        </div>                              
                                     
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </li>

                    </ul>
                    <br />
                </div>
            </div>

            <%--Product Specifications--%>

            <div class="widget" style="display: none;">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Product Specifications</label>
                    </h5>
                </div>

                <div class="widget_body">
                    <ul class="form_fields_container">
                        <li class="multilinefield">
                            <label>Add Product Specifications</label>
                            <div class="form_input">
                                <asp:UpdatePanel ID="udpProductSpecifications" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:GridView ID="GVProductSpecifications" runat="server" AutoGenerateColumns="False" Visible="false"
                                            ShowFooter="True" OnRowDataBound="GVProductSpecifications_RowDataBound">
                                            <EmptyDataTemplate>
                                                <asp:Button ID="btnAddProductSpecification" runat="server" class="button_small greyishBtn"
                                                    Text="Add New" OnClick="btnAddProductSpecification_Click" />
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:DropDownList runat="server" ID="ddlProductSpecificationTypes" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" ID="txtProductSpecificationName"
                                                            Text='<%#Eval("ProductSpecificationName") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" Style="height: 25px;" ID="txtProductSpecificationValue"
                                                            Text='<%#Eval("ProductSpecificationNameValues") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" Text="Delete" ID="btnDeleteProductSpecification" class="button_small greyishBtn"
                                                            OnClick="btnDeleteProductSpecification_Click" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddProductSpecification" runat="server" class="button_small greyishBtn"
                                                            Text="Add New" OnClick="btnAddProductSpecification_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <%--End  Product Specifications--%>


            <%--Product Features--%>
            <div id="Div1" runat="server" visible="false">
                <div class="widget">
                    <div class="widget_title relative_position">
                        <span class="iconsweet">r</span>
                        <h5>
                            <label>Product Features</label>
                        </h5>
                    </div>

                    <div class="widget_body">
                        <ul class="form_fields_container">
                            <li class="multilinefield">
                                <label>Add Product Features</label>
                                <div class="form_input">
                                    <asp:UpdatePanel ID="udpproductFeatures" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:GridView ID="GVProductFeatures" runat="server" ShowFooter="True" DataSourceID="DBSFillFeaturesSubCategoriesData"
                                                AutoGenerateColumns="False" DataKeyNames="FeaturesCategoryId">
                                                <EmptyDataTemplate>
                                                    <asp:Button ID="btnAddProductFeatures" runat="server" Text="Add New" class="button_small greyishBtn"
                                                        OnClick="btnAddProductFeatures_Click" />
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="FeaturesCategoryName" SortExpression="FeaturesCategoryName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FeaturesCategoryName") %>'></asp:Label>
                                                            <asp:HiddenField runat="server" Value='<%# Bind("FeaturesCategoryId") %>' ID="hiddnFeatureCatId" />
                                                            <asp:GridView ID="GVFeatureSubCat" runat="server" AutoGenerateColumns="False" DataSourceID="EntityDataSource1">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FeaturesSubCategoryName" HeaderText="FeaturesSubCategoryName"
                                                                        ReadOnly="True" SortExpression="FeaturesSubCategoryName" />
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField runat="server" Value='<%# Bind("FeaturesSubCategoryId") %>' ID="hiddnFeatureSubCatId" />
                                                                            <asp:TextBox ID="txtFeaturesSubCatDesc" runat="server" Text=''></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=db_Zon_HuwEntities"
                                                                DefaultContainerName="db_Zon_HuwEntities" EnableFlattening="False" EntitySetName="FeaturesSubCategories"
                                                                Select="it.[FeaturesSubCategoryName], it.[FeaturesSubCategoryId]" AutoGenerateWhereClause="True"
                                                                Where="" EntityTypeFilter="">
                                                                <WhereParameters>
                                                                    <asp:ControlParameter ControlID="hiddnFeatureCatId" DbType="Int64" DefaultValue="0"
                                                                        Name="FeaturesCategoryId" PropertyName="Value" />
                                                                </WhereParameters>
                                                            </asp:EntityDataSource>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:EntityDataSource ID="DBSFillFeaturesSubCategoriesData" runat="server" ConnectionString="name=db_Zon_HuwEntities"
                                                DefaultContainerName="db_Zon_HuwEntities" EnableFlattening="False" EntitySetName="FeaturesCategories"
                                                EntityTypeFilter="FeaturesCategory" Where="it.[BusinessId]=1">
                                            </asp:EntityDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <%--End  Product Features--%>


            <%--Product Images--%>
            <div id="ctl00_ctl00_Main_Main_fldsetPrdImg" class="widget">
                <div class="widget_title">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label id="ctl00_ctl00_Main_Main_legProImg">Product Images</label>
                    </h5>
                </div>
                <div class="widget_body">
                    <ul class="form_fields_container">
                        <li id="ctl00_ctl00_Main_Main_litsmallcontainer">
                            <label class="col-12">
                                Product Image</label>
                            <div class="col-lg-12">  
                                <div class="row">
                                    <div class="col-lg-6">
                                        <asp:FileUpload ID="ProductImage" validate="form1" require="This field is required" onchange="ShowPreview(this)" name="ProductImage" Style="height: 25px;" runat="server" />
                                        <asp:RegularExpressionValidator ID="RegExValFileUploadFileType" Style="color: Red;" runat="server"
                                            ControlToValidate="ProductImage"
                                            ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed"
                                            ValidationExpression="(.*?)\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$" CssClass="Validator"></asp:RegularExpressionValidator>
                                        <asp:Image ID="impPrev" Style="display: none" runat="server" Height="100px" />
                                    </div>
                                     <div class="col-lg-6">
                                        <asp:FileUpload ID="FileUpload3" validate="form1" require="This field is required" onchange="ShowPreview1(this)" name="ProductImage1" Style="height: 25px;" runat="server" />
                                        <asp:Image ID="impPrev2" Style="display: none" runat="server" Height="100px" />
                                    </div>
                                    <%--<input name="ctl00$ctl00$Main$Main$txtSmallIgmageName" id="ctl00_ctl00_Main_Main_txtSmallIgmageName" disabled="disabled" style="background-color:White;" type="text">
                                    <span id="ctl00_ctl00_Main_Main_spnImg100" class="cp_formNote">150px X 150 px</span></span>
                                <a id="ctl00_ctl00_Main_Main_cmdSmallIgmageName" class="button_small greyishBtn" href="javascript:ImagePicker('ctl00_ctl00_Main_Main_txtSmallIgmageNameclient','ctl00_ctl00_Main_Main_txtSmallIgmageName','Small','150','150','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG','150','150','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG');">Add Image</a>
                                <input name="ctl00$ctl00$Main$Main$txtSmallIgmageNameclient" id="ctl00_ctl00_Main_Main_txtSmallIgmageNameclient" type="hidden">--%></span>
                            </div>
                            </div>
                        </li>
                        <div style="display: none">
                            <input type="checkbox" id="checkProductgalary" class="Check" onclick="CheckboxGallery(this)" />Is Product Gallery
                          <div id="divGallery">
                              <li>
                                  <label>
                                      Small and Large Images</label>
                                  <div class="form_input">
                                      <span class="oneThree">
                                          <asp:FileUpload Style="height: 25px;" ID="FileUpload1" runat="server" />
                                          <span class="cp_formNote">
                                              <span id="Span1">(Upload Zip File </span>)</span></span>
                                      <%-- <input name="ctl00$ctl00$Main$Main$txtLargeImageName" id="ctl00_ctl00_Main_Main_txtLargeImageName" disabled="disabled" style="background-color:White;" type="text">
                                    <span id="ctl00_ctl00_Main_Main_spnImg300" class="cp_formNote">300px X 300 px</span></span>
                                <a id="ctl00_ctl00_Main_Main_CmdLargeImageName" class="button_small greyishBtn" href="javascript:ImagePicker('ctl00_ctl00_Main_Main_txtLargeImageNameclient','ctl00_ctl00_Main_Main_txtLargeImageName','Large','300','300','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG','300','300','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG');">Add Image</a><br>
                                <input name="ctl00$ctl00$Main$Main$txtLargeImageNameclient" id="ctl00_ctl00_Main_Main_txtLargeImageNameclient" type="hidden">--%>
                                  </div>
                              </li>
                          </div>
                            <br />
                            <input type="checkbox" id="chkSizeGuide" class="check" onclick="chkSizeGuides(this)" />Is Product Size Guide 
                        <div id="divSizeGuide">
                            <li>
                                <label>
                                    Product Size Guide Images</label>
                                <div class="form_input">
                                    <span class="oneThree">
                                        <asp:FileUpload Style="height: 25px;" ID="FileUpload2" runat="server" />
                                        <span id="Span3">(Upload Zip File Image size must be 500X300</span>)</span>
                                </div>
                            </li>
                        </div>
                        </div>
                    </ul>
                    <br />
                </div>
            </div>
            <%--End Product Images--%>
            <%--Product Settings--%>
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Product Settings</label>
                    </h5>
                </div>

                <div class="widget_body" style="line-height:30px;">
                    <ul>
                        <li>
                            <div>
                                        <span>
                                            <asp:CheckBox runat="server" ID="CheckBox1" /> Is Prescription Needed?
                                        </span>                                
                            </div>
                        </li>
                        <li>
                            <div>
                                        <span>
                                            <asp:CheckBox runat="server" ID="chkIsFeatureProduct" /> Is Feature Product?
                                            <%--<input type="checkbox" name="ctl00$ctl00$Main$Main$chkCreditCard" id="ctl00_ctl00_Main_Main_chkCreditCard" style="opacity: 0;">--%>
                                        </span>

                            </div>
                            <%--<div id="Div5">                         
                                    <span id="Span1" class="oneThree">        
                                    </span>  </div>--%>                                     
                        </li>

                        <li>
                            <div>
                                        <span>
                                            <asp:CheckBox runat="server" ID="chkBoxHasReviews" /> Can Product Contains Reviews?
                                        </span>
                            </div>
                        </li>

                        <li>
                            <div>
                                    <label>
                                        <span>
                                            <asp:CheckBox runat="server" ID="chkBoxCompare" /><label class="cp_checkboxcaption"> Can Product Compare with Other Products?</label>
                                        </span>
                                    </label>
                            </div>
                        </li>

                        <li>
                            <div>
                                        <span>
                                            <asp:CheckBox runat="server" ID="chkBoxAdminPerissionsOnReviews" /> Need Admin Permissions To show Reviews?
                                        </span>
                            </div>
                        </li>

                    </ul>
                </div>
            </div>

            <%--Related Products--%>
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Related Products</label>
                    </h5>
                </div>

                <div class="widget_body">
                    <ul class="form_fields_container">
                        <li class="multilinefield">
                            <label id="Label2">Choose Related Products</label>
                            <div class="form_input">
                                <input type="text" id="txtRelatedProducts" placeholder="Enter Related Product Name" onkeyup="SearchRelatedProducts()" />
                                <br />
                                <div id="last_msg_loader">
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <div id="product-list">
                                            </div>
                                        </td>
                                        <td>
                                            <div id="DisplayRelatedProductList">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <%--End Related Products--%>
            <%--Free Products--%>
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Free Products</label>
                    </h5>
                </div>

                <div class="widget_body">
                    <ul class="form_fields_container">
                        <li class="multilinefield">
                            <label id="lblFreeProducts">Choose Free Products</label>
                            <div class="form_input">
                                <input type="text" id="txtFreeProducts" placeholder="Enter Free Product Name" onkeyup="SearchFreeProducts()" />
                                <br />

                                <div id="free-last_msg_loader">
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <div id="free-product-list">
                                            </div>
                                        </td>
                                        <td>
                                            <div id="DisplayFreeProductList">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <%--End Free Products--%>
            <!-- cahsback section -->
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Cashback</label>
                    </h5>
                </div>

                <div class="widget_body">
                    <ul class="form_fields_container box-list">
                        <li class="multilinefield">
                            <label id="Labl2">Choose Cashback coupons for this Product</label>
                            <asp:CheckBoxList ID="chkaccess" runat="server" RepeatDirection="Vertical" RepeatColumns="3" RepeatLayout="Table"></asp:CheckBoxList>

                        </li>
                    </ul>
                </div>
            </div>
            <div class="widget">
                <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Additional Info</label>
                    </h5>
                </div>
            <div class="widget_body">
                    <ul class="form_fields_container">
                        <li>
                            <label>
                                <label>
                                    Manufacturer Date
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="Manufacturer_Date" Style="width: 500px;" type="date" placeholder="Here Manufacturer_Date" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="Manufacturer_Date" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    Batch No
                                </label>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="txtBatch" Style="width: 500px;" type="date" placeholder="Here Batch Number" />
                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    Best Before
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="Best_Before_Date"  type="number"  placeholder="Here Best_Before_Date" Width="500px" />
                                    Or
                                    <input style="width: 500px;" type="date"  id="bestbeforedate" onchange="Getmonths(this.value)"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Best_Before_Date" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    Country Of Origin
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="Country_Of_Origin" Rows="5" placeholder="Here Country_Of_Origin" Width="500px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Country_Of_Origin" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    Name & Address of Manufacturer / Importer / Packer
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="Manufacturer" TextMode="MultiLine"  Rows="5" placeholder="Name & Address of Manufacturer / Importer / Packer" Width="400px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="Manufacturer" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    Name & Address of Marketed By
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="Marketerdetails" TextMode="MultiLine" Rows="5" placeholder="Name & Address of Marketed By" Width="400px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="Marketerdetails" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </li>
                        <li>
                            <label>
                                <label>
                                    GTIN
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            </label>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server" ID="GTIN" Rows="5" placeholder="Here GTIN" Width="500px" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="GTIN" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>

                                </span>
                            </div>
                        </li>
                         <li>
                                <label>
                                    HSN Code
                                </label>
                                <span class="cp_mandatory_field">*</span>
                            <div>
                                <span class="oneThree">
                                    <asp:TextBox runat="server"  ID="HSNCode"  placeholder="Here HSN Code" Style="width: 240px;" />
                                </span>
                                <div id="">
                                </div>
                            </div>
                        
                        </li>
                    </ul>
                </div>
                </div>

            <!-- cahsback section ends -->
        </div>
        <label class="form_input">
             <label id="">H2Tag</label>
                                    <asp:TextBox runat="server" ID="tcth2tag" validate="form1" require="This field is required" Width="150px" />
                                    <span class="cp_mandatory_field">*</span>
                                </label>
        <div class="globalaction_bar text_right" style="padding-bottom: 4px;">
            <asp:Label ID="lblError" runat="server" ForeColor="#ff0000"></asp:Label>
            <asp:Button ID="btnSaveProduct" Style="margin-top: 2px;" class="button_small greyishBtn" runat="server" Text="Save" OnClientClick="return validate('form1');" OnClick="btnSaveProduct_Click" />
        </div>
    </div>

    <%--    </form>--%>
</asp:Content>
