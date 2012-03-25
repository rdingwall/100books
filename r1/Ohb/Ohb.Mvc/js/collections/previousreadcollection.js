Ohb.Collections.PreviousReadCollection = (function ($, _, Backbone, PreviousRead, eventBus) {
    "use strict";

    return Backbone.Collection.extend({
        model: PreviousRead,
        url: "/api/v1/previousreads",

        initialize: function () {
            // todo: fix memory leak (unsubscribe this handler)
            eventBus.on("previousread:removed", this.onPreviousReadRemoved, this);
        },

        onPreviousReadRemoved: function (id) {
            var model = this.get(id);
            this.remove(model);
        }
    });

}($, _, Backbone, Ohb.Models.PreviousRead, Ohb.eventBus));