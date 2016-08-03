

calorie.helpers = {

    date: {

        dateGlobalScan: function () {
            
            calorie.location.findLocation(function (thisLocation) {

                var localeCode = calorie.currency.getLocaleCodeFromCountryCode(thisLocation);
                
                var dateTexts = $(":root").find("[data-date!=''][data-date]");
                dateTexts.each(function (idx, elem) {
                    calorie.helpers.date.convertToLocalDate(elem, localeCode);
                });

            });

        },

        convertToLocalDate: function(elem,dateLocale) {
            var dtStr = $(elem).data("date");
            var date = new Date(dtStr);
            $(elem).html(date.toLocaleDateString(dateLocale));
        }
        
    },
    


    bigTextGlobalScan: function () {
      
        var bigTexts = $(":root").find(".bigtext");
        bigTexts.each(function(idx, elem) {
            $(elem).bigtext({ maxfontsize: 35, minfontsize: 10 }).resize();
        });

    },

    bigtext: function(elem) {
        elem.bigtext({maxfontsize: 35,minfontsize:10 });
    },

    fancyFadeInTable: function(target) {
        var rows = target.find('tr');
        rows.each(function (i, row) {            
            $(row).fadeTo(0,0);
        });

        rows.each(function (i, row) {
            setTimeout(function () {                
                 $(row).fadeTo(750,1);            
            }, 150 * i);
        });
    },
    

    reApplyTableStriping: function(){
        $("tr:not(.hidden)").each(function (index) {
            $(this).toggleClass("stripe", !!(index & 1));
        });
    },
    

    hideButtonsAndFullDetails: function (target) {
        target.find(".btn").hide();
        target.find(".fulldetails").hide();
    },

    conversions: {

        metersToMiles: function(meters){
            return meters * 0.000621371;
        }

    },

    currency: {

        DisplayAmountFromBaseURL: "",

        toLocalCurrency: function (amount, complete, error) {

            calorie.location.findLocation(function(outputCode) {

                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'html',
                    type: 'POST',
                    data: JSON.stringify({ 'BaseCurrencyInputAmount': amount, 'OutputCode': outputCode }),
                    url: calorie.helpers.currency.DisplayAmountFromBaseURL,
                    success: function(result) {
                        complete(result);
                    }
                }).error(function(er) {
                    error(er);
                });

            });

        }
    },

    toTwoDP: function(input){
        return parseFloat(Math.round(input * 100) / 100).toFixed(2);
    },

    getRandomInt: function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    },

    generatePassword: function() {
        var length = 20,
        charset = "!£$%^&*(){}[]~@:#?><.,abcdefghijklnopqrsPQRSTUVWXYZ0123456789",
        retVal = "";
        for (var i = 0, n = charset.length; i < length; ++i) {
            retVal += charset.charAt(Math.floor(Math.random() * n));
            }
            return retVal;
    },


    addMap: function (mapid,latLongArray){

        var geojson = [
            {
                "type": "Feature",
                "geometry": {
                "type": "LineString",
                "coordinates": []
            },
            "properties": {
                    "stroke": "#fc4353",
                    "stroke-width": 5
                    }
            }
        ];

        var points = [];
        latLongArray.forEach(function (itm) {
            points.push(L.latLng(itm.latitude, itm.longitude));
            geojson[0].geometry.coordinates.push([itm.longitude, itm.latitude]);
        });

        L.mapbox.accessToken = 'pk.eyJ1IjoiamFtZXNydW1rZWUiLCJhIjoiY2lnOThnZ3k4MGFpNXU0a2piZ3R5aGpkZCJ9.8C2kJEj7RjDNXSitcNJoKQ';
        var map = L.mapbox.map(mapid, 'mapbox.streets');

        L.geoJson(geojson, { style: L.mapbox.simplestyle.style }).addTo(map);

        map.fitBounds(L.latLngBounds(points));

    },

    spinnerHTML: '<span class="spinner"><i class="fa fa-circle-o-notch fa-spin-2x calorie-green-color"></i></span>',
    spinnerInnerHTML: '<i class="fa fa-circle-o-notch fa-spin-2x calorie-green-color">',

    serialisableList: function() {

        this.outputnode = null;
        this.listname = "";
        this.list = [];
        
        this.add= function(elem){
            if (this.list.indexOf(elem)<0){
                this.list.push(elem);
                this.render();
            }  
        },
        
        this.remove= function(elem) {
            var idx = this.list.indexOf(elem);
            if (idx>-1) {
                this.list.splice(idx, 1);
                this.render();
            }   
        },
        
        this.render=function(){
            this.outputnode.empty();
            for (i = 0; i <= this.list.length - 1; i++) {
                this.outputnode.append('<input name="' + this.listname + '[' + i + ']" type="hidden" value="' + this.list[i] + '">');
            }
            
        }
    
    },

    buttonDisabler: function () {

        this.button = null;
        this.wasAlreadyDisabled = false;
        this.replacedHtml = null;

        this.disable = function () {

            this.replacedHtml = this.button.children('span.glyphicon').replaceWith(calorie.helpers.spinnerHTML);

            if (this.replacedHtml.length === 0) {
                this.replacedHtml = this.button.find('i.fa').first().replaceWith(calorie.helpers.spinnerHTML);
            }
            if (this.replacedHtml === null) {
                this.button.append(calorie.helpers.spinnerHTML);
            }
                        

            if (!this.button.hasClass("disabled")) {
                this.button.addClass("disabled");
                this.wasAlreadyDisabled = false;
            } else {
                this.wasAlreadyDisabled = true;
            }
            

        }

        this.enable = function () {

            if (this.replacedHtml === null) {
                this.button.children('.spinner').remove();
            } else {
                this.button.find('span.spinner').replaceWith(this.replacedHtml);
            }

            
            if (this.wasAlreadyDisabled === false) {
                this.button.removeClass("disabled");
            }
            

        }

    },

    checkboxController: function (real,substitute) {

    this.real = real;
    this.substitute = substitute;
    //var _this = this;
        
    this.real.hide();

                
    if(this.real.is(':checked')){
            this.substitute.show();
    }else{
            this.substitute.hide();
    }
        
    this.substitute.parent().on("click",
            $.proxy(function () {
                this.real.prop("checked", !this.real.is(':checked'));
                this.real.trigger('change');
                this.substitute.toggle();
            }, this)
        );

        
   


}





}