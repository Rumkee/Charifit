

calorie.imageGallery = {
       

    GalleryImageChooseButton: null,
    Gallery: null,
    SerialiseObj: new calorie.helpers.serialisableList,

    init: function(){

        calorie.imageGallery.SerialiseObj.listname = "GalleryIDs";
        calorie.imageGallery.SerialiseObj.outputnode = $("#GalleryHiddenInputs");

        calorie.imageGallery.GalleryImageChooseButton = $("#GalleryImageChooseButton");
        calorie.imageGallery.Gallery = $("#Gallery");
        $('.btn-file :file').on('fileselect', calorie.imageGallery.SaveGalleryImage);

    },

    loadEditableImage: function(url,id,trashID){

        var imageHTML = calorie.imageGallery.getImageHTML(url, id, trashID);
        $(imageHTML).hide().appendTo(calorie.imageGallery.Gallery).fadeIn(500);        
        calorie.imageGallery.GalleryCounter += 1;
        calorie.imageGallery.SerialiseObj.add(id);
    },


    SaveGalleryImage: function (event, numFiles, file) {


        //disable button
        calorie.imageGallery.GalleryImageChooseButton.prop('disabled', true)
            .children(":first")
            .html("Uploading " + calorie.helpers.spinnerInnerHTML);


        //get data
        var formData = new FormData();
        formData.append("FileUpload", file);
        formData.append("Type", "PledgePhoto");


        //make submission
        calorie.images.SaveImage(calorie.imageGallery.gallarySaveComplete, calorie.imageGallery.gallarySaveError, formData);


    },

    getImageHTML: function(thisUrl,thisID,thisTrashID){

        var source = $("#image-entry-template").html();
        var template = Handlebars.compile(source);
        var context = { url: thisUrl, ID: thisID, TrashID: thisTrashID};
        return template(context);

    },

    gallarySaveComplete: function(response){

        var trashID = 'Trash' + response;
        var imgHTML = calorie.imageGallery.getImageHTML(calorie.images.getImageURL, response, trashID);

        $(imgHTML).hide().appendTo(calorie.imageGallery.Gallery).fadeIn(500);
        calorie.imageGallery.GalleryCounter += 1;
        
        //reset choose button
        calorie.imageGallery.GalleryImageChooseButton.prop('disabled', false)
            .children(":first")
            .html('<span class="glyphicon glyphicon-cloud-upload"></span>&nbsp;Upload');


        calorie.imageGallery.SerialiseObj.add(response);

    },

    gallarySaveError: function () {

        //display error message
        calorie.imageGallery.Gallery.append("<span class=\"text-danger field-validation-error\" data-valmsg-replace=\"true\" data-valmsg-for=\"Gallery\"><span for=\"Gallery\" class=\"\">oops! that didn't work. Try a smaller picture or a different format</span></span>");

        //reset choose button
        calorie.imageGallery.GalleryImageChooseButton.prop('disabled', false)
            .children(":first")
            .html('<span class="glyphicon glyphicon-cloud-upload"><span>&nbsp;Upload');

    },

    
    removeImage: function (iD) {
      
        var itm = $("#" + iD);
        var idToRemove = itm.data('deleteimageid');
        
        itm.parent().fadeOut(400, function () { itm.parent().remove() });
        calorie.imageGallery.SerialiseObj.remove(idToRemove);
               
      
    }
  


}

      

      
