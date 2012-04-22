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
        SearchResult
    ) {
        describe("SearchResult", function () {

            describe("Search results with no no thumbnail image", function () {
                it("should use the default placeholder thumbnail image", function () {
                    var model = { volumeInfo: { } };
                    var searchResult = SearchResult.fromGoogle(model);
                    expect(searchResult.get("smallThumbnailUrl")).toEqual("img/search-result-no-cover.png");
                });
            });

        });
    }(
        Ohb.Models.SearchResult
    ));
});