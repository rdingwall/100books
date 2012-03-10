$(function () {
    "use strict";

    var Ohb = window;

    (function (app, Backbone) {
        app.initialize();
        Backbone.history.start();
    }(Ohb.app, Backbone));

});