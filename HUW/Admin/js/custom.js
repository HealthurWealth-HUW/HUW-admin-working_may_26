
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    //alert(results);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
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

function Insertpin() {

    var cartJson = { 'State_UnionTerritory': $("#ddlstate  option:selected").text(), 'District': $("#txtDistrict").val(), 'City': $("#txtCity").val(), 'LocationCode': $("#txtLocation").val(), 'pincode': $("#txtpincode").val(), 'cod': $("#txtcod").val(), 'ControlingStation': $("#txtControlingStation").val(), 'ShippingAmount': $("#txtShippingAmount").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/Insertpin',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            $('#divinsertstatus').html('<div class="msgbar msg_Info hide_onC" id="ctl00_ctl00_Main_Main_divMessage"><span class="iconsweet" id="ctl00_ctl00_Main_Main_msgicon">*</span><p><span id="ctl00_ctl00_Main_Main_lblMessage">' + data.Message + '</span></p></div>');
            $('input:text').val('');
        },
        error:
        {
            //Show error message
        }
    });
}



function Insertofflinesales() {

    var cartJson = { 'OfflineInvoiceID': $("#txtOfflineInvoiceID").val(), 'SubProductId': $("#txtSubProductId").val(), 'ProductId': $("#txtProductId").val(), 'Quantity': $("#txtQuantity").val(), 'Totalprice': $("#txtTotalprice").val(), 'UserName': $("#txtbuyername").val(), 'CreatedOn': $("#txtsolddate").val(), 'Phonenumber': $("#txtPhonenum").val(), 'AddressLine1': $("#txtAddress").val(), 'IsActive': $("#txtIsactive").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/Insertofflinesales',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {

            $('#divinsertstatus').html('<div class="msgbar msg_Info hide_onC" id="ctl00_ctl00_Main_Main_divMessage"><span class="iconsweet" id="ctl00_ctl00_Main_Main_msgicon">*</span><p><span id="ctl00_ctl00_Main_Main_lblMessage">' + data.Message + '</span></p></div>');

            $('input:text').val('');
        },
        error:
        {
            //Show error message
        }
    });
}






function ShowInfo(CustomResponse, Control) {

    $('.success, .warning, .attention, .information, .error').remove();

    if (CustomResponse != undefined)
        if (CustomResponse.Status == "Fail") {
            $('#notification').html('<div class="error" style="display: none;">' + CustomResponse.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
            $('.error').fadeIn('slow');
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
        else if (CustomResponse.Status == "Success") {
            $(Control).append(CustomResponse.Result);
            $('#ctl00_ctl00_Main_Main_grdShippingOrders').dataTable({
                "order": [[1, "desc"]]
            });
        }
        else if (CustomResponse.Status == "NoData") {
            $('#notification').html('<div class="attention" style="display: none;">' + CustomResponse.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
            $('.success').fadeIn('slow');
            $('html, body').animate({ scrollTop: 0 }, 'slow');
        }
}

function SearchRelatedProducts() {
    var searchText = $('#txtRelatedProducts').val();
    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/SearchRelatedProductList?sord=' + searchText,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#product-list').empty();
            if (data.Status == "NoData") {
                $('#last_msg_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#product-list').empty();
                ShowInfo(data, '#product-list');

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
            var data;
            ShowInfo(data, '');
        }
    });

}


function SearchFreeProducts() {
    var searchText = $('#txtFreeProducts').val();
    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/SearchFreeProductList?sord=' + searchText,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#free-product-list').empty();
            if (data.Status == "NoData") {
                $('#free-last_msg_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#free-product-list').empty();
                ShowInfo(data, '#free-product-list');
                $('div#free-last_msg_loader').empty();
            }
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });

}

function SearchAllProducts() {
    var searchText = $('#txtProductName').val();


    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/SearchAllProductList?sord=' + searchText,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#All_product-list').empty();
            if (data.Status == "NoData") {
                $('#last_Product_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#All_product-list').empty();
                ShowInfo(data, '#All_product-list');
                $('div#last_Product_loader').empty();
            }
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });

}
function SearchAllProductsforprescription(datatext) {


    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/SearchAllProductList?sord=' + datatext,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#All_product-list').empty();
            if (data.Status == "NoData") {
                $('#last_Product_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#All_product-list').empty();
                ShowInfo(data, '#All_product-list');
                $('div#last_Product_loader').empty();
            }
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });

}
function ProductAutocompletess(ProductName) {
    var re = /,/g;
    products = ProductName.replace(re, " ");
    $('#txtProductName').val(products);
    $('#All_product-list').hide();
}

function SearchAllBrands() {
    var searchText = $('#txtProductBrand').val();

    jQuery.support.cors = true;
    $.ajax({
        url: '../api/Master/SearchAllBrandList?sord=' + searchText,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#All_Brand-list').empty();
            if (data.Status == "NoData") {
                $('#last_Brand_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#All_Brand-list').empty();
                ShowInfo(data, '#All_Brand-list');
                $('div#last_Brand_loader').empty();
            }
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });

}


function BrandAutocomplete(Brand) {
    var re = /,/g;
    brands = Brand.replace(re, " ")
    $('#txtProductBrand').val(brands);
    $('#All_Brand-list').hide();
}

function CostCaluclations() {
    if ($('#txtProductCost').val() == "" || $('#txtProductCost').val() == NaN)
        $('#txtProductCost').val("");

    if ($('#txtProductDiscount').val() == "" || $('#txtProductDiscount').val() == NaN)
        $('#txtProductDiscount').val("");

    var ProductCost = $('#txtProductCost').val() == "" ? 0 : parseFloat($('#txtProductCost').val());
    var ProductDiscount = $('#txtProductDiscount').val() == "" ? 0 : parseFloat($('#txtProductDiscount').val());

    var FinalAount = ProductCost - (ProductCost * ProductDiscount / 100);

    if (ProductDiscount != 0) {
        $('#txtProductCostAfterDiscount').val(FinalAount.toFixed(2));
    }
    else {
        $('#txtProductDiscount').val(0);
        $('#txtProductCostAfterDiscount').val(FinalAount.toFixed(2));
    }
}

function DiscountCaluclations() {
    if ($('#txtProductCost').val() == "" || $('#txtProductCost').val() == NaN)
        $('#txtProductCost').val("");

    if ($('#txtProductCostAfterDiscount').val() == "" || $('#txtProductCostAfterDiscount').val() == NaN)
        $('#txtProductCostAfterDiscount').val("");

    var ProductCost = $('#txtProductCost').val() == "" ? 0 : parseFloat($('#txtProductCost').val());
    var ProductAmount = $('#txtProductCostAfterDiscount').val() == "" ? 0 : parseFloat($('#txtProductCostAfterDiscount').val());
    var FinalDiscount = (100 - (ProductAmount / ProductCost) * 100);
    if (FinalDiscount != NaN) {
        $('#txtProductDiscount').val(FinalDiscount.toFixed(1));
    }
}

function SubProductCostCaluclations() {
    var Quantity = 0;
    $("#GVSubProducts tr").each(function () {
        var Quantity1 = $(this).find("input[id^='txtSubProductQuantity']").val();
        if (Quantity1 != undefined && Quantity1 != NaN) {
            Quantity = parseInt(Quantity) + parseInt(Quantity1);
        }
        var SubProductOriginalCost = $(this).find("input[id^='txtSubProductOriginalCost']").val();
        var SubProductDiscountPercentage = $(this).find("input[id^='txtSubProductDiscountPercentage']").val();
        if (SubProductOriginalCost != undefined) {
            if (SubProductOriginalCost == "" || SubProductOriginalCost == NaN)
                SubProductOriginalCost = "0";
            if (SubProductDiscountPercentage == "" || SubProductDiscountPercentage == NaN)
                SubProductDiscountPercentage = "0";
            var SubProductFinalAmountAfterDiscount = SubProductOriginalCost - (SubProductOriginalCost * SubProductDiscountPercentage / 100);
            var SubProdcutDiscountCost = (SubProductOriginalCost - SubProductFinalAmountAfterDiscount);
            $(this).find("input[id^='txtSubProductProductCost']").val(SubProductFinalAmountAfterDiscount);
        }
    });
    if (Quantity == NaN) {
        $('#txtProductQuantity').val("");
    }
    $('#txtProductQuantity').val(Quantity);
    // $("#txtSubProductQuantity").attr("readonly", true);
}


//function chkReltdProduct(product_id) {
//    if (document.getElementById("chkRelatdPrdcts").checked == true) {
//        addToRelatedProduct(product_id);
//    }
//    else if (document.getElementById("chkRelatdPrdcts").checked == false) {
//        removeRelatedProduct(product_id);
//    }
//}

//function chkReltdProduct(dis) {
// $('input[class=CheckRltd][type=checkbox]').click(function () {
//     var product_id = $(dis).val();
//    var checked = $(dis).is(':checked');
//    if (checked == true) {
//        addToRelatedProduct(product_id);
//   }
//   else if (checked == false) {
//       removeRelatedProduct(RelatedPrdctID, product_id);
//    }

//        $('input[id=chkRelatdPrdcts][type=checkbox]').each(function () {
//            this.checked = false;
//            removeRelatedProduct(RelatedPrdctID, product_id);
//        });
//        if (checked) {
//            this.checked = true;
//            addToRelatedProduct(product_id);

//        }
//   });
//
//}

function addToRelatedProduct(product_id) {

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: "../api/Master/AddToRelatedProductList?id=" + product_id,
        type: 'GET',
        //data: JSON.stringify(cartJson),
        //   data:  cartJson,
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success")
                displayCart();
            //displayrelatedproducts              
        }
    });
}

function addToFreeProduct(product_id) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: "../api/Master/AddToFreeProductList?id=" + product_id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success")
                displayFreeCart();
        }
    });
}

function displayFreeCart() {
    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'api/Master/ShowFreeProductsList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "NoData") {
                $('#free-last_msg_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#DisplayFreeProductList').empty();
                ShowInfo(data, '#DisplayFreeProductList');
                $('div#free-last_msg_loader').empty();
            }

        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }


    });
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

// Get Product Images for edit and delete images. created on 18/11/2014 created by nagul
function GetProductImages() {

    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]); vars[hash[0]] = hash[1];
    }
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/getProductImages?PrdId=' + hash[1],
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            if (data.Status == "Success") {
                var strHtmltr = "";

                var count = 0;
                var x = 0;
                for (list in data.Result) {
                    x = 0;
                    strHtmltr = strHtmltr + " <div style='float:left'><img width='125px' height='125px' src='" + data.Result[count].img.ProductImgUrl + "'><br/>Product Image";
                    strHtmltr = strHtmltr + " <br/><a onclick='editimg(" + data.Result[count].img.ProductId + ",\"1\")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
                    if (data.Result[count].img.SizeGuidePath != null) {
                        strHtmltr = strHtmltr + " <div style='float:left'><img width='125px' height='125px' src='" + data.Result[count].img.SizeGuidePath + "'><br/>Size Chart Image";
                        strHtmltr = strHtmltr + " <br/><a onclick='editimg(" + data.Result[count].img.ProductId + ",\"2\")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
                    }
                    if (data.Result[count].Galleries != "") {
                        for (list2 in data.Result[count].Galleries)
                            strHtmltr = strHtmltr + " <div style='float:left'><img width='125px' height='125px' src='" + data.Result[count].Galleries[x].ImgUrl + "'><br/>Galler Image";
                        strHtmltr = strHtmltr + " <br/><a>Edit</a>&nbsp&nbsp<a>Delete</a></div>";
                        strHtmltr = strHtmltr + " <div style='float:left'><img width='125px' height='125px' src='" + data.Result[count].Galleries[x].LargeImgurl + "'><br/>Galler Image";
                        strHtmltr = strHtmltr + " <br/><a onclick='editimg(" + data.Result[count].img.ProductId + ",\"3\")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
                        x++;
                    }
                    count++;
                }
                $('#divimgs').html(strHtmltr);
            }
            if (data.Status == "NoData") {
                $('#last_msg_loader').html('<div><p>No More Products.</p></div>');
            }
            else {
                $('#DisplayRelatedProductList').empty();
                ShowInfo(data, '#DisplayRelatedProductList');

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

function editimg(Id, type) {
    if (type = "1") {
        $('.overlay').show();
        $('#productImage').attr("src", Id);
        $('#lblPrdId').text(Id);
        $('#lblimgtype').text(type);
    }
}

function btnupdate() {
    var imgurl = $('#fileimg').val();
    var productId = $('#lblPrdId').text();
    var type = $('#lblimgtype').text();
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateProductImgs?imgurl=' + imgurl + '&PrdID=' + productId + '&imgtype=' + type,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            //            if (document.getElementById("chkRelatdPrdcts").checked == true) {
            //                document.getElementById("chkRelatdPrdcts").checked = false;
            //            }
            displayCart();
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}

function removeRelatedProduct(RelatedProdctId, productId) {
    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/deleteFromRelatedProductList?RelatedPrdctId=' + RelatedProdctId + '&PrdctID=' + productId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            //            if (document.getElementById("chkRelatdPrdcts").checked == true) {
            //                document.getElementById("chkRelatdPrdcts").checked = false;
            //            }
            displayCart();
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}

function removeFreeProduct(productId) {
    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/deleteFromFreeProductList?PrdctID=' + productId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            displayFreeCart();
        },
        error: function (x, y, z) {
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}

//Begin Super Category

function SuperCategoryGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetSuperCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "CategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['SuperCategoryId', 'SuperCategoryName', 'IsActive', 'IsDeleted'],
        colModel: [
            { name: 'SuperCategoryId', index: 'SuperCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'SuperCategoryName', index: 'SuperCustomerName', width: 200, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
            { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],
        //                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                      editoptions: { size: 12, dataInit: function (el) {
        //                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                      }
        //                      }, sorttype: "date"
        //                  },
        //               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                   editoptions: { size: 12, dataInit: function (el) {
        //                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                   }
        //                   }, sorttype: "date"
        //               },

        //                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Customer Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SuperCategoryId');
                    return value;
                }
            },
            url: "../api/Master/EditSuperCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateSuperCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SuperCategoryId');
                    return value;
                }
            }, url: "../api/Master/DeleteSuperCategory", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }

            }
        }
    );
}
//End SuperCategory



//Begin Category

