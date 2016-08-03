//Activites script

var Activities = new calorie.helpers.serialisableList();
Activities.outputnode = $("#ActivitiesList");
Activities.listname = "Activities";

function ActivityClick(event) {
    setActivity($(event.currentTarget));
}

function setActivity(target, forceChecked) {

    var activityName = target.data("activityname");

    if (target.hasClass("disabled")) {
        return;
    }

    if ((!target.data("checked")) | forceChecked) {

        target.find("#tickbox").show();
        target.data("checked", true);
        Activities.add(activityName);

    } else {

        target.find("#tickbox").hide();
        target.data("checked", false);
        Activities.remove(activityName);
    }

}



//expiry date
function ExpiryDateChanged(event) {

    var newDate = new Date(event.target.value);
    var currentDate = new Date(thisExpiry);
    if (newDate < currentDate) {
        $("#expiryDateEarlyValidationMsg").fadeIn();
        $("#saveBtn").addClass("disabled");
    } else {
        $("#expiryDateEarlyValidationMsg").fadeOut();
        $("#saveBtn").removeClass("disabled");
    }
}