
calorie.charts.lineChart = function(elemName, series,labels,legendNames) {

    this.elemName = elemName;
    this.series = series;
    this.labels = labels;
    this.legendNames = legendNames;

    this.seq = 0;
    this.delays = 50;
    this.durations = 300;

    this.chart = null;
    var me = this;
    
    this.initialiseAnimation= function()
    {
      
        // Let's put a sequence number aside so we can use it in the event callbacks
        //me.seq = 0,
          //delays = 50,
          //durations = 300;

        // Once the chart is fully created we reset the sequence
        me.chart.on('created', function () {
            me.seq = 0;
        });

        // On each drawn element by Chartist we use the Chartist.Svg API to trigger SMIL animations
        me.chart.on('draw', function (data) {
            me.seq++;

            if (data.type === 'line') {
                // If the drawn element is a line we do a simple opacity fade in. This could also be achieved using CSS3 animations.
                data.element.animate({
                    opacity: {
                        // The delay when we like to start the animation
                        begin: me.seq * me.delays + 1000,
                        // Duration of the animation
                        dur: me.durations,
                        // The value where the animation should start
                        from: 0,
                        // The value where it should end
                        to: 1
                    }
                });
            }
            else if (data.type === 'label' && data.axis === 'x') {
                data.element.animate({
                    y: {
                        begin: me.seq * me.delays,
                        dur: me.durations,
                        from: data.y + 100,
                        to: data.y,
                        // We can specify an easing function from Chartist.Svg.Easing
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'label' && data.axis === 'y') {
                data.element.animate({
                    x: {
                        begin: me.seq * me.delays,
                        dur: me.durations,
                        from: data.x - 100,
                        to: data.x,
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'point') {
                data.element.animate({
                    x1: {
                        begin: me.seq * me.delays,
                        dur: me.durations,
                        from: data.x - 10,
                        to: data.x,
                        easing: 'easeOutQuart'
                    },
                    x2: {
                        begin: me.seq * me.delays,
                        dur: me.durations,
                        from: data.x - 10,
                        to: data.x,
                        easing: 'easeOutQuart'
                    },
                    opacity: {
                        begin: me.seq * me.delays,
                        dur: me.durations,
                        from: 0,
                        to: 1,
                        easing: 'easeOutQuart'
                    }
                });
            } else if (data.type === 'grid') {
                // Using data.axis we get x or y which we can use to construct our animation definition objects
                var pos1Animation = {
                    begin: me.seq * me.delays,
                    dur: me.durations,
                    from: data[data.axis.units.pos + '1'] - 30,
                    to: data[data.axis.units.pos + '1'],
                    easing: 'easeOutQuart'
                };

                var pos2Animation = {
                    begin: me.seq * me.delays,
                    dur: me.durations,
                    from: data[data.axis.units.pos + '2'] - 100,
                    to: data[data.axis.units.pos + '2'],
                    easing: 'easeOutQuart'
                };

                var animations = {};
                animations[data.axis.units.pos + '1'] = pos1Animation;
                animations[data.axis.units.pos + '2'] = pos2Animation;
                animations['opacity'] = {
                    begin: me.seq * me.delays,
                    dur: me.durations,
                    from: 0,
                    to: 1,
                    easing: 'easeOutQuart'
                };

                data.element.animate(animations);
            }
        });
      
        
    }

    this.init = function () {
       
        me.chart = new Chartist.Line('.chart-' + me.elemName, {

            series:  me.series,
            labels:  me.labels


        },  {
            fullWidth: true,
            low: 0,        
            height:300,        
            plugins: [
                        Chartist.plugins.legend({
                            clickable: false,
                            legendNames: me.legendNames
                            })                    
                    ]
        }
        );
       
        me.initialiseAnimation();
       
    }

    
    //run on new
    this.init();
    
}