function CategoryGrid() {
    //CategoryId SuperCategoryId  CategoryName  IsActive  IsDeleted  CreatedOn  UpdatedOn  CreatedBy  UpdatedBy
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "CategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['CategoryId', 'CategoryName', 'SuperCategoryName', 'SuperCategoryName', 'IsActive', 'IsDeleted'],
        colModel: [
            { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true, search: true },
            { name: 'CategoryName', index: 'CategoryName', width: 250, editable: true, editrules: { required: true }, edittype: 'text', search: true },
            //           
            //                    { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            //                    { name: 'CategoryName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

            { name: 'SuperCategoryId', index: 'SuperCategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl: '../api/Master/GetSuperCategoryListAsHtml', width: 150 } },
            { name: 'SuperCategoryName', index: 'SuperCategoryName', width: 100, viewable: true },

            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
            { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],
        //                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                      editoptions: { size: 12, dataInit: function (el) {
        //                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                      }
        //                      }, sorttype: "date"
        //                  },
        //               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                   editoptions: { size: 12, dataInit: function (el) {
        //                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                   }
        //                   }, sorttype: "date"
        //               },

        //                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of SuperCategories'
    });
    // jQuery("#list").searchGrid(options);
    //jQuery("#list").jqGrid('filterToolbar', { stringResult: false, searchOnEnter: false, defaultSearch: 'cn', ignoreCase: true });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'CategoryId');
                    return value;
                }
            }, url: "../api/Master/EditCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'CategoryId');
                    return value;
                }
            }, url: "../api/Master/DeleteCategory", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End Category


//Begin Sub Category

function SubCategoryGrid() {
    //SubCategoryId CategoryId  SubCategoryName  IsActive  IsDeleted  CreatedOn  UpdatedOn  CreatedBy  UpdatedBy
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetSubCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "SubCategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['SubCategoryId', 'SubCategoryName', 'CategoryName', 'CategoryName', 'IsActive', 'IsDeleted'],
        colModel: [
            { name: 'SubCategoryId', index: 'SubCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'SubCategoryName', index: 'SubCategoryName', width: 250, editable: true, editrules: { required: true }, edittype: 'text' },
            //           
            //                    { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            //                    { name: 'CategoryName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

            { name: 'CategoryId', index: 'CategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl: '../api/Master/GetCategoryListAsHtml', width: 150 } },
            { name: 'CategoryName', index: 'CategoryName', width: 200, viewable: true },

            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
            { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],
        //                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                      editoptions: { size: 12, dataInit: function (el) {
        //                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                      }
        //                      }, sorttype: "date"
        //                  },
        //               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                   editoptions: { size: 12, dataInit: function (el) {
        //                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                   }
        //                   }, sorttype: "date"
        //               },

        //                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: ' List Of Categories'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SubCategoryId');
                    return value;
                }
            }, url: "../api/Master/EditSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SubCategoryId');
                    return value;
                }
            }, url: "../api/Master/DeleteSubCategory", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End Sub Category


//Begin BusinessType
function BusinessGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetBusinessTypeList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "BusinessId",
        mtype: 'GET',
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['BusinessId', 'BusinessTypeName', 'BusinessTypeDescription'],
        colModel: [
            { name: 'BusinessId', index: 'BusinessId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'BusinessName', index: 'BusinessName', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'BusinessDescription', index: 'BusinessDescription', width: 150, editable: true, editrules: { required: true }, edittype: 'text' }],

        pager: '#pager',
        sortname: 'BusinessId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        //        autowidth: true,
        //        autoheight: true,
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of SubCategories '
    });


    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'BusinessId');
                    return value;
                }
            }, url: "../api/Master/EditBusinessType", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateBusinessType", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'BusinessId');
                    return value;
                }
            }, url: "../api/Master/DeleteBusinessType", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End BusinessType

//Begin FeaturesCategory

function FeaturesCategoryGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetFeaturesCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "FeaturesCategoryId",
        mtype: 'GET',
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['FeaturesCategoryId', 'FeaturesCategoryName', 'BusinessTypeName', 'BusinessTypeName'],
        colModel: [
            { name: 'FeaturesCategoryId', index: 'FeaturesCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'FeaturesCategoryName', index: 'FeaturesCategoryName', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'BusinessId', index: 'BusinessId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl: '../api/Master/GetBusinessTypeListAsHtml', width: 150 } },
            { name: 'BusinessName', index: 'BusinessName', width: 150, viewable: true }],
        pager: '#pager',
        sortname: 'FeaturesCategoryId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of FeaturesCategories '
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesCategoryId');
                    return value;
                }
            }, url: "../api/Master/EditFeaturesCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateFeaturesCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                    return value;
                }
            }, url: "../api/Master/DeleteFeaturesCategory", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End Features Category



//Begin  Features Sub Category

function FeaturesSubCategoryGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetFeaturesSubCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "FeaturesSubCategoryId",
        mtype: 'GET',
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['FeaturesSubCategoryId', 'FeaturesSubCategoryName', 'FeaturesCategoryName', 'FeaturesCategoryName'],
        colModel: [
            { name: 'FeaturesSubCategoryId', index: 'FeaturesSubCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'FeaturesSubCategoryName', index: 'FeaturesSubCategoryName', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },

            { name: 'FeaturesCategoryId', index: 'FeaturesCategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl: '../api/Master/GetFeaturesCategoryListAsHtml', width: 150 } },
            { name: 'FeaturesCategoryName', index: 'FeaturesCategoryName', width: 150, viewable: true }],
        pager: '#pager',
        sortname: 'FeaturesSubCategoryId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of  FeaturesSubCategories Records '
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                    return value;
                }
            }, url: "../api/Master/EditFeaturesSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateFeaturesSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                    return value;
                }
            }, url: "../api/Master/DeleteFeaturesSubCategory", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}

function EditProductGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetproductListAdmin',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "ProductId",
        mtype: 'GET',
        rowNum: 20,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['ProductId', 'SubCategoryId', 'ProductId', 'ProductName', 'ProductCost(Rs)', 'ProductDiscountPercentage(%)', 'updteQty', 'Action'],
        colModel: [
            { name: 'ProductId', index: 'ProductId', width: 80, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true, search: true },
            { name: 'SubCategoryId', index: 'SubCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'ProductId', index: 'ProductId', width: 80, editable: true, editrules: { required: false }, edittype: 'text' },
            { name: 'ProductName', index: 'ProductName', width: 250, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'ProductCost', index: 'ProductCost', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'ProductDiscountPercentage', index: 'ProductDiscountPercentage', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'ProductId', index: 'ProductId', edittype: 'select', width: 80, editable: false, sortable: false, formatter: returnUpdateQty },
            { name: 'ProductId', index: 'ProductId', edittype: 'select', width: 80, editable: false, sortable: false, formatter: returnMyLink }],
        pager: '#pager',

        sortname: 'CreatedOn',
        sortorder: 'asc',
        height: "100%",
        width: "150%",
        prmNames: { nd: null, search: null },
        caption: 'Product Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'ProductId');
                    return value;
                }
            },
            url: "../api/Master/EditProduct", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300,
            beforeShowForm: function (formid) {

                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var productId = $("#list").jqGrid('getCell', selId, 'ProductId');
                EditProductFeatureGrid(productId);
                EditProductSpecificationsGrid(productId);
                EditProductsGalleryGrid(productId);
            },

            afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'ProductId');
                    return value;
                }
            }, url: "../api/Master/DeleteProduct", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );

    function returnUpdateQty(cellValue, options, rowdata) {
        return "<a href='UpdateProductQuantity.aspx?ID=" + cellValue + "'>updteQty</a>";
    }

    function returnMyLink(cellValue, options, rowdata) {
        return "<a href='UpdateProducts.aspx?ID=" + cellValue + "'>Edit/Deactivate</a>";
    }

}
function GetCouponslist() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetCouponslist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = CoupData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#CoupList").show();
            }
        }
    });
}

function GetBox() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetBox',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divBoxGrid').html(data.Result);
            }
        }
    });
}
function DeleteBoxDetails(boxid) {
    if (confirm("Are you sure")) {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/DeleteBoxPacking?BoxId='+boxid,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            GetBox()
        }
    });
    }
}
function CoupData(Cashback) {

    var data = Cashback;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Coupon Name</th>";
    strHtml = strHtml + "<th>From Date</th>";
    strHtml = strHtml + "<th>To Date</th>";
    strHtml = strHtml + "<th>Coupon %</th>";
    strHtml = strHtml + "<th>Coupon Amount</th>";
    strHtml = strHtml + "<th>Total Times Usable</th>";
    strHtml = strHtml + "<th>Total Times Usable per Person</th>";
    strHtml = strHtml + "<th>Min qty</th>";
    strHtml = strHtml + "<th>Min Productvalue</th>";
    strHtml = strHtml + "<th>Max Cashback</th>";
    strHtml = strHtml + "<th>Newuser</th></tr ></thead > <tbody>";

    for (list in Cashback) {

        strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='UpdateCoupons.aspx?ID=" + Cashback[x].Coup_ID + "'>" + Cashback[x].Coup_ID + "</a></td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Coup_Code + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].From_Date + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].To_Date + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Coup_Percentage + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Coup_Amount + "</td>";

        strHtmltr = strHtmltr + "<td>" + Cashback[x].Total_Times_Usable + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Total_Times_Usable_Per_User + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Min_Qty + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Min_Prod_Val + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].Max_Cashback + "</td>";
        strHtmltr = strHtmltr + "<td>" + Cashback[x].New_User + "</td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}
// end product

