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
        Book,
        BookDetailsView
    ) {

        describe("BookDetailsView", function () {

            describe("Clicking the button to toggle a book's status", function () {
                it("should change the book's hasPreviouslyRead attr to true", function () {
                    var model  = new Book();

                    var view = new BookDetailsView({
                        model: model
                    });

                    view.render();

                    view.$el.find(".status-toggle-button.btn-success").trigger("click");
                    expect(model.get("hasPreviouslyRead")).toBeTruthy();

                    view.close();
                    view.$el.remove();
                });
            });

        });
    }(
        $,
        Ohb.Models.Book,
        Ohb.Views.BookDetailsView
    ));
});