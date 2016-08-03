
var editScript = $('EditScript');
var killAlertUrl = editScript.data('kill-alert-url');
var killAllAlerstUrl = editScript.data('kill-all-alerts-url');


//Exerciser Settings
var exercisorChk = $("#User_IsExercisor");
exercisorChk.change(function () {
    if (exercisorChk.is(':checked')) {
        $("#ExerciserSettings").fadeIn();
    } else {
        $("#ExerciserSettings").fadeOut();
    }
});



//Company Account
var isCompany = $("#User_IsCompany");
isCompany.change(function () {
    if (isCompany.is(':checked')) {
        $("#SponsoringRow").fadeOut();
        $("#ExerciserRow").fadeOut();
        $("#CorporateAccountSettings").fadeIn();
        reApplyStripe();
    } else {
        $("#SponsoringRow").fadeIn();
        $("#ExerciserRow").fadeIn();
        $("#CorporateAccountSettings").fadeOut();
        reApplyStripe();
    }
});


new calorie.textEditor('DescriptionTextEditor', 'User_CompanyDescription', 'EditAccountForm');

//$('#DescriptionTextEditor').summernote({ minHeight: 300 }, 'code', '');

//$(function () {
//    $('#EditAccountForm').on('submit', function () {
//        $('#User_CompanyDescription').empty();
//        $('#User_CompanyDescription').val($('#DescriptionTextEditor').summernote('code'));
//    });
//});



//Teams

function btnAddTeam(event) {
    event.preventDefault();
    event.stopPropagation();

    $('#TeamSelector').modal('show');

}

function btnTeamClick(event) {

    event.preventDefault();
    event.stopPropagation();

    $('#TeamSelector').modal('hide');

    var teamNode = calorie.teams.getAndTidyTeamNode(event);
    var teamID = calorie.teams.getTeamID(event);
    $("#User_TeamID").val(teamID);
    $("#selectedteam").empty();
    $("#selectedteam").append(teamNode);

}




//Charities

var PreferredjustGivingCharities = new calorie.helpers.serialisableList();
PreferredjustGivingCharities.listname = "PreferredjustGivingCharities";
PreferredjustGivingCharities.outputnode = $("#PreferredjustGivingCharities");

function btnAddCharity(event) {
    event.preventDefault();
    event.stopPropagation();

    $('#CharitySelector').modal('show');

}

function btnRemoveCharityClick(event) {

    event.preventDefault();
    event.stopPropagation();

    var justgivingcharityid = $(event.currentTarget).data('justgivingcharityid');
    justgivingcharityid = String(justgivingcharityid).replace(" ", "");
    PreferredjustGivingCharities.remove(justgivingcharityid);

    var charToRemove = $("#charity" + justgivingcharityid);
    charToRemove.fadeOut(null, function() { charToRemove.remove() });
    calorie.helpers.reApplyTableStriping();

}

function btnCharityClick(event) {

    event.preventDefault();
    event.stopPropagation();

    $('#CharitySelector').modal('hide');
    var justgivingcharityid = $(event.currentTarget).data('justgivingcharityid');
    justgivingcharityid = String(justgivingcharityid).replace(" ", "");

    PreferredjustGivingCharities.add(String(justgivingcharityid));


    var target = $(event.currentTarget.parentNode.parentNode).clone();


    target.find(".fulldetails").hide();
    target.find("#btnAdd").hide();
    target.find("#btnRemove").show();
    $('<tr id="charity' + justgivingcharityid + '"><td><div class="row">' + target.html() + '</div></tr></td>').hide().appendTo("#charityContainer").fadeIn();


}





//Alerts
function removeAlert(ident, event) {

    var disabler = new calorie.helpers.buttonDisabler();
    disabler.button = $(event.currentTarget);
    disabler.disable();
    $("#AlertFeedback").hide();

    $.ajax({
        type: "POST",
        url: killAlertUrl,
        data: JSON.stringify({ 'ID': ident }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        processData: false,
        success: function () {

            var alertElem = $(":root").find("[data-alertid='" + ident + "']");
            alertElem.fadeOut(null, function () {
                calorie.helpers.reApplyTableStriping();
            });

            $("#AlertFeedbackMsg").html("Alert has been removed.");
            $("#AlertFeedback").fadeIn();


        },

        error: function () {

            disabler.enable();
        }
    });


}

function KillAllAlerts() {
    var disabler = new calorie.helpers.buttonDisabler();
    disabler.button = $("#btnKillAllAlerts");
    disabler.disable();
    $("#AlertFeedback").hide();

    $.ajax({
        type: "POST",
        url: killAllAlerstUrl,
        data: null,
        dataType: 'html',
        contentType: false,
        processData: false,
        success: function () {

            var alertElems = $(":root").find("[data-alertid!=''][data-alertid]");
            alertElems.fadeOut();

            $("#AlertFeedbackMsg").html("All alerts have been removed.");
            $("#AlertFeedback").fadeIn();

            disabler.enable();
            disabler.button.addClass("disabled");
        },

        error: function () {

            disabler.enable();
        }
    });

}


//User Name
$(function () {

    var usernameValidationMsg = $("#usernameValidationMsg");
    var usernameInput = $('#usernameInput');
    var usernameInputGroup = $('#usernameInputGroup');
    var usernameInputResult = $('#usernameInputResult');
    var saveBtn = $('#saveBtn');

    function sucessFunction(result) {
        if (result === "yes") {
            showUsernameValidationMessageSuccess();
        } else {
            showUsernameValidationMessageFail("Username is already taken");
        }

    };


    function errorFunction() {
    };


    usernameInput.on('keyup', function () {
        if (usernameInput.val().length > 10) {
            showUsernameValidationMessageFail("Username is too long.");
        } else {
            hideUsernameValidationMessage();
            calorie.account.checkUserName(usernameInput.val(), usernameInputResult, sucessFunction, errorFunction);
        }

    });

    function hideUsernameValidationMessage() {
        usernameValidationMsg.fadeOut();
        usernameInputResult.fadeOut();
    };

    function showUsernameValidationMessageFail(msg) {
        usernameValidationMsg.html(msg);
        usernameValidationMsg.fadeIn();
        usernameInputGroup.addClass('has-error');
        usernameInputResult.html('<span class=" glyphicon glyphicon-remove " ></span>');
        usernameInputResult.fadeIn();
        saveBtn.prop('disabled', true);
    };

    function showUsernameValidationMessageSuccess() {
        usernameValidationMsg.fadeOut();
        usernameInputGroup.removeClass('has-error');
        usernameInputResult.html('<span class="calorie-green-color glyphicon glyphicon-ok " ></span>');
        usernameInputResult.fadeIn();
        saveBtn.prop('disabled', false);
    };
});



//preferred activities
var preferredActivities = new calorie.helpers.serialisableList();
preferredActivities.outputnode = $("#PreferredActivitiesList");
preferredActivities.listname = "PreferredActivities";

function activityClicked(event) {

    setActivity($(event.currentTarget));

}

function setActivity(target, forceChecked) {

    var activityName = target.data("activityname");

    if ((!target.data("checked")) | forceChecked) {

        target.find("#tickbox").show();
        target.data("checked", true);
        preferredActivities.add(activityName);

    } else {

        target.find("#tickbox").hide();
        target.data("checked", false);
        preferredActivities.remove(activityName);
    }

}


//Generic
function reApplyStripe() {
    calorie.helpers.reApplyTableStriping();
}

