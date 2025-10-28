
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

function ShowInfo(CustomResponse, Control) {

    $('.success, .warning, .attention, .information, .error').remove();
    if (CustomResponse.Status == "Fail") {
        $('#notification').html('<div class="error" style="display: none;">' + CustomResponse.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
        $('.error').fadeIn('slow');
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    }
    else if (CustomResponse.Status == "Success") {
        $(Control).append(CustomResponse.Result);
    }
    else if (CustomResponse.Status == "NoData") {
        $('#notification').html('<div class="attention" style="display: none;">' + CustomResponse.Message + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');
        $('.success').fadeIn('slow');
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    }
}

var isNew = true;
var pminprice = 1;
var pmaxprice = 200000;
var loading = true;
var psord = '';
var prows = 0;
var pcategoryId = 0;
var psubCategoryId = 0;
var PreviousScrollPos = 0;

$(document).ready(function () {
    //    //FLEXISLIDER
    CartShortInfo();
    WishListShortInfo();
    jQuery('.flexslider').flexslider({
        animation: "slide",
        pauseOnAction: false,
        start: function (slider) {
            $('body').removeClass('loading');
        }
    });

    //    // JCAROUSEL
    jQuery('.first-and-second-carousel').jcarousel();

});

function LoadSearch() {

    var a = getParameterByName("cid");
    var b = getParameterByName("scid");
    pcategoryId = a == "" ? 0 : a;
    psubCategoryId = b == "" ? 0 : b;

    if (pcategoryId == null || pcategoryId == 0)
        pcategoryId = 1;

    GetSubCategoriesByCategory(pcategoryId);
    //SHORTCODES
    //Toggle Box
    jQuery(".toggle_box > li:first-child .toggle_title, .toggle_box > li:first-child .toggle_content").addClass('active');
    jQuery(".toggle_box > li > a.toggle_title").toggle(function () {

        $(this).addClass('active');
        //  $(this).siblings('.toggle_content').slideDown(300);
    }, function () {
        $(this).removeClass('active');
        //  $(this).siblings('.toggle_content').slideUp(300);
    });

    //SUBMENU
    jQuery("ul.departments > li.menu_cont > a").click(function () {
        $(this).toggleClass('active').next().slideToggle(300);

    });

    //$('ul.departments > li.menu_cont > a.active + .side_sub_menu').slideDown(300); ;

    GetProducts();
    $(window).scroll(function () {
        var CurretScrollPos = $(this).scrollTop();
        var a = $('#all-product').height();
        var b = $(window).scrollTop();
        if (CurretScrollPos > PreviousScrollPos) {
            if ($('#all-product').height() < $(window).scrollTop()) {

                if (loading) {
                    loading = false;
                    isNew = false;
                    GetProducts();
                }
            }

        }
        PreviousScrollPos = CurretScrollPos;
    });
}

function displayCart() {

    jQuery.support.cors = true;
    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + 'PersonService.asmx/ShowCart',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            clearconsole();
            //  alert(data.d);
            $('..cart_drop > *').html(data.d);
            $('.cart_drop').slideDown(300);
            jQuery('.minicart').live('mouseleave', function () {
                $('.cart_drop').slideUp(300);
            });

        },
        error: function (x, y, z) {
            clearconsole();

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
            clearconsole();
            displayCart();
        },
        error: function (x, y, z) {
            clearconsole();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');
        }
    });

}

//SLIDE TOGGLE
jQuery(".minicart").live('hover', function () {
    // displayCart();
    //   $('.cart_drop').load('PersonService.asmx/ShowCart .cart_drop > *');
    //$('.cart_drop').slideDown(300);
    //        jQuery('.minicart').live('mouseleave', function () {
    //            $('.cart_drop').slideUp(300);
    //        });
});


/* Search */
$('.button-search').bind('click', function () {
    url = $('base').attr('href') + 'index.php?route=product/search';

    var filter_name = $('input[name=\'filter_name\']').attr('value');

    if (filter_name) {
        url += '&filter_name=' + encodeURIComponent(filter_name);
    }

    location = url;
});

