

//Timezone stuff
var thisUTCDateTime = new Date(Date.UTC(j_year, j_javascriptmonth, j_day));
                            
$('#tzSelect').on('change', function() {
    setTimeZone(this.value);
});


var clock = $('.countdownclock').FlipClock(j_secconds, {
    clockFace: 'DailyCounter',
    countdown: true
});

populateTimeZones();

function setTimeZone(localeName) {
    setDefaultLocaleName(localeName);
    fillDateTimeField();
    document.addEventListener("currencyChanged", function(e) {
        fillDateTimeField();
    });

}

function fillDateTimeField() {

    calorie.location.findLocation(function(thisLocation) {

        var dateLocaleCode = calorie.currency.getLocaleCodeFromCountryCode(thisLocation);
        moment.locale(dateLocaleCode);

        var localDateTimeString = moment(thisUTCDateTime).tz(getDefaultLocaleName()).format("LLL z");

        $("#localTime").hide();
        $("#localTime").html(localDateTimeString).fadeIn();

    });

}

function populateTimeZones() {
    var tzSelect = $("#tzSelect");
    moment.tz.names().forEach(function(name) {
        tzSelect.append('<option val="' + name + '">' + name + '</option>');
    });

    var defaultLocale = getDefaultLocaleName();
    tzSelect.val(defaultLocale);
    setTimeZone(defaultLocale);

}

function getDefaultLocaleName() {

    var cookieLocale = Cookies.get(LocaleCookieName());

    if (typeof cookieLocale !== 'undefined') {
        return cookieLocale;
    } else {

        var def = moment.tz.guess();
        setDefaultLocaleName(def);
        return def;
    }

}

function setDefaultLocaleName(localeName) {
    Cookies.set(LocaleCookieName(), localeName, { expires: 9999 });
}

function LocaleCookieName() {
    return "ChariFit-Locale";
}



//lightbox
$(function () {
    $('#Gallery').magnificPopup({
        delegate: 'a',
        type: 'image',
        gallery: {
            enabled: true
        }
    });
})

