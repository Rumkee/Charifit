$(function () {
    
    var createTeamRequest;
    var teamInputResult = $('#teamInputResult');
    var teamInputGroup = $('#teamInputGroup');
    var teamCreateBtn = $('#teamCreateBtn');



    $('#teamInput').on('keyup', function () {

        //set spinner animation
        teamInputResult.empty();
        teamInputResult.append(calorie.helpers.spinnerInnerHTML);

        if (typeof createTeamRequest !== 'undefined') {
            createTeamRequest.abort();
        };


        createTeamRequest = $.ajax({
            type: "GET",
            url: createTeamURL,
            data: "TeamName=" + $('#teamInput').val(),
            dataType: 'html',
            contentType: false,
            processData: false,
            cache: false,
            success: function (response) {

                if (response === "yes") {
                    teamInputResult.empty();
                    teamInputResult.append('<span class="calorie-green-color glyphicon glyphicon-ok " data-toggle="tooltip" title="This team name is available"></span>');
                    teamInputGroup.removeClass('has-error');
                    $('[data-toggle="tooltip"]').tooltip();
                    teamCreateBtn.prop('disabled', false);
                } else {
                    teamInputResult.empty();
                    teamInputResult.append('<span class="calorie-green-color glyphicon glyphicon-remove " data-toggle="tooltip" title="team name is already taken"></span>');
                    teamInputGroup.addClass('has-error');
                    $('[data-toggle="tooltip"]').tooltip();
                    teamCreateBtn.prop('disabled', true);
                }

            },

            error: function () {
                teamInputResult.empty();
                teamInputResult.append('<span class="calorie-green-color glyphicon glyphicon-remove " data-toggle="tooltip" title="there was an error."></span>');
                teamInputGroup.addClass('has-error');
                $('[data-toggle="tooltip"]').tooltip();
                teamCreateBtn.prop('disabled', true);

            }


        });

    });
})
