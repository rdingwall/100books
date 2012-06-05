$(function () {

    "use strict";

    Ohb.Views.CompositeProfileView = (function ($, Backbone, _, ProfileCardView,
                                       PreviousReadCollectionView) {

        var log = $.jog("ProfileView");

        return Backbone.View.extend({
            className: "profile",

            initialize: function (options) {
                this.profileModel = options.profileModel;
                this.previousReadCollection = options.previousReadCollection;
            },

            render: function () {
                log.info("Rendering CompositeProfileView.");

                this.profileCardView = new ProfileCardView({
                    model: this.profileModel
                });

                this.$el.append(this.profileCardView.render().el);

                this.collectionView = new PreviousReadCollectionView({
                    collection: this.previousReadCollection,
                    el: this.$el.append("<div/>")
                });

                this.collectionView.render();

                return this;
            },

            close: function () {
                this.collectionView.close();
            }
        });
    }(
        $,
        Backbone,
        _,
        Ohb.Views.ProfileCardView,
        Ohb.Views.PreviousReadCollectionView
    ));
});