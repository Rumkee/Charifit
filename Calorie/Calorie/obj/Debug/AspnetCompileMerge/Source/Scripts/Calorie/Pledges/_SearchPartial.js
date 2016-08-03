  

    var cancelPrompt = false;
    var userSearchResults = $("#userSearchResults");
    var userSearchInput = $("#userSearchInput");
    var userSearchPromptResults = $("#userSearchPromptResults");
    var searchBtn = $("#search");
    var searchURL = searchBtn.data('searchURL');


    $(function () {

        $('form').submit(function () {
            return false;
        });

        $('form input').keyup(function (event) {
            if (event.keyCode === 13) {
                cancelPrompt = true;
                userSearch();
                event.preventDefault();
                return false;
            } else {
                cancelPrompt = false;
                userSearchPrompt();
            }
        });


    });


    function AddPledgeAlert(inputStr) {

        $.ajax({
            type: "POST",
            url: addPledgeAlertURL,
            data: { searchString: inputStr },
            dataType: 'html',
            success: function (result) {

                userSearchResults.empty();
                userSearchResults.show();
                userSearchResults.append(result);



                enableSearch();

            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });


    }



    function userPromptSelect(value) {
        userSearchInput.val(value);
        userSearch();
    }

    function userSearch() {

        var searchval = userSearchInput.val();

        if (searchval === "") {
            return;
        }

        clearuserPromptResults();
        disableSearch();


        $.ajax({
            type: "POST",
            url: userSearchURL,
            data: { searchString: searchval },
            dataType: 'html',
            success: function (result) {

                userSearchResults.empty();
                userSearchResults.show();
                userSearchResults.append(result);
                

                calorie.helpers.fancyFadeInTable(userSearchResults);

                calorie.currency.globalCurrencyConvertScan();
                enableSearch();

            },
            error: function () {
                enableSearch();
            }
        });

    }

    function clearuserPromptResults() {

        userSearchPromptResults.find(".userSearchPromptTeam").remove();
        $("#TeamActivityDivider").remove();
        userSearchPromptResults.find(".userSearchPromptActivities").remove();
        $("#ActivityCharityDivider").remove();
        userSearchPromptResults.find(".userSearchPromptCharities").remove();
        CheckShowUserPromptResults();
    }

    function userSearchPrompt() {


        var searchval = userSearchInput.val();


        if (searchval === "" || searchval.length < 4) {
            clearuserPromptResults();
            return;
        }


        $.getJSON(userSearchPromptURL,
               { searchString: searchval },
                function (result) {

                    if (!cancelPrompt) {


                        //team results
                        //clear old team resuls
                        userSearchPromptResults.find(".userSearchPromptTeam").remove();
                        //populate each team prompt
                        result.teams.slice(0, 10).forEach(function (t) {
                            userSearchPromptResults.append(getUserSearchPrompULItemtHTML("userSearchPromptTeam", t.Url, t.Name));
                        });
                        //activities results
                        //clear old activities results and divider
                        userSearchPromptResults.find(".userSearchPromptActivities").remove();
                        $("#TeamActivityDivider").remove();

                        //add new activities divider if needed
                        if (result.activities.length > 0) {
                            userSearchPromptResults.append('<li class="divider" id="TeamActivityDivider"></li>');
                        }
                        //add new activities
                        result.activities.slice(0, 10).forEach(function (t) {
                            userSearchPromptResults.append(getUserSearchPrompULItemtHTML("userSearchPromptActivities", t.Url, t.Name));
                        });
                        //charities results
                        //remove old charities results and divider
                        userSearchPromptResults.find(".userSearchPromptCharities").remove();
                        $("#ActivityCharityDivider").remove();

                        //add new charities divider if needed
                        if (result.charities.length > 0) {
                            userSearchPromptResults.append('<li class="divider" id="ActivityCharityDivider"></li>');
                        }
                        //add new charities
                        result.charities.slice(0, 10).forEach(function (itm) {
                            userSearchPromptResults.append(getUserSearchPrompULItemtHTML("userSearchPromptCharities", itm.Url + '?template=size120x120&imagetype=charitybrandinglogo', itm.Name));
                        });


                        CheckShowUserPromptResults();
                    }

                }
               ).error(
               function () {
                   $("#TeamActivityDivider").remove();
                   CheckShowUserPromptResults();
               });


    }

    function getUserSearchPrompULItemtHTML(_class, _url, _name) {
        return '<li onclick="userPromptSelect(\'' + _name + '\')" class="' + _class + '"><a href="#"><img src="' + _url + '" class="pledgePromptSearchImage"  />' + _name + '</a> </li>'
    }


    function disableSearch() {
        searchBtn.empty();
        searchBtn.append('<span class="spinner btn form-control-feedback ">' + calorie.helpers.spinnerInnerHTML + '</span>');
        searchBtn.data('isenabled', 'false');
    }

    function enableSearch() {
        searchBtn.empty();
        searchBtn.append('<span class="btn form-control-feedback glyphicon glyphicon-search calorie-green-color hyperlinkUnderline"  onclick="userSearch()"></span>');
        searchBtn.data('isenabled', 'true');
    }

    function CheckShowUserPromptResults() {

        if (userSearchPromptResults.children().length > 0) { //&& searchResults.children().length == 0
            userSearchPromptResults.fadeIn();
        } else {
            userSearchPromptResults.fadeOut();
        }

    }
