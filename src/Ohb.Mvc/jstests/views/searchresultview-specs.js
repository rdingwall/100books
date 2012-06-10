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
                    expect(view.$el.find(".searchresult-title a").attr("href")).toEqual(view.model.get("viewUrl"));
                });
            });

            describe("Clicking the button to mark a SearchResult as read", function () {
                it("should change the SearchResult's hasPreviouslyRead attr to true", function () {
                    var model  = new SearchResult();

                    var view = new SearchResultView({
                        el: "#test-search-result",
                        model: model
                    });

                    view.render();

                    view.$el.find("a.status-toggle-button").trigger("click");
                    expect(model.get("hasPreviouslyRead")).toBeTruthy();

                    view.close();
                    view.$el.remove();
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