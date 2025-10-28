<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="Admin_ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript" src="RichTextEditor/tinymce/jscripts/tiny_mce/tiny_mce.js"></script>
   <link href="RichTextEditor/tinymce/jscripts/tiny_mce/themes/advanced/skins/default/ui.css" rel="stylesheet" type="text/css" />
   <link href="RichTextEditor/tinymce/jscripts/tiny_mce/plugins/inlinepopups/skins/clearlooks2/window.css" rel="stylesheet" type="text/css" />
   <script src="js/custom.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

    <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
 
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
			{ title: 'Red text', inline: 'span', styles: { color: '#ff0000'} },
			{ title: 'Red header', block: 'h1', styles: { color: '#ff0000'} },
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
        $(document).ready(function () {
            $("#divGallery").hide();
            displayCart();
        });

        SubProductCostCaluclations().empty;
        SubProductCostCaluclations();
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

        
    </script>
<script type="text/javascript">
    function DeleteSubProduct(dsp) {
        var row = dsp.parentNode.parentNode;
        var DltSubPrdQty = row.cells[1].getElementById("#txtSubProductQuantity").value;
        var Quantity = document.getElementById("#txtProductQuantity").value;
        Quantity = Quantity - DltSubPrdQty;
        $('#txtProductQuantity').val(Quantity);
    }
</script>

  <%--<form>--%>
  <div runat="server" id="UpdateIdDiv">
 <div id="ctl00_ctl00_Main_Main_dvCheckPageControl">
        <div id="activity_stats">
        <h1>
            <label id="ctl00_ctl00_Main_Main_lblPageTitle">Update Product</label>
        </h1>
    </div>

        <div><%--class="one_wrap fl_left"--%>
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
                                <label>
                                   Super Categories
                                </label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                             <div id="ctl00_ctl00_Main_Main_dvStoreWithLabel" >
                                  <div id="ctl00_ctl00_Main_Main_dvStoreCategory">                         
                                    <span id="Span2" class="oneThree">                                   
                                     <%--<asp:DropDownList ID="ddlSuperCatogeries" Style="height: 25px;" runat="server" AutoPostBack="true"
                                     OnSelectedIndexChanged="ddlSuperCatogeries_SelectedIndexChanged">
                                     </asp:DropDownList>--%>
                                    </span>  </div>
                                
                            </div>                         
                        </li>

                        <li>
                             <label>                     
                                <label>
                                   Categories
                                </label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                             <div>               
                                    <span  class="oneThree">                                   
                                    <%-- <asp:DropDownList ID="ddlCategory" Style="height: 25px; width: 123px;" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                    AutoPostBack="true">--%>                            
                                     </asp:DropDownList>
                                    </span> 
                                </div>                    
                        </li>
                          
                        <li>
                             <label>                     
                                <label>
                                  Sub Categories
                                </label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                             <div>               
                                    <span  class="oneThree">                                   
                                      <%--<asp:DropDownList ID="ddlSubCategory" Style="height: 25px; width: 123px;" runat="server">
                                </asp:DropDownList>--%>
                                    </span> 
                                </div>                    
                        </li>
                       
                       <li>
                             <label>                     
                                <label>
                                  Product Name
                                </label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                             <div>               
                                    <span  class="oneThree">                                   
                                    <asp:TextBox runat="server" ID="txtProductName" placeholder="Here Product Title" style="width: 420px;" />
                                    </span> 
                                </div>                    
                        </li>
                        
                            <li>
                             <label>                     
                                <label>
                                  Product Brand
                                </label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                             <div>               
                                    <span  class="oneThree">                                   
                                    <asp:TextBox runat="server" ID="txtProductBrand" style="width: 150px;" placeholder="Here Product Brand" />
                                    </span> 
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
                                <label id="ctl00_ctl00_Main_Main_lblShortDescription">Short Description :</label>
                                 </li>                                                            
                                <div class="form_input">
                                    <div id="container1">
                                       <textarea id="elm1" name="elm1" rows="15" runat="server" cols="80" style="width: 80%">               
                                     </textarea>
                                    </div>
                                </div>
                                
                           <li >
                            <li class="multilinefield">
                                <label id="ctl00_ctl00_Main_Main_lblLongDescription">Long Description :</label>
                                 </li>                               
                                <div class="form_input">
                                    <div id="container2">
                                        <asp:TextBox runat="server" ID="txtProductDescription" TextMode="MultiLine"  />                                       
                                    </div>
                                </div>
                            </li>

                            <li>
                             <li class="multilinefield">
                                <label id="ctl00_ctl00_Main_Main_lblFeatures">Features :</label>
                                </li>
                                <div class="form_input">
                                    <div id="container3">
                                        <textarea id='ha' name="txtFeatures"  runat="server" clientidmode="Static"></textarea>                                       
                                    </div>
                                </div>
                            </li>
                        </ul>
                    </div>
                
