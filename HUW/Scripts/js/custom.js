function clearconsole() {
    console.log(window.console);
    if (window.console || window.console.firebug) {
        console.clear();
    }
}

var siteURL = window.location.href;
if (siteURL.indexOf('www')) {
    siteURL = "http://admin.healthurwealth.com/";
}
else if (siteURL.indexOf('local')) {
    siteURL = "http://admin.healthurwealth.com/";
}
else {
    siteURL = "http://admin.healthurwealth.com/";
}


var isNew = true;
var pminprice = 30;
var pmaxprice = 20000;
var loading = true;
var psord = '';
var prows = 0;
var pSuperCategoryId = 0;
var pCategoryId = 0;
var pSubCategoryId = 0;
var pSelectedSubCategoryIds = [];
$(document).ready(function () {
    displayCart();
    GetCartProducts();
    CartShortInfo();
    WishListShortInfo();

    Loginink();
    GetWishListProducts();
    CompareListShortInfo();
    $('.spancart').css({ 'display': 'none' });
});
function Discountimg() {
    $.ajax({
        url: siteURL + 'api/Master/GetDiscountImg',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                var Str = "";
                for (x = 0; x < data.Result.length; x++) {
                    Str = Str + "<div class='promo_block2'><a href='#'><img src='" + data.Result[x].ProductImgUrl + "' alt='' /></a></div>";
                }
                $('#divdiscountimg').append(Str);
            }
            if (data.Status == "NoData") {
                window.location = "index.aspx"
            }
        }
    });
}


$('.checkout-heading a').live('click', function () {
    $('.checkout-content').slideUp('slow');

    $(this).parent().parent().find('.checkout-content').slideDown('slow');
});


function ShowInfo(CustomResponse, Control) {

    $('.success, .warning, .attention, .information, .error').remove();
    if (CustomResponse != undefined)
        if (CustomResponse.Status == "Fail") {
            $('#notification').html('<div class="error"  style="display: none;">' + CustomResponse.Message + '</div>');
            $('.error').fadeIn('slow');
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        else if (CustomResponse.Status == "Success") {
            $(Control).append(CustomResponse.Result);
        }
        else if (CustomResponse.Status == "NoData") {
            $('#notification').html('<div class="attention"  style="display: none;">' + CustomResponse.Message + ' </div>');
            $('.success').fadeIn('slow');
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
}
//CheckcartItemsInfo
function CheckcartItemsInfo() {
    $.ajax({
        url: siteURL + 'api/Master/GetCartItems',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                window.location = "CheckOut.aspx";
            }
            if (data.Status == "NoData") {
                window.location = "index.aspx"
            }
        }
    });
}

//btnCheck link
function btncheckout() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        window.location = "Userlogin.aspx";
    }
    else {
        window.location = "MyCart.aspx";
    }

}
//end

function ViewMoreProducts(SupCat) {
    $('#viewmoreProducts').parent('div').find('#loadingGif').show();
    $('#viewmoreProducts').hide();
    $.ajax({
        url: siteURL + 'api/Master/ViewMoreProducts?SuperCategoryID=' + SupCat,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#lazyLoadAlt').append(data.Result);
                if (SupCat != 6) {
                    $('#viewmoreProducts').attr('onclick', 'ViewMoreProducts(' + (parseInt(SupCat) + 1) + ')');
                    //$('#viewmoreProducts').show();
                    $('#viewmoreProducts').removeClass('loading');
                    $('#viewmoreProducts').parent('div').find('#loadingGif').hide();
                }
                else {
                    $('#viewmoreProducts').hide();
                    $('#viewmoreProducts').parent('div').find('#loadingGif').hide();
                }
            }
            else {
                $('#lazyLoadAlt').append(data.Result);
            }
        }
    });
}


//CheckOut_Registration



function UserRegistration() {
    var cartJson = { 'FirstName': $("#txbFirstName").val(), 'LastName': $("#txbLastName").val(), 'EmailId': $("#txbEmailId").val(), 'PassCode': $("#txbPassword").val(), 'MobileNo': $("#txbMobileNumber").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/SmartRegistration',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                window.location = 'CheckOut.aspx';
                LoadAddressTab();
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
            }
            else {
                $('#notification').empty();
                ShowInfo(data, '#notification');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
            }
        },
        error:
        {
            //Show error message
        }
    });
}

//End CheckOut_Registration

function Insertpin() {
    var cartJson = { 'State': $("#ddlstate").val(), 'District': $("#txtDistrict").val(), 'City': $("#txtCity").val(), 'Location': $("#txtLocation").val(), 'pincode': $("#txtpincode").val(), 'cod': $("#txtcod").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/Insertpin',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                window.location = 'CheckOut.aspx';
                LoadAddressTab();
            }
            else {
                ShowInfo(data, '#notification');
            }
        },
        error:
        {
            //Show error message
        }
    });
}



//UserRegistration
function btnUserRegistration() {
    var cartJson = { 'FirstName': $("#txbFirstName").val(), 'LastName': $("#txbLastName").val(), 'EmailId': $("#txbEmailId").val(), 'PassCode': $("#txbPassword").val(), 'MobileNo': $("#txbMobileNumber").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/NewUserRegistration",
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            //            data = jQuery.parseJSON(data);
            if (data.Status == "Success") {
                ClearTextBox();
                $('.success').fadeIn('slow');
                $('html, body').animate({ scrollTop: 0 }, 'slow');
                $('#notification').empty();
                $('#divRegmessage').empty();
                $('#divRegmessage').html('<div id="success"> <a href="UserLogin.aspx">  ' + data.Message + '</a> </div>');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });

            }
            if (data.Status == "Fail") {
                $('#notification').empty();
                $("#divRegmessage").empty();
                $('html, body').animate({ scrollTop: 0 }, 'slow');
                $('#divRegmessage').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
                //                $('#notification').html('<div id="error"> <a href="UserLogin.aspx">  ' + data.Message + '</a> </div>');
            }
        },
        error:
        {
            //Show error message
        }
    });
}

//Clear All Sessions
function ClearAllSessions() {
    $.ajax({
        url: siteURL + 'api/Master/ClearSession',
        type: 'GEt',
        dataType: "json",
        headers: { "Content-Type": "application/json" },
        success: function (data) {
            if (data.Status == "Success") {
                window.location = 'index.aspx';
            }
            else {
                ShowInfo(data, '#notification');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });
}
function GetUserIP() {
    $.ajax({
        url: siteURL + 'api/Master/GetUserIP',
        type: 'GEt',
        dataType: "json",
        headers: { "Content-Type": "application/json" },
        success: function (data) {

            var ip = data.d.ipaddress;
            var host = data.dHostName;

        },


        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });
}


//Login and Logot
function Loginink() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {

        $("#editProfile").hide();
        $("#editPwd").hide();
        $("#btnUcLogOut").hide();

    }
    else {
        $("#btnUserName").html(json.firstName);
        $("#a1").hide();
        $("#btnCreateAccount").hide();
        $("#btnUclogin").hide();
        $("#btnUcLogOut").show();
        $("#editProfile").show();
        $("#editPwd").show();
        $('#btnForgot').hide();
        $('#btnlogin').hide();
        $('#btnReg').hide();
        $("#btnregistration").hide();
    }
};

//End

function addressnext() {
    AddressCount = AddressCount + 3;
    LoadAddressTab();
}

function addressprev() {
    AddressCount = AddressCount - 3;
    LoadAddressTab();
}

//End UserRegistration
var AddressCount = 0;


function LoadAddressTab() {

    GetStates();
    $.ajax({
        url: siteURL + 'api/Master/UserAddressDetails?rows=' + AddressCount,
        type: 'GET',
        dataType: 'json',
        beforeSend: function () {
            $('#button-account').attr('disabled', true);
            $('#button-account').after('<span class="wait">&nbsp;<img src="catalog/view/theme/default/image/loading.gif" alt="" /></span>');
        },

        success: function (data) {
            $('#privAddress').empty();
            //   AddressCount = data.Rows;
            ShowInfo(data, '#privAddress');
            btnBilling();
            ;
            if (data.Status == "NoData") {
                $('#addprev').hide();

                $('#addressCount').html('(' + (AddressCount + data.Rows) + '/' + data.Rows + ')');
            }
            else {
                $('#addressCount').html('(' + (AddressCount + 3) + '/' + data.Rows + ')');

            }
            if (AddressCount >= 3) {
                $('#addnext').show();
                $('#addprev').show();
            }
            if (AddressCount < 3) {
                $('#addnext').show();
                $('#addprev').hide();
            }
            var TotalCount = AddressCount + 3;
            if (TotalCount > data.Rows) {
                $('#addressCount').html('(' + (data.Rows) + '/' + data.Rows + ')');
                $('#addnext').hide();
            }


            LoadAddressLogic();
        }


    });
}

//ConformOrder
function ConfirmOrder() {

    $.ajax({
        url: siteURL + 'api/Master/ConfirmOrder',
        type: 'post',
        dataType: 'json',
        success: function (json) {
            if (json['redirect']) {
                location = json['redirect'];
            }

            if (json['Result']) {
                $('#confirm .checkout-content').html(json['Result']);

                $('#payment-address .checkout-content').slideUp('slow');

                $('#confirm .checkout-content').slideDown('slow');



                $('#payment-address .checkout-heading').append('<a>Modify &raquo;</a>');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });
}


function Loginpopup() {
    $('a.login-window').click(function () {

        // Getting the variable's value from a link 
        var loginBox = $(this).attr('href');

        //Fade in the Popup and add close button
        $(loginBox).fadeIn(300);

        //Set the center alignment padding + border
        var popMargTop = ($(loginBox).height() + 24) / 2;
        var popMargLeft = ($(loginBox).width() + 24) / 2;

        $(loginBox).css({
            'margin-top': -popMargTop,
            'margin-left': -popMargLeft
        });

        // Add the mask to body
        $('body').append('<div id="mask"></div>');
        $('#mask').fadeIn(300);

        return false;
    });

    // When clicking on the button close or the mask layer the popup closed
    $('a.close, #mask').live('click', function () {
        $('#mask , .login-popup').fadeOut(300, function () {
            $('#mask').remove();
        });
        return false;
    });
}
//Check ReviesLogin
function CheckReviesLogin() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        vpb_show_login_box();
    }
    else {
        WriteProductReviews();
    }
};

//End ReviewsLogin


//Check Loginsession from WishList
function CheckWishListLogin() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        window.location.href = "Userlogin.aspx";
    }
    else {
        window.location.href = "WishList.aspx";
    }
};


//End WishList Login

//Check Loginsession from MyAccount
function CheckMyAccountLogin() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        window.location = "Userlogin.aspx";
    }
    else {
        window.location = "MyTransactions.aspx?ID=" + json['UserId'];
    }
};


//End MyAccount Login

//Cart Login

function LoadCheckOutTabs() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        $('#checkout-step-login').slideDown('slow');
        $('#button-account').attr('disabled', false);
        //    alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        var cartitems = CheckCartCount();
        if (cartitems != 0) {
            $('#button-account').attr('disabled', true);
            $('#checkout-step-login').slideUp('slow');
            var CustomResponse = IsAddressAssignedToCart();

            if (CustomResponse != undefined)
                if (CustomResponse.Status == "Success") {

                    if (CustomResponse.Result == undefined || CustomResponse.Result['ShippingAddressId'] == null || CustomResponse.Result['ShippingAddressId'] == '0' || CustomResponse.Result['BillingAddressId'] == null || CustomResponse.Result['BillingAddressId'] == '0') {
                        //  $('#checkout .checkout-content').slideDown('slow');
                        $('#button-account').attr('disabled', false);
                        LoadAddressTab();
                    }
                    else {
                        //  GetOrderOverview();
                        LoadAddressTab();
                    }
                }
                else ShowInfo(CustomResponse);
        }
        else {
            window.location.href = "index.aspx";
        }
    }
};





//Post Address
function AddUserAddress() {
    var cartJson;
    if ($('chkBillingAsShippingtrue').is(':checked')) {
        cartJson = [
    {
        'StreetAddress1': $("#txbShippingAddress1").val(),
        'StreetAddress2': $("#txbShippingAddress2").val(),
        'LandMark': $("#txbShippingLandmark").val(),
        'PinCode': $("#txbShippingPincode").val(),
        'StateName': $('#txbShippingState option:selected').text(),
        'City': $('#txbShippingCity').val(),
        'StateId': $('#txbShippingState option:selected').val(),
        'CountryId': $('#ddlShippingCountry option:selected').val(),
        'UserAddressId': $('#ShippingAddressId').val()
    },
    {
        'StreetAddress1': $("#txtAddress1Bill").val(),
        'StreetAddress2': $("#txtAddress2Bill").val(),
        'LandMark': $("#txtLandmarkBill").val(),
        'PinCode': $("#txtPincodeBill").val(),
        'StateName': $('#txbShippingState option:selected').text(),
        'City': $('#txtCityBill').val(),
        'StateId': $('#txbStateBill option:selected').val(),
        'CountryId': $('#ddlShippingCountry option:selected').val(),
        'UserAddressId': $('#BillingAddressId').val()
    }];
    }
    else {
        cartJson = [
            {
                'StreetAddress1': $("#txbShippingAddress1").val(),
                'StreetAddress2': $("#txbShippingAddress2").val(),
                'LandMark': $("#txbShippingLandmark").val(),
                'PinCode': $("#txbShippingPincode").val(),
                'StateName': $('#txbShippingState option:selected').text(),
                'City': $('#txbShippingCity').val(),
                'StateId': $('#txbShippingState option:selected').val(),
                'CountryId': $('#ddlShippingCountry option:selected').val(),
                'UserAddressId': $('#ShippingAddressId').val()
            }];

    }

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/PostAddress',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),
        success: function (data) {
            if (data.Status == "NoData") {
                document.getElementById('PincodeAvailMsg').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Sorry! Product cannot be delivered at your pincode location.</span></div>";
            }
            if (data.Status == "No Pin Code") {
                document.getElementById('Div1').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Sorry! Product cannot be delivered at your pincode location.</span></div>";
                //                ShowInfo(data.Result, 'Product can not be delivered at your location.');
            }
            if (data.Status == "Success") {

                btnProductOverview();
                GetOrderOverview();
            }
            else {

                ShowInfo(data, 'Give any control id to show fail message');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });


}

//New function for AddUserAddress


