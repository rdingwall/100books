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
        SearchResultView,
        eventBus,
        SearchResultCollection,
        SearchResult
        ) {

        describe("SearchResultView", function () {

            beforeEach(function () {
                $("#fixture").html($("#searchresultview-tests").text());
            });

            describe("Rendering a search result", function () {
                it("should render it", function () {
                    var view = new SearchResultView({
                        el: "#test-search-result",
                        model: new SearchResult({
                            title: "Harry Potter",
                            authors: "JK Rowling",
                            smallThumbnailUrl: "http://2.gravatar.com/avatar/87acbe2fc2f40edf8fa5a816515bff9f",
                            id: "42"
                        })
                    });
                    view.render();

                    expect(view.$el.find(".searchresult-title").text()).toEqual("Harry Potter");
                    expect(view.$el.find("p.searchresult-authors").text()).toEqual("JK Rowling");
                });
            });



            describe("Clicking on a search result", function () {
                it("should mark the result as selected", function () {

                    var model = new SearchResult({ title: "foo" });
                    var view = new SearchResultView({ el: "#test-search-result", model: model });

                    view.$el.trigger("click");

                    expect(model.get("selected")).toBeTruthy();
                });
            });

            describe("Marking a search result as selected", function () {
                it("should raise a search:result:selected event", function () {

                    var model = new SearchResult({ title: "foo" });

                    eventBus.on("search:result:selected", function (sr) {
                        expect(sr).toEqual(model);
                    });

                    model.set("selected", true);
                });
            });
        });
    }(
        $,
        Ohb.Views.SearchResultView,
        Ohb.eventBus,
        Ohb.Collections.SearchResultCollection,
        Ohb.Models.SearchResult
    ));
});