$('header input[name=\'filter_name\']').bind('keydown', function (e) {
    if (e.keyCode == 13) {
        url = $('base').attr('href') + 'index.php?route=product/search';

        var filter_name = $('input[name=\'filter_name\']').attr('value');

        if (filter_name) {
            url += '&filter_name=' + encodeURIComponent(filter_name);
        }

        location = url;
    }
});

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
            clearconsole();
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
            clearconsole();
            json = json.d;
            data = json;

        }
    });
    return data;
}

function addToCart(product_id, quantity, isRedirect, specifications) {
    if (checkCartItemExist(product_id)) {

        $('.success, .warning, .attention, .information, .error').remove();

        $('#notification').html('<div class="success" style="display: none;">' + 'Selected product is already added to the cart. <br><br> <a href="mycart.aspx"> Click here to Continue with existing cart </a> <img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

        $('.success').fadeIn('slow');
        $('html, body').animate({ scrollTop: 0 }, 'slow');
    }
    else {
        quantity = typeof (quantity) != 'undefined' ? quantity : 1;
        isRedirect = typeof (isRedirect) != 'undefined' ? isRedirect : 0;
        specifications = typeof (specifications) != 'undefined' ? specifications : null;
        var userProductTransaction = {}; // var product = {}; var user = {};

        userProductTransaction.ProductId = product_id;
        userProductTransaction.UserProductTransactionSpecifications = specifications;
        //	cart.CartProduct = product;
        //cart.CartUser = user;
        userProductTransaction.Quantity = quantity;
        var cartJson = { 'isRedirect': isRedirect, 'cart': userProductTransaction };

        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + 'PersonService.asmx/AddToCart',
            type: 'post',
            data: JSON.stringify(cartJson),
            dataType: 'json',
            success: function (json) {
                clearconsole();
                clearconsole();
                json = json.d;
                $('.success, .warning, .attention, .information, .error').remove();

                if (json['redirect']) {
                    location = json['redirect'];
                }

                if (json['success']) {
                    $('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

                    $('.success').fadeIn('slow');

                    $('.minicart > .minicart_link').html(json['total']);

                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }
            }

        });
    }
}




function addToWishList(product_id) {


    if (checkWishListItemExist(product_id)) {

        $('.success, .warning, .attention, .information, .error').remove();

        $('#notification').html('<div class="success" style="display: none;">' + 'Selected product is already added to the WishList. <br> <br><a href="index.aspx"> Click here to Continue with existing WishList </a> <img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

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
            success: function (json) {
                clearconsole();
                json = json.d;
                $('.success, .warning, .attention, .information').remove();
                if (json['redirect']) {
                    location = json['redirect'];
                }

                if (json['success']) {
                    $('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

                    $('.success').fadeIn('slow');

                    $('#wishlist-total').html(json['total']);

                    $('html, body').animate({ scrollTop: 0 }, 'slow');
                }
            }
        });
    }
}
function addToCompare(product_id) {
    $.ajax({
        url: 'index.php?route=product/compare/add',
        type: 'post',
        data: 'product_id=' + product_id,
        dataType: 'json',
        success: function (json) {
            clearconsole();
            $('.success, .warning, .attention, .information').remove();

            if (json['success']) {
                $('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

                $('.success').fadeIn('slow');

                $('#compare-total').html(json['total']);

                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }
    });
}



function deleteFromCart(product_id) {

    var cartJson = { 'id': product_id };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/DeleteFromCart",
        type: 'post',
        data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            clearconsole();
            json = json.d;
            $('.success, .warning, .attention, .information, .error').remove();

            if (json['redirect']) {
                location = json['redirect'];
            }

            if (json['success']) {
                $('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

                $('.success').fadeIn('slow');

                $('.minicart > .minicart_link').html(json['total']);

                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }
        }
    });
}

