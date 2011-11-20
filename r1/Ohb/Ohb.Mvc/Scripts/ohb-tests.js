$(document).ready(function() {

                test("foo",  function() {

        $.getJSON('https://www.googleapis.com/books/v1/volumes/abYKXvCwEToC&callback=?',
            function(volume) {
                var searchResult = CreateBookSearchResult(volume);

                assertEquals("Harry Potter: the story of a global business phenomenon", searchResult.title);
                assertEquals("Susan Gunelius", searchResult.authors);
                assertEquals("http://bks6.books.google.co.uk/books?id=abYKXvCwEToC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api",
                    searchResult.smallThumbnailUrl);
            })
            .error(function() { throw "failed"; });
    });

)