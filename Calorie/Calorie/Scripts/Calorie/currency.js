

calorie.currency = {

    currencyHTMLWithoutTip: '<span>{{amount}}</span>',

    globalCurrencyConvertScan: function (){
        var currencyTexts = $(":root").find("[data-currencycode!=''][data-currencycode]");
        currencyTexts.each(function(idx, elem) {
            calorie.currency.checkConvertAmount($(elem));
        });
    },

    checkConvertAmount: function (node) {

        //var num = node.text();
        var num = node.data("currencyamount");
        

        var baseCurrenyCode = node.data('currencycode');
        if ($.isNumeric(num)) {

            node.empty();
            node.append(calorie.helpers.spinnerHTML);

            calorie.location.findLocation(function(thisLocation) {


                var localCurrencyCode = calorie.currency.getCurrencyCodeFromCountryCode(thisLocation);
                var localCurrencyPrefix = calorie.currency.getCurrencyPrefix(localCurrencyCode);
                var localCurrencyLongDescription = calorie.currency.getCurrencyLongDescription(localCurrencyCode);

                if (localCurrencyCode != baseCurrenyCode) {
                    num = calorie.currency.convertToLocal(num, baseCurrenyCode, thisLocation);
                    localCurrencyLongDescription += "<BR>Amount is an approximation";

                }

                if (localCurrencyPrefix === "€") {
                    num = accounting.formatMoney(num, localCurrencyPrefix, 2, ".", ",");
                } else {
                    num = accounting.formatMoney(num, localCurrencyPrefix, 2, ",", ".");
                }

                var innerHTML = calorie.currency.currencyHTMLWithoutTip.replace("{{amount}}", num);
                innerHTML = innerHTML.replace("{{tooltip}}", localCurrencyLongDescription);

                node.empty();
                node.append(innerHTML);
                
                var callback = node.data('callback');

                var x = eval(callback);
                if (typeof x == 'function') {
                    x();
                }

                return;

            });

        }
        
    },

    ConvertAmountDirect: function (num,baseCurrenyCode,thisLocation,numberOnly) {

        if ($.isNumeric(num)) {

            var localCurrencyCode = calorie.currency.getCurrencyCodeFromCountryCode(thisLocation);
            var localCurrencyPrefix = calorie.currency.getCurrencyPrefix(localCurrencyCode);
                

                if (localCurrencyCode !== baseCurrenyCode) {
                    num = calorie.currency.convertToLocal(num, baseCurrenyCode, thisLocation);

                }

                if (numberOnly) {
                    return num;
                }

                if (localCurrencyPrefix === "€") {
                    num = accounting.formatMoney(num, localCurrencyPrefix, 2, ".", ",");
                } else {
                    num = accounting.formatMoney(num, localCurrencyPrefix, 2, ",", ".");
                }

        }
        return num;

    },

    convertToLocal: function (amount, fromCurrencyCode, local) {

        var baseCoefficient = calorie.currency.getConvertCoefficient( fromCurrencyCode);
        var baseAmount = amount / baseCoefficient;

        var toCurrencyCode = calorie.currency.getCurrencyCodeFromCountryCode(local);
        return baseAmount * calorie.currency.getConvertCoefficient( toCurrencyCode);

    },

    getConvertCoefficient: function (currencyCode) {

        switch (currencyCode) {
            case "AED":
                return 5.57;
            case "AUD":
                return 2.18;
            case "CAD":
                return 2.04;
            case "EUR":
                return 1.35;
            case "HKD":
                return 11.76;
            case "SGD":
                return 2.17;
            case "USD":
                return 1.52;
            case "GBP":
            default:
                return 1.00;
        }

    },

    getCurrencyCodeFromCountryCode: function (countryCode) {

        switch (countryCode) {

            case "AT":
            case "BE":
            case "CY":
            case "EE":
            case "FI":
            case "FR":
            case "DE":
            case "GL":
            case "IE":
            case "IT":
            case "LV":
            case "LT":
            case "MT":
            case "NL":
            case "PT":
            case "ES":
            case "SI":
            case "SK":
            case "ME":
                return "EUR";

            case "AE":
                return "AED";

            case "AU":
                return "AUD";

            case "CA":
                return "CAD";

            case "GB":
                return "GBP";

            case "HK":
                return "HKD";

            case "SG":
                return "SGD";

            default:
                return "USD";

        }
    },

    getLocaleCodeFromCountryCode: function (countryCode) {

        switch (countryCode) {

            case "AT":
            case "BE":
            case "CY":
            case "EE":
            case "FI":
            case "FR":
            case "DE":
            case "GL":
            case "IE":
            case "IT":
            case "LV":
            case "LT":
            case "MT":
            case "NL":
            case "PT":
            case "ES":
            case "SI":
            case "SK":
            case "ME":
                return "de-de";

            case "AE":
                return "ar-ae";

            case "AU":
                return "en-au";

            case "CA":
                return "fr-ca";

            case "GB":
                return "en-gb";

            case "HK":
                return "zh-hk";

            case "SG":
                return "zh-sg";

            default:
                return "en-us";

        }
    },

    getNearestSupportedCountryCodeFromCountryCode: function (countryCode) {

        switch (countryCode) {

            case "AT":
            case "BE":
            case "CY":
            case "EE":
            case "FI":
            case "FR":
            case "DE":
            case "GL":
            case "IE":
            case "IT":
            case "LV":
            case "LT":
            case "MT":
            case "NL":
            case "PT":
            case "ES":
            case "SI":
            case "SK":
            case "ME":
                return "DE";

            case "AE":
                return "AE";

            case "AU":
                return "AU";

            case "CA":
                return "CA";

            case "GB":
                return "GB";

            case "HK":
                return "HK";

            case "SG":
                return "SG";

            default:
                return "US";

        }
    },

    getCurrencyPrefix: function (currencyCode) {

        switch (currencyCode) {
            case "AED":
                return "د.إ.";
            case "AUD":
                return "$";
            case "CAD":
                return "$";
            case "EUR":
                return "€";
            case "HKD":
                return "$";
            case "SGD":
                return "$";
            case "USD":
                return "$";
            case "GBP":
            default:
                return "£";

        }

    },
    getCurrencyLongDescription: function (currencyCode) {

        switch (currencyCode) {
            case "AED":
                return "United Arab Emirates Dirhams";
            case "AUD":
                return "Australian Dollars";
            case "CAD":
                return "Canadian Dollars";
            case "EUR":
                return "Euros";
            case "HKD":
                return "Hong Kong Dollars";
            case "SGD":
                return "Singapore Dollars";
            case "USD":
                return "US Dollars";
            case "GBP":
            default:
                return "British Pounds";

        }
        
    }

}



