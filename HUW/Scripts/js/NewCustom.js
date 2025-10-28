//New***********************************************************************

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
var min = 0;
var max = 0;
var productsfromDB = new Array();
function getSelectedBrandProducts() {
    var selectedBrands = $('#dvBrand input[type=checkbox]:checked').map(function () {
        return $(this).val();
    }).get();
    if (selectedBrands.length != 0) {
        $('#product-list').empty();
        var htmlString = "";
        htmlString += '<ul class="products-grid ajaxMdl3">';
        var numofProd = 0;
        for (i = 0; i < selectedBrands.length; i++) {
            if (i == 0) {
                htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + (numofProd) + '</strong>&nbsp;Product(s)</div></div>';
            }
            var brand = selectedBrands[i].split(',')[3];
            var json = productsfromDB;
            if (json.length == 0) {
                //sidelist = data.Message;
                htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
            }

            $.each(json, function (idx, obj) {
                if (obj.Brand != null) {
                    if (brand.trim() == obj.Brand.trim()) {
                        numofProd++;
                        //var pName = obj.ProductName;
                        if (obj.ProductName.indexOf('...')) {
                            obj.ProductName = obj.ProductName.substring(0, 30);
                        }
                        htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                        htmlString += '<a href="' + siteURL + 'product/' + obj.ProductName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                        htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                        htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + obj.ProductName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                        if (obj.ProductDiscountPercentage > 0) {
                            var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                            htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        else {
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                            htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                        htmlString += '</div><div class="product_icons" align="center">'
                        htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                        htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                        htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                    }
                }
            });

        }
        htmlString += '</ul>';
        $('#product-list').append(htmlString);
        $('#numberOfProducts').text(numofProd);
    }
    else {
        var sid = 0;
        var id = 0;
        var subid = 0;

        $.ajax({
            url: siteURL + 'api/Master/GetCategoryInputData',
            type: 'GET',
            dataType: 'json', async: false,
            success: function (data) {
                if (data != "") {
                    id = data;
                }
            }
        });

        if (id == 0 && subid == 0) {
            getSelectedSuperCategoryProducts(sid);
        }
        else
            getSelectedProducts(sid, id, subid, this);
    }
}

function getBrandProducts() {

    var selectedBrands = $('#dvBrand input[type=checkbox]:checked').map(function () {
        return $(this).val();
    }).get();
    if (selectedBrands.length != 0) {
        var json;
        $.ajax({
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: siteURL + 'ProductSearch.aspx/GetBrandProducts',
            data: JSON.stringify({ 'selectedBrands': selectedBrands, 'sord': GetParameter('sord') }),
            success: function (data) {
                $('#productsSearchResults').html(data.d);
            }
        });
    }
}

function getSelectedProducts(sid, id, subid, dis) {
    $('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
    if (dis != null && dis.checked) {
        var chk = $('input[name=checkboxlist]');
        var url = '';
        if (subid == null)
            url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
        else
            url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
        if ($('input[name=checkboxlist]:checked').length != 0) {
            var numofProd = 0;
            var brand = "";
            for (i = 0; i < chk.length; i++) {
                if (chk[i].checked) {
                    sid = chk[i].value.split(',')[0];
                    id = chk[i].value.split(',')[1];
                    subid = chk[i].value.split(',')[2];
                    brand = chk[i].value.split(',')[3];
                    var url = '';
                    if (subid == null || subid == undefined)
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                    else
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            //$('#dvSubCat').empty();
                            var json = $.parseJSON(data.Result);
                            numofProd = parseInt(numofProd) + json.length;
                            var sidelist = '';
                            var htmlString = "";
                            if (json.length == 0) {
                                sidelist = data.Message;
                                htmlString = '<div class="NoProducts" align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                            }
                            else {
                                $.each(json, function (idx, obj) {
                                    productsfromDB.push(obj);
                                    if (idx == 0) {
                                        sidelist += obj.sidelist;
                                        htmlString += '<ul class="products-grid ajaxMdl3">';
                                        // htmlString += obj.breadcrumb
                                        htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + (numofProd) + '</strong>&nbsp;Product(s)</div></div>';
                                    }
                                    var pName = obj.ProductName;
                                    if (obj.ProductName.length > 33) {
                                        obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                                    }
                                    htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                                    htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                                    htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                                    htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                                    if (obj.ProductDiscountPercentage > 0) {
                                        var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                        htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                                    }
                                    else {
                                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                        htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                                    }
                                    //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                                    htmlString += '</div><div class="product_icons" align="center">'
                                    htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                                    htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                                    htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                                });
                            }
                            htmlString += '</ul>';
                            $('#loadingGifProducts').hide();
                            //$('#dvSubCat').append(sidelist);
                            $('#product-list').append(htmlString);
                            $('#numberOfProducts').html(numofProd);
                            setTimeout(clearBreadCrumb, 3000);
                            $('.NoProducts').hide();
                            $('.NoProducts:first').show();
                        },
                        error: function (x, y, z) {
                            var data;
                            ShowInfo(data, '');
                        }
                    });
                }
                else {
                    $('.NoProducts').hide();
                    $('.NoProducts:first').show();
                }
            }
        }
    }
    else if (min == 0 && max == 0) {
        productsfromDB = new Array();
        var chk = $('input[name=checkboxlist]');
        if ($('input[name=checkboxlist]:checked').length != 0) {
            for (i = 0; i < chk.length; i++) {
                if (chk[i].checked) {
                    sid = chk[i].value.split(',')[0];
                    id = chk[i].value.split(',')[1];
                    subid = chk[i].value.split(',')[2];
                    var url = '';
                    if (subid == null || subid == undefined)
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                    else
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {

                            //$('#dvSubCat').empty();
                            var json = $.parseJSON(data.Result);
                            var sidelist = '';
                            var htmlString = "";
                            if (json.length == 0) {
                                sidelist = data.Message;
                                htmlString = '<div class="NoProducts" align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                            }
                            $.each(json, function (idx, obj) {
                                productsfromDB.push(obj);
                                if (idx == 0) {
                                    sidelist += obj.sidelist;
                                    htmlString += '<ul class="products-grid ajaxMdl3">';
                                    //htmlString += obj.breadcrumb
                                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                                }
                                var pName = obj.ProductName;
                                if (obj.ProductName.length > 33) {
                                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                                }
                                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                                htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                                if (obj.ProductDiscountPercentage > 0) {
                                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                    htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                else {
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                    htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                                htmlString += '</div><div class="product_icons" align="center">'
                                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                            });
                            htmlString += '</ul>';
                            $('#loadingGifProducts').hide();
                            //$('#dvSubCat').append(sidelist);
                            $('#product-list').append(htmlString);
                            setTimeout(clearBreadCrumb, 3000);
                            $('.NoProducts').hide();
                            $('.NoProducts:first').show();
                        },
                        error: function (x, y, z) {
                            var data;
                            ShowInfo(data, '');
                        }
                    });
                }
            }
        }
        else {

            var sid = 0;
            var id = 0;
            var subid = 0;

            $.ajax({
                url: siteURL + 'api/Master/GetCategoryInputData',
                type: 'GET',
                dataType: 'json', async: false,
                success: function (data) {

                    if (data != "") {
                        var idsinfo = data.split("#");
                        sid = idsinfo[0];
                        id = idsinfo[1];
                        subid = idsinfo[2];
                    }
                }
            });
            if (subid == null || subid == undefined)
                url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
            else
                url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
            if (subid != null && $('input[name=checkboxlist]:checked').length == 0)
                url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#product-list').empty();
                    //$('#dvSubCat').empty();
                    var json = $.parseJSON(data.Result);
                    var sidelist = '';
                    var htmlString = "";
                    if (json.length == 0) {
                        sidelist = data.Message;
                        htmlString = '<div class="NoProducts" align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                    }
                    $.each(json, function (idx, obj) {
                        productsfromDB.push(obj);
                        if (idx == 0) {
                            sidelist += obj.sidelist;
                            htmlString += '<ul class="products-grid ajaxMdl3">';
                            //htmlString += obj.breadcrumb
                            htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                        }
                        var pName = obj.ProductName;
                        if (obj.ProductName.length > 33) {
                            obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                        }
                        htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                        htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                        htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                        htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                        if (obj.ProductDiscountPercentage > 0) {
                            var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                            htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        else {
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                            htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                        htmlString += '</div><div class="product_icons" align="center">'
                        htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                        htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                        htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                    });
                    htmlString += '</ul>';
                    $('#loadingGifProducts').hide();
                    //$('#dvSubCat').append(sidelist);
                    $('#product-list').append(htmlString);
                    setTimeout(clearBreadCrumb, 3000);
                    $('.NoProducts').hide();
                    $('.NoProducts:first').show();
                },
                error: function (x, y, z) {
                    var data;
                    ShowInfo(data, '');
                }
            });

        }
    }
    else {
        var chk = $('input[name=checkboxlist]');

        for (i = 0; i < chk.length; i++) {
            if (chk[i].checked) {
                sid = chk[i].value.split(',')[0];
                id = chk[i].value.split(',')[1];
                subid = chk[i].value.split(',')[2];
                var url = '';
                if (subid == null)
                    url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                else
                    url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
                $.ajax({
                    url: url,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        $('#product-list').empty();
                        //$('#dvSubCat').empty();
                        var json = $.parseJSON(data.Result);
                        var sidelist = '';
                        var htmlString = "";
                        if (json.length == 0) {
                            sidelist = data.Message;
                            htmlString = '<div class="NoProducts" align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                        }
                        $.each(json, function (idx, obj) {
                            if (idx == 0) {
                                sidelist += obj.sidelist;
                                htmlString += '<ul class="products-grid ajaxMdl3">';
                                //htmlString += obj.breadcrumb
                                htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                            }
                            if (parseInt(obj.ProductCost) >= parseInt(min) && parseInt(obj.ProductCost) <= parseInt(max)) {
                                productsfromDB.push(obj);
                                var pName = obj.ProductName;
                                if (obj.ProductName.length > 33) {
                                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                                }
                                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                                htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                                if (obj.ProductDiscountPercentage > 0) {
                                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                    htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                else {
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                    htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                                htmlString += '</div><div class="product_icons" align="center">'
                                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                            }
                        });
                        htmlString += '</ul>';
                        $('#loadingGifProducts').hide();
                        //$('#dvSubCat').append(sidelist);
                        $('#product-list').append(htmlString);
                        setTimeout(clearBreadCrumb, 3000);
                    },
                    error: function (x, y, z) {
                        var data;
                        ShowInfo(data, '');
                    }
                });
            }
        }

    }


}

function NumberOfProducrts() {
    $('#numberOfProducts').html($('#product-list li.item').length);
}

function clearBreadCrumb() {
    $('.numberOfProductsClass').parentsUntil('.breadcrumbs').hide();
    $('.numberOfProductsClass:first').parentsUntil('.breadcrumbs').show();
}

function getSortedProducts(dis) {
    $('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
    var chk = $('input[name=checkboxlist]');
    var brand = "";
    if ($('input[name=checkboxlist]:checked').length != 0) {
        $('#product-list').empty();
        if (min != 0 && max != 0) {
            for (i = 0; i < chk.length; i++) {
                if (chk[i].checked) {
                    sid = chk[i].value.split(',')[0];
                    id = chk[i].value.split(',')[1];
                    subid = chk[i].value.split(',')[2];
                    var url = '';
                    brand = chk[i].value.split(',')[3];
                    if (subid == null)
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + dis.value + '&brand=' + brand
                    else
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + dis.value + '&brand=' + brand
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            //$('#dvSubCat').empty();
                            var json = $.parseJSON(data.Result);
                            var sidelist = '';
                            var htmlString = "";
                            if (json.length == 0) {
                                sidelist = data.Message;
                                htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                            }
                            $.each(json, function (idx, obj) {
                                if (idx == 0) {
                                    sidelist += obj.sidelist;
                                    htmlString += '<ul class="products-grid ajaxMdl3">';
                                    if (GetURLParameter('sid') == '8')
                                        obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                                    //htmlString += obj.breadcrumb
                                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                                }
                                if (parseInt(obj.ProductCost) >= parseInt(min) && parseInt(obj.ProductCost) <= parseInt(max)) {
                                    var pName = obj.ProductName;
                                    if (obj.ProductName.length > 33) {
                                        obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                                    }
                                    htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                                    htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                                    htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                                    htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                                    if (obj.ProductDiscountPercentage > 0) {
                                        var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                        htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                                    }
                                    else {
                                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                        htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                                    }

                                    //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                                    htmlString += '</div><div class="product_icons" align="center">'
                                    htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                                    htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                                    htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                                }
                            });
                            htmlString += '</ul>';
                            //$('#dvSubCat').append(sidelist);
                            $('#loadingGifProducts').hide();
                            $('#product-list').append(htmlString);
                            setTimeout(clearBreadCrumb, 3000);
                        },
                        error: function (x, y, z) {
                            var data;
                            ShowInfo(data, '');
                        }
                    });
                }
            }
        }
        else {
            for (i = 0; i < chk.length; i++) {
                if (chk[i].checked) {
                    sid = chk[i].value.split(',')[0];
                    id = chk[i].value.split(',')[1];
                    subid = chk[i].value.split(',')[2];
                    var url = '';
                    if (subid == null)
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=' + dis.value + '&brand=' + brand
                    else
                        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + dis.value + '&brand=' + brand
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            //$('#dvSubCat').empty();
                            var json = $.parseJSON(data.Result);
                            var sidelist = '';
                            var htmlString = "";
                            if (json.length == 0) {
                                sidelist = data.Message;
                                htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                            }
                            $.each(json, function (idx, obj) {
                                if (idx == 0) {
                                    sidelist += obj.sidelist;
                                    htmlString += '<ul class="products-grid ajaxMdl3">';
                                    //htmlString += obj.breadcrumb
                                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                                }
                                var pName = obj.ProductName;
                                if (obj.ProductName.length > 33) {
                                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                                }
                                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                                htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                                if (obj.ProductDiscountPercentage > 0) {
                                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                    htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                else {
                                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                    htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                                }
                                // htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                                htmlString += '</div><div class="product_icons" align="center">'
                                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                            });
                            htmlString += '</ul>';
                            $('#loadingGifProducts').hide();
                            //$('#dvSubCat').append(sidelist);
                            $('#product-list').append(htmlString);
                            setTimeout(clearBreadCrumb, 3000);
                        },
                        error: function (x, y, z) {
                            var data;
                            ShowInfo(data, '');
                        }
                    });
                }
            }
        }
    }
    else {
        $('#product-list').empty();
        var sid = 0;
        var id = 0;
        var subid = 0;

        $.ajax({
            url: siteURL + 'api/Master/GetCategoryInputData',
            type: 'GET',
            dataType: 'json', async: false,
            success: function (data) {

                if (data != "") {
                    var idsinfo = data.split("#");
                    sid = idsinfo[0];
                    id = idsinfo[1];
                    subid = idsinfo[2];
                }
            }
        });
        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=' + $('#ddlProductSort').val() + '&brand=' + brand
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                //$('#dvSubCat').empty();
                var json = $.parseJSON(data.Result);
                var sidelist = '';
                var htmlString = "";
                if (json.length == 0) {
                    sidelist = data.Message;
                    htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                }
                $.each(json, function (idx, obj) {
                    if (idx == 0) {
                        sidelist += obj.sidelist;
                        htmlString += '<ul class="products-grid ajaxMdl3">';
                        //htmlString += obj.breadcrumb
                        htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                    }
                    var pName = obj.ProductName;
                    if (obj.ProductName.length > 33) {
                        obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                    }
                    htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                    htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                    htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                    htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                    if (obj.ProductDiscountPercentage > 0) {
                        var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                        htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                    }
                    else {
                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                        htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                    }
                    //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                    htmlString += '</div><div class="product_icons" align="center">'
                    htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                    htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                    htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                });
                htmlString += '</ul>';
                $('#loadingGifProducts').hide();
                //$('#dvSubCat').append(sidelist);
                $('#product-list').append(htmlString);
                setTimeout(clearBreadCrumb, 3000);
            },
            error: function (x, y, z) {
                var data;
                ShowInfo(data, '');
            }
        });

    }
}

