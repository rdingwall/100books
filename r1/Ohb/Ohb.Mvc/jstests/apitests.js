/*globals require, define, _ */
/*jslint white: false, onevar: true, undef: true, nomen: false, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, newcap: true, immed: true */
/*global window: false, document: false, $: false, log: false, bleep: false,
 QUnit: false,
 test: false,
 asyncTest: false,
 expect: false,
 module: false,
 ok: false,
 equal: false,
 notEqual: false,
 deepEqual: false,
 notDeepEqual: false,
 strictEqual: false,
 notStrictEqual: false,
 raises: false,
 start: false,
 stop: false
 */

require.config({
    baseUrl: "../js",
    paths: {
        underscore: 'lib/underscore/underscore',
        backbone: 'lib/backbone/backbone',
        qunit: 'lib/qunit/qunit',
        jsmockito: 'lib/jsmockito/jsmockito',
        mustache: 'lib/mustache/mustache',
        bootstrapModal: 'lib/bootstrap/bootstrap-modal'
    }
});

require([
    'lib/requires/order!main',
    'jquery',
    'backbone',
    'models/searchresult',
    'models/book',
    'eventbus',
    'router',
    'qunit',
    'lib/jog'
], function (
    main,
    $,
    Backbone,
    SearchResult,
    Book,
    eventBus,
    router
) {
    "use strict";

    var log = $.jog("ApiTests");

    log.info("hiya");

    $(function () {

        module("When getting a book by ID");

        test("it should blah", 1, function() {

            var BookCollection = Backbone.Collection.extend({
                model: Book,
                url: "/api/v1/books"
            });

            var books = new BookCollection();

            var model = books.get("4YydO00I9JYC");

            equal(model.get("id"), "4YydO00I9JYC");
            equal(model.get("publisher"), "Delacorte Press");
            equal(model.get("title"), "The Google story");
            equal(model.get("authors"), "David A. Vise, Mark Malseed");
            equal(model.get("publishedYear"), "2005");
            equal(model.get("description"), "Here is the story behind one of the most remarkable Internet successes of our time. Based on scrupulous research and extraordinary access to Google, the book takes you inside the creation and growth of a company whose name is a favorite brand and a standard verb recognized around the world. Its stock is worth more than General Motors’ and Ford’s combined, its staff eats for free in a dining room that used to be<b> </b>run<b> </b>by the Grateful Dead’s former chef, and its employees traverse the firm’s colorful Silicon Valley campus on scooters and inline skates.<br><br><b>THE GOOGLE STORY </b>is the definitive account of the populist media company powered by the world’s most advanced technology that in a few short years has revolutionized access to information about everything for everybody everywhere. <br>In 1998, Moscow-born Sergey Brin and Midwest-born Larry Page dropped out of graduate school at Stanford University to, in their own words, “change the world” through a search engine that would organize every bit of information on the Web for free.<br><br>While the company has done exactly that in more than one hundred languages, Google’s quest continues as it seeks to add millions of library books, television broadcasts, and more to its searchable database. <br>Readers will learn about the amazing business acumen and computer wizardry that started the company on its astonishing course; the secret network of computers delivering lightning-fast search results; the unorthodox approach that has enabled it to challenge Microsoft’s dominance and shake up Wall Street. Even as it rides high, Google wrestles with difficult choices that will enable it to continue expanding while sustaining the guiding vision of its founders’ mantra: DO NO EVIL.");
            equal(model.get("pageCount"), "0");
            equal(model.get("thumbnailUrl"), "http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=1&source=gbs_api");
            equal(model.get("smallThumbnailUrl"), "http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5&source=gbs_api");

        });


    });
});