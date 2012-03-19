$(function () {

    "use strict";

    var template = '<div class="row span12">\
        <img src="{{ profileImageUrl }}" alt="{{ displayName }}" class="pull-left" />\
        <h1 class="profile-card-display-name" style="line-height: 50px; margin-left: 60px;">{{ displayName }}</h1>\
    </div>';

    Ohb.Views.ProfileCardView = (function ($, Backbone, _, Mustache,
                                     template) {

        var log = $.jog("ProfileCardView");

        return Backbone.View.extend({
            className: "profile-card",

            render: function () {
                log.info("Rendering ProfileCardView.");

                var el = $(Mustache.to_html(template, this.model.toJSON()));
                this.$el.html(el);

                return this;
            }
        });
    }(
        $,
        Backbone,
        _,
        Mustache,
        template
    ));
});