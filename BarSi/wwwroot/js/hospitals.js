//var equipmentAvailableSupply = [][];

$(function () {
    initSupplyModal();

    //$('#SupplyEquipment').on('change', function () {

    //    var selected = $("#SupplyEquipment :selected")
    //    alert(selected.val() + "=" + selected.text())
    //    $('#SupplyQuantity').attr('max', '5')
    //});
});

initSupplyModal = function () {

    (function () {
        'use strict';
        window.addEventListener('load', function () {
            // Fetch the order form
            var form = $("#OrderForm")

            // Prevent submission
            var validation = Array.prototype.filter.call(form, function (f) {
                f.addEventListener('submit', function (event) {
                    if (f.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    else {
                        orderSupply(form);
                    }

                    f.classList.add('was-validated');
                }, false);
            });
        }, false);
    })();

    //$("#SupplyModal").on("submit", "#OrderForm", function (e) {
    //    e.preventDefault();

    //    if (e.checkValidity()) {
    //        orderSupply(e);
    //    }
    //});

    $('#SupplyEquipment').on('change', function () {
        var selected = $("#SupplyEquipment :selected")
        //alert(selected.val() + "=" + selected.text())

        //var equipmentId = $(this).data('EquipmentId');
        $.ajax({
            url: "/Hospitals/AvailableSupply/" + selected.val(),
            cache: false
        }).done(function (data) {
            $("#SupplyAvailable").text("There are currently " + data + " available");
            $("#SupplyQuantity").attr('max', data.toString());
        });

        //$('#OrderForm').validate({
        //    rules: {
        //        EquipmentId: { required: true },
        //        Quantity: { required: true, min: random },
        //    },
        //    messages: {
        //        Quantity: "Hi"
        //    },
        //    submitHandler: function (form) {
        //        orderSupply($(form));
        //    }
        //});
    });
};

orderSupply = function (form) {

    //$(".btn.mylink").on("click", function () {
    //    var articleId = $(this).data('id');
    //    $.ajax({
    //        url: "/article/get/" + articleId,
    //        cache: false
    //    }).done(function (data) {
    //        $("#Title").val(data.Title);
    //        $("#Summary").val(data.Summary);
    //        $("#Magazine").val(data.Magazine);
    //        $("#Url").val(data.Url);
    //        $("#PubMonth").val(data.Year.toString() + "-" +
    //            ("00" + data.Month.toString()).slice(-2));
    //    });
    //});

    var url = "/Hospitals/Order";
    var postData = form.serialize();

    $.ajax({
        url: url,
        method: "POST",
        data: postData,
        success: function (data) {
            console.log("Success: " + data);
            alert("Success: " + data);
            document.location.href = '/Hospitals/Details/' + data;
        },
        error: function (data) {
            console.log("Error: " + data);
            alert("Error: " + data);
        }
    }).done(function (data) {
        console.log("DONE");
        alert("DONE");
    });

    //alert("End");

};