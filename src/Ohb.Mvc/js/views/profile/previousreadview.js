Ohb.Views.PreviousReadView = (function (
    Backbone,
    Mustache
) {
    "use strict";

    var template = '<div class="row"> \
        <div class="span2"> \
            <img class="previous-read-thumbnail" src="{{ smallThumbnailUrl }}" alt="{{ title }}" /> \
        </div> \
        <div class="span8"> \
            <h3 class="previous-read-title"><a href="{{ viewUrl }}">{{ title }}</a></h3> \
            <p class="previous-read-authors">{{ authors }}</p> \
        </div> \
        <div class="span2"> \
            <button class="btn btn-inverse btn-remove-previousread">Remove</button>\
        </div>\
    </div>';

    return Backbone.View.extend({
        className: "previous-read",

        events: {
            "click .btn-remove-previousread" : "remove"
        },

        render: function () {
            this.$el.html(Mustache.to_html(template, this.model.toJSON()));
            this.$el.attr("id", "previous-read-" + this.model.id);
            return this;
        },

        remove: function () {
            this.model.remove();
        },

        close: function () {
            this.$el.slideUp("slow", _.bind(function () {
                this.$el.remove();
            }, this));
        }
    });
}(Backbone, Mustache));