function AddUserAddress() {
    var cartJson;
    if ($('chkBillingAsShippingtrue').is(':checked')) {
        cartJson = [
    {
        'StreetAddress1': $("#txbShippingAddress1").val(),
        'StreetAddress2': $("#txbShippingAddress2").val(),
        'LandMark': $("#txbShippingLandmark").val(),
        'PinCode': $("#txbShippingPincode").val(),
        'StateName': $('#txbShippingState option:selected').text(),
        'City': $('#txbShippingCity').val(),
        'StateId': $('#txbShippingState option:selected').val(),
        'CountryId': $('#ddlShippingCountry option:selected').val(),
        'UserAddressId': $('#ShippingAddressId').val()
    },
    {
        'StreetAddress1': $("#txtAddress1Bill").val(),
        'StreetAddress2': $("#txtAddress2Bill").val(),
        'LandMark': $("#txtLandmarkBill").val(),
        'PinCode': $("#txtPincodeBill").val(),
        'StateName': $('#txbShippingState option:selected').text(),
        'City': $('#txtCityBill').val(),
        'StateId': $('#txbStateBill option:selected').val(),
        'CountryId': $('#ddlShippingCountry option:selected').val(),
        'UserAddressId': $('#BillingAddressId').val()
    }];
    }
    else {
        cartJson = [
            {
                'StreetAddress1': $("#txbShippingAddress1").val(),
                'StreetAddress2': $("#txbShippingAddress2").val(),
                'LandMark': $("#txbShippingLandmark").val(),
                'PinCode': $("#txbShippingPincode").val(),
                'StateName': $('#txbShippingState option:selected').text(),
                'City': $('#txbShippingCity').val(),
                'StateId': $('#txbShippingState option:selected').val(),
                'CountryId': $('#ddlShippingCountry option:selected').val(),
                'UserAddressId': $('#ShippingAddressId').val()
            }];

    }

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/PostAddress',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),
        success: function (data) {
            if (data.Status == "NoData") {
                document.getElementById('PincodeAvailMsg').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Sorry! Product cannot be delivered at your pincode location.</span></div>";
            }
            if (data.Status == "No Pin Code") {
                document.getElementById('Div1').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Sorry! Product cannot be delivered at your pincode location.</span></div>";
                //                ShowInfo(data.Result, 'Product can not be delivered at your location.');
            }
            if (data.Status == "Success") {

                btnProductOverview();
                CheckoutOrder();
            }
            else {

                ShowInfo(data, 'Give any control id to show fail message');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });


}

//End function for AddUserAddress


//Edit Address
function EditUserAddress() {
    //    var bid = $('div .tradus-select-address').find("input[id*='hdnShippingAddId']").val().trim()
    //   var sid= $('div .tradus-select-address').find("input[id*='hdnShippingAddId']").val().trim()
    //    $('#BillingAddressId').val($(this).div .tradus-select-address("input[id*='hdnBillingAddId']").val().trim());
    //    $('#ShippingAddressId').val();
    var cartJson = [
    {
        'StreetAddress1': $("#txbEditShippingAddress1").val(),
        'StreetAddress2': $("#txbEditShippingAddress2").val(),
        'LandMark': $("#txbEditShippingLandmark").val(),
        'PinCode': $("#txbEditShippingPincode").val(),
        'StateName': $('#txbEditShippingState option:selected').text(),
        'City': $('#txbEditShippingCity').val(),
        'StateId': $('#txbEditShippingState option:selected').val(),
        'CountryId': $('#ddlEditShippingCountry option:selected').val(),
        'UserAddressId': $('#addressId').val()


    }];

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/EditUserAddressDetails',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            $("#lblresult").html('');
            if (data.Status == "Success") {
                if (data.Message == "Invalid Zip/Postal Code") {
                    $("#lblresult").html(data.Message);
                }
                else {
                    $("#notification").empty();
                    $("#notification").append(data.Message);
                    setTimeout(hideMsg, 5000);
                    vpb_hide_popup_boxes();
                    LoadAddressTab();
                }
                // btnPayment();

            }
            else {
                ShowInfo(data, 'Give any control id to show fail message');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });


}
function hideMsg() { $("#notification").empty() }

//End Edit Address

//New Userlogin
function btnNewuserRegForm() {
    $('#checkout-step-login').slideUp('slow');

    $('#checkout-step-NewUser').slideDown('slow');
}
function btnBilling() {
    $('#checkout-step-login').hide();
    $('#checkout-step-NewUser').slideUp('slow');
    $('#checkout-step-payment').slideUp('slow');
    $('#checkout-step-review').slideUp('slow');
    $('div #checkoutsteps #opc-login').removeClass('active');

    $('div #checkoutsteps #opc-billing').addClass('active');
    $('div #checkoutsteps #opc-billing').removeClass('allow');
    $('div #checkoutsteps #opc-login').removeClass('allow');
    $('div #checkoutsteps #opc-payment').removeClass('active');
    $('div #checkoutsteps #opc-review').removeClass('active');
    $('#checkout-step-billing').slideDown('slow');
}

//end

//Billing Tab
function btnPayment() {
    $('#divstack').empty();
    $('#checkout-step-login').hide();
    $('#checkout-step-review').slideUp('slow');
    $('#checkout-step-payment').slideDown('slow');
    $('div #checkoutsteps #opc-login').removeClass('active');
    $('div #checkoutsteps #opc-login').removeClass('allow');
    $('div #checkoutsteps #opc-billing').addClass('allow');
    $('div #checkoutsteps #opc-review').removeClass('active');
    $('div #checkoutsteps #opc-payment').addClass('active');
    $('div #checkoutsteps #opc-billing').removeClass('active');
    fillData();
    //if (parseInt($('#lblAmount').text()) >= 1050) {
    //    $('#spnCOD').show();
    //}
    //else {
    //    $('#spnCOD').hide();
    //}

}
function btnProductOverview() {
    //    $('#checkout-step-login').hide();
    //    $('#checkout-step-billing').hide();
    $('#checkout-step-billing').slideUp('slow');
    $('#checkout-step-review').slideDown('slow');
    $('div #checkoutsteps #opc-billing').removeClass('active');
    $('div #checkoutsteps #opc-billing').addClass('allow');
    $('div #checkoutsteps #opc-review').addClass('active');

    //    $('#checkout-step-billing').slideUp('slow');
    //    $('#checkout-step-login').hide();
    //    $('#checkout-step-payment').slideDown('slow');
}


//CheckOut_Login
function btnLoginClicked() {
    var cartJson = { 'EmailId': $("#txtUserName").val(), 'PassCode': $("#txtPwd").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/UserLogin',
        type: 'post',
        dataType: "json",
        data: JSON.stringify(cartJson),
        success: function (data) {
            if (data.Status == "Success") {
                //     $('#checkout .checkout-heading').append('<a>Modify &raquo;</a>');
                // $('#checkout .checkout-content').slideDown('slow');
                //  window.location = 'CheckOut.aspx';
                LoadAddressTab();
                CheckboxDisable();
                Loginink();
                //  $('#tradus-billing-address').hide();

                $('input[type=text]').each(function () {
                    $(this).val('');
                });
            }
            else {
                $('#checkout .checkout-heading').empty();
                $('#checkoutloginmsg').empty();
                $('#checkoutloginmsg').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                //ShowInfo(data, '#notification');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });
}
//End Checkout_Login

//UserLogin

function btnUserLoginClicked() {
    val = getParameterByName("redirectval");
    var cartJson = { 'EmailId': $("#txtUserName").val(), 'PassCode': $("#txtPwd").val(), 'Redirectvalue': val };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/UserLogin',
        type: 'post',
        dataType: "json",
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                $("#notification").empty();
                $("#divloginmsg").empty();

                // window.location = data.PageRedirect;
                window.location.href = "index.aspx";


            }
            if (data.Status == "NoData") {
                $("#divloginmsg").empty();
                //$("#notification").empty();
                $('#divloginmsg').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                // $("#notification").html('<div class="error">' + data.Message + '</div>');
                //                ShowInfo();

            }
            if (data.Status == "Fail") {
                $("#notification").empty();
                $("#divloginmsg").empty();
                $('#divloginmsg').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                // $("#notification").html('<div class="error">' + data.Message + '</div>');
            }
        },
        error:
        {
            //Show error message
        }
    });
}
//EndUserLogin

//GetProfile Details
function GetprofileInfo() {

    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/GetProfileDetails?Uid=' + json['UserId'],
            type: 'GET',
            dataType: 'json',
            success: function (data) {

                $("#txbFirstName").val(data.Result.FirstName),
    $("#txbLastName").val(data.Result.LastName),
    $("#txbEmailId").val(data.Result.EmailId),
    $("#txbMobileNumber").val(data.Result.MobileNo)
                // window.location = "Edit_Profile.aspx";
            }

        });
    }
}
//END

//EditPassword

function btnPasswordUpdate() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        var cartJson = { 'PassCode': $("#txtCPwd").val(), 'UserId': json['UserId'] };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + 'api/Master/EditPassword',
            type: 'post',
            dataType: "json",
            data: JSON.stringify(cartJson),
            headers: { "Content-Type": "application/json" },
            success: function (data) {
                if (data.Status == "Success") {
                    $("#txtPwd").val('');
                    $("#txtCPwd").val('');
                    //                $("#txtCPwd").val('')
                    //                    $.msgBox({
                    //                        title: "Selam / Hello",
                    //                        content: "Merhaba Dünya! / Hello World!",
                    //                        type: "info"
                    //                    });

                    $("#divMsgPwdEdit").empty();
                    document.getElementById('divMsgPwdEdit').innerHTML = "<span class='iconsweet' sty></span>Your Password has Updated Successfully.";
                    document.getElementById('divMsgPwdEdit').style.display = 'block';

                }
                else {
                    //                    $('#checkout .checkout-heading').empty();
                    //                    ShowInfo(data, '#notification');
                    $("#divMsgPwdEdit").empty();
                    document.getElementById('divMsgPwdEdit').innerHTML = "<span class='iconsweet'></span>Your Password not Updated, Please Try again.";
                    document.getElementById('divMsgPwdEdit').style.display = 'block';
                }
            },
            error:
        {
            //Show error message
        }
        });
    }
}
//End

function ResendPassword() {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/SendForgotpassword?EmailId=' + $('#email_address').val(),
        type: 'post',
        dataType: "json",
        headers: { "Content-Type": "application/json" },
        success: function (data) {
            if (data.Status == "Success") {

                $('#divforgotstatus').html(data.Message);
                alert(data.Message);
                window.location.href = "Userlogin.aspx";
            }
            else {
                $('#divforgotstatus').html(data.Message);
                alert(dat.Message);
            }
        },
        error:
    {
        //Show error message
    }
    });

}

//Edit UserInfo
function btnUpdate() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        //   alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        var cartJson = { 'EmailId': $("#txbEmailId").val(), 'FirstName': $("#txbFirstName").val(), 'LastName': $("#txbLastName").val(), 'MobileNo': $("#txbMobileNumber").val(), 'UserId': json['UserId'] };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + 'api/Master/EditUsers',
            type: 'post',
            dataType: "json",
            data: JSON.stringify(cartJson),
            headers: { "Content-Type": "application/json" },
            success: function (data) {
                if (data.Status == "Success") {
                    $("#txbFirstName").val('');
                    $("#txbLastName").val('');
                    $("#txbEmailId").val('');
                    $("#txbMobileNumber").val('');
                    $("#divMsgProfileEdit").empty();
                    document.getElementById('divMsgProfileEdit').innerHTML = "<span class='iconsweet'  style='font-weight: bold;'></span>Your Profile Updated Successfully.";
                    document.getElementById('divMsgProfileEdit').style.display = 'block';
                    ShowInfo(data.Status, '#notification');
                }
                else {
                    $('#checkout .checkout-heading').empty();
                    ShowInfo(data, '#notification');
                    $("#divMsgProfileEdit").empty();
                    document.getElementById('divMsgProfileEdit').innerHTML = "<span class='iconsweet' sty></span>Your Password not Updated, Please try again.";
                    document.getElementById('divMsgProfileEdit').style.display = 'block';
                }
            },
            error:
        {
            //Show error message
        }
        });
    }
}
// end Edit UserInfo


//Invoice

function FillUserTransctionDetails() {
    var CurrentURL = window.location.href;
    var id = CurrentURL.split('=')[1];
    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/GetInvoice?TransactionId=' + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#divInvoice').empty();
           
            ShowInfo(data, '#divInvoice');
           
        }

    });

}
function FillMedicineUserTransctionDetails() {
    var CurrentURL = window.location.href;
    var id = CurrentURL.split('=')[1];
    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/GetMedicineInvoice?TransactionId=' + id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#divInvoice').empty();

            ShowInfo(data, '#divInvoice');

        }

    });

}

function ContactUS() {
    var Name = $('#name').val();
    var Email = $('#email').val();
    var Mobile = $('#telephone').val();
    var Comments = $('#comment').val();
    $.ajax({
        url: siteURL + 'api/Master/ContactUS?Name=' + Name + '&Email=' + Email + '&Mobile=' + Mobile + '&Comments=' + Comments,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            alert('Your message successfully sent to Healthurwealth.');
        }

    });

}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
}

function displayCart() {

    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/ShowRelatedProductsList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "NoData") {
                $('#last_msg_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#DisplayRelatedProductList').empty();
                ShowInfo(data, '#DisplayRelatedProductList');

                //    $("img").lazyload({ threshold: "200", effect: "fadeIn", effectspeed: 2000 });

                //                $("img.lazyload").each(function () {
                //                    $(this).attr("src", $(this).attr("original"));
                //                    $(this).removeAttr("original");
                //                });

                $('div#last_msg_loader').empty();
                // loading = true;
            }

        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }


    });
}

function removeCartItem(productId) {
    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'PersonService.asmx/ShowCart',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            displayCart();
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}

//SLIDE TOGGLE
jQuery(".minicart").live('hover', function () {
    // displayCart();
    //   $('.cart_drop').load( siteURL+'PersonService.asmx/ShowCart .cart_drop > *');
    //$('.cart_drop').slideDown(300);
    //        jQuery('.minicart').live('mouseleave', function () {
    //            $('.cart_drop').slideUp(300);
    //        });
});

//SHORTCODES
//Toggle Box
jQuery(".toggle_box > li:first-child .toggle_title, .toggle_box > li:first-child .toggle_content").addClass('active');

function ActiveTab(dis) {

    //var $this = $(dis); // cache $(this)

    dis.addClass('active');
    $($this.attr('href')).slideToggle();

    return false; // prevent anchor click event

}


//jQuery("div .step-title a").toggle(function () {

//    $(this).addClass('active');
//    //  $(this).siblings('.toggle_content').slideDown(300);
//}, function () {
//    $(this).removeClass('active');
//    //  $(this).siblings('.toggle_content').slideUp(300);
//});

//SUBMENU
jQuery("ul.departments > li.menu_cont > a").click(function () {
    $(this).toggleClass('active').next().slideToggle(300);

});

$('ul.departments > li.menu_cont > a.active + .side_sub_menu').slideDown(300);;

/* Search */
//$('.button-search').bind('click', function () {
//    url = $('base').attr('href') + 'index.php?route=product/search';

//    var filter_name = $('input[name=\'filter_name\']').attr('value');

//    if (filter_name) {
//        url += '&filter_name=' + encodeURIComponent(filter_name);
//    }

//    location = url;
//});

//$('header input[name=\'filter_name\']').bind('keydown', function (e) {
//    if (e.keyCode == 13) {
//        url = $('base').attr('href') + 'index.php?route=product/search';

