$(document).ready(function () {

    $("#header-search-input").keyup(function (event) {
        if (event.keyCode == 13) {
            window.location = "/#search/" + encodeURIComponent($("#header-search-input").val());
        }
    });


    var SearchController = Backbone.Controller.extend({
        routes: {
            "search/:q": "search"
        },

        search: function (q) {
            $.getJSON("https://www.googleapis.com/books/v1/volumes?callback=?",
            {
                q: decodeURIComponent(q),
                projection: "lite"
            },
            function (data) {

                var results = new SearchResults();

                $(data.items).each(function () {
                    results.add(SearchResult.fromGoogle($(this)[0]));
                });

                var searchResultsCollectionView = new SearchResultsCollectionView({
                    collection: results,
                    el: $('div.book-search-results')[0]
                });

                searchResultsCollectionView.render();
            });

        }
    });

    SearchResult = Backbone.Model.extend({
        title: null,
        authors: null,
        smallThumbnailUrl: null
    });

    SearchResult.fromGoogle = function (volume) {
        var info = volume.volumeInfo;

        var title = info.title;
        if (info.subtitle)
            title += ": " + info.subtitle;

        var imageLinks = info.imageLinks;
        var smallThumbnailUrl = imageLinks ? imageLinks.smallThumbnail : null;

        var authors = info.authors ? info.authors.join(", ") : null;

        return new SearchResult({
            title: title,
            authors: authors,
            smallThumbnailUrl: smallThumbnailUrl
        });
    }

    SearchResults = Backbone.Collection.extend({
});

SearchResultView = Backbone.View.extend({
    tagName: "div",
    className: "book-search-result",

    render: function () {
        $(this.el).append(ich.tmplBookSearchResult(this.model.toJSON()));
        return this;
    }
});

SearchResultsCollectionView = Backbone.View.extend({
    initialize: function () {
        var that = this;
        this.searchResultViews = [];

        this.collection.each(function (searchResult) {
            that.searchResultViews.push(new SearchResultView({
                model: searchResult,
                tagName: 'div'
            }));
        });
    },

    render: function () {
        var that = this;
        // Clear out this element.
        $(this.el).empty();

        // Render each sub-view and append it to the parent view's element.
        _(this.searchResultViews).each(function (srv) {
            $(that.el).append(srv.render().el);
        });
    },

    addSearchResult: function (model) {
        $(this.el).append(model.render().el);
    }
});

var yC = new SearchController;

Backbone.history.start();

});


function CreateBookSearchResult(volume) {
    var info = volume.volumeInfo;

    var title = info.title;
    if (info.subtitle)
        title += ": " + info.subtitle;

    var imageLinks = info.imageLinks;
    var smallThumbnailUrl = imageLinks ? imageLinks.smallThumbnail : null;

    return new SearchResult({
        title: title,
        authors: volume.volumeInfo.authors.join(", "),
        smallThumbnailUrl: smallThumbnailUrl
    });
}