function CartShortInfo() {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/CartShortInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            clearconsole();
            json = json.d;
            $('.minicart > .minicart_link').html(json['total']);
        }

    });
}

function WishListShortInfo() {


    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/WishListShortInfo",
        type: 'GET',
        // data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            clearconsole();
            json = json.d;
            $('#wishlist-total').html(json['total']);

        }

    });
}


function deleteFromWishList(product_id) {

    var cartJson = { 'id': product_id };

    $.ajax({
        contentType: "application/json; charset=utf-8",
        url: siteURL + "PersonService.asmx/DeleteFromWishList",
        type: 'post',
        data: JSON.stringify(cartJson),
        dataType: 'json',
        success: function (json) {
            clearconsole();
            json = json.d;
            $('.success, .warning, .attention, .information, .error').remove();

            if (json['redirect']) {
                location = json['redirect'];
            }

            if (json['success']) {
                $('#notification').html('<div class="success" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/leisure/images/close.png" alt="" class="close" /></div>');

                $('.success').fadeIn('slow');

                $('#wishlist-total').html(json['total']);

                GetWishListProducts();

                $('html, body').animate({ scrollTop: 0 }, 'slow');
            }

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
        url: siteURL + 'PersonService.asmx/ShowWishList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            clearconsole();
            data = data.d;
            WriteWishListResponse(data);

        },
        error: function (x, y, z) {
            clearconsole();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }

    });
}





