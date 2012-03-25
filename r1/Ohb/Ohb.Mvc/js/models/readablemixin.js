Ohb.Models.ReadableMixin = (function (eventBus) {
    "use strict";

    return {
        bindReadableEvents: function () {
            eventBus.on("previousread:added", function (id) {
                if (this.id === id) {
                    this.set("hasPreviouslyRead", true);
                }
            }, this);

            eventBus.on("previousread:removed", function (id) {
                if (this.id === id) {
                    this.set("hasPreviouslyRead", false);
                }
            }, this);
        },

        toggleStatus: function () {
            var previouslyRead = this.get("hasPreviouslyRead");
            this.set({ hasPreviouslyRead: !previouslyRead });

            var event = previouslyRead ?
                    "previousread:removeRequested" :
                    "previousread:addRequested";

            eventBus.trigger(event, this.id);
        }
    };

}(Ohb.eventBus));
