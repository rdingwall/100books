/*globals require, define, Router */
define([
    'jquery',
    'underscore',
    'backbone',
    'router',
    'eventbus'
],
    function ($, _, Backbone, AppRouter, eventBus) {
        "use strict";

        return {
            initialize: function () {
                // Pass in our Router module and call it's initialize function
                AppRouter.initialize();
            }
        };
});