function EverydayPerformanceGraphJS() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/EverydayPerformanceGraphMC',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            alert(data.Status);
            if (data.Status == "Success") {
                var chart = new CanvasJS.Chart("chartContainer", {
                    animationEnabled: true,
                    title: {
                        text: "HealthUrWealth - Monthly Sales report",
                        fontFamily: "arial black",
                        fontColor: "#695A42"
                    },
                    axisX: {
                        interval: 1,
                        intervalType: "year"
                    },
                    axisY: {
                        valueFormatString: "$#0bn",
                        gridColor: "#B6B1A8",
                        tickColor: "#B6B1A8"
                    },
                    toolTip: {
                        shared: true,
                        content: toolTipContent
                    },
                    data: [{
                        type: "stackedColumn",
                        showInLegend: true,
                        color: "#696661",
                        name: "Q1",
                        dataPoints: [
                            { y: data1[0], x: new Date(date[0], 0) },
                            { y: data1[1], x: new Date(date[1], 0) },
                            { y: data1[2], x: new Date(date[2], 0) },
                            { y: data1[3], x: new Date(date[3], 0) },
                            { y: data1[4], x: new Date(date[4], 0) },
                            { y: data1[5], x: new Date(date[5], 0) },
                            { y: data1[6], x: new Date(date[6], 0) },
                            { y: data1[7], x: new Date(date[7], 0) },
                            { y: data1[8], x: new Date(date[8], 0) },
                            { y: data1[9], x: new Date(date[9], 0) },
                            { y: data1[10], x: new Date(date[10], 0) },
                            { y: data1[11], x: new Date(date[11], 0) },
                            { y: data1[12], x: new Date(date[12], 0) },
                            { y: data1[13], x: new Date(date[13], 0) },
                            { y: data1[14], x: new Date(date[14], 0) },
                            { y: data1[15], x: new Date(date[15], 0) },
                            { y: data1[16], x: new Date(date[16], 0) },
                            { y: data1[17], x: new Date(date[17], 0) },
                            { y: data1[18], x: new Date(date[18], 0) },
                            { y: data1[19], x: new Date(date[19], 0) },
                            { y: data1[20], x: new Date(date[20], 0) },
                            { y: data1[21], x: new Date(date[21], 0) },
                            { y: data1[22], x: new Date(date[22], 0) },
                            { y: data1[23], x: new Date(date[23], 0) },
                            { y: data1[24], x: new Date(date[24], 0) },
                            { y: data1[25], x: new Date(date[25], 0) },
                            { y: data1[26], x: new Date(date[26], 0) },
                            { y: data1[27], x: new Date(date[27], 0) },
                            { y: data1[28], x: new Date(date[28], 0) },
                            { y: data1[29], x: new Date(date[29], 0) },
                            { y: data1[30], x: new Date(date[30], 0) },
                            { y: data1[31], x: new Date(date[31], 0) }
                        ]
                    },
                    {
                        type: "stackedColumn",
                        showInLegend: true,
                        name: "Q2",
                        color: "#EDCA93",
                        dataPoints: [
                            { y: data2[0], x: new Date(date[0], 0) },
                            { y: data2[1], x: new Date(date[1], 0) },
                            { y: data2[2], x: new Date(date[2], 0) },
                            { y: data2[3], x: new Date(date[3], 0) },
                            { y: data2[4], x: new Date(date[4], 0) },
                            { y: data2[5], x: new Date(date[5], 0) },
                            { y: data2[6], x: new Date(date[6], 0) },
                            { y: data2[7], x: new Date(date[7], 0) },
                            { y: data2[8], x: new Date(date[8], 0) },
                            { y: data2[9], x: new Date(date[9], 0) },
                            { y: data2[10], x: new Date(date[10], 0) },
                            { y: data2[11], x: new Date(date[11], 0) },
                            { y: data2[12], x: new Date(date[12], 0) },
                            { y: data2[13], x: new Date(date[13], 0) },
                            { y: data2[14], x: new Date(date[14], 0) },
                            { y: data2[15], x: new Date(date[15], 0) },
                            { y: data2[16], x: new Date(date[16], 0) },
                            { y: data2[17], x: new Date(date[17], 0) },
                            { y: data2[18], x: new Date(date[18], 0) },
                            { y: data2[19], x: new Date(date[19], 0) },
                            { y: data2[20], x: new Date(date[20], 0) },
                            { y: data2[21], x: new Date(date[21], 0) },
                            { y: data2[22], x: new Date(date[22], 0) },
                            { y: data2[23], x: new Date(date[23], 0) },
                            { y: data2[24], x: new Date(date[24], 0) },
                            { y: data2[25], x: new Date(date[25], 0) },
                            { y: data2[26], x: new Date(date[26], 0) },
                            { y: data2[27], x: new Date(date[27], 0) },
                            { y: data2[28], x: new Date(date[28], 0) },
                            { y: data2[29], x: new Date(date[29], 0) },
                            { y: data2[30], x: new Date(date[30], 0) },
                            { y: data2[31], x: new Date(date[31], 0) }
                        ]
                    },
                    {
                        type: "stackedColumn",
                        showInLegend: true,
                        name: "Q3",
                        color: "#695A42",
                        dataPoints: [
                            { y: data3[0], x: new Date(date[0], 0) },
                            { y: data3[1], x: new Date(date[1], 0) },
                            { y: data3[2], x: new Date(date[2], 0) },
                            { y: data3[3], x: new Date(date[3], 0) },
                            { y: data3[4], x: new Date(date[4], 0) },
                            { y: data3[5], x: new Date(date[5], 0) },
                            { y: data3[6], x: new Date(date[6], 0) },
                            { y: data3[7], x: new Date(date[7], 0) },
                            { y: data3[8], x: new Date(date[8], 0) },
                            { y: data3[9], x: new Date(date[9], 0) },
                            { y: data3[10], x: new Date(date[10], 0) },
                            { y: data3[11], x: new Date(date[11], 0) },
                            { y: data3[12], x: new Date(date[12], 0) },
                            { y: data3[13], x: new Date(date[13], 0) },
                            { y: data3[14], x: new Date(date[14], 0) },
                            { y: data3[15], x: new Date(date[15], 0) },
                            { y: data3[16], x: new Date(date[16], 0) },
                            { y: data3[17], x: new Date(date[17], 0) },
                            { y: data3[18], x: new Date(date[18], 0) },
                            { y: data3[19], x: new Date(date[19], 0) },
                            { y: data3[20], x: new Date(date[20], 0) },
                            { y: data3[21], x: new Date(date[21], 0) },
                            { y: data3[22], x: new Date(date[22], 0) },
                            { y: data3[23], x: new Date(date[23], 0) },
                            { y: data3[24], x: new Date(date[24], 0) },
                            { y: data3[25], x: new Date(date[25], 0) },
                            { y: data3[26], x: new Date(date[26], 0) },
                            { y: data3[27], x: new Date(date[27], 0) },
                            { y: data3[28], x: new Date(date[28], 0) },
                            { y: data3[29], x: new Date(date[29], 0) },
                            { y: data3[30], x: new Date(date[30], 0) },
                            { y: data3[31], x: new Date(date[31], 0) }
                        ]
                    },
                    {
                        type: "stackedColumn",
                        showInLegend: true,
                        name: "Q4",
                        color: "#B6B1A8",
                        dataPoints: [
                            { y: data4[0], x: new Date(date[0], 0) },
                            { y: data4[1], x: new Date(date[1], 0) },
                            { y: data4[2], x: new Date(date[2], 0) },
                            { y: data4[3], x: new Date(date[3], 0) },
                            { y: data4[4], x: new Date(date[4], 0) },
                            { y: data4[5], x: new Date(date[5], 0) },
                            { y: data4[6], x: new Date(date[6], 0) },
                            { y: data4[7], x: new Date(date[7], 0) },
                            { y: data4[8], x: new Date(date[8], 0) },
                            { y: data4[9], x: new Date(date[9], 0) },
                            { y: data4[10], x: new Date(date[10], 0) },
                            { y: data4[11], x: new Date(date[11], 0) },
                            { y: data4[12], x: new Date(date[12], 0) },
                            { y: data4[13], x: new Date(date[13], 0) },
                            { y: data4[14], x: new Date(date[14], 0) },
                            { y: data4[15], x: new Date(date[15], 0) },
                            { y: data4[16], x: new Date(date[16], 0) },
                            { y: data4[17], x: new Date(date[17], 0) },
                            { y: data4[18], x: new Date(date[18], 0) },
                            { y: data4[19], x: new Date(date[19], 0) },
                            { y: data4[20], x: new Date(date[20], 0) },
                            { y: data4[21], x: new Date(date[21], 0) },
                            { y: data4[22], x: new Date(date[22], 0) },
                            { y: data4[23], x: new Date(date[23], 0) },
                            { y: data4[24], x: new Date(date[24], 0) },
                            { y: data4[25], x: new Date(date[25], 0) },
                            { y: data4[26], x: new Date(date[26], 0) },
                            { y: data4[27], x: new Date(date[27], 0) },
                            { y: data4[28], x: new Date(date[28], 0) },
                            { y: data4[29], x: new Date(date[29], 0) },
                            { y: data4[30], x: new Date(date[30], 0) },
                            { y: data4[31], x: new Date(date[31], 0) }
                        ]
                    }]
                });
                chart.render();

                function toolTipContent(e) {
                    var str = "";
                    var total = 0;
                    var str2, str3;
                    for (var i = 0; i < e.entries.length; i++) {
                        var str1 = "<span style= \"color:" + e.entries[i].dataSeries.color + "\"> " + e.entries[i].dataSeries.name + "</span>: $<strong>" + e.entries[i].dataPoint.y + "</strong>bn<br/>";
                        total = e.entries[i].dataPoint.y + total;
                        str = str.concat(str1);
                    }
                    str2 = "<span style = \"color:DodgerBlue;\"><strong>" + (e.entries[0].dataPoint.x).getFullYear() + "</strong></span><br/>";
                    total = Math.round(total * 100) / 100;
                    str3 = "<span style = \"color:Tomato\">Total:</span><strong> $" + total + "</strong>bn<br/>";
                    return (str2.concat(str)).concat(str3);
                }
            }
        }
    });
}
function Remitting_Excell_upload() {
    var strHtml = "";
    strHtml = strhtml + "<input type='file' id='excelfile' />";
    strHtml = strhtml + "<input type='button' id='viewfile' value='Export To Table' onclick='ExportToTable()' />";
    strHtml = strhtml + "<br />";
    strHtml = strhtml + "<br />";
    strHtml = strhtml + "<table id='exceltable'>";
    strHtml = strhtml + "</table>";
    $('#pager').html(strHtml);
}
function Remitting_ajax() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetRoutinecustomers',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            alert(data);
            if (data.Status == "Success") {
                var prddata = RoutineData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#PrdctList").show();
            }
        }
    });
}
function Remitting_data_binder(data) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Product Name</th>";
    strHtml = strHtml + "<th>Qty</th></tr></thead><tbody>";
    //strHtml = strHtml + "<th>Dis.(%)</th>";
    //strHtml = strHtml + "<th>Cost</th>";
    //strHtml = strHtml + "<th>Activate</th>";

    for (list in Product) {

        //strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='UpdateProducts.aspx?ID=" + Product.productId + "'>" + Product.productId + "</a></td>";
        strHtmltr = strHtmltr + "<tr><td>" + Product[x].productid + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].userid + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].count + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        //strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        //strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}
function Routinecustomers() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetRoutinecustomers',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = RoutineData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#PrdctList").show();
            }
        }
    });
}
function RoutineData(Product) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    var y = 1;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>S.No</th>";
    strHtml = strHtml + "<th>User Name</th>";
    strHtml = strHtml + "<th>E-Mail</th>";
    strHtml = strHtml + "<th>Mobile No</th>";
    strHtml = strHtml + "<th>Product Name</th>";
    strHtml = strHtml + "<th>Price</th>";
    //strHtml = strHtml + "<th>Total Cost</th>";
    strHtml = strHtml + "<th>Estimated Date</th>";
    strHtml = strHtml + "<th>Rejected</th></tr ></thead ><tbody>";

    for (list in Product) {

        //strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='UpdateProducts.aspx?ID=" + Product.productId + "'>" + Product.productId + "</a></td>";
        strHtmltr = strHtmltr + "<tr><td>" + y + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].FirstName + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Emailid + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].MobileNo + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].ProductName + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].Totalcost + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].CreatedOn + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].CreatedOn + "</td>";
        if (Product[x].Status == true) {
            strHtmltr = strHtmltr + "<td><input type='checkbox' class='checkbox' name='checkboxesforcust' id=checkbox_" + Product[x].userid + "_" + Product[x].ProductId + " checked='checked' onclick='Checkboxajaxcall(" + Product[x].userid + "," + Product[x].ProductId + ",this.id)'></td>";
        }
        else {
            strHtmltr = strHtmltr + "<td><input type='checkbox' class='checkbox' name='checkboxesforcust' id=checkbox_" + Product[x].userid + "_" + Product[x].ProductId + " onclick='Checkboxajaxcall(" + Product[x].userid + "," + Product[x].ProductId + ",this.id)'></td>";
        }
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        //strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        //strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++;
        y++;
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}
function ActivateProducts() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetDeactivatedProductList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = ActivateData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#PrdctList").show();
            }
        }
    });
}

function ActivateData(Product) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Product Name</th>";
    strHtml = strHtml + "<th>Qty</th>";
    strHtml = strHtml + "<th>Dis.(%)</th>";
    strHtml = strHtml + "<th>Cost</th>";
    strHtml = strHtml + "<th>Activate</th></tr></thead><tbody>";

    for (list in Product) {
        var color = "";
        if (Product[x].Quantity == 0) {
            color = "Red";
        }
        else if (Product[x].Quantity == 1) {
            color = "#ffbf00";
        }
        else if (Product[x].Quantity > 1) {
            color = "Green";
        }
        strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>" + Product[x].ProductId + "</a></td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].ProductName + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].Quantity + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].ProductDiscountPercentage + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        //strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        //strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}


function GetProductID() {
    var ProductID = $('#txtProductID').val();
    $.ajax({
        datatype: 'json',
        url: '../api/Master/GetDeactivatedProductID?ProductID=' + ProductID,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = ActivateData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#PrdctList").show();
            }
            else {
                $('#PrdctList').empty();
                $('#PrdctList').append(data.Result);
                $("#PrdctList").hide();
            }
        }
    });

}
//Begin Role

function RoleGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetRoleList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "RoleId",
        mtype: 'GET',
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['RoleId', 'RoleName', 'RoleDescription', 'IsActive', 'IsDeleted'],
        colModel: [
            { name: 'RoleId', index: 'RoleId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true, search: true },
            { name: 'RoleName', index: 'RoleName', width: 100, editable: true, editrules: { required: true }, edittype: 'text', search: true },
            { name: 'RoleDescription', index: 'RoleDescription', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
            { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],
        //                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                      editoptions: { size: 12, dataInit: function (el) {
        //                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                      }
        //                      }, sorttype: "date"
        //                  },
        //               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                   editoptions: { size: 12, dataInit: function (el) {
        //                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                   }
        //                   }, sorttype: "date"
        //               }],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        //search: true,
        //prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of Roles'
    });



    $("#list").jqGrid('navGrid', '#pager', { search: true, edit: true, add: true, del: true },

        //            $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false, defaultSearch: 'cn' }),


        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'RoleId');
                    return value;
                }
            }, url: "../api/Master/EditRole", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateRole", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'RoleId');
                    return value;
                }
            }, url: "../api/Master/DeleteRole", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End Role

// user grid
function UsersGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetUsersList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "UserId",
        mtype: 'GET',
        rowNum: 20,
        autosize: true,
        rowList: [20, 40, 60, 80, 100],
        viewrecords: true,
        colNames: ['UserId', 'RoleId', 'FirstName', 'LastName', 'EmailId', 'MobileNo', 'RegStatus', 'IsActive'],
        colModel: [
            { name: 'UserId', index: 'UserId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'RoleId', index: 'RoleId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'FirstName', index: 'FirstName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'LastName', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'EmailId', index: 'EmailId', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'MobileNo', index: 'MobileNo', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'RegStatus', index: 'RegStatus', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],

        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'CreatedOn',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Users Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'UserId');
                    return value;
                }
            }, url: "../api/Master/EditUsers", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateUsers", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'UserId');
                    return value;
                }
            }, url: "../api/Master/DeleteUsers", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );
}
//End Users

// MonthlyWiseUserProductTransactions
function MonthlyWiseUserProductTransactionsGrid() {

    var dateString = $.datepicker.formatDate("yy-mm-dd", $("#date-start").datepicker("getDate"));
    var fromDate = new Date(dateString);
    dateString = $.datepicker.formatDate("yy-mm-dd", $("#date-end").datepicker("getDate"));
    var toDate = new Date(dateString);
    var OrderIdTxt = document.getElementById('txtOrderID').value;
    var UsrMailIDTxt = document.getElementById('txtUserMailID').value;
    if (OrderIdTxt == "") {
        OrderIdTxt = 0;
    }
    $('#list').empty();
    $('#list').jqGrid('GridUnload');
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetMonthlyWiseUserProductTransactionList?fromDate=' + fromDate.toUTCString() + '&toDate=' + toDate.toUTCString() + '&OrderIdTxt=' + OrderIdTxt + '&UsrMailIDTxt=' + UsrMailIDTxt,
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "TransactionId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['Order Id', 'Products', 'Quantity', 'Currency', 'Amount', 'Message'],
        colModel: [
            { name: 'PaymentTransactionId', index: 'PaymentTransactionId', width: 100, edittype: 'select', editable: true, formatter: returnMyLink },
            //                    { name: 'PGTxnId', index: 'PGTxnId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false },
            { name: 'products', index: 'products', width: 0, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'Quantity', index: 'Quantity', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'currency', index: 'currency', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'TxnAmount', index: 'TxnAmount', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'TxnMessage', index: 'TxnMessage', width: 200, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: false, visable: true }],
        //                    { name: 'TxnStatus', index: 'TxnStatus', width: 100, editable: true, editrules: { required: true }, edittype: 'text', visable: true },
        //                    {name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', datatype: "local", formatoptions: { srcformat: 'Y-m-dTH:i:s', newformat: 'd/m/Y H:i:s' },

        //                    editoptions: { size: 12, dataInit: function (el) {
        //                        setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                    }
        //                    }, sorttype: "date"
        //                } ],
        //{ name: 'PaymentTransactionId', index: 'PaymentTransactionId', edittype: 'select', width: 130, editable: false, sortable: false, formatter: returnMyLink}],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'CreatedOn',
        emptyrecords: 'No Records Found.',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'UserProductTransaction Records',
        viewrecords: true
    });


    $("#list").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false });

    document.getElementById('txtOrderID').value = "";
    document.getElementById('txtUserMailID').value = "";
}
//End UserProductTransactions

function returnMyLink(cellValue, options, rowdata) {
    return "<a href='Orders.aspx?ID=" + cellValue + "'>" + cellValue + " </a>";

}

//End UserProductTransactions


//Begin States

function StatesGrid() {
    //SubCategoryId CategoryId  SubCategoryName  IsActive  IsDeleted  CreatedOn  UpdatedOn  CreatedBy  UpdatedBy
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetStatesList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "StateId",
        mtype: 'GET',
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['StateId', 'StateName', 'CountryId', 'CountryName', 'IsActive', 'IsDeleted'],
        colModel: [
            { name: 'StateId', index: 'StateId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'StateName', index: 'StateName', width: 200, editable: true, editrules: { required: true }, edittype: 'text' },
            //           
            //                    { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            //                    { name: 'CategoryName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

            { name: 'CountryId', index: 'CountryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl: '../api/Master/GetCountryListAsHtml', width: 150 } },
            { name: 'CountryName', index: 'CountryName', width: 100, viewable: true },

            { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
            { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' }],
        //                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                      editoptions: { size: 12, dataInit: function (el) {
        //                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                      }
        //                      }, sorttype: "date"
        //                  },
        //               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
        //                   editoptions: { size: 12, dataInit: function (el) {
        //                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
        //                   }
        //                   }, sorttype: "date"
        //               },

        //                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of States'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'StateId');
                    return value;
                }
            }, url: "../api/Master/EditStates", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateStates", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'StateId');
                    return value;
                }
            }, url: "../api/Master/DeleteStates", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );


}
//End States

