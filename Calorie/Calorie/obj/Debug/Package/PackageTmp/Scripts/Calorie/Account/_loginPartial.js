


var btnDisabler = new calorie.helpers.buttonDisabler();
btnDisabler.button = $('#partialLoginBtn');
var logInFailMsg = $('#LogInFailMsg');

function LogInPartial(event) {


    var postUrl = $('#_loginPartial').data('loginmodal-url');
    logInFailMsg.fadeOut();
    event.preventDefault();

    if ($('#UsernameOrEmail').valid() & $('#Password').valid()) {

        btnDisabler.disable();

        var form = $('#loginform');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            url: postUrl,
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                model: {
                    UsernameOrEmail: $('#UsernameOrEmail').val(),
                    Password: $('#Password').val(),
                    RememberMe: $('#RememberMe').is(':checked')
                }
            },
            success: function (result) {
                location.reload(true);

            },
            error: function (err) {
                logInFailMsg.empty();
                logInFailMsg.append("Nope. try again.");
                logInFailMsg.fadeIn();
                btnDisabler.enable();
            }
        });

    }

}
