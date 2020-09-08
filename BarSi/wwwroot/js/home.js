$(function () {
    $.ajax({
        method: "GET",
        url: "https://api.covid19api.com/summary",
        success: function (data) {
            var globalReasults = data["Global"];
            globalReasults["Country"] = "Global";
            var results = [globalReasults]

            for (const country of data["Countries"]) {
                results.push(country);
            }
            $('tbody').html('');
            $('#global-covid19-data-results').tmpl(results).appendTo('tbody');


        },
        error: function (data) {
            alert("error!");
        }
    });

    $.ajax({
        method: "GET",
        url: "/Hospitals/PatientsByHospitalCount",
        success: function (data) {
            createPatientsByHospitalGraph(data);
        },
        error: function (data) {
            alert("error!");
        }
    });

    $.ajax({
        method: "GET",
        url: "Doctors/PatientsForDoctorStatistic",
        success: function (data) {
            createPatientsForDoctorStatisticGraph(data);
        },
        error: function (data) {
            alert("error!");
        }
    });
});

function createPatientsByHospitalGraph(data) {
    height = 500;
    width = 500;

    margin = ({ top: 30, right: 0, bottom: 30, left: 40 })

    x = d3.scaleBand()
        .domain(data.map(d => d.hospitalName))
        .rangeRound([margin.left, width - margin.right])
        .padding(0.1)

    y = d3.scaleLinear()
        .domain([0, d3.max(data, d => d.count)]).nice()
        .range([height - margin.bottom, margin.top])


    xAxis = g => g
        .attr("transform", `translate(0,${height - margin.bottom})`)
        .call(d3.axisBottom(x).tickFormat(i => i).tickSizeOuter(0))

    yAxis = g => g
        .attr("transform", `translate(${margin.left},0)`)
        .call(d3.axisLeft(y).ticks(null, data.format))
        .call(g => g.select(".domain").remove())
        .call(g => g.append("text")
            .attr("x", -margin.left)
            .attr("y", 10)
            .attr("fill", "currentColor")
            .attr("text-anchor", "start"));


    const svg = d3.select("#patients-by-hospital-graph")
        .attr("viewBox", [0, 0, width, height]);

    svg.append("g")
        .attr("fill", "cadetblue")
        .selectAll("rect")
        .data(data)
        .join("rect")
        .attr("x", d => x(d.hospitalName))
        .attr("y", d => y(d.count))
        .attr("height", d => y(0) - y(d.count))
        .attr("height", d => y(0) - y(d.count))
        .attr("width", x.bandwidth());

    svg.append("g")
        .call(xAxis);

    svg.append("g")
        .call(yAxis);
}

function createPatientsForDoctorStatisticGraph(patientsForDoctorData) {

    var width = 400
    height = 400
    margin = 30

    // The radius of the pieplot is half the width or half the height (smallest one). I subtract a bit of margin.
    var radius = Math.min(width, height) / 2 - margin

    // append the svg object to the div called 'patients-by-doctor-graph'
    var svg = d3.select("#patients-by-doctor-graph")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");


    // set the color scale
    var color = d3.scaleOrdinal()
        .domain(patientsForDoctorData)
        .range(d3.schemeSet2);

    // Compute the position of each group on the pie:
    var pie = d3.pie()
        .value(function (d) {
            //  console.log("pie: ");
            //  console.log(d);
            return d.value.patientsCount;
        })
    var data_ready = pie(d3.entries(patientsForDoctorData))
    // Now I know that group A goes from 0 degrees to x degrees and so on.

    // shape helper to build arcs:
    var arcGenerator = d3.arc()
        .innerRadius(0)
        .outerRadius(radius)

    // Build the pie chart: Basically, each part of the pie is a path that we build using the arc function.
    svg
        .selectAll('mySlices')
        .data(data_ready)
        .enter()
        .append('path')
        .attr('d', arcGenerator)
        .attr('fill', function (d) {
            // console.log("color: ");
            // console.log(d);
            return (color(d.data.value.doctorName))
        })
        .attr("stroke", "black")
        .style("stroke-width", "2px")
        .style("opacity", 0.7)

    // Now add the annotation. Use the centroid method to get the best coordinates
    svg
        .selectAll('mySlices')
        .data(data_ready)
        .enter()
        .append('text')
        .text(function (d) {
            //console.log("last: ");
            // console.log( d);
            return d.data.value.doctorName
        })
        .attr("transform", function (d) { return "translate(" + arcGenerator.centroid(d) + ")"; })
        .style("text-anchor", "middle")
        .style("font-size", 17)

}