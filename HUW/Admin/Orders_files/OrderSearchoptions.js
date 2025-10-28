/*Json Parser Only Needed In The Case Of IE 7*/ 
if (!this.JSON) { JSON = function () { function f(n) { return n < 10 ? "0" + n : n } Date.prototype.toJSON = function () { return this.getUTCFullYear() + "-" + f(this.getUTCMonth() + 1) + "-" + f(this.getUTCDate()) + "T" + f(this.getUTCHours()) + ":" + f(this.getUTCMinutes()) + ":" + f(this.getUTCSeconds()) + "Z" }; var escapeable = /["\\\x00-\x1f\x7f-\x9f]/g, gap, indent, meta = { "\b": "\\b", "\t": "\\t", "\n": "\\n", "\f": "\\f", "\r": "\\r", '"': '\\"', "\\": "\\\\" }, rep; function quote(string) { return escapeable.test(string) ? '"' + string.replace(escapeable, function (a) { var c = meta[a]; if (typeof c === "string") { return c } c = a.charCodeAt(); return "\\u00" + Math.floor(c / 16).toString(16) + (c % 16).toString(16) }) + '"' : '"' + string + '"' } function str(key, holder) { var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === "object" && typeof value.toJSON === "function") { value = value.toJSON(key) } if (typeof rep === "function") { value = rep.call(holder, key, value) } switch (typeof value) { case "string": return quote(value); case "number": return isFinite(value) ? String(value) : "null"; case "boolean": case "null": return String(value); case "object": if (!value) { return "null" } gap += indent; partial = []; if (typeof value.length === "number" && !(value.propertyIsEnumerable("length"))) { length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || "null" } v = partial.length === 0 ? "[]" : gap ? "[\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "]" : "[" + partial.join(",") + "]"; gap = mind; return v } if (typeof rep === "object") { length = rep.length; for (i = 0; i < length; i += 1) { k = rep[i]; if (typeof k === "string") { v = str(k, value, rep); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } } else { for (k in value) { v = str(k, value, rep); if (v) { partial.push(quote(k) + (gap ? ": " : ":") + v) } } } v = partial.length === 0 ? "{}" : gap ? "{\n" + gap + partial.join(",\n" + gap) + "\n" + mind + "}" : "{" + partial.join(",") + "}"; gap = mind; return v } } return { stringify: function (value, replacer, space) { var i; gap = ""; indent = ""; if (space) { if (typeof space === "number") { for (i = 0; i < space; i += 1) { indent += " " } } else { if (typeof space === "string") { indent = space } } } if (!replacer) { rep = function (key, value) { if (!Object.hasOwnProperty.call(this, key)) { return undefined } return value } } else { if (typeof replacer === "function" || (typeof replacer === "object" && typeof replacer.length === "number")) { rep = replacer } else { throw new Error("JSON.stringify") } } return str("", { "": value }) }, parse: function (text, reviver) { var j; function walk(holder, key) { var k, v, value = holder[key]; if (value && typeof value === "object") { for (k in value) { if (Object.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v } else { delete value[k] } } } } return reviver.call(holder, key, value) } if (/^[\],:{}\s]*$/.test(text.replace(/\\["\\\/bfnrtu]/g, "@").replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]").replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) { j = eval("(" + text + ")"); return typeof reviver === "function" ? walk({ "": j }, "") : j } throw new SyntaxError("JSON.parse") }, quote: quote} } () };


var arrSearchFields = new Array();

var SearchFields = new Object();
SearchFields.FieldName = "Courier Service";
SearchFields.FieldValueId = "ddlCustomerService";
SearchFields.FieldType = "DropDownList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

var SearchFields = new Object();
SearchFields.FieldName = "Seller";
SearchFields.FieldValueId = "hdnSuppliers";
SearchFields.FieldType = "HiddenField";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

var SearchFields = new Object();
SearchFields.FieldName = "Browse Node";
SearchFields.FieldValueId = "txtBrowseNode";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

