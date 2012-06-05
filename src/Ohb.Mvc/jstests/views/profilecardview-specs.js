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
        ProfileCardView
    ) {
        describe("ProfileCardView", function () {

            beforeEach(function () {
                $("#fixture").html($("#profilecardview-tests").text());
            });

            describe("Rendering a profile card view", function () {
                it("should render the details", function () {
                    var model = new Profile({
                        displayName: "test user"
                    });

                    var view = new ProfileCardView({
                        model: model,
                        el: "#test-profile-card"
                    });

                    view.render();

                    expect(view.$el.find(".profile-card-display-name").text()).toEqual(model.get("displayName"));
                });
            });
        });

    }(
        $,
        Ohb.Models.Profile,
        Ohb.Views.ProfileCardView
    ));
});