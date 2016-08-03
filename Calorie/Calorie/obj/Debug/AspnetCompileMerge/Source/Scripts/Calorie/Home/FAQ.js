
function faqClick(event) {

    $(".faq-answer").slideUp();
    var thisAnswer = $(event.currentTarget).find(".faq-answer");
    if (!thisAnswer.is(":visible")) {
        thisAnswer.slideDown();
    }
}

function faqHelpfulClick(id) {
    calorie.social.likes.likeClick(event, 'FAQ', id, $('#faq-helpful-' + id)); event.stopPropagation();
}

function faqHelpfulLoad(id) {

    calorie.social.likes.count('FAQ', id, function (e) {
        $("#faq-helpful-" + id).empty(); $("#faq-helpful-" + id).append(e);
    });

}