function GetURLParameter(sParam) {
    var sPageURL = window.location.href;
    var sURLVariables = sPageURL.split('/');
    return sURLVariables[sURLVariables.length - 1];
}

function GetParameter(sParam) {
    var sPageURL = window.location.href;
    var sURLVariables = sPageURL.split('=');
    return sURLVariables[sURLVariables.length - 1];
}

function getSelectedCategoryProducts(subCatID, dis) {
    if ($('#dvCategory input:checked').length == 0) {
        var sid = 0;
        var id = 0;
        var subid = 0;

        $.ajax({
            url: siteURL + 'api/Master/GetSuperCategoryInputData',
            type: 'GET',
            dataType: 'json', async: false,
            success: function (data) {
                if (data != "") {
                    sid = data;
                }
            }
        });
        if (sid != 0) {
            getSelectedSuperCategoryProducts(sid);
        }
        else
            getSelectedProducts(sid, id, subid, this);
    }
    else {
        $('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
        var htmlString = '';
        var noOfProducts = 0;
        var inS = 0;
        $('#dvCategory input:checked').each(function (idx1, ele1) {
            var prodList = $.grep(productsfromDB, function (ele, idx) {
                return ele.SubCategoryId == $(ele1).attr('id');
            });
            noOfProducts += prodList.length;

            $.each(prodList, function (idx, obj) {
                if (inS == 0) {
                    htmlString += '<ul class="products-grid ajaxMdl3">';
                    if (GetURLParameter('sid') == '8')
                        obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li> Offers </li> </ul></div>'
                    //htmlString += obj.breadcrumb
                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + noOfProducts + '</strong>&nbsp;Product(s)</div></div>';
                }
                inS++;
                var pName = obj.ProductName;
                if (obj.ProductName.length > 33) {
                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                }
                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                htmlString += '<div class="price-box" style="text-align:center;">';
                if (obj.ProductDiscountPercentage > 0) {
                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                    htmlString += "<div style='float:left; font-size:13px; color:#999999;'><s>Rs" + ProductCostbeforediscount + "</s>" + " |<span> " + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                    htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                }
                else {
                    htmlString += '<div></div>';
                    htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'

                }
                htmlString += '</div><div class="product_icons" align="center">'
                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
            });
            setTimeout(function () {
                $('.numberOfProductsClass').html(noOfProducts);
            }, 500);
        });
        htmlString += '</ul>';
        $('#loadingGifProducts').hide();
        $('#product-list').append(htmlString);
    }
}

function SuperCategoryPage(value) {
    //$('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
    var sid = 0;

    $.ajax({
        url: siteURL + 'api/Master/GetSuperCategoryInputData',
        type: 'GET',
        dataType: 'json', async: false,
        success: function (data) {
            if (data != "") {
                sid = data;
            }
        }
    });

    var brandUrl = siteURL + 'api/Master/GetAllBrands?SuperCatId=' + sid + '&SubId=0&CatId=0&sort=0&brand=';

    $.ajax({
        url: siteURL + 'api/Master/GetAllProductsBySuperCategory?SuperCatId=' + sid + '&sort=0&value=' + value,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            //$('#product-list').empty();
            var json = $.parseJSON(data.Result);
            var htmlString = "";

            $.each(json, function (idx, obj) {
                productsfromDB.push(obj);
                if (idx == 0) {
                    //htmlString += '<ul class="products-grid ajaxMdl3">';

                    //if (value == 1) {
                    //    if (GetURLParameter('sid') == '8')
                    //        obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                    //    htmlString += obj.breadcrumb
                    //}

                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass"> 15 </strong>&nbsp;Product(s)</div></div>';
                }


                var pName = obj.ProductName;
                if (obj.ProductName.length > 33) {
                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                }
                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                htmlString += '<a target="_blank" href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" ><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                if (obj.ProductDiscountPercentage > 0) {
                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                    htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                }
                else {
                    htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                    htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                }
                //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + obj.ProductCost + '</span></div>'
                htmlString += '</div><div class="product_icons" align="center">'
                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
            });

            $("#dvCategory").empty();
            var CategoryString = "<ul id='side_menu'>";
            $.extend({
                distinctObj: function (obj, propertyName) {
                    var result = [];
                    $.each(obj, function (i, v) {
                        var prop = eval("v." + propertyName);
                        if ($.inArray(prop, result) == -1) result.push(prop);
                    });
                    return result;
                }
            });
            var SubCategoryIds = $.distinctObj(productsfromDB, 'SubCategoryId');
            var CategoryIds = $.distinctObj(productsfromDB, 'CategoryId');
            if (parseInt(sid) == 8) {
                $.each(SubCategoryIds, function (idx, obj1) {
                    var obj = $.grep(productsfromDB, function (ele, idx) { return ele.SubCategoryId == obj1 })[0];
                    CategoryString += "<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + obj.SubCategoryId + "'  onclick='getSelectedCategoryProducts(" + obj.SubCategoryId + ", this)'  />  " + obj.SubCategoryName + "</label></a></li>";
                });
            }
            else {
                $.each(CategoryIds, function (idx, obj1) {
                    var obj = $.grep(productsfromDB, function (ele, idx) { return ele.CategoryId == obj1 })[0];
                    CategoryString += "<li id='side_menu1'><a style='color:#d9387e;font-weight:Bold' href='" + siteURL + "" + obj.SuperCategoryName + "/" + obj.CategoryName + "/" + obj.SuperCategoryId + "/" + obj.CategoryId + "' id='" + obj.CategoryId + "'> " + obj.CategoryName + "</a></li>";
                    $.each(SubCategoryIds, function (idx, obj2) {
                        var object = $.grep(productsfromDB, function (ele, idx) { return ele.SubCategoryId == obj2 })[0];
                        if (obj.CategoryId == object.CategoryId) {
                            CategoryString += "<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + object.SubCategoryId + "'  onclick='getSelectedCategoryProducts(" + object.SubCategoryId + ", this)'  />  " + object.SubCategoryName + "</label></a></li>";
                        }
                    });
                });
            }
            CategoryString += "</ul>";
            //htmlString += "</ul>";
            $('#loadingGifProducts').hide();
            $('#viewmoreProducts').parent('div').find('#loadingGif').hide();
            $('#product-list').append(htmlString);
            $("#dvCategory").append(CategoryString);
            $('#product-list').removeClass('loading');
            setTimeout(clearBreadCrumb, 3000);
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });
    $.ajax({
        url: brandUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#dvBrand').empty();
            var json = $.parseJSON(data.Result);
            var sidelist = '';
            var Brand = '';
            var htmlString = "";
            $.each(json, function (idx, obj) {

                if (idx == 0) {
                    sidelist += obj.sidelist;
                    Brand += obj.ProductName;
                    //Brand += obj.Brand;
                    htmlString += '<ul class="products-grid ajaxMdl3">';
                    //htmlString += obj.breadcrumb
                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                }
            });
            htmlString += '</ul>';
            $('#dvBrand').append(sidelist);
            if ($('#dvBrand #side_menu').height() < 300) {
                $('#dvBrand').removeAttr('style');
            }

        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });
}


