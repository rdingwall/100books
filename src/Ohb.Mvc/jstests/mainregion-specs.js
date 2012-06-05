/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false
 */

$(function () {
    "use strict";

    (function (
        $,
        mainRegion
    ) {

        describe("MainRegion", function () {

            beforeEach(function () {
                $("#fixture").html($("#mainregion-tests").text());
            });

            describe("Changing the content of the main region", function () {
                it("should call close() on the previous view", function () {

                    var wasClosed = false;

                    mainRegion.show(new (Backbone.View.extend({
                        close: function () { wasClosed = true; }
                    }))());

                    mainRegion.close();
                    expect(wasClosed).toBeTruthy();
                });

                it("should clear the previous content", function () {
                    mainRegion.show(new (Backbone.View.extend({
                        render: function () {
                            this.$el.html("<div id='mainregion-clear-test'>hello</div>");
                            return this;
                        }
                    }))());

                    mainRegion.close();

                    expect($("#mainregion-clear-test")).toBeEmpty();
                });

                it("should render the new content", function () {
                    mainRegion.show(new (Backbone.View.extend({
                        render: function () {
                            this.$el.html("<div id='mainregion-render-test'>hello</div>");
                            return this;
                        }
                    }))());

                    expect($("#mainregion-render-test").text()).toEqual("hello");

                    mainRegion.close();
                });
            });

            describe("Showing an error", function () {
                it("should render the error", function () {
                    var msg = "test error message";
                    mainRegion.showError(msg);

                    expect($(".error-message p").text()).toEqual(msg);

                    mainRegion.close();
                });
            });

        });

    }(
        $,
        Ohb.mainRegion
    ));
});