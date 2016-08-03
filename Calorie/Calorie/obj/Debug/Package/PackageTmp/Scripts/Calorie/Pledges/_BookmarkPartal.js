
 calorie.Bookmark = function (btn, pledgeID, removeBookmarkURL, addBookmarkURL) {

    this.btn = btn;
    this.checked = this.btn.data("checked");
    this.pledgeID = pledgeID;

    this.removeBookmarkURL = removeBookmarkURL;
    this.addBookmarkURL = addBookmarkURL;
    var me = this;
    
    this.bookmarkClicked= function() {
        
        me.bookmarkSetSpinner( true);

        if (me.checked) {
            //remove

            $.ajax({
                type: "GET",
                url: me.removeBookmarkURL,
                data: 'pledgeID= @Model.PledgeID',
                dataType: 'html',
                contentType: false,
                processData: false,
                cache: false,
                success: function () {
                    me.bookmarkSetSpinner( false);

                    me.btn.data("bookmarkid", "");
                    me.bookmarkSetChecked( false);


                },

                error: function () {
                    me.bookmarkSetSpinner( false);
                    me.bookmarkSetChecked(true);
                }

            });


        } else {
            //add

            $.ajax({
                type: "GET",
                url: me.addBookmarkURL,
                data: 'pledgeID= ' + me.pledgeID ,
                dataType: 'html',
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    me.bookmarkSetSpinner( false);

                    me.btn.data("bookmarkid", response);
                    me.bookmarkSetChecked(true);

                },

                error: function () {
                    me.bookmarkSetSpinner( false);

                    me.btn.data("bookmarkid", "");
                    me.bookmarkSetChecked(false);
                }

            });

        }


    }

    this.bookmarkSetSpinner = function(isOn) {
        if (isOn) {
            me.btn.find("#icon").empty().append(calorie.helpers.spinnerInnerHTML).fadeIn();
        } else {
            me.btn.find("#icon").empty();
        }
    }

    this.bookmarkSetChecked = function(checked) {

        me.checked = checked;

        if (me.checked) {
            me.btn.find("#icon").empty().append('<span class="calorie-green-color glyphicon glyphicon-ok pull-right"></span>').fadeIn();
            me.btn.find(".comment").empty().append("&nbsp;Bookmarked&nbsp;");
            me.btn.data("checked", true);
        } else {
            me.btn.find("#icon").fadeOut().empty();
            me.btn.find(".comment").empty().append("&nbsp;Bookmark&nbsp;");
            me.btn.data("checked", false);
        }
    }

    this.btn.click(function() { me.bookmarkClicked()});

}
