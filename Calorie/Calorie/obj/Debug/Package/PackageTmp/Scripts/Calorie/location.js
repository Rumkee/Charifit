

calorie.location= {
    
    cookieName: 'ChariFit-CountryCode',   
    cookieLifeDays: 1,//0.00069,
    queue: [],
    active: false,

    checkForOverride: function (currentCode) {
                
        var cookieCurrencyCode = calorie.currency.getCurrencyCodeFromCountryCode(currentCode)
        var uc = calorie.user.usercurrency();
        if (uc !== cookieCurrencyCode) {

            switch (uc) {
                case "EUR":
                    calorie.location.setCountry("DE");
                    break;
                case "AED":
                    calorie.location.setCountry("AE");
                    break;
                case "AUD":
                    calorie.location.setCountry("AU");
                    break;
                case "CAD":
                    calorie.location.setCountry("CA");
                    break;
                case "GBP":
                    calorie.location.setCountry("GB");
                    break;
                case "HKD":
                    calorie.location.setCountry("HK");
                    break;
                case "SGD":
                    calorie.location.setCountry("SG");
                    break;
                case "USD":
                    calorie.location.setCountry("US");
                    break;

            }

            return calorie.location.getCountry();
    
        }

        return currentCode;
      

    },

    setCountry: function(countryCode) {
        Cookies.set(calorie.location.cookieName, countryCode, { expires: calorie.location.cookieLifeDays });
    },
    getCountry: function() {
        return Cookies.get(calorie.location.cookieName);
    },
    

    findLocation: function (callback) {
        
        
        if (calorie.location.active === true) {
            calorie.location.queue.push(callback);
            return;
        }
        

        var CookieCode = calorie.location.getCountry();
        if (typeof CookieCode !== 'undefined') {

            callback(calorie.location.checkForOverride(CookieCode));
        } else {
                      
            calorie.location.active = true;
            $.ajax({
                url: "http://ipinfo.io/json",
                dataType: 'json',
                async: true,
                success: function (location) {
                    calorie.location.setCountry(location.country);
                    callback(calorie.location.checkForOverride(location.country));

                    calorie.location.queue.forEach(function(item) { item(location.country) });
                    calorie.location.active = false;
                },

                error: function () {
                    calorie.location.setCountry("US");
                    callback(calorie.location.checkForOverride("US"));
                    calorie.location.queue.forEach(function(item) { item("US") });
                    calorie.location.active = false;
                }   
            }
            );
            
        }

    }
           
}