</div>
            </div>
          
              <%--Basic Information--%>
              <div class="widget">
                <div class="widget_title">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label id="ctl00_ctl00_Main_Main_legBasicInfo">Basic Information</label>
                    </h5>
                </div>
                <div class="widget_body">
                    <%--<label id="Label2">Product Cost & Discount Details</label>--%>
                    <ul class="form_fields_container">
                                              
                        <li id="ctl00_ctl00_Main_Main_liMRP">
                            <div id="ctl00_ctl00_Main_Main_divMrpPrice">
                                <div class="two_colfields">
                                    <label id="ctl00_ctl00_Main_Main_lblPrdctMRP">MRP </label>
                                    <div class="form_input">
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtProductCost" Width="120px"
                        onkeyup="CostCaluclations();" />
                                        <span class="cp_formNote">
                                            <span id="ctl00_ctl00_Main_Main_spnCurSymbol1">(In <span class="WebRupee">Rs. </span>)</span></span>                                        
                                        <span id="ctl00_ctl00_Main_Main_revtxtMRP" style="color:Red;display:none;"></span>
                                         <span id="ctl00_ctl00_Main_Main_cvMRP" style="color:Red;display:none;"></span>
                                   
                                        <input name="ctl00$ctl00$Main$Main$ajxrevtxtMRP_ClientState" id="ctl00_ctl00_Main_Main_ajxrevtxtMRP_ClientState" type="hidden">
                                       
                                    </div>
                                </div>                                
                            </div>
                        </li>
                        
                        <li id="">
                        <div class="two_colfields">
                                <label id="ctl00_ctl00_Main_Main_lblDiscount">Discount</label>
                                <div class="form_input">
                                     <asp:TextBox runat="server" ClientIDMode="Static" ID="txtProductDiscount" Width="120px"
                        onkeyup="CostCaluclations();" />
                                    <span id="ctl00_ctl00_Main_Main_cfvtxtRRP" style="color:Red;display:none;"></span>
                                    <span id="ctl00_ctl00_Main_Main_rgvExpValtxtRRP" style="color:Red;display:none;"></span>
                                    <input name="ctl00$ctl00$Main$Main$ajxValCallExttxtRRP_ClientState" id="ctl00_ctl00_Main_Main_ajxValCallExttxtRRP_ClientState" type="hidden">
                                    <input name="ctl00$ctl00$Main$Main$ajxcfvtxtRRP_ClientState" id="ctl00_ctl00_Main_Main_ajxcfvtxtRRP_ClientState" type="hidden">
                                    <span class="cp_formNote">(In % ) </span>
                                </div>
                            </div>
                        <div class="two_colfields">
                                    <label id="ctl00_ctl00_Main_Main_spnWebPrice">Product Cost</label>
                                    <div class="form_input">
                                        <asp:TextBox runat="server" ClientIDMode="Static" ID="txtProductCostAfterDiscount"
                        Width="120px" Enabled="false" />
                                        <span class="cp_formNote">
                                            <span id="ctl00_ctl00_Main_Main_spnCurSymbol2">(In <span class="WebRupee">Rs. </span>)</span></span>
                                        <span id="ctl00_ctl00_Main_Main_revtxtWebPrice" style="color:Red;display:none;"></span>
                                        <span id="ctl00_ctl00_Main_Main_cfvtxtWebPrice" style="color:Red;display:none;"></span>
                                         
                                        <input name="ctl00$ctl00$Main$Main$ajxrevtxtWebPrice_ClientState" id="ctl00_ctl00_Main_Main_ajxrevtxtWebPrice_ClientState" type="hidden">
                                        <input name="ctl00$ctl00$Main$Main$ajxcfvtxtWebPrice_ClientState" id="ctl00_ctl00_Main_Main_ajxcfvtxtWebPrice_ClientState" type="hidden">
                                       
                                    </div>
                                </div>                                                        
                        </li>

                        <li class="multilinefield">
                         <label id="lblSubPrdcts">Sub Products :</label>
                         <div class="form_input">
                        <%--<asp:UpdatePanel ID="udpSubProducts">
                            <ContentTemplate>
                                <asp:GridView ID="GVSubProducts" runat="server" ClientIDMode="Static" AutoGenerateColumns="False" ShowFooter="True" 
                                    OnRowDataBound="GVSubProducts_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Button ID="btnAddSubProducts"  runat="server" Text="Add New" class="button_small greyishBtn"
                                            OnClick="btnAddSubProducts_Click" /></EmptyDataTemplate>
                                    <Columns>
                                      <asp:TemplateField HeaderText="SPName">
                                            <ItemTemplate>
                                            <asp:HiddenField  runat="server"  ID="hdnSubProductId" Value ='<%#Eval("SubProductId") %>' />
                                                <asp:TextBox runat="server" Style="height: 25px;"  ID="txtSPName" Text='<%#Eval("SPName") %>' ></asp:TextBox>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantity">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Style="height: 25px;"  ID="txtSubProductQuantity" Text='<%#Eval("Quantity") %>'  ClientIDMode="Static" onkeyup="SubProductCostCaluclations();" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Original Cost">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Style="height: 25px;" ID="txtSubProductOriginalCost" ClientIDMode="Static"
                                                    Text='<%#Eval("ProductOriginalCost") %>' onkeyup="SubProductCostCaluclations();"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Discount(%)">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Style="height: 25px;" ID="txtSubProductDiscountPercentage" ClientIDMode="Static"
                                                    Text='<%#Eval("ProductDiscountPercentage") %>' onkeyup="SubProductCostCaluclations();"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Final Cost">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" Style="height: 25px;"  ReadOnly="true" ID="txtSubProductProductCost" Text='<%#Eval("ProductCost") %>' ClientIDMode="Static" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button runat="server"  Text="Delete" ID="btnDeleteSubProducts" class="button_small greyishBtn"
                                                    OnClick="btnDeleteSubProducts_Click" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddSubProducts" runat="server" Text="Add New" class="button_small greyishBtn"
                                                    OnClick="btnAddSubProducts_Click"  />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>          

                                <div>
                                    <label>                     
                                <label>Product Quantity</label>                                 
                                <span class="cp_mandatory_field">*</span>    
                                </label>
                                                                                       
                                    <span  class="">                                   
                                     <asp:TextBox runat="server" ID="txtProductQuantity" ClientIDMode="Static" Width="120px"/>                                                                             
                                    </span> 
                                </div>                                                        
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        </div>
                        </li> 
                                               
                    </ul>
                    <br>
                </div>
            </div>

              <%--Product Settings--%>
             <div class="widget">
             <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Product Settings</label>
                    </h5>
                </div>

             <div class="widget_body">  
                    <ul class="form_fields_container">

                        <li>
                        <div>		
                                        <div>
                                         <label>
                                        <span>                                       
                                        <asp:CheckBox runat="server" ID="chkIsFeatureProduct" />
                                       
                                        </span>
                                        </div>
                                         </label>
                                        <label>Is Feature Product?</label>
                                                                         
	                                    </div>
                                                          
                        </li>

                         <li>
                        <div>		 
                                        <div class="">
                                        <label>
                                        <span>
                                       <asp:CheckBox runat="server" ID="chkBoxHasReviews" />                                   
                                        </span>
                                        </label>
                                        </div>
                                        <label>Can Product Contains Reviews?</label>                                   
	                                    </div>
                       </li>

                        <li>
                        <div>		
                                        <div class="">
                                        <label>
                                        <span>
                                       <asp:CheckBox runat="server" ID="chkBoxCompare" />                                   
                                        </span>
                                        </label>
                                        </div>
                                        <label class="cp_checkboxcaption">Can Product Compare with Other Products?</label>                                   
	                                    </div>
                       </li>
                      
                       <li>
                        <div>		
                                        <div class="">
                                        <label>
                                        <span>
                                       <asp:CheckBox runat="server" ID="chkBoxAdminPerissionsOnReviews" />                            
                                        </span>
                                        </label>
                                        </div>
                                        <label > Need Admin Permissions To show Reviews?</label>                                   
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

             <%--Product Specifications--%>
             <%--<div class="widget">
             <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Product Specifications</label>
                    </h5>
                </div>

             <div class="widget_body">  
                    <ul class="form_fields_container">
                        <li class="multilinefield">
                         <label> Add Product Specifications</label>
                         <div class="form_input">
                        <asp:UpdatePanel ID="udpProductSpecifications" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView ID="GVProductSpecifications" runat="server" AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDataBound="GVProductSpecifications_RowDataBound">
                                    <EmptyDataTemplate>
                                        <asp:Button ID="btnAddProductSpecification"  runat="server" class="button_small greyishBtn"
                                            Text="Add New" OnClick="btnAddProductSpecification_Click" /></EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="ddlProductSpecificationTypes" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:HiddenField runat="server" Value='<%# Bind("ProductSpecificationId") %>' ID="hiddnProductSpecificationId" />
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
                                                <asp:Button runat="server"  Text="Delete" ID="btnDeleteProductSpecification" class="button_small greyishBtn"
                                                    OnClick="btnDeleteProductSpecification_Click" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddProductSpecification"  runat="server" class="button_small greyishBtn"
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
             </div>--%>
             <%--End  Product Specifications--%>


              <%--Product Features--%>
              <%--<div id="Div1" runat="server" visible="false">
             <div class="widget" >
             <div class="widget_title relative_position">
                    <span class="iconsweet">r</span>
                    <h5>
                        <label>Product Features</label>
                    </h5>
                </div>

             <div class="widget_body">  
                    <ul class="form_fields_container">
                        <li class="multilinefield">
                         <label> Add Product Features</label>
                         <div class="form_input">
                        <asp:UpdatePanel ID="udpproductFeatures" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView ID="GVProductFeatures" runat="server" ShowFooter="True" DataSourceID="DBSFillFeaturesSubCategoriesData"
                                    AutoGenerateColumns="False" DataKeyNames="FeaturesCategoryId">
                                    <EmptyDataTemplate>
                                        <asp:Button ID="btnAddProductFeatures"  runat="server" Text="Add New" class="button_small greyishBtn"
                                            OnClick="btnAddProductFeatures_Click" /></EmptyDataTemplate>
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
             </div>--%>
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
                    <li>
                    <div>		 
                                        <div>
                                        <label>
                                        <span>
                                    <%--   <asp:CheckBox runat="server" ID="chkDeletePhotosAndUpldphtos" autopostback="true"
                                        oncheckedchanged="chkDeletePhotosAndUpldphtos_CheckedChanged" />      --%>                            
                                        </span>
                                        </label>
                                        </div>
                                        <label>Delete existing Photo and  add New Photo.</label>                                   
	                                    </div>
                    </li>
                        <li id="ctl00_ctl00_Main_Main_litsmallcontainer">
                            <label>
                                Product Image</label>
                            <div class="form_input">
                                <span class="oneThree">
                                <asp:FileUpload ID="ProductImage" name="ProductImage" Style="height: 25px;" runat="server" />
                                    <%--<input name="ctl00$ctl00$Main$Main$txtSmallIgmageName" id="ctl00_ctl00_Main_Main_txtSmallIgmageName" disabled="disabled" style="background-color:White;" type="text">
                                    <span id="ctl00_ctl00_Main_Main_spnImg100" class="cp_formNote">150px X 150 px</span></span>
                                <a id="ctl00_ctl00_Main_Main_cmdSmallIgmageName" class="button_small greyishBtn" href="javascript:ImagePicker('ctl00_ctl00_Main_Main_txtSmallIgmageNameclient','ctl00_ctl00_Main_Main_txtSmallIgmageName','Small','150','150','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG','150','150','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG');">Add Image</a>
                                <input name="ctl00$ctl00$Main$Main$txtSmallIgmageNameclient" id="ctl00_ctl00_Main_Main_txtSmallIgmageNameclient" type="hidden">--%>
                            </div>
                        </li>

                        <li>
                        
                        </li>
                          <input type="checkbox" id="checkProductgalary" class="Check"  onclick="CheckboxGallery(this)"/>Is Product Gallery
                          <div id="divGallery">
                          <div>		 
                                        <div>
                                        <label>
                                        <span>
                                     <%--  <asp:CheckBox runat="server" ID="chkDeleteFolderPhtosandUpload" 
                                        autopostback="true" 
                                        oncheckedchanged="chkDeleteFolderPhtosandUpload_CheckedChanged" />    --%>                            
                                        </span>
                                        </label>
                                        </div>
                                        <label>Delete existing  Photos and add New Photos.</label>                                   
	                                    </div>
                        <li>
                            <label>
                             Small and Large Images</label>
                            <div class="form_input">
                                <span class="oneThree">
                                <asp:FileUpload Style="height: 25px;" ID="FileUpload1" runat="server" />
                                   <%-- <input name="ctl00$ctl00$Main$Main$txtLargeImageName" id="ctl00_ctl00_Main_Main_txtLargeImageName" disabled="disabled" style="background-color:White;" type="text">
                                    <span id="ctl00_ctl00_Main_Main_spnImg300" class="cp_formNote">300px X 300 px</span></span>
                                <a id="ctl00_ctl00_Main_Main_CmdLargeImageName" class="button_small greyishBtn" href="javascript:ImagePicker('ctl00_ctl00_Main_Main_txtLargeImageNameclient','ctl00_ctl00_Main_Main_txtLargeImageName','Large','300','300','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG','300','300','.jpg,.JPG,.gif,.GIF,.jpeg,.JPEG');">Add Image</a><br>
                                <input name="ctl00$ctl00$Main$Main$txtLargeImageNameclient" id="ctl00_ctl00_Main_Main_txtLargeImageNameclient" type="hidden">--%>
                            </div>
                        </li>
                        </div>
                    </ul>
                    <br>
                </div>
            </div>
              <%--End Product Images--%>
