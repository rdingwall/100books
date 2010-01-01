jQuery.fn.hasATooltip = function() {

    $(this).tooltip({
        position: "bottom center",
        events: {
            def: "mouseover, mouseout",
            input: "mouseover, mouseout"
        },
    });
};

$(document).ready(function() {

    // Close menus whenever anyone clicks outside them
    $("body").click(function(e) {
        if ($(e.target).is("div.menu-container *"))
            return; // Clicked inside menu, keep it open

        $("div.menu-container").removeClass("open");
    });

    // Show/hide menus
    $("div.menu-container > button").click(function() {
        var container = $(this).parents("div.menu-container");

        if (container.hasClass("open"))
            container.removeClass("open");
        else {
            // First close any other menu that might still be open :)
            $("div.menu-container").removeClass("open");
            container.addClass("open");
        }
    });

    $("div.blank-list input.title:first").hasATooltip();
    $("div.blank-list div.search-results:first").hasATooltip();
});

