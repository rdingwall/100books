/*globals require, define, Router */
define(['jquery', 'underscore', 'backbone', 'router', 'eventbus'], function ($, _, Backbone, Router, eventBus) {
    var initialize = function () {
        // Pass in our Router module and call it's initialize function
        Router.initialize();
    };

    return {
        initialize: initialize
    };
});
