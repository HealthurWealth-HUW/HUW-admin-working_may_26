<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="SuperCategoryInfo.aspx.cs" Inherits="Admin_SuperCategoryInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"/>
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

 <link href="../sc/validator.css" rel="stylesheet" type="text/css" />
    <script src="../sc/jquery.min.js" type="text/javascript"></script>
    <script src="../sc/jquery.validator-0.3.6.min.js" type="text/javascript"></script>
     <link href="../Styles/css/stylesheet/stylesheet.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<%--<link href="../Validation/stylesheet.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>--%>
  <title>SuperCategories</title>
    <script type="text/javascript">
        //<![CDATA[
        jQuery(document).ready(function () {
            GetSuperCategories();
//            $.blockUI();

//            setTimeout(function () {
//                $.unblockUI({

//                });
//            }, 12000);
            // SuperCategoryGrid();
        });
    //]]>
        </script>
        <script type="text/javascript">
            function ClearContrlsAndAdd() {
                $("#txtSuperCatgryID").val("");
                $("#txtSuperCatgryName").val("");
                $('#ddlIsActive').find('option:first').attr('Select', 'Select');
                $("#SuprCatId").hide();
                $("#dvAddSuprCat").show();
                $("#dvUpdateSuprCat").hide();                       
                vpb_show_login_box();
            }
    </script>

    <script type="text/javascript">
        function PopupAndEdit() {
            $("#SuprCatId").show();
            $("#dvUpdateSuprCat").show();
            $("#dvAddSuprCat").hide();
            vpb_show_login_box();          
        }
    </script>

    <script type="text/javascript">
        function Add() {
            var valdteCheck;
            valdteCheck = validate('customValidatorGroupName');
            if (valdteCheck == true)
            AddSuperCategory();
            return false;
        }
        function Update() {
            var valdteCheck;
            valdteCheck = validate('customValidatorGroupName');
            if (valdteCheck == true)
            UpdateSuperCategory();
            return false;
        }
    </script>
    

  <%--
    <table id="list">
            <tr>
                <td />
            </tr>
        </table>
        <div id="pager">
        </div>--%>

        <div id="content_wrap">
    <div id="cp_placeholder">
  <!--Activity Stats-->
    <div id="activity_stats">
        <h1>
            <label id="ctl00_ctl00_Main_Main_lblPageTitle">Super Categories</label>
        </h1>
    </div>
    <!--Quick Actions-->
    
    <div id="divMsg" class="msgbar msg_Success hide_onC" style="display: none;">
    </div>
    <div>
        <div id="ctl00_ctl00_Main_Main_PageMessage" class="msgbar msg_Success hide_onC" style="display: none;">
            <span class="iconsweet">=</span>
            <label id="ctl00_ctl00_Main_Main_lblSuccess">
            </label>
        </div>
        <div style="display: none;" id="ctl00_ctl00_Main_Main_PanMsg" class="cp_success">
        </div>
    </div>
    <!--One_Three-->
    <div class="one_wrap fl_left">
        <div aria-hidden="true" role="status" id="ctl00_ctl00_Main_Main_updProgress" style="display:none;">
                <div id="ctl00_ctl00_Main_Main_divloadingOrders">
                    <center>
                        <img id="imgLoad" alt="" src="Orders_files/progress-indicator.gif">
                    </center>
                </div>   
</div>
        <div id="ctl00_ctl00_Main_Main_UpdatePanel1">
                <div id="divSuperCategories"></div>    
</div>
        
    </div>
    <div class="clear">
    </div>
        <div class="widget">
   
</div>
   

    </div>
</div>

<div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
    <div style="background-image: url('Orders_files/widget_title_bg.png');  border: medium none; border-radius: 0; height: 37px;">
        <span style="color: #FFFFFF; float: left; font-family: CorbelBold;font-size: 14px;font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024" >Super Categories</span>
      
<div class="widget">
                            <div class="widget_body">
                      <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr id="SuprCatId">
                            <td style=" width:40%; text-align:right; padding-right:10px;"><label id="lblSuperCatgryID">Super Category ID</label></td>
                            <td style="text-align:left; padding-left:10px;"><input  name="txtSuperCatgryID" readonly="readonly" id="txtSuperCatgryID" style="width:186px;" type="text" validate="customValidatorGroupName" require="Please enter your Super category id." /></td>
                         
                         
                             </tr>

                             <tr>
                            <td style=" width:40%; text-align:right; padding-right:10px;"><label id="lblSuperCatgryName">Super Category Name</label></td>
                            <td style="text-align:left; padding-left:10px;">
                            <input  name="txtSuperCatgryName"  id="txtSuperCatgryName" style="width:186px;" type="text" validate="customValidatorGroupName" require="Please enter your super category name." /></td>
                         
                         
                             </tr>
                                 <tr>
                             <td style=" width:40%; text-align:right; padding-right:10px;"> <label id="lblIsActive" style="width: 50px;">IsActive</label></td>
                            <td style="text-align:left;  padding-left:10px;">   
                                <%--<asp:DropDownList ClientIDMode="Static" id="ddlIsActive"  runat="server">
                                    <asp:ListItem value="select">select</asp:ListItem>
                                    <asp:ListItem value="true">true</asp:ListItem>
                                     <asp:ListItem value="false">false</asp:ListItem>
                                </asp:DropDownList> 
                                                --%>

                                                                  <select name='ddlIsActive' class="dropinput" id='ddlIsActive' style="width: 260px;"  validate="customValidatorGroupName" require="Please enter IsActive.">
                                                                        <option value="">Please Select</option>
                                                                    <option  value="true">true</option>
                                                             <option  value="false">false</option>
                                                                </select>
                                    </td>
                                    <td>
                                  
                                    </td>
                            </tr>                           
                            </table>
                                
                                <div class="action_bar text_right"> 
                                <div id="dvAddSuprCat" > 
                                <input value="Add" onclick='return Add()' id="AddSuprCategory" class="button_small greyishBtn" type="button" />                                   
                                    </div>
                                    <div id="dvUpdateSuprCat" >
                                    <input value="Update" onclick='return Update()' id="updateSuprCategory" class="button_small greyishBtn" type="button" />
                                    </div>
                                    <input  value="Close" onclick='vpb_hide_popup_boxes()' id="vh" class="button_small greyishBtn" type="button" />
                                </div>
                            </div>
                        </div>
</div>
 
    

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>

</asp:Content>