//Begin pincode
function PinCodeGrid() {
    //SubCategoryId CategoryId  SubCategoryName  IsActive  IsDeleted  CreatedOn  UpdatedOn  CreatedBy  UpdatedBy
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/GetPincodes',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "PinCodeId",
        mtype: 'GET',
        rowNum: 10,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['PinCodeId', 'Pincode', 'District'],
        colModel: [
            { name: 'PinCodeId', index: 'PinCodeId', editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
            { name: 'Pincode', index: 'Pincode', editable: true, editrules: { required: true }, edittype: 'text' },

            { name: 'District', index: 'District', viewable: true },
        ],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: 600,
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'List Of Pincodes'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'PinCodeId');
                    return value;
                }
            }, url: "../api/Master/EditStates", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            url: "../api/Master/CreateStates", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterAdd: true, left: 450, top: 300, width: 520, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully saved");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'PinCodeId');
                    return value;
                }
            }, url: "../api/Master/DeletePin", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );
}
//End pincode

//Begin paymentreports
function PaymentReports() {
    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/PaymentReports',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "PaymentTransactionId",
        mtype: 'GET',
        search: true,
        rowNum: 5,
        loadonce: true,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['PaymentTransactionId', 'UserId', 'TxnRefNo', 'TxnStatus', 'TxnMessage', 'TxnAmount', 'CurrencyCode'],
        colModel: [
            { name: 'PaymentTransactionId', index: 'PaymentTransactionId', width: 100, viewable: true },
            { name: 'UserId', index: 'UserId', width: 70, viewable: true },
            { name: 'TxnRefNo', index: 'TxnRefNo', width: 100, viewable: true },

            { name: 'TxnStatus', index: 'TxnStatus', width: 100, viewable: true },
            { name: 'TxnMessage', index: 'TxnMessage', width: 100, viewable: true },
            { name: 'TxnAmount', index: 'TxnAmount', width: 100, viewable: true },
            { name: 'CurrencyCode', index: 'CurrencyCode', width: 100, viewable: true },
        ],
        pager: '#pager',
        sortname: 'TakenOn',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Payment Reports'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true }


    );
    $("#search").click(function () {
        var searchFiler = $("#filter").val(), grid = $("#list"), f;

        if (searchFiler.length === 0) {
            grid[0].p.search = false;
            $.extend(grid[0].p.postData, { filters: "" });
        }
        f = { groupOp: "OR", rules: [] };
        f.rules.push({ field: "name", op: "cn", data: searchFiler });
        f.rules.push({ field: "note", op: "cn", data: searchFiler });
        grid[0].p.search = true;
        $.extend(grid[0].p.postData, { filters: JSON.stringify(f) });
        grid.trigger("reloadGrid", [{ page: 1, current: true }]);
    });


}
//End paymentreports

//Begin approvereviews
function GetProductReviews() {
    var rows;
    var minval = 5;
    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetProductReviews?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#tblreviews').html('');
                $('#tblreviews').html(data.Result);
            }
            if (data.Status == "NoData") {
                $('#tblreviews').append(data.Result);
            }
        }
    });
}
//End approve review

//IsApproveReview 
function AdminIsApproveReview(ReviewId) {
    $.ajax({
        url: '../api/Master/IsApproveReview?rid=' + ReviewId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert(data.Message);
            }
            else {
                ShowInfo(data, "#notification");
            }
        }
    });
}
//End IsApproveReview

//delete review
function deleteFromCart(product_id) {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: "../api/Master/DeleteFromCart?id=" + product_id,
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
//end

function GetProduct() {

    $('#main').empty();
    pid = getParameterByName("id");
    if (pid == undefined || pid == null || pid == "") {
        $('#main').html(" Please select the Product to see the details. ");
    }
    else {
        jQuery.support.cors = true;
        $.ajax({
            url: '../api/Master/GetProductInfo?productId=' + pid,
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
                if (data.Status == "Fail") {
                    alert(data.Status);
                    $('#main').append(data.Result);
                }
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
}

function GetPaymentPendingOrder() {
    var rows;
    var minval = 5;
    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetPaymentPendingOrder?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
            }
        }
    });
}

function GetPerformanceIndicator() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    $.ajax({
        url: '../api/Master/GetPerformanceIndicator',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPerformanceIndicator').empty();
                ShowInfo(data, '#divPerformanceIndicator');
                div.remove();
            }
            if (data.Status == "NoData") {
                $('#divPerformanceIndicator').append(data.Result);
                div.remove();
            }
        }
    });
}
function GetRTOOrders() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetRTOOrders?rows=120',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divReturnPaymentOrdersGrd').empty();
                ShowInfo(data, '#divReturnPaymentOrdersGrd');
                div.remove();
                $("#dvbtn").hide();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divReturnPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetAuthorizedOrders() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetAuthorizedOrders?rows=120',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                div.remove();
                $("#dvbtn").hide();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}
function GetEmployeelist() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;

    var minval = 50;

    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetEmployeelist?rows=50',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divempGrd').empty();
                ShowInfo(data, '#divempGrd');
                div.remove();
                $("#dvbtn").hide();
                $("#dvbtn").hide();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divempGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetUnAuthorizedOrders() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetUnAuthorizedOrders?rows=1000',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetUserCancelledOrders() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetUserCancelledOrders?rows=1000',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetAdminCancelledOrders() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetAdminCancelledOrders?rows=1000',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetSalesReport() {
    //getsales();
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    if ($('#ContentPlaceHolder1_txtFromDate').val() != '' && $('#ContentPlaceHolder1_txtToDate').val() != '') {
        $.ajax({
            url: '../api/Master/GetSalesReport?rows=500000&FromDate=' + $('#ContentPlaceHolder1_txtFromDate').val() + '&ToDate=' + $('#ContentPlaceHolder1_txtToDate').val() + '&CurrentStatus=' + $('#ContentPlaceHolder1_drpOrderStatuss').val(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#divPaymentOrdersGrd').empty();
                    ShowInfo(data, '#divPaymentOrdersGrd');
                    $("#dvbtn").hide();
                    div.remove();
                }
                if (data.Status == "NoData") {
                    $('#divPaymentOrdersGrd').append(data.Result);
                    div.remove();
                }
            }
        });
    }
    else {
        $.ajax({
            url: '../api/Master/GetSalesReport?rows=500000',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#divPaymentOrdersGrd').empty();
                    ShowInfo(data, '#divPaymentOrdersGrd');
                    $("#dvbtn").hide();
                    div.remove();
                }
                if (data.Status == "NoData") {
                    $('#divPaymentOrdersGrd').append(data.Result);
                    div.remove();
                }
            }
        });
    }
}

function SalesReport(FromDate, ToDate, Status) {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetSalesReport?rows=500000&FromDate=' + FromDate + '&ToDate=' + ToDate + '&CurrentStatus=' + Status,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function GetProductSalesReport() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    if ($('#txtFromDate').val() != '' && $('#txtToDate').val() != '' || $('#drpOrderStatuss').val() != 0) {
        $.ajax({
            url: '../api/Master/GetProductSalesReport?rows=5000&FromDate=' + $('#txtFromDate').val() + '&ToDate=' + $('#txtToDate').val(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#divPaymentOrdersGrd').empty();
                    ShowInfo(data, '#divPaymentOrdersGrd');
                    $("#dvbtn").hide();
                }
                if (data.Status == "NoData") {
                    $('#divPaymentOrdersGrd').append(data.Result);
                }
            }
        });
    }
    else {
        $.ajax({
            url: '../api/Master/GetSalesReport?rows=500000',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $('#divPaymentOrdersGrd').empty();
                    ShowInfo(data, '#divPaymentOrdersGrd');
                    $("#dvbtn").hide();
                    div.remove();
                }
                if (data.Status == "NoData") {
                    $('#divPaymentOrdersGrd').append(data.Result);
                    div.remove();
                }
            }
        });
    }
}

function ProductSalesReport(FromDate, ToDate) {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetProductSalesReport?rows=5000&FromDate=' + FromDate + '&ToDate=' + ToDate,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}

function DispatchedCheckboxChecked(dis) {
    $('.Check').on('change', function () {
        $('.Check').not(this).prop('checked', false);
    });

    $("#lblPtrnsId").val($(dis).val());
    $("#txtBlkOrderNo").val($(dis).val());
    var v = new Array($('input[class=Check][type=checkbox]:checked').length);
    var i = 0;
    $('input[class=Check][type=checkbox]:checked').each(function () {
        $(this).closest('tr').addClass("checked");
        v[i] = $(this).val();
        i++;
    });
    //$('#lblSelectedboxes').text(i + " Order(s) Selected.");
    $("#lblPtrnsId").val(v);
    $("#txtBlkOrderNo").val(v);
    var checked = $(dis).is(':checked');
    if (checked == true) {
        $("#dvbtn").show();
    }
    if (checked == false) {
        $("#dvbtn").hide();
        $(dis).closest('tr').removeClass('checked');
    }
    $('input[class=Check][type=checkbox]').each(function () {
        dis.checked = false;
    });
    if (checked) {
        dis.checked = true;
    }
}

function CheckboxChecked(dis) {
    $("#lblPtrnsId").val($(dis).val());
    $("#txtBlkOrderNo").val($(dis).val());
    var v = new Array($('input[class=Check][type=checkbox]:checked').length);
    var i = 0;
    $('input[class=Check][type=checkbox]:checked').each(function () {
        $(this).closest('tr').addClass("checked");
        v[i] = $(this).val();
        i++;
    });
    $('#lblSelectedboxes').text(i + " Order(s) Selected.");
    $("#lblPtrnsId").val(v);
    $("#txtBlkOrderNo").val(v);
    var checked = $(dis).is(':checked');
    if (checked == true) {
        $("#dvbtn").show();
    }
    if (checked == false) {
        $("#dvbtn").hide();
        $(dis).closest('tr').removeClass('checked');
    }
    $('input[class=Check][type=checkbox]').each(function () {
        dis.checked = false;
    });
    if (checked) {
        dis.checked = true;
    }
}

function ShipmentOrder(Id, Courier) {
    $.ajax({
        url: '../api/Master/GetShipmentDetails?OrderID=' + Id + '&CourierName=' + Courier + '&awb=EcomTest',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#divPaymentOrdersGrd').empty();
            $('#divMsg').html("Shipment Successfully Created.").show();
            //GetAuthorizedOrders();
            $("#dvbtn").hide();
            if (data.CourierName == "Aramex") {
                window.open(data.Comments);
            }
        }
    });
}

function ShipmentOrderEcom(Id, Courier) {

    var awbData = 'username=sonal871439&password=snp87psdn4npade9m&count=1&type=PPD';
    $.ajax({
        url: 'https://api.ecomexpress.in/apiv2/fetch_awb/',
        type: 'Post',
        crossDomain: true,
        data: awbData,
        dataType: "json",
        success: function (info) {

            $.ajax({
                url: '../api/Master/GetShipmentDetails?OrderID=' + Id + '&CourierName=' + Courier + '&awb=' + info.awb,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#divPaymentOrdersGrd').empty();
                    $('#divMsg').html("Shipment Successfully Created.").show();
                    //GetAuthorizedOrders();
                    $("#dvbtn").hide();
                    if (data.CourierName == "Aramex") {
                        window.open(data.Comments);
                    }
                }
            });
        },
        error: function (xhr) {
            alert(xhr);
        }
    });
    //$('#divPaymentOrdersGrd').empty();
    //$('#divMsg').html("Shipment Successfully Created.").show();
    //$("#dvbtn").hide();

}

//start Super Categories //
//Get Super Categories
function GetSuperCategories() {
    var rows;
    var minval = 5;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetAllSuperCategories?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divSuperCategories').empty();
                ShowInfo(data, '#divSuperCategories');
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divSuperCategories').append(data.Result);
            }
        }
    });
}


function SuperCatCheckboxChecked(dis) {

    $("#txtSuperCatgryID").val($(dis).val());
    var SuprCatID = $(dis).val();
    $.ajax({
        url: '../api/Master/GetSuperCategory?ID=' + SuprCatID,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                // $("#txtSuperCatgryName").val($(dis).val());
                var dta = data;
                $("#txtSuperCatgryName").val(dta.Result[0].SuperCategoryName);
                // $("#ddlIsActive").val(dta.Result[0].IsActive);
                // $("#ddlIsActive").text(dta.Result[0].IsActive);
                var sts = dta.Result[0].IsActive;
                $("#ddlIsActive option:contains(" + sts + ")").attr('selected', 'selected');
            }
            if (data.Status == "NoData") {
            }
        }

    });

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

function UpdateSuperCategory() {
    var cartJson = { 'SuperCategoryId': $("#txtSuperCatgryID").val(), 'SuperCategoryName': $("#txtSuperCatgryName").val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateSuperCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Super Category Updated successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Super Category not Updated successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });

}

function AddSuperCategory() {
    var cartJson = { 'SuperCategoryName': $("#txtSuperCatgryName").val(), 'SuperCategoryId': $('#ddlSuperCatogeries option:selected').val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/AddSuperCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Super Category Added successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Super Category not Added successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });
}
//End Super Categories//

//  start Categories //
function GetCategories() {
    var rows;
    var minval = 5;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetAllCategories?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divCategories').empty();
                ShowInfo(data, '#divCategories');
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divCategories').append(data.Result);
            }
        }
    });
}

function CatCheckboxChecked(dis) {

    $("#txtCatgryID").val($(dis).val());
    var CatID = $(dis).val();
    $.ajax({
        url: '../api/Master/GetCategory?ID=' + CatID,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                // $("#txtSuperCatgryName").val($(dis).val());
                var dta = data;
                $("#txtCatgryName").val(dta.Result[0].CategoryName);
                // $("#ddlIsActive").val(dta.Result[0].IsActive);
                $("#ddlSuperCatogeries option:contains(" + dta.Result[0].SuperCategoryName + ")").attr('selected', 'selected');
                var sts = dta.Result[0].IsActive;
                $("#ddlIsActive option:contains(" + sts + ")").attr('selected', 'selected');
            }
            if (data.Status == "NoData") {
            }
        }

    });

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

function AddCategory() {
    var cartJson = { 'CategoryName': $("#txtCatgryName").val(), 'SuperCategoryName': $('#ddlSuperCatogeries option:selected').text(), 'SuperCategoryId': $('#ddlSuperCatogeries option:selected').val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/AddCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Category Added successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Category not Added successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });
}


