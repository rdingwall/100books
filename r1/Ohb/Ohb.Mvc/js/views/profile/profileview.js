$(function () {

    "use strict";

    var Ohb = window;

    Ohb.ProfileView = (function ($, Backbone, _, Mustache,
                                     eventBus, Profile) {

        var log = $.jog("ProfileView");

        return Backbone.View.extend({

            el: "#content-main",

            initialize: function () {
                eventBus.on("profile:requested", this.onProfileRequested, this);
            },

            onProfileRequested: function (id) {
                log.info("Fetching user from API...");
                this.model = new Profile({ id: id });
                this.model.fetch({
                    success: _.bind(this.render, this),
                    error: _.bind(this.onFetchError, this)
                });
            },

            render: function () {
                log.info("Successfully fetched user. Rendering.");

                $.get("/templates/profile/profile.html", "text",
                    _.bind(function (template) {
                        var el = $(Mustache.to_html(template, this.model.toJSON()));
                        $(this.el).html(el);
                        $(this.el).show();
                        eventBus.trigger("profile:rendered", this.model);
                    }, this));

                return this;
            },

            onFetchError: function () {
                log.warning("Error loading user");

                $.get("/templates/profile/fetcherror.html", "text",
                    _.bind(function (template) {
                        $(this.el).html(template);
                        eventBus.trigger("profile:fetchError");
                    }, this));
            }
        });

    }(
        $,
        Backbone,
        _,
        Mustache,
        Ohb.eventBus,
        Ohb.Profile
    ));
});