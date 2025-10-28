<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DeliveredOrders.aspx.cs" Inherits="Admin_DeliveredOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
    <link href="Orders_files/main.css" rel="stylesheet" type="text/css">

    <link href="Orders_files/typography.css" rel="stylesheet" type="text/css" />

<link href="Orders_files/reset.css" rel="stylesheet" type="text/css" />
<link href="Orders_files/main.css" rel="stylesheet" type="text/css">

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery.dataTables.min.js"></script>
    <link href="../Styles/css/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/js/dataTables.tableTools.js"></script>
    <link href="../Styles/dataTables.tableTools.css" rel="stylesheet" />
    <script src="js/SelectedRowstoExcel.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        GetDeliveredOrders();
    });

    function ExporttoExcel() {
        $("#divDeliveredOrderssGrd").table2excel({
            name: "Worksheet Name",
            filename: "Order_Process" //do not include extension
        });
    } 
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
        function UpdatePickUP() {
            var ShipmentID = $('#txtShipmentID_edit').val();
            var PickUP = $('#txtPickUPID_edit').val();
            var PickUPID;
            var checkedVals = $('.CheckPickUP:checkbox:checked').map(function () { return this.value; }).get();
            var ID = $("#hdnOrderID_edit").val();
            $.ajax({
                contentType: "application/json; charset=utf-8",
                url: '../api/Master/UpdatePickup?ID=' + ID + '&ShipmentID=' + ShipmentID + '&PickUPID=' + PickUP,
                type: 'post',
                dataType: 'json',
                success: function (data) {
                    if (data.Status == "Success") {
                        GetDeliveredOrders();
                    }
                    else {
                        GetDeliveredOrders();
                    }
                    vpb_hide_popup_boxes();
                }
            });
            //alert($('input[class=Check][type=checkbox]:checked').length);
            //$('input[class=Check][type=checkbox]:checked').each(function () {
            //    var pid = PickUPID;
            //    UpdateWaitingForPickOrder($(this).val(), pid);
            //});
        }
    </script>


<div id="content_wrap">
    <div id="cp_placeholder">
  <!--Activity Stats-->
    <div id="activity_stats">
        <h1>
            <label id="ctl00_ctl00_Main_Main_lblPageTitle">Delivered Orders</label>
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
            <label id="lblSelectedboxes"></label>
                    <input style='display: inline-block;' type='button' onclick='ExporttoExcel()' class='button_small greyishBtn fl_right' value='Export to Excel' />

                <div id="divDeliveredOrderssGrd"></div>
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
        <span style="color: #FFFFFF; float: left; font-family: CorbelBold;font-size: 14px;font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024" >Delivered Orders</span>
      
<div class="widget">
                            <div class="widget_body">
                      <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                            <td style=" width:40%; text-align:right; padding-right:10px;"><label id="Label1"> Order ID</label></td>
                            <td style="text-align:left; padding-left:10px;"><input  name="txtShipment" readonly="readonly" id="txtBlkOrderNo" style="width:186px;" type="text" /></td>
                         
                         
                             </tr>
                                 
                            
                            <tr>
                             <td style=" width:40%; text-align:right; padding-right:10px;">  <label id="Label4"> Delivered Date </label></td>
                            <td style="text-align:left; padding-left:10px;">
                            <input name="Delivered" value="" id="txtDeliveredDate"  style="width:186px;" type="text" />
                            <img src="Orders_files/Calendar.gif" align="absmiddle" border="0">
                            </td>
                            </tr>
                             <tr>
                             <td style=" width:40%; text-align:right; padding-right:10px;"><label id="Label6"> Received By </label></td>
                            <td style="text-align:left; padding-left:10px;">
                          
                             <input name="txtBlkShipperName" id="txtReceivedBy"  type="text" style="width:186px;" />
                            </td>
                            </tr>
                            <tr>
                             <td style=" width:40%; text-align:right; padding-right:10px;"><label id="Label5">  Comments  </label></td>
                            <td style="text-align:left; padding-left:10px;">
                          <div class="form_input">
                        <textarea id="txtComments" cols="20" rows="2"></textarea>
                                        </div>
                            </td>
                            </tr>
                            </table>
                                
                                <div class="action_bar text_right">
                                    
                                    <input value="Ship" onclick='UpdateAuthorizedOrders()' id="Submit1" class="button_small greyishBtn" type="button" />
                                    <input  value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                                </div>
                            </div>
                        </div>
</div>
 

<script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
    <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>

    <div id="vpb_login_pop_up_box_edit" class="tab_content ui-tabs-panel ui-widget-content ui-corner-bottom active">
        <div style="background-image: url('Orders_files/widget_title_bg.png'); border: medium none; border-radius: 0; height: 37px;">
            <span style="color: #FFFFFF; float: left; font-family: CorbelBold; font-size: 14px; font-weight: normal; padding: 13px 0 10px 13px; text-shadow: 0 1px 0 #1D2024">Waiting for Pickup</span>

            <div class="widget">
                <div class="widget_body">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPickUP_edit">
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label1_edit">Shipment ID:</label></td>
                            <td style="text-align: left; padding-left: 10px;">
                                <input name="txtShipmentID" id="txtShipmentID_edit" type="text" style="width: 186px;" /><br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; text-align: right; padding-right: 10px;">
                                <label id="Label2_edit">PickUP ID:</label></td>
                            <td style="text-align: left; padding-left: 10px;">

                                <input name="txtPickUPID" id="txtPickUPID_edit" type="text" style="width: 186px;" />
                                <input type="hidden" id="hdnOrderID_edit" />
                            </td>
                            
                        </tr>

                    </table>

                    <div class="action_bar text_right">

                        <input value="Update" onclick='UpdatePickUP()' id="btnUpadteShipment" class="button_small greyishBtn" type="button" />
                        <input value="Close" onclick='vpb_hide_popup_boxes()' id="Submit2" class="button_small greyishBtn" type="button" />
                    </div>
                </div>
            </div>
        </div>


        <script src="../JavaScript/vpb_script.js" type="text/javascript"></script>
        <link href="../JavaScript/style.css" rel="stylesheet" type="text/css" />
    </div>
    
     <link rel="stylesheet" href="css/jquery-ui.css" />

  <script src="js/jquery-ui.js"></script>
  <script>
      $(function () {
          $("#txtDeliveredDate").datepicker({
              dateFormat: 'dd-mm-yy'
          });
      });
  </script>
</asp:Content>

