$(function () {

    "use strict";

    var template = '<div class="profile" xmlns="http://www.w3.org/1999/html">\
                <div class="row span12">\
                <img src="{{ profileImageUrl }}" alt="{{ displayName }}" class="pull-left" />\
                <h1 class="profile-display-name" style="line-height: 50px; margin-left: 60px;">{{ displayName }}</h1>\
        </div>\
    </div>';

    Ohb.Views.MyProfileView = (function ($, Backbone, _, Mustache,
                                     template) {

        var log = $.jog("MyProfileView");

        return Backbone.View.extend({
            className: "my-profile",

            render: function () {
                log.info("Successfully fetched user. Rendering.");

                var el = $(Mustache.to_html(template, this.model.toJSON()));
                $(this.el).html(el);

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