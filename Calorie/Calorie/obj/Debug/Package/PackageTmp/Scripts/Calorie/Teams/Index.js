

function doFilter(_type, _count) {

    $.ajax({
        type: "POST",
        url: teamFilterURL,
        data: { count: _count, type: _type },
        dataType: 'html',
        success: function (result) {

            var teamResult = $("#teamResults");
            teamResult.empty();
            teamResult.append(result);
            calorie.filter.filterEnable();
        },
        error: function () {
            calorie.filter.filterEnable();
        }
    });

}
