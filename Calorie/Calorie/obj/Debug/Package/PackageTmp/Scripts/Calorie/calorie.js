
$(function () {

    //global stuff here


    //shim for date picker in internet explorer
    webshims.setOptions('forms-ext', { types: 'date' });
    webshims.polyfill('forms forms-ext');
    
    //globally configure the help controls
    var questionbuttons = $(":root").find("[data-help-button-id!=''][data-help-button-id]");
    var questionalerts = $(":root").find("[data-help-alert-id!=''][data-help-alert-id]");
    questionbuttons.click(function(evnt) {
        id = $(evnt.currentTarget).data("help-button-id");
        questionalerts.closest("[data-help-alert-id='" + id + "']").slideToggle();
    });
    

    //trigger all global conversions
    calorie.currency.globalCurrencyConvertScan();
    calorie.helpers.date.dateGlobalScan();
    

    //switch on tool tips
    var tooltips = $('[data-toggle="tooltip"]');
    if (tooltips.length > 0) {
        tooltips.tooltip();
    }

});

var calorie = {};
