

var approvedUsers = new calorie.helpers.serialisableList();
approvedUsers.outputnode = $("#ApprovedUserIDs");
approvedUsers.listname = "IDs";

function ApproveUserClicked(event) {

    setActivity($(event.currentTarget));

}

function setActivity(target) {

    var userid = target.data("userid");

    if (!target.data("checked")) {

        target.find("#tickbox").css('opacity', 1);
        target.data("checked", true);
        approvedUsers.add(userid);

    } else {

        target.find("#tickbox").css('opacity', 0);
        target.data("checked", false);
        approvedUsers.remove(userid);
    }
    
}