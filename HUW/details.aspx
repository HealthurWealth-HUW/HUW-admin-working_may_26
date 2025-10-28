<%@ Page Language="C#" AutoEventWireup="true" CodeFile="details.aspx.cs" Inherits="OnlineShopping.details" %>

<%--<asp:content id="Content1" contentplaceholderid="CustContentPlaceHolder" runat="server">

        <%--<link href="http://admin.healthurwealth.com/JavaScript/style.css" rel="stylesheet" type="text/css" />
        <script src="http://admin.healthurwealth.com/JavaScript/vpb_script.js" type="text/javascript"></script>
        <script src="http://admin.healthurwealth.com/js/tabs.js" type="text/javascript"></script>
        <link href="http://admin.healthurwealth.com/sc/validator.css" rel="stylesheet" type="text/css" />--%>
<script src="http://admin.healthurwealth.com/sc/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">
    jQuery(document).ready(function () {
        var url = window.location.href;
        window.location.href = url.replace("admin", "www");
        //GetProduct();
    });

</script>
<%--<script src="http://admin.healthurwealth.com/sc/jquery.validator-0.3.6.min.js" type="text/javascript"></script>
    <link href="http://admin.healthurwealth.com/Styles/css/stylesheet/stylesheet.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="http://admin.healthurwealth.com/skin/frontend/default/megashop-pink/css/cloud-zoom.css"
        media="all" />
    <script type="text/javascript" src="http://admin.healthurwealth.com/skin/frontend/default/megashop-pink/js/cloud-zoom.1.0.2.min.js"></script>
    <link href="http://admin.healthurwealth.com/sc/cart.css" rel="stylesheet" type="text/css" />
    <script src="http://admin.healthurwealth.com/sc/jquery.hashchange.min.js" type="text/javascript"></script>--%>
<%--    <script type="text/javascript">
        function PincodeAvailCheck() {
            var Pincode = $('#txtPincodecheck').val();
            Pincodecheck(Pincode);
        }
    </script>
    
    <script type="text/javascript">
        function ValidateReviewLogin() {
            var valdteCheck;
            valdteCheck = validate('customReviewGroupName');
            if (valdteCheck == true)
                btnReviewClicked();            
        }
    </script>--%>
<%--<script type="text/javascript">
        function ValidateNotifyingDetails() {
            var valdteCheck;
            valdteCheck = validate('customValidatorGroupName');
            if (valdteCheck == true) {

            }
            AddToNotify();
            //            else
            //                validate('customValidatorGroupName');
        }
        function updateQty() {
            var valdteCheck;
            valdteCheck = validate('customValidatorGroupName');
            if (valdteCheck == true) {
                AddToNotifyforQty();
            }
        }
    </script>

    <script type="text/javascript">
        function ValidateReviewText() {
            var valdteCheck;
            valdteCheck = validate('customReviewLoginGroupName');
            if (valdteCheck == true)
                CheckReviesLogin();            
        }
    </script>--%>
