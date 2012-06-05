Ohb.Commands.AddPreviousReadCommand = (function (
    $,
    eventBus
) {
    "use strict";

    var command = function () {};

    command.prototype.execute = function (id) {
        $.ajax({
            url: "/api/v1/previousreads/" + id,
            type: 'PUT',
            success: function () {
                eventBus.trigger("previousread:added", id);
            }
        });
    };

    return command;
}(
    $,
    Ohb.eventBus
));