</div>
   
        <div class="globalaction_bar text_right" style="padding-bottom: 4px;">
      <%--  <asp:Button ID="btnUpdateProduct" class="button_small greyishBtn" Text="Update" runat="server" OnClick = "btnUpdateProduct_Click" />  --%>   
     </div>
   </div>
   </div>
    <%--    </form>--%>

<%--<div class="one_wrap fl_left">
      <div class="widget">
        <div class="widget_title relative_position"><span class="iconsweet">r</span>
          <h5>Current Product</h5>
        </div>
        <div class="widget_body">
          <div class="toppadding_10">
            <div class="product_img fl_left"> <img id="ctl00_ctl00_Main_Main_imgSmall" src="ProductView1_files/5515.jpg" style="height:100px;width:100px;border-width:0px;" border="0"></div>
            <div class="fl_right one_two_wrap_80">
              <div class="one_two_wrap fl_left">
                <ul class="form_fields_container product_details ">
                  <li class="padding_0">
                    <label>Product Title</label>
                    <div class="form_input">
                      <label id="ctl00_ctl00_Main_Main_spnPageTitle">OLYMPUS C770 Compact Digital Camera</label> 
                    </div>
                  </li>
                  <li class="padding_0">
                    <label>Brand</label>
                    <div class="form_input">
                      <label id="ctl00_ctl00_Main_Main_spnBrand">Olympus</label>
                    </div>
                  </li>
                  <li class="padding_0">
                    <label>Category</label>
                    <div class="form_input">
                     <label id="ctl00_ctl00_Main_Main_spnCategory">Digital Cameras</label>
                    </div>
                  </li>
                </ul>
              </div>
              <div class="one_two_wrap fl_right">
                <ul class="form_fields_container product_details ">
                  <li class="padding_0">
                    <label>SKU</label>
                    <div class="form_input">
                      <label id="ctl00_ctl00_Main_Main_spnSKU">SC770</label>
                    </div>
                  </li>
                  <li id="ctl00_ctl00_Main_Main_liInventory" class="padding_0">
                    <label>Inventory</label>
                    <div class="form_input">
                      <label id="ctl00_ctl00_Main_Main_spnInventory">100</label>
                    </div>
                  </li>
                  <li class="padding_0">
                    <label id="ctl00_ctl00_Main_Main_lblMRPText">MRP</label>
                    <div class="form_input">
                      <label><span id="ctl00_ctl00_Main_Main_snpMrp"><span class="WebRupee">Rs. </span> 17,000</span></label>
                    </div>
                  </li>
                  <li class="padding_0">
                    <label id="ctl00_ctl00_Main_Main_lblWebPriceText">Web Price</label>
                    <div class="form_input">
                      <label><span id="ctl00_ctl00_Main_Main_spnWebPrice"><span class="WebRupee">Rs. </span> 16,500</span></label>
                    </div>
                  </li>
                </ul>
              </div>
            </div>
            <div class="clear"></div> 
          </div>
          <input name="ctl00$ctl00$Main$Main$hdnServiceType" id="ctl00_ctl00_Main_Main_hdnServiceType" value="PS" type="hidden">
          <div id="tabs" class="ui-tabs ui-widget ui-widget-content ui-corner-all">
       <ul class="tabs_holder ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">
       <li id="ctl00_ctl00_Main_Main_libasicinfo" class="ui-state-default ui-corner-top ui-tabs-selected ui-state-active"><a href="#basicinfo">Product Information</a></li>
	  <li id="ctl00_ctl00_Main_Main_lilongdesc" class="ui-state-default ui-corner-top"><a href="#longdesc">Description</a></li>
	  <li id="ctl00_ctl00_Main_Main_livarintinfo" class="ui-state-default ui-corner-top"><a href="#varintinfo"><span id="ctl00_ctl00_Main_Main_spnVarInfo">Variant Info</span></a></li>
	  <li id="ctl00_ctl00_Main_Main_liattributeinfo" class="ui-state-default ui-corner-top"><a href="#attributeinfo"><span id="ctl00_ctl00_Main_Main_spnAtrInfo">Attribute info</span></a></li>
	  <li id="ctl00_ctl00_Main_Main_lipaymentandshipping" class="ui-state-default ui-corner-top"><a href="#paymentandshipping"> <span id="ctl00_ctl00_Main_Main_lblNamepay">Payment &amp; Shipping</span></a></li>
       
	  <li id="ctl00_ctl00_Main_Main_liadditiobalinfo" class="ui-state-default ui-corner-top"><a href="#additiobalinfo">Additional info</a></li>
            </ul>
            <div id="basicinfo" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom">
                
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $('#divBasicInfo .form_fields_container').find('li').each(function () {
            if ($(this).find('label').html() == "") {
                $(this).hide();
            }
            else if ($(this).find('span').html() == "") {
                $(this).hide();
            }
            else {
                $(this).show();
            }

        });
    });
