$(document).ready(function() {

    // Set up menu show/hide business
    $("div.menu-container > button").click(function() {
        var container = $(this).parents("div.menu-container");
        var menu = $(this).siblings("ul.menu");

        if (menu.hasClass("visible")) {
            menu.removeClass("visible");
        }
        else {
            menu.addClass("visible");

            function waitForClickOutside(container) {
                $("body").one("mousedown", function(e) {
                    var c = container;
                    if ($(e.target).is("div.menu-container *")) {
                        // Click was inside the menu. Ignore.
                        waitForClickOutside(c);
                        return;
                    }

                    c.children("ul.menu").removeClass("visible");
                });
            };
            
            waitForClickOutside(container);
        }
    });
    
});