//        var filter_name = $('input[name=\'filter_name\']').attr('value');

//        if (filter_name) {
//            url += '&filter_name=' + encodeURIComponent(filter_name);
//        }

//        location = url;
//    }
//});

/* Mega Menu */
$('#menu ul > li > a + div').each(function (index, element) {
    // IE6 & IE7 Fixes
    if ($.browser.msie && ($.browser.version == 7 || $.browser.version == 6)) {
        var category = $(element).find('a');
        var columns = $(element).find('ul').length;

        $(element).css('width', (columns * 143) + 'px');
        $(element).find('ul').css('float', 'left');
    }

    var menu = $('#menu').offset();
    var dropdown = $(this).parent().offset();

    i = (dropdown.left + $(this).outerWidth()) - (menu.left + $('#menu').outerWidth());

    if (i > 0) {
        $(this).css('margin-left', '-' + (i + 5) + 'px');
    }
});

// IE6 & IE7 Fixes
if ($.browser.msie) {
    if ($.browser.version <= 6) {
        $('#column-left + #column-right + #content, #column-left + #content').css('margin-left', '195px');

        $('#column-right + #content').css('margin-right', '195px');

        $('.box-category ul li a.active + ul').css('display', 'block');
    }

    if ($.browser.version <= 7) {
        $('#menu > ul > li').bind('mouseover', function () {
            $(this).addClass('active');
        });

        $('#menu > ul > li').bind('mouseout', function () {
            $(this).removeClass('active');
        });
    }
}

$('.success img, .warning img, .attention img, .information img').live('click', function () {
    $(this).parent().fadeOut('slow', function () {
        $(this).remove();
    });
});


function getURLVar(urlVarName) {
    var urlHalves = String(document.location).toLowerCase().split('?');
    var urlVarValue = '';

    if (urlHalves[1]) {
        var urlVars = urlHalves[1].split('&');

        for (var i = 0; i <= (urlVars.length) ; i++) {
            if (urlVars[i]) {
                var urlVarPair = urlVars[i].split('=');

                if (urlVarPair[0] && urlVarPair[0] == urlVarName.toLowerCase()) {
                    urlVarValue = urlVarPair[1];
                }
            }
        }
    }

    return urlVarValue;
}

function checkCartItemExist(product_id) {

    var cartJson = { 'productId': product_id };
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/CheckProductInCart",
        type: 'post',
        async: false,
        data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            json = json.d;
            data = json;



        }
    });
    return data;
}
function checkWishListItemExist(product_id) {

    var cartJson = { 'productId': product_id };
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/CheckProductInWishList",
        type: 'post',
        async: false,
        data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            json = json.d;
            data = json;
            if (data == false) {
                // window.location.href = "Userlogin.aspx";
            }
        }
    });
    return data;
}

//Updateqtyin shortcart
function UpdateQty(product_id, SubProductId, quantity, isRedirect,issold) {

    // alert('update quantity method new ' + $(quantity).closest('table').html());

    jQuery("div.add-to-cart #qtinc").append('<div class="add">&#8250;</div><div class="dec add">&#8249;</div>');
    
    var jQueryadd = jQuery(quantity);
    var oldValue = jQueryadd.parent().find("input").val();
    var newVal = 1;

    if (jQueryadd.text() == "Up") {
        newVal = parseFloat(oldValue) + 1;
        // AJAX save would go here

    }
    else {
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

    var prvQty = $(quantity).closest('div.cart-quantity').find('#txtPrvQty').val();
    // var prvQty = $(" div#divCart table td.quantity #txtPrvQty").val();
    quantity = typeof (newVal) != 'undefined' ? newVal : 1;
    isRedirect = typeof (isRedirect) != 'undefined' ? isRedirect : 0;
    specifications = typeof (specifications) != 'undefined' ? specifications : null;
    var userProductTransaction = {}; // var product = {}; var user = {};

    userProductTransaction.ProductId = product_id;
    userProductTransaction.SubProductId = SubProductId;
    userProductTransaction.UserProductTransactionSpecifications = specifications;
    //	cart.CartProduct = product;
    //cart.CartUser = user;
    userProductTransaction.Quantity = newVal;
    var cartJson = { 'isRedirect': isRedirect, 'cart': userProductTransaction };
    if (quantity > 0) {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: apiurl + "/PersonService.asmx/UpdateQuantityToCart",
            type: 'post',
            data: JSON.stringify(cartJson),
            //   data:  cartJson,
            dataType: 'json',
            success: function (json) {
                if (json.d.Status == "Success") {
                    //  GetProduct();
                    $('#divstack').empty();
                    $('#divstack').hide();
                    $("#lblProductcost").empty();
                    $("#divoutofstock").empty();
                    $("#btnadtocart").html('');
                    $("#btnadtocart").append("<div class='add-to-cart' id='btnadtocart' ><button type='button' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='NavigateToMyCart()'><span><span>Buy Now</span></span></button></div>");
                    $("#lblProductcost").html(json.d.CartSum);
                    GetCartProducts();
                    CartShortInfo();
                    GetOrderOverview();

                }
                if (json.d.Status == "Fail") {
                    // NavigateToMyCart();
                    GetCartProducts();
                    CartShortInfo();
                    //$("#lblProductcost").html(json.d.CartSum);
                    $("#txtqt").val('');
                    $("#txtqt").val(json.d.Result);
                    GetCartProducts();
                    $("#cart-message").empty();
                    var msg = json.d.Message + ". Notify Me <a onClick='vpb_show_login_box();'>Click here</a>"
                    $('#divstack').show();
                    $('#divstack').html(msg);
                    //$('#divoutofstock').empty();
                    ////                    $("#no-stock-75835").append(data.d.Message);
                    //$('#divoutofstock').append('<div  class="no-stock ">' + json.d.Message + ' </div>');
                    // ShowInfo(data.d, '#notification');
                    $('.success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }
                //if (json.d.Status == "Fail") {

                //    // NavigateToMyCart();
                //    GetCartProducts();
                //    CartShortInfo();
                //    //$("#lblProductcost").html(json.d.CartSum);
                //    $("#txtqt").val('');
                //    $("#txtqt").val(json.d.Result);
                //    GetCartProducts();
                //    $("#cart-message").empty();
                //    $('#divoutofstock').empty();
                //    $('#divoutofstock1').show();
                //    //                    $("#no-stock-75835").append(data.d.Message);
                //    var msg = json.d.Message + " Notify Me <a onClick='vpb_show_login_box();'>Click</a>"
                //    $('#divstack').html(msg);
                //    $('#divstack').show();
                //    // ShowInfo(data.d, '#notification');
                //    $('.success').fadeIn('slow');
                //    $('html, body').animate({ scrollTop: 0 }, 'slow');
                //}

            }


        });
    }


}



function SubProdUpdateQty(product_id, SubProductId, quantity, isRedirect, thi) {

    //jQuery("div.add-to-cart #qtinc").append('<div class="add">&#8250;</div><div class="dec add">&#8249;</div>');

    var jQueryadd = jQuery(quantity);
    var oldValue = jQueryadd.parent().find("input").val();
    var newVal = 1;
    if (jQueryadd.text() == "Up") {
        newVal = parseFloat(oldValue) + 1;
        // AJAX save would go here
    }
    else {
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

    var prvQty = $(quantity).closest('div.cart-quantity').find('#txtPrvQty').val();
    // var prvQty = $(" div#divCart table td.quantity #txtPrvQty").val();
    quantity = typeof (newVal) != 'undefined' ? newVal : 1;
    isRedirect = typeof (isRedirect) != 'undefined' ? isRedirect : 0;
    specifications = typeof (specifications) != 'undefined' ? specifications : null;
    var userProductTransaction = {}; // var product = {}; var user = {};

    userProductTransaction.ProductId = product_id;
    userProductTransaction.SubProductId = SubProductId;
    userProductTransaction.UserProductTransactionSpecifications = specifications;
    //	cart.CartProduct = product;
    //cart.CartUser = user;
    userProductTransaction.Quantity = newVal;
    var cartJson = { 'isRedirect': isRedirect, 'cart': userProductTransaction };
    if (quantity > 0) {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/SubPrdUpdateQuantityToCart",
            type: 'post',
            data: JSON.stringify(cartJson),
            //   data:  cartJson,
            dataType: 'json',
            success: function (json) {
                if (json.d.Status == "Success") {
                    //  GetProduct();
                    //var value = $(".lblSubProd").html(json.d.price);
                    //$('#lblProductcost').html(json.d.CartSum);
                    jQueryadd.parent('div').parent('div').closest('td').next('td').find('label').html(json.d.price)
                    //$("#lblProductcost").empty();
                    //$("#divoutofstock").empty();
                    //$("#btnadtocart").html('');
                    //$("#btnadtocart").append("<div class='add-to-cart' id='btnadtocart' ><button type='button' title='Add to Cart' class='button btn-cart' style='margin-top:5px;'  onclick='NavigateToMyCart()'><span><span>Buy Now</span></span></button></div>");
                    //$("#lblProductcost").html(json.d.CartSum);
                    //GetCartProducts();
                    //CartShortInfo();
                    //GetOrderOverview();

                    $('#divstack').empty();
                    $('#divstack').hide();
                }

                if (json.d.Status == "Fail") {

                    // NavigateToMyCart();
                    GetCartProducts();
                    CartShortInfo();
                    //$("#lblProductcost").html(json.d.CartSum);
                    //  $("#txtqt").val('');
                    jQueryadd.parent().find("input").val(json.d.Result);
                    //  $("#txtqt").val(json.d.Result);
                    GetCartProducts();
                    $("#cart-message").empty();
                    var msg = json.d.Message + ". Notify Me <a onClick='vpb_show_login_box();'>Click</a>"
                    $('#divstack').show();
                    $('#divstack').html(msg);
                    //$('#divoutofstock').empty();
                    ////                    $("#no-stock-75835").append(data.d.Message);
                    //$('#divoutofstock').append('<div  class="no-stock ">' + json.d.Message + ' </div>');
                    // ShowInfo(data.d, '#notification');
                    $('.success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }

            }


        });
    }


}
//
function BuyNow() {
    var price = $('#lblProductcost').text();
    if ((parseInt($('#lblProductcost').text())) == 0) {
        alert('Your Cart is Empty');
    } else {
        $('#divstack').empty();
        NavigateToMyCart();
    }

}

function addToCart(product_id, SubProductId, quantity, isRedirect) {

    $("#btnbuynow").attr("disabled", "disabled");
    quantity = $('#txtqt').val();
    quantity = typeof (quantity) != 'undefined' ? quantity : 1;
    isRedirect = typeof (isRedirect) != 'undefined' ? isRedirect : 0;
    specifications = typeof (specifications) != 'undefined' ? specifications : null;
    var userProductTransaction = {}; // var product = {}; var user = {};

    userProductTransaction.ProductId = product_id;
    userProductTransaction.SubProductId = SubProductId;

    userProductTransaction.UserProductTransactionSpecifications = specifications;
    //	cart.CartProduct = product;
    //cart.CartUser = user;
    userProductTransaction.Quantity = quantity;
    var cartJson = { 'isRedirect': isRedirect, 'cart': userProductTransaction };
    if (quantity > 0 && quantity != "") {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/AddToCart",
            type: 'post',
            data: JSON.stringify(cartJson),
            //   data:  cartJson,
            dataType: 'json',
            success: function (data) {
                if (data.d.Status == "Success") {
                    GetCartProducts();
                    CartShortInfo();
                    $('#notification').empty();
                    ShowInfo(data.d, '#notification');
                    NavigateToMyCart();
                }
                if (data.d.Status == "NoData") {
                    alert('Out of stock. Available Qunatity is: ' + data.d.Result);
                    GetCartProducts();
                    CartShortInfo();
                    $('#notification').empty();
                    ShowInfo(data.d, '#notification');
                    NavigateToMyCart();
                }
                if (data.d.Status == "Fail") {
                    //resetting the quantity to its previus quantity
                    $('#txtQty').val(quantity);

                    $('#notification').empty();
                    var msg = json.d.Message + ". Notify Me <a onClick='vpb_show_login_box();'>Click</a>"
                    $('divstack').show();
                    ShowInfo(data, '#notification');
                    $('.success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }

            }

        });
    }

}

function SuggestedProductsPopup(Amount) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/GetSuggestedProducts?Amount=" + Amount,
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            $('.scrollToTop').click();
            $('#divSuggestedProducts').html(data.Result);
            $('.SuggestedPopupback, .SuggestedPopup').show();
        },
        error: function (x, y, z) {

        }
    });
}

function SuggestedOfferedProducts(Amount) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/GetSuggestedOfferedProducts?Amount=" + Amount,
        type: 'Get',
        dataType: 'json',
        success: function (data) {
            $('.scrollToTop').click();
            $('#divSuggestedOfferedProducts').html(data.Result);
            $('.SuggestedPopupback, .SuggestedOfferedProductsPopup').show();
        },
        error: function (x, y, z) {

        }
    });


}

function addOfferedProduct(OfferProductId) {
    $.ajax({
        url: siteURL + 'api/Master/AddOfferProducttoCart?ProductID=' + OfferProductId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('.SuggestedPopupback, .SuggestedOfferedProductsPopup').hide();
            data = data.Result;
            var trData = " <tr id='product_5_9_0_0' class='cart_item first_item address_0 odd'>";
            trData += "                         <td class='cart_product'>";
            trData += "                                  <img style='height:100px;' src='" + data.Product_Image + "' alt='" + data.Offer_Product_Name + "'>";
            trData += "                              </td>";
            trData += "                              <td class='cart_description hidden-phone'>";
            trData += "                                   <p class='s_title_block'>" + data.Offer_Product_Name + "";
            trData += "                           </p>";
            trData += "                          <!--Brand:   <a style='cursor:default'>" + data.Product_Brand + "</a>-->";
            trData += "                                 </td>";
            trData += "                              <td>";
            trData += "                            1";
            trData += "                          </td>";
            trData += "                              <td class='cart_unit'>";
            trData += "                                 <span style='font-weight:bold;'>" + data.Product_Actual_Cost + "</span>";
            trData += "                                 </td>";
            trData += "                            <td class='cart_total'> <span style='font-weight:bold;'>FREE</span>";
            trData += "                            </td>";
            trData += "                                        <td class='cart_delete'>";
            trData += "   <span style='font-weight:bold;'> - </span>";
            trData += "                            </td>";
            trData += "                                 </tr>";

            $('#cart_summary tbody').append(trData);

        }
    });
}