</script>
  <div class="widget margintop0" id="divBasicInfo">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5><label id="ctl00_ctl00_Main_Main_uctBasicInfo_lgdBasicInformn">Basic Information</label></h5>
                </div>
                <div class="widget_body">
                  <div class="one_two_wrap fl_left">
                    <ul id="ul-left" class="form_fields_container">
                      <li class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSKUtext">Product SKU</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSKU">SC770</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblInvantorytext">Inventory</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblInventory">100</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liMRP" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblMRPtext">MRP</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblMRP"><span class="WebRupee">Rs. </span> 17,000</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liCommision" class="padding_0" style="display:none">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblCommisionText"></label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblCommision"></span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liWarranty" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblWarrantytext">Warranty</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblWarranty">0</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liProductWeight" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblProductWeighttext">Product Weight</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblWeight">0</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liModelNumber" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblModelNumbertext">Model Number</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblModelNo"> &nbsp;olympus c-770</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liSupprName" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSupplierNametext">Supplier Name</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSupplierId">No Supplier</span></label>
                        </div>
                      </li>
                      <li style="display: none;" class="padding_0">
                      <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSellTknPriceText"></label>
                         <div class="form_input">
                          
                        </div>
                      </li>
                    </ul>
                  </div>
                  <div class="one_two_wrap fl_right">
                    <ul class="form_fields_container ">
                      <li class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblBlkQtytext">Bulk Quantity</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblQuantity">1</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liWebPrice" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblWebPricetext">Web Price</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblWebPrice"><span class="WebRupee">Rs. </span> 16,500</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liLstPrice" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblLstPrice">Cost Price</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblListPrice"><span class="WebRupee">Rs. </span> 0</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liTknPrice" class="padding_0" style="display:none">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblTknPrice">Token Price</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblTokenPrice"></span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liCondition" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblConditiontext">Condition</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblCondition">New</span></label>
                        </div>
                      </li>
                      
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liBarcode" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblBarCodetext">Bar Code</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblBarCode">NA</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liCatalogCode" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblCatalogCodetext">Catalog Code</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblCatalogCode">NA</span></label>
                        </div>
                      </li>
                      <li id="ctl00_ctl00_Main_Main_uctBasicInfo_liSupplierSKU" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSupplierSKUtext">Supplier SKU</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblSupplierSKU">NA</span></label>
                        </div>
                      </li>
                      <li style="display: none;" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblPriceOnReq"></label>
                        <div class="form_input">
                          
                        </div>
                      </li>
                      <li style="display: none;" id="ctl00_ctl00_Main_Main_uctBasicInfo_liActdate" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblActivatedDatetext"></label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblActivatedDate"></span></label>
                        </div>
                      </li>
                      <li style="display: none;" id="ctl00_ctl00_Main_Main_uctBasicInfo_liDeactdate" class="padding_0">
                        <label id="ctl00_ctl00_Main_Main_uctBasicInfo_lblDeActivatedDatetext"></label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblDeActivatedDate"></span></label>
                        </div>
                      </li>
                    </ul>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
              <div id="ctl00_ctl00_Main_Main_uctBasicInfo_fldPrductstockInformation" class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Product Stock Information</h5>
                </div>
                <div class="widget_body">
                  <div class="one_two_wrap fl_left">
                    <ul class="form_fields_container">
                      <li class="padding_0">
                        <label>Reserve Quantity</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblReserverQty">0</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Stock Alert Quantity</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblStockAlertQty">0</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Preorder</label>
                        <div class="form_input"> 
                         </div>
                      </li>
                    </ul>
                  </div>
                  <div class="one_two_wrap fl_right">
                    <ul class="form_fields_container ">
                      <li class="padding_0">
                        <label>Reorder Stock Level</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblReorderStock">0</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Stock Available Date</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctBasicInfo_lblStockAvailableData"></span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Backorder</label>
                        <div class="form_input">  </div>
                      </li>
                    </ul>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
              <div id="ctl00_ctl00_Main_Main_uctBasicInfo_fldProductPromotion" class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Product Promotions</h5>
                </div>
                  
