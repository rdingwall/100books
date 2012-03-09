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
    'models/searchresult',
    'eventbus',
    'router',
    'qunit',
    'lib/jog',
    'models/book'
], function (
    main,
    $,
    SearchResult,
    eventBus,
    router,
    Book
) {
    "use strict";

    var log = $.jog("ApiTests");

    log.info("hiya");

    $(function () {

    });
});