
calorie.images = {

    getImageURL: '',
    saveImageURL: '',
    deleteImageURL: '',
    saveImageFromURLURL: '',

    SaveImage: function (successCallback, errorCallback, _data) {

        $.ajax({
            type: "POST",
            url: calorie.images.saveImageURL,
            data: _data,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function (response) {
                successCallback(response);
            },
            
            error: function (error) {
                errorCallback(error);
            }
        });

    },

    deleteImage: function (successCallback, errorCallback, Id,callbackObj) {

        var formData = new FormData();
        formData.append("_ImageID", Id);

        $.ajax({
            type: "POST",
            url: calorie.images.deleteImageURL,
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function () {
                successCallback(callbackObj);
            },

            error: function (error) {
                errorCallback(error);
            }

        });

    },


    saveImageFromURL: function(successCallback, errorCallback, URL){
        
        var formData = new FormData();
        formData.append("url", URL);

        $.ajax({
            type: "POST",
            url: calorie.images.saveImageFromURLURL,
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            cache: false,
            success: function (response) {
                successCallback(response);        
                },

            error: function (error) {
                errorCallback(error);
            }
            
        });
    }
}
