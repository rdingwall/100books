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
        urlHelper
    ) {

        describe("UrlHelper", function () {

            describe("The URL for a book", function () {

                it("should include the ID and title as a stub", function () {
                    expect(urlHelper.bookUrl(42, "Hello World")).toEqual("#books/42/hello-world");
                });

                it("should not include the title if the title is null", function () {
                    expect(urlHelper.bookUrl(42, null)).toEqual("#books/42");
                });

                it("should ignore extra whitespace and punctuation", function () {
                    expect(urlHelper.bookUrl(42, "Hello -_--:%$--' world", "/")).toEqual("#books/42/hello-world");
                });


            });

        });

    }(
        Ohb.urlHelper
    ));
});