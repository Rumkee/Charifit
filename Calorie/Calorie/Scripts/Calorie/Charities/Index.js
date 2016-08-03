
function doFilter(filterType, filterCount) {

    $.ajax({
        type: "POST",
        url: $('#indexScript').data('filter-url'),
        data: { count: filterCount, type: filterType },
        dataType: 'html',
        success: function (result) {
            var charitiesResult = $('#charitiesResults');
            charitiesResult.empty();
            $('#charitiesResults').append(result);
            calorie.filter.filterEnable();
        },
        error: function () {
            calorie.filter.filterEnable();
        }
    });

}