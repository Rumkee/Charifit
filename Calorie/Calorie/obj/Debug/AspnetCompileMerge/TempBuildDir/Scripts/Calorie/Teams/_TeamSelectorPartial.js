

var teamSearchResults = $('#SearchResults');
var teamTopTeams = $('#TopTeams');
var teamSearchBtn = $("#search");
var teamSelector = $('#TeamSelector');

$(function () {
    $('form').submit(function () {
        if (teamSelector.hasClass('in')) {//if this modal is showing then cancel any submissions
            return false;
        }
        return true;
    });

    $('form input').keyup(function (event) {
        if (teamSelector.hasClass('in') && event.keyCode === 13) {
            teamSearch();
            event.preventDefault();
            return false;
        }
        return true;
    });
});

function TopTeamsSelect() {
    teamSearchResults.hide();
    teamTopTeams.fadeIn();
}

function teamSearch() {

    var searchval = $("#userSearchInput").val();

    if (searchval === "") {
        return;
    }
    teamTopTeams.hide();
    teamSearchResults.hide().empty();
    disableTeamSearch();

    $.ajax({
        type: "POST",
        url: '@Url.Action("Search", "Teams")',
        data: { searchTerm: searchval },
        dataType: 'html',
        success: function (result) {
            teamTopTeams.hide();
            teamSearchResults.append(result).fadeIn();
            calorie.helpers.fancyFadeInTable(teamSearchResults);

            enableTeamSearch();
        },
        error: function () {
            enableTeamSearch();
        }
    });

}

function disableTeamSearch() {
    teamSearchBtn.empty();
    teamSearchBtn.append('<span class="spinner btn btn-sm form-control-feedback ">' + calorie.helpers.spinnerInnerHTML + '</span>');
}

function enableTeamSearch() {
    teamSearchBtn.empty();
    teamSearchBtn.append('<span class="btn btn-sm form-control-feedback glyphicon glyphicon-search" style="pointer-events:auto;" onclick="teamSearch()"></span>');
}