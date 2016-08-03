
function doFilter(filterType, filterCount) {

    $.ajax({
        type: "POST",
        url: $('#indexScript').data('filter-url'),
        data: { count: filterCount, type: filterType },
        dataType: 'html',
        success: function (result) {

            $('#filterResults').empty();
            $('#filterResults').append(result);
            calorie.currency.globalCurrencyConvertScan();

            calorie.filter.filterEnable();
        },
        error: function () {
            calorie.filter.filterEnable();
        }
    });
}