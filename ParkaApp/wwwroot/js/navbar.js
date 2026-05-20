$(document).ready(function () {

    $("#menuButton").click(function () {

        $("#floatingMenu").toggleClass("show");
        $("#floatingMenu").toggleClass("hidden");

        $(this).toggleClass("menu-open");

    });

});
