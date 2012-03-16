// A simple region manager for controlling the 'main region'
Ohb.mainRegion = (function ($, _, Backbone, ErrorMessage,
                            ErrorMessageView) {

    "use strict";

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
            var model = new ErrorMessage({ message: message });
            this.show(new ErrorMessageView({ model: model }));
        }
    }, Backbone.Events);
}($, _, Backbone, Ohb.Models.ErrorMessage, Ohb.Views.ErrorMessageView));