var SearchFields = new Object();
SearchFields.FieldName = "Category";
SearchFields.FieldValueId = "txtCategory";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Dispatch Date From";
SearchFields.FieldValueId = "txtfromdate";
SearchFields.FieldType = "DispatchTextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Dispatch Date To";
SearchFields.FieldValueId = "txttodate";
SearchFields.FieldType = "DispatchTextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Orders From Date";
SearchFields.FieldValueId = "txtAllOrdersFrom";
SearchFields.FieldType = "OrdersTextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Orders To Date";
SearchFields.FieldValueId = "txtAllOrdersTo";
SearchFields.FieldType = "OrdersTextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

//Dynamic

SearchFields = new Object();
SearchFields.FieldName = "Order No";
SearchFields.FieldValueId = "txtOrderNo";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtOrderNo' class='searchby_txt' value='' onkeypress='return validatenumbers(event)' />";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "SKU";
SearchFields.FieldValueId = "txtSku";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtSku' value='' class='searchby_txt'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Awb No";
SearchFields.FieldValueId = "txtAwb";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtAwb' value='' class='searchby_txt'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);


SearchFields = new Object();
SearchFields.FieldName = "Email Id";
SearchFields.FieldValueId = "txtEmailId";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtEmailId' value='' class='searchby_txt'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);


SearchFields = new Object();
SearchFields.FieldName = "Phone No";
SearchFields.FieldValueId = "txtPhoneNo";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtPhoneNo' value='' class='searchby_txt'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);


SearchFields = new Object();
SearchFields.FieldName = "Coupon Code";
SearchFields.FieldValueId = "txtCouponCode";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtCouponCode' value='' class='searchby_txt'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Checkout Type";
SearchFields.FieldValueId = "rdbCheckouttype";
SearchFields.FieldType = "CheckBoxList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<div ><input type='checkbox' name='rdbCheckouttype' value='COD' /> <label>COD </label> <input type='checkbox' name='rdbCheckouttype' value='TPG' /><label>Online payment</label></div>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Dispatch Status";
SearchFields.FieldValueId = "rdbDispatchStatus";
SearchFields.FieldType = "CheckBoxList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "</ br><div ><input type='checkbox' name='rdbDispatchStatus' value='Picked up by Courier' /> <label>Picked up by Courier</label> <input type='checkbox' name='rdbDispatchStatus' value='Reached Destination City' /> <label>Reached Destination City</label> <input type='checkbox' name='rdbDispatchStatus' value='Handed Over to Delivery' /> <label>Handed Over to Delivery</label><input type='checkbox' name='rdbDispatchStatus' value='Delivery attempted' /> <label>Delivery attempted</label><input type='checkbox' name='rdbDispatchStatus' value='Dispatched' /> <label>Dispatched</label> </div>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

SearchFields = new Object();
SearchFields.FieldName = "Logistic Type";
SearchFields.FieldValueId = "drpLogisticType";
SearchFields.FieldType = "DropDownList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<div ><select name='drpLogisticType' id='drpLogisticType'> <option value=''>Select</option>  <option value='Market Place'>Market Place</option>  <option value='Seller'>Seller</option>  </select></div>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

//Issuetype
SearchFields = new Object();
SearchFields.FieldName = "Issue Type";
SearchFields.FieldValueId = "chkIssueType";
SearchFields.FieldType = "CheckBoxList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<div ><input type='checkbox' name='chkIssueType' value='Lost in Transit' /> <label>Lost in Transit </label> <input type='checkbox' name='chkIssueType' value='Returned to Orgin' /><label>Returned to Orgin</label></div> <input type='checkbox' name='chkIssueType' value='Other Issues' /><label>Other Issues</label></div>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

//txtSupplier
var SearchFields = new Object();
SearchFields.FieldName = "Seller";
SearchFields.FieldValueId = "txtSupplier";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

//txtShipingNo
SearchFields = new Object();
SearchFields.FieldName = "Shipment ID";
SearchFields.FieldValueId = "txtShipmentNo";
SearchFields.FieldType = "TextBox";
SearchFields.ValidationRegex = "";
SearchFields.Template = "<input type='text' id='txtShipmentNo' class='searchby_txt' value='' onkeypress='return validatenumbers(event)'/>";
SearchFields.IsDynamic = true;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);

