/*globals require, define, Router */
define([
    'jquery',
    'underscore',
    'backbone',
    'router',
    'eventbus'
],
    function ($, _, Backbone, AppRouter, eventBus) {


    var initialize = function () {
        // Pass in our Router module and call it's initialize function
        AppRouter.initialize();
    };



    return {
        initialize: initialize
    };
});