<div class="widget_body">
<div>

</div>
			<div id="ctl00_ctl00_Main_Main_uctBasicInfo_Promo1_divNoOffer" class="content_pad">
				<div class="msgbar msg_Info hide_onC"><span class="iconsweet">*</span><p>Currently there is no promotion applied to this product</p></div>
			</div>
	  </div>
            
   

                
              </div>
	       </div>
           <div id="longdesc" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
                
   
   
              <div class="widget margintop0">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Product Description</h5>
                </div>
                <div class="widget_body">
                  <ul class="form_fields_container">
                    <li class="multilinefield">
                      <label>Short Description</label>
                      <div class="form_input">
                        
                        <div id="ctl00_ctl00_Main_Main_uctLongDesc_divShortDescription" class="product_description" name="divShortDescription"></div>
                      </div>
                    </li>
                    <li class="multilinefield">
                      <label>Long Description</label>
                      <div class="form_input">
                       
                        <div id="ctl00_ctl00_Main_Main_uctLongDesc_divLongDescription" class="product_description" name="divLongDescription"></div>
                      </div>
                    </li>
                  </ul>
                </div>
              </div>
              <div id="ctl00_ctl00_Main_Main_uctLongDesc_dvShowImage" class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Product Images</h5>
                </div>
                <div class="widget_body">
                  <div class="one_two_wrap fl_left">
                    <div class="cp_helphorizontal">
                      <p class="helpcontent">Image displayed on category page</p>
                    </div>
                    <ul class="form_fields_container">
                      <li class="padding_0">
                        <label>Image Name</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctLongDesc_lblSmallImage">5515.jpg</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Size </label>
                        <div class="form_input">
                          <label>100px X 100px</label>
                        </div>
                      </li>
                      <li class="padding_0">
                      <img id="ctl00_ctl00_Main_Main_uctLongDesc_imgSmall" src="ProductView1_files/5515.jpg" style="border-width:0px;" border="0">
                      </li>
                    </ul>
                  </div>
                  <div class="one_two_wrap fl_right">
                    <div class="cp_helphorizontal">
                      <p class="helpcontent">Image displayed on Product Detail page</p>
                    </div>
                    <ul class="form_fields_container">
                      <li class="padding_0">
                        <label>Image Name</label>
                        <div class="form_input">
                          <label><span id="ctl00_ctl00_Main_Main_uctLongDesc_lblLargeImage">5515.jpg</span></label>
                        </div>
                      </li>
                      <li class="padding_0">
                        <label>Size </label>
                        <div class="form_input">
                          <label>300px X 300px</label>
                        </div>
                      </li>
                      <li class="padding_0"><img id="ctl00_ctl00_Main_Main_uctLongDesc_imgLarge" src="ProductView1_files/5515_002.jpg" style="border-width:0px;" border="0"></li> 
                    </ul>
                  </div>
                  <div class="clear"></div>
                </div>
              </div>
              <div class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Product Note</h5>
                </div>
                <div class="widget_body">
                 <span id="ctl00_ctl00_Main_Main_uctLongDesc_lblProductNote"></span>
                  <div id="ctl00_ctl00_Main_Main_uctLongDesc_divNoNotes" class="content_pad">
                    <div class="msgbar msg_Info hide_onC"><span class="iconsweet">*</span>
                      <p>Currently there are no Product notes available</p>
                    </div>
                  </div>
                </div>
              </div>
   
   		 
           </div>
           <div id="varintinfo" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
                

