// A simple region manager for controlling the 'main region'
Ohb.mainRegion = (function ($, _, Backbone) {

    "use strict";

    var log = $.jog("MainRegion");

    return _.extend({
        el: "#main-region",

        show: function (view) {
            this.close();
            this.view = view;
            $(this.el).append(this.view.render().el);
            this.trigger("view:changed");
        },

        close: function () {
            if (!this.view) {
                return;
            }

            if (this.view.close) {
                this.view.close();
            }
            this.view = null;
            $(this.el).empty();
        },

        showError: function (message) {
            log.severe(message);
            // todo: render generic error view here
        }
    }, Backbone.Events);
}($, _, Backbone));