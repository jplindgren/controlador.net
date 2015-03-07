; (function () {
    $.blockUI.defaults.css.border = 'none';
    $.blockUI.defaults.css.padding = '15px';
    $.blockUI.defaults.css.top = '30px';
    $.blockUI.defaults.css.backgroundColor = '#000';
    $.blockUI.defaults.css["-webkit-border-radius"] = '10px';
    $.blockUI.defaults.css["-moz-border-radius"] = '10px';
    $.blockUI.defaults.css.opacity = '.5';
    $.blockUI.defaults.css.color = '#fff';
    $.blockUI.defaults.css.baseZ = 9999

    //$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $(window).resize(function () {
        ellipses1 = $("#bc1 :nth-child(2)")
        if ($("#bc1 a:hidden").length > 0) { ellipses1.show() } else { ellipses1.hide() }
    });

    jQuery.browser = {};    
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();