<div class="widget margintop0">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>List of product Variant Properties</h5>
                </div>
                <div class="widget_body">
                  
                 <div id="ctl00_ctl00_Main_Main_uctVariantInfo_divNoVariants" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">*</span><p>Currently there are no variants added to this product.</p></div></div>
                </div>
              </div>

 



    
           </div>
           <div id="attributeinfo" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
              
<div class="widget margintop0">
               <table id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute" style="width:100%;border-collapse:collapse;" border="0" cellspacing="0">
	<tbody><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Dimensions</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_lblname">Width</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_lblPreviousAttributevalue">104</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl00_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl01_lblname">Depth</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl01_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl01_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl01_lblPreviousAttributevalue">68.5</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl01_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl02_lblname">Height</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl02_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl02_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl02_lblPreviousAttributevalue">60</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl02_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl03_lblname">Weight</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl03_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl03_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl03_lblPreviousAttributevalue">280</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl03_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Exposure Control</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_lblname">White Balance</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_lblPreviousAttributevalue">auto</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl04_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl05_lblname">Frames Per Second</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl05_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl05_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl05_lblPreviousAttributevalue">2.1</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl05_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl06_lblname">Aperture Range</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl06_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl06_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl06_lblPreviousAttributevalue">f2.8 / f3.7 (w / t)</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl06_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl07_lblname">Shutter Speed</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl07_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl07_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl07_lblPreviousAttributevalue">16 - 1 / 1000</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl07_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Flash</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_lblname">Flash Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_lblPreviousAttributevalue">Built-In &amp; External</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl08_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl09_lblname">ISO Speeds</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl09_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl09_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl09_lblPreviousAttributevalue">Auto, 100, 200, 400, 64</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl09_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl10_lblname">Flash Functions</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl10_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl10_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl10_lblPreviousAttributevalue">Flash Off, Auto Flash, Fill-In Flash, Red-Eye Reduction Flash, Slow Sync</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl10_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Image Quality</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_lblname">Image Resolutions</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_lblPreviousAttributevalue">640 x 480, 3200 x 2400, 2288 x 1712, 2288 x 1520, 2048 x 1536, 1600 x 1200, 1280 x 960, 1024 x 768</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl11_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Included Features</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_lblname">Self Timer</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_lblPreviousAttributevalue">10</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl12_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl13_lblname">Built-in Speaker</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl13_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl13_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl13_lblPreviousAttributevalue">With Built-In Speaker</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl13_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl14_lblname">Built-in Microphone</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl14_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl14_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl14_lblPreviousAttributevalue">With Built-in Microphone</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl14_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Interfaces</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_lblname">Interface Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_lblPreviousAttributevalue">USB</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl15_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Key Features</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_lblname">Optical Zoom Range</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_lblPreviousAttributevalue">7.1 to 10x<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl16_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl17_lblname">Image Sensor Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl17_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl17_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl17_lblPreviousAttributevalue">CCD</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl17_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl18_lblname">Camera Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl18_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl18_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl18_lblPreviousAttributevalue">Compact<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl18_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl19_lblname">Resolution</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl19_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl19_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl19_lblPreviousAttributevalue">4.2</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl19_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl20_lblname">Optical Zoom</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl20_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl20_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl20_lblPreviousAttributevalue">10</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl20_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl21_lblname">Camera Resolution</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl21_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl21_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl21_lblPreviousAttributevalue">Upto 5 MP<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl21_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl22_lblname">Digital Zoom Range</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl22_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl22_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl22_lblPreviousAttributevalue">4 to 4.9x<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl22_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Lens</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_lblname">Interchangeable Lens</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_lblPreviousAttributevalue">Without Interchangeable Lens</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl23_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl24_lblname">35mm Zoom Lens</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl24_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl24_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl24_lblPreviousAttributevalue">38 to 380</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl24_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl25_lblname">Focus Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl25_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl25_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl25_lblPreviousAttributevalue">Auto Focus and Manual Focus</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl25_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl26_lblname">Digital Zoom</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl26_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl26_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl26_lblPreviousAttributevalue">4</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl26_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl27_lblname">Focal Length</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl27_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl27_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl27_lblPreviousAttributevalue">6.3 to 63</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl27_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl28_lblname">Focus Range</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl28_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl28_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl28_lblPreviousAttributevalue">23.62 in. to Infinity (w) / 78.74 in. to Infinity (t)</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl28_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Miscellaneous</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_lblname">Browse Node1</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_lblPreviousAttributevalue">802863031</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl29_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Power Supply</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_lblname">Battery Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_lblPreviousAttributevalue">Lithium Ion<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl30_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl31_lblname">Power Source</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl31_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl31_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl31_lblPreviousAttributevalue">Battery Powered<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl31_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Storage</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_lblname">Compression Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_lblPreviousAttributevalue">JPEG, TIFF</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl32_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl33_lblname">Compression Modes</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl33_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl33_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl33_lblPreviousAttributevalue">Fine, Super Fine, Uncompressed</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl33_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl34_lblname">Memory Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl34_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl34_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl34_lblPreviousAttributevalue">SD Card</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl34_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl35_lblname">Built-in Memory Size</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl35_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl35_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl35_lblPreviousAttributevalue">10</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl35_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>System Requirements</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_lblname">Operating System</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_lblPreviousAttributevalue">Apple Mac OS 8, Apple Mac OS X, Microsoft Windows 2000, Microsoft Windows 98SE, Microsoft Windows ME</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl36_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Video</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_lblname">Video Speed</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_lblPreviousAttributevalue">30</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl37_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl38_lblname">Max Movie Length</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl38_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl38_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl38_lblPreviousAttributevalue">Without Limit (Depends on the Camera Free Memory Size)</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl38_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl39_lblname">Video Resolutions</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl39_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl39_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl39_lblPreviousAttributevalue">320 x 240 (QVGA), 640 x 480 (VGA), 160 x 120</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl39_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl40_lblname">Video Format</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl40_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl40_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl40_lblPreviousAttributevalue">QuickTime</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl40_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_groupnamepanel">
			
    <div class="widget_title"><span class="iconsweet">r</span><h5>Viewfinder / Display</h5></div>
                
		</div>
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_lblname">LCD Screen Resolution</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_lblPreviousAttributevalue">114,000</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl41_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl42_lblname">Display Type</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl42_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl42_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl42_lblPreviousAttributevalue">LCD<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl42_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl43_lblname">LCD Panel Size</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl43_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl43_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl43_lblPreviousAttributevalue">1.8</span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl43_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr><tr>
		<td>
    
	 <div class="widget_body look-up">
			<ul class="form_fields_container">
				<li><label class="width125"><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl44_lblname">Viewfinder</span></label>
                 <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl44_DrpPanel">
			
                  
                   
		</div>
                  <div id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl44_Txtpanel">
			 
                     
                  
		</div>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl44_lblPreviousAttributevalue">Digital<br></span></label>
                 <label><span id="ctl00_ctl00_Main_Main_uctAtrInfo_dlstSelectAttribute_ctl44_Label1"></span></label>
              </li></ul>
               
               
               </div>
             
                </td>
	</tr>
