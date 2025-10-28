var attrCatalogId = 0;
$(document).ready(function() {
	$("#thumb-bottom").click(function(){
		rightCount = rightCount + 1;
		$("#thumb-top").css("visibility","visible");
		if (rightCount == slideThumbSize-6){
			$(this).css("visibility", "hidden");
			rightCount = 0;
		}
	    $("#product-thumbs").animate({"top": "-=68px"}, "fast");
	});
	                
	$("#thumb-top").click(function(){
		leftCount = leftCount + 1;
		if (leftCount == slideThumbSize-6){
			$(this).css("visibility", "hidden");
			leftCount = 0;
		}
		$("#product-thumbs").animate({"top": "+=68px"}, "fast");
		$("#thumb-bottom").css("visibility","visible");
	
	});
	
	cashBackHelp();
});

function cashBackHelp(){
	var posFromTop  = 0;
    $('.cashBackHelp').mouseenter(function(){
   	posFromTop = $('.cashBackHelp').position().top -67;
    	$('#cashBackPopupouter').css('top',posFromTop);
		$('#cashBackPopupouter').css('display','block');
	});
	
	$('.cashBackHelp').mouseleave(function(){
		$('#cashBackPopupouter').css('display','none');
	});
}

function emiValues(sellingPrice){
	var html = '';
	if(parseInt($('#emiMinLimit').html(),10) >0 && sellingPrice >= parseInt($('#emiMinLimit').html(),10)){
		html += '<div class="buyEmiText"> 3 Months EMI : <strong id="3EMI">Rs '+Math.round(sellingPrice/3)+'</strong></div>';
	}
	if(parseInt($('#emiSixMonth').html(),10) >0 && sellingPrice >= parseInt($('#emiSixMonth').html(),10)){
		html += '<div class="buyEmiText"> 6 Months EMI : <strong id="6EMI">Rs '+Math.round(sellingPrice/6)+'</strong></div>';
	}
	if(parseInt($('#emiNineMonth').html(),10) >0 && sellingPrice >= parseInt($('#emiNineMonth').html(),10)){
		html += '<div class="buyEmiText"> 9 Months EMI : <strong id="9EMI">Rs '+Math.round(sellingPrice/9)+'</strong></div>';
	}
	if(parseInt($('#emiTwelveMonth').html(),10) >0 && sellingPrice >= parseInt($('#emiTwelveMonth').html(),10)){
		html += '<div class="buyEmiText"> 12 Months EMI : <strong id="12EMI">Rs '+Math.round(sellingPrice/12)+'</strong></div>';
	}
	return html;
}

function emiProcessingTextHtml(text){
	var html= '';
	html += '<div class="emiProcessingText">';
	html += '	<div>'+text+'</div>';
	html += '</div>';
	return html;
}

function emiAvailableText(text){
	var html = '';
	html += '	<div class="lfloat">'+text+'</div>';
	html += '	<div class="emi-help lfloat"></div>';
	return html;
}

function emiOuterBox(obj){
	var posFromTop  = 0;
	var posFromLeft = 0;
	posFromTop = $(obj).position().top +17;
	posFromLeft = $(obj).position().left-70;
	$('#emi-details-box-outer').css('top',posFromTop);
	$('#emi-details-box-outer').css('left',posFromLeft);
	$('#emi-details-box-outer').show();
	fireOmnitureScriptForEmi();
}

