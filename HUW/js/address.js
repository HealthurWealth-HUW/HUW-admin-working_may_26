/**
 * Created with JetBrains PhpStorm.
 * User: Rajan.Lumb
 * Date: 12/30/12
 * Time: 5:13 PM
 * To change this template use File | Settings | File Templates.
 */
var shipping_address_id;
var shipping_address_checksum;
var billing_address_id;
var billing_address_checksum;

function select_address_and_proceed(address_id, address_checksum, billing_address_id, billing_address_checksum)
{
    document.form_action_proceed.address_id.value = address_id;
    document.form_action_proceed.address_checksum.value = address_checksum;
    document.form_action_proceed.billing_address_id.value = billing_address_id;
    document.form_action_proceed.billing_address_checksum.value = billing_address_checksum;
    document.form_action_proceed.submit();
}

function open_edit_address_dialog(address_id)
{
    var url = "/edit?" + "Aid=" + address_id + "&addressType=" + "&random=" + "&destination=/cart/select-address";
    $.colorbox({href: url, width: 644, height: 620, iframe: false});
}

var add_address_and_proceed_progress = false;
function add_address_and_proceed()
{
    add_address_and_proceed_progress = true;

    var address_keys = 'first_name,last_name,street1,city,zone,country,postal_code,phone';
    address_keys = address_keys.split(',');


    shipping_address_id = '';
    shipping_address_checksum = '';
    billing_address_id = '';
    billing_address_checksum = '';

    var shipping_address = $('input[type=checkbox][name=shippingAddress]:checked');
    if (shipping_address.length == 1) {
        shipping_address_id = shipping_address.attr('id').replace('ShippingAddress', '');
        shipping_address_checksum = shipping_address.val();
    }

    var billing_address = $('input[type=checkbox][name=billingAddress]:checked');
    if (billing_address.length == 1) {
        billing_address_id = billing_address.attr('id').replace('BillingAddress', '');
        billing_address_checksum = billing_address.val();
    }



    if (shipping_address.length == 1 && billing_address.length == 1) {
        select_address_and_proceed(shipping_address_id, shipping_address_checksum, billing_address_id, billing_address_checksum);
        return;
    }



    var sameShippingAndBillingAddress = ($('input[type=checkbox][name=billing-address-checkbox]:checked').length == 1);

    if (shipping_address.length && sameShippingAndBillingAddress) {
        select_address_and_proceed(shipping_address_id, shipping_address_checksum, shipping_address_id, shipping_address_checksum);
        return;
    }


    var addresses = {};
    if (shipping_address.length == 0) {
        addresses.shipping_address = {};
        for (var i in address_keys)
            addresses.shipping_address[address_keys[i]] = $('#NewShippingAddress input[name=' + address_keys[i] + ']').val();
        addresses.shipping_address['zone'] = $('#NewShippingAddress select[name=zone]').val();
    }

    if (billing_address.length == 0) {
        if (!sameShippingAndBillingAddress) {
            addresses.billing_address = {};
            for (var i in address_keys)
                addresses.billing_address[address_keys[i]] = $('#NewBillingAddress input[name=' + address_keys[i] + ']').val();
            addresses.billing_address['zone'] = $('#NewBillingAddress select[name=zone]').val();
        }
    }

    addresses['action'] = 'add_address';

    $.post('/cart/select-address', addresses, function(response){
        if (response) {
            if (response.status == 'OK') {
                if (response.data.shipping_address) {
                    shipping_address_id = response.data.shipping_address.address_id;
                    shipping_address_checksum = response.data.shipping_address.address_checksum;
                }
                if (response.data.billing_address) {
                    billing_address_id = response.data.billing_address.address_id;
                    billing_address_checksum = response.data.billing_address.address_checksum;
                }
                if (sameShippingAndBillingAddress) {
                    billing_address_id = shipping_address_id;
                    billing_address_checksum = shipping_address_checksum;
                }
                select_address_and_proceed(shipping_address_id, shipping_address_checksum, billing_address_id, billing_address_checksum);
            } else {
                if (response.data) {
                    if (response.data.shipping_address) {
                        alert('Please correct following fields in Shipping Address' + "\n" + response.data.shipping_address);
                    }
                    if (response.data.billing_address) {
                        alert('Please correct following fields in Billing Address' + "\n" + response.data.billing_address);
                    }
                } else
                    alert('Some error occurred. Please refresh and try again.');
                add_address_and_proceed_progress = false;
            }
        } else {
            alert('Some error occurred. Please refresh and try again.');
            add_address_and_proceed_progress = false;
        }
    }, 'json');
}

function billingOptions()
{
    var checked = $('input[name="billing-address-checkbox"]:checked').length > 0;
    if ( checked == 1 ) {
        $('input[type=checkbox][name=billingAddress]').removeAttr('checked');
//        billingAddressSelected(0, false);

        $('#tradus-billing-address-container').hide();
    } else {
        $('#tradus-billing-address-container').show();
    }
}

