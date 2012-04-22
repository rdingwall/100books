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
        Book
    ) {
        describe("Book", function () {

            describe("Books with thumbnails", function () {
                it("should use the real thumbnail image in search results", function () {
                    var model = new Book({ smallThumbnailUrl: "test" });
                    expect(model.getSearchResultThumbnail()).toEqual("test");
                });
            });

        });
    }(
        Ohb.Models.Book
    ));
});