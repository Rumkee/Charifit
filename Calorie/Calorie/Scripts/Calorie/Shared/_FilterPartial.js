
calorie.FilterPartial = function () {
  
    this.filterType = $('.filter-type');
    this.filterCount = $('.filter-count');
    this.icon = $("#filter-icon");
    this.iconSpinner = $("#filter-icon-spinner");
    var _this = this;

    this.filterType.on('change', function () {

       _this.filterDisable();
       _this.filterSearch();

    });

    this.filterCount.on('change', function () {

        _this.filterDisable();
        _this.filterSearch();
    });

    this.filterDisable = function () {
        _this.filterType.prop("disabled", true);
        _this.filterCount.prop("disabled", true);
        _this.icon.empty();
        _this.icon.hide();
        _this.iconSpinner.fadeIn();

    };

    this.filterEnable = function () {
        _this.filterType.prop("disabled", false);
        _this.filterCount.prop("disabled", false);
        _this.iconSpinner.hide();
        _this.icon.fadeIn();
    };

    this.filterSearch = function () {
        window.doFilter(_this.filterType.val(), _this.filterCount.val());
    }


}
calorie.filter = new calorie.FilterPartial();

