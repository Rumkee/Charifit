

var cookieWarning = {

    notOKURL: 'http://zapatopi.net/afdb/',
    policyURL: '',
    cookieName: 'ChariFit-UseCookieWarning',


    check: function() {

        if (typeof Cookies.get(cookieWarning.cookieName) !== 'undefined') {
            //already accepted. do nothing
        } else {
            //not accepted or expired.
            $("#majorAlerts").append($(
                '<div class="alert major-alert alert-warning " style="display:none;" role="alert">' +
                '<div class="row">' +
                '<div class="col-xs-12 col-sm-9">' +
                '<text style="line-height:25px">We use cookies in order to provide the best experience on this website, by continuing you are agreeing to <a href="' + cookieWarning.policyURL + '">the cookie policy.</a></text>' +
                '</div>' +
                '<div class="col-xs-12 col-sm-3">' +
                '<a type="button" class="btn btn-sm btn-danger pull-right"  href="' + cookieWarning.notOKURL + '" aria-label="NOT OK"><span aria-hidden="true">NOT OK</span></a>' +
                '<text class="pull-right">&nbsp;</text>' +
                '<button type="button" class="btn btn-sm btn-success pull-right " onclick="cookieWarning.cookieAcceptEvent()" data-dismiss="alert" aria-label="ok"><span aria-hidden="true">OK</span></button>' +
                '</div>' +
                '</div>' +
                '</div>').fadeIn()
            );

        };
        
    },

    cookieAcceptEvent: function() {
        Cookies.set(cookieWarning.cookieName, 'Accepted', { expires: 9999 });
    }
    
}

$(function() {

    cookieWarning.policyURL = $('#cookieWarningScript').data('cookie-policy-url');
    cookieWarning.check();

});

