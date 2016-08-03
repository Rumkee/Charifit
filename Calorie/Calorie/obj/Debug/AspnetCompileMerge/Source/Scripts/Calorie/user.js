

calorie.LoggedinUser = function(_info) {

    this.info = _info;

    this.isLoggedIn= function(){        
        return this.info.data(userloggedin);
    },
    
    this.getProp= function(propName){        
        return this.info.data(propName);

    },

    this.userName= function(){        
        return this.info.data(propName);
    },

    this.userid= function(){        
        return this.info.data("userid");
    },

    this.usercurrency= function(){        
        return this.info.data("usercurrency");
    },

    this.useremail= function(){        
        return this.info.data("useremail");
    },

    this.useriscompany= function(){        
        return this.info.data("useriscompany");
    },

    this.userisexercisor= function(){        
        return this.info.data("userisexercisor");
    },

    this.userissponsor= function(){        
        return this.info.data("userissponsor");
    },

    this.userteamid= function(){        
        return this.info.data("userteamid");
    },

    this.userprofilepictureid= function(){        
        return this.info.data("userprofilepictureid");
    }

    this.userPartial= function () {

        this.url = null;
        this.pic = null;
        this.pop = null;
        this.userID = null;
        this.countryCode = null;

        var _this = this;

        _this.init = function (_url, _pic, _pop, _userID) {

            _this.url = _url;
            _this.pic = _pic;
            _this.pop = _pop;
            _this.userID = _userID;

            _this.pic.on('mouseenter', _this.pic_mouseenter);
            _this.pic.on('mouseleave', _this.pic_mouseleave);
            _this.pic.on('mousemove', _this.pic_mousemove);


        }//init


        _this.pic_mouseenter = function () {

            if (!_this.pic.data('loaded')) {


                $.getJSON(_this.url,
                {
                    UserID: _this.userID
                },
                function (json) {
                    var source = $("#userPopUp-template").html();
                    var template = Handlebars.compile(source);
                    var html = template(json);
                    _this.pop.html(html);
                    calorie.currency.checkConvertAmount($(_this.pop).find('#pledgedTotal'));
                    _this.pic.data('loaded', 'true');
                }
               ).error(function () { });

            }

            _this.pop.removeClass('userPopUp-hidden');

        }



        _this.pic_mouseleave = function () {
            _this.pop.addClass('userPopUp-hidden');
        }

        _this.pic_mousemove = function (event) {

            _this.pop.css({
                left: (event.offsetX || event.originalEvent.layerX) + 10,
                top: (event.offsetY || event.originalEvent.layerY) + 10

            });

        }


    }
    
}

calorie.user = new calorie.LoggedinUser($("#loggedInUserData"));


