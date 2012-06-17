(function () {
    "use strict";
    var system = require("system");

    var url = system.args[1];

    phantom.viewportSize = {width: 800, height: 600};

    console.log("Opening " + url);

    var page = new WebPage();
    //This is required because PhantomJS sandboxes the website and it does not show up the console messages form that page by default
    page.onConsoleMessage = function (msg) {
        console.log(msg);

        if (msg) {
            if (msg.indexOf("##teamcity[testSuiteFinished name='mocha.suite'") !== -1 ||
            msg.indexOf("ohb-jasmine-all-finished") !== -1) {
            phantom.exit();
        }
    };

    //Open the website
    page.open(url, function (status) {
        //Page is loaded!
        if (status !== 'success') {
            console.log('Unable to load the address!');
            phantom.exit(-1);
        } else {
            //Using a delay to make sure the JavaScript is executed in the browser
            window.setTimeout(function () {
                phantom.exit();
            }, 60 * 1000);
        }
    });

}());