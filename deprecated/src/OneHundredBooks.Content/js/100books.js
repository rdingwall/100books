jQuery.fn.hasTooltip = function() {
    $(this).tooltip({
        position: "bottom center",
        events: {
            input: "mouseover, mouseout"
        }
    });
};

// Close menus whenever anyone clicks outside them
jQuery.fn.closesMenusOnClick = function() {
    $(this).click(function(e) {
        if ($(e.target).is("div.menu-container *"))
            return; // Clicked inside menu, keep it open

        $("div.menu-container").removeClass("ui-state-active");
    });
};

// Show/hide menus
jQuery.fn.opensMenu = function() {
    $(this).click(function() {
        var container = $(this).parents("div.menu-container");

        if (container.hasClass("ui-state-active"))
            container.removeClass("ui-state-active");
        else {
            // First close any other menu that might still be open :)
            $("div.menu-container").removeClass("ui-state-active");
            container.addClass("ui-state-active");
        }
    });
};

$(document).ready(function() {
    $("div.menu-container > button").opensMenu();
    $("body").closesMenusOnClick();
    $("div.blank-list input.title:first").hasTooltip();
    $("div.blank-list div.search-results:first").hasTooltip();
});