function UpdateCategory() {
    var cartJson = { 'CategoryId': $("#txtCatgryID").val(), 'SuperCategoryId': $('#ddlSuperCatogeries option:selected').val(), 'CategoryName': $("#txtCatgryName").val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Category Updated successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Category not Updated successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });

}
//  End  Categories //


//  start Sub Categories //
function GetSubCategories() {
    var rows;
    var minval = 5;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);;

    $.ajax({
        url: '../api/Master/GetAllSubCategories?rows=' + rows,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divSubCategories').empty();
                ShowInfo(data, '#divSubCategories');
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divSubCategories').append(data.Result);
            }
        }
    });
}

function SubCatCheckboxChecked(dis) {

    $("#txtCatgryID").val($(dis).val());
    var CatID = $(dis).val();
    $.ajax({
        url: '../api/Master/GetSubCategory?ID=' + CatID,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                // $("#txtSuperCatgryName").val($(dis).val());
                var dta = data;
                $("#txtSubCatgryID").val(dta.Result[0].SubCategoryId);
                $("#txtSubCatgryName").val(dta.Result[0].SubCategoryName);
                $("#ddlSuperCatogeries option:contains(" + dta.Result[0].SuperCategoryName + ")").attr('selected', 'selected');
                $("#ddlCatogeries option:contains(" + dta.Result[0].CategoryName + ")").attr('selected', 'selected');
                var sts = dta.Result[0].IsActive;
                $("#ddlIsActive option:contains(" + sts + ")").attr('selected', 'selected');
            }
            if (data.Status == "NoData") {
            }
        }

    });

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

function AddSubCategory() {
    var cartJson = { 'SubCategoryName': $("#txtSubCatgryName").val(), 'CategoryName': $('#ddlCatogeries option:selected').text(), 'CategoryId': $('#ddlCatogeries option:selected').val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/AddSubCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Sub Category Added successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Sub Category not Added successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });
}


function UpdateSubCategory() {
    var cartJson = { 'SubCategoryId': $("#txtSubCatgryID").val(), 'CategoryId': $('#ddlCatogeries option:selected').val(), 'SubCategoryName': $("#txtSubCatgryName").val(), 'IsActive': $('#ddlIsActive option:selected').text() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateSubCategory',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>SubCategory Updated successfully.";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetSuperCategories();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span> SubCategory not Updated successfully";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });

}
//  End Sub Categories //


// Start Get SuperCategories and Categories in dropdown List //
function GetSuperCategoriesInddl() {
    var items = $("#ddlSuperCatogeries option").length;
    if (items <= 0) {
        //Default item 

        var newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#ddlSuperCatogeries').append(newOption);

        $.ajax({
            url: '../api/Master/GetSuperCategries',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $.each(data.Result, function (index, item) {

                        newOption = $('<option>');
                        newOption.attr('value', item.SuperCategoryId).text(item.SuperCategoryName);
                        $('#ddlSuperCatogeries').append(newOption);

                    });
                }
            }
        });
    }
}

function GetCategoriesInddl() {
    var items = $("#ddlCatogeries option").length;
    if (items <= 0) {
        //Default item 

        var newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#ddlCatogeries').append(newOption);

        $.ajax({
            url: '../api/Master/GetCategries',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $.each(data.Result, function (index, item) {

                        newOption = $('<option>');
                        newOption.attr('value', item.CategoryId).text(item.CategoryName);
                        $('#ddlCatogeries').append(newOption);

                    });
                }
            }
        });
    }
}

function GetCategoriesBySuperCatId(Id) {
    $.ajax({
        url: '../api/Master/GetCategoriesBySuprCatID?ID=' + Id,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var newOption = $('<option>');
                newOption.attr('value', "").text('Please Select');
                $('#ddlCatogeries').append(newOption);

                $.each(data.Result, function (index, item) {
                    newOption = $('<option>');
                    newOption.attr('value', item.StateId).text(item.StateName);
                    $('#ddlCatogeries').append(newOption);
                });
            }
        }
    });
}

// End Get SuperCategories and Categories in Dropdown List //

function UpdateAuthorizedOrders() {
    var cartJson = { 'PaymentTransactionId': $("#txtBlkOrderNo").val(), 'ShipmentType': $('#ddlBlkShipType option:selected').text(), 'Location': $('#ddlBlkLocation option:selected').text(), 'ShipmentDate': $("#txtBlkShipDate").val(), 'CourierName': $("#txtBlkShipperName").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateAuthorizedOrders',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + data.Result + "'  has been successfully moved to ‘Waiting for Pickup Orders";
                document.getElementById('divMsg').style.display = 'block';

                vpb_hide_popup_boxes();
                GetPaymentPendingOrder();
            }
            else {
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').innerHTML = "<span class='iconsweet'>=</span>Order No. '" + data.Result + "'  has been successfully moved to ‘Waiting for Pickup Orders";
                document.getElementById('ctl00_ctl00_Main_Main_divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
            }
        },
        error:
        {
            //Show error message
        }
    });
}


function GetWaitingForPickOrder() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loadings.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    $.ajax({
        url: '../api/Master/GetWaitingForPickOrder',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('.overlay').hide();
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                div.remove();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').empty();
                $('#divPaymentOrdersGrd').append(data.Result);
                $('.overlay').hide();
                div.remove();
            }
        }
    });
}
function Getinpacking() {

    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    $.ajax({
        url: '../api/Master/Getinpacking',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                div.remove();
                //Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').empty();
                $('#divPaymentOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}


// Products Search based on given criteria
function GetProductSearchLst() {
    var val = $("#ddlNoRows option:selected").val();

    //var div = document.createElement("div");
    //div.className += "overlay";
    //div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    //document.body.appendChild(div);

    var rows = 500;
    var minval = 5;
    //rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    var ProductID = document.getElementById('txtProductId').value;
    if (ProductID == "")
        ProductID = 0;
    var ProductName = document.getElementById('txtProductName').value;
    var SuperCategory = $("#ddlSuperCatogeries").val();
    var Category = $("#ddlCategory").find("option:selected").val();
    var SubCategory = $("#ddlSubCategory").find("option:selected").val();
    var Quantity = $("#ddlQunatitysearch").find("option:selected").val();
    var ProductStatus = $("#ddlProdcutStatus").val();
    var Brand = $("#ddlBrand").find("option:selected").text();
    $.ajax({
        url: '../api/Master/GetProductListBySrch?rows=' + rows + '&ProdsuctId=' + ProductID + '&ProductName=' + ProductName + '&SuperCategory=' + SuperCategory + '&Category=' + Category + '&SubCategory=' + SubCategory + '&ProductStatus=' + ProductStatus + '&Brand=' + Brand + '&Quantity=' + Quantity,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#ProductLst').empty();
                var prddata = bindData(data.Result);
                $('#ProductLst').append(prddata);
                $rep_datatable = $('#tbl').dataTable(
                    {
                        order: [[0, 'desc']]
                    });
                $("#divPrdctList").show();
                //div.remove();
            }
            if (data.Status == "NoData") {
                document.getElementById('divMsg').innerHTML = "<div class='msgbar msg_Success hide_onC'><span class='iconsweet'>=</span><p class='text-white'>There are No Product(s) found as per  search criteria </p></div>.";
                document.getElementById('divMsg').style.display = 'block';
                $('#ProductLst').empty();
                $('#ProductLst').append(data.Result);
                $("#divPrdctSrch").show();
                $("#divPrdctList").hide()
            }
        }
    });

}
function GetProductSearchLstfortopselling() {
    var val = $("#ddlNoRows option:selected").val();

    //var div = document.createElement("div");
    //div.className += "overlay";
    //div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    //document.body.appendChild(div);

    var rows = 500;
    var minval = 5;
    //rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    var ProductID = document.getElementById('txtProductId').value;
    if (ProductID == "")
        ProductID = 0;
    var ProductName = document.getElementById('txtProductName').value;
    var SuperCategory = $("#ddlSuperCatogeries").val();
    var Category = $("#ddlCategory").find("option:selected").val();
    var SubCategory = $("#ddlSubCategory").find("option:selected").val();
    var Quantity = $("#ddlQunatitysearch").find("option:selected").val();
    var ProductStatus = $("#ddlProdcutStatus").val();
    var Brand = $("#ddlBrand").find("option:selected").text();

    var NoOfProds = $("#noofprods").val();
    var NoOfPastDays = $("#noofdayspast").val();
    //alert(NoOfProds);
    if (NoOfProds == "") {
        //alert('1');
        NoOfProds = -1;
    }
    if (NoOfPastDays == "") {
        //alert('2');
        NoOfPastDays = -1;
    }
    $.ajax({
        url: '../api/Master/GetProductListBySrchfortopselling?rows=' + rows + '&ProductId=' + ProductID + '&ProductName=' + ProductName + '&SuperCategory=' + SuperCategory + '&Category=' + Category + '&SubCategory=' + SubCategory + '&ProductStatus=' + ProductStatus + '&Brand=' + Brand + '&Quantity=' + Quantity + '&NoOfProds=' + NoOfProds + '&NoOfPastDays=' + NoOfPastDays,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#ProductLst').empty();
                var prddata = bindData(data.Result);
                $('#ProductLst').append(prddata);

                $rep_datatable = $('#tbl').dataTable({ "order": [[7, "desc"]] });
                $("#divPrdctList").show();
                //div.remove(); 
            }
            if (data.Status == "NoData") {
                document.getElementById('divMsg').innerHTML = "<div class='msgbar msg_Success hide_onC'><span class='iconsweet'>=</span><p>There are No Product(s) found as per  search criteria </p></div>.";
                document.getElementById('divMsg').style.display = 'block';
                $('#ProductLst').empty();
                $('#ProductLst').append(data.Result);
                $("#divPrdctSrch").show();
                $("#divPrdctList").hide()
            }
        }
    });

}
function bindData(Product) {
    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = "<div class='msgbar msg_Success bg-gray hide_onC border-0'><p class='text-white'>Found " + Product.length + " Product(s) as per search criteria </p></div>";
    strHtml = strHtml + "<input type='button' onclick='toExcel()' class='btn btn-success' style='display: inline-block; margin-left:0px; padding:3px 2px 3px 2px;float:right;margin:20px 0px 0 0;' value='Export to Excel'/>";
    strHtml = strHtml + "<div class='one_wrap ' id='divSearchResult' style=''><div id='divSearchgrids' class='widget' style=''> <div class='widget_title'>";
    strHtml = strHtml + " <h5> <label id='ctl00_ctl00_Main_Main_lblProductList'><b>Product List</b></label> </h5></div>";
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary px-3' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Product Name</th>";
    strHtml = strHtml + "<th>Qty</th>";
    strHtml = strHtml + "<th>MRP</th>";
    strHtml = strHtml + "<th>Dis.(%)</th>";
    strHtml = strHtml + "<th>Cost</th>";
    //strHtml = strHtml + "<th>Tax code</th>";
    strHtml = strHtml + "<th>Actions</th>";
    strHtml = strHtml + "<th>Sold Qty</th>";
    strHtml = strHtml + "<th>Status</th></tr></thead><tbody>";

    for (list in Product) {
        var color = "";
        if (Product[x].Quantity == 0) {
            color = "Red";
        }
        else if (Product[x].Quantity == 1) {
            color = "#ffbf00";
        }
        else if (Product[x].Quantity > 1) {
            color = "Green";
        }
        strHtmltr = strHtmltr + "<tr><td><a href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>" + Product[x].ProductId + "</a></td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].ProductName + "</td>";
        strHtmltr = strHtmltr + "<td><input type='text' Id='txtQuantity' style='width:30px;color:" + color + ";font-weight:bold;border-color:" + color + "' value='" + Product[x].Quantity + "'/></td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "/" + "</span>&nbsp;" + Product[x].ProductOriginalCost + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].ProductDiscountPercentage + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td style='display:none' role='gridcell'>" + Product[x].ProductDiscountPercentage + "%</td>";
        strHtmltr = strHtmltr + "<td><div id='divlinks'><input style='display: inline-block; margin-left:0px; padding:3px 2px 3px 2px;' id='" + Product[x].ProductId + "'  value='UpdateQty' class='button_small greyishBtn fl_right' onclick='UpdateProductQuantity(" + Product[x].ProductId + ",this);'  type='button'></div></td>";
        strHtmltr = strHtmltr + "<td role='gridcell'>" + Product[x].SoldQty + "</td>";
        if (Product[x].IsActive == false) {
            strHtmltr = strHtmltr + "<td role='gridcell'><a onclick='Activeproduct(" + Product[x].ProductId + ")'><span class='green_highlight pj_cat'>Activate</span></a><a target='_blank' href='../../Admin/Productanalysis.aspx?mn=6&nm=" + Product[x].ProductName  + "&id=" + Product[x].ProductId + "'><span class='red_highlight pj_cat'>Analysis</span></a></td>";
        } else {
            strHtmltr = strHtmltr + "<td role='gridcell'><a onclick='return Deleteproduct(" + Product[x].ProductId + ")';><span class='red_highlight pj_cat'>InActivate</span></a><a target='_blank' href='../../Admin/Productanalysis.aspx?mn=6&nm=" + Product[x].ProductName  + "&id=" + Product[x].ProductId + "'><span class='red_highlight pj_cat'>Analysis</span></a>&nbsp;&nbsp;</td></tr>";
        }
    
        x++;
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}

//Products Serach End

function UpdateProductQuantity(ProductId, dis) {
    var Quantity = $(dis).parents('tr').find('#txtQuantity').val();
    $.ajax({
        url: '../api/Master/UpdateProductQuantity?ProductID=' + ProductId + '&Quantity=' + Quantity,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert('Quantity Updated Successfully');
            }
            if (data.Status == "NoData") {
                alert('Quantity Not Updated');
            }
        }
    });
}

function NavigateToUpdteQty(ProductId) {
    window.location.href = "UpdateProductQuantity.aspx?ID=" + ProductId;
}

function UpdateWaitingForPickOrder() {
    var cartJson = { 'PaymentTransactionId': $("#lblPtrnsId").val() };
    var orderid;
    if ($('#chkkShip').prop('checked')) {
        orderid = $('#chkkShip').val();
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../api/Master/UpdateWaitingForPickOrder',
            type: 'post',
            dataType: 'json',
            data: JSON.stringify(cartJson),

            success: function (data) {
                if (data.Status == "Success") {
                    $("#divMsg").empty();
                    document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + data.Result + "'  has been successfully moved to ‘Dispatched Orders";
                    document.getElementById('divMsg').style.display = 'block';
                    GetWaitingForPickOrder();
                }
                else {
                    $("#divMsg").empty();
                    document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + data.Result + "' has been successfully moved to ‘Dispatched Orders";
                    document.getElementById('divMsg').style.display = 'block';
                    GetWaitingForPickOrder();
                }
            },
            error:
            {
                //Show error message
            }

        });
    }
    else {
        document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span> Please select atleast one Order";
        document.getElementById('divMsg').style.display = 'block';
    }
}


