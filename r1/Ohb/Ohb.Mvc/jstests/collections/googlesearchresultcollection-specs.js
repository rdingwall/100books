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
        GoogleSearchResultCollection
    ) {

        describe("GoogleSearchResultCollection", function () {

            describe("Fetching a search result collection", function () {

                it("should perform a search and returned the results", function () {
                    runsAsync(function (callback) {
                        var results = new GoogleSearchResultCollection();

                        results.fetch({ data: { q: "harry potter" }, success: function (collection) {
                            expect(collection.length).toEqual(10);
                            var result = collection.get("0W0DRgAACAAJ");
                            expect(result.get("authors")).toEqual("J. K. Rowling");
                            expect(result.get("title")).toEqual("Harry Potter and the Deathly Hallows");
                            expect(result.get("smallThumbnailUrl")).toEqual("http://bks2.books.google.co.uk/books?id=0W0DRgAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");
                            callback();
                        }});
                    });
                });

                // Google returns 2 the same book twice when searching for 'girl
                // tattoo'. (Not strictly dupes as they have different etags).
                it("should ignore duplicate results", function () {
                    runsAsync(function (callback) {
                        var results = new GoogleSearchResultCollection();

                        results.fetch({ data: { q: "girl tattoo" }, success: function (collection) {
                            expect(collection.where({ id: "z9ej-ZLbAHAC" }).length).toEqual(1);
                            callback();
                        }});
                    });
                });
            });
        });
    }(
        Ohb.Collections.GoogleSearchResultCollection
    ));
});