require.config({
    paths: {
        underscore: 'lib/underscore/underscore',
        backbone: 'lib/backbone/backbone'
    }
});

require([
    'main',
    'eventbus',
    'jquery',
    'underscore',
    'backbone',
    'views/menubar/menubarview',
    'http://code.jquery.com/qunit/git/qunit.js',
    'lib/jsmockito/jsmockito.js'
],
    function (main, eventBus, $, _, Backbone, MenuBarView, dummy) {

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
                eventBus.unbind();

                var view = new MenuBarView({ el: $("#qunit-fixture") });
                var expected = 'test search';
                expect(1);

                eventBus.bind('searchRequested', function (q) {
                    equals(q, expected);
                });

                $("#menubar-search-input").val(expected);
                e = $.Event('keyup');
                e.which = 13;
                $("#menubar-search-input").trigger(e);


            });

            test('It shouldn\'t raise any event if the search box is empty', function () {
                eventBus.unbind();
                var view = new MenuBarView({ el: $("#qunit-fixture") });

                $("#menubar-search-input").val('');
                e = $.Event('keyup');
                e.which = 13;

                eventBus.bind('searchRequested', function (q) {
                    ok(false, "should not have been raised!");
                });

                $("#menubar-search-input").trigger(e);
            });

        });
    });