function GetDispatchedOrders() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    //var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    $.ajax({
        url: '../api/Master/GetDispatchedOrders',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divDispatchedtOrdersGrd').empty();
                ShowInfo(data, '#divDispatchedtOrdersGrd');
                $("#dvbtn").hide();
                div.remove();
            }
            if (data.Status == "NoData") {
                $('#divDispatchedtOrdersGrd').empty();
                $('#divDispatchedtOrdersGrd').append(data.Result);
                div.remove();
            }
        }
    });
}


//function GetDispatchedOrders() {
//    var rows;
//    var minval = 5;
//    var val = $("#ddlNoRows option:selected").val();
//    rows = typeof (val) != 'undefined' ? val : 0;
//    rows = parseInt(minval) + parseInt(rows);
//    $.ajax({
//        url: '../api/Master/GetDispatchedOrders?rows=' + rows,
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            if (data.Status == "Success") {
//                $('#divDispatchedtOrdersGrd').empty();
//                ShowInfo(data, '#divDispatchedtOrdersGrd');

//            }
//            if (data.Status == "NoData") {
//                $('#divDispatchedtOrdersGrd').empty();
//                $('#divDispatchedtOrdersGrd').append(data.Result);
//            }
//        }
//    });
//}

function UpdateDispatchedOrders() {
    var OrderID = $("#txtBlkOrderNo").val();
    var cartJson = { 'PaymentTransactionId': $("#txtBlkOrderNo").val(), 'ReceivedBy': $("#txtReceivedBy").val(), 'Comments': $("#txtComments").val(), 'DeliveredDate': $("#txtDeliveredDate").val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/UpdateDispatchedOrders',
        type: 'post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {
                $("#divMsg").empty();
                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + OrderID + "'  has been successfully moved to ‘Delivered Orders";
                document.getElementById('divMsg').style.display = 'block';
                vpb_hide_popup_boxes();
                GetDispatchedOrders();
            }
            else {
                vpb_hide_popup_boxes();
                $("#divMsg").empty();
                document.getElementById('divMsg').innerHTML = "<span class='iconsweet'></span>Order No. '" + OrderID + "'has been successfully moved to ‘Delivered Orders";
                document.getElementById('divMsg').style.display = 'block';

            }
        },
        error:
        {
            //Show error message
        }
    });
}

function GetDeliveredOrders() {
    var div = document.createElement("div");
    div.className += "overlay";
    div.innerHTML = "<span class='pleaseWaitText'>Please wait... Loading.</span>";
    document.body.appendChild(div);

    var rows;
    var minval = 50;
    // var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);

    $.ajax({
        url: '../api/Master/GetDeliveredOrders',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divDeliveredOrderssGrd').empty();
                ShowInfo(data, '#divDeliveredOrderssGrd');
                div.remove();

            }
            if (data.Status == "NoData") {
                $('#divDeliveredOrderssGrd').empty();
                $('#divDeliveredOrderssGrd').append(data.Result);
                div.remove();
            }
        }
    });
}


function GetSearchorders() {

    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    var CreatedOn;
    var UpdatedOn;
var CreatedOnPr;
    var UpdatedOnPr;
    if ($("#txtFromDate").val() == null || $("#txtFromDate").val() == '') {
        CreatedOn = '1/1/0001 12:00:00 AM';
        UpdatedOn = '1/1/0001 12:00:00 AM';
    }
    else {
        CreatedOn = $("#txtFromDate").val();
        UpdatedOn = $("#txtToDate").val();
    }
 if ($("#txtFromDatePr").val() == null || $("#txtFromDatePr").val() == '') {
        CreatedOnPr = '1/1/0001 12:00:00 AM';
        UpdatedOnPr = '1/1/0001 12:00:00 AM';
    }
    else {
        CreatedOnPr = $("#txtFromDatePr").val();
        UpdatedOnPr = $("#txtToDatePr").val();
    }

    if ($('#txtMobile').val() != '' && $('#txtProductName').val() == '') {

        var MobileNo = $('#txtMobile').val();

        $.ajax({
            url: '../api/Master/GetSearchordersbyMobile?Mobile=' + MobileNo,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                $("#divPaymentSearchOrdersGrd").empty();
                if (data.Status == "Success") {
                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();
                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            $('#nxtOrders').val(data.CartSum);
                        }
                }
            }
        });
    }
    else if ($('#txtMobile').val() == '' && $('#txtProductName').val() != '') {
        var ProductName = $('#txtProductName').val();
        // alert($("#txtOrderNo").val() + ', ' + $("#drpOrderStatus option:selected").val());
        $.ajax({
            url: '../api/Master/GetSearchordersbyProductName?ProductName=' + ProductName + '&CreatedOn=' + CreatedOnPr + '&UpdatedOn=' + UpdatedOnPr,
            type: 'post',
            dataType: 'json',

            success: function (data) {
                if (data.Status == "Success") {
                    $("#divPaymentSearchOrdersGrd").empty();

                    //ShowInfo(data, "#divPaymentSearchOrdersGrd");

                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();

                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                            div.remove();
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            //$rep_datatable = $('#grdShippingOrders').dataTable();
                            //div.remove();
                        }
                    $('#nxtOrders').val(data.CartSum);
                }
            }

        });
    }
    else {
        //var cartJson = { 'rows': 20, 'PaymentTransactionId': $("#txtOrderNo").val(), 'CreatedOn': CreatedOn, 'UpdatedOn': UpdatedOn, 'OrderCurrentStatus': $("#drpOrderStatus option:selected").val(), 'PaymentMode': $("#ddlPaymentModes option:selected").val(), 'PaymentStatus': $("#ddlSuccess option:selected").val(), 'ShipmentId': $('#txtShipmentId').val(), 'CourierName': $('#ddlCourier option:selected').text() };
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../api/Master/GetSearchorders?rows= 20&PaymentTransactionId=' + $("#txtOrderNo").val() + '&CreatedOn=' + CreatedOn + '&UpdatedOn=' + UpdatedOn + '&OrderCurrentStatus=' + $("#drpOrderStatus option:selected").val() + '&PaymentMode=' + $("#ddlPaymentModes option:selected").val() + '&PaymentStatus=' + $("#ddlSuccess option:selected").val() + '&ShipmentId=' + $('#txtShipmentId').val() + '&CourierName=' + $('#ddlCourier option:selected').text(),
            type: 'Get',
            dataType: 'json',
            //data: JSON.stringify(cartJson),
            success: function (data) {
                $("#divPaymentSearchOrdersGrd").empty();
                if (data.Status == "Success") {
                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();
                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            //$rep_datatable = $('#grdShippingOrders').dataTable({
                            //    "order": [[0, "desc"]]
                            //});
                            $('#nxtOrders').val(data.CartSum);
                        }
                }
            }
        });
    }
}
function GetCheckoutSearchorders() {
    $('.load').hide();
    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    var CreatedOn;
    var UpdatedOn;
    if ($("#txtFromDate").val() == null || $("#txtFromDate").val() == '') {
        CreatedOn = '1/1/0001 12:00:00 AM';
        UpdatedOn = '1/1/0001 12:00:00 AM';
    }
    else {
        CreatedOn = $("#txtFromDate").val();
        UpdatedOn = $("#txtToDate").val();
    }

    if ($('#txtMobile').val() != '' && $('#txtProductName').val() == '') {

        var MobileNo = $('#txtMobile').val();

        $.ajax({
            url: '../api/Master/GetCheckoutSearchordersbyMobile?Mobile=' + MobileNo,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                $("#divPaymentSearchOrdersGrd").empty();
                if (data.Status == "Success") {
                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();
                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            $('#nxtOrders').val(data.CartSum);
                        }
                }
            }
        });
    }
    else if ($('#txtMobile').val() == '' && $('#txtProductName').val() != '') {
        var ProductName = $('#txtProductName').val();
        // alert($("#txtOrderNo").val() + ', ' + $("#drpOrderStatus option:selected").val());
        $.ajax({
            url: '../api/Master/GetCheckoutSearchordersbyProductName?ProductName=' + ProductName,
            type: 'post',
            dataType: 'json',

            success: function (data) {
                if (data.Status == "Success") {
                    $("#divPaymentSearchOrdersGrd").empty();

                    //ShowInfo(data, "#divPaymentSearchOrdersGrd");

                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();

                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                            div.remove();
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            //$rep_datatable = $('#grdShippingOrders').dataTable();
                            //div.remove();
                        }
                    $('#nxtOrders').val(data.CartSum);
                }
            }

        });
    }
    else {
        //var cartJson = { 'rows': 20, 'PaymentTransactionId': $("#txtOrderNo").val(), 'CreatedOn': CreatedOn, 'UpdatedOn': UpdatedOn, 'OrderCurrentStatus': $("#drpOrderStatus option:selected").val(), 'PaymentMode': $("#ddlPaymentModes option:selected").val(), 'PaymentStatus': $("#ddlSuccess option:selected").val(), 'ShipmentId': $('#txtShipmentId').val(), 'CourierName': $('#ddlCourier option:selected').text() };
        var orderstatus = $('#ddlOrderStatus').val();
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../api/Master/GetCheckoutSearchorders?rows= 20&PaymentTransactionId=' + $("#txtOrderNo").val() + '&CreatedOn=' + CreatedOn + '&UpdatedOn=' + UpdatedOn + '&OrderStatus=' + orderstatus,
            type: 'Get',
            dataType: 'json',
            //data: JSON.stringify(cartJson),
            success: function (data) {
                $("#divPaymentSearchOrdersGrd").empty();
                if (data.Status == "Success") {
                    $('.success, .warning, .attention, .information, .error').remove();
                    $('#divPaymentSearchOrdersGrd').empty();
                    if (data != undefined)
                        if (data.Status == "Fail") {
                            $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                            $('.error').fadeIn('slow');
                            $('html, body').animate({ scrollTop: 0 }, 'slow');
                        }
                        else if (data.Status == "Success") {
                            $('#divPaymentSearchOrdersGrd').append(data.Result);
                            //$rep_datatable = $('#grdShippingOrders').dataTable({
                            //    "order": [[0, "desc"]]
                            //});
                            $('#nxtOrders').val(data.CartSum);
                        }
                }
            }
        });
    }
}


function GetUsers() {
    $('#divPaymentSearchOrdersGrd').empty();
    var OrderId = $('#txtorderid').val();
    var UserData = { 'rows': 20, 'Name': $("#txtName").val(), 'Mobile': $('#txtMobile').val(), 'Email': $('#txtEmail').val(), 'OrderId': OrderId, };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/GetUsers',
        type: 'Post',
        dataType: 'json',
        data: JSON.stringify(UserData),
        success: function (data) {
            if (data.Result.length != 1391) {
                $('#divUserSearchGrd').empty();
                ShowInfo(data, "#divUserSearchGrd");
                //$('#divPaymentSearchOrdersGrd').append(data.Result);
                //$rep_datatable = $('#tblContact').dataTable();
                //$('#nxtOrders').val(data.CartSum);
            }
            else {
                // var str = "No records found";
                var html = '<div class="validation" style="color:red;margin-bottom: 20px;">No records found</div>';
                $('#divPaymentSearchOrdersGrd').append(html);
            }
        }
    });

}
function GetCashbackTransactionRecords() {
    var x = "1";
    var fromdate = "2000-06-25 18:20:46.000";
    var todate = "2020-06-25 18:20:46.000";
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/Cashbacktransactionsajaxcall2?userid=' + "" + '&tranasactionID=' + "" + '&datefrom=' + fromdate + '&datetill=' + todate + '',
        type: 'get',
        dataType: 'json',
        //data: JSON.stringify(),
        success: function (data) {
            //alert(data);
            //$('#tablehere').append(data);
            var prddata = GetCashbackTransactionRecords2(data.Result);
            $('#pager').html(prddata);
            $rep_datatable = $('#tbl').dataTable();
            $("#PrdctList").show();
        }
    });

}
function GetCashbackTransactionRecords2(Product) {
    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    var y = 1;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>S.No</th>";
    strHtml = strHtml + "<th>orderid</th>";
    strHtml = strHtml + "<th>date</th>";
    strHtml = strHtml + "<th>totalamount</th>";
    strHtml = strHtml + "<th>credit</th>";
    strHtml = strHtml + "<th>debit</th>";
    //strHtml = strHtml + "<th>Total Cost</th>";
    strHtml = strHtml + "<th>balance</th>";
    strHtml = strHtml + "<th>userid</th>";
    strHtml = strHtml + "<th>PGTxnid</th>";
    strHtml = strHtml + "<th>Messages</th></tr ></thead ><tbody>";

    for (list in Product) {

        //strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='UpdateProducts.aspx?ID=" + Product.productId + "'>" + Product.productId + "</a></td>";

        //strHtmltr = strHtmltr + "<tr><td>" + y + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].orderid + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].date + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].totalAmount + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].credit + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].debit + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].balance + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].Userid + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].PGTxnid + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].Messages + "</td>";
        //alert(JSON.stringify(item.orderid));
        strHtmltr = strHtmltr + "<tr><td>" + y + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].orderid + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].date + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].totalAmount + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].credit + "</td>";
        strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].debit + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].balance + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Userid + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].PGTxnid + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Messages + "</td>";

        //strHtmltr = strHtmltr + "<td>" + Product[x].CreatedOn + "</td>";
        //if (Product[x].Status == true) {
        //    strHtmltr = strHtmltr + "<td><input type='checkbox' class='checkbox' name='checkboxesforcust' id=checkbox_" + Product[x].userid + "_" + Product[x].ProductId + " checked='checked' onclick='Checkboxajaxcall(" + Product[x].userid + "," + Product[x].ProductId + ",this.id)'></td>";
        //}
        //else {
        //    strHtmltr = strHtmltr + "<td><input type='checkbox' class='checkbox' name='checkboxesforcust' id=checkbox_" + Product[x].userid + "_" + Product[x].ProductId + " onclick='Checkboxajaxcall(" + Product[x].userid + "," + Product[x].ProductId + ",this.id)'></td>";
        //}
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        //strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        //strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++;
        y++;
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}

