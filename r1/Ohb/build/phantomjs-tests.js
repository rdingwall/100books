// from http://blog.danmerino.com/continuos-integration-ci-for-javascript-jasmine-and-teamcity/
console.log('Loading a web page');

//This was tricky, this is the way to open LOCAL files


var urls = [
    "http://localhost:88/jstests/jasmine.html",
    "http://localhost:88/jstests/mocha-loggedin.html"
];

phantom.viewportSize = {width: 800, height: 600};

for (var url in urls) {

    console.log("Opening " + url);

    var page = new WebPage();
    //This is required because PhantomJS sandboxes the website and it does not show up the console messages form that page by default
    page.onConsoleMessage = function (msg) { console.log(msg); };
    //Open the website

    page.open(url, function (status) {
        //Page is loaded!
        if (status !== 'success') {
            console.log('Unable to load the address!');
        } else {
            //Using a delay to make sure the JavaScript is executed in the browser
            window.setTimeout(function () {
                page.render("output.png");
                phantom.exit();
            }, 5000);
        }
    });
}