
calorie.charts.donutChart = function (elemName, isCurrency, series, labels, total, isAnimated, isCenterLabel, legendNames) {

    this.elemName = elemName;
    this.series = series;
    this.labels = labels;
    this.total = total;
    this.isCurrency = isCurrency;
    this.isCenterLabel= isCenterLabel ;
    this.legendNames = legendNames;
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
                    dx: $('.chart-' + me.elemName).width() / 2, //ctx.element.root().width() / 2,
                    dy: ($('.chart-' + me.elemName).height() / 2) + 10 //(ctx.element.root().height() / 2) - 10
                });

                var splits = ctx.text.split('<BR>');
                if (splits.length > 1) {

                    

                    var percentage = splits[0].replace('%', '');
                   // var clone = ctx.element._node.cloneNode(true);

                    ctx.element._node.removeChild(ctx.element._node.firstChild);
                    var newNode = document.createTextNode(splits[0]);
                    
                    ctx.element._node.appendChild(newNode);
                    $(ctx.element._node).attr('id', 'complete-percent-counter-' + me.elemName);

                    //var currentdy = parseFloat(clone.getAttribute('dy'));
                    //var currentdx = parseFloat(clone.getAttribute('dx'));

                    //clone.setAttribute('dy', currentdy +20);
                    //clone.removeChild(clone.firstChild);

                    //clone.appendChild(document.createTextNode(splits[1]));
                   // ctx.element._node.parentNode.appendChild(clone);
                    //$('counter-' + me.elemName).addClass('complete-percent-counter');

                    var options = {
                        useEasing: true,
                        useGrouping: false,
                        separator: ',',
                        decimal: '.',
                        prefix: '',
                        suffix: '%'
                    };
                    var counter = new CountUp('complete-percent-counter-' + me.elemName, 0, percentage, 0, 3.5, options);
                    counter.start();
                }
            }
        });

    }

    this.init = function () {
        
        var thisPlugins = [
                                Chartist.plugins.legend({
                                clickable: false,
                                legendNames: me.legendNames
                                    })
                              ];
        
        //initialise chart
        me.chart = new Chartist.Pie('.chart-' + me.elemName, {
            series: me.series,
            labels: me.labels

        }, {
            donut: true,
            total: me.total,
            plugins: me.isCenterLabel ?   null : thisPlugins           
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

        //enable centering of label
        if (isCenterLabel) {
            me.initialiseCenterLabel();
        }
        
    }

    
    //run on new
    if (isCurrency) {

        calorie.location.findLocation(function (thisLocation) {           
            //me.series.forEach(function (elem, i) { me.series[i] = calorie.currency.ConvertAmountDirect(elem, "GBP", thisLocation, true) });
            me.labels.forEach(function (elem, i) { me.labels[i] = calorie.currency.ConvertAmountDirect(elem, "GBP", thisLocation) });
            calorie.currency.ConvertAmountDirect(me.total, "GBP", thisLocation, true);
            me.init();
        });
        
    } else {
        this.init();
    }

   
}