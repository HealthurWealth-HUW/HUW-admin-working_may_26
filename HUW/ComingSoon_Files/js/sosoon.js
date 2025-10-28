/* ==================================================
 * sosoon.js
 * v1.0 9/26/2013
 ================================================== */

$(function () {
    "use strict";

    /* Count Down */
    var endDate = "Oct 31, 2013 18:35:00"; /* Change End Date Here */

    var cdHTML = "";
    $('.countdown').countdown({
        date: endDate,
        render: function (data) {
            cdHTML = ''; /* Code not optomized here for readability */
            cdHTML = cdHTML + '<div class="forth color-1"><div class="label">DAYS</div><div id="days">' + this.leadingZeros(data.days) + '</div></div>';
            cdHTML = cdHTML + '<div class="forth color-2"><div class="label">HOURS</div><div id="hours">' + this.leadingZeros(data.hours, 2) + '</div></div>';
            cdHTML = cdHTML + '<div class="forth color-3"><div class="label">MINUTES</div><div id="minutes">' + this.leadingZeros(data.min, 2) + '</div></div>';
            cdHTML = cdHTML + '<div class="forth color-4"><div class="label">SECONDS</div><div id="seconds">' + this.leadingZeros(data.sec, 2) + '</div></div>';
            $(this.el).html(cdHTML);
        },
        onEnd: function () {
            $('.countdown').addClass('hidecountdown');
            $('.endcountdown').addClass('ended');
        }
    });

    /* Contact form */
    $('#cform').submit(function () {
        var action = $(this).attr('action');
        $("#message").slideUp(700, function () {
            $('#message').hide();
            $('#submit').attr('disabled', 'disabled');
            $.post(action, {
                name: $('#name').val(),
                email: $('#email').val(),
                comments: $('#comments').val()
            }, function (data) {
                $('#message').html(data);
                $('#message').slideDown('slow');
                $('#submit').removeAttr('disabled');
                if (data.match('success') != null) $('#cform').slideUp('slow');
            });
        });
        return false;
    });

    /* Subscribe Form */
    $('#subscribeform').submit(function () {
        var action = $(this).attr('action');
        $("#msg").slideUp(700, function () {
            $('#msg').hide();
            $('#subsubmit').attr('disabled', 'disabled');
            $.post(action, {
                email: $('#subemail').val()
            }, function (data) {
                $('#msg').html(data);
                $('#msg').slideDown('slow');
                $('#subsubmit').removeAttr('disabled');
                if (data.match('success') != null) $('#subscribeform').slideUp('slow');
            });
        });
        return false;
    });

});

/* End of Documents */
