
let photoBytes = null;
$(document).ready(function () {

    // AJAX: Load Cities on State Change
    $("#StateId").change(function () {
        var stateId = $(this).val();
        $("#CityId").empty().append('<option value="">--Select City--</option>');

        if (stateId) {
            $.ajax({
                url: '/User/GetCities',
                type: 'GET',
                data: { stateId: stateId },
                success: function (data) {
                    $.each(data, function (i, city) {
                        $("#CityId").append(`<option value="${city.id}">${city.name}</option>`);
                    });
                }
            });
        }

       

        // Read image file and convert to base64 on file input change
        $("#Photo").on("change", function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    photoBytes = e.target.result.split(',')[1]; // Strip the base64 header
                };
                reader.readAsDataURL(file);
            }
        });
    });

    // Client-side Validation
    $("#userForm").submit(function (e) {
        let valid = true;
        let contact = $("#ContactNumber").val().trim();
        let email = $("#Email").val().trim();
        let termsChecked = $("#IsTermsAccepted").is(":checked");


        // Check Contact or Email required
        if (contact === "" && email === "") {
            alert("Either Email or Contact Number is required.");
            valid = false;
        }


        if (valid && !termsChecked) {
            $("#termsError").text("Please agree to Terms and Conditions.");
            valid = false;
        } else {
            $("#termsError").text("");
        }

        if (!valid)
            e.preventDefault();
    });
});

