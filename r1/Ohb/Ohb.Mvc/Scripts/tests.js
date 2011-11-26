﻿require.config({
    paths:{
        underscore:'lib/underscore/underscore',
        backbone:'lib/backbone/backbone'
    }
});

require([
    'main',
    'router',
    'eventbus',
    'jquery',
    'underscore',
    'backbone',
    'views/menubar/menubarview',
    'lib/qunit/qunit.js',
    'lib/jsmockito/jsmockito.js'
],
    function (main, router, eventBus, $, _, Backbone, MenuBarView) {

        console.log("hiya");

        $(function () {

            module("When registering modules");

            test("It should inject the event bus", function () {
                ok(eventBus);
            });

            test("The event bus should be extended with Backbone events", function () {
                ok(eventBus.bind);
            });

            test("It should inject jQuery", function () {
                ok($);
            });

            test("It should inject underscore", function () {
                ok(_);
            });

            test("It should inject backbone", function () {
                ok(Backbone);
            });

            module('when pressing enter in the search box');

            test('It should raise the searchRequested event', function () {
                eventBus.reset();

                var view = new MenuBarView({ el:$("#qunit-fixture") });
                var expected = 'test search';
                expect(1);

                eventBus.bind('searchRequested', function (q) {
                    equals(q, expected);
                });

                $("#menubar-search-input").val(expected);
                var e = $.Event('keyup');
                e.which = 13;
                $("#menubar-search-input").trigger(e);
            });

            test('It shouldn\'t raise any event if the search box is empty', function () {
                eventBus.reset();
                var view = new MenuBarView({ el:$("#qunit-fixture") });

                $("#menubar-search-input").val('');
                e = $.Event('keyup');
                e.which = 13;

                eventBus.bind('searchRequested', function (q) {
                    ok(false, "should not have been raised!");
                });

                $("#menubar-search-input").trigger(e);
                expect(0);
            });


            module("When a searchRequested event is raised");

            asyncTest('It should perform a search and raise a resultsArrived event with the results', 2,
                function () {
                    eventBus.reset();
                    router.initialize();

                    var wasRaised = false;
                    eventBus.bind('searchResultsArrived', function (results) {
                        wasRaised = true;
                        ok(results);
                        equal(results.length, 10);
                    });

                    eventBus.bind('searchFailed', function (results) {
                        ok(false, "search failed!");
                    });

                    eventBus.trigger("searchRequested", "harry potter");

                    setTimeout(start, 2000);
                });

            asyncTest('It should raise a searchBegan event', 2,
                function () {
                    eventBus.reset();
                    router.initialize();

                    eventBus.bind('searchBegan', function (q) {
                        ok(true);
                        equal(q, "harry potter");
                    });

                    eventBus.trigger("searchRequested", "harry potter");

                    setTimeout(start, 1000);
                });

            asyncTest('It should raise a searchCompleted event', 1,
                function () {
                    eventBus.reset();
                    router.initialize();

                    eventBus.bind('searchCompleted', function (q) {
                        ok(true);
                    });

                    eventBus.trigger("searchRequested", "harry potter");

                    setTimeout(start, 2000);
                });

            asyncTest('When the test fails, it should raise a searchFailed event', 1,
                function () {
                    eventBus.reset();
                    router.initialize();

                    eventBus.bind('searchFailed', function (results) {
                        ok(true);
                    });

                    eventBus.bind('searchResultsArrived', function (results) {
                        ok(false, "should not have been raised!");
                    });

                    eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

                    setTimeout(start, 2000);
                });

            asyncTest('When there are no results, it should raise a no results event', 1,
                function () {
                    eventBus.reset();
                    router.initialize();

                    eventBus.bind('searchFailed', function (results) {
                        ok(false, "searchFailed was raised");
                    });

                    eventBus.bind('searchReturnedNoResults', function (results) {
                        ok(true);
                    });

                    eventBus.bind('searchResultsArrived', function (results) {
                        equal(results.length, 0);
                        ok(false, "searchResultsArrived was raised");
                    });

                    eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

                    setTimeout(start, 2000);
                });


            module("when a search fails");

            asyncTest('It should render the error modal', 2, function() {
                eventBus.reset();
                router.initialize();

                ok(!($("#search-failed-modal").is(":visible")), "was visible to start with");

                eventBus.trigger("searchFailed");

                setTimeout(start, 1000);

                ok($("#search-failed-modal").is(":visible"));
            });
        });
    });