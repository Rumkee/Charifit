

calorie.account = {

    checkUserNameURL: null,
    checkingUserName: null,


    checkUserName: function(inputname,resultControl,sucesssFunction,errorFunction){
                         

            //set spinner animation
        resultControl.empty();
        resultControl.append(calorie.helpers.spinnerHTML);

            if (calorie.account.checkingUserName !== null) {
                calorie.account.checkingUserName.abort();
            };


        calorie.account.checkingUserName = $.ajax({
            type: "GET",
            url: calorie.account.checkUserNameURL,
            data: "UserName=" + inputname,
            dataType: 'html',
            contentType: false,
            processData: false,
            cache: false,
            success: function(response) {
                sucesssFunction(response);
            },

            error: function(err) {
                errorFunction(err);

            }

        });

    }
    
}

