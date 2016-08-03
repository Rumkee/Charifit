

calorie.runKeeper = {


    //RunKeeperClientID: "",
    //RunKeeperClientSecret: "",
    //RunKeeperAuthorizationURL: "",
    //RunKeeperAccessTokenURL: "",
    //RunKeeperDeAuthURL: "",
    //RunKeeperAPIURL:"",


    //getPastActivityData: function (token,successCallback,errorCallback) {

    //    var thisdata = {
    //        Headers: [
    //                     { Key:"Accept",  Value:"application/vnd.com.runkeeper.FitnessActivityFeed+json"},
    //                     { Key:"Authorization", Value:"Bearer " + token}
    //        ],
    //        URL: calorie.runKeeper.RunKeeperAPIURL + "/fitnessActivities"

    //    };

    //    calorie.useProxy(thisdata, successCallback,errorCallback)           

    //},


    //getSpecificActivityData: function (token, ActivityURL, successCallback, errorCallback) {

    //    var thisdata = {
    //        Headers: [
    //                     { Key: "Accept", Value: "application/vnd.com.runkeeper.FitnessActivity+json" },
    //                     { Key: "Authorization", Value: "Bearer " + token }
    //        ],
    //        URL: calorie.runKeeper.RunKeeperAPIURL + ActivityURL

    //    };

    //    calorie.useProxy(thisdata, successCallback, errorCallback)


    //},

    addPropsToResponse: function (response) {

        response.duration_mins = response.duration / 60.0;
        response.total_distanceFormatted = parseFloat(Math.round(response.total_distance * 100) / 100).toFixed(2);
        response.duration_minsFormatted = parseFloat(Math.round(response.duration_mins * 100) / 100).toFixed(2);
        response.mapId = 'map' + calorie.helpers.getRandomInt(0, 9999).toString();
        response.galleryID = 'gallery' + calorie.helpers.getRandomInt(1, 999);
        response.jsonblob = JSON.stringify(response);

}


}