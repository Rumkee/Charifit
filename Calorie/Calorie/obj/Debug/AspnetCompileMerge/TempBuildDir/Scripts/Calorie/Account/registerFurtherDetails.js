

var preferredActivities = new calorie.helpers.serialisableList();
preferredActivities.outputnode = $("#PreferredActivitiesList");
preferredActivities.listname = "PreferredActivities";

function activityClicked(event) {
    setActivity($(event.currentTarget));
}

function setActivity(target, forceChecked) {

    var activityName = target.data("activityname");

    if ((!target.data("checked")) | forceChecked) {
        target.find("#tickbox").fadeIn();
        target.data("checked", true);
        preferredActivities.add(activityName);

    } else {
        target.find("#tickbox").fadeOut();
        target.data("checked", false);
        preferredActivities.remove(activityName);
    }

}
