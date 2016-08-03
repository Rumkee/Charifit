

calorie.teams = {
    
    search: function (searchTerm,getURL,callback) {

        $.getJSON(getURL + "?SearchTerm=" + searchTerm,
            function (json) {
                callback(json);
            }).error(function (error) {
                alert(error);
            });
    },

    
    getTeamPanel: function (imgID, name, membership, teamId, description) {

        var mergeData=
        {
            imgID: imgID,
            name: name,
            nameNoSpace: name.replace(' ',''),
            membership: membership,
            teamID: teamId,
            description: description
        }

        var source = $("#team-template").html();
        var template = Handlebars.compile(source);
        return template(mergeData);       
        
    },
    
    getAndTidyTeamNode: function (event) {

        var teamNode = $(event.currentTarget.parentNode.parentNode.parentNode.parentNode).clone();
        calorie.helpers.hideButtonsAndFullDetails(teamNode);
        return teamNode;

    },

    getTeamID: function (event){
        return event.currentTarget.dataset.teamid;
    }      
             
}