function WriteWishListResponse(products) {
    $('#all-product ul').empty();
    var strResult = "";
    // for (var i = 0; i < 20; i++) {

    if (products.length != 0) {


        $.each(products, function (index, item) {

            strResult += "<li><a  href='" + siteURL + "product/" + item.ProductName + "/" + item.ProductId + "' class='product_image'><img width='225px' height='350px'  src='" + item.ProductImgUrl.replace("~/", siteURL) + "' alt='" + item.ProductName + "' /></a>" +
                    "<div class='product_info'>" +
                    "<h3><a href='" + siteURL + "product/" + item.Product.ProductName + "/" + item.ProductId + "'>" + item.ProductName + "</a></h3>" +
                    "   </div>" + //<small>" + item.ProductDescription + "</small> 
                    "<div class='price_info'> <a onClick='deleteFromWishList(" + item.ProductId + ");'>+ Remove From Wish List</a>" +
                    "<button class='price_add' title='' type='button' onClick='NavigateToDetails(" + item.ProductId + ");'>" +
                    "<span class='pr_price'>" + item.ProductCost + "</span>" +
                    "<span class='pr_add'>Add to Cart</span></button>" +
                    "</div></li>";

        });
    }
    else {
        // strResult = strResult + "<div class='success' style='display:none;'><p>No products currently in list</p></div>"

    }

    $('#all-product ul').append(strResult);

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



//Begin Categories & Sub-Categories
function GetAllCategories() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            clearconsole();
            WriteResponse(data);

        },
        error: function (x, y, z) {
            clearconsole();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
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

function SearchData(dis, CatId, SubCatId) {
    psubCategoryId = SubCatId;
    if (pcategoryId != CatId) {
        $('#all-product ul').empty();

        //Select Category Background color
        $('ul.primary_nav li').removeClass('active');
        $(dis).parent().addClass("active");

        pcategoryId = CatId;
        GetSubCategoriesByCategory(pcategoryId);


        //        $('ul.primary_nav li').removeClass('active');
        //        $("ul.primary_nav li").each(function () {
        //            if ($(this).attr('catid') != 'undefined' && $(this).attr('catid') == pcategoryId) {
        //                $(this).addClass("active");
        //            }
        //        });
    }
    //assign subcatIds to checkboxes
    $("ul.category.departments input:checkbox[name=checkboxlist]").each(function () {
        if ($(this).attr("fieldvalue") == SubCatId) {
            $(this).prop('checked', true);
        }
    });

    GetProducts();
}

function SearchByCategory(dis, id) {

    $('ul.primary_nav li').removeClass('active');
    // $('li.menu_cont a').style.removeAttribute("display");

    $(dis).parent().addClass("active");
    //  $(dis).style.createAttribute("display","block");

    $('#all-product ul').empty();
    pcategoryId = id;

    GetSubCategoriesByCategory(pcategoryId);
    psubCategoryId = 0;

    $("#lblSelectedCategory").text($(dis).text());


    //GetSubCategoriesByCategory(pcategoryId);
    GetProducts();
}

function SearchBySubCategory(catId, subCatid) {
    $('#all-product ul').empty();
    pcategoryId = catId;
    psubCategoryId = subCatid;

    $('ul.primary_nav li').removeClass('active');

    $("ul.primary_nav li").each(function () {

        if ($(this).attr('catid') != 'undefined' && $(this).attr('catid') == pcategoryId) {
            $(this).addClass("active");
        }
    });

    GetSubCategoriesByCategory(pcategoryId);
    GetProducts();
}

function SortBy() {
    $('#all-product ul').empty();
    GetProducts();
}

function GetProducts() {

    if (checkPage()) {
        window.location.href = "index.aspx?cid=" + pcategoryId + "&scid=" + psubCategoryId;
    }
    else {
        $('div#last_msg_loader').html('<img src="catalog/view/theme/leisure/image/loading.gif">');

        if (isNew == true) {
            $('#all-product ul').empty();
        }
        else {
            isNew = true;
        }

        if (pcategoryId == null || pcategoryId == 0)
            pcategoryId = 1;


        psord = $("#ddlProductSort option:selected").val();
        prows = $('#all-product ul > li').length;

        //  $("ul.category.departments").html(strResult);
        var selectedSubCategories = "";
        var selectedSubCategoryNames = "";

        //        $("ul.category.departments input:checkbox[name=checkboxlist]").each(function () {
        //            //    var val = $(this).attr("fieldvalue");
        //            if ($(this).attr("fieldvalue") == psubCategoryId) {
        //                $(this).prop('checked', true);
        //                pcategoryId = 0;
        //            }


        //            //   selectedSubCategories = selectedSubCategories == "" ? $(this).attr("fieldvalue") : selectedSubCategories + "^" + $(this).attr("fieldvalue");
        //        });

        $("ul.category.departments input:checkbox[name=checkboxlist]:checked").each(function () {
            //    var val = $(this).attr("fieldvalue");
            selectedSubCategories = selectedSubCategories == "" ? $(this).attr("fieldvalue") : selectedSubCategories + "^" + $(this).attr("fieldvalue");
            selectedSubCategoryNames = selectedSubCategoryNames == "" ? $(this).attr("fieldtext") : selectedSubCategoryNames + "," + $(this).attr("fieldtext");

        });

        $("#lblSelectedSubCategories").text(selectedSubCategoryNames);


        var Colors = "";
        $("ul.side_sub_menu input:checkbox[name=Color_s]:checked").each(function () {
            // $('ul.side_sub_menu').each(function () {
            var filterName = $(this).attr('name');
            var filterType = $(this).attr('filterType');
            if (filterType == "checkbox" && $(this).attr("checked") == "checked") {
                //    $("input:checkbox[name=" + filterName + "]:checked").each(function (dixi) {
                Colors = Colors == "" ? $(this).val() : Colors + "^" + $(this).val();
                //   });
            }
        });

        jQuery.support.cors = true;
        $.ajax({
            url: siteURL + 'api/Master/GetProductList?sord=' + psord + '&minprice=' + pminprice + '&maxprice=' + pmaxprice + '&SubCategories=' + selectedSubCategories + '&Colors=' + Colors + '&rows=' + prows + '&categoryId=' + pcategoryId + '&subCategoryId=' + psubCategoryId,
            type: 'GET',
            dataType: 'json',

            success: function (data) {
                clearconsole();
                WriteProductsResponse(data);
            },
            error: function (x, y, z) {
                clearconsole();
                $('.success, .warning, .attention, .information, .error').remove();
                $('.error').fadeIn('slow');

            }
        });
    }
}

function WriteProductsResponse(products) {
    var strResult = "";
    var isRecordExists = false;

    var data = GetCurrency();
    var cost = 0;

    // for (var i = 0; i < 20; i++) {
    $.each(products, function (index, item) {

        cost = parseFloat(data.Value * item.ProductCost).toFixed(2);

        var rows = $('#all-product ul > li').length;
        if (rows == 0) {
            strResult += "<li><a  href='" + siteURL + "product/" + item.ProductName + "/" + item.ProductId + "' class='product_image'><img  class='lazyload' width='250' height='425' src='Lazy/grey.gif' original='" + item.ProductImgUrl.replace("~/", siteURL) + "' alt='" + item.ProductName + "' /></a>";
            strResult += "<div class='product_info'>";
            strResult += "<h3><a href='" + siteURL + "product/" + item.Product.ProductName + "/" + item.ProductId + "'>" + item.ProductName + "</a></h3>";
            strResult += "   </div>"; //<small>" + item.ProductDescription + "</small> 
            strResult += "<div class='price_info'> <a onClick='addToWishList(" + item.ProductId + ");'>+ Add to Wish List</a>";
            strResult += "<button class='price_add' title='' type='button' onClick='NavigateToDetails(" + item.ProductId + ");'>";
            strResult += "<span class='pr_price'>" + data.Symbol + " " + cost + "</span>";
            strResult += "<span class='pr_add'>Add to Cart</span></button>";
            strResult += "</div></li>";

        } else {
            isRecordExists = false;
            $("#all-product ul a").each(function () {

                var anchor = $(this).attr('href');
                if (anchor != undefined) {
                    var querystring = $(this).attr('href').split("?")[1]; //GuessID=123&someother=123
                    if (querystring != undefined) {
                        var values = querystring.split("&")[0];
                        var first = values.split("id=")[1];  // GuessID=123
                        if (first != undefined && item.ProductId == first) {
                            isRecordExists = true;
                        }
                    }
                }
            });

            if (isRecordExists == false) {
                strResult += "<li><a  href='" + siteURL + "product/" + item.ProductName + "/" + item.ProductId + "' class='product_image'><img class='lazyload'  width='250' height='425' src='Lazy/grey.gif' original='" + item.ProductImgUrl.replace("~/", siteURL) + "' alt='" + item.ProductName + "' /></a>";
                strResult += "<div class='product_info'>";
                strResult += "<h3><a href='" + siteURL + "product/" + item.Product.ProductName + "/" + item.ProductId + "' style='overflow:hidden;text-overflow:ellipsis;'>" + item.ProductName + "</a></h3>";
                strResult += "   </div>"; //<small>" + item.ProductDescription + "</small> 
                strResult += "<div class='price_info'> <a onClick='addToWishList(" + item.ProductId + ");'>+ Add to Wish List</a>";
                strResult += "<button class='price_add' title='' type='button' onClick='NavigateToDetails(" + item.ProductId + ");'>";
                strResult += "<span class='pr_price'>" + data.Symbol + " " + cost + "</span>";
                strResult += "<span class='pr_add'>Add to Cart</span></button>";
                strResult += "</div></li>";
            }

        }
    });
    //  }

    $('#all-product ul').append(strResult);

    $("img").lazyload({ threshold: "200", effect: "fadeIn", effectspeed: 2000 });

    $("img.lazyload").each(function () {
        $(this).attr("src", $(this).attr("original"));
        $(this).removeAttr("original");
    });

    $('div#last_msg_loader').empty();
    if (products.length == 0) {
        $('div#last_msg_loader').html('<div class="success"><p>No More Products.</p></div>');
    }
    loading = true;
}

//End Products

//Begin Categories & Sub-Categories
function GetAll() {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            clearconsole();
            WriteResp(data);

        },
        error: function (x, y, z) {
            clearconsole();
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




//Begin  Sub-Categories
function GetSubCategoriesByCategory(id) {
    jQuery.support.cors = true;
    $.ajax({
        url: siteURL + 'api/Master/GetSubCategoryListByCategory?id=' + id,
        type: 'GET',

        dataType: 'json',
        success: function (data) {
            clearconsole();
            WriteSubCategories(data);

        },
        error: function (x, y, z) {
            clearconsole();
            $('.success, .warning, .attention, .information, .error').remove();
            $('.error').fadeIn('slow');

        }
    });
}

function WriteSubCategories(subCategories) {
    var strResult = "";
    strResult += "";

    $.each(subCategories, function (index, item) {

        if (index == 0)
            $("#category_nameheading").text(item.CategoryName);

        strResult += "<li><a href='#' ><input type='checkbox' name='checkboxlist' fieldvalue='" + item.SubCategoryId + "'  fieldtext='" + item.SubCategoryName + "'   onclick='GetProducts()'/>   " + item.SubCategoryName + "</a></li>";
    });

    $("#category_departments").html(strResult);
    $('ul.departments > li.menu_cont > a.active + .side_sub_menu').slideDown(300);;

}
//End  Sub-Categories

//Payment

var merchantURLPart = "https://www.citruspay.com/chettinads";
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
    var currency = document.getElementById("currency").value;
    var param = "merchantId=" + vanityURLPart + "&orderAmount=" + orderAmount
				+ "&merchantTxnId=" + merchantTxnId + "&currency=" + currency;
    reqObj.onreadystatechange = process;
    reqObj.open("POST", "hmac_signature.aspx?" + param, false);
    reqObj.send(null);
}
function process() {
    if (reqObj.readyState == 4) {
        document.getElementById("secSignature").value = reqObj.responseText;
        submitForm();
    }
}

function submitForm() {
    $("#form1").attr('action', merchantURLPart);
    $("#form1").attr('method', 'POST');
    $('#form1').submit();
    //   alert('hi');
}

function MakePayment(product_id, quantity, isRedirect, specifications) {
    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        fillData();
        $.ajax({
            contentType: "application/json; charset=utf-8",
            url: siteURL + "PersonService.asmx/MakePayment",
            type: 'post',
            data: '{}',
            dataType: 'json',
            success: function (json) {
                clearconsole();
                json = json.d;
                document.getElementById("merchantTxnId").value = json['id'];
                generateHMAC();
            }
        });
    }
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
            clearconsole();
            data = json.d;

        }
    });
    return data;
}



