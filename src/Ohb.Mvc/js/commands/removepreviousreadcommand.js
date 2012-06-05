Ohb.Commands.RemovePreviousReadCommand = (function (
    $,
    eventBus
) {
    "use strict";

    var command = function () {};

    command.prototype.execute = function (id) {
        $.ajax({
            url: "/api/v1/previousreads/" + id,
            type: 'DELETE',
            success: function () {
                eventBus.trigger("previousread:removed", id);
            }
        });
    };

    return command;
}(
    $,
    Ohb.eventBus
));