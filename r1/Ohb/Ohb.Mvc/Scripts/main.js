require.config({
    paths: {
        underscore: 'lib/underscore/underscore',
        backbone: 'lib/backbone/backbone'
    }
});

require([

// Load our app module and pass it to our definition function
  'app',

// Some plugins have to be loaded in order due to there non AMD compliance
// Because these scripts are not "modules" they do not pass any values to the definition function below
  'order!lib/underscore/underscore-min',
  'order!lib/backbone/backbone-min'
], function (App) {
    // The "app" dependency is passed in as "App"
    // Again, the other dependencies passed in are not "AMD" therefore don't pass a parameter to this function
    App.initialize();
});