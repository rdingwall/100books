$(function () {
    "use strict";
    Ohb.Views.ReadableView = (function (
        Backbone
    ) {
        return Backbone.View.extend({
            className: "dummy-readable",

            initialize: function () {
                this.model.on("change:hasPreviouslyRead",
                    this.onModelStatusChanged, this);
            },

            close: function () {
                this.model.off("change:hasPreviouslyRead",
                    this.onModelStatusChanged, this);
            },

            onModelStatusChanged: function () {
                this.updateToggleButton(this.$el);
            },

            toggleStatus: function (event) {
                event.preventDefault();
                this.model.toggleStatus();
            }
        });
    }(Backbone));
});
