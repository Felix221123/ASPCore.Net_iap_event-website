$(document).ready(function () {
    // Hide error messages initially
    $(".errorStyle").hide();

    // Function to remove error styling when user starts typing
    $("input").on("input", function () {
        $(this).next(".errorStyle").fadeOut();
        $(this).css("border-bottom", "1px solid #ccc"); // Reset border
    });

    // Handle form submission
    $("form").on("submit", function (e) {
        let isValid = true;

        // Check each input field
        $("input[type='text'], input[type='password']").each(function () {
            if ($(this).val().trim() === "") {
                $(this).next(".errorStyle").fadeIn();
                $(this).css("border-bottom", "2px solid red");
                isValid = false;
            }
        });

        // Prevent form submission if any field is empty
        if (!isValid) {
            e.preventDefault();
        }
    });
});
