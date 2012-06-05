/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false
 */

$(function () {
    "use strict";

    (function (
        $,
        Profile,
        PreviousReadCollection,
        PreviousRead,
        CompositeProfileView
    ) {

        describe("CompositeProfileView", function () {

            beforeEach(function () {
                $("#fixture").html($("#compositeprofileview-tests").text());
            });

            describe("Rendering a composite profile view", function () {
                it("should render both the profile card and the previous reads", function () {
                    var model = new Profile({
                        displayName: "test user"
                    });

                    var collection = new PreviousReadCollection();
                    collection.add(new PreviousRead({
                        title: "title 1"
                    }));

                    var view = new CompositeProfileView({
                        el: "#test-composite-profile",
                        previousReadCollection: collection,
                        profileModel: model
                    });

                    view.render();

                    expect(view.$el.find(".profile-card-display-name").text()).toEqual(model.get("displayName"));
                    expect(view.$el.find(".previous-read-title").text()).toEqual(collection.at(0).get("title"));
                });

            });

        });

    }(
        $,
        Ohb.Models.Profile,
        Ohb.Collections.PreviousReadCollection,
        Ohb.Models.PreviousRead,
        Ohb.Views.CompositeProfileView
    ));
});