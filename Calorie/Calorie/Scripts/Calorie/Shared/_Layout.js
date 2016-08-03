
//google analytics
(function (i, s, o, g, r, a, m) {
    i['GoogleAnalyticsObject'] = r;
    i[r] = i[r] || function () {
        (i[r].q = i[r].q || []).push(arguments)
    }, i[r].l = 1 * new Date();
    a = s.createElement(o),
        m = s.getElementsByTagName(o)[0];
    a.async = 1;
    a.src = g;
    m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

ga('create', 'UA-73422334-1', 'auto');
ga('send', 'pageview');


//facebook
window.fbAsyncInit = function () {
    FB.init({
        appId: '@ConfigurationManager.AppSettings["FacebookAppID"]',
        xfbml: true,
        version: 'v2.5'
    });
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));


//browser warning
$(function() {
    $("#majorAlerts").append(browserCheck.checkBrowserWarning());
});


$(function() {

    //navbar currency selector
    var anonUserCurrencySelectList = $('#anonUserCurrencySelectList');

    calorie.location.findLocation(function (location) {
        anonUserCurrencySelectList.val(calorie.currency.getNearestSupportedCountryCodeFromCountryCode(location));
        $("#anonUserCurrencyOptionGroup").fadeIn();
    });
    
    anonUserCurrencySelectList.change(function () {
        var code = $(this).val();
        calorie.location.setCountry(code);
        calorie.currency.globalCurrencyConvertScan();
        calorie.helpers.date.dateGlobalScan();
        var event = new CustomEvent('currencyChanged', { 'detail': code });
        document.dispatchEvent(event);
    });
    
});