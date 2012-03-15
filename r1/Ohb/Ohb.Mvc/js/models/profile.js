$(function () {

    "use strict";

    Ohb.Models.Profile = (function (Backbone) {

        return Backbone.Model.extend({
            urlRoot: "/api/v1/profiles",
            defaults : {
                imageUrl: null,
                name: ""
            }
        });

    }(Backbone));
});