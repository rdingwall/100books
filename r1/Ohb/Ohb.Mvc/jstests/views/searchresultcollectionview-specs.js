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
        SearchResultCollectionView,
        eventBus,
        SearchResultCollection,
        SearchResult
    ) {

        describe("SearchResultCollectionView", function () {

            beforeEach(function () {
                $("#fixture").html($("#searchresultcollectionview-tests").text());
            });

            describe("When the search result box is open", function () {

                describe("clicking outside the search box", function () {
                    it("should close the results", function () {
                        var view = new SearchResultCollectionView({
                            el: "#test-search-results"
                        });

                        expect(view.$el).toBeHidden();

                        view.render();

                        expect(view.$el).toBeVisible();

                        $("body").trigger("click");

                        expect(view.$el).toBeHidden();
                        expect(view.views).toBeEmpty();
                    });
                });

                describe("clicking inside the search box", function () {
                    it("should not close the results", function () {
                        var view = new SearchResultCollectionView({
                            el: "#test-search-results"
                        });

                        expect(view.$el).toBeHidden();

                        view.render();

                        expect(view.$el).toBeVisible();

                        view.$el.trigger("click");

                        expect(view.$el).toBeVisible();
                    });
                });

                describe("clicking in the menu bar", function () {
                    it("should not close the results", function () {
                        var view = new SearchResultCollectionView({
                            el: "#test-search-results"
                        });

                        expect(view.$el).toBeHidden();
                        view.render();

                        expect(view.$el).toBeVisible();

                        $("#menubar").trigger("click");

                        expect(view.$el).toBeVisible();
                    });
                });

            }); // When the search result box is open


            describe("When starting a new search", function () {
                it("the previous search results should be closed", function () {
                    var view = new SearchResultCollectionView({
                        el: "#test-search-results"
                    });

                    view.render();

                    eventBus.trigger("search:began");

                    expect(view.$el).toBeHidden();
                });
            });

            describe("When there are no search results available", function () {
                it("a 'no search results' message should be displayed", function () {
                    var view = new SearchResultCollectionView({
                        el: "#test-search-results"
                    });

                    expect(view.$el).toBeHidden();

                    view.render();

                    expect(view.$el).toBeVisible();
                    expect(".searchresult-no-results-available").toBeVisible();
                });
            });

            describe("When rendering a collection of search results", function () {
                it("each result should be rendered", function () {

                    expect("#test-search-results").toBeHidden();

                    var collection = new SearchResultCollection();
                    collection.add(new SearchResult({ title: "test book" }));
                    collection.add(new SearchResult({ title: "test book 2" }));
                    var view = new SearchResultCollectionView({
                        el: $("#test-search-results"),
                        collection: collection
                    }).render();

                    expect(view.$el).toBeVisible();
                    expect(view.$el.children().length).toEqual(2);
                });
            });
        });

    }(
        $,
        Ohb.Views.SearchResultCollectionView,
        Ohb.eventBus,
        Ohb.Collections.SearchResultCollection,
        Ohb.Models.SearchResult
    ));
});