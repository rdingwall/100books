/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 runsAsync: false
 */

$(function () {
    "use strict";

    (function (
        $,
        PreviousRead,
        PreviousReadCollection,
        PreviousReadCollectionView,
        eventBus
    ) {

        describe("PreviousReadCollectionView", function () {

            beforeEach(function () {
                $("#fixture").html($("#previousreadcollectionview-tests").text());
            });

            describe("Rendering a previous read collection", function () {
                it("should render the details of the items", function () {
                    var model1 = new PreviousRead({
                        title: "title 1",
                        id: "1"
                    });
                    var model2 = new PreviousRead({
                        title: "title 2",
                        id: "2"
                    });

                    var collection = new PreviousReadCollection();
                    collection.reset([ model1, model2 ]);

                    var view = new PreviousReadCollectionView({
                        el: "#test-previous-reads",
                        collection: collection
                    });

                    view.render();

                    expect(view.$el.find("#previous-read-1").find(".previous-read-title").text())
                        .toEqual(model1.get("title"));
                    expect(view.$el.find("#previous-read-2").find(".previous-read-title").text()).
                        toEqual(model2.get("title"));
                });
            });

            describe("Firing a previousread:removed event", function () {
                it("should remove the view", function () {
                    runsAsync(function (callback) {
                        var model1 = new PreviousRead({
                            title: "title 1",
                            id: "1"
                        });
                        var model2 = new PreviousRead({
                            title: "title 2",
                            id: "2"
                        });

                        var collection = new PreviousReadCollection();
                        collection.reset([ model1, model2 ]);

                        var view = new PreviousReadCollectionView({
                            el: "#test-previous-reads",
                            collection: collection
                        });

                        view.render();

                        expect(view.$el.find("#previous-read-1").find(".previous-read-title").text())
                            .toEqual(model1.get("title"));

                        eventBus.trigger("previousread:removed", model1.id);

                        setTimeout(function () {
                            expect(view.$el.find("#previous-read-1").length).toEqual(0);
                            callback();
                        }, 1000);
                    });
                });
            });

        });

    }(
        $,
        Ohb.Models.PreviousRead,
        Ohb.Collections.PreviousReadCollection,
        Ohb.Views.PreviousReadCollectionView,
        Ohb.eventBus
    ));
});