<%--<div class="mrg80" style="padding-top: 10px;"></div>
    <div id='divstack' align="center" style="background-color: pink; font-weight: bold; font-size: 16px; margin: 10px; display: none;"></div>
    <div class="main" id="main">
    </div>
    <input type="hidden" id="lblPrice" />

    <script type="text/javascript">
        function ScorlImage() {
            $("#navlist li").click(function () {
                $(this).toggleClass("active");
                $(this).siblings().removeClass("active");
            });
            jQuery('.more-views').jcarousel({
                start: 1,
                scroll: 1,
                wrap: null,
            });
            jQuery('#mycarouselleft').jcarousel({
                wrap: 'circular',
                scroll: 1
            });
            jQuery('#block-related').jcarousel({
                scroll: 1,

            });
            jQuery("div.add-to-cart .qty_pan").append('<div class="add">&#8250;</div><div class="dec add">&#8249;</div>');

            jQuery(".add").click(function () {
                var jQueryadd = jQuery(this);
                var oldValue = jQueryadd.parent().find("input").val();
                var newVal = 1;

                if (jQueryadd.text() == "›") {
                    newVal = parseFloat(oldValue) + 1;
                    // AJAX save would go here
                } else {
                    // Don't allow decrementing below zero
                    if (oldValue >= 1) {
                        newVal = parseFloat(oldValue) - 1;
                        // AJAX save would go here
                    }
                    if (oldValue == 1) {
                        newVal = parseFloat(oldValue);
                    }
                }
                jQueryadd.parent().find("input").val(newVal);
            });


        }
    </script>
    <script type="text/javascript" src="http://admin.healthurwealth.com/skin/frontend/default/megashop-pink/js/jquery.jcarousel.min.js"></script>
    <div id="vpb_pop_up_background"></div>
    <div id="vpb_login_pop_up_box" class="vpb_show_login_box">
        <section class="container">
    <div class="login">
      <h2><div style="text-align:left;float:left;width:520px;background:none repeat scroll 0 0 #d9387e;position:absolute;padding-left:20px;">Notify Me</div><div style="float:right;background:none repeat scroll 0 0 #d9387e;padding-right:20px;position:absolute;right:0;cursor:pointer;" onclick="closePopup()">X</div></h2>
      <form>
      <table style="padding-top:40px;width:100%;">
      <tr>
          <td style=" width:30%;"><p>Username</p></td>
          <td><p><input type="text" id="txtName" validate="customValidatorGroupName" require="Please enter your  Username." value="" style="margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" placeholder="User Name" /></p></td>
      </tr>
      <tr>
          <td><p>Email</p></td>
          <td><p><input type="text" id="txtEmail"  validate="customValidatorGroupName" class="input-text required-entry validate-email highlight"  email="Invalid Email Address, please check and try again." require="Please enter your  EmailId." style=" margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" value="" placeholder=" Enter EmailId" /></p></td>
      </tr>
      <tr>
          <td><p>Mobile Number</p></td>
          <td><p><input type="text" id="txtPhone" validate="customValidatorGroupName" class="input-text required-entry validate-password highlight" validexpress=".{10,}" regular="Invalid Mobile Number, the Mobile Number must be 10 Number ." style=" margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" name=" MobileNumber" value="" style=" margin: 5px; padding: 0 10px;width: 200px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" placeholder="Enter Mobile Number" /></p></td>
      </tr>
      <tr>
          <td colspan="2"><p class="submit"><input type="button" name="commit" onclick="updateQty()" value="Submit" /></p></td>
        </tr>
      </table>
       
      </form>
    </div>

    
    </div>

    <br clear="all">
    <br clear="all">
    </div>
    <link href="http://admin.healthurwealth.com/images/style.css" rel="stylesheet" type="text/css" />
    <div id="notifyme" class="vpb_signup_pop_up_box">
        <section class="container">
    <div class="login">
      <h2>Notify Me</h2>
      <form >
      <table >
      <tr><td  style=" width:30%;"><p>Your Username</p></td><td><p>
      <input type="text" id="txtNUserName" validate="customNotifyGroupName" require="Please enter your  Username." value="" style=" margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" placeholder="Name" /></p></td></tr>
      <tr><td><p>Email</p></td><td><p><input type="text" id="txtEmailId"  validate="customNotifyGroupName" class="input-text required-entry validate-email highlight"  email="Invalid Email Address, please check and try again." require="Please enter your  EmailId." style=" margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" value="" placeholder=" Enter EmailId" /></p></td></tr>
      <tr><td><p>Mobile Number</p></td><td><p><input type="text" id="txtMobileNumber" validate="customNotifyGroupName" class="input-text required-entry validate-password highlight" validexpress=".{10,}" regular="Invalid Mobile Number, the Mobile Number must be 10 Number ." style=" margin: 5px; padding: 0 10px;width: 230px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" name=" MobileNumber" value="" style=" margin: 5px; padding: 0 10px;width: 200px;height: 34px;color: #404040;background: white; border: 1px solid; border-color: #c4c4c4 #d1d1d1 #d4d4d4;border-radius: 2px;outline: 5px solid #eff4f7;-moz-outline-radius: 3px;-webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.12);" name="Email" placeholder="Enter Mobile Number" /></p></td></tr>
      <tr><td><p class="submit"><input type="button" name="commit" onclick="ValidateNotifyingDetails()" value="Submit" /></p></td><td> <p class="submit"><input type="button" name="commit" onclick="    vpb_hide_popup_boxes()" value="Cancel" /></p></td></tr>
      
       
      </table>
       
      </form>
    </div>

    
  </section>

        <br clear="all">
        <br clear="all">
    </div>

    <style type="text/css">
        .tooltip {
            display: none;
            position: absolute;
            border: 1px solid #333;
            background-color: #fff;
            border-radius: 5px;
            width: 300px;
            height: 100px;
            overflow-y: scroll;
            margin-left: 100px;
            z-index: 9999999999;
            padding: 10px;
            color: #000;
            text-transform: none;
            font-weight: normal;
        }

        .deliveredDiv {
            font-weight: bold;
            text-transform: uppercase;
        }
    </style>


    <script>
        setTimeout(out, 5000);
        function out() {
            var div = document.createElement('div');
            if ($('.outOfStock').length > 0)
                $('body').append(div);
            div.style.position = "absolute";
            div.align = "center";
            div.innerHTML = "<h2>Out of stock</h2>";
            div.clientWidth = "400px";
            div.clientHeight = "400px";
            div.style.textAlign = "Center";
            div.style.verticalAlign = "middle";
            div.style.top = ($('.outOfStock').offset().top + 200) + "px";
            div.style.left = ($('.outOfStock').offset().left + 100) + "px";
            div.style.zIndex = "99";
        }
    </script>

    <div class="overlay">
        <div class="imageDiv">
            <div class="close">X</div>
            <img id="Img2" src="" />
        </div>
    </div>

    <div id='divoutofstock' style='padding-left: 30px'>
        <div class="overlay">
            <div class="imageDiv">
                <div class="close" style="position: fixed">X</div>
                <img id="productImage" src="" />
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('.close, .overlay').click(function () {
                $('.overlay').hide();
            });

        });
    </script>
    <style>
        .close {
            float: right;
            font-size: 16px;
            cursor: pointer;
        }

        .imageDiv {
            top: 50%;
            left: 50%;
            width: 500px;
            height: 300px;
            margin-left: -250px;
            margin-top: -170px;
            color: #fff;
            position: absolute;
        }

            .imageDiv img {
                width: 500px;
                height: 300px;
                border: 5px solid #c4c4c4;
            }

        .overlay {
            background: rgba(0, 0, 0, 0.5);
            width: 100%;
            height: 100%;
            position: fixed;
            top: 0;
            left: 0;
            z-index: 9999999999;
            display: none;
        }
    </style>
    </div>
    <div id="tooltipData" style="display: none;">
        <h6>What is the estimated delivery time?</h6>
        We generally procure and ship the items within the time specified on the product page. Business days exclude public holidays and Sundays. Estimated delivery time depends on the following factors: We offering the product Product's availability with the destination to which you want the order shipped to and the location.
        <br />
        <br />
        <h6>Why does the delivery date not correspond to the delivery timeline of X-Y business days?</h6>
        It is possible our courier partners observe a holiday between the day you placed your order and the date of delivery, which is based on the timelines shown on the product page. In this case, we add a day to the estimated date. Some courier partners and we do not work on Sundays and this is factored in to the delivery dates. 
        <br /><br />
        <h6>What do the different tags like "In Stock", "Available" and "Out of Stock" mean?</h6>
        <br />
        <h6>'In Stock'</h6>
        For items listed as "In Stock", we will mention the delivery time based on your location pincode (usually 3-4 business days, 4-5 business days or 6-8 business days in areas where standard courier service is available). For other areas, orders will be sent by Registered Post  which may take 5-11 days depending on the location.
        <br />
        <h6>'Available'</h6>
        For items listed as "Available", we will mention the delivery time based on your location pincode (usually 3-4 business days, 4-5 business days or 6-8 business days in areas where standard courier service is available). For other areas, orders will be sent by Registered Post  which may take 5-11 days depending on the location.
        <br />
        <h6>'Out of Stock'</h6>
        Currently, the item is not available for sale. Use the 'Notify Me' feature to know once it is available for purchase.
    </div>
</asp:content>--%>
