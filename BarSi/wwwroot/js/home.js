$(function () {
    $.ajax({
        method: "GET",
        url: "https://api.covid19api.com/summary",
        success: function (data) {
            var globalReasults = data["Global"];
            globalReasults["Country"] = "Global";
            var results = [globalReasults]

            for (const country of data["Countries"])
            {
                results.push(country);
            }
            $('tbody').html('');
            $('#global-covid19-data-results').tmpl(results).appendTo('tbody');

            
        },
        error: function (data) {
            alert("error!");
        }
    });
});