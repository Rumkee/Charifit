
calorie.social = {

    facebookShare: function(url,comment,event) {
        
        FB.ui({
            method: 'share',
            href: url,
            mobile_iframe: true,
            quote:comment
        }, function (response) { });

        event.stopPropagation();
    },

    linkedInShare: function (url, comment,event) {


        var thisUrl = 'https://www.linkedin.com/shareArticle?mini=true'
            + '&url=' + url
            + "&title="
            + '&summary=' + encodeURI(comment)
            + '&source=ChariFit';
        
        window.open(thisUrl, 'targetWindow', 'toolbar=no,location=yes,status=yes,menubar=no,scrollbars=yes,resizable=no,width=520,height=570');
      
        event.stopPropagation();
    },

    twitterShare: function (url, comment,event) {


        var thisUrl = 'https://twitter.com/share?' +
        'url=' + encodeURI(url) +
        '&via=ChariFitApp'+
        //'&related=twitterapi%2Ctwitter'+
        '&hashtags=charity%2Cfitness' +
        '&text=' + encodeURI(comment);

        window.open(thisUrl, 'targetWindow', 'toolbar=no,location=yes,status=yes,menubar=no,scrollbars=yes,resizable=no,width=520,height=570');

        event.stopPropagation();
    },
    
}

calorie.social.likes = {

    addUrl: "",
    countUrl: "",

    like: function(linkType, linkID, callback) {

        //make submission
        var formData = new FormData();
        formData.append("LinkType", linkType);
        formData.append("LinkID", linkID);

        $.ajax({
            type: "POST",
            url: calorie.social.likes.addUrl,
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function(response) {

                if ($.isNumeric(response)) {
                    callback(response);
                } else {
                    callback(0);
                }
                
            },

            error: function() {
                callback(0);
            }
        });

    },

    count: function (linkType, linkID, callback) {

        var formData = new FormData();
        formData.append("LinkType", linkType);
        formData.append("LinkID", linkID);

        $.ajax({
            type: "POST",
            url: calorie.social.likes.countUrl,
            data: formData,
            dataType: 'html',
            contentType: false,
            processData: false,
            success: function(response) {

                if ($.isNumeric(response)) {
                    callback(response);
                } else {
                    callback(0);
                }
                
            },

            error: function() {
                callback(0);
            }
        
        });

    },

    likeClick: function (event, linkType, linkID, countTarget) {
        
            var disabler = new calorie.helpers.buttonDisabler();
            disabler.button = $(event.currentTarget);
            disabler.disable();

        calorie.social.likes.like(linkType, linkID,
            function(count) {
                countTarget.html(count);
                disabler.enable();
            }
        );
        event.stopPropagation();

    }

   
}




