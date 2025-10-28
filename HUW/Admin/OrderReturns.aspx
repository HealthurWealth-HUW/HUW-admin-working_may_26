<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="OrderReturns.aspx.cs" Inherits="Admin_OrderReturns" %>

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
        GetReturnOrders(trnsId);
    });
</script>
<div id="content_wrap">
    <div id="cp_placeholder">
  <!--Activity Stats-->
    <div id="activity_stats">
        <h1>
            <label id="ctl00_ctl00_Main_Main_lblPageTitle">Order Returns</label>
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
        <input type="hidden" id="lblPtrnsId" />
        
                <div id="divOrderReturns"></div>    
</div>
        
    </div>
    <div class="clear">
    </div>
        <div class="widget">
   
</div>
   

    </div>
</div>

  
   
</asp:Content>
