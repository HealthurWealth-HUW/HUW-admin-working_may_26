<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ProcessReturns.aspx.cs" Inherits="Admin_ProcessReturns" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<script type="text/javascript">
    $(document).ready(function () {
        var trnsId = getParameterByName("transId");
        GetProcessReturnsDetails(trnsId);
    });
</script>
<div id="activity_stats">
    <h1><label id="ctl00_ctl00_Main_Main_cp_pagetitle">Order Returns</label></h1>
  </div>
  
	<div id="PageMessage"></div>
           
     

<div id="divproductReturns"></div>
<div class="widget" id="divsearch">
            <div class="widget_title">
                <span class="iconsweet">r</span>
                <h5 id="ctl00_ctl00_Main_Main_H2"> <label id="ctl00_ctl00_Main_Main_Label2">Select Replacement Product(s)</label> </h5>
                </div>
                <div id="divOrderReplacement"></div>
            <div class="widget_body">

                                <div id="result">
                                </div>
                                <div class="form_input"><input name="ctl00$ctl00$Main$Main$txtReplaceProducts" id="txtReplaceProducts" style="display:none;" type="text"></div>
                                
                            </div>

                       

                            <div>
                                <ul class="form_fields_container">
                                    <li>
                                           <div class="two_colfields">  
                                        <label>SKU </label>
                                       <div class="form_input">
                                            <input  id="txttransId" class="hasDatepicker" type="text" />
                                           </div>
                                               </div>
                                         <div class="two_colfields">  
                                                <input  value="Search" onclick="GetReplacementProduct();" id="txttransId" class="button_small greyishBtn" type="button" />
                                            
                                            </li>
                                    
                                    <li>
                                    <div class="two_colfields">  
                                    <label>Qty </label> <div class="form_input">
                                        <input  id="txtqty" type="text" />
                                       
                                    </div>
                                    </div>
                                    <div class="two_colfields">  
                                    <label>Price</label>
                                     <div class="form_input">
                                        <input  id="txtprice" type="text" />
                                       
                                        </div>
                                        </div>
                                    </li>
                                    
                                </ul>
                                <div class="action_bar text_right">
                               
                                        
                                            <input value="Add" onclick='AddReplacementProduct()' class="button_small greyishBtn" type="button" />
                                     
                                        
                                            <input  value="Update" onclick="return UpdateProduct();" id="btnSave" class="button_small greyishBtn" type="button" />
                                     
                                        <input  value="Cancel" onclick="return Cancel();" id="ctl00_ctl00_Main_Main_btnCancel" class="button_small greyishBtn" type="button">
                                   
                                </div>
                            </div>
                            
                    </div>
 <div id="diverrormessage"></div>
   
</asp:Content>