function addToCartbySubprod(product_id, SubProductId, quantity, isRedirect, thi) {
    quantity = $(thi).parent().parent().find('td').eq(1).find('#txtqt').val();
    quantity = typeof (quantity) != 'undefined' ? quantity : 1;
    //alert(quantity);
    isRedirect = typeof (isRedirect) != 'undefined' ? isRedirect : 0;
    specifications = typeof (specifications) != 'undefined' ? specifications : null;
    var userProductTransaction = {}; // var product = {}; var user = {};

    userProductTransaction.ProductId = product_id;
    userProductTransaction.SubProductId = SubProductId;

    userProductTransaction.UserProductTransactionSpecifications = specifications;
    //	cart.CartProduct = product;
    //cart.CartUser = user;
    userProductTransaction.Quantity = quantity;
    var cartJson = { 'isRedirect': isRedirect, 'cart': userProductTransaction };
    if (quantity > 0 && quantity != "") {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/AddToCart",
            type: 'post',
            data: JSON.stringify(cartJson),
            //   data:  cartJson,
            dataType: 'json',
            success: function (data) {
                if (data.d.Status == "Success") {
                    GetCartProducts();
                    CartShortInfo();
                    $('#notification').empty();
                    ShowInfo(data.d, '#notification');

                    $(thi).css({ "background": "#fff" }).attr("disabled", true);
                    $(thi).children('span').children('span').html("Added to Cart");
                    $(thi).parent('td').parent('tr').children('td:nth-child(2)').find('.down,.up').remove();
                    var jQueryadd = jQuery(thi);
                    var price = $('#lblPrice').text();
                    if (price == "") {
                        price = "0";
                    }
                    price = parseInt(price) + parseInt(jQueryadd.parent('td').prev('td').find('label').html().replace(/,/g, ""));
                    $('#lblPrice').text(price);
                    $('#DivTotal').show();
                    $('#lblProductcost').text("₹ " + price);
                    //$('#AddtoCartSub').val('Added To Cart');
                    //$('#AddtoCart').attr("disabled",false);
                    //NavigateToMyCart();
                }

                if (data.d.Status == "Fail") {
                    //resetting the quantity to its previus quantity
                    $('#txtQty').val(quantity);
                    $('#spancart').show();
                    $('#notification').empty();

                    ShowInfo(data, '#notification');
                    $('.success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }

            }

        });
    }

}

///write Reviews

function WriteProductReviews() {

    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        //  alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        var cartJson = { 'productId': getParameterByName("id"), 'UserId': json['UserId'], 'Review': $('#txtReview').val() };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "api/Master/WriteReview",
            type: 'post',
            data: JSON.stringify(cartJson),
            dataType: 'json',
            success: function (data) {

                if (data.Status == "Success") {
                    ShowInfo(data, '#notification');
                    //  CompareListShortInfo();
                    //  CompareDisPlayInfo();

                    $('.success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    $('#notification').empty();
                    $('#notification').html('<div id="success">   ' + data.Message + '</div>');
                    $("#txtReview").val('');
                    $('#tabs a').tabs();
                }
                if (data.Status == "Fail") {

                    ShowInfo(data, '#notification');
                }
            }
        });
    }
}
//End Reviews



function addToWishList(product_id) {


    if (checkWishListItemExist(product_id)) {
        $('.success, .warning, .attention, .information, .error').remove();
        $('#notification').html('<div class="success" style="display: none;">' + 'Selected product is already added to the WishList. <br> <a href="WishList.aspx"> Click here to Continue with existing WishList </a> <img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
        $('.success').fadeIn('slow');
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    }

    else {
        var cartJson = { 'productId': product_id };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/AddToWishList",
            type: 'post',
            data: JSON.stringify(cartJson),
            dataType: 'json',
            success: function (data) {
                if (data.d.Status == "Success") {
                    $('#notification').empty();
                    $('#notification').html('<div id="success" >' + data.d.Message + ' </div>');
                    //$('#notification').append(data.d.Message);
                    $('.Success').fadeIn('slow');
                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                    WishListShortInfo();
                }
                if (data.d.Status == "NoData") {

                    $('#wishlist-total').append(data.d.Result);
                    WishListShortInfo();
                }


            }
        });
    }
}


//GetcompareProducts
function GetCompareProductList() {
    $.ajax({
        url: siteURL + 'api/Master/GetCompareProductList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#compeProducts').empty();
            ShowInfo(data, '#compeProducts');
        }
    });
}

//End

function CheckCompareList(Dis, product_id) {

    if ($(Dis).attr("checked") == "checked") {
        AddToCompare(product_id)
    }
    else {
        deleteFromCompareList(product_id)
    }


}

function AddToCompare(product_id) {

    var cartJson = { 'ProductId': product_id, 'UserProductTransactionSpecifications': null };

    $.ajax({

        url: siteURL + "api/Master/AddToCompare",
        type: 'post',
        data: JSON.stringify(cartJson),
        headers: { "Content-Type": "application/json" },
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                ShowInfo(data, '#notification');
                //  CompareListShortInfo();
                CompareDisPlayInfo();
            }
            if (data.Status == "Fail") {

                ShowInfo(data, '#notification');
            }
        }
    });
}



function GetOrderOverview() {
    $.ajax({
        url: siteURL + 'api/Master/GetOrderOverview',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                btnProductOverview();
                $('#divOrderOverview').empty();
                ShowInfo(data, '#divOrderOverview');

            }
            if (data.Status == "NoData") {
                //                $("#lblcontinue").hide();
                //                $("#btnCheckout").hide();
                $('#divCart').empty();
                $("#divCartmsg").empty();
                $("#divCartmsg").append(data.Message);
            }
        }
    });
}


function GetCartProducts() {
    $.ajax({
        url: siteURL + 'api/Master/GetCartList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divCart').empty();
                ShowInfo(data, '#divCart');

            }
            if (data.Status == "NoData") {
                $('#divCart').empty();
                $("#divCartmsg").empty();
                $("#divCartmsg").append(data.Message);
                return data.Message;

            }
        }
    });
}

function CheckoutOrder() {
    $.ajax({
        url: siteURL + 'api/Master/OrderCheckOut',
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                btnProductOverview();
                $('#divOrderOverview').empty();
                ShowInfo(data, '#divOrderOverview');

            }
            if (data.Status == "NoData") {
                $('#divCart').empty();
                $("#divCartmsg").empty();
                $("#divCartmsg").append(data.Message);
            }
        }
    });
}


function GetStates() {
    var items = $("#txbShippingState option").length;
    if (items <= 0) {
        //Default item 

        var newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#txbShippingState').append(newOption);

        newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#txbStateBill').append(newOption);

        newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#txbEditShippingState').append(newOption);


        $.ajax({
            url: siteURL + 'api/Master/GetStates',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $.each(data.Result, function (index, item) {

                        newOption = $('<option>');
                        newOption.attr('value', item.StateId).text(item.StateName);
                        $('#txbShippingState').append(newOption);

                        newOption = $('<option>');
                        newOption.attr('value', item.StateId).text(item.StateName);
                        $('#txbStateBill').append(newOption);

                        newOption = $('<option>');
                        newOption.attr('value', item.StateId).text(item.StateName);
                        $('#txbEditShippingState').append(newOption);

                    });
                }
            }
        });
    }
}


function deleteFromCart(product_id, subproduct_id) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/DeleteFromCart?id=" + product_id + "&sid=" + subproduct_id,
        type: 'Get',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                GetCartProducts();
                CartShortInfo();
                GetOrderOverview();
                //                window.location.href = dis;
                //                NavigateToMyCart();
            }
            if (data.Status == "NoData") {
                $('#cart_summary').hide();
                $('#customer_cart_total').hide();
                $('#lblcontinue').hide();
                window.location.href = siteURL + "MyCart.aspx";
                //                $('#lblempty').show();
                //                window.location.href = "index.aspx";
                //                GetCartProducts();
                //                CartShortInfo();
                //                vpb_hide_popup_boxes();
                //   NavigateToMyCart();
                //                $("#divEmpty").append(data.Message);
                // ShowInfo(data, '#divCartmsg');
            }
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}

function CartShortInfo() {
    var CurrentURL = window.location.href;

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/CartShortInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#cart_total').empty();
                ShowInfo(data, '#cart_total');
            }
            if (data.Status == "NoData") {
                $('#cart_total').empty();
                $('#cart_total').append(data.Result);
            }
        }

    });
}
//displayCompareInfo
function CompareDisPlayInfo() {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/CompareDisPlayInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                $('#compareHeader').show();
                //GetCompareProductList();

                $('#compareInfo').empty();
                ShowInfo(data, '#compareInfo');
            }
            else if (data.Status == "NoData") {
                $('#compareHeader').hide();

                CompareListShortInfo();


            }
            if (data.Status == "Fail") {
                $('#compareHeader').hide();
                ShowInfo(data, '#divmsg');
            }
        },
        error: function (x, y, z) {
            $('#compareHeader').hide();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');


        }

    });
}
//End

function CompareListShortInfo() {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/CompareListShortInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (data) {
            $('#compareList').empty();
            ShowInfo(data, '#compareList');
        }

    });
}
function WishListShortInfo() {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/WishListShortInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#wishlist-total').empty();
                ShowInfo(data, '#wishlist-total');
            }
            if (data.Status == "NoData") {
                $('#wishlist-total').empty();
                $('#divMsg').append(data.Message);
                $('#divmessage').append('<div style="visibility:visible" class="attention">' + data.Message + '</div>');
                $('#wishlist-total').append(data.Result);
            }
        }

    });
}


function deleteFromWishList(product_id) {

    //  var cartJson = { 'id': product_id };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/deleteFromWishList?id=" + product_id,
        type: 'Get',
        //data: JSON.stringify(cartJson),
        dataType: 'json',
        headers: { "Content-Type": "application/json" },
        success: function (data) {
            if (data.Status == "Success") {
                GetWishListProducts();
                WishListShortInfo();

            }
            else {
                GetWishListProducts();
                WishListShortInfo();
                ShowInfo(data, '#divmsg');
            }
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}
function deleteFromCompareList(product_id) {

    //  var cartJson = { 'id': product_id };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/DeleteFromCompare?id=" + product_id,
        type: 'Get',
        //data: JSON.stringify(cartJson),
        dataType: 'json',
        headers: { "Content-Type": "application/json" },
        success: function (data) {
            //            $('input:checkbox[name=checkme]').attr('checked', false);
            if (data.Status == "Success") {
                $('#compareInfo').empty();
                //GetCompareProductList();
                CompareListShortInfo();
                CompareDisPlayInfo();

                $("#product-list div.compare input[id*='checkCompare']").each(function () {

                    if ($(this).attr('prodId') == product_id)
                        $(this).attr('checked', false);

                });

            }
            else if (data.Status == "NoData") {
                $('#compareHeader').hide();
                $('#compareInfo').empty();
                //GetCompareProductList();
                CompareDisPlayInfo();


                $("#product-list div.compare input[id*='checkCompare']").each(function () {

                    if ($(this).attr('prodId') == product_id)
                        $(this).attr('checked', false);

                });
            }
            if (data.Status == "Fail") {
                $('#compareHeader').hide();
                ShowInfo(data, '#divmsg');
            }
        },
        error: function (x, y, z) {
            $('#compareHeader').hide();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}
function GetWishListProducts() {
    //  $('div#last_msg_loader').html('<img src="catalog/view/theme/leisure/image/loading.gif">');

    //   psord = $("#ddlProductSort option:selected").val();
    //  prows = $('#all-product ul > li').length;

    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/ShowWishList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("#all-product").empty();

            $("#all-product").empty();
            if (data.Status == "Success") {
                ShowInfo(data, '#all-product');
            }
            if (data.Status == "NoData") {
                ShowInfo(data, "#divMsg");
                $('#divmessage').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
            }
            if (data.Status == "Fail") {
                ShowInfo(data, "#notification");
            }

        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }

    });
}





function WriteWishListResponse(products) {
    $('#product-list').empty();
    var strResult = "";
    // for (var i = 0; i < 20; i++) {

    if (products.length != 0) {


        $.each(products, function (index, item) {

            strResult += "<li><a href='" + siteURL + "product/" + item.ProductName.replaceAll(" ", "-") + "/" + item.ProductId + "' class='product_image'><img width='225px' height='350px'  src='" + item.ProductImgUrl.replace("~/", siteURL) + "' alt='" + item.ProductName + "' /></a>" +
                    "<div class='product_info'>" +
                    "<h3><a href='" + siteURL + "product/" + item.Product.ProductName.replaceAll(" ", "-") + "/" + item.ProductId + "'>" + item.ProductName + "</a></h3>" +
                    "   </div>" + //<small>" + item.ProductDescription + "</small> 
                    "<div class='price_info'> <a onClick='deleteFromWishList(" + item.ProductId + ");'>+ Remove From Wish List</a>" +
                    "<button class='price_add' title='' type='button' onClick='NavigateToDetails(" + item.ProductId + ");'>" +
                    "<span class='pr_price'>" + item.ProductCost + "</span>" +
                    "<span class='pr_add'>Add to Cart</span></button>" +
                    "</div></li>";

        });
    }
    else {
        //        strResult = strResult + "<div class='success' ><p>No products currently in list</p></div>";

    }

    $('#product-list').append(strResult);

    $('div#last_msg_loader').empty();
}

function NavigateToDetails(productId) {
    window.location.href = siteURL + 'details.aspx?id=' + productId;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}



function WriteResponse(categories) {
    var strResult = "";
    $.each(categories, function (index, category) {
        strResult += "<li class='menu_cont'>";
        if (index == 0)
            strResult += "<a href='#' title='" + category.CategoryId + "' onclick='SearchByCategory(this," + category.CategoryId + ")' class='active'>" + category.CategoryName + "</a><ul class='side_sub_menu' style='display: block;'>";
        else
            strResult += "<a href='#' title='" + category.CategoryId + "' onclick='SearchByCategory(this," + category.CategoryId + ")' class=''>" + category.CategoryName + "</a><ul class='side_sub_menu'>";

        $.each(category.SubCategories, function (index1, item) {
            strResult += "<li><a href='#' onclick='SearchBySubCategory(" + item.CategoryId + "," + item.SubCategoryId + ")'> - " + item.SubCategoryName + "</a></li>";
        });
        strResult += "</ul></li>";
    });

    $("ul.category.departments").html(strResult);
    //  $('ul.primary_nav li').children('a.active')
    pcategoryId = $('ul.primary_nav').children('li.active').attr('catid');
    GetProducts();
}
//End Categories & Sub-Categories

//Begin Products

//function checkPage() {
//    var filename = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
//    if (filename.contains('index.aspx')) {
//        return false;
//    }
//    else {
//        return true;
//    }
//}

function checkPage() {
    var filename = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
    if (filename.indexOf('index.aspx') > -1) {

        //Uncaught TypeError: Object index.aspx has no method 'contains' (repeated 2 times)
        return false;
    }
    else {
        return true;
    }
}


function SortBy() {
    $('#product-list').empty();
    GetProducts();
}





//End Products

