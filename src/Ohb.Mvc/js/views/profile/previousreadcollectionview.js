$(function () {

    "use strict";

    Ohb.Views.PreviousReadCollectionView = (function ($, Backbone, PreviousReadView) {

        var log = $.jog("PreviousReadCollectionView");

        return Backbone.Marionette.CollectionView.extend({
            itemView: PreviousReadView,

            close: function () {
                this.collection.unbindEvents();
            }
        });
    }(
        $,
        Backbone,
        Ohb.Views.PreviousReadView
    ));
});