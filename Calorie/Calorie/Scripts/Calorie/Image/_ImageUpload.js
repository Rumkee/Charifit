

var script = $('#imageUploadScript');
var type = script.data('type');
var saveImageURL = script.data('save-img-url');
var getImageURL = script.data('get-image-url');

var pictureDiv = $('#' + script.data('picture-div'));
var pictureErrorDiv = $('#' + script.data('picture-error-div'));
var hiddenForElementID = $('#' + script.data('hidden-for-element-id'));


$(document).on('change', '.btn-file :file', function () {
    var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        file = input.get(0).files[0];
    input.trigger('fileselect', [numFiles, file]);
});

$(function () {

    var btnDisabler = new calorie.helpers.buttonDisabler();
    btnDisabler.button = $("#ImageChooseButton");


    $('.btn-file :file').on('fileselect', function (event, numFiles, file) {
          
        btnDisabler.disable();


        //make submission
        var formData = new FormData();
        formData.append("FileUpload", file);
        formData.append("CurrentImageID", pictureDiv.children(":first").data("imageid"));
        formData.append("Type", type);

        $.ajax({
            type: "POST",
            url: saveImageURL,
            data: formData,
        dataType: 'html',
        contentType: false,
        processData: false,
        success: function (response) {

            var ID = response;
            //add picture to dom            
            pictureDiv.html("<img class='uploaded-img' src=" + getImageURL + "?ImageID=" + ID + " data-imageid=" + ID + " />");

            //get the ID from the dom and set to hidden field
            var imageID = pictureDiv.children(":first").data("imageid");
            hiddenForElementID.val(imageID);

            //reset choose button
            btnDisabler.enable();
            pictureErrorDiv.empty();
        },


        error: function () {

            //display error message
            pictureErrorDiv.empty();
            pictureErrorDiv.append("<span class=\"text-danger field-validation-error\" data-valmsg-replace=\"true\" data-valmsg-for=\"@thisID\"><span for=\"@thisID\" class=\"\">oops! that didn't work. Try a smaller picture or a different format</span></span>");

            //reset choose button
            btnDisabler.enable();                    
        }
    });


});
});