function GetProduct() {

    $('#main').empty();
    pid = getParameterByName("id");
    if (pid == undefined || pid == null || pid == "") {
        $('#main').html(" Please select the Product to see the details. ");
    }
    else {
        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/GetProductInfo?productId=' + pid,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#main').empty();
                    // var myDiv = $.create("div");
                    //    var myDiv = $('<div />')
                    //   var decoded = myDiv.html(data.Result).text();
                    $('#main').html(data.Result);

                    ScorlImage();
                    jQuery('.cloud-zoom, .cloud-zoom-gallery').CloudZoom();
                    // tabs();
                    $('#tabs a').tabs();
                    // $('.fancybox').fancybox({ cyclic: true });
                    // ShowInfo(decoded, '#padding20');
                    //  WriteProductResponse(data); 
                }
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
}

//Begin Categories & Sub-Categories
function GetAll() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResp(data);

        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}

function WriteResp(categories) {
    var strResult = "";
    var selectedCategoryNames = "";
    $.each(categories, function (index, category) {
        strResult += "<li>";
        if (index == 0)
            strResult += "<a href='#' title='" + category.CategoryId + "' onclick='SearchByCategory(this," + category.CategoryId + ")' class='active'>" + category.CategoryName + "</a><ul class='sub_menu' style='display: block;'><li>";
        else
            strResult += "<a href='#' title='" + category.CategoryId + "' onclick='SearchByCategory(this," + category.CategoryId + ")' class=''>" + category.CategoryName + "</a><ul class='sub_menu'><li>";

        $.each(category.SubCategories, function (index1, item) {
            strResult += "<ul><li><a href='#' onclick='SearchBySubCategory(" + item.CategoryId + "," + item.SubCategoryId + ")'> - " + item.SubCategoryName + "</a></li></ul>";
        });
        strResult += "</li></ul></li>";
    });



    $("ul.primary_nav").html(strResult);

}
//End Categories & Sub-Categories





// Begin GetBrandNames GetBrandNames(pcategoryId, psubCategoryId);


function GetBrandNames(pcategoryId, psubCategoryId) {

    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetBrands?pcategoryId=' + pcategoryId + '&subCategories=' + psubCategoryId,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            WriteBrands(data);
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });
}

function WriteBrands(data) {
    $("#dvBrand").empty();
    ShowInfo(data, "#dvBrand");
}
//End GetBrandNames


//Payment

var merchantURLPart = "https://www.citruspay.com/healthurwealth";
//var marchantPaytmUrlpart = "https://pguat.paytm.com/oltp-web/processTransaction";
var marchantPaytmUrlpart = "https://secure.paytm.in/oltp-web/processTransaction";
var vanityURLPart = "";
var reqObj = null;
function generateHMAC() {
    if (window.XMLHttpRequest) {
        reqObj = new XMLHttpRequest();
    } else {
        reqObj = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (merchantURLPart.lastIndexOf("/") != -1) {
        vanityURLPart = merchantURLPart.substring(merchantURLPart.lastIndexOf("/") + 1)
    }
    var orderAmount = document.getElementById("orderAmount").value;
    var merchantTxnId = document.getElementById("merchantTxnId").value;
    var currency = document.getElementById("Ordercurrency").value;
    var param = "merchantId=" + vanityURLPart + "&orderAmount=" + orderAmount
				+ "&merchantTxnId=" + merchantTxnId + "&currency=" + currency;
    reqObj.onreadystatechange = process;
    reqObj.open("POST", "hmac_signature.aspx?" + param, false);
    reqObj.send(null);
}

function generatePAYTM() {
    if (window.XMLHttpRequest) {
        reqObj = new XMLHttpRequest();
    } else {
        reqObj = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (marchantPaytmUrlpart.lastIndexOf("/") != -1) {
        vanityURLPart = marchantPaytmUrlpart.substring(marchantPaytmUrlpart.lastIndexOf("/") + 1)
    }
    var MID = "SonalE04048888945700";//"SonalE12191086990327";
    var RequestType = "DEFAULT";
    var userId = document.getElementById("userId").value;
    var orderAmount = document.getElementById("orderAmount").value;
    var merchantTxnId = document.getElementById("merchantTxnId").value;
    var currency = document.getElementById("Ordercurrency").value;
    var checksum = document.getElementById("checkSum").value;
    var param = "REQUEST_TYPE=" + RequestType + "&MID=" + MID + "&ORDER_ID=" + merchantTxnId + "&CUST_ID=" + userId + "&TXN_AMOUNT=" + orderAmount + "&CHANNEL_ID=WEB&INDUSTRY_TYPE_ID=Retail109&WEBSITE=SonalEWEB&MOBILE_NO=8885044001&EMAIL=test@g.com&CHECKSUMHASH=" + checksum;
    // reqObj.onreadystatechange = process1;
    //marchantPaytmUrlpart = "https://pguat.paytm.com/oltp-web/processTransaction?" + param;
    marchantPaytmUrlpart = "https://secure.paytm.in/oltp-web/processTransaction?" + param;
    submitPaytmForm(marchantPaytmUrlpart);
    // reqObj.open("POST", marchantPaytmUrlpart);
    // reqObj.send();
}

function process() {
    if (reqObj.readyState == 4) {
        document.getElementById("secSignature").value = reqObj.responseText;
        submitForm();
    }
}

function process1() {
    if (reqObj.readyState == 4) {
        document.getElementById("secSignature").value = reqObj.responseText;
        submitPaytmForm();
    }
}

function submitPaytmForm(marchantPaytmUrlpart) {
    //var form = document.createElement("form");

    //form.method = "POST";
    //form.action = marchantPaytmUrlpart;
    $("#form1").attr('action', marchantPaytmUrlpart);
    $("#form1").attr('method', 'POST');
    $('#form1').submit();
    //form.submit();
    //   alert('hi');
}

function submitForm() {
    $("#form1").attr('action', merchantURLPart);
    $("#form1").attr('method', 'POST');
    $('#form1').submit();
    //   alert('hi');
}

function RepaymentgenerateHMAC(data) {
    document.getElementById("orderAmount").value = data.TxnAmount;
    document.getElementById("merchantTxnId").value = data.PaymentTransactionId;
    document.getElementById("firstName").value = data.FirstName;
    document.getElementById("email").value = data.EmailId;
    document.getElementById("lastName").value = data.LastName;
    document.getElementById("phoneNumber").value = data.MobileNo;
    document.getElementById("addressStreet1").value = data.StreetAddress1;
    document.getElementById("addressCity").value = data.City;
    document.getElementById("addressZip").value = data.PinCode;
    document.getElementById("addressState").value = data.StateName;
    document.getElementById("addressStreet2").value = data.LandMark;

    if (window.XMLHttpRequest) {
        reqObj = new XMLHttpRequest();
    } else {
        reqObj = new ActiveXObject("Microsoft.XMLHTTP");
    }
    if (merchantURLPart.lastIndexOf("/") != -1) {
        vanityURLPart = merchantURLPart.substring(merchantURLPart.lastIndexOf("/") + 1)
    }
    var orderAmount = document.getElementById("orderAmount").value;
    var merchantTxnId = document.getElementById("merchantTxnId").value;
    var currency = document.getElementById("Ordercurrency").value;
    var param = "merchantId=" + vanityURLPart + "&orderAmount=" + orderAmount
				+ "&merchantTxnId=" + merchantTxnId + "&currency=" + currency;
    reqObj.onreadystatechange = process;
    reqObj.open("POST", "hmac_signature.aspx?" + param, false);
    reqObj.send(null);
}


function ReOrderProcess(OrderID) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/MakeRepayment?OrderID=" + OrderID,
        type: 'post',
        data: '{}',
        dataType: 'json',
        success: function (json) {
            json = json.Result;
            RepaymentgenerateHMAC(json);
        }
    });
}


function MakePayment(product_id, quantity, isRedirect, specifications) {
    $("#btnMakepayment").attr("disabled", true);

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... We are processing your order.</span>";
    document.body.appendChild(div);

    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/MakePaymentCitrus",
            type: 'post',
            data: '{}',
            dataType: 'json',
            success: function (json) {
                json = json.d;
                document.getElementById("merchantTxnId").value = json['id'];
                generateHMAC();
            }
        });
    }
}

function MakePaytm(product_id, quantity, isRedirect, specifications) {
    $("#btnPaytm").attr("disabled", true);

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... We are processing your order.</span>";
    document.body.appendChild(div);

    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/MakePaymentPaytm",
            type: 'post',
            data: '{}',
            dataType: 'json',
            success: function (json) {
                json = json.d;
                document.getElementById("merchantTxnId").value = json['id'];
                document.getElementById("userId").value = json['userId'];
                document.getElementById("checkSum").value = json['checkSum'];
                generatePAYTM();
            }
        });
    }
}

function Promocode() {
    var PromocodeText = $('#txtPromocode').val();
    var orderAmount = $('#total_product').text();
    // alert('From ' + orderAmount);
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/CheckPromoCode?PromoCode=" + PromocodeText + "&OrderAmount=" + orderAmount,
        type: 'Post',
        success: function (data) {
            if (data.Status == 'Success') {
                var x = document.getElementById('lblPromocode');
                x.style.color = 'Green';
                x.style.fontWeight = 'Bold';
                x.innerHTML = data.Message;
                alert(data.Message);
                window.location.href = siteURL + "MyCart.aspx";
                //document.getElementById("orderAmount").value = parseFloat(data.Result).toFixed(2);
            }
            else {
                var x = document.getElementById('lblPromocode');
                x.style.color = 'Red';
                x.style.fontWeight = 'Bold';
                x.innerHTML = data.Message;
                //document.getElementById("orderAmount").value = parseFloat(data.Result).toFixed(2);
            }

        }
    });
}

function CheckLogin() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/GetUserSession",
        type: 'post',
        data: '{}',
        async: false,
        dataType: 'json',
        success: function (json) {
            data = json.d;

        }
    });
    return data;
}

function CheckCartCount() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/Checkcartcount",
        type: 'post',
        data: '{}',
        async: false,
        dataType: 'json',
        success: function (json) {
            data = json.d;

        }
    });
    return data;
}
//Checkout CartSession
function CheckCart() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/GetCartSession",
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data == 0) {
                window.location = siteURL + "index.aspx";
            }
            else {
                window.location = siteURL + "CheckOut.aspx";
            }

        }
    });
    return data;
}
//End CartSession

//Check CartInfo
function CheckCartInfo() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/GetCartSession",
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data == '0' || data == null || data == undefined) {
                window.location = "index.aspx";
            }
            else {
                // window.location = "MyCart.aspx";
            }

        }
    });
    return data;
}

//End Cart Info

function IsAddressAssignedToCart() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/GetUserCartAddresses",
        type: 'Get',
        data: '{}',
        async: false,
        dataType: 'json',
        success: function (json) {
            data = json;
        }
    });
    return data;
}


function fillData() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        //    alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        //  alert(location.protocol + "//" + location.host + "/PaymentStatus.aspx");
        document.getElementById("returnUrl").value = location.protocol + "//" + location.host + "/PaymentStatus.aspx";
        //  alert(document.getElementById("returnUrl").value); 

        document.getElementById("firstName").value = json['firstName'];
        document.getElementById("lblFirstName").innerHTML = json['firstName'];

        document.getElementById("lastName").value = json['lastName'];
        document.getElementById("lblLastName").innerHTML = json['lastName'];

        document.getElementById("email").value = json['email'];
        document.getElementById("lblEmailID").innerHTML = json['email'];

        document.getElementById("phoneNumber").value = json['phoneNumber'];
        document.getElementById("lblMobileNo").innerHTML = json['phoneNumber'];

        //   document.getElementById("orderAmount").value = json['orderAmount'];
        document.getElementById("addressStreet1").value = json['addressStreet1'];
        document.getElementById("lblAddress").innerHTML = json['addressStreet1'];

        document.getElementById("addressCity").value = json['addressCity'];
        document.getElementById("lblCitys").innerHTML = json['addressCity'];

        document.getElementById("addressState").value = json['addressState'];
        document.getElementById("lblStatesss").innerHTML = json['addressState'];

        document.getElementById("addressZip").value = json['addressZip'];
        document.getElementById("lblZipCode").innerHTML = json['addressZip'];

        //        document.getElementById("orderAmount").value = parseFloat(json['CurrencyValue'] * json['orderAmount']).toFixed(2);
        document.getElementById("orderAmount").value = parseFloat(json['orderAmount']).toFixed(2);
        document.getElementById("lblAmount").innerHTML = parseFloat(json['orderAmount']).toFixed(2);

        document.getElementById("Ordercurrency").value = json['CurrencyCode'];
        document.getElementById("lblCurrency").innerHTML = json['CurrencyCode'];

        document.getElementById("lblUserID").value = json['UserId'];

    }
}

//End payment

//Currency

function GetCurrency() {
    var data;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/GetCurrency",
        type: 'post',
        async: false,
        data: '{}',
        dataType: 'json',
        success: function (json) {
            json = json.d;
            data = json;

        }
    });
    return data;
}

//function GetCurrency() {
//    var data;
//    $.ajax({
//        contentType: "application/json; charset=utf-8",
//        url: siteurl+"PersonService.asmx/GetCurrency",
//        type: 'post',
//        async: false,
//        data: '{}',
//        dataType: 'json',
//        success: function (json) {
//            json = json.d;
//            data = json;

//        }
//    });
//    return data;
//}
//End Currency

// UserProductTransactions
function UserProductTransactionsGrid() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        $("#list").jqGrid({
            datatype: 'json',
            url: siteURL + 'api/Master/GetUserProductTransactionList?id=' + json['UserId'],
            jsonReader: { repeatitems: false },
            loadui: "block",
            key: "TransactionId",
            mtype: 'GET',
            rowNum: 5,
            autosize: true,
            rowList: [5, 10, 20, 30],
            viewrecords: true,
            colNames: ['Order Id', 'Transaction Id', 'Products', 'Quantity', 'Currency', 'Amount', 'Message', 'Status', 'CreatedOn', 'Action'],
            colModel: [
              { name: 'PaymentTransactionId', index: 'PaymentTransactionId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false },
                    { name: 'PGTxnId', index: 'PGTxnId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false },
                    { name: 'products', index: 'products', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'Quantity', index: 'Quantity', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                   { name: 'currency', index: 'currency', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                     { name: 'TxnAmount', index: 'TxnAmount', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false },
                       { name: 'TxnMessage', index: 'TxnMessage', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false, visable: true },
                    { name: 'TxnStatus', index: 'TxnStatus', width: 100, editable: true, editrules: { required: true }, edittype: 'text', visable: true },
                    {
                        name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', datatype: 'local', formatoptions: { srcformat: 'Y-m-dTH:i:s', newformat: 'Y/m/d H:i:s' },

                        editoptions: {
                            size: 12, dataInit: function (el) {
                                setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "Y/m/d H:i:s"); }, 300);
                            }
                        }, sorttype: "date"
                    },
             { name: 'PaymentTransactionId', index: 'PaymentTransactionId', edittype: 'select', width: 130, editable: false, sortable: false, formatter: returnMyLink }],
            pager: '#pager',
            emptyrecords: "No transactions found",
            sortname: 'TakenOn',
            sortorder: 'CreatedOn',
            height: "100%",
            width: "100%",
            prmNames: { nd: null, search: null }, // we switch of data caching on the server
            // and not use _search parameter
            caption: 'UserProductTransaction Records',
            gridComplete: function () {
                var ids = jQuery("#list").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    be = "<input style='height:22px;' type='button' value='Edit' onclick=\"window.location.href='OrderDetails.aspx?ID=" + cl + "'\"  />";
                    jQuery("#list").jqGrid('setRowData', ids[i], { Action: be });
                }
            }
        });

        $("#list").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, view: true });
    }
}
function ShowImageFormatter(cellvalue, options, rowObject) {
    return "<img src='" + cellvalue.replace("~/", siteURL) + "' alt='" + rowObject["ProductName"] + "'  />";
};



