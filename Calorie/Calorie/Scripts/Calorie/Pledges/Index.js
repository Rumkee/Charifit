


function myPledgesBtnClick() {
    $("#myPledgesBody").slideToggle(null, calorie.helpers.bigTextGlobalScan());
    $("#myPledgesMoreBtn").toggle();
    $("#myPledgesLessBtn").toggle();

}

function myBookmarkBtnClick() {
    $("#myBookmarkBody").slideToggle();
    $("#myBookmarkMoreBtn").toggle();
    $("#myBookmarkLessBtn").toggle();
    calorie.helpers.bigTextGlobalScan();
}

function doFilter(thisType, thisCount) {

    var resultDiv = $("#pledgeFilterResults");
    resultDiv.empty();

    $.ajax({
        type: "POST",
        url: filterURL,
        data: { count: thisCount, type: thisType },
        dataType: 'html',
        success: function (result) {
            resultDiv.append(result);
            calorie.filter.filterEnable();
            calorie.currency.globalCurrencyConvertScan();
        },
        error: function () {
            calorie.filter.filterEnable();
        }
    });

}