
var CreateScript = $('#CreateScript');
var activityUnits = CreateScript.data('activity-units');


function AddOffset(parent, thisBlob, thisIdent, meters, secconds, calories, dataSource) {

    parent.find(".btn").hide();
    $("#offSetPanelBody").empty();
    $("#offSetPanelBody").append(parent).slideDown();

    //close the selectors
    $('#RunKeeperSelector').modal('hide');
    $('#FitbitSelector').modal('hide');

    var offsetAmount = offSetsCreate.getoffSetAmount(meters, secconds, calories);

    $('#offSetPanelBodyMessage').empty().append(offSetsCreate.getActivityDescription(meters, secconds, calories));

    $('#CreateOffsetBtn').removeClass("disabled");
    $('#OffsetAmount').val(offsetAmount);
    $('#BlobType').val(dataSource);
    $('#JSONBlob').val(JSON.stringify(thisBlob));
    $('#ThirdPartyIdentifier').val(thisIdent);
}

var offSetsCreate = {

    activityRequiredUnits: activityUnits,

    getActivityDescription: function (meters, seconds, calories) {

        switch (this.activityRequiredUnits) {
            case 'Sessions':
                return "This activity will add 1 session to the activities logged for this pledge";
            case 'Hours':

                var hours = ((seconds / 60.00) / 60.00);
                return "This activity will add " + calorie.helpers.toTwoDP(hours) + " hours to the activities logged for this pledge";
            case 'Minutes':
                var mins = (seconds / 60.00);
                return "This activity will add " + calorie.helpers.toTwoDP(mins) + " minutes to the activities logged for this pledge";
            case 'Meters':
                return "This activity will add " + calorie.helpers.toTwoDP(meters) + " meters to the activities logged for this pledge";
            case 'Kilometers':
                var kilos = (meters / 1000.00);
                return "This activity will add " + calorie.helpers.toTwoDP(kilos) + " kilometers to the activities logged for this pledge";
            case 'Miles':
                var miles = calorie.helpers.conversions.metersToMiles(meters);
                return "This activity will add " + calorie.helpers.toTwoDP(miles) + " kilometers to the activities logged for this pledge";
            case 'Calories':
                return "This activity will add " + calories + " calories to the activities logged for this pledge";
        };
        return '';
    },

    getoffSetAmount: function (meters, seconds, calories) {

        switch (this.activityRequiredUnits) {
            case 'Sessions':
                return 1;
            case 'Hours':

                var hours = ((seconds / 60.00) / 60.00);
                return calorie.helpers.toTwoDP(hours);
            case 'Minutes':
                var mins = (seconds / 60.00);
                return calorie.helpers.toTwoDP(mins);
            case 'Meters':
                return calorie.helpers.toTwoDP(meters);
            case 'Kilometers':
                var kilos = (meters / 1000.00);
                return calorie.helpers.toTwoDP(kilos);
            case 'Miles':
                var miles = calorie.helpers.conversions.metersToMiles(meters);
                return calorie.helpers.toTwoDP(miles);
            case 'Calories':
                return calories;
        }
        return 0;
    }

}