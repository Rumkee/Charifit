

var charitiesSearchResults = $('#CharitiesSearchResults');
var charitySelector = $('#CharitySelector');
var topCharities = $('#TopCharities');
var searchBtn = $('#Charitysearch');

var charitySelectorPartial = $('#CharitySelectorPartialScript');
var searchCharitiesUrl = charitySelectorPartial.data('search-charities-url');


function TopCharitiesSelect() {
    charitiesSearchResults.hide();
    topCharities.fadeIn();
}

$(function () {

    $('form').submit(function () {
        if (charitySelector.hasClass('in')) { //if this modal is showing then cancel any submissions
            return false;
        }
        return true;
    }
    );

    $('form input').keyup(function (event) {
        if (charitySelector.hasClass('in') && event.keyCode === 13) {
            CharitySearch();
            event.preventDefault();
            return false;
        }
        return true;
    });
});


function CharitySearch() {

    var searchval = $('#CharitiesSearchInput').val();

    if (searchval === '') {
        return;
    }
    topCharities.hide();
    charitiesSearchResults.hide().empty();

    disableCharitySearch();

    $.ajax({
        type: 'POST',
        url: searchCharitiesUrl,
        data: { searchTerm: searchval },
        dataType: 'html',
        success: function (result) {
            topCharities.hide();
            charitiesSearchResults.append(result).fadeIn();
            calorie.helpers.fancyFadeInTable(charitiesSearchResults);

            enableChairtySearch();

        },
        error: function () {
            enableChairtySearch();
        }
    });


}



function disableCharitySearch() {
    searchBtn.empty();
    searchBtn.append('<span class="spinner btn btn-sm form-control-feedback ">' + calorie.helpers.spinnerInnerHTML + '</span>');
}

function enableChairtySearch() { 
    searchBtn.empty();
    searchBtn.append('<span class="btn btn-sm form-control-feedback glyphicon glyphicon-search" style="pointer-events:auto;" onclick="CharitySearch()"></span>');
}
