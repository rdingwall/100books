$(document).ready(function () {

    $("#header-search-input").keyup(function (event) {
        if (event.keyCode == 13) {
            window.location = "/Search/" + $("#header-search-input").val();
        }
    });

});