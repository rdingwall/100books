$(function () {

    "use strict";

    Ohb.Views.CompositeProfileView = (function ($, Backbone, _, ProfileCardView,
                                       PreviousReadCollectionView) {

        var log = $.jog("ProfileView");

        return Backbone.View.extend({
            className: "profile",

            initialize: function (options) {
                this.profileModel = options.profileModel;
                this.previousReadsCollection = options.previousReadsCollection;
            },

            render: function () {
                log.info("Rendering CompositeProfileView.");

                this.$el.append(new ProfileCardView({
                    model: this.profileModel
                }).render().el);
                
                new PreviousReadCollectionView({
                    collection: this.previousReadsCollection,
                    el: this.$el.append("<div/>")
                }).render();

                return this;
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