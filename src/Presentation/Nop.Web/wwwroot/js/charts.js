
    var ctx = document.getElementById('myChart').getContext('2d');
    var ctx2 = document.getElementById('myChart2').getContext('2d');
    // var ctx3 = document.getElementById('myChart3').getContext('2d');
    // var ctx4 = document.getElementById('myChart4').getContext('2d');

    const xValues = [50, 60, 70, 80, 90, 100];
    const yValues = [7, 8, 20, 12, 10, 9, 6];
    var gradient = ctx.createLinearGradient(0, 200, 0, 0);
    gradient.addColorStop(0, 'rgba(61, 0, 94, 0.05');
    gradient.addColorStop(0.1, 'rgba(61, 0, 94,0.1');
    gradient.addColorStop(0.2, 'rgba(61, 0, 94, 0.2');
    gradient.addColorStop(0.3, 'rgba(61, 0, 94, 0.3');
    gradient.addColorStop(0.4, 'rgba(61, 0, 94, 0.4');
    gradient.addColorStop(0.5, 'rgba(61, 0, 94, 0.5');
    gradient.addColorStop(1, 'rgba(61, 0, 94, 0.7');

    var gradient = ctx2.createLinearGradient(0, 200, 0, 0);
    gradient.addColorStop(0, 'rgba(61, 0, 94, 0.05');
    gradient.addColorStop(0.1, 'rgba(61, 0, 94,0.1');
    gradient.addColorStop(0.2, 'rgba(61, 0, 94, 0.2');
    gradient.addColorStop(0.3, 'rgba(61, 0, 94, 0.3');
    gradient.addColorStop(0.4, 'rgba(61, 0, 94, 0.4');
    gradient.addColorStop(0.5, 'rgba(61, 0, 94, 0.5');
    gradient.addColorStop(1, 'rgba(61, 0, 94, 0.7');

    // hoverLine plugin block -work with v 0.4
    // const hoverLine = {
    //     id: 'hoverLine',
    //     afterDatasetsDraw(chart, args, plugins) {
    //         const { ctx, tooltip, chartArea: { top, bottom, left, right,
    //             width, heigth }, scales: { x, y } } = chart;
    //         if (tooltip._active.length > 0) {
    //             const xCoor = x.getPixelForValue(tooltip.dataPoints[0].
    //                 dataIndex);
    //             const yCoor = y.getPixelForValue(tooltip.dataPoints[0].
    //                 parsed.y);
    //             ctx.save();
    //             ctx.beginPath();
    //             ctx.lineWidth =3;
    //             ctx.strokeStyle = 'rgba(0, 0, 0, 1)';
    //             ctx.setLineDash([6, 6])
    //             ctx.moveTo(xCoor, yCoor);
    //             ctx.lineTo(xCoor, bottom);
    //             ctx.stroke();
    //             ctx.closePath();
    //         }
    //     }
    //     }

        var chartOptions = {
            responsive: true,
            datasetStroke: true,
            datasetFill: true,
            legend: {
                display: false
            },
            scales: {

                y: {
                    beginAtZero: true
                },

                yAxes: [{
                    display: false
                }],
                xAxes: [{
                    display: false
                }],
            },
  
        };
        var myChart = new Chart(ctx, {
            type: "line",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: gradient,
                    borderColor: "#3D005E",
                    borderWidth: 2,
                    pointBorderColor:"#b27aff",
                    pointBorderWidth:5,
                    pointColor:"#b27aff",
                    data: yValues
                }]
            },
            options: chartOptions,
            // plugins: [hoverLine]
        });

        var myChart2 = new Chart(ctx2, {
            type: "line",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: gradient,
                    borderColor: "#3D005E",
                    borderWidth: 2,
                      pointBorderColor:"#b27aff",
                    pointBorderWidth:5,
                    pointColor:"#b27aff",
                    data: yValues
                }]
            },
            options: chartOptions,
        });
        var myChart3 = new Chart(ctx3, {
            type: "line",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: gradient,
                    borderColor: "#3D005E",
                    borderWidth: 2,
                      pointBorderColor:"#b27aff",
                    pointBorderWidth:5,
                    pointColor:"#b27aff",
                    data: yValues
                }]
            },
            options: chartOptions,
        });
        var myChart4 = new Chart(ctx4, {
            type: "line",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: gradient,
                    borderColor: "#3D005E",
                    borderWidth: 2,
                      pointBorderColor:"#b27aff",
                    pointBorderWidth:5,
                    pointColor:"#b27aff",
                    data: yValues
                }]
            },
            options: chartOptions,
        });