//End UserProductTransactions

function returnMyLink(cellValue, options, rowdata) {
    return "<a  class='active' href='OrderDetails.aspx?ID=" + cellValue + "' >  [View] </a>";

}

//Address Binding logic
function ShippingToBilling() {
    $('#txtAddress1Bill').val($('#txbShippingAddress1').val());
    $('#txtAddress2Bill').val($('#txbShippingAddress2').val());
    $('#txtLandmarkBill').val($('#txbShippingLandmark').val());
    $('#txtCityBill').val($('#txbShippingCity').val());
    $('#ddlCountryBill').val($('#ddlShippingCountry').val());
    $('#txbStateBill').val($('#txbShippingState').val());
    $('#txtPincodeBill').val($('#txbShippingPincode').val());
    $('#BillingAddressId').val($('#ShippingAddressId').val());
    $('#txtAddress1Bill').attr('readonly', true);
    $('#txtAddress2Bill').attr('readonly', true);
    $('#txtLandmarkBill').attr('readonly', true);
    $('#txtCityBill').attr('readonly', true);
    $('#ddlCountryBill').attr('readonly', true);
    $('#txbStateBill').attr('readonly', true);
    $('#txtPincodeBill').attr('readonly', true);
}
function ClearShipping() {
    $('#ShippingAddressId').val('');
    $('#txbShippingAddress1').val('');
    $('#txbShippingAddress2').val('');
    $('#txbShippingLandmark').val('');
    $('#txbShippingCity').val('');
    $('#ddlShippingCountry').val('');
    $("#txbShippingState").val('');

    $('#txbShippingPincode').val('');
    $("#PincodeAvailMsg").empty();

    $('#txbShippingAddress1').attr('readonly', false);
    $('#txbShippingAddress2').attr('readonly', false);
    $('#txbShippingLandmark').attr('readonly', false);
    $('#txbShippingCity').attr('readonly', false);
    $('#ddlShippingCountry').attr('readonly', false);
    $('#txbShippingState').attr('readonly', false);
    $('#txbShippingPincode').attr('readonly', false);
}
function ClearBilling() {
    $('#BillingAddressId').val('');
    $('#txtAddress1Bill').val('');
    $('#txtAddress2Bill').val('');
    $('#txtLandmarkBill').val('');
    $('#txtCityBill').val('');
    $('#txbStateBill').val(0);

    $("#ddlCountryBill").val('');

    $('#txtPincodeBill').val('');
    $('#txtAddress1Bill').attr('readonly', false);
    $('#txtAddress2Bill').attr('readonly', false);
    $('#txtLandmarkBill').attr('readonly', false);
    $('#txtCityBill').attr('readonly', false);
    $('#ddlCountryBill').attr('readonly', false);
    $('#txbStateBill').attr('readonly', false);
    $('#txtPincodeBill').attr('readonly', false);
}

function CheckboxDisable() {
    var Address1 = $('#txbShippingAddress1').val();
    var Address2 = $('#txbShippingAddress2').val();
    var City = $('#txbShippingCity').val();

    var State = $('#txbShippingState option:selected').val();
    var Country = $('#ddlShippingCountry option:selected').val();

    //if (Address1 == "" && Address2 == "" && City == "" && State == 0 && Country == 0) {
    //    $("#chkBillingAsShipping").prop('disabled', true);
    //}
    //else {
    //    $("#chkBillingAsShipping").prop('disabled', false);
    //}

}


function checkDropdownSelect() {
    $('.dropinput option').each(function () {
        //$("#chkBillingAsShipping").prop('disabled', true);
        if ($(this).attr('selected') == 'selected') {
            selectedItem = $(this).val();
            var State = $('#txbShippingState option:selected').val();
            var Country = $('#ddlShippingCountry option:selected').val();
            var Address1 = $('#txbShippingAddress1').val();
            var Country = $('#ddlShippingCountry').val();
            var pincode = $('#txbShippingPincode').val();
            //if (State == "" || Country == "" || Address1 == "" || City == "" || pincode == "") {

            //    $("#chkBillingAsShipping").prop('disabled', true);
            //}
            //else {

            //    $("#chkBillingAsShipping").prop('disabled', false);

            //}
        }
    });
}

function checkTextboxesEmpty() {

    $('div#Shipping').find("input[type=text]").each(function () {

        //$("#chkBillingAsShipping").prop('disabled', true);
        if ($(this).val() === '') {
            var Address1 = $('#txbShippingAddress1').val();
            var City = $('#txbShippingCity').val();

            var State = $('#txbShippingState option:selected').val();
            var Country = $('#ddlShippingCountry option:selected').val();
            var pincode = $('#txbShippingPincode').val();
            //if (Address1 == "" || City == "" || pincode == "" || State == "" || Country == "") {
            //    $("#chkBillingAsShipping").prop('disabled', true);
            //}

        }
        else {

            //$("#chkBillingAsShipping").prop('disabled', false);
            //checkDropdownSelect();
        }
    });

}


function LoadAddressLogic() {

    $('#NewBillingAddress').hide();
    $('#chkBillingAsShipping').click(function () {
        var checked = $(this).is(':checked');
        //ClearBilling();
        if (checked) {
            $('input[class=BillingAddress][type=checkbox]').each(function () {
                this.checked = false;
            });
            $('#NewBillingAddress').show();
            //ShippingToBilling();
        }
        else {
            $('#NewBillingAddress').hide();
            //ClearBilling();
        }
    });


    $('input[class=BillingAddress][type=checkbox]').click(function () {
        ClearBilling();

        var checked = $(this).is(':checked');
        $('input[class=BillingAddress][type=checkbox]').each(function () {
            this.checked = false;
        });

        if (checked) {
            $('#chkBillingAsShipping').prop('checked', false);

            this.checked = true;
            CheckboxDisable();
            var tbl = $(this).closest("div .tradus-select-user-address-bg");

            $('#BillingAddressId').val($(this).closest('div .tradus-select-address').find("input[id*='hdnBillingAddId']").val().trim());

            $('#txtAddress1Bill').val(tbl.find('span').find("label[id*='lblAdd1']").text().trim());
            $('#txtAddress2Bill').val(tbl.find('span').find("label[id*='lblAdd2']").text().trim());
            $('#txtLandmarkBill').val(tbl.find('span').find("label[id*='lblLandMark']").text().trim());
            $('#txtCityBill').val(tbl.find('span').find("label[id*='lblCity']").text().trim());




            //$('#ddlCountryBill').html(tbl.find('span').find("label[id*='lblCountry']").text().trim());
            //  $('#txbStateBill').val(tbl.find('span').find("label[id*='lblState']").text().trim());
            $('#txtPincodeBill').val(tbl.find('span').find("label[id*='lblPinCode']").text().trim());

            //            var BillPincode = tbl.find('span').find("label[id*='lblPinCode']").text().trim();
            //            Pincodecheck(BillPincode);           

            var country = tbl.find('span').find("label[id*='lblCountry']").text().trim();
            var state = tbl.find('span').find("label[id*='lblState']").text().trim();



            var arry = state.split(':');
            var arry2 = country.split(':');
            $("#txbStateBill option:contains(" + arry[1] + ")").attr('selected', 'selected');
            $("#ddlCountryBill option:contains(" + arry2[1] + ")").attr('selected', 'selected');

            $('#txtAddress1Bill').attr('readonly', true);
            $('#txtAddress2Bill').attr('readonly', true);
            $('#txtLandmarkBill').attr('readonly', false);
            //  $('#txtCityBill').attr('readonly', true);
            $('#ddlCountryBill').attr('readonly', true);
            //  $('#txbStateBill').attr('readonly', true);
            $('#txtPincodeBill').attr('readonly', true);

        } else {
            $('#chkBillingAsShipping').prop('checked', false);
            // ShippingToBilling();
        }


    });
    fillDetails();
    function fillDetails() {
        var tbl = $('input[class=ShippingAddress][type=checkbox]:first').closest("div .tradus-select-user-address-bg");

        $('#ShippingAddressId').val($('input[class=ShippingAddress][type=checkbox]:first').closest('div .tradus-select-address').find("input[id*='hdnShippingAddId']").val().trim());
        $('#txbShippingAddress1').val(tbl.find('span').find("label[id*='lblAdd1']").text().trim());
        $('#txbShippingAddress2').val(tbl.find('span').find("label[id*='lblAdd2']").text().trim());
        $('#txbShippingLandmark').val(tbl.find('span').find("label[id*='lblLandMark']").text().trim());
        $('#txbShippingCity').val(tbl.find('span').find("label[id*='lblCity']").text().trim());
        var country = tbl.find('span').find("label[id*='lblCountry']").text().trim();
        var state = tbl.find('span').find("label[id*='lblState']").text().trim();
        var arry = state.split(':');
        var arry2 = country.split(':');
        //$('#txbShippingState').find("option:contains(" + state + ")").prop('selected', true);
        $("#txbShippingState option:contains(" + arry[1] + ")").attr('selected', 'selected');
        $("#ddlShippingCountry option:contains(" + arry2[1] + ")").attr('selected', 'selected');

        $('#txbShippingPincode').val(tbl.find('span').find("label[id*='lblPinCode']").text().trim());

        var ShipngPincode = tbl.find('span').find("label[id*='lblPinCode']").text().trim();
        Pincodecheck(ShipngPincode);


        $('#txbShippingAddress1').attr('readonly', true);
        $('#txbShippingAddress2').attr('readonly', true);
        $('#txbShippingLandmark').attr('readonly', true);
        $('#txbShippingCity').attr('readonly', true);
        $('#ddlShippingCountry').attr('readonly', true);
        $('#txbShippingState').attr('readonly', true);
        $('#txbShippingPincode').attr('readonly', true);
        CheckboxDisable();
    }
    $('input[class=ShippingAddress][type=checkbox]').click(function () {

        $('#chkBillingAsShipping').removeAttr('checked');
        //ClearBilling();
        ClearShipping();

        var checked = $(this).is(':checked');
        $('input[class=ShippingAddress][type=checkbox]').each(function () {
            this.checked = false;
        });
        if (checked) {
            this.checked = true;
            var tbl = $(this).closest("div .tradus-select-user-address-bg");

            $('#ShippingAddressId').val($(this).closest('div .tradus-select-address').find("input[id*='hdnShippingAddId']").val().trim());
            $('#txbShippingAddress1').val(tbl.find('span').find("label[id*='lblAdd1']").text().trim());
            $('#txbShippingAddress2').val(tbl.find('span').find("label[id*='lblAdd2']").text().trim());
            $('#txbShippingLandmark').val(tbl.find('span').find("label[id*='lblLandMark']").text().trim());
            $('#txbShippingCity').val(tbl.find('span').find("label[id*='lblCity']").text().trim());
            var country = tbl.find('span').find("label[id*='lblCountry']").text().trim();
            var state = tbl.find('span').find("label[id*='lblState']").text().trim();
            var arry = state.split(':');
            var arry2 = country.split(':');
            //$('#txbShippingState').find("option:contains(" + state + ")").prop('selected', true);
            $("#txbShippingState option:contains(" + arry[1] + ")").attr('selected', 'selected');
            $("#ddlShippingCountry option:contains(" + arry2[1] + ")").attr('selected', 'selected');

            $('#txbShippingPincode').val(tbl.find('span').find("label[id*='lblPinCode']").text().trim());

            var ShipngPincode = tbl.find('span').find("label[id*='lblPinCode']").text().trim();
            Pincodecheck(ShipngPincode);


            $('#txbShippingAddress1').attr('readonly', true);
            $('#txbShippingAddress2').attr('readonly', true);
            $('#txbShippingLandmark').attr('readonly', true);
            $('#txbShippingCity').attr('readonly', true);
            $('#ddlShippingCountry').attr('readonly', true);
            $('#txbShippingState').attr('readonly', true);
            $('#txbShippingPincode').attr('readonly', true);
            CheckboxDisable();
        }
        else {
            $("#chkBillingAsShipping").prop('disabled', true);
            $('#chkBillingAsShipping').removeAttr('checked');
            //  ClearBilling();

        }
    });

}

function vpb_show_sign_up_box1(dis, id) {
    $("#vpb_pop_up_background1").css({
        "opacity": "0.4"
    });
    var tbl = $(dis).closest("div .tradus-select-user-address-bg");

    $('#BillingAddressId').val(id); //tbl.find("input[id*='hdnBillingAddId']").val().trim()
    $('#addressId').val(id);


    $('#txbEditShippingAddress1').val(tbl.find('span').find("label[id*='lblAdd1']").text().trim());
    $('#txbEditShippingAddress2').val(tbl.find('span').find("label[id*='lblAdd2']").text().trim());
    $('#txbEditShippingLandmark').val(tbl.find('span').find("label[id*='lblLandMark']").text().trim());
    $('#txbEditShippingCity').val(tbl.find('span').find("label[id*='lblCity']").text().trim());

    var country = tbl.find('span').find("label[id*='lblCountry']").text().trim();
    var state = tbl.find('span').find("label[id*='lblState']").text().trim();
    var arry = state.split(':');
    var arry2 = country.split(':');
    //$('#txbShippingState').find("option:contains(" + state + ")").prop('selected', true);
    $("#txbEditShippingState option:contains(" + arry[1] + ")").attr('selected', 'selected');
    $("#ddlEditShippingCountry option:contains(" + arry2[1] + ")").attr('selected', 'selected');
    //$("#txbEditShippingState option:contains(" + state + ")").attr('selected', 'selected');
    //$("#ddlEditShippingCountry option:contains(" + country + ")").attr('selected', 'selected');

    // $('#ddlEditShippingCountry').val(tbl.find('span').find("label[id*='lblCountry']").text().trim());
    // $('#txbEditShippingState').val(tbl.find('span').find("label[id*='lblState']").text().trim());
    $('#txbEditShippingPincode').val(tbl.find('span').find("label[id*='lblPinCode']").text().trim());
    $("#vpb_pop_up_background1").fadeIn("slow");
    $("#vpb_signup_pop_up_box1").fadeIn('fast');
    window.scroll(0, 0);
}


function NavigateToCheckout() {
    window.location.href = "CheckOut.aspx";
}

