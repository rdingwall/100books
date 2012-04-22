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


            describe("Mapping google book search results", function () {

                var assertBookMapping = function (callback, googleBookId, title, authors, smallThumbnailUrl) {
                    $.getJSON("https://www.googleapis.com/books/v1/volumes/" + googleBookId + "?&key=AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs&callback=?",
                        function (data) {
                            var searchResult = SearchResult.fromGoogle(data);

                            expect(searchResult.get("title")).toEqual(title);
                            expect(searchResult.get("authors")).toEqual(authors);
                            expect(searchResult.get("smallThumbnailUrl")).toStartWith(smallThumbnailUrl);
                            callback();
                        })
                        .error(function () {
                            expect(false).toBeTruthy();
                            callback();
                        });
                };

                it("should include the book's subtitle", function () {
                    runsAsync(function (callback) {
                        assertBookMapping(callback, "abYKXvCwEToC", "Harry Potter: The Story of a Global Business Phenomenon", "Susan Gunelius", "http://bks6.books.google.co.uk/books?id=abYKXvCwEToC&printsec=frontcover&img=1&zoom=5&edge=curl");
                    });
                });

                it("should list multiple authors", function () {
                    runsAsync(function (callback) {
                        assertBookMapping(callback, "GGpXN9SMELMC", "Head First Design Patterns", "Eric Freeman, Elisabeth Freeman, Kathy Sierra, Bert Bates", "http://bks9.books.google.co.uk/books?id=GGpXN9SMELMC&printsec=frontcover&img=1&zoom=5&edge=curl");
                    });
                });

                it("should handle books without thumbnails", function () {
                    runsAsync(function (callback) {
                        assertBookMapping(callback, "DAAAAAAACAAJ", "Harry Potter: 5 Years of Magic, Adventure, and Mystery at Hogwarts", "J. K. Rowling", "img/search-result-no-cover.png");
                    });
                });

                it("should handle books with no authors", function () {
                    runsAsync(function (callback) {
                        assertBookMapping(callback, "3zgFSQAACAAJ", "The Girl With the Dragon Tattoo", null, "http://bks1.books.google.co.uk/books?id=3zgFSQAACAAJ&printsec=frontcover&img=1&zoom=5");
                    });
                });

            });

        });
    }(
        Ohb.Models.SearchResult
    ));
});