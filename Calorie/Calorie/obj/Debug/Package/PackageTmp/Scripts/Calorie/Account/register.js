
//im feeling lucky image
function ProfilePicLucky() {

    var spinner = $('#LuckySpinner');
    spinner.empty();
    spinner.append(calorie.helpers.spinnerInnerHTML).fadeIn();

    var toDelete = $('#ProfilePictureDiv').find('img').data('imageid');


    var formData = new FormData();
    formData.append('url', 'http://loremflickr.com/320/240/funny');
    formData.append('idToDelete', toDelete);

    $.ajax({
        type: 'POST',
        url: $('#registerScript').data('saveimagefromurl'),
        data: formData,
        dataType: 'html',
        contentType: false,
        processData: false,
        cache: false,
        success: function (response) {

            var ID = response;
            //add picture to dom
            var URL = $('#registerScript').data('getimage');

            $('#ProfilePictureDiv').html('<img class="uploaded-img" src=' + URL + '?ImageID=' + ID + ' data-imageid=' + ID + ' />');

            //get the ID from the dom and set to hidden field
            //var imageID = $('#ProfilePictureDiv').children(':first').data('imageid');
            $('#ProfilePictureImageID').val(ID);
            spinner.empty();
        },

        error: function () {
            spinner.empty();
        }


    });


};


//auto populate script
$(function() {

    $('#auto').on('click', function() {

        $.ajax({
            url: 'https://randomuser.me/api/1.0/',
            dataType: 'json',
            success: function(data) {

                $('#usernameInput').val(data.results[0].login.username);
                $('#Email').val(data.results[0].email);
                $('#Password').val('Secret#1');
                $('#ConfirmPassword').val('Secret#1');

                var formData = new FormData();
                formData.append("url", data.results[0].picture.large);

                $.ajax({
                    type: "POST",
                    url: $('#registerScript').data('saveimagefromurl'),
                    data: formData,
                    dataType: 'html',
                    contentType: false,
                    processData: false,
                    cache: false,
                    success: function(response) {

                        var ID = response;
                        //add picture to dom
                        var URL = $('#registerScript').data('getimage');

                        $('#ProfilePictureDiv').html('<img class="uploaded-img" src=' + URL + '?ImageID=' + ID + ' data-imageid=' + ID + ' />');                        
                        $('#ProfilePictureImageID').val(ID);

                    },

                    error: function() {

                    }


                });


            }
        });


    });

});


//username validation

$(function() {


    var usernameInput = $('#usernameInput');
    var usernameInputResult = $('#usernameInputResult');
    var usernameValidationMsg = $("#usernameValidationMsg");
    var usernameInputGroup = $('#usernameInputGroup');
    var registerBtn = $('#registerBtn');

    function sucessFunction(result) {

        if (result === "yes") {
            showUsernameValidationMessageSuccess();
        } else {
            showUsernameValidationMessageFail("Username is already taken");
        }

    }

    function errorFunction() {
    }


    usernameInput.on('keyup', function() {
        if (usernameInput.val().length > 10) {
            showUsernameValidationMessageFail("Username is too long.");
        } else {
            hideUsernameValidationMessage();
            calorie.account.checkUserName(usernameInput.val(), usernameInputResult, sucessFunction, errorFunction);
        }

    });

    function hideUsernameValidationMessage() {
        usernameValidationMsg.fadeOut();
        usernameInputResult.fadeOut();
    }

    function showUsernameValidationMessageFail(msg) {
        usernameValidationMsg.html(msg);
        usernameValidationMsg.fadeIn();
        usernameInputGroup.addClass('has-error');
        usernameInputResult.html('<span class=" glyphicon glyphicon-remove " ></span>');
        usernameInputResult.fadeIn();
        registerBtn.prop('disabled', true);
    }

    function showUsernameValidationMessageSuccess() {
        usernameValidationMsg.fadeOut();
        usernameInputGroup.removeClass('has-error');
        usernameInputResult.html('<span class="calorie-green-color glyphicon glyphicon-ok " ></span>');
        usernameInputResult.fadeIn();
        registerBtn.prop('disabled', false);
    }
});