//Undelivered Orders
var SearchFields = new Object();
SearchFields.FieldName = "Undelivered Orders";
SearchFields.FieldValueId = "chkOrderwithIssues";
SearchFields.FieldType = "CheckBoxList";
SearchFields.ValidationRegex = "";
SearchFields.Template = "";
SearchFields.IsDynamic = false;
SearchFields.Value = "";
arrSearchFields.push(SearchFields);


function ConstructSearch() {
    $.each(arrSearchFields, function () {
        if (this.IsDynamic && this.Value != "" && this.Value != undefined) {
            ConstructSearchLi(this);
            IsPostBack = true;
        }
    });

    if (!IsPostBack) {
        ConstructSearchLi(arrSearchFields[8]);        
    }

    $('.MasterSearchCriteria').unbind('change').change(function () {
        DropDownChange(this);
    });
}

function DisableOptionsAllDropDowns() {
    //Reset DropDown Of All Feilds
    var OptionsValueToBeDisabled = '';
    $('.SearchCriteriaLi').each(function () {
        OptionsValueToBeDisabled += $(this).attr('rel');
    });
    $('.SearchCriteriaLi').each(function () {
        $(this).find('.MasterSearchCriteria').find('option').each(function () {
            if (OptionsValueToBeDisabled.indexOf($(this).attr('value')) == -1) {
                $(this).attr('disabled', false);
            } else {
                $(this).attr('disabled', true);
            }
        });
    });
}

function DropDownChange(SelectEl) {
    //Change Parent Li Rel Tag
    $(SelectEl).parents('.SearchCriteriaLi').eq(0).attr('rel', $(SelectEl).val());
    $(SelectEl).siblings('span').html($(SelectEl).find('option:selected').text());

    DisableOptionsAllDropDowns();
    var TempObj;
    $.each(arrSearchFields, function () {
        if (this.IsDynamic) {
            if ($(SelectEl).parents('.SearchCriteriaLi').eq(0).attr('rel') == this.FieldName) {
                TempObj = this;
            }
        }
    });

    var TempTemplate = TempObj.Template;
    if ($(SelectEl).parents('.SearchCriteriaLi').next('.SearchCriteriaLi').length > 0) {
        TempTemplate += '<span class="removesearch_node AddNewSearch" rel="Remove"></span>';
    }
    else {
        TempTemplate += '<span class="addsearch_node AddNewSearch" rel="Add"></span>';
    }
    $(SelectEl).parents('.SearchCriteriaLi').find('.TemplateFeild').html(TempTemplate)

    AppyWaterMarker(TempObj.FieldType, TempObj.FieldValueId, TempObj.FieldName, TempObj.Value)

    $(SelectEl).parents('.SearchCriteriaLi').find('.AddNewSearch').unbind('click').click(function (e) {
        if ($(e.target).attr('rel') == 'Add') {
            $(e.target).attr('rel', 'Remove');
            $(e.target).removeClass('addsearch_node').addClass('removesearch_node');
            ConstructSearchLi('');
        } else {
            RemoveSearchLi($(e.target));
        }
    });
}

function clearcontrols() {
    $('input[id*=txtfromdate]').val('');
    $('input[id*=txttodate]').val('');
    $('input[id*=txtAllOrdersFrom]').val('');
    $('input[id*=txtAllOrdersTo]').val('');
    $('input[id*=txtSku]').val('');
    $('input[id*=txtSupplier]').val('');
    $('select[id*=drpOrderStatus]').val('P');
    $('select[id*=drpOrderStatus]').prev().text("Pending");
    $('select[id*=drpcheckouttype]').val('ALL');
    $('select[id*=drpcheckouttype]').prev().text('ALL');
    $('input[id*=txtOrdNo]').val('');
    $('input[id*=txtAwb]').val('');
    $('input[id*=txtEmailId]').val('');
    $('input[id*=txtPhoneNo]').val('');
    $('input[id*=txtOrderNo]').val('');
    $('input[id*=txtnwOrderNo]').val('');
    $('input[id*=txtCouponCode]').val('');
    $('input[id*=txtCategory]').val('');
    $('input[name*=rdbCheckouttype]').each(function () { $(this).parents('span').removeClass('checked'); })
    $('input[name*=rdbDispatchStatus]').each(function () { $(this).parents('span').removeClass('checked'); })
    $('select[id*=drpLogisticType]').find('option').eq(0).attr('selected', true);
    $('select[id*=drpLogisticType]').trigger('change');
    $('select[id*=drpOrderAge]').find('option').eq(0).attr('selected', true);
    $('select[id*=drpOrderAge]').trigger('change');
    ClearHiddenFields();
    $(arrSearchFields).each(function () {
        this.Value = "";
    });
    $('.search_summery').html('');
}