$(document).ready(function(){

    $('#tradus-billing-address').hide();
    if ($(window).width() < 480 || $(window).height() < 480) {                          // previos addresses will remain close in case of mobile mode
        $('.tradus-select-user-address-page2').hide();
    }
    $('.tradus-add-address-header').click(function(){
        if($(this).find('span.icon-up-open').length > 0)
            $(this).find('span.icon-up-open').removeClass('icon-up-open').addClass('icon-down-open');
        else
            $(this).find('span.icon-down-open').removeClass('icon-down-open').addClass('icon-up-open');
        $('.tradus-select-user-address-page2').toggle();


    });
    $('[name=billing-address-checkbox]').click(function(){
       if($(this).attr('checked') == "checked")
           $('#tradus-billing-address').hide();
       else
           $('#tradus-billing-address').show();
    });
    $('input[type=checkbox][name=shippingAddress]').bind('click', function(event){
        shippingAddressSelected(event.target.id.replace('ShippingAddress', ''), event.target.checked);
    });
    $('input[type=checkbox][name=billingAddress]').bind('click', function(event){
        billingAddressSelected(event.target.id.replace('BillingAddress', ''), event.target.checked);
    });
});

function shippingAddressSelected(id, checked)
{
    $('input[type=checkbox][name=shippingAddress]').removeAttr('checked');

    if (checked) {
        $('input[type=checkbox]#ShippingAddress' + id).attr('checked', 'checked');
        $('#NewShippingAddress input[name=first_name]').val($('#txbShippingAddress1' + id).html());
        $('#NewShippingAddress input[name=last_name]').val($('#txbShippingAddress2' + id).html());
        $('#NewShippingAddress input[name=ShippingLandmark]').val($('#txbShippingLandmark' + id).html());
        $('#NewShippingAddress input[name=city]').val($('#txbShippingCity' + id).html());
        $('#NewShippingAddress select[name=zone]').val($('#txbShippingState' + id).text());
        $('#NewShippingAddress input[name=country]').val($('#ddlShippingCountry' + id).html());
 
        $('#NewShippingAddress input[name=postal_code]').val($('#txbShippingPincode' + id).html());
        $('#NewShippingAddress input').attr('disabled', 'true');
        $('#NewShippingAddress select').attr('disabled', 'true');
    } else {
        $('#NewShippingAddress input[name=first_name]').val('');
        $('#NewShippingAddress input[name=last_name]').val('');
        $('#NewShippingAddress input[name=street1]').val('');
        $('#NewShippingAddress input[name=city]').val('');
        $('#NewShippingAddress input[name=postal_code]').val('');
        $('#NewShippingAddress select[name=zone]').val('');
        $('#NewShippingAddress input[name=country]').val('');
        $('#NewShippingAddress input[name=phone]').val('');
        $('#NewShippingAddress input').removeAttr('disabled', '');
        $('#NewShippingAddress select').removeAttr('disabled', '');
    }
}

function billingAddressSelected(id, checked)
{
    $('input[type=checkbox][name=billingAddress]').removeAttr('checked');

    if (checked) {
        $('input[type=checkbox]#BillingAddress' + id).attr('checked', 'checked');

        $('#NewBillingAddress input[name=first_name]').val($('#FirstName' + id).html());
        $('#NewBillingAddress input[name=last_name]').val($('#LastName' + id).html());
        $('#NewBillingAddress input[name=street1]').val($('#Address' + id).html());
        $('#NewBillingAddress input[name=city]').val($('#City' + id).html());
        $('#NewBillingAddress input[name=postal_code]').val($('#Pincode' + id).html());
        $('#NewBillingAddress select[name=zone]').val($('#State' + id).text());
        $('#NewBillingAddress input[name=country]').val($('#Country' + id).html());
        $('#NewBillingAddress input[name=phone]').val($('#Mobile' + id).html());

        $('#NewBillingAddress input').attr('disabled', 'true');
        $('#NewBillingAddress select').attr('disabled', 'true');

        $('#tradus-billing-address-container').show();
        $('input[name=billing-address-checkbox]').removeAttr('checked');
    } else {
        $('#NewBillingAddress input[name=first_name]').val('');
        $('#NewBillingAddress input[name=last_name]').val('');
        $('#NewBillingAddress input[name=street1]').val('');
        $('#NewBillingAddress input[name=city]').val('');
        $('#NewBillingAddress input[name=postal_code]').val('');
        $('#NewBillingAddress select[name=zone]').val('');
        $('#NewBillingAddress input[name=country]').val('');
        $('#NewBillingAddress input[name=phone]').val('');

        $('#NewBillingAddress input').removeAttr('disabled', '');
        $('#NewBillingAddress select').removeAttr('disabled', '');

        $('#tradus-billing-address-container').hide();
        $('input[name=billing-address-checkbox]').attr('checked', 'checked');
    }
}
