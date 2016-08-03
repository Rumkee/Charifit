
for (var property in Modernizr) {
    if (Modernizr.hasOwnProperty(property)) {
        $("#sandboxModernizrDetails").append("<tr><td>" + property + "</td><td>" + Modernizr[property] + "</td></tr>");
    }
}


function sandboxInfoClick() {
    $("#sandboxMoreDetails").slideToggle();
    $("#sandboxInfoClickMore").toggle();
    $("#sandboxInfoClickLess").toggle();
}