function ConstructSearchLi(Obj) {
    var DynamicSearchCriteria = '';
    if ($('input[id*=TodoSearchcriteria]').val() != '') {
        DynamicSearchCriteria = $('input[id*=TodoSearchcriteria]').val();
    }
    if (!Obj) {
        //Clicked On Plus Button
        $.each(arrSearchFields, function () {
            if (this.IsDynamic && DynamicSearchCriteria.indexOf(this.FieldName) != -1) {
                if ($('.SearchCriteriaLi[rel="' + this.FieldName + '"]').length == 0) {
                    Obj = this;
                    return false;
                } else {
                    Obj = 'AllFeildsCompleted';
                }
            }
        });
    }
    if (Obj != 'AllFeildsCompleted') {
        var TempStr = '';
        TempStr += '<li class="NewSearchCriteriaLi" rel="' + Obj.FieldName + '" style="display:none;">';
        TempStr += '	<label class="search_label">Search by</label>';
        TempStr += '	<div class="form_input fl_left">';
        TempStr += '		<select class="MasterSearchCriteria">';
        var DynamicSearchCriteriaJSON = JSON.parse(DynamicSearchCriteria);
        $(DynamicSearchCriteriaJSON).each(function (index, value) {
            TempStr += '<option Value="' + value + '">' + value + '</option>';
        });
        TempStr += '		 </select>';
        if ($('.SearchCriteriaLi').eq(0).find('.MasterSearchCriteria').find('option').length == ($('.SearchCriteriaLi').length +1)) {
            TempStr += '	<div class="searchby TemplateFeild">' + Obj.Template + '<span class="removesearch_node AddNewSearch" rel="Remove"></span></div>';
        } else {
            TempStr += '	<div class="searchby TemplateFeild">' + Obj.Template + '<span class="addsearch_node AddNewSearch" rel="Add"></span></div>';
        }
        TempStr += '	</div>';
        TempStr += '</li>';

        $('.SearchUi').append(TempStr).find('.NewSearchCriteriaLi').slideDown('slow');

        $('.NewSearchCriteriaLi').find('.MasterSearchCriteria').unbind('change').change(function () {
            DropDownChange(this)
        });

        AppyWaterMarker(Obj.FieldType, Obj.FieldValueId, Obj.FieldName, Obj.Value)

        //Select The value in Drop Down And Add Html To Span
        $('.NewSearchCriteriaLi').find('.MasterSearchCriteria').uniform();
        $('.NewSearchCriteriaLi').find('.MasterSearchCriteria').find('option[value="' + Obj.FieldName + '"]').attr('selected', true);
        $('.NewSearchCriteriaLi').find('.MasterSearchCriteria').siblings('span').html($('.NewSearchCriteriaLi').find('.MasterSearchCriteria').find('option:selected').text());

        $('.NewSearchCriteriaLi').find('.AddNewSearch').unbind('click').click(function (e) {
            if ($(e.target).attr('rel') == 'Add') {
                $(e.target).attr('rel', 'Remove');
                $(e.target).removeClass('addsearch_node').addClass('removesearch_node');
                ConstructSearchLi('');
            } else {
                RemoveSearchLi($(e.target));
            }
        });
        $('.NewSearchCriteriaLi').removeClass('NewSearchCriteriaLi').addClass('SearchCriteriaLi');

        DisableOptionsAllDropDowns();
    }
}