</tbody></table>
                
              </div>

		
	

       
            </div>
           <div id="paymentandshipping" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
                <div id="ctl00_ctl00_Main_Main_uctPay_Ship_pnlPaymentDetails">
	
  <div class="widget margintop0">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Payment Details</h5>
                </div>
                <div class="widget_body">
                 <div class="lefttoppadding_15">
                 <h6 class="bottommargin6"> Payment Methods Accepted</h6>
                 </div>
                 <ul class="form_fields_container">
                 	<li>
                    	<div class="two_colfields">
                  <label>Online Payment</label>
                  <div class="form_input">
                 	<span id="ctl00_ctl00_Main_Main_uctPay_Ship_spnOnlinePay" class="iconsweet margintop6 display_inlineblock">=</span>
                    </div>
                </div>
                        <div class="two_colfields">
                  <label>Cash on Delivery</label>
                  <div class="form_input">
                 	<span id="ctl00_ctl00_Main_Main_uctPay_Ship_spnCOD" class="iconsweet margintop6 display_inlineblock">X</span>
                    </div>
                </div>
                    </li>
                    <li>
                    	<div class="two_colfields">
                  <label>Cheque/DD</label>
                  <div class="form_input">
                 	<span id="ctl00_ctl00_Main_Main_uctPay_Ship_spnChequeDD" class="iconsweet margintop6 display_inlineblock">=</span>
                    </div>
                </div>
                        <div class="two_colfields">
                  <label>Offline Bank Transfer</label>
                  <div class="form_input">
                 	<span id="ctl00_ctl00_Main_Main_uctPay_Ship_spnBT" class="iconsweet margintop6 display_inlineblock">=</span>
                    </div>
                </div>
                    </li>
                 </ul>
                 
                 
                 
                </div>
              </div>
              
</div>
       <div id="ctl00_ctl00_Main_Main_uctPay_Ship_pnlShipping">
	       
              	<div class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Shipping Details</h5>
                </div>
                <div class="widget_body">
                <div class="one_two_wrap">
                	<div class="lefttoppadding_15">
                 <h6 class="bottommargin6"> Available Shipping Codes</h6>
                 </div>
                 <ul class="form_fields_container">
                 	<li class="padding_0">
                    	
                  <label class="width_auto"><span id="ctl00_ctl00_Main_Main_uctPay_Ship_lblAvailableShipCodes">ExDom</span></label>
                  
                    </li>
                   
                 </ul>
                </div>
                 <div class="one_two_wrap">
                 	<div class="lefttoppadding_15">
                 <h6 class="bottommargin6"> Available Shipping Methods</h6>
                 </div>
                 <span id="ctl00_ctl00_Main_Main_uctPay_Ship_lblAvailShippingMethods"><table class="activity_datatable" border="0" cellpadding="8" cellspacing="0" width="100%"><tbody><tr><th width="50%">Shipping Option</th><th width="50%">Express</th></tr><tr><td>India</td><td><span class="WebRupee">Rs. </span> 0</td></tr></tbody></table></span> 
                   </div>
               <br> 
                
                <div class="clear padding_10">
              
                </div>
                </div>
              </div>
              
