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
        PreviousRead,
        PreviousReadView
    ) {

        describe("PreviousReadView", function () {

            beforeEach(function () {
                $("#fixture").html($("#previousreadview-tests").text());
            });

            describe("Rendering a previous read", function () {

                it("should render the details", function () {
                    var model = new PreviousRead({
                        title: "test title",
                        authors: "test author",
                        smallThumbnailUrl: "test url",
                        id: 42
                    });

                    var view = new PreviousReadView({
                        model: model,
                        el: "#test-previous-read"
                    });

                    view.render();

                    expect(view.$el.find(".previous-read-title").text()).toEqual(model.get("title"));
                    expect(view.$el.find(".previous-read-authors").text()).toEqual(model.get("authors"));
                    expect(view.$el.find(".previous-read-thumbnail").attr("src")).toEqual(model.get("smallThumbnailUrl"));
                    expect(view.$el.find(".previous-read-title a").attr("href")).toEqual(model.get("viewUrl"));
                });

            });

        });

    }(
        $,
        Ohb.Models.PreviousRead,
        Ohb.Views.PreviousReadView
    ));
});