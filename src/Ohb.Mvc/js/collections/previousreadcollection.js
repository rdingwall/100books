Ohb.Collections.PreviousReadCollection = (function ($, _, Backbone, PreviousRead, eventBus) {
    "use strict";

    return Backbone.Collection.extend({
        model: PreviousRead,
        url: "/api/v1/previousreads",

        initialize: function () {
            eventBus.on("previousread:removed", this.onPreviousReadRemoved, this);
        },

        onPreviousReadRemoved: function (id) {
            var model = this.get(id);
            this.remove(model);
        },

        unbindEvents: function () {
            eventBus.off("previousread:removed", this.onPreviousReadRemoved, this);
        }
    });

}($, _, Backbone, Ohb.Models.PreviousRead, Ohb.eventBus));