function NavigateToMyCart() {
    $("#btnbuynow").attr("disabled", "disabled");
    window.location.href = location.origin + "/MyCart.aspx";
}

function NavigateToMyTransactionspage() {
    var json = CheckLogin();
    if (json == undefined || json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        window.location = "Userlogin.aspx";
    }
    else {
        window.location = "MyTransactions.aspx?ID=" + json['UserId'];
    }

}


//End of address Binding logic
function removeRelatedProduct(productId) {
    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/deleteFromRelatedProductList?id=' + productId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            displayCart();
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}



function SearchRelatedProducts() {
    var searchText = $('#txtRelatedProducts').val();
    if (searchText.length > 1) {
        var CurrentURL = window.location.href;

        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/SearchAutoCompletList?sord=' + searchText,
            type: 'GET',
            dataType: 'json',
            async: true,
            cache: false,
            success: function (data) {
                $('#Reproduct-list').empty();
                $('#Reproduct-list').hide();
                if (data.Status == "NoData") {
                    $('#Reproduct-list').html('<div><p>No More Products.</p></div>');
                }
                else {
                    $('#Reproduct-list').show();
                    $('#Reproduct-list').empty();
                    $('#Reproduct-list').css({ display: "block" });
                    ShowInfo(data, '#Reproduct-list');

                    $('div#last_msg_loader').empty();
                }
            },
            error: function (x, y, z) {
                var data;
                ShowInfo(data, '');
            }
        });
    }
    $('#Reproduct-list').empty();
}





function addToRelatedProduct(product_id) {

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "api/Master/AddToRelatedProductList?id=" + product_id,
        type: 'GET',
        //data: JSON.stringify(cartJson),
        //   data:  cartJson,
        dataType: 'json',
        success: function (json) {
            displayCart();
        }

    });
}

//function displayCart() {

//    jQuery.support.cors = true;
//    $.ajax({
//        contentType: "application/json; charset=utf-8",
//        url:  siteURL+'api/Master/ShowRelatedProductsList',
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {

//            if (data.Status == "NoData") {
//                $('#last_msg_loader').html('<div><p>No More Products.</p></div>');
//            }
//            else {
//                $('#DisplayRelatedProductList').empty();
//                ShowInfo(data, '#DisplayRelatedProductList');

//                //    $("img").lazyload({ threshold: "200", effect: "fadeIn", effectspeed: 2000 });

//                //                $("img.lazyload").each(function () {
//                //                    $(this).attr("src", $(this).attr("original"));
//                //                    $(this).removeAttr("original");
//                //                });

//                $('div#last_msg_loader').empty();
//                // loading = true;
//            }

//        },
//        error: function (x, y, z) {
//            $('.success, .warning, .attention, .information, .error').remove();
//            $('.error').fadeIn('slow');
//        }


//    });
//}


function GetQuckInfo() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetQuckInfo?productId=' + 116,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#quickInfo').empty();
            // var myDiv = $.create("div");
            //    var myDiv = $('<div />')
            //   var decoded = myDiv.html(data.Result).text();
            $('#quickInfo').html(data.Result);

            // ScorlImage();
            // jQuery('.cloud-zoom, .cloud-zoom-gallery').CloudZoom();
            // tabs();
            $('#tabs a').tabs();
            // $('.fancybox').fancybox({ cyclic: true });
            // ShowInfo(decoded, '#padding20');
            //  WriteProductResponse(data); 
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

//Check Review Login
function btnReviewClicked() {

    var cartJson = { 'EmailId': $("#txtUserName").val(), 'PassCode': $("#txtPwd").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/UserLogin',
        type: 'post',
        dataType: "json",
        data: JSON.stringify(cartJson),
        success: function (data) {
            if (data.Status == "Success") {

                vpb_hide_popup_boxes();
                Loginink();
            }
            if (data.Status == "Fail") {
                $("#lblMsg").empty();
                $("#lblMsg").append(data.Message);
                $("#txtUserName").val('');
                $("#txtPwd").val('');
            }
            if (data.Status == "NoData") {
                $("#lblMsg").empty();
                $("#lblMsg").append(data.Message);
                $("#txtUserName").val('');
                $("#txtPwd").val('');
            }
            else {
                // $('#checkout .checkout-heading').empty();
                ShowInfo(data, '#notification');
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}

//end


//IsApproveReview 

function AdminIsApproveReview(ReviewId) {
    $.ajax({
        url: siteURL + 'api/Master/IsApproveReview?rid=' + ReviewId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                GetProductReviewsList();
            }
            else {
                ShowInfo(data, "#notification");
            }
        }
    });
}

//End IsApproveReview



function GetProduct() {
    $('#main').empty();
    // pid = getParameterByName("id");
    pid = "";
    $.ajax({
        url: siteURL + 'api/Master/GetProductInputData',
        type: 'GET',
        dataType: 'json', async: false,
        success: function (data) {
            if (data != "") {
                pid = data;
            }
        }
    });


    //GetProductInputData
    pid = window.location.href.split("/")[5];
    if (pid == undefined || pid == null || pid == "") {
        $('#main').html(" No matching products available. ");
    }
    else {
        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/GetProductInfo?productId=' + pid,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#main').empty();
                    // var myDiv = $.create("div");
                    //    var myDiv = $('<div />')
                    //   var decoded = myDiv.html(data.Result).text();
                    $('#main').html(data.Result);

                    ScorlImage();
                    jQuery('.cloud-zoom, .cloud-zoom-gallery').CloudZoom({
                        zoomFlyOut: true,
                        lensWidth: 50,
                        lensHeight: 50
                    });
                    // tabs();
                    $('#tabs a').tabs();
                    // $('.fancybox').fancybox({ cyclic: true });
                    // ShowInfo(decoded, '#padding20');
                    //  WriteProductResponse(data); 
                }
                else if (data.Status == "NoData") {
                    $('#main').empty();
                    $('#main').html('<div align="center" style="font-size:18px;color:#d9387e;line-height:100px;">There is no product with this id </div>');

                }
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
}

function cartRedirect() {
    window.location.href = "http://admin.healthurwealth.com/MyCart.aspx";
}




//GetProductReviewsTable
function GetProductReviewsList() {
    $.ajax({
        url: siteURL + 'api/Master/GetProductReviews',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#list').empty();
            ShowInfo(data, '#list');
        }
    });
}

//EndReviews


function NavigateToSearch() {
    window.location.href = 'index.aspx';
}

function ClearTextBox() {
    $("#txbFirstName").val('');
    $("#txbLastName").val('');
    $("#txbEmailId").val('');
    $("#txbPassword").val('');
    $("#txbMobileNumber").val('');
}


function Mytransactions() {

    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        $.ajax({
            url: siteURL + 'api/Master/GetOrderDetails?uId=' + json['UserId'],
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#orderDetails').empty();
                    ShowInfo(data, '#orderDetails');
                    //  Pageing();
                }
                if (data.Status == "NoData") {
                    $('#orderDetails').append(data.Result);
                }
            }
        });
    }
}

function GetMyOrders() {
    var rows;
    var minval = 5;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    $.ajax({
        url: siteURL + 'api/Master/GetMyOrders?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divDeliveredOrderssGrd').empty();
                ShowInfo(data, '#divDeliveredOrderssGrd');

            }
            if (data.Status == "NoData") {
                $('#divDeliveredOrderssGrd').empty();
                $('#divDeliveredOrderssGrd').append(data.Result);
            }
        }
    });
}

function GetProductOverview(transId) {


    $.ajax({
        url: siteURL + 'api/Master/GetProductOverview?transId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divOrdersOverviewGrd').empty();
                ShowInfo(data, '#divOrdersOverviewGrd');
                setTimeout(function () {
                    if ($('#statename').val() != "Telangana") {
                        $('#igst').remove();
                        $('#CGST').text('IGST');
                    }
                }, 500);
            }
            if (data.Status == "NoData") {
                $('#divOrdersOverviewGrd').empty();
                $('#divOrdersOverviewGrd').append(data.Result);
            }
        }
    });
}
function GetReturnOrders(transId) {


    $.ajax({
        url: siteURL + 'api/Master/GetReturnOrders?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {

                $('#divOrderReturns').empty();
                ShowInfo(data, '#divOrderReturns');
                $("#divgvwReturnProductsSelected").hide();
            }
            if (data.Status == "NoData") {
                $('#divOrderReturns').empty();
                $('#divOrderReturns').append(data.Result);
            }
        }
    });
}

function NavigatetoMyOrderDetailsPage(transId) {
    window.location.href = "MyOrderDetails.aspx?transId=" + transId;
}


function GetProcessReturnsDetails(transId) {

    //  var transId = $("#txttransId").val();
    $.ajax({
        url: siteURL + 'api/Master/GetProcessReturnsDetails?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {

                $('#divproductReturns').empty();
                ShowInfo(data, '#divproductReturns');

            }
            if (data.Status == "NoData") {
                $('#divproductReturns').empty();
                $('#divproductReturns').append(data.Result);
            }
        }
    });
}

function GetReplacementProduct() {

    var transId = $("#txttransId").val();
    $.ajax({
        url: siteURL + 'api/Master/EditReplaceOrders?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.PaymentTransactionId);
                $("#txtqty").val(data.Result.Quantity);
                $("#txtprice").val(data.Result.ProductCost);


            }
            if (data.Status == "NoData") {
                $('#divOrderReplacement').empty();
                $('#divOrderReplacement').append(data.Result);
            }
        }
    });
}
function AddReplacementProduct() {

    var transId = $("#txttransId").val();
    $.ajax({
        url: siteURL + 'api/Master/GetReplacementProduct?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.PaymentTransactionId);
                $("#txtqty").val(data.Result.Quantity);
                $("#txtprice").val(data.Result.ProductCost);
                $('#divOrderReplacement').empty();
                ShowInfo(data, '#divOrderReplacement');
            }
            if (data.Status == "NoData") {
                $('#divOrderReplacement').empty();
                $('#divOrderReplacement').append(data.Result);
            }
        }
    });
}
function EditReplaceOrders(transId) {


    $.ajax({
        url: siteURL + 'api/Master/EditReplaceOrders?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.PaymentTransactionId);
                $("#txtqty").val(data.Result.Quantity);
                $("#txtprice").val(data.Result.ProductCost);


            }
            if (data.Status == "NoData") {
                $('#divOrderReplacement').empty();
                $('#divOrderReplacement').append(data.Result);
            }
        }
    });
}

function UpdateReturnProductAction(trnsId) {
    var cartJson = { 'PaymentTransactionId': trnsId, 'OrdersReturnReason': $("#ddlReturnReason option:selected").html(), 'OrdersReturnAction': $("#ddlReturnAction option:selected").html() };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/UpdateReturnProductAction',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                NavigatetoProcessReturnsPage(data.Result)
            }
            else {
                $("#divMsg").empty();
                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + data.Result + "' has been Cancelled successfully.";
                document.getElementById('divMsg').style.display = 'block';

            }
        }

    });
}

function CheckboxChecked(dis) {

    $("#lblPtrnsId").val($(dis).val());
    $("#txtBlkOrderNo").val($(dis).val());

    $('input[class=Check][type=checkbox]').click(function () {


        var checked = $(this).is(':checked');
        $('input[class=Check][type=checkbox]').each(function () {
            this.checked = false;
        });
        if (checked) {
            this.checked = true;


        }
    });

}
function NavigatetoOrderReturnsPage() {
    var transId = $("#lblPtrnsId").val();
    window.location.href = "OrderReturns.aspx?transId=" + transId;
}
function NavigatetoOrderReturnsPage() {
    var transId = $("#lblPtrnsId").val();
    window.location.href = "OrderReturns.aspx?transId=" + transId;
}

function NavigateToSearchPage() {
    window.location.href = "index.aspx";
}

function CheckboxSelect(dis) {
    $("#divgvwReturnProductsSelected").show();

    $('input[class=Check][type=checkbox]').click(function () {


        var checked = $(this).is(':checked');
        $('input[class=Check][type=checkbox]').each(function () {
            this.checked = false;
            $("#divgvwReturnProductsSelected").hide();
        });
        if (checked) {
            this.checked = true;
            $("#divgvwReturnProductsSelected").show();

        }
    });

}

function UpdateReturnProductAction(trnsId) {

    var ReturnActionValue = $("#ddlReturnAction option:selected").val()
    if (ReturnActionValue == 2) {
        var cartJson = { 'PaymentTransactionId': trnsId, 'OrdersReturnReason': $("#ddlReturnReason option:selected").html(), 'OrdersReturnAction': $("#ddlReturnAction option:selected").html() };

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + 'api/Master/UpdateReturnProductAction',
            type: 'post',
            dataType: 'json',
            data: JSON.stringify(cartJson),

            success: function (data) {
                if (data.Status == "Success") {
                    NavigatetoProcessReturnsPage(data.Result)
                }
                else {
                    $("#divMsg").empty();
                    document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + data.Result + "' has been Cancelled successfully.";
                    document.getElementById('divMsg').style.display = 'block';

                }
            }

        });
    }

    if (ReturnActionValue == 1) {
    }

    if (ReturnActionValue == 3) {
    }
}

function Pincodecheck(Pincode) {
    //  var Pincode = $('#txtPincodecheck').val();
    $.ajax({
        url: siteURL + 'api/Master/PincodeAvailabilityCheck?Pincode=' + Pincode,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                document.getElementById('PincodeAvailMsg').innerHTML = "<div><span class='tickIco' style='padding:0px 18px'>Product can be delivered at your location.</span></div>";
                document.getElementById('PincodeAvailMsg').style.display = 'block';
                $("#billing-buttons-container").show();
                //                $("#billing-buttons-container").hide();
            }
            if (data.Status == "NoData") {
                document.getElementById('PincodeAvailMsg').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Sorry! Product cannot be delivered at your pincode location.</span></div>";
                document.getElementById('PincodeAvailMsg').style.display = 'block';
                $("#billing-buttons-container").removeAttr('disabled');
                $("#billing-buttons-container").hide();
            }
        }
    });
}

function Estimateshipping(Pincode) {
    var Pincode = $('#txtzipcode').val();
    $.ajax({
        url: siteURL + 'api/Master/Estimateshipping?Pincode=' + Pincode,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {

                $("#total_shipping").html("");
                $("#total_price").html("");
                $("#total_shipping").html(data.ShippingAmount);
                $("#total_price").html(data.CartSum);
                $("#divCarriers").empty();
                $("#divCarriers").append(data.Message);
            }
            if (data.Status == "NoData") {
                $("#divCarriers").empty();
                $("#divCarriers").append(data.Message);
            }
        }
    });
}

