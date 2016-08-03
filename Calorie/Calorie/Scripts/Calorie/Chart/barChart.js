
calorie.charts.barChart = function(elemName, isCurrency, series, legendNames,isAnimated) {

    this.elemName = elemName;
    this.series = series;
    this.isCurrency = isCurrency;
    this.legendNames = legendNames;
    this.chart = null;
    var me = this;

    this.initialiseAnimation= function()
    {
        
        var seq = 0,
          delays = 50,
          durations = 300;

        me.chart.on('created', function () {
            seq = 0;
        });

        me.chart.on('draw', function (data) {
            seq++;

            if (data.type === 'bar') {
                data.element.animate({
                    y2: {
                        dur: 1000,
                        from: data.y1,
                        to: data.y2,
                        easing: Chartist.Svg.Easing.easeOutQuint
                    },
                    opacity: {
                        dur: 1000,
                        from: 0,
                        to: 1,
                        easing: Chartist.Svg.Easing.easeOutQuint
                    }
                });
            } else if (data.type === 'label' && data.axis === 'x') {
                data.element.animate({
                    y: {
                        begin: seq * delays,
                        dur: durations,
                        from: data.y + 100,
                        to: data.y,
                        // We can specify an easing function from Chartist.Svg.Easing
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'label' && data.axis === 'y') {
                data.element.animate({
                    x: {
                        begin: seq * delays,
                        dur: durations,
                        from: data.x - 100,
                        to: data.x,
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'point') {
                data.element.animate({
                    x1: {
                        begin: seq * delays,
                        dur: durations,
                        from: data.x - 10,
                        to: data.x,
                        easing: 'easeOutQuart'
                    },
                    x2: {
                        begin: seq * delays,
                        dur: durations,
                        from: data.x - 10,
                        to: data.x,
                        easing: 'easeOutQuart'
                    },
                    opacity: {
                        begin: seq * delays,
                        dur: durations,
                        from: 0,
                        to: 1,
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'grid') {
                var pos1Animation = {
                    begin: seq * delays,
                    dur: durations,
                    from: data[data.axis.units.pos + '1'] - 30,
                    to: data[data.axis.units.pos + '1'],
                    easing: 'easeOutQuart'
                };

                var pos2Animation = {
                    begin: seq * delays,
                    dur: durations,
                    from: data[data.axis.units.pos + '2'] - 100,
                    to: data[data.axis.units.pos + '2'],
                    easing: 'easeOutQuart'
                };

                var animations = {};
                animations[data.axis.units.pos + '1'] = pos1Animation;
                animations[data.axis.units.pos + '2'] = pos2Animation;
                animations['opacity'] = {
                    begin: seq * delays,
                    dur: durations,
                    from: 0,
                    to: 1,
                    easing: 'easeOutQuart'
                };

                data.element.animate(animations);
            }
        });
   }

    this.init = function () {

        //initialise chart
        me.chart = new Chartist.Bar('.chart-' + me.elemName, {
            labels: [""],
            series: me.series

        }, {
            seriesBarDistance: 40,
            plugins: [
                    Chartist.plugins.legend({
                        clickable: false,
                        legendNames: me.legendNames
                        })    
                    ]
            }
        );

        //configure mutation observer
        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function () {
                me.chart.update();
            });
        });
        var target = document.getElementById(me.elemName);
        observer.observe(target, { attributes: true, attributeFilter: ['style'] });

        //enable animation if needed
        if (isAnimated) {
            me.initialiseAnimation();
        }
        
    }

    
    //run on new
    if (isCurrency) {

        calorie.location.findLocation(function (thisLocation) {           
            me.series[0].forEach(function (elem, i) { me.series[0][i] = calorie.currency.ConvertAmountDirect(elem, "GBP", thisLocation, true) });
            me.init();
        });
        
    } else {
        this.init();
    }

   
}