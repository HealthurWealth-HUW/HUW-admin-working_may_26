
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
$(document).ready(function () {
	/* Search */
	$('.button-search').bind('click', function() {
		url = 'index.php?route=product/search';
		 
		var filter_name = $('input[name=\'filter_name\']').attr('value')
		
		if (filter_name) {
			url += '&filter_name=' + encodeURIComponent(filter_name);
		}
		
		location = url;
	});

	
	
	$('#header input[name=\'filter_name\']').keydown(function(e) {
		if (e.keyCode == 13) {
			url = 'index.php?route=product/search';
			 
			var filter_name = $('input[name=\'filter_name\']').attr('value')
			
			if (filter_name) {
				url += '&filter_name=' + encodeURIComponent(filter_name);
			}
			
			location = url;
		}
	});
	
	/* Ajax Cart */
	$('#cart > .heading a').bind('click', function() {
		$('#cart').addClass('active');
		
		$.ajax({
			url: 'index.php?route=checkout/cart/update',
			dataType: 'json',
			success: function(json) {
				if (json['output']) {
					$('#cart .content').html(json['output']);
				}
			}
		});			
		
		$('#cart').bind('mouseleave', function() {
			$(this).removeClass('active');
		});
	});
	
	/* Mega Menu */
	$('#menu ul > li > a + div').each(function(index, element) {
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
			$('#menu > ul > li').bind('mouseover', function() {
				$(this).addClass('active');
			});
				
			$('#menu > ul > li').bind('mouseout', function() {
				$(this).removeClass('active');
			});	
		}
	}
});

$('.success img, .warning img, .attention img, .information img').live('click', function() {
	$(this).parent().fadeOut('slow', function() {
		$(this).remove();
	});
});

function addToCart(product_id) {
	$.ajax({
		url: 'index.php?route=checkout/cart/update',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information, .error').remove();
			
			if (json['redirect']) {
				location = json['redirect'];
			}
			
			if (json['error']) {
				if (json['error']['warning']) {
					$('#notification').html('<div class="warning" style="display: none;">' + json['error']['warning'] + '<img src="catalog/view/theme/default/image/close.png" alt="" class="close" /></div>');
				}
			}	 
						
			if (json['success']) {
				$('#notification').html('<div class="attention" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/default/image/close.png" alt="" class="close" /></div>');
				
				$('.attention').fadeIn('slow');
				
				$('#cart_total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow'); 
			}	
		}
	});
}

function removeCart(key) {
	$.ajax({
		url: 'index.php?route=checkout/cart/update',
		type: 'post',
		data: 'remove=' + key,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
			
			if (json['output']) {
				$('#cart_total').html(json['total']);
				
				$('#cart .content').html(json['output']);
			}			
		}
	});
}

function removeVoucher(key) {
	$.ajax({
		url: 'index.php?route=checkout/cart/update',
		type: 'post',
		data: 'voucher=' + key,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
			
			if (json['output']) {
				$('#cart_total').html(json['total']);
				
				$('#cart .content').html(json['output']);
			}			
		}
	});
}

function addToWishList(product_id) {
	$.ajax({
		url: 'index.php?route=account/wishlist/update',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
						
			if (json['success']) {
				$('#notification').html('<div class="attention" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/default/image/close.png" alt="" class="close" /></div>');
				
				$('.attention').fadeIn('slow');
				
				$('#wishlist_total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow'); 				
			}	
		}
	});
}

function addToCompare(product_id) { 
	$.ajax({
		url: 'index.php?route=product/compare/update',
		type: 'post',
		data: 'product_id=' + product_id,
		dataType: 'json',
		success: function(json) {
			$('.success, .warning, .attention, .information').remove();
						
			if (json['success']) {
				$('#notification').html('<div class="attention" style="display: none;">' + json['success'] + '<img src="catalog/view/theme/default/image/close.png" alt="" class="close" /></div>');
				
				$('.attention').fadeIn('slow');
				
				$('#compare_total').html(json['total']);
				
				$('html, body').animate({ scrollTop: 0 }, 'slow'); 
			}	
		}
	});
}

//Begin Super Category