function BetweenPrice(id) {
    var MinPrice = 0;
    var MaxPrice = 10000;
    if (id == "below2500") {
        MinPrice = 0;
        MaxPrice = 2500;
    }
    else if (id == "2500to5000") {
        MinPrice = 2500;
        MaxPrice = 5000;
    }
    else if (id == "5000to7500") {
        MinPrice = 5000;
        MaxPrice = 7500;
    }
    else if (id == "7500to10000") {
        MinPrice = 7500;
        MaxPrice = 10000;
    }
    else if (id == "10000above") {
        MinPrice = 10000;
        MaxPrice = 100000;
    }

    var sid = 0;
    var id = 0;
    var subid = 0;

    $.ajax({
        url: siteURL + 'api/Master/GetCategoryInputData',
        type: 'GET',
        dataType: 'json', async: false,
        success: function (data) {

            if (data != "") {
                var idsinfo = data.split("#");
                sid = idsinfo[0];
                id = idsinfo[1];
                subid = idsinfo[2];
            }
        }
    });

    if (sid == 0 && id == 0 && subid == 0) {
        $.ajax({
            url: siteURL + 'api/Master/GetSuperCategoryInputData',
            type: 'GET',
            dataType: 'json', async: false,
            success: function (data) {
                if (data != "") {
                    sid = data;
                }
            }
        });
    }


    if (sid != 0 && id == 0 && subid == 0) {
        $.ajax({
            url: siteURL + 'api/Master/GetAllProductsBetweenPriceinSuperCategory?SuperCatId=' + sid + '&sort=' + $('#ddlProductSort').val() + '&MinPrice=' + MinPrice + '&MaxPrice=' + MaxPrice,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#product-list').empty();
                var json = $.parseJSON(data.Result);
                var htmlString = "";

                $.each(json, function (idx, obj) {
                    productsfromDB.push(obj);

                    if (idx == 0) {

                        htmlString += '<ul class="products-grid ajaxMdl3">';
                        if (GetURLParameter('sid') == '8')
                            obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                        //htmlString += obj.breadcrumb
                        htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                    }
                    var pName = obj.ProductName;
                    if (obj.ProductName.length > 33) {
                        obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                    }
                    htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                    htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                    htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                    htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                    if (obj.ProductDiscountPercentage > 0) {
                        var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                        htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                    }
                    else {
                        htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                        htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                    }
                    //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + obj.ProductCost + '</span></div>'
                    htmlString += '</div><div class="product_icons" align="center">'
                    htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                    htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                    htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                });
                var CategoryString = "<ul id='side_menu'>";
                $.extend({
                    distinctObj: function (obj, propertyName) {
                        var result = [];
                        $.each(obj, function (i, v) {
                            var prop = eval("v." + propertyName);
                            if ($.inArray(prop, result) == -1) result.push(prop);
                        });
                        return result;
                    }
                });
                var SubCategoryIds = $.distinctObj(productsfromDB, 'SubCategoryId');
                var CategoryIds = $.distinctObj(productsfromDB, 'CategoryId');
                if (parseInt(sid) == 8) {
                    $.each(SubCategoryIds, function (idx, obj1) {
                        var obj = $.grep(productsfromDB, function (ele, idx) { return ele.SubCategoryId == obj1 })[0];
                        CategoryString += "<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + obj.SubCategoryId + "'  onclick='getSelectedCategoryProducts(" + obj.SubCategoryId + ", this)'  />  " + obj.SubCategoryName + "</label></a></li>";
                    });
                }
                else {
                    $.each(CategoryIds, function (idx, obj1) {
                        var obj = $.grep(productsfromDB, function (ele, idx) { return ele.CategoryId == obj1 })[0];
                        CategoryString += "<li id='side_menu1'><a style='color:#d9387e;font-weight:Bold' href='" + siteURL + "" + obj.SuperCategoryName + "/" + obj.CategoryName + "/" + obj.SuperCategoryId + "/" + obj.CategoryId + "' id='" + obj.CategoryId + "'> " + obj.CategoryName + "</a></li>";
                        $.each(SubCategoryIds, function (idx, obj2) {
                            var object = $.grep(productsfromDB, function (ele, idx) { return ele.SubCategoryId == obj2 })[0];
                            if (obj.CategoryId == object.CategoryId) {
                                CategoryString += "<li id='side_menu1'><a href='#'><label><input type='checkbox' name='checkboxlist' id='" + object.SubCategoryId + "'  onclick='getSelectedCategoryProducts(" + object.SubCategoryId + ", this)'  />  " + object.SubCategoryName + "</label></a></li>";
                            }
                        });
                    });
                }
                CategoryString += "</ul>";
                htmlString += '</ul>';
                $('#loadingGifProducts').hide();
                $('#product-list').append(htmlString);
                $("#dvCategory").append(CategoryString);
                setTimeout(clearBreadCrumb, 3000);
            },
            error: function (x, y, z) {
                var data;
                ShowInfo(data, '');
            }
        });
    }
    else {
        if (subid == null || subid == 0)
            url = siteURL + 'api/Master/GetAllProductsBetweenPriceinCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=0&brand=' + "" + '&MinPrice=' + MinPrice + '&MaxPrice=' + MaxPrice;
        else
            url = siteURL + 'api/Master/GetAllProductsBetweenPriceinCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=0&brand=' + "" + '&MinPrice=' + MinPrice + '&MaxPrice=' + MaxPrice;
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                $('#product-list').empty();
                $('#dvSubCat').empty();
                var json = $.parseJSON(data.Result);
                var sidelist = '';
                var Brand = '';
                var htmlString = "";
                if (json.length == 0) {
                    sidelist = data.Message;
                    //Brand = data.CartSum;
                    htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
                }
                $.each(json, function (idx, obj) {
                    productsfromDB.push(obj);
                    if (idx == 0) {
                        sidelist += obj.sidelist;
                        // Brand += obj.Brand;
                        htmlString += '<ul class="products-grid ajaxMdl3">';
                        //htmlString += obj.breadcrumb
                        htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                    }
                    var pName = obj.ProductName;
                    if (obj.ProductName.length > 33) {
                        obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                    }
                    htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'

                    htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                    htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                    htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                    htmlString += '<div class="price-box" style="text-align:center;">';
                    if (obj.ProductDiscountPercentage > 0) {
                        var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                        htmlString += "<div style='float:left; font-size:13px; color:#999999;'><s>Rs" + ProductCostbeforediscount + "</s>" + " |<span> " + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                        htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                    }
                    else {
                        htmlString += '<div></div>';
                        htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'

                    }
                    htmlString += '</div><div class="product_icons" align="center">'
                    htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                    htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                    htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                });
                htmlString += '</ul>';
                $('#loadingGifProducts').hide();
                $('#dvSubCat').append(sidelist);
                $('#product-list').append(htmlString);

                var cid = GetURLParameter('Subid');
                $('input[id=' + cid + ']').prop('checked', true);
                setTimeout(clearBreadCrumb, 3000);
            },
            error: function (x, y, z) {
                var data;
                ShowInfo(data, '');
            }
        });
    }

}

