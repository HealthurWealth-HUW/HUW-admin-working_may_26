

//This function is called automatically upon page load
$(document).ready(function() 
{	
	$("#vpb_pop_up_background").click(function() {
	    $("#notifyme").hide();
		$("ba").hide(); 
		$("#vpb_login_pop_up_box").hide();
		$("#vpb_pop_up_background").fadeOut("slow");
	});
});



//This function displays the login box when called
function vpb_show_login_box()
{

	$("#vpb_pop_up_background").css({
		"opacity": "0.6"
	});
	$("#vpb_pop_up_background").fadeIn("slow");
	$("#vpb_login_pop_up_box").fadeIn('fast');

	window.scroll(0, 0);
}
function vpb_show_Comment_box() {

    $("#vpb_pop_up_backgroundcomment").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_backgroundcomment").fadeIn("slow");
    $("#vpb_login_pop_up_boxcomment").fadeIn('fast');

    window.scroll(0, 0);
}
function vpb_show_login_box2() {

    $("#vpb_pop_up_background2").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_background2").fadeIn("slow");
    $("#vpb_login_pop_up_box2").fadeIn('fast');

    window.scroll(0, 0);
}

function vpb_show_login_box_edit(OrderID,shipmentID,PickUpID) {

    $('#txtShipmentID_edit').val(shipmentID);
    $('#txtPickUPID_edit').val(PickUpID);
    $('#hdnOrderID_edit').val(OrderID);
    $("#vpb_pop_up_background").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_background").fadeIn("slow");
    $("#vpb_login_pop_up_box_edit").fadeIn('fast');

    window.scroll(0, 0);
}

function vpb_show_login_box1() {

    $("#vpb_pop_up_background1").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_background1").fadeIn("slow");
    $("#vpb_login_pop_up_box1").fadeIn('fast');

    window.scroll(0, 0);
}
function closePopup() {
    $("#vpb_pop_up_background").fadeOut("slow");
    $("#vpb_login_pop_up_box").fadeOut('fast');
}
function MycartPopup() {
    $("#vpb_pop_up_background").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_background").fadeIn("slow");
    $("#vpb_login_pop_up_box").fadeIn('fast');

    window.scroll(0, 0);
}
function NotifyMe() {
    $("#vpb_pop_up_background").css({
        "opacity": "0.6"
    });
    $("#vpb_pop_up_background").fadeIn("slow");
    $("#notifyme").fadeIn('fast');

    window.scroll(0, 0);
}



function vpb_hide_popup_boxes() {
    $("#notifyme").hide();
    $("#loginbox").hide();
    $("#vpb_signup_pop_up_box, #vpb_signup_pop_up_box1").hide();
    $("#vpb_login_pop_up_box, #vpb_login_pop_up_box1,#vpb_login_pop_up_box2, #vpb_login_pop_up_box_edit,#vpb_login_pop_up_boxcomment").hide();
    $("#vpb_pop_up_background, #vpb_pop_up_background1,#vpb_pop_up_background2").fadeOut("slow");
	//$('#tabs a').tabs();
}