function RemoveSearchLi(DeleteBtnEl) {
    var DeletedOption = $(DeleteBtnEl).parents('.SearchCriteriaLi').eq(0).attr('rel');
    $(DeleteBtnEl).parents('.SearchCriteriaLi').eq(0).slideUp('fast', function () {
        $(this).remove();
        if ($('.SearchCriteriaLi').length < $('.SearchCriteriaLi').eq(0).find('.MasterSearchCriteria').find('option').length) {
            $('.SearchCriteriaLi').last().find('.AddNewSearch').attr('rel', 'Add');
            $('.SearchCriteriaLi').last().find('.AddNewSearch').removeClass('removesearch_node').addClass('addsearch_node');
        }
    });
    $('.SearchCriteriaLi').each(function () {
        $(this).find('.MasterSearchCriteria').find('option').each(function () {
            if ($(this).val() == DeletedOption) {
                $(this).attr('disabled', false);
            }
        });
    });
}

function AppyWaterMarker(FieldType, FieldValueId, FieldName, FieldValue) {
    if (FieldType == 'TextBox') {
        //$('#' + FieldValueId + '').attr('DefaultValue', FieldName);
        //if (FieldValue != '') {
        //    $('#' + FieldValueId + '').val(FieldValue);
        //}
        //$('#' + FieldValueId + '').unbind('focus').focus(function () {
        //    if ($(this).val() == $(this).attr('DefaultValue')) {
        //        $(this).val('');
        //    }
        //});
        //$('#' + FieldValueId + '').unbind('blur').blur(function () {
        //    if ($(this).val() == '') {
        //        $(this).val($(this).attr('DefaultValue'));
        //    }
        //});
    }
    else if (FieldType == 'CheckBoxList') {
        $('input[type="checkbox"][name="' + FieldValueId + '"]').uniform();
    }
    else if (FieldType == 'DropDownList') {
        $('select[name="' + FieldValueId + '"]').uniform();
    }
}

function ShowHideContentPad(status) {
    var isstatus = document.getElementById("margin_10").style.display;
    if (status != '0') {
        if (isstatus == 'none') {
            document.getElementById("margin_10").style.display = 'block';
            $('#btnshowsearch').attr('class', 'btn_search hidesearch');
        }
        else {
            document.getElementById("margin_10").style.display = 'none';
            $('#btnshowsearch').attr('class', 'btn_search showsearch');
        }
    }
}

