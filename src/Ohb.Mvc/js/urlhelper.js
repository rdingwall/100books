Ohb.urlHelper = (function () {
    "use strict";

    return {
        bookUrl: function (id, title) {
            var url = "#books/" + id;

            if (!title) { return url; }

            var slug = title
                .replace(/[^\w\s]|_/g, "")
                .replace(/\s+/g, "-")
                .toLowerCase();

            return url + "/" + slug;
        }
    };
}());