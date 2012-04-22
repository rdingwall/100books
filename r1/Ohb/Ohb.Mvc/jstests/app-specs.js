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
        eventBus,
        app,
        SearchResult
    ) {
        describe("App", function () {

            beforeEach(function () {
                Ohb.menuBarView.initialize();
                $("#fixture").html($("#app-tests").text());
            });

            describe("Firing a search:failed event", function () {

                it("should render the 'search failed' error modal", function () {
                    app.initialize();

                    expect("#search-failed-modal").toBeHidden();

                    eventBus.trigger("search:failed");

                    expect("#search-failed-modal").toBeVisible();

                    $("#search-failed-modal").hide();
                });
            });


            describe("Firing a search:result:selected event", function () {
                it("should navigate to the new route", function () {
                    app.initialize();

                    var model = new SearchResult({
                        id: "foo",
                        title: "Harry Potter's amazing #(*@(#)(# adventure$ 2008"
                    });

                    eventBus.trigger("search:result:selected", model);

                    expect(window.location.hash).toEqual("#books/foo/harry-potters-amazing-adventure-2008");
                });

                // This one fails when run with the other tests for some reason
                it("should raise a book:requested event", function () {
                    runsAsync(function (callback) {
                        app.initialize();
                        var model = new SearchResult({ id: "foo" });

                        eventBus.on("book:requested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        eventBus.trigger("search:result:selected", model);
                    });
                });

            });


        });
    }(
        $,
        Ohb.eventBus,
        Ohb.app,
        Ohb.Models.SearchResult
    ));
});