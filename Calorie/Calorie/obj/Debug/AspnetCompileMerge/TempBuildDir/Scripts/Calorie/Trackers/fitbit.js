

calorie.fitbit = {
    
    fitBitURL: "",
    fitBitClientID: "",
    trackerGetAccessCodeURL:"",

    getAccessCode: function (completeFunction,errorFunction) {

        postURL = calorie.fitbit.trackerGetAccessCodeURL + "?TrackerType=Fitbit";

        $.ajax({
            url: postURL,
            dataType: 'text',
            async: true,
            success: function (result) {
                completeFunction(result);
            },

            error: function (err) {
                errorFunction(err);
            }
        }
        );

        
        
    },

    addPropsToResponse: function (response) {

        response.distance_formatted = calorie.helpers.toTwoDP(response.distance);
        response.duration_mins_formatted = calorie.helpers.toTwoDP(response.duration / 60000.00);       
        response.jsonblob = JSON.stringify(response);

    }


}