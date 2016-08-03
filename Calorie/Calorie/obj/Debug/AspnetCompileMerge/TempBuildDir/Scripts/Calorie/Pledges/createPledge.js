

//globals
$(document).on("keypress", 'form', function (e) {
    var code = e.keyCode || e.which;
    if (code === 13) {
        e.preventDefault();
        return false;
    }
    return true;
});


//Activites
var activities = new calorie.helpers.serialisableList;
activities.outputnode = $("#ActivitiesList");
activities.listname = "ActivityIDs";

function activityClick(event) {
    var target = $(event.currentTarget);
    if (target.data("checked")) {
        target.find("#tickbox").hide();
        target.data("checked", false);
        calorie.createPledge.activities.remove(target.data("activityname"));

    } else {
        target.find("#tickbox").show();
        target.data("checked", true);
        calorie.createPledge.activities.add(target.data("activityname"));
    }

}

var charitySelector = $('#CharitySelector');
var charityContainer = $("#charityContainer");
var teamSelector = $('#TeamSelector');

//Charities
function btnAddCharity(event) {
    event.preventDefault();
    event.stopPropagation();

    charitySelector.modal('show');

}


function btnCharityClick(event) {

    event.preventDefault();
    event.stopPropagation();

    charitySelector.modal('hide');
    var justgivingcharityid = $(event.currentTarget).data('justgivingcharityid');
    $('#JustGivingCharityID').val(justgivingcharityid);

    var target = $(event.currentTarget.parentNode.parentNode).clone();

    target.find(".fulldetails").hide();
    target.find("#btnAdd").hide();

    charityContainer.html(target);
    charityContainer.fadeIn();
    
}



//Teams
var teamList = new calorie.helpers.serialisableList();
teamList.listname = "TeamIDs";
teamList.outputnode = $("#hiddenTeamNodes");

//show team selector dialog
function btnPickTeam(event) {
    event.preventDefault();
    event.stopPropagation();

    teamSelector.modal('show');

}

function btnTeamClick(event) {

    event.preventDefault();
    event.stopPropagation();

    teamSelector.modal('hide');

    var teamNode = calorie.teams.getAndTidyTeamNode(event);
    var teamID = calorie.teams.getTeamID(event);
    $("#selectedTeams").append("<tr><td>" + teamNode.html() + "</td></tr>");

    teamList.add(teamID);

}