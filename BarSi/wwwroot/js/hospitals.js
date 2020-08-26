$(function () {
    // Getting the current URL
    var url = window.location.href.toString();
    var originalUrl = url.split("?")[0];

    // Checking if an order was just made
    if (url.includes("?")) {
        var currHospitalId = originalUrl.substring(originalUrl.lastIndexOf('/') + 1);

        // Attempt to suggest an order based on the current situation
        suggestOrder(currHospitalId, originalUrl);
    }

    // Initializing the supply Modal
    initSupplyModal();
});

initSupplyModal = function () {
    // Add validation to the modal
    (function () {
        'use strict';
        window.addEventListener('load', function () {
            // Fetch the order form
            var form = $("#OrderForm")

            // Prevent submission
            var validation = Array.prototype.filter.call(form, function (f) {
                f.classList.remove('was-validated');

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

    // Display the current quantity of the selected equipment (and set max value)
    $('#SupplyEquipment').on('change', function () {
        var selected = $("#SupplyEquipment :selected")

        $.ajax({
            url: "/Hospitals/AvailableSupply/" + selected.val(),
            cache: false
        }).done(function (data) {
            $("#SupplyAvailable").text("There are currently " + (data != 0 ? data : "none") + " available");
            $("#SupplyQuantity").attr('max', data.toString());
        });
    });
};

orderSupply = function (form) {
    var url = "/Hospitals/Order";
    var postData = form.serialize();

    $.ajax({
        url: url,
        method: "POST",
        data: postData,
        async: false,
        success: function (data) {
            console.log("Success: " + data.hospitalId);
        },
        error: function (data) {
            console.log("Error: " + (data.hospitalId).toString());
           alert("Error: " + data);
        }
    }).done(function (data) {
        console.log("DONE " + data.hospitalId);
        //alert("DONE");
    });
};

suggestOrder = function (currHospitalId, url) {
    // Ask the users if they want to keep "shopping"
    $.ajax({
        url: "/Hospitals/SuggestOrder/" + currHospitalId,
        cache: false,
        success: function (data) {
            initSuggestionModal(data, url);
            //alert("success: " + data.equipmentName);       
        },
        error: function (data) {
            console.log("error: " + data);
            navigateToUrl(url);
        }
    });
}

initSuggestionModal = function (data, url) {
    // Initialize the order suggestion modal
    var suggestionModal = $("#SuggestionModal");
    $("#SuggestEquipment").text("Would you like to proceed and order some " + data.equipmentName + " supplies?");
    $("#CurrSuggestionState").text("You currently have " + data.currentQuantity + " and there's " + data.availableSupply + " available!");
    suggestionModal.modal("show");

    // When clicking "Yes" open an order modal with the relevant equipment selected
    $("#OrderSuggested").on("click", function () {
        suggestionModal.modal("hide");

        var equipmentSelect = $('#SupplyEquipment');
        equipmentSelect.val(data.equipmentId);
        equipmentSelect.change()

        $("#SupplyModal").modal("show");
    });

    // When clicking "No" go back to the details page
    $("#SuggesttionDeclined").on("click", function () {
        navigateToUrl(url);
    });
}

navigateToUrl = function (url) {
    document.location.href = url;
}