Ohb.Models.Profile = (function (Backbone) {
    "use strict";

    return Backbone.Model.extend({
        urlRoot: "/api/v1/profiles",
        defaults : {
            imageUrl: null,
            name: ""
        }
    });

}(Backbone));