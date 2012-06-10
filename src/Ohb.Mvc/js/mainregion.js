// A simple region manager for controlling the 'main region'
Ohb.mainRegion = (function ($, _, Backbone, ErrorMessage,
                            ErrorMessageView, eventBus) {

    "use strict";

    var log = $.jog("MainRegion");

    return {
        el: "#main-region",

        show: function (view) {
            this.close();
            this.view = view;
            log.info("Rendering view...");
            $(this.el).append(this.view.render().el);
            eventBus.trigger("mainRegion:view:changed", view);
        },

        close: function () {
            if (!this.view) {
                return;
            }

            log.info("Closing previous view");

            if (this.view.close) {
                this.view.close();
            }
            this.view = null;
            $(this.el).empty();
        },

        showError: function (message) {
            var model = new ErrorMessage({ message: message });
            this.show(new ErrorMessageView({ model: model }));
        }
    };
}(
    $,
    _,
    Backbone,
    Ohb.Models.ErrorMessage,
    Ohb.Views.ErrorMessageView,
    Ohb.eventBus
));