function CategoryPage() {
    $('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
    var sid = 0;
    var id = 0;
    var subid = 0;

    $.ajax({
        url: siteURL + 'api/Master/GetCategoryInputData',
        type: 'GET',
        dataType: 'json', async: false,
        success: function (data) {

            if (data != "") {
                var idsinfo = data.split("#");
                sid = idsinfo[0];
                id = idsinfo[1];
                subid = idsinfo[2];
            }
        }
    });

    var url = '';
    var brandurl = '';
    if (subid == undefined || subid == 0)
        brandUrl = siteURL + 'api/Master/GetAllBrands?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=0&brand=';
    else
        brandUrl = siteURL + 'api/Master/GetAllBrands?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=0&brand=';

    if (subid == null || subid == 0)
        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=0&CatId=' + id + '&sort=0&brand=';
    else
        url = siteURL + 'api/Master/GetAllProductsByCategory?SuperCatId=' + sid + '&SubId=' + subid + '&CatId=' + id + '&sort=0&brand=';
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#product-list').empty();
            $('#dvSubCat').empty();
            var json = $.parseJSON(data.Result);
            var sidelist = '';
            var Brand = '';
            var htmlString = "";
            if (json.length == 0) {
                sidelist = data.Message;
                //Brand = data.CartSum;
                htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
            }
            $.each(json, function (idx, obj) {
                productsfromDB.push(obj);
                if (idx == 0) {
                    sidelist += obj.sidelist;
                    // Brand += obj.Brand;
                    htmlString += '<ul class="products-grid ajaxMdl3">';
                    ////htmlString += obj.breadcrumb
                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                }
                var pName = obj.ProductName;
                if (obj.ProductName.length > 33) {
                    obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                }
                htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'

                htmlString += '<a target="_blank" href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" ><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                htmlString += '<div class="price-box" style="text-align:center;">';
                if (obj.ProductDiscountPercentage > 0) {
                    var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                    htmlString += "<div style='float:left; font-size:13px; color:#999999;'><s>Rs" + ProductCostbeforediscount + "</s>" + " |<span> " + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                    htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                }
                else {
                    htmlString += '<div></div>';
                    htmlString += '<div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'

                }
                htmlString += '</div><div class="product_icons" align="center">'
                htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                htmlString += '<ul class="add-to-links"></ul></div></div></li>'
            });
            htmlString += '</ul>';
            $('#loadingGifProducts').hide();
            $('#dvSubCat').append(sidelist);
            $('#product-list').append(htmlString);

            var cid = GetURLParameter('Subid');
            $('input[id=' + cid + ']').prop('checked', true);
            setTimeout(clearBreadCrumb, 3000);
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });
    $.ajax({
        url: brandUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#dvBrand').empty();
            var json = $.parseJSON(data.Result);
            var sidelist = '';
            var Brand = '';
            var htmlString = "";
            if (json.length == 0) {
                sidelist = data.Message;
                //Brand = data.CartSum;
                htmlString = '<div align="center" style="padding-top:30px;"><h5>There are no Products</h5></div>';
            }
            $.each(json, function (idx, obj) {
                if (idx == 0) {
                    sidelist += obj.sidelist;
                    // Brand += obj.Brand;
                    htmlString += '<ul class="products-grid ajaxMdl3">';
                    //htmlString += obj.breadcrumb
                    htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                }
            });
            htmlString += '</ul>';
            $('#dvBrand').append(sidelist);
            if ($('#dvBrand #side_menu').height() < 300) {
                $('#dvBrand').removeAttr('style');
            }
        },
        error: function (x, y, z) {
            var data;
            ShowInfo(data, '');
        }
    });
}