function SuperCategoryGrid() {

    $("#list").jqGrid({
        datatype: 'json',
        url: siteURL + 'api/Master/GetSuperCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "CategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['SuperCategoryId', 'SuperCategoryName', 'IsActive', 'IsDeleted', 'CreatedOn', 'UpdatedOn', 'CreatedBy', 'UpdatedBy'],
        colModel: [
                    { name: 'SuperCategoryId', index: 'SuperCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'SuperCategoryName', index: 'SuperCustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                    { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                      editoptions: { size: 12, dataInit: function (el) {
                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                      }
                      }, sorttype: "date"
                  },
               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                   editoptions: { size: 12, dataInit: function (el) {
                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                   }
                   }, sorttype: "date"
               },

                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
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
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'SuperCategoryId');
                return value;
            }
            }, url: siteURL + "api/Master/EditSuperCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
                url: siteURL + "api/Master/CreateSuperCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SuperCategoryId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteSuperCategory", mtype: 'GET',
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
        url:  siteURL+'api/Master/GetCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "CategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['CategoryId', 'CategoryName', 'SuperCategoryName', 'SuperCategoryName', 'IsActive', 'IsDeleted', 'CreatedOn', 'UpdatedOn', 'CreatedBy', 'UpdatedBy'],
        colModel: [
            { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'CategoryName', index: 'CategoryName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //           
        //                    { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
        //                    { name: 'CategoryName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

 {name: 'SuperCategoryId', index: 'SuperCategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl:  siteURL+'api/Master/GetSuperCategoryListAsHtml', width: 150} },
                   { name: 'SuperCategoryName', index: 'SuperCategoryName', width: 100, viewable: true },

                    { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                    { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                      editoptions: { size: 12, dataInit: function (el) {
                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                      }
                      }, sorttype: "date"
                  },
               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                   editoptions: { size: 12, dataInit: function (el) {
                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                   }
                   }, sorttype: "date"
               },

                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
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
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'CategoryId');
                return value;
            }
            }, url:  siteURL+"api/Master/EditCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { url:  siteURL+"api/Master/CreateCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SubCategoryId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteCategory", mtype: 'GET',
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
        url:  siteURL+'api/Master/GetSubCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "CategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['SubCategoryId', 'SubCategoryName', 'CategoryName', 'CategoryName', 'IsActive', 'IsDeleted', 'CreatedOn', 'UpdatedOn', 'CreatedBy', 'UpdatedBy'],
        colModel: [
            { name: 'SubCategoryId', index: 'SubCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'SubCategoryName', index: 'SubCategoryName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
        //           
        //                    { name: 'CategoryId', index: 'CategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
        //                    { name: 'CategoryName', index: 'CustomerName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

 {name: 'CategoryId', index: 'CategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl:  siteURL+'api/Master/GetCategoryListAsHtml', width: 150} },
                   { name: 'CategoryName', index: 'CategoryName', width: 100, viewable: true },

                    { name: 'IsActive', index: 'LastName', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                    { name: 'IsDeleted', index: 'EmailID', width: 100, editable: true, editrules: { required: true }, edittype: 'checkbox' },
                  { name: 'CreatedOn', index: 'CreatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                      editoptions: { size: 12, dataInit: function (el) {
                          setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                      }
                      }, sorttype: "date"
                  },
               { name: 'UpdatedOn', index: 'UpdatedOn', width: 100, editable: true, editrules: { required: true }, edittype: 'text', formatter: 'date', formatoptions: { srcformat: 'd/m/Y', newformat: 'd/m/Y' },
                   editoptions: { size: 12, dataInit: function (el) {
                       setTimeout(function () { $(el).datepicker(); $(el).datepicker("option", "dateFormat", "dd-mm-yy"); }, 300);
                   }
                   }, sorttype: "date"
               },

                 { name: 'CreatedBy', index: 'Village', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'UpdatedBy', index: 'ZipCode', width: 100, editable: true, edittype: 'text'}],
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
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'SubCategoryId');
                return value;
            }
            }, url:  siteURL+"api/Master/EditSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { url:  siteURL+"api/Master/CreateSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'SubCategoryId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteSubCategory", mtype: 'GET',
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
        url:  siteURL+'api/Master/GetBusinessTypeList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "BusinessId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['BusinessId', 'BusinessTypeName', 'BusinessTypeDescription'],
        colModel: [
                    { name: 'BusinessId', index: 'BusinessId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'BusinessName', index: 'BusinessName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'BusinessDescription', index: 'BusinessDescription', width: 100, editable: true, editrules: { required: true }, edittype: 'text'}],
                   
        pager: '#pager',
        sortname: 'BusinessId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Customer Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'BusinessId');
                return value;
            }
            }, url:  siteURL+"api/Master/EditBusinessType", closeOnEscape: true, reloadAfterSubmit: true,
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
            { url:  siteURL+"api/Master/CreateBusinessType", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'BusinessId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteBusinessType", mtype: 'GET',
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
        url:  siteURL+'api/Master/GetFeaturesCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "FeaturesCategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['FeaturesCategoryId', 'FeaturesCategoryName', 'BusinessTypeName', 'BusinessTypeName'],
        colModel: [
            { name: 'FeaturesCategoryId', index: 'FeaturesCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'FeaturesCategoryName', index: 'FeaturesCategoryName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },
                    { name: 'BusinessId', index: 'BusinessId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl:  siteURL+'api/Master/GetBusinessTypeListAsHtml', width: 150} },
                   { name: 'BusinessName', index: 'BusinessName', width: 100, viewable: true }],
        pager: '#pager',
        sortname: 'FeaturesCategoryId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Customer Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'FeaturesCategoryId');
                return value;
            }
        }, url:  siteURL+"api/Master/EditFeaturesCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { url:  siteURL+"api/Master/CreateFeaturesCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteFeaturesCategory", mtype: 'GET',
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
        url:  siteURL+'api/Master/GetFeaturesSubCategoryList',
        jsonReader: { repeatitems: false },
        loadui: "block",
        key: "FeaturesSubCategoryId",
        mtype: 'GET',
        rowNum: 5,
        autosize: true,
        rowList: [5, 10, 20, 30],
        viewrecords: true,
        colNames: ['FeaturesSubCategoryId', 'FeaturesSubCategoryName', 'FeaturesCategoryName', 'FeaturesCategoryName'],
        colModel: [
            { name: 'FeaturesSubCategoryId', index: 'FeaturesSubCategoryId', width: 100, editoptions: { size: 10 }, editable: true, editrules: { edithidden: false }, hidden: true },
                    { name: 'FeaturesSubCategoryName', index: 'FeaturesSubCategoryName', width: 100, editable: true, editrules: { required: true }, edittype: 'text' },

 { name: 'FeaturesCategoryId', index: 'FeaturesCategoryId', width: 100, editable: true, editrules: { edithidden: true, required: true }, edittype: 'select', hidden: true, editoptions: { dataUrl:  siteURL+'api/Master/GetFeaturesCategoryListAsHtml', width: 150} },
                   { name: 'FeaturesCategoryName', index: 'FeaturesCategoryName', width: 100, viewable: true }],
        pager: '#pager',
        sortname: 'FeaturesSubCategoryId',
        sortorder: 'asc',
        height: "100%",
        width: "100%",
        prmNames: { nd: null, search: null }, // we switch of data caching on the server
        // and not use _search parameter
        caption: 'Customer Records'
    });

    $("#list").jqGrid('navGrid', '#pager', { edit: true, add: true, del: true },
            { editData: { name: function () {
                var selId = $("#list").jqGrid('getGridParam', 'selrow');
                var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                return value;
            }
        }, url:  siteURL+"api/Master/EditFeaturesSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { url:  siteURL+"api/Master/CreateFeaturesSubCategory", closeOnEscape: true, reloadAfterSubmit: true,
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
            { delData: {
                name: function () {
                    var selId = $("#list").jqGrid('getGridParam', 'selrow');
                    var value = $("#list").jqGrid('getCell', selId, 'FeaturesSubCategoryId');
                    return value;
                }
            }, url:  siteURL+"api/Master/DeleteFeaturesSubCategory", mtype: 'GET',
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
//End Features Sub Category