</div>
               <div id="ctl00_ctl00_Main_Main_uctPay_Ship_pnlTax">
	
              <div class="widget">
                <div class="widget_title"><span class="iconsweet">r</span>
                  <h5>Tax Details</h5>
                </div>
                <div class="widget_body">
                
                
                 <div class="content_pad"><div id="ctl00_ctl00_Main_Main_uctPay_Ship_divLebelTaxModeMethods" class="msgbar msg_Info hide_onC"><span class="iconsweet">*</span><p>Tax is not Configured in your Account.</p></div></div>
                 <div class="clear padding_10"></div>
                
                </div>
              </div>
                   
</div>


           
  
  
            </div>
           <div style="display: none;" id="productdescription" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
                 
	      </div>
          <div id="additiobalinfo" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom ui-tabs-hide">
	              <div id="ctl00_ctl00_Main_Main_uctAddiInfo_fldofferDesc" class="widget">
	<div class="widget_title"><span class="iconsweet">r</span><h5><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lgdOffrDescrption">Offer Description</label></h5></div>
	  <div class="widget_body">
      
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divNoOffer" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">!</span><p>Currently there is no Offer information available </p></div></div>
	  </div>
	</div>
	<div class="clear"></div>

	<div class="widget">
	<div class="widget_title"><span class="iconsweet">r</span><h5>Search Engine Optimization</h5></div>
	  <div class="widget_body">
      
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divNoSeoInfo" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">!</span><p>Currently there is no Seo information available </p></div></div>
	  </div>
	</div>
	<div class="clear"></div>

	<div id="ctl00_ctl00_Main_Main_uctAddiInfo_fldsrvce" class="widget">
	<div class="widget_title"><span class="iconsweet">r</span><h5><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lgndservice">Fulfillment Services</label></h5></div>
	  <div class="widget_body"> 
	   
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divFulfillmentsrvce_msg" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">!</span><p>Currently there is no Fulfillment service available </p></div></div>
          
	  </div>
	</div>
	<div class="clear"></div>
    
    <div class="clear"></div>
	<div id="ctl00_ctl00_Main_Main_uctAddiInfo_fldDeliveryOptn" class="widget">
		<div class="widget_title"><span class="iconsweet">r</span><h5><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lgdDeliveryOptn">Delivery Options</label></h5></div>
		  <div class="widget_body">

			<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divDeliveryOptn" class="tabs_holder padd_15">
            <div class="one_two_wrap fl_left">
              <div class="widget">
                <div class="widget_body">
                  <ul class="form_fields_container">
					<li><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lblShip" class="width125">Ship</label><label><span id="ctl00_ctl00_Main_Main_uctAddiInfo_spnShip" class="iconsweet">=</span></label></li>
					<li><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lblOffline" class="width125">Offline</label><label><span id="ctl00_ctl00_Main_Main_uctAddiInfo_spnOffline" class="iconsweet">X</span></label></li>
                  </ul>
              </div>
              </div>
            </div>

            <div class="one_two_wrap fl_right">
              <div class="widget">
                <div class="widget_body">
					<ul class="form_fields_container">
					<li><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lblOnline" class="width125">Online</label><label><span id="ctl00_ctl00_Main_Main_uctAddiInfo_spnOnline" class="iconsweet">X</span></label></li>
                    <li><label></label></li>
                    <li style="display:none"><label></label></li>
					</ul>
              </div>
              </div>
            </div>
			<div class="clear"></div>
			</div>

	  </div>
	</div>
    <div class="clear"></div>

    <div id="ctl00_ctl00_Main_Main_uctAddiInfo_fldCustomFilds" class="widget">
	<div class="widget_title"><span class="iconsweet">r</span><h5><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lgdCustomFilds">Custom Fields</label></h5></div>
	  <div class="widget_body">
      
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divCustomFilds_msg" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">!</span><p>Currently there is no Section available</p></div></div>
	  </div>
	</div>
    <div class="clear"></div>
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_fldLocations" class="widget">
	<div class="widget_title"><span class="iconsweet">r</span><h5><label id="ctl00_ctl00_Main_Main_uctAddiInfo_lgdLocations">Locations</label></h5></div>
	  <div class="widget_body">
      <div id="ctl00_ctl00_Main_Main_uctAddiInfo_divLocation">
               <div>

</div> 
             </div>
		<div id="ctl00_ctl00_Main_Main_uctAddiInfo_divLocation_msg" class="content_pad"><div class="msgbar msg_Info hide_onC"><span class="iconsweet">!</span><p>Currently there is no Locations available</p></div></div>
	  </div>
	</div>
	<div class="clear"></div>
	      </div>
          </div>
        </div>
        <div id="ctl00_ctl00_Main_Main_pnlButtons">
	
<div class="action_bar text_right"> 
     <input name="ctl00$ctl00$Main$Main$btnAddNewProduct" value="Add New Product" id="ctl00_ctl00_Main_Main_btnAddNewProduct" class="button_small greyishBtn" type="submit">
     <input name="ctl00$ctl00$Main$Main$btnEditProduct" value="Edit Product" id="ctl00_ctl00_Main_Main_btnEditProduct" class="button_small greyishBtn" type="submit">
     <input name="ctl00$ctl00$Main$Main$btnDelete" value="Delete" id="ctl00_ctl00_Main_Main_btnDelete" class="button_small greyishBtn" type="submit">
     <input name="ctl00$ctl00$Main$Main$btnDeactivate" value="Deactivate" id="ctl00_ctl00_Main_Main_btnDeactivate" class="button_small greyishBtn" type="submit">
     <input name="ctl00$ctl00$Main$Main$btnProductList" value="View Product List" id="ctl00_ctl00_Main_Main_btnProductList" class="button_small greyishBtn" type="submit">           
	 
     
     
      
      
<div class="clear"></div>
</div>

</div>
      </div>
    </div>--%>
</asp:Content>

