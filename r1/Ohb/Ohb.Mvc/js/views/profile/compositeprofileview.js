$(function () {

    "use strict";

    Ohb.Views.CompositeProfileView = (function ($, Backbone, _, ProfileCardView,
                                       PreviousReadCollectionView) {

        var log = $.jog("ProfileView");

        return Backbone.View.extend({
            className: "profile",

            initialize: function () {
                this.$profileCardEl = $("<div />");
                this.$previousReadsEl = $("<div />");
            },

            render: function () {
                log.info("Rendering ProfileView.");

                if (this.rendered) {
                    return;
                }

                this.rendered = true;

                this.$el.append(this.$profileCardEl);
                this.$el.append(this.$previousReadsEl);
                return this;
            },

            renderProfileCard: function (model) {
                this.render();
                new ProfileCardView({
                    model: model,
                    el: this.$profileCardEl
                }).render();
                this.trigger("profilecard:rendered");
            },

            renderPreviousReads: function (collection) {
                this.render();
                new PreviousReadCollectionView({
                    collection: collection,
                    el: this.$previousReadsEl
                }).render();
                this.trigger("previousreads:rendered");
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