function SelectedSearchSummary() {
    var TempDynamicSearchObject = [];
    $.each(arrSearchFields, function () {
        if (this.FieldType != undefined) {
            if (this.FieldType == 'TextBox') {
                if (this.IsDynamic) {
                    if (this.Value != '' && this.Value != undefined) {
                        TempDynamicSearchObject.push(this);
                    }
                }
                else {
                    if ($('#' + this.FieldValueId + '').val() != undefined && $('#' + this.FieldValueId + '').val() != '') {
                        this.Value = $('#' + this.FieldValueId + '').val();
                        if ($('#' + this.FieldValueId + '').val() != '') {
                            TempDynamicSearchObject.push(this);
                        }
                    }
                }
            }
            else if (this.FieldType == "DispatchTextBox") {
                if ($("input[id*=txtfromdate]").val() != "" && $("input[id*=txtfromdate]").val() != undefined) {
                    if ($("input[id*=txtfromdate]").attr('id') == this.FieldValueId) {
                        TempDynamicSearchObject.push(this);
                    }
                }
            }
            else if (this.FieldType == "OrdersTextBox") {
                if ($("input[id*=txtAllOrdersFrom]").val() != "") {
                    if ($("input[id*=txtAllOrdersFrom]").attr('id') == this.FieldValueId) {
                        TempDynamicSearchObject.push(this);
                    }
                }
            }
            else if (this.FieldType == 'CheckBoxList') {
                var checkboxchekedvale = '';
                if (this.IsDynamic) {
                    if (this.Value != '' && this.Value != undefined) {
                        checkboxchekedvale = this.Value.replace("COD", "Cash on Delivery").replace("TPG", "Prepaid");
                    }
                }
                else {
                    $('input[name*="' + this.FieldValueId + '"]').each(function () {
                        if ($(this).is(':checked')) {
                            if (checkboxchekedvale == '') {
                                checkboxchekedvale = $(this).val();
                            }
                            else {
                                checkboxchekedvale = checkboxchekedvale + "," + $(this).val();
                            }
                        }
                    });
                }
                if (checkboxchekedvale != '') {
                    this.Value = checkboxchekedvale;
                    TempDynamicSearchObject.push(this);
                }
            }
            else if (this.FieldType == 'DropDownList') {
                if ($("select[id*=" + this.FieldValueId + "]").val() != '' && $("select[id*=" + this.FieldValueId + "]").val() != undefined) {
                    this.Value = $("select[id*=" + this.FieldValueId + "]").val();
                    TempDynamicSearchObject.push(this);
                }
            }
            else if (this.FieldType == 'hidden') {
                if ($("input[id*=" + this.FieldValueId + "]").val() != '' && $("input[id*=" + this.FieldValueId + "]").val() != undefined) {
                    this.Value = $("select[id*=" + this.FieldValueId + "]").val();
                    TempDynamicSearchObject.push(this);
                }
            }
        }
    });
    ConstructSelectedSearchCriteria(TempDynamicSearchObject);
}
function ConstructSelectedSearchCriteria(TempDynamicSearchObject) {
    var SelectedFields = '';
    $.each(TempDynamicSearchObject, function () {
        if (this.FieldType == "DispatchTextBox") {
            if ($("input[id*=txtfromdate]").val() != "" && $("input[id*=txtfromdate]").val() != undefined) {
                if ($("input[id*=txtfromdate]").attr('id') == this.FieldValueId) {
                    SelectedFields = SelectedFields + "<li>";
                    SelectedFields = SelectedFields + "<label>Dispatch Date range: </label>";
                    SelectedFields = SelectedFields + "<span>" + $("input[id*=txtfromdate]").val() + " - " + $("input[id*=txttodate]").val() + "</span>";
                    SelectedFields = SelectedFields + "</li>";
                }
            }
        }
        else if (this.FieldType == "OrdersTextBox") {
            if ($("input[id*=txtAllOrdersFrom]").val() != "") {
                if ($("input[id*=txtAllOrdersFrom]").attr('id') == this.FieldValueId) {
                    SelectedFields = SelectedFields + "<li>";
                    SelectedFields = SelectedFields + "<label>Orders Date range: </label>";
                    SelectedFields = SelectedFields + "<span>" + $("input[id*=txtAllOrdersFrom]").val() + " - " + $("input[id*=txtAllOrdersTo]").val() + "</span>";
                    SelectedFields = SelectedFields + "</li>";
                }
            }
        }
        else {
            SelectedFields = SelectedFields + "<li>";
            SelectedFields = SelectedFields + "<label>" + this.FieldName + ": </label>";
            SelectedFields = SelectedFields + "<span>" + this.Value + "</span>";
            SelectedFields = SelectedFields + "</li>";
        }
    });
    if (SelectedFields != "") {
        $('.search_summery').html("<div class='caption'>Search Criteria</div><ul class='searched_fields'>" + SelectedFields + "</ul><a href='#' onclick='clearcontrols();return false;' class='fl_right clear_search'>Clear Search Criteria</a>");
    }
}

function ClearHiddenFields() {
    $('input[id*=hdnOrderNo]').val('');
    $('input[id*=hdnSKU]').val('');
    $('input[id*=hdnAwbNo]').val('');
    $('input[id*=hdnEmailId]').val('');
    $('input[id*=hdnPhoneNo]').val('');
    $('input[id*=hdnCouponCode]').val('');
    $('input[id*=hdnCheckoutType]').val('');
    $('input[id*=hdnDispatchStatus]').val('');
    $('input[id*=hdnLogisticType]').val('');    
    $("[ID$=hdnSupplierMid]").val('');
    if ($("[ID$=txtCategory]").val() == '' || $("[ID$=txtCategory]").val() == undefined) {
        $("[ID$=hdnCategory]").val('');
    }
}

function validatenumbers(e)
{
 if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
         return false;
    }
}