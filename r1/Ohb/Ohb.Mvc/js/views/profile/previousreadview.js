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
            <h3 class="previous-read-title">{{ title }}</h3> \
            <p class="previous-read-authors">{{ authors }}</p> \
        </div> \
    </div>';

    return Backbone.View.extend({
        className: "previous-read",

        render: function () {
            this.$el.html(Mustache.to_html(template, this.model.toJSON()));
            this.$el.attr("id", "previous-read-" + this.model.id);
            return this;
        }
    });
}(Backbone, Mustache));