function UserOrderDetailsPage(UserId) {
    // alert(UserId);
    $('#lblName1').html($('#use_' + UserId).find('#lblName').html());
    $('#lblEmail1').html($('#use_' + UserId).find('#lblEmail').html());
    $('#lblAddress1').html($('#use_' + UserId).find('#lblAddress').html());
    $('#lblMobile1').html($('#use_' + UserId).find('#lblMobile').html());
    $('#lblPassword1').html($('#use_' + UserId).find('#lblPassword').html());
    $('#lblStatus1').html($('#use_' + UserId).find('#lblStatus').html());
    $('#OrderBody').empty();
    var UserData = { 'UserId': UserId, 'Name': $("#txtName").val(), 'Mobile': $('#txtMobile').val(), 'Email': $('#txtEmail').val(), 'OrderId': $('#txtorderid').val() };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/GetUserOrdesInfo',
        type: 'Post',
        dataType: 'json',
        data: JSON.stringify(UserData),
        success: function (data) {
            if (data.Result.length != 0) {
                $('#OrderBody').append(data.Result);
                $('#myModal').show();
            }
            else {
                //  var str = "No records found";
                var html = '<div class="validation" style="color:red;margin-bottom: 20px;">No records found</div>';
                $('#OrderBody').append(html);
                $('#myModal').show();
            }
        }
    });
}
function SingleOrderPage(OrderId) {
    window.open('../admin/OrderDetails.aspx?transId=' + OrderId, '_blank');
}

function GetNextOrders() {
    var rows;
    var minval = 50;
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    var CreatedOn;
    var UpdatedOn;
    if ($("#txtFromDate").val() == null || $("#txtFromDate").val() == '') {
        CreatedOn = '1/1/0001 12:00:00 AM';
        UpdatedOn = '1/1/0001 12:00:00 AM';
    }
    else {
        CreatedOn = $("#txtFromDate").val();
        UpdatedOn = $("#txtToDate").val();
    }
    var rowsCount = $('#nxtOrders').val();
    var cartJson = { 'rows': 20, 'PaymentTransactionId': $("#txtOrderNo").val(), 'CreatedOn': CreatedOn, 'UpdatedOn': UpdatedOn, 'OrderCurrentStatus': $("#drpOrderStatus option:selected").val(), 'PaymentMode': $("#ddlPaymentModes option:selected").val(), 'PaymentStatus': $("#ddlSuccess option:selected").val(), 'ServiceTax': rowsCount };
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/GetNextOrders',
        type: 'Post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                $('.success, .warning, .attention, .information, .error').remove();

                if (data != undefined)
                    if (data.Status == "Fail") {
                        $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                        $('.error').fadeIn('slow');
                        $('html, body').animate({ scrollTop: 0 }, 'slow');
                        div.remove();
                    }
                    else if (data.Status == "Success") {
                        var count = parseInt(rowsCount) + parseInt(20);
                        $('#nxtOrders').val(count);
                        $('#divPaymentSearchOrdersGrd').append(data.Result);
                        //$rep_datatable = $('#grdShippingOrders').dataTable();
                        //div.remove();
                    }
            }
        }
    });

}


function GenrateInvoice() {
    var trnsid = $("#lblPtrnsId").val();
    window.location.href = "Invoice.aspx?id=" + trnsid
}

function Trackshipment(awb) {
    var awbInfo = '&username=ecomexpress&password=Ke$3c@4oT5m6h#$&awb=' + awb;
    $.ajax({
        url: 'https://staging.ecomexpress.in/track_me/api/mawbd/',
        type: 'Post',
        dataType: 'xml',
        data: awbInfo,
        success: function (data) {
            alert(data);
        }
    });
}

function GetProductOverview(transId) {
    $.ajax({
        url: '../api/Master/GetProductOverview?transId=' + transId,
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
function GetProductOverviewforpres(transId) {
    $.ajax({
        url: '../api/Master/GetProductOverviewforpres?transId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divOrdersOverviewGrd').empty();
                ShowInfo(data, '#divOrdersOverviewGrd');
            }
            if (data.Status == "NoData") {
                $('#divOrdersOverviewGrd').empty();
                $('#divOrdersOverviewGrd').append(data.Result);
            }
        }
    });
}

function GetCheckoutProductOverview(transId) {
    $.ajax({
        url: '../api/Master/GetCheckoutProductOverview?transId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divOrdersOverviewGrd').empty();
                ShowInfo(data, '#divOrdersOverviewGrd');
            }
            if (data.Status == "NoData") {
                $('#divOrdersOverviewGrd').empty();
                $('#divOrdersOverviewGrd').append(data.Result);
            }
        }
    });
}

function popupOrderSuccess(trnsId) {
    $.ajax({
        url: '../api/Master/SendOtpToMakeOrderSuccess?transId=' + trnsId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#vpb_pop_up_background1').show();
                $('#vpb_login_pop_up_box1').show();
            }
        }
    });
}

function popupCheckoutOrderSuccess(trnsId) {
    $.ajax({
        url: '../api/Master/SendOtpToMakeCheckoutOrderSuccess?transId=' + trnsId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#vpb_pop_up_background1').show();
                $('#vpb_login_pop_up_box1').show();
            }
        }
    });
}

function PendingtoSuccessOrder(trnsId, PgTxnId, OTP, payMode) {
    $.ajax({
        url: '../api/Master/PendingtoSuccessOrder?transId=' + trnsId + '&PgTxnId=' + PgTxnId + '&OTP=' + OTP + '&payMode=' + payMode,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert('success');
                location.reload();
            }
        }
    });
}
function CheckouttoSuccessOrder(trnsId, PgTxnId, OTP, payMode) {
    $.ajax({
        url: '../api/Master/CheckouttoSuccessOrder?transId=' + trnsId + '&PgTxnId=' + PgTxnId + '&OTP=' + OTP + '&payMode=' + payMode,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert('success');
                location.reload();
            }
        }
    });
}
function CheckoutOrderClose(trnsId) {
    $.ajax({
        url: '../api/Master/CheckoutOrderClose?transId=' + trnsId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                alert('success');
                location.reload();
            }
        }
    });
}
function GetReturnOrders(transId) {
    $.ajax({
        url: '../api/Master/GetReturnOrders?trnsId=' + transId,
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
            //if (data.Status == "NoData") {
            //    $('#divOrderReturns').empty();
            //    $('#divOrderReturns').append(data.Result);
            //}
        }
    });
}

function MakePaymentFromAdmin(transId) {


    $.ajax({
        url: '../api/Master/MakePaymentFromAdmin?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#divsearch").empty();
                $("#divproductReturns").empty();
                $("#divOrderReplacement").empty();
                $('#PageMessage').empty();
                document.getElementById('PageMessage').innerHTML = " <div  class='msgbar msg_Success'><span class='iconsweet'><span id='lblspan'>=</span></span><p>Order No. ‘" + transId + "’ has been Replaced and new Order No. ‘" + data.Result + "’ has been created successfully</p><br></div></div>"



                //                <span class='iconsweet'></span>Order No. '" + data.Result + "' has been Cancelled successfully.";
                //                $("#divgvwReturnProductsSelected").hide();
            }
            if (data.Status == "NoData") {
                $('#divOrderReturns').empty();
                $('#divOrderReturns').append(data.Result);
            }
        }
    });
}

function NavigatetoOrderDetailsPage(transId) {
    window.open('OrderDetails.aspx?transId=' + transId, '_blank');
    //window.location.href = "OrderDetails.aspx?transId=" + transId;
}
function NavigatetoupdatePage(transId) {
    window.open('Updateemployee.aspx?ID=' + transId, '_blank');
    //window.location.href = "OrderDetails.aspx?transId=" + transId;
}
function NavigatetoCheckoutOrderDetailsPage(transId) {
    window.open('CheckoutOrderDetails.aspx?transId=' + transId, '_blank');
    //window.location.href = "OrderDetails.aspx?transId=" + transId;
}
function NavigatetoOrderReturnsPage() {
    var transId = $("#lblPtrnsId").val();
    window.location.href = "OrderReturns.aspx?transId=" + transId;
}

function NavigatetoProcessReturnsPage(transId) {
    // var transId = $("#lblPtrnsId").val();
    window.location.href = "ProcessReturns.aspx?transId=" + transId;
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

function GetProcessReturnsDetails(transId) {

    //  var transId = $("#txttransId").val();
    $.ajax({
        url: '../api/Master/GetProcessReturnsDetails?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {

                $('#divproductReturns').empty();
                ShowInfo(data, '#divproductReturns');

            }
            if (data.Status == "NoData") {
                $("#divsearch").empty();
                $('#divproductReturns').empty();
                $('#divproductReturns').append(data.Result);
            }
        }
    });
}

function GetReplacementProduct() {

    var transId = $("#txttransId").val();
    $.ajax({
        url: '../api/Master/EditReplaceOrders?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.TransactionId);
                $("#txtqty").val(data.Result.Quantity);
                $("#txtprice").val(data.Result.ProductCost);


            }
            if (data.Status == "NoData") {
                $('#divOrderReplacement').empty();
                //                $('#divOrderReplacement').append(data.Result);
                $("#diverrormessage").append(data.Result);
            }
        }
    });
}

function AddReplacementProduct() {

    var transId = $("#txttransId").val();
    $.ajax({
        url: '../api/Master/GetReplacementProduct?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.TransactionId);
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
        url: '../api/Master/EditReplaceOrders?trnsId=' + transId,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $("#txttransId").val(data.Result.TransactionId);
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

function GetPaymentStaus(rows) {

    var rowc = rows;
    var minval = 50;
    var value = $("#ddlstatus option:selected").val();
    // var val = $("#ddlNoRows option:selected").val();
    //    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rowc);
    $.ajax({
        url: '../api/Master/GetPaymentStatus?rows=50&value=' + value,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                $('#divPaymentOrdersGrd').empty();
                ShowInfo(data, '#divPaymentOrdersGrd');
                Pageing();
            }
            if (data.Status == "NoData") {
                $('#divPaymentOrdersGrd').append(data.Result);
            }
        }
    });
}

function UpdateReturnProductAction(trnsId) {

    var ReturnActionValue = $("#ddlReturnAction option:selected").val()
    if (ReturnActionValue == 2) {
        var cartJson = { 'TransactionId': trnsId, 'OrdersReturnReason': $("#ddlReturnReason option:selected").html(), 'OrdersReturnAction': $("#ddlReturnAction option:selected").html() };

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: '../api/Master/UpdateReturnProductAction',
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

function Deleteproduct(prdId) {
    var r = confirm("Are you sure you want to DeActivate this?");
    if (r == true) {
        var ID = 0;
        var name = "";
        if (prdId == 'Do you want to delete this record?') {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                ID = sParameterName[1];
            }
        } else {
            ID = prdId;
        }
        $.ajax({
            url: '../api/Master/DeleteProduct?id=' + ID + '&name=' + name,
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (data) {
                alert('Record successfully deleted new');
                if (prdId != null) {
                    GetProductSearchLst();
                }

            },
            error: function (x, y, z) {
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');
            }
        });
    }
    else if (r == false) {

    }
}
function Activeproduct(prdId) {
    var r = confirm("Ary you sure you want to activate this product?");
    if (r == true) {
        var ID = 0;
        var name = "";
        if (prdId == 'Do you want to Active this record?') {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                ID = sParameterName[1];
            }
        } else {
            ID = prdId;



        }
        $.ajax({
            url: '../api/Master/Activeproduct?id=' + ID + '&name=' + name,
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (data) {

                alert('Record successfully Activated new');
                //window.location.href = "Updateproducts.aspx?id=" + prdId;

                if (prdId != null) {
                    GetProductSearchLst();
                }


            },
            error: function (x, y, z) {
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');
            }
        });
    }
    else if (r == false) {

    }
}
//delete reviews by gopi
function DeleteReviews(ReviewId) {
    var r = confirm("Ary you sure you want to delete this?");
    {
        $.ajax({
            url: '../api/Master/DeleteReviews?id=' + ReviewId,
            type: 'GET',
            async: false,
            dataType: 'json',
            success: function (data) {
                alert('Record successfully deleted');
            },
            error: function (x, y, z) {
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');
            }
        });
    }
}
//end delete reviews

function GetStates() {
    var items = $("#ddlstate option").length;
    if (items <= 0) {
        //Default item 

        newOption = $('<option>');
        newOption.attr('value', "").text('Please Select');
        $('#ddlstate').append(newOption);



        $.ajax({
            url: '../api/Master/GetStates',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.Status == "Success") {
                    $.each(data.Result, function (index, item) {

                        newOption = $('<option>');
                        newOption.attr('value', item.StateId).text(item.StateName);
                        $('#ddlstate').append(newOption);

                    });
                }
            }
        });
    }
}

