<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SearchProduct.aspx.cs" Inherits="Admin_SearchProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />

    <div id="Searchtabs" class="">
     
       <div class='one_wrap ' id='divSearchResult' style=''>
         <div id='divSearchgrids' class='widget' style=''>
            <div class='widget_title'>
                                            <span  style="font-family: iconSweets;">}</span>
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
                            <input type="text" id="txtProductId" name="txtProductId"/>
                        </div>
                    </li>
                    <li>
                        <label class="search_label" id="lblstoresku">Product Name</label>
                        <div class="form_input fl_left">
                            <input type="text" id="txtProductName" name="txtProductName"/>
                        </div>
                    </li>
                    <li>
                        <div class="two_colfields">
                            <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblByCategory">Super Category</label>
                            <div class="form_input fl_left">
                                <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_drpCtrlCategory">
                                <asp:DropDownList ID="ddlSuperCatogeries"  runat="server" ClientIDMode="Static" AutoPostBack="true" style="-moz-user-select: none;">
                                </asp:DropDownList>                             
                                </div>
                        </div>
                        </div>
                        <div class="two_colfields">
                            <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblAvailability">Product Status</label>
                            <div class="form_input fl_left">
                                <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlAvailablility">                                
                                    <asp:DropDownList runat="server" ID="ddlProdcutStatus" runat="server" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem>Select</asp:ListItem>
                                     <asp:ListItem  Value="0">InActive</asp:ListItem>    
                                    <asp:ListItem Value="1" >Active</asp:ListItem>                                                         
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
                                 <asp:DropDownList ID="ddlCategory"  runat="server" ClientIDMode="Static" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>
                        <div class="two_colfields">
                            <label class="search_label" id="ctl00_ctl00_Main_Main_SelectProductUserControl_lblCtrlProductTags">Brand</label>
                            <div class="form_input fl_left">
                                <div class="selector" id="uniform-ctl00_ctl00_Main_Main_SelectProductUserControl_ddlProductTags">
                                <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlBrand"  AutoPostBack="true">                                        
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
                                <asp:DropDownList ID="ddlSubCategory" Style="height: 25px;" runat="server" AutoPostBack="true" ClientIDMode="Static">
                                </asp:DropDownList>                               
                                </div>
                            </div>
                        </div>
                    </li>                                   

                <div class="globalaction_bar text_right">
                    <span style="color:Red;float:left;visibility:hidden;" class="validation" title="Atleast one value is mandatory" id="ctl00_ctl00_Main_Main_SelectProductUserControl_CustomValidator1">*</span>
                    <input type="submit" class="button_small greyishBtn" id="btnSearch" onclick=" return makeSearch();" value="Search" name="btnSearch">
                </div>              
                </div>
                </div>             
            </div>

        </div>
       </div> 

  </div>

</asp:Content>

