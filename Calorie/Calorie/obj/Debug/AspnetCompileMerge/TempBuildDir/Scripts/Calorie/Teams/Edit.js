



var teamNameValidationMsg = $("#TeamnameValidationMsg");
var teamNameInputResult = $('#TeamnameInputResult');
var teamNameInputGroup = $('#TeamnameInputGroup');
var teamNameInput = $('#TeamnameInput');
var saveBtn = $('#saveBtn');


$(function () {

    function sucessFunction(result) {

        if (result === "yes") {
            showTeamnameValidationMessageSuccess();
        } else {
            showTeamnameValidationMessageFail("Team name is already taken");
        }

    }

    function errorFunction() {
        showTeamnameValidationMessageFail("There was a problem, please try again later.");
    }

    teamNameInput.on('keyup', function () {

        var name = teamNameInput.val();

        if (name === currentTeamName) {
            showTeamnameValidationMessageSuccess();
            return;
        }

        if (teamNameInput.val().length > 10) {

            showTeamnameValidationMessageFail("Team name is too long.");

        } else {
            hideTeamnameValidationMessage();


            $.ajax({
                type: "GET",
                url: checkTeamNameURL,
                data: "TeamName=" + name,
                dataType: 'html',
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    sucessFunction(response);

                },

                error: function (error) {
                    errorFunction(error);
                }

            });

        }

    });

    function hideTeamnameValidationMessage() {
        teamNameValidationMsg.fadeOut();
        teamNameInputResult.fadeOut();
    }

    function showTeamnameValidationMessageFail(msg) {
        teamNameValidationMsg.html(msg);
        teamNameValidationMsg.fadeIn();
        teamNameInputGroup.addClass('has-error');
        teamNameInputResult.html('<span class=" glyphicon glyphicon-remove"></span>');
        teamNameInputResult.fadeIn();
        saveBtn.prop('disabled', true);
    }

    function showTeamnameValidationMessageSuccess() {
        teamNameValidationMsg.fadeOut();
        teamNameInputGroup.removeClass('has-error');
        teamNameInputResult.html('<span class="calorie-green-color glyphicon glyphicon-ok"></span>');
        teamNameInputResult.fadeIn();
        saveBtn.prop('disabled', false);
    }
});