//for redirecting to Refund.aspx from AuthorizedOrders by gopi
function Redirect(transId) {
    window.location.href = 'Refund.aspx';
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

function CancelOrder() {
    var id = getQueryVariable('transId');
    var r = confirm("Ary you sure you want to cancel this order?");
    if (r == true) {
        $.ajax({
            url: '../api/Master/CancelOrder?transid=' + id,
            type: 'Get',
            async: false,
            dataType: 'json',
            success: function (data) {
                alert('Order successfully cancelled');
                $('#vpb_pop_up_background2').css("display", "none");
                $('#vpb_login_pop_up_box2').css("display", "none");


                $('html, body').animate({ scrollTop: 1600 }, 800);
                //Redirect();
            },
            error: function (x, y, z) {
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');
            }
        });
    }
}

//for edit shipping address adminside by gopi

function vpb_show_sign_up_box(dis, id) {
    $("#vpb_pop_up_background").css({
        "opacity": "0.4"
    });
    var tbl = $(dis).closest("div .tradus-select-user-address-bg");

    $('#BillingAddressId').val(id); //tbl.find("input[id*='hdnBillingAddId']").val().trim()
    $('#addressId').val(id);


    $('#txbEditShippingAddress1').val(tbl.find('span').find("data.FirstOrDefault().UserAddress.StreetAddress1").text().trim());
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
    $("#vpb_pop_up_background").fadeIn("slow");
    $("#vpb_signup_pop_up_box").fadeIn('fast');
    window.scroll(0, 0);
}

//transaction details
function Transactiondetails() {

    $("#list").jqGrid({
        datatype: 'json',
        url: '../api/Master/Transactiondetails',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "ProductId",
        mtype: 'GET',
        rowNum: 20,
        autosize: true,
        rowList: [20, 40, 60, 80, 100],
        viewrecords: true,
        colNames: ['PaymentTransactionId', 'UserId', 'PGTxnId', 'TxnStatus', 'TxnMessage', 'TxnAmount'],
        colModel: [
            { name: 'PaymentTransactionId', index: 'PaymentTransactionId', width: 80, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, search: true },
            { name: 'UserId', index: 'UserId', width: 80, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false } },
            { name: 'PGTxnId', index: 'PGTxnId', width: 150, editable: true, editrules: { required: false }, edittype: 'text' },
            { name: 'TxnStatus', index: 'TxnStatus', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'TxnMessage', index: 'TxnMessage', width: 150, editable: true, editrules: { required: true }, edittype: 'text' },
            { name: 'TxnAmount', index: 'TxnAmount', width: 80, editable: true, editrules: { required: true }, edittype: 'text' },
        ],
        pager: '#pager',

        sortname: 'CreatedOn',
        sortorder: 'asc',
        height: "100%",
        width: "150%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Product Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
        {
            editData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'ProductId');
                    return value;
                }
            },
            url: "../api/Master/EditProduct", closeOnEscape: true, reloadAfterSubmit: true,
            closeAfterEdit: true, left: 400, top: 300,
            beforeShowForm: function (formid) {

                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var productId = $("#list").jqGrid('getCell', selId, 'ProductId');
                EditProductFeatureGrid(productId);
                EditProductSpecificationsGrid(productId);
                EditProductsGalleryGrid(productId);
            },

            afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully updated");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        },
        {
            delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'ProductId');
                    return value;
                }
            }, url: "../api/Master/DeleteProduct", mtype: 'GET',
            closeOnEscape: true, reloadAfterSubmit: true, left: 450, top: 300, afterSubmit: function (response, postdata) {
                var res = $.parseJSON(response.responseText);
                if (res == "Success") {
                    jQuery("#success").show();
                    jQuery("#success").html("Record successfully deleted");
                    jQuery("#success").fadeOut(6000);
                    return [true, response.responseText];
                }
                else {
                    return [false, response.responseText];
                }
            }
        }

    );

    function returnUpdateQty(cellValue, options, rowdata) {
        return "<a href='UpdateProductQuantity.aspx?ID=" + cellValue + "'>updteQty</a>";
    }

    function returnMyLink(cellValue, options, rowdata) {
        return "<a href='UpdateProducts.aspx?ID=" + cellValue + "'>Edit/Deactivate</a>";
    }

}
//end transaction details

function fn_get_rep_table() {
    var oSettings = $rep_datatable.fnSettings();
    var colTitles = $.map(oSettings.aoColumns, function (node) {
        return node.sTitle;
    });

    var $str_return = '<thead><tr>';

    jQuery.each(colTitles, function () {
        $str_return += '<th>' + this + '</th>';
    });

    $str_return += '</tr></thead><tbody>';

    var $rep_data = $rep_datatable.fnGetData();

    $.each($rep_data, function (key1, value1) {
        $str_return += '<tr>';
        $.each(value1, function (key2, value2) {
            $str_return += '<td>' + value2 + '</td>';
        });
        $str_return += '</tr>';
    });

    $str_return += '</tbody>';
    return $str_return;
}

var tableToExcelBlob = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) {
            return window.btoa(unescape(encodeURIComponent(s)))
        },
        format = function (s, c) {
            return s.replace(/{(\w+)}/g, function (m, p) {
                return c[p];
            })
        }
    return function (tableMarkup, name) {
        var ctx = {
            worksheet: name || 'Worksheet',
            table: tableMarkup
        }
        return new Blob([format(template, ctx)]);
    }
})()

function saveBlob(fileNameToSaveAs, blob) {
    var ie = navigator.userAgent.match(/MSIE\s([\d.]+)/),
        ie11 = navigator.userAgent.match(/Trident\/7.0/) && navigator.userAgent.match(/rv:11/),
        ieVer = (ie ? ie[1] : (ie11 ? 11 : -1));

    if (ie && ieVer < 10) {
        console.log("No blobs on IE ver<10");
        return;
    }

    if (ie || ie11) {
        window.navigator.msSaveBlob(blob, fileNameToSaveAs);
    } else {
        var downloadLink = document.createElement("a");
        downloadLink.download = fileNameToSaveAs;
        downloadLink.innerHTML = "Download File";
        if (window.URL !== undefined) {
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.style.display = "none";
            $('body').append(downloadLink);
        } else if (window.webkitURL !== undefined) {
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.style.display = "none";
            $('body').append(downloadLink);
        } else {
            console.log('Method Unavailable: createObjectURL');
        }
        //if (window.webkitURL !== null) {
        //    // Chrome allows the link to be clicked
        //    // without actually adding it to the DOM.

        //    downloadLink.href = (window.URL || window.webkitURL || {}).createObjectURL(blob);
        //} else {
        //    // Firefox requires the link to be added to the DOM
        //    // before it can be clicked.
        //    downloadLink.href = window.URL.createObjectURL(blob);
        //    downloadLink.onclick = destroyClickedElement;
        //    downloadLink.style.display = "none";
        //    document.body.appendChild(downloadLink);
        //}

        downloadLink.click();
    }
}

function toExcel() {

    //saveBlob('test_table.xls', tableToExcelBlob(fn_get_rep_table(), 'test_table'));
    $("#grdShippingOrders").table2excel({
        // exclude: ".noExl",
        name: "Worksheet Name",
        filename: "HUW_Orders" //do not include extension
    });
}




function pendingrecordsnew() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/Getpendinglistnew?status=1',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = PendingDataNew(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#pending").show();
            }
        }
    });
}
function pendingrecordsnewmedicines() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/Getpendinglistmedicinenew?status=1',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = PendingDataNewMedicines(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#pending").show();
            }
        }
    });
}
function PendingDataNewMedicines(Product) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Name</th>";
    strHtml = strHtml + "<th>Mobile</th>";
    strHtml = strHtml + "<th>PatientName</th>";
    strHtml = strHtml + "<th>Deliverytype</th>";
    strHtml = strHtml + "<th>Amount</th>";
    strHtml = strHtml + "<th>CreatedOn</th></tr></thead><tbody>";

    for (list in Product) {
        var color = "";
        //var dateObj = Date.parse(Product[x].CreatedOn);
        var date = new Date(Date.parse(Product[x].CreatedOn));
        //var dtobj = $.datepicker.formatDate("yy-mm-dd",dateObj);
        //alert(date);
        var month = date.getUTCMonth() + 1; //months from 1-12
        var day = date.getUTCDate();
        var year = date.getUTCFullYear();

        var newdate = year + "/" + month + "/" + day;

        //var userdata = Product[x].Users;
        strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='medicineupdateupdatenew.aspx?transid=" + Product[x].OrderId + "'>" + Product[x].OrderId + "</a></td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].UserId + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Name + "</td>";

        strHtmltr = strHtmltr + "<td>" + Product[x].Mobile + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].PatientName + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Deliverytype + "</td>";
        if (Product[x].Amount != null) {
            strHtmltr = strHtmltr + "<td>" + Product[x].Amount + "</td>";
        }
        else {
            strHtmltr = strHtmltr + "<td>N/A</td>";
        }
        strHtmltr = strHtmltr + "<td>" + Product[x].CreatedOn + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].ProductDiscountPercentage + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        ////strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        ////strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        ////strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        ////strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}
function pendingrecords() {
    $.ajax({
        datatype: 'json',
        url: '../api/Master/Getpendinglist',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                var prddata = PendingData(data.Result);
                $('#pager').html(prddata);
                $rep_datatable = $('#tbl').dataTable();
                $("#pending").show();
            }
        }
    });
}
function PendingDataNew(Product) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>Name</th>";
    strHtml = strHtml + "<th>Mobile</th>";
    strHtml = strHtml + "<th>PatientName</th>";
    strHtml = strHtml + "<th>Deliverytype</th>";

    strHtml = strHtml + "<th>CreatedOn</th></tr></thead><tbody>";

    for (list in Product) {
        var color = "";
        //var dateObj = Date.parse(Product[x].CreatedOn);
        var date = new Date(Date.parse(Product[x].CreatedOn));
        //var dtobj = $.datepicker.formatDate("yy-mm-dd",dateObj);
        //alert(date);
        var month = date.getUTCMonth() + 1; //months from 1-12
        var day = date.getUTCDate();
        var year = date.getUTCFullYear();

        var newdate = year + "/" + month + "/" + day;

        //var userdata = Product[x].Users;
        strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='prescriptionupdatenew.aspx?transid=" + Product[x].OrderId + "'>" + Product[x].OrderId + "</a></td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].UserId + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Name + "</td>";

        strHtmltr = strHtmltr + "<td>" + Product[x].Mobile + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].PatientName + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].Deliverytype + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].CreatedOn + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].ProductDiscountPercentage + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        ////strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        ////strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        ////strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        ////strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}
function GetNextCheckOutOrders() {
    var rows;
    var minval = 50;
    alert("hii")
    var val = $("#ddlNoRows option:selected").val();
    rows = typeof (val) != 'undefined' ? val : 0;
    rows = parseInt(minval) + parseInt(rows);
    var CreatedOn;
    var UpdatedOn;
    if ($("#txtFromDate").val() == null || $("#txtFromDate").val() == '') {
        CreatedOn = '1/1/0001 12:00:00 AM';
        UpdatedOn = '1/1/0001 12:00:00 AM';
    }
    else {
        CreatedOn = $("#txtFromDate").val();
        UpdatedOn = $("#txtToDate").val();
    }
    var rowsCount = $('#nxtCheckOutOrders').val();
    var orderstatus = $('#ddlOrderStatus').val();
    var cartJson = { 'rows': 20, 'PaymentTransactionId': $("#txtOrderNo").val(), 'CreatedOn': CreatedOn, 'UpdatedOn': UpdatedOn, 'OrderCurrentStatus': orderstatus, 'ServiceTax': rowsCount };
    alert(cartJson);
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: '../api/Master/GetNextCheckOutOrders',
        type: 'Post',
        dataType: 'json',
        data: JSON.stringify(cartJson),

        success: function (data) {
            if (data.Status == "Success") {

                $('.success, .warning, .attention, .information, .error').remove();

                if (data != undefined)
                    if (data.Status == "Fail") {
                        $('#notification').html('<div class="error" style="display: none;">' + data.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
                        $('.error').fadeIn('slow');
                        $('html, body').animate({ scrollTop: 0 }, 'slow');
                        div.remove();
                    }
                    else if (data.Status == "Success") {
                        var count = parseInt(rowsCount) + parseInt(20);
                        $('#nxtOrders').val(count);
                        $('#divPaymentSearchOrdersGrd').append(data.Result);
                        //$rep_datatable = $('#grdShippingOrders').dataTable();
                        //div.remove();
                    }
            }
        }
    });

}
//new modify by shashi
function getValue(TrsId, boxid) {
    var TransesctionId = TrsId;
    var BoxId = boxid;
    $.ajax({
        url: '../api/Master/UpdateBoxInpackaing?OrderID=' + TransesctionId + '&BoxId=' + BoxId,
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.Status == "Success") {
                //window.location.href = "CheckoutOrderDetails.aspx?transId=";
            }
            if (data.Status == "NoData") {
                alert('Comment Not Updated');
            }
        }
    });
}
function PendingData(Product) {

    var data = Product;
    var strHtml = "";
    var strHtmltr = "";
    var x = 0;
    strHtml = strHtml + "<div class='widget_body'> <div class='cp_productlist_view'> <div class='products_views'><div class='products_links'></div></div></div>";
    strHtml = strHtml + "<div id='ctl00_ctl00_Main_Main_divGridHTMLcontainer'> <div id='productListcontainer' class='position_relative '> <div id='productListexample' class='k-content'></div>";
    strHtml = strHtml + "<div style='overflow-x:scroll' class='k-grid k-widget k-secondary' data-role='grid' id='divstoregrid'>";
    strHtml = strHtml + "<table id='tbl' width='100%' class='activity_datatable'><thead>";
    //strHtml = strHtml + " <tr class='k-grouping-row'><td style='padding-top: 20px; padding-bottom: 18px; padding-left:15px; ' aria-expanded='true' colspan='10' class=''><div style='font-size:15px; font-family: Verdana, Geneva, sans-serif; color:#000000;  '></div></td></tr>";
    strHtml = strHtml + "<tr>";
    strHtml = strHtml + "<th>ID</th>";
    strHtml = strHtml + "<th>UserId</th>";
    strHtml = strHtml + "<th>CreatedOn</th></tr></thead><tbody>";

    for (list in Product) {
        var color = "";
        //var dateObj = Date.parse(Product[x].CreatedOn);
        var date = new Date(Date.parse(Product[x].CreatedOn));
        //var dtobj = $.datepicker.formatDate("yy-mm-dd",dateObj);
        //alert(date);
        var month = date.getUTCMonth() + 1; //months from 1-12
        var day = date.getUTCDate();
        var year = date.getUTCFullYear();

        var newdate = year + "/" + month + "/" + day;

        //var userdata = Product[x].Users;
        strHtmltr = strHtmltr + "<tr><td><a style='color:#' href='prescriptionupdate.aspx?transid=" + Product[x].PaymentTransactionId + "'>" + Product[x].PaymentTransactionId + "</a></td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].UserId + "</td>";
        strHtmltr = strHtmltr + "<td>" + Product[x].UserId + "</td>";

        strHtmltr = strHtmltr + "<td>&nbsp;" + newdate + "</td>";
        //strHtmltr = strHtmltr + "<td>" + Product[x].ProductDiscountPercentage + "</td>";
        //strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;" + Product[x].ProductCost + "</td>";
        ////strHtmltr = strHtmltr + "<td><span class='WebRupee'>" + "" + "</span>&nbsp;<a forecolor='Blue' href='UpdateProducts.aspx?ID=" + Product[x].ProductId + "'>Activate</a></td>";
        ////strHtmltr = strHtmltr + " <br/><a onclick='activeproduct(" + Product[x].ProductId  + ")'>Edit</a>&nbsp;&nbsp;<a>Delete</a></div>";
        ////strHtmltr = strHtmltr + "<td role='gridcell'><a OnClientClick="return Activeproduct('Do you want to delete this record?')><span class='green_highlight pj_cat'>Active</span></a></td>";
        ////strHtmltr = strHtmltr + "<td><input type='button' OnClick='Activeproduct'</td>";
        //strHtmltr = strHtmltr + "<td role='gridcell'><a  onclick='Activeproduct(" + Product[x].ProductId + ")'><span>Activate</span></a></td>";
        x++
    }
    strHtmltr = strHtmltr + "</tbody></table>";
    strHtmltr = strHtmltr + "<div id='dvHidden'></div><div class='clear'></div></div></div> </div></div></div></div></div></div>";

    return strHtml + strHtmltr;

}



