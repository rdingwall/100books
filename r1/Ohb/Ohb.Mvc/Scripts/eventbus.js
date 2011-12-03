/*globals define, Backbone */
define([
    'underscore',
    'backbone'
],
    function (_, Backbone) {
        "use strict";

        var instance = null;

        function EventBus() {

            if (instance !== null) {
                throw new Error("Cannot instantiate more than one EventBus, use EventBus.getInstance()");
            }

            this.initialize();
        }

        EventBus.prototype = {
            initialize: function() {
                console.log("EventBus ctor");

                _.extend(this, Backbone.Events);
                this.reset();
            },

            reset: function() {
                console.log("eventBus.reset");
                this.unbind();
                this.bind("all", function (eventName) {
                    console.log(eventName);
                });
            }
        };

        EventBus.getInstance = function(){
            // summary:
            //      Gets an instance of the singleton. It is better to use
            if(instance === null){
                instance = new EventBus();
            }
            return instance;
        };


        console.log("returning eventBus...");
        return EventBus.getInstance();

    });