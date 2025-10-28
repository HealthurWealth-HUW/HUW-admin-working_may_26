$(function () {

    $("#content").validate({
        rules: {
            txbFirstName: {
                required: true,
                minlength: 3
            },
            txbEmailId: {
                required: true
            },
            txbMobileNumber: {
                required: true,
                number: true,
                minlength: 6
            },
//            email: {
//                required: true,
//                email: true
//            },
            message: {
                required: true
            }
        },
        messages: {
            txbFirstName: {
                required: 'This field is required',
                minlength: 'Minimum length: 3'
            },
            txbEmailId: {
                required: 'This field is required'
            },
            txbMobileNumber: {
                required: 'This field is required',
                number: 'Invalid phone number',
                minlength: 'Minimum length: 6'
            },
//            email: 'Invalid e-mail address',
//            message: {
//                required: 'This field is required'
//            }
       },
        success: function (label) {
            label.html('OK').removeClass('error').addClass('ok');
            setTimeout(function () {
                label.fadeOut(500);
            }, 2000)
        }
    });

});