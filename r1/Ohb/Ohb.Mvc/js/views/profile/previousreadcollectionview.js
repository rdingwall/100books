$(function () {

    "use strict";

    Ohb.Views.PreviousReadCollectionView = (function ($, Backbone, PreviousReadView) {

        var log = $.jog("PreviousReadCollectionView");

        return Backbone.Marionette.CollectionView.extend({
            itemView: PreviousReadView
        });
    }(
        $,
        Backbone,
        Ohb.Views.PreviousReadView
    ));
});