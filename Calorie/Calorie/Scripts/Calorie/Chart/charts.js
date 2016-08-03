
calorie.charts = {}

calorie.charts.chartToggleButtons = function (barDiv, pieDiv, barBtn, pieBtn ) {

    this.barDiv = barDiv;
    this.barBtn = barBtn;
    
    this.pieDiv = pieDiv;
    this.pieBtn = pieBtn;  

    var _this = this;


    this.showBar = function() {
        
        _this.barDiv.find(".ct-chart").hide();
        _this.barDiv.find(".ct-chart").show();

        _this.pieDiv.hide();
        _this.barDiv.fadeIn();

    }

    this.showPie = function() {
        
        _this.pieDiv.find(".ct-chart").hide();
        _this.pieDiv.find(".ct-chart").show();

        _this.barDiv.hide();
        _this.pieDiv.fadeIn();

    }


    this.barBtn.click(function() {
        _this.showBar();
    });
    this.pieBtn.click(function() {
        _this.showPie();
    });
    
}
