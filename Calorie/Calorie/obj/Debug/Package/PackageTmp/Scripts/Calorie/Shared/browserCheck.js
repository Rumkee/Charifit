
browserCheck = {

    checkBrowserSupported: function(redurectUrl) {


        if (!Modernizr.fontface || !Modernizr.json || !Modernizr.es5object || !Modernizr.mutationobserver) {
            window.location = redurectUrl;
        }


    },
    
    checkBrowserWarning: function() {

        var browserDoNotWarnSetting = browserCheck.getBrowserDoNotWarnSetting();

        if ((!Modernizr.textshadow || !Modernizr.smil) && (typeof browserDoNotWarnSetting == 'undefined')) {
            return '<div class="alert major-alert alert-warning fade in " role="alert">' +
                '<div class="row">' +
                '<div class="col-xs-12 col-sm-7 col-md-8">' +
                '<text style="line-height:25px">Your browser does not support all the features used by this website.<br>Some aspects of this site may not function as intended, <a href="https://whatbrowser.org/">please consider updating your browser</a></text>' +
                '</div>' +
                '<div class="col-xs-12 col-sm-5 col-md-4">' +
                '<button type="button" class="btn btn-sm btn-success pull-right " onclick="browserCheck.browserWarningIgnore(event)" data-dismiss="alert" aria-label="ok"><span aria-hidden="true">Ignore</span></button>' +
                '<span class="pull-right" style="">&nbsp;</span>' +
                '<input class="pull-right input-sm" id="browserWarningDontWarnAgain" style="margin-top:2px;" data-val="true" value="true" type="checkbox">&nbsp;&nbsp;' +
                '<span class="pull-right" style="font-size:13px; margin-top:7px;">Don\'t remind me again&nbsp;</span>' +
                '</div>' +
                '</div>' +
                '</div>';
        }

        return "";
    },

    browserWarningIgnore: function() {

        var doNotWarnAgain = $("#browserWarningDontWarnAgain").is(':checked');
        if (doNotWarnAgain) {
            browserCheck.setDontWarnAgain();
        }

    },

    setDontWarnAgain: function() {
        Cookies.set('ChariFit-BrowserUpdateDoNotWarn', 'true', { expires: calorie.location.cookieLifeDays });
    },

    getBrowserDoNotWarnSetting: function() {
        return Cookies.get('ChariFit-BrowserUpdateDoNotWarn');
    }


};

//jquery not loaded at this point.
browserCheck.checkBrowserSupported(document.getElementById('browserCheckScript').getAttribute('data-redirect-url'));