function AddToNotify() {
    alert('AddToNotify');
    var url = window.location.pathname;
    pid = url.split('/')[url.split('/').length - 1];
    var cartJson = { 'ProductId': pid, 'UserName': $("#txtNUserName").val(), 'EmailId': $("#txtEmailId").val(), 'MobileNumber': $("#txtMobileNumber").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",  
        url: siteURL + 'api/Master/AddNotifyMe',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                // alert('Successful update Notify');
                $('#divstack').empty();
                $('#divstack').show();
                $('#divstack').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
                //vpb_hide_popup_boxes();
                //  $('#divstack').empty();
                //document.getElementById('divmsg').innerHTML = "<div><span class='crossIco' style='padding:0px 18px'>Successful update Notify </span></div>";
                // $("#lblNotifysuccess").html(data.Message);
            }
            else {
                //   alert('Failed to update Notify');
                $('#divstack').empty();
                $('#divstack').show();
                $('#divstack').append('<div  class="attention" style="visibility:visible">' + data.Message + ' </div>');
                //$("#divMsg").empty();

            }
        }

    });
}

function AddToNotifyforQty() {
    alert('AddToNotifyforQty');
    pid = getParameterByName("id");
    var cartJson = { 'ProductId': pid, 'UserName': $("#txtName").val(), 'EmailId': $("#txtEmail").val(), 'MobileNumber': $("#txtPhone").val() };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/AddNotifyMe',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                alert('Successful update Notify');
                vpb_hide_popup_boxes();
                $('#divstack').empty();
                $('#divstack').hide();
            }
            else {
                $("#divMsg").empty();
                $('#divstack').hide();
            }
        }

    });
}

function GetCartProductsSession(dis, pid) {
    $.ajax({
        url: siteURL + 'api/Master/GetCartProductsSession',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#lblTotalAmount").html("");
                // var pid= $("#subtotal").val();
                //                $("#txtSubtotal").html();
                //                $("#txtSubtotal").html(data.Result);

                //  var data = GetCartProducts();
                $("#lblTotalAmount").html(data.CartSum);
                var itm = $(dis).closest('div.cart-item-cont');
                var it = itm.find('#txtSubtotal').html(data.Result);
                //  var id = $(dis)
                var productId = $(this).closest($("#div.cart-total1 ").attr("fieldsupercategoryid"));

                var prvQty = $(quantity).closest('div.cart-quantity').find('#txtPrvQty').val();

                //               
                //                if (productId == pid) {
                //               $('#txtSubtotal').html(data.Result);
                //                }



            }
            if (data.Status == "NoData") {

            }
        }
    });
}

function GetFeaturedProducts() {
    var CurrentURL = window.location.href;
    if (CurrentURL.indexOf('www.') == -1) {
        SiteURL = siteURL;
    }
    else {
        SiteURL = siteURL;
    }
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: SiteURL + "api/Master/GetAllFeaturedProducts",
        type: 'GET',
        //data: JSON.stringify(cartJson),
        //   data:  cartJson,
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                $('.Featuredproducts').html(data);
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: siteURL + "api/Master/NewProducts",
                    type: 'GET',
                    //data: JSON.stringify(cartJson),
                    //   data:  cartJson,
                    dataType: 'json',
                    success: function (data) {
                        if (data != null) {
                            $('.Newproducts').empty();
                            $('.Newproducts').html(data);
                        }
                        //displayrelatedproducts              
                    }
                });
            }
            //displayrelatedproducts              
        }
    });
}

function GetNewProducts() {
    var CurrentURL = window.location.href;
    if (CurrentURL.indexOf('www.') == -1) {
        SiteURL = siteURL;
    }
    else {
        SiteURL = siteURL;
    }
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: SiteURL + "api/Master/GetAllNewProducts",
        type: 'GET',
        //data: JSON.stringify(cartJson),
        //   data:  cartJson,
        dataType: 'json',
        success: function (data) {
            if (data != null) {
                $('.Newproducts').html(data);
                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: siteURL + "api/Master/FeaturedProducts",
                    type: 'GET',
                    //data: JSON.stringify(cartJson),
                    //   data:  cartJson,
                    dataType: 'json',
                    success: function (data) {
                        if (data != null) {
                            $('.Featuredproducts').html(data);
                        }
                        //displayrelatedproducts              
                    }
                });
            }
            //displayrelatedproducts              
        }
    });
}

function SizeChart(Id) {
    $('.overlay').show();
    $('#productImage').attr("src", Id);
}

function SignupNewslettrMail() {
    var EmailId = $('#txtnewsletter').val();
    $.ajax({
        url: siteURL + 'api/Master/SignupNewslettrMailId?EmailId=' + EmailId,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert('Congratulations! You will receive News Updates from Health ur wealth.');
                $('input[type=text]').each(function () {
                    $(this).val('');
                });
                // vpb_show_login_box();
                //alert("successfull");
                document.getElementById('divNewslttrMsg').innerHTML = "<div class='form-subscribe' style='display: block; color:#D9387E ; padding-left: 120px;'>Congratulations! You will receive News Updates from Health ur wealth.</div>";
                document.getElementById('divNewslttrMsg').style.display = 'block';
            }
        }
    });
}

$('#form1').click(function () {
    $('#Reproduct-list').empty();
    $('#Reproduct-list').hide();
});


$(document).ready(function () {
    $('.scrollToTop').fadeOut();
    //Check to see if the window is top if not then display button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scrollToTop').fadeIn();
        } else {
            $('.scrollToTop').fadeOut();
        }
    });

    //Click event to scroll to top
    $('.scrollToTop').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 800);
        return false;
    });
});
function Tooltip() {
    var title = $('#tooltipData').html();
    $('<div id="toolTipBg" onclick="closeTooltip()"></div>').css({
        position: 'fixed',
        width: '100%',
        height: '100%',
        top: '0',
        left: '0',
        background: 'rgba(0,0,0,0.005)',
        zIndex: '999999'
    }).appendTo('body');
    $('<p class="tooltip"></p>').html("<div style='right:20px;position:absolute;cursor:pointer;color:#f00;zIndex:99999999999' onclick='closeTooltip()'>X</div>" + title).appendTo('.deliveredDiv').fadeIn('slow');

    $('body').addClass('tooltipOpen');
}
function closeTooltip() {
    $('.tooltip').fadeOut().remove();
    $('body').removeClass('tooltipOpen');
    $('#toolTipBg').remove();
}





// Search Products Script Starts

function SearchBySuperCat(id) {
    $('#product-list').empty(); //Clearing all products
    $("#dvSubCat").empty(); //clearing left side sub caegories
    pSuperCategoryId = id;
    GetProducts();
}

//Begin  Sub-Categories
function GetSubCategoriesByCategory() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryListByCategory?id=' + pCategoryId,
        type: 'GET',
        async: false,
        dataType: 'json',
        success: function (data) {
            WriteSubCategories(data);
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });
}

function WriteSubCategories(data) {
    $("#dvSubCat").empty();
    ShowInfo(data, "#dvSubCat");
}


//End  Sub-Categories


function GetProducts() {
    
    if (checkPage()) {
        window.location.href = "index.aspx?cid=" + pCategoryId + "&scid=" + pSubCategoryId + "&supid=" + pSuperCategoryId;
    }
    else {

        if (isNew == true) {
            $('#product-list').empty();
        }
        else {
            isNew = true;
        }

        if (pCategoryId == null || pCategoryId == 0) {
            pCategoryId = $("#menu ul:first li:first div.menu-category-wall-sub-name:first a").attr("fieldCategoryId");
            //    if (pSuperCategoryId == 0)//for the first time , display sub categories of the first category menu item from the category menu items
            //        GetSubCategoriesByCategory(pCategoryId);
        }

        psord = $("#ddlProductSort option:selected").val();
        prows = $('#product-list div').length;

        var selectedBrandNames = "";
        $("ul#side_menu2 input:checkbox[name=checkboxlist]:checked").each(function () {
            $(this).attr('checked', 'checked');
            selectedBrandNames = selectedBrandNames == "" ? $(this).attr("fieldtext") : selectedBrandNames + "," + $(this).attr("fieldtext");
        });
        $("#lblselectedBrandNames").text(selectedBrandNames);

        var selectedSubCategories = "";
        var selectedSubCategoryNames = "";
        //check if sub categories are cecked or not 
        $("ul#side_menu input:checkbox[name=checkboxlist]:checked").each(function () {
            selectedSubCategories = selectedSubCategories == "" ? $(this).attr("fieldvalue") : selectedSubCategories + "^" + $(this).attr("fieldvalue");
            selectedSubCategoryNames = selectedSubCategoryNames == "" ? $(this).attr("fieldtext") : selectedSubCategoryNames + "," + $(this).attr("fieldtext");
            GetBrandNames(pCategoryId, selectedSubCategories);
        });

        if (selectedSubCategories == null || selectedSubCategories == '') {
            pSubCategoryId = 0;
        }

        $("#lblSelectedSubCategories").text(selectedSubCategoryNames);

        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/GetProductList?sord=' + psord + '&minprice=' + pminprice + '&maxprice=' + pmaxprice + '&SubCategories=' + selectedSubCategories + '&Brands=' + selectedBrandNames + '&Colors=' + "" + '&rows=' + prows + '&categoryId=' + pCategoryId + '&subCategoryId=' + pSubCategoryId + '&SuperCatId=' + pSuperCategoryId,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#product-list').empty();
                if (data.Status == "NoData") {
                    GetSubCategoriesByCategory();
                    //   CheckSubCategory(pSubCategoryId);
                    $('#last_msg_loader').html('<div><p>No More Products.</p></div>');
                }
                else if ((data.Status == "Fail")) {
                    $('#last_msg_loader').html('<div style="font-weight:bold;"><p>No More Products.</p></div>');
                }
                else {
                    $('#product-list').empty();
                    $('#last_msg_loader').empty();
                    ShowInfo(data, '#product-list');
                    GetSubCategoriesByCategory();
                    // CheckSubCategory(pSubCategoryId);
                    // $("img").lazyload({ threshold: "200", effect: "fadeIn", effectspeed: 2000 });

                    $("img.lazyload").each(function () {
                        $(this).attr("src", $(this).attr("original"));
                        $(this).removeAttr("original");
                    });

                    $('div#last_msg_loader').empty();
                    //loading = true;
                }
            },
            error: function (x, y, z) {
                var data;
                ShowInfo(data, '');
            }
        });
    }
}

//check Side menu check box
var i = 0;
function CheckSubCategory(psubCategoryId) {
    //alert('selected subcategory is '+psubCategoryId);
    $("ul#side_menu input:checkbox[name=checkboxlist]").each(function () {
        //alert('CheckSubCategory');
        // alert($(this).attr("fieldvalue"));

        $.urlParam = function (name) {
            var results = new RegExp('[\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
            return results[1] || 0;
        }

        // example.com?param1=name&amp;param2=&amp;id=6

        var scid = $.urlParam('scid'); // name

        if ($(this).attr("fieldvalue") == scid && i == 0) {
            $(this).prop('checked', true);
            i = 1;
        }
        if ($(this).attr("fieldvalue") == psubCategoryId && $(this).prop("checked")) {
            $(this).prop('checked', true);
            //$(this).prop('checked', false);
        }
    });
}


function SearchData(dis, CatId, SubCatId) {

    // alert('data received : categoryid'+CatId+'sbucatid'+SubCatId);
    SuperCatId = 0;
    if (checkPage()) {
        window.location.href = "index.aspx?cid=" + CatId + "&scid=" + SubCatId + "&supid=" + pSuperCategoryId;

        GetBrandNames(pCategoryId, pSubCategoryId);
    }
    else {

        if (pCategoryId != CatId) {
            $('#product-list').empty();
            //Select Category Background color
            $('ul.primary_nav li').removeClass('active');
            $(dis).parent().addClass("active");
            pCategoryId = CatId;
            GetSubCategoriesByCategory(pCategoryId);
        }



        //assign subcatIds to checkboxes
        $("ul#side_menu input:checkbox[name=checkboxlist]").each(function () {
            // alert($(this).attr("fieldvalue"));
            if ($(this).attr("fieldvalue") == SubCatId) {
                $(this).attr('checked', 'checked');
                // alert('SearchData');
                //   $(this).prop('checked', true);
            }
        });

        GetProducts();
    }

}


function SearchByCategory(dis, id) {

    $('ul.primary_nav li').removeClass('active');
    // $('li.menu_cont a').style.removeAttribute("display");

    $(dis).parent().addClass("active");
    //  $(dis).style.createAttribute("display","block");

    $('#product-list').empty();
    pCategoryId = id;

    GetSubCategoriesByCategory(pCategoryId);

    $("#lblSelectedCategory").text($(dis).text());


    GetSubCategoriesByCategory(pCategoryId);
    GetProducts();
}

function SearchBySubCategory(catId, subCatid) {
    $('#product-list').empty();
    pCategoryId = catId;
    pSubCategoryId = subCatid;

    $('ul.primary_nav li').removeClass('active');

    $("ul.primary_nav li").each(function () {

        if ($(this).attr('catid') != 'undefined' && $(this).attr('catid') == pCategoryId) {
            $(this).addClass("active");
        }
    });

    GetSubCategoriesByCategory(pCategoryId);
    GetProducts();
}



function SearchBySuperCat(id) {

    // alert('search by supercategory .supercategoryid is '+id);
    $('#product-list').empty(); //Clearing all products
    $("#dvSubCat").empty(); //clearing left side sub caegories
    pSuperCategoryId = id;
    GetProducts();
}

//Begin Categories & Sub-Categories
function GetAllCategories() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data);

        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}
//function LoadSearch() {
//    //$(window).scroll(function () {
//    //    if ($('#product-list').height() <= $(window).scrollTop()) {

//    //        if (loading) {
//    //            loading = false;
//    //            isNew = false;
//    //            GetProducts();
//    //        }
//    //    }
//    //});
//    var a = getParameterByName("cid");
//    var b = getParameterByName("scid");
//    var c = getParameterByName("supid");
//    SuperCatId = getParameterByName("supid") == "" ? 0 : getParameterByName("supid");
//    if (SuperCatId == null || SuperCatId == 0) {
//        SuperCatId = c;
//    }
//    //pcategoryId = a == "" ? 0 : a;
//    //  psubCategoryId = b == "" ? 0 : b;
//    // alert('super category id b is'+b);
//    // alert('pcategory id is'+pcategoryId);
//    if (pcategoryId == null || pcategoryId == 0) {
//        if (b != null || b != 0) {
//            pcategoryId = a;
//        }
//        else {
//            pcategoryId = $("#menu .menu-category-wall-sub-name a").attr("fieldCategoryId");
//        }
//        // alert('pcategory id after computation is' + pcategoryId);
//        ////        pcategoryId = $("#menu  .column a").attr("fieldCategoryId");

//        //GetSubCategoriesByCategory(a);
//        //pcategoryId = a;
//        //psubCategoryId = b;
//        //GetBrandNames(a, b);
//    }
//    //else {
//    GetSubCategoriesByCategory(pcategoryId);
//    if (psubCategoryId == 0 || psubCategoryId == null) {
//        psubCategoryId = b;
//    }
//    GetBrandNames(pcategoryId, psubCategoryId);
//    //GetBrandNames();

//    //}
//    GetProducts();
//}


//Search Products Script ends