Snapdeal.AttributeHandler = function() {
    var self = this;
    this.attributeNames = {};
    this.init = function(attributeJson) {
        this.attributeJson = attributeJson;
        depth = this.getDepth();
        var selectHtml = "";
        
        for ( var i = 0; i < depth; i++) {
            selectHtml += '<div class="product-attr" id="attr-level-' + i + '" style="display:'+((i>0) ? 'none':'block')+'">';
            selectHtml += '<div class="attrText"><span class="opti-txt">' + this.attributeNames[i] +'</span>:'+ '</div>';
            selectHtml += '<div class="lfloat '+ ((this.attributeNames[i]== 'Color') ? 'colorMenu':'selmenu') +'" level="'+i+ '" id="attribute-select-'+ i+ '"></div>';
        if(this.attributeNames[i]=='Size' && sizeChartAttr){
            selectHtml +='<div class="sizechart"><nobr>View Size Chart</nobr></div>';
        }
        
        selectHtml +='</div>';
        }
        $("#product-option").html(selectHtml);
        var level1Html = "";
        level1Html += "<div class='overhid'>";
        for ( var i = 0; i < this.attributeJson.length; i++) {
        	if(depth == 1 && !this.attributeJson[i].live) {
                continue;
            }
        	var soldOutMessage = "";
            var attrState = "attrActive";
            if(depth == 2) {
            	var subAttributes = this.attributeJson[i].subAttributes;
            	var showFlag = false;
                for ( var j = 0; j < subAttributes.length; j++) {
                    if(subAttributes[j].live){
                    	showFlag = true;
                    	break;
                    }
                }
                if(showFlag == false) {
                	attrState = 'attrDisabled';
                }
            }
            if(depth == 1 && this.attributeJson[i].soldOut) {
            	soldOutMessage = " - Sold Out";
            	attrState = 'attrDisabled';
            }
            if( this.attributeJson[i].value == attrVal) {
				level1Html += "<div class='lfloat pdpAattr " + attrState + " selected' id='" + this.attributeJson[i].value + "' title='"+this.attributeJson[i].value + soldOutMessage+"'><span>" + this.attributeJson[i].value+"<div class='attrState'></div></span></div>";	
            } else {
				level1Html += "<div class='lfloat pdpAattr " + attrState + "' id='" + this.attributeJson[i].value + "' title='"+this.attributeJson[i].value + soldOutMessage+"'><span>" + this.attributeJson[i].value+"<div class='attrState'></div></span></div>";
			} 
        }
        level1Html += "</div>";
        $("#attribute-select-0").html(level1Html);
        $(".attrActive").live("click",function() {
            self.attributeChange($(this));
        });
        
        if(attrVal!=''){
        	$(".selected").trigger('click');
        }
        
//    	$('#attribute-select-0').bxSlider({
//            auto : false,
//            pager : false,
//            infiniteLoop: false,
//            hideControlOnEnd: true,
//        });
//        $("a.bx-prev").html('');
//        $("a.bx-next").html('');
	};

    this.getDepth = function() {
        var depth = 0;
        var attributeNames = {};
        var subAttributes = this.attributeJson;
        while (subAttributes.length > 0) {
            this.attributeNames[depth] = subAttributes[0].name;
            depth++;
            subAttributes = subAttributes[0].subAttributes;
        }
        this.depth = depth;
        return depth;
    };

    this.attributeChange = function(aSelect) {
        var level = parseInt(aSelect.parent().parent().attr("level"));
        var buyLink = Snapdeal.getStaticPath('') + "/checkout?catalog=";
        $(aSelect).parent().parent().attr('slctAttr',aSelect.attr("id"));
        var catalogId = 'undefined';
        var supc = 'undefined';
        var subAttributes = '';
        $("#attribute-select-"+level).children().children().removeClass("selected");
        $(aSelect).addClass('selected');
        $("#attr-level-" + (level + 1)).show();
         if (level != this.depth - 1) {
            var optionHtml = "";
            subAttributes = this.getAttributesAtDepth(level + 1);
            var levelHtml = "";
            var zoomHtml = "";
            $("div[pdpAttr='ColorrBox']").html('<ul id="zoom-select-1" class="sd-droplist" num="1"></ul>');
            levelHtml += "<div class='overhid'>";
            for ( var i = 0; i < subAttributes.length; i++) {
                var soldOutMessage = ""
                var attrState = "attrActive";
                if(!subAttributes[i].live){
                	continue;
                }
                if(level == this.depth - 2 && subAttributes[i].soldOut) {
               	soldOutMessage = " - Sold Out";
                	attrState = 'attrDisabled';
                }
                if(subAttributes[i].name=='Color' || subAttributes[i].name=='Colour'){
               	 	levelHtml += "<div class='lfloat " + attrState + "' id='" + subAttributes[i].value + "' title='"+ subAttributes[i].value + soldOutMessage+"'><span style='background-color:"+ subAttributes[i].value.replace(/\ /g,'') +"'><div class='attrState'></div></span></div>";
               	 	
               	 	if( attrState == "attrActive" ){
               	 		zoomHtml += "<div class='color-code-box' colorName="+subAttributes[i].value+" style='background-color:"+ subAttributes[i].value.replace(/\ /g,'') +"'></div>";	
               	 	}
               	 	
                }else{
               	 	levelHtml += "<div class='lfloat pdpAattr " + attrState + "' id='" + subAttributes[i].value + "' title='"+ subAttributes[i].value + soldOutMessage+"'><span>" + subAttributes[i].value +"<div class='attrState'></div></span></div>";
               	 	zoomHtml += "<li>"+subAttributes[i].value+"</li>";
                }
            }
            levelHtml += "</div>";
            $("#attribute-select-" + (level + 1)).html(levelHtml);
            $("#attribute-select-" + (level + 1)).removeClass('disabled');
            
            if( $("#attribute-select-" + (level + 1)).hasClass("colorMenu") ) {
            	$("div[pdpAttr='ColorrBox']").append(zoomHtml);
            } else {
                $("#zoom-select-" + (level + 1)).html(zoomHtml).show();
                $("#zoom-select-" + (level + 1)).parent().parent().show();
            }            
            
            // disable rest of select options
            for ( var i = level + 2; i < this.depth; i++) {
                $("#attribute-select-" + i).addClass('disabled');
            }
            buyLink += "undefined";
        } else {
       	 	var selectedValue = $("#attribute-select-" + level).attr('slctAttr');
            subAttributes = this.getAttributesAtDepth(level);
            for ( var j = 0; j < subAttributes.length; j++) {
                if (subAttributes[j].value == selectedValue) {
                    buyLink += subAttributes[j].catalogId;
                    catalogId = subAttributes[j].catalogId;
                    supc = subAttributes[j].supc;
                    $('#original-price-id').html(subAttributes[j].price);
                    $('#selling-price-id').html(subAttributes[j].sellingPrice);
                    $('#discount-id').html(subAttributes[j].discount);
                    pincodeButtonClick = false;
                    attrbuteWisePrice(supc,catalogId);
                    attrNotSelected = false;
                    
                    if( subAttributes[j].images != undefined && subAttributes[j].images != null && subAttributes[j].images != "" ) {
                       	this.changeImageOnSizeSelection(subAttributes[j].images);
                     }
                }
            }
         }
        $('#attribute-error-message').hide();
        $('.buylink').attr('href', buyLink);
        $('.buylink').attr('catalog', catalogId);
        $('.buylink').attr('supc',supc);
        if(supc != 'undefined'){
        	$('#hightLightSupc').html(supc);
        };
        $('.verifylink').attr('supc', supc);
	    $('.verifylink').attr('catalogId', catalogId);
	    
    };
    
    /*
    this.changeImageOnSizeSelection = function(contentDTO){
		 $("#product-thumbs li img").each(function(i,v){
			 var thumbnail = contentDTO.mainPictures[i].contentPath.replace('img/product/main/', 'img/product/main/small/');
			 //var largeImage = contentDTO.mainPictures[i].contentPath.replace('img/product/main/', 'img/product/main/large/');
			 $(this).attr('src', Snapdeal.getResourcePath(thumbnail));
			 $("#product-slider li img").eq(i).attr('src', Snapdeal.getResourcePath(contentDTO.mainPictures[i].contentPath) ); 
		 });
	 };*/
    
    this.changeImageOnSizeSelection = function(images){
		 $.each(images, function(a, b){
			 if( b != undefined &&  b != "" ) {
				 var thumbnail = b.replace('img/product/main/', 'img/product/main/small/');
				 $("#product-thumbs li img").eq(a).attr('src', Snapdeal.getResourcePath(thumbnail));
				 $("#product-slider li img").eq(a).attr('src', Snapdeal.getResourcePath(b) );
			 }
		 });		 
	 };

    this.getAttributesAtDepth = function(depth) {
        var subAttributes = this.attributeJson;
        for ( var i = 0; i < depth; i++) {
            var selectedValue = $("#attribute-select-" + i).attr('slctAttr');
            for ( var j = 0; j < subAttributes.length; j++) {
                if (subAttributes[j].value == selectedValue) {
                    subAttributes = subAttributes[j].subAttributes;
                    break;
                }
            }
        }
        return subAttributes;
    };
};