function getSelectedSuperCategoryProducts(dis) {

    $('#product-list').html('<img id="loadingGifProducts" src="' + siteURL + 'images/giphy.gif" />');
    var sid = 0;
    var id = 0;
    var subid = 0;

    $.ajax({
        url: siteURL + 'api/Master/GetSuperCategoryInputData',
        type: 'GET',
        dataType: 'json', async: false,
        success: function (data) {
            if (data != "") {
                sid = data;
            }
        }
    });
    if (dis == null) {
        if (min == 0 && max == 0) {
            var url = '';
            url = siteURL + 'api/Master/GetAllProductsBySuperCategory?SuperCatId=' + sid + '&sort=' + $('#ddlProductSort').val() + '&value=0',
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#product-list').empty();
                    //$('#dvSubCat').empty();
                    var json = $.parseJSON(data.Result);
                    var sidelist = '';
                    var htmlString = "";
                    $.each(json, function (idx, obj) {
                        if (idx == 0) {
                            sidelist += obj.sidelist;
                            htmlString += '<ul class="products-grid ajaxMdl3">';
                            if (dis == '8')
                                obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                            //htmlString += obj.breadcrumb
                            htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                        }
                        var pName = obj.ProductName;
                        if (obj.ProductName.length > 33) {
                            obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                        }
                        htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                        htmlString += '<a target="_blank" href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                        htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                        htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                        if (obj.ProductDiscountPercentage > 0) {
                            var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                            htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        else {
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                            htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                        htmlString += '</div><div class="product_icons" align="center">'
                        htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                        htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                        htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                    });
                    htmlString += '</ul>';
                    $('#loadingGifProducts').hide();
                    //$('#dvSubCat').append(sidelist);
                    $('#product-list').append(htmlString);

                    var cid = GetURLParameter('Subid');
                    $('input[id=' + cid + ']').prop('checked', true);
                    setTimeout(clearBreadCrumb, 3000);
                },
                error: function (x, y, z) {
                    var data;
                    ShowInfo(data, '');
                }
            });
        }
        else {
            $('#product-list').empty();
            var url = '';
            url = siteURL + 'api/Master/GetAllProductsBySuperCategory?SuperCatId=' + sid + '&sort=' + $('#ddlProductSort').val() + '&value=0'
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    //$('#dvSubCat').empty();
                    var json = $.parseJSON(data.Result);
                    var sidelist = '';
                    var htmlString = "";

                    $.each(json, function (idx, obj) {
                        if (idx == 0) {
                            sidelist += obj.sidelist;
                            htmlString += '<ul class="products-grid ajaxMdl3">';
                            if (GetURLParameter('sid') == '8')
                                obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li> <a title="Go to Offers Page" href=' + siteURL + '"Offers/8">Offers</li> </ul></div>'
                            //htmlString += obj.breadcrumb

                            htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                        }


                        if (parseInt(obj.ProductCost) >= parseInt(min) && parseInt(obj.ProductCost) <= parseInt(max)) {

                            var pName = obj.ProductName;
                            if (obj.ProductName.length > 33) {
                                obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                            }
                            htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                            htmlString += '<a target="_blank" href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                            htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                            htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                            if (obj.ProductDiscountPercentage > 0) {
                                var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                            }
                            else {
                                htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                            }
                            //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                            htmlString += '</div><div class="product_icons" align="center">'
                            htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                            htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                            htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                        }
                    });
                    htmlString += '</ul>';
                    $('#loadingGifProducts').hide();

                    //$('#dvSubCat').append(sidelist);
                    $('#product-list').append(htmlString);
                    setTimeout(clearBreadCrumb, 3000);
                    NumberOfProducrts();
                },
                error: function (x, y, z) {
                    var data;
                    ShowInfo(data, '');
                }
            });
        }
    }
    else if (dis != null) {
        $('#product-list').empty();
        var url = '';
        url = siteURL + 'api/Master/GetAllProductsBySuperCategory?SuperCatId=' + sid + '&sort=' + $('#ddlProductSort').val() + '&value=0'
        if (min == 0 && max == 0) {
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    //$('#dvSubCat').empty();
                    var json = $.parseJSON(data.Result);
                    var sidelist = '';
                    var htmlString = "";
                    $.each(json, function (idx, obj) {
                        if (idx == 0) {
                            sidelist += obj.sidelist;
                            htmlString += '<ul class="products-grid ajaxMdl3">';
                            if (GetURLParameter('sid') == '8')
                                obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                            //htmlString += obj.breadcrumb
                            htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                        }
                        var pName = obj.ProductName;
                        if (obj.ProductName.length > 33) {
                            obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                        }
                        htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                        htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                        htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                        htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                        if (obj.ProductDiscountPercentage > 0) {
                            var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                            htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        else {
                            htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                            htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                        }
                        //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                        htmlString += '</div><div class="product_icons" align="center">'
                        htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                        htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                        htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                    });
                    htmlString += '</ul>';
                    $('#loadingGifProducts').hide();
                    //$('#dvSubCat').append(sidelist);
                    $('#product-list').append(htmlString);
                    setTimeout(clearBreadCrumb, 3000);
                },
                error: function (x, y, z) {
                    var data;
                    ShowInfo(data, '');
                }
            });
        }
        else {
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    //$('#dvSubCat').empty();
                    var json = $.parseJSON(data.Result);
                    var sidelist = '';
                    var htmlString = "";
                    $.each(json, function (idx, obj) {
                        if (idx == 0) {
                            sidelist += obj.sidelist;
                            htmlString += '<ul class="products-grid ajaxMdl3">';
                            if (GetURLParameter('sid') == '8')
                                obj.breadcrumb = '<div class="breadcrumbs"> <ul>  <li class="home"> <a title="Go to Home Page" href=' + siteURL + '>Home</a><span>&gt;</span></li> <li>Offers</li> </ul></div>'
                            //htmlString += obj.breadcrumb
                            htmlString += '<div class="breadcrumbs"><div style="color:#66BCDA; font-weight:bold;">Showing&nbsp;<strong style="color:#D9387E;" id="numberOfProducts" class="numberOfProductsClass">' + json.length + '</strong>&nbsp;Product(s)</div></div>';
                        }
                        if (parseInt(obj.ProductCost) >= parseInt(min) && parseInt(obj.ProductCost) <= parseInt(max)) {
                            var pName = obj.ProductName;
                            if (obj.ProductName.length > 33) {
                                obj.ProductName = obj.ProductName.substring(0, 30) + " ...";
                            }
                            htmlString += '<li class="item first"><div class="outer_pan"><div class="image_rotate"><div class="image_rotate_inner">'
                            htmlString += '<a href="' + siteURL + 'product/' + pName + '/' + obj.ProductId + '" class="product-image" target="_blank"><img src="' + obj.ProductImgUrl.replace("~/", siteURL) + '" alt="' + obj.ProductName + '" width="220px;" height="210px;"></a></div>'
                            htmlString += '</div></div><div class="outer_bottom"><h2 class="product-name">'
                            htmlString += '<a style="overflow:hidden;text-overflow:ellipsis;  width:225px;item last quick10" title=' + pName + ' id="prolink">' + obj.ProductName + '</a></h2>'
                            if (obj.ProductDiscountPercentage > 0) {
                                var ProductCostbeforediscount = Math.round(obj.ProductCost + (obj.ProductCost * (obj.ProductDiscountPercentage / 100)) + (obj.ProductCost * (0.05)));
                                htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<s>Rs" + obj.ProductOriginalCost + "</s>" + "|<span>" + obj.ProductDiscountPercentage + "%off  </span> </div> ";
                                htmlString += '<div></div>  <div style="text-align:center;" class="price-box"> <span style="color:#D9387E; font-size:18px; font-weight:bold;">&nbsp;&nbsp;₹' + Math.round(obj.ProductCost) + '</span></div>'
                            }
                            else {
                                htmlString += "<div style='text-align:center; font-size:13px; color:#999999;' class='price-box'>&nbsp;&nbsp;<div></div>";
                                htmlString += '<div></div>  <div style="text-align:center;"> <span style="color:#D9387E; font-size:18px; font-weight:bold;"> ₹' + Math.round(obj.ProductCost) + '</span></div>'
                            }
                            //htmlString += '<div class="price-box" style="text-align:center;"><div></div>  <div style="align:center;"> <span style="color:#D9387E; font-size:19px; font-weight:bold;">₹ ' + Math.round(obj.ProductCost) + '</span></div>'
                            htmlString += '</div><div class="product_icons" align="center">'
                            htmlString += '<table><tbody><tr><td style="padding-top:10px;"><a class="link-wishlist" onclick="addToWishList(' + obj.ProductId + ')">Add to Wishlist</a>  </td></tr>'
                            htmlString += '<tr><td></td><td></td></tr></tbody></table>';
                            htmlString += '<ul class="add-to-links"></ul></div></div></li>'
                        }
                    });
                    htmlString += '</ul>';
                    $('#loadingGifProducts').hide();
                    //$('#dvSubCat').append(sidelist);
                    $('#product-list').append(htmlString);
                    setTimeout(clearBreadCrumb, 3000);
                },
                error: function (x, y, z) {
                    var data;
                    ShowInfo(data, '');
                }
            });
        }
    }
}

//New***********************************************************************