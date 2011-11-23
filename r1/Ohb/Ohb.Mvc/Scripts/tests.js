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
    'http://code.jquery.com/qunit/git/qunit.js'
],
    function (main, eventBus, $, _, Backbone, dummy) {

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

    });