function mvVendorsdataDisplay(data,supc,catalogId){
	var html='';
	var cashback;
	$.each(data , function(index, val){
		if(index==0){
			return true;
		}
		html+="<div class='cont handCursr mVName' id='mVName-"+index+"' style='border-top:"+(index == 1 ? '0px' : '1px')+" solid #f1f1f1'>";
		html+="		<div class='lfloat'>";
		html+="			<div class='wrapper lfloat'>";
		html+="				<div class='truncateText overhid'>"+val.vendorDisplayName+"</div>";
		if(val.overallRating!=undefined){
			html+="<div class='vendorStarsSmall' style='background-position:0px -"+imageOffsetForRating(val.overallRating)*10+"px'>Seller Score: <span class='scoreColor'>"+val.overallRating+"/5</span></div>";
		}else{
			html+="<div class='scoreColor'>New Seller</div>";
		}
		html+="			</div>";
		html+="			<div class='lfloat'>";
		html+="				<div class='greyText fnt10'>Dispatched in "+val.shippingDays+" business days</div>";
		if(val.freebies.length > 0){
			html+="				<div class='freebie-ico lfloat greyText'>Freebies</div>";		
		}
		if(val.freebies.length > 0 && val.externalCashBackApplicable && val.cashBackValue > 0){
			html+="<div class='cashBackPlusImage'></div>";
		}
		if(val.externalCashBackApplicable && val.cashBackValue > 0){
			cashback = val.cashBackValue.toString().split('.');
			if(val.cashBackType == 'PCT'){
				if(cashback[1] > 1){
					html+="      <div class='cashbackMoreSeller'><span>Insta Cashback:</span><span> (-) "+val.cashBackValue+"%</span></div>";
				}else{
					html+="      <div class='cashbackMoreSeller'><span>Insta Cashback:</span><span> (-) "+cashback[0]+"%</span></div>";	
				}
			}
			if(val.cashBackType == 'ABS'){
				html+="      <div class='cashbackMoreSeller'><span>Insta Cashback:</span><span> (-) Rs. "+cashback[0]+"</span></div>";	
			}
		}
		html+="			</div>";
		html+="		</div>";
		html+="		<div class='rfloat' align='right'>";
		html+="			<div class='redText fnt12'><strong>Rs "+val.sellingPrice+"</strong></div>";
		if(val.soldOut){
			html+="			<div class='mvBuyButton-soldOut'><span>SOLD OUT</span></div>";
		}else{
			html+="			<button class='buyBlueButton buylink mvBuyButton-small' catalog='"+catalogId+"' supc='"+supc+"' vendorCode='"+val.vendorCode+"' id='BuyButton-"+(index + 1)+"' vendorScore='"+val.overallRating+"'>Buy</button>";
		}
		html+="		</div>";
		html+="		<div class='clear'></div>";
		html+="		<div id='"+val.vendorCode+"' class='mvPincodeCheck'></div>";	
		html+="		<div class='clear'></div>";
		html+="		<div class='mVTooltipOuter' id='mVTooltip-"+index+"'>";
		html+=" 		<div class='mVTooltipArrow rfloat'></div>";
		html+=" 		<div class='mVTooltipCont lfloat'>";
		html+="				<div class='mvTooltipContText'>";
		html+="  				<div><span class='greyText'>Sold by </span>"+val.vendorDisplayName+"</div>";
		if(val.overallRating!=undefined){
			html+="<div class='vendorStarsSmall' style='background-position:0px -"+imageOffsetForRating(val.overallRating)*10+"px'>Seller Score: <span class='scoreColor'>"+val.overallRating+"/5</span></div>";
		}else{
			html+="<div class='scoreColor'>New Seller</div>";
		}
		html+="  				<div class='clear'></div>";
		html+="  				<div class='greyText fnt10'><strong>Dispatched in "+val.shippingDays+" business days</strong></div>";
		html+="					<div class='cashBackMoreSellerToolTip'>";	
		html+="   					<div class="+(($('#showInternalCashback').html()=='true' && val.sellingPriceBefIntCashBack != null && val.sellingPriceBefIntCashBack != val.sellingPrice ) ? '':'no-display')+">Selling Price: <span class='float-r_mrgn-40' style='margin-left:7px'><strike>Rs "+val.sellingPriceBefIntCashBack+"</strike></span></div>";
		html+="	  					<div>"+(($('#showInternalCashback').html()=='true' && val.sellingPriceBefIntCashBack != null && val.sellingPriceBefIntCashBack != val.sellingPrice ) ? 'Offer Price:':'Selling Price:' )+" <span class='float-r_mrgn-40 redText bold  "+(($('#showInternalCashback').html()=='true' && val.sellingPriceBefIntCashBack != null ) ? 'mar-14-left':'mar-7-left')+"'>Rs "+val.sellingPrice+"</span></div>";
			if(val.externalCashBackApplicable && val.cashBackValue > 0){
				cashback = val.cashBackValue.toString().split('.');
				if(val.cashBackType == 'PCT'){
					if(cashback[1] > 1){
						html+="      <div style='color:#004B91'><span>Insta Cashback: (-)</span><span class='float-r_mrgn-40' style='margin-left:6px'>"+val.cashBackValue+"%</span></div>";
					}else{
						html+="      <div style='color:#004B91'><span>Insta Cashback: (-)</span><span class='float-r_mrgn-40' style='margin-left:6px'>"+cashback[0]+"%</span></div>";	
					}
				}
				if(val.cashBackType == 'ABS'){
					html+="      	<div style='color:#004B91'><span>Insta Cashback: (-)</span><span class='float-r_mrgn-40' style='margin-left:7px'>Rs. "+cashback[0]+"</span></div>";	
				}
			}
		html+="		</div>";
		
		if( !(val.emiCheck) || $('#emiEnabled').html()=='true' ){
			html+="<div class='mar-5-top mar-5-btm overhid'>Zero EMI Processing Charges</div>";
		}
			if(val.freebies.length > 0){
				html+="<div class='mVTooltipFreebies'>";
				for (i=0;i<val.freebies.length; i++){
					html+="<span style='border-top:"+(i==0 ? 0:'')+"px'>"+val.freebies[i]+"</span>";
				}
				html+="</div>";
			}
		html+=" 			</div>";
		if(val.inventoryText!=undefined){
			html+="  		<div class='mVTooltipInevntory'>"+val.inventoryText+"</div>";
		}
		html+=" 	</div>";
		html+="			<div class='clear'></div>";
		html+="		</div>";
		html+="</div>";
		if(index==2){
			return false;
		}
		return html;
	 });
	 $('#buyMultiVendorBoxCont').html(html);
}

