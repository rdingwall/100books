$(function () {
    "use strict";

    Ohb.Commands.ViewProfileCommand = (function (
        $,
        Profile,
        PreviousRead,
        PreviousReadCollection,
        mainRegion,
        CompositeProfileView
    ) {

        var log = $.jog("SearchCommand");

        var command = function () {};

        command.prototype.execute = function () {
            log.info("Fetching combined profile from API...");
            $.ajax({
                url: "/api/v1/profiles/me",
                dataType: 'json',
                success: function (data) {
                    var model = new Profile(data);
                    var previousReads = _.map(data.recentReads, function (item) {
                        return new PreviousRead(item);
                    });
                    var collection = new PreviousReadCollection(previousReads);

                    var view = new CompositeProfileView({
                        profileModel: model,
                        previousReadCollection: collection
                    });
                    mainRegion.show(view);
                },
                error: function () {
                    mainRegion.showError("Sorry, there was an error retrieving this profile.");
                }
            });
        };

        return command;
    }(
        $,
        Ohb.Models.Profile,
        Ohb.Models.PreviousRead,
        Ohb.Collections.PreviousReadCollection,
        Ohb.mainRegion,
        Ohb.Views.CompositeProfileView
    ));
});