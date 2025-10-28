$.fn.tabs = function () {
    var selector = this;

    this.each(function () {
        var obj = $(this);
        var ul = $(this).closest("div.product-collateral ul.product-tabs li");
        $(obj.attr('href')).hide();

        $(obj).click(function () {
            $(ul).removeClass('active');
            $( ul).addClass('active');
            $(selector).removeClass('selected');
         
            $(selector).each(function (i, element) {

                $($(element).attr('href')).hide();
               
            });

            $(this).addClass('selected');

            $($(this).attr('href')).fadeIn();

            return false;
        });
    });

    $(this).show();

    $(this).first().click();
};