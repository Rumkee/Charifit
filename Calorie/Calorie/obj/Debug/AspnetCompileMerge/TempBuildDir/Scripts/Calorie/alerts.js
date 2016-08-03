

calorie.alerts = {

    deleteURL: "",

    populateAlerts: function (getURL,jNodeAlerts,doneCallback) {

        $.ajax({
            type: "GET",
            url: getURL,
            //data: ,
            dataType: 'json',
            contentType: false,
            processData: false,
            cache: false,
            success: function (response) {

                var source = $("#alert-template").html();
                var alerttemplate = Handlebars.compile(source);

                response.forEach(function(alertjson) {

                    var html = alerttemplate(alertjson);
                    jNodeAlerts.append(html);

                });
                doneCallback();
            },
            
            error: function () {}

        });
        
    },

    dismissAlert: function (id) {
        
        var formData = new FormData();
        formData.append('_AlertID',id);

        $.ajax({
            type: 'POST',
            url: calorie.alerts.deleteURL,
            data: formData,
        dataType: 'html',
        contentType: false,
        processData: false,
       

    });


    }
    
}

$(function() {

    $('.alert').on('closed.bs.alert', function(event) {
            var id = $(event.currentTarget).data('alertid');
            calorie.alerts.dismissAlert(id);

        }
    );
});

