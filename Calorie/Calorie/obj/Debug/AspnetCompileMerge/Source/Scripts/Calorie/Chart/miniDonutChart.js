
calorie.charts.miniDonutChart = function (elemName, size, series, labels, total, isAnimated, legendNames) {

    this.elemName = elemName;
    this.series = series;
    this.labels = labels;
    this.total = total;
    this.legendNames = legendNames;
    this.size = size;
    this.chart = null;
    var me = this;


    this.initialiseAnimation= function()
    {
       
        //animation
        me.chart.on('draw', function (data) {
            if (data.type === 'slice' && Modernizr.smil) {
                var pathLength = data.element._node.getTotalLength();

                data.element.attr({
                    'stroke-dasharray': pathLength + 'px ' + pathLength + 'px'
                });

                var animationDefinition = {
                    'stroke-dashoffset': {
                        id: 'anim' + data.index,
                        dur: 500,
                        from: -pathLength + 'px',
                        to: '0px',
                        easing: Chartist.Svg.Easing.easeInOutCirc,
                        fill: 'freeze'
                    }
                };

                if (data.index !== 0) {
                    animationDefinition['stroke-dashoffset'].begin = 'anim' + (data.index - 1) + '.end';
                }

                data.element.attr({
                    'stroke-dashoffset': -pathLength + 'px'
                });

                data.element.animate(animationDefinition, false);
            }
        });

        
        me.chart.on('created', function () {
            if (Modernizr.smil) {
                me.chart.update.bind(me.chart);

            }

        });
        
    }

    this.initialiseCenterLabel = function() {

        me.chart.on('draw', function(ctx) {

            if (ctx.type === 'label') {

                // If this is our done label, we position it into the center of the chart, otherwise we remove all labels
                ctx.element.attr({
                    dx: me.size/2.0 ,
                    dy: (me.size / 2.0) + 4.0
                });

                var splits = ctx.text.split('<BR>');
                if (splits.length>1){

                    ctx.element._node.removeChild(ctx.element._node.firstChild);
                    ctx.element._node.appendChild(document.createTextNode(splits[0]));

                }
            }
        });

    }

    this.init = function () {
        
       
        //initialise chart
        me.chart = new Chartist.Pie('.chart-' + me.elemName, {
            series: me.series,
            labels: me.labels

        }, {
               donut: true,
               total: me.total,
               chartPadding: 15,
               startAngle: 180,
               width: me.size,
               height: me.size
            }
        );
        
        //enable animation if needed
        if (isAnimated) {
            me.initialiseAnimation();
        }

        //enable centering of label        
       me.initialiseCenterLabel();
        
        
    }
    
    //run on new
    this.init();
    
}