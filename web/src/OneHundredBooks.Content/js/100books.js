$(document).ready(function() {

    // Close menus whenever anyone clicks outside them
    $("body").click(function(e) {
        if ($(e.target).is("div.menu-container *"))
            return; // Clicked inside menu, keep it open

        $("ul.menu").removeClass("visible");
    });

    // Show/hide menus
    $("div.menu-container > button").click(function() {
        var menu = $(this).siblings("ul.menu");

        if (menu.hasClass("visible"))
            menu.removeClass("visible");
        else {
            // First close any other menu that might still be open :)
            $("ul.menu").removeClass("visible"); 
            menu.addClass("visible");
        }
    });

});