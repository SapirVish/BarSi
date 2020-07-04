$(function () {
    $('#filterButton').on('click', function () {
        $('#filter-cart-icon').toggleClass("fa-chevron-circle-up");
        $('#filter-cart-icon').toggleClass("fa-chevron-circle-down");
    });

    var searchForm = $('#searchForm');
    var url = searchForm.attr('action');

    $('#doctorsSearchButton').on('click', function () {
        $.ajax({
            method: "POST",
            url: url,
            data: searchForm.serialize(),
            success: function (data) {
                $('tbody').html('');
                $('#doctorsResults').tmpl(data).appendTo('tbody');
            },
            error: function (data) {
                alert("error!");
            }
        });
    });
});
