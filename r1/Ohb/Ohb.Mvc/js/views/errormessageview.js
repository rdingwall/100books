Ohb.Views.ErrorMessageView = (function ($, Backbone, Mustache) {

    "use strict";

    var template = '<p>{{message}}</p>';

    return Backbone.View.extend({
        className: "error-message",

        render: function () {
            var el = $(Mustache.to_html(template, this.model.toJSON()));
            this.$el.html(el);

            return this;
        }
    });
}(
    $,
    Backbone,
    Mustache
));