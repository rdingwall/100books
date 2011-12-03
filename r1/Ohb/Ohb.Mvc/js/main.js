/*globals require */
require.config({
    paths: {
        underscore: 'lib/underscore/underscore',
        backbone: 'lib/backbone/backbone',
        mustache: 'lib/mustache/mustache',
        bootstrapModal: 'lib/bootstrap/bootstrap-modal'
    }
});

require([

// Load our app module and pass it to our definition function
    'app',
    'backbone'
], function (App, Backbone) {
    "use strict";

    // The "app" dependency is passed in as "App"
    // Again, the other dependencies passed in are not "AMD" therefore don't pass a parameter to this function
    App.initialize();
    Backbone.history.start();
});