function fillData() {
    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
        alert('Session got expired during inactive for long time.Please login again.');
    }
    else {
        //  alert(location.protocol + "//" + location.host + "/PaymentStatus.aspx");
        document.getElementById("returnUrl").value = location.protocol + "//" + location.host + "/PaymentStatus.aspx";
        //  alert(document.getElementById("returnUrl").value); 

        document.getElementById("firstName").value = json['firstName'];
        document.getElementById("lastName").value = json['lastName'];
        document.getElementById("email").value = json['email'];
        document.getElementById("phoneNumber").value = json['phoneNumber'];
        //   document.getElementById("orderAmount").value = json['orderAmount'];
        document.getElementById("addressStreet1").value = json['addressStreet1'];
        document.getElementById("addressCity").value = json['addressCity'];
        document.getElementById("addressState").value = json['addressState'];
        document.getElementById("addressZip").value = json['addressZip'];
        //        document.getElementById("orderAmount").value = parseFloat(json['CurrencyValue'] * json['orderAmount']).toFixed(2);
        document.getElementById("orderAmount").value = parseFloat(json['orderAmount']).toFixed(2);
        document.getElementById("currency").value = json['CurrencyCode'];
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
            clearconsole();
            json = json.d;
            data = json;

        }
    });
    return data;
}

//End Currency

// UserProductTransactions
function UserProductTransactionsGrid() {
    var json = CheckLogin();
    if (json['UserId'] == undefined || json['UserId'] == null || json['UserId'] == '0') {
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