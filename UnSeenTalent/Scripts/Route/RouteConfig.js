
function routing($routeProvider, $locationProvider) {
    // route for the alert security page
    $routeProvider
        .when('/test', {
            templateUrl: siteUrl + '/Dashboard/Index2',
            controller: 'dashboardCtrl'
        })

     .when('/', {
         templateUrl: siteUrl + '/Dashboard/Dashboard',
         controller: 'dashboardCtrl'
     })

    .when('/my-files', {
        templateUrl: siteUrl + '/Dashboard/MyFiles',
        controller: 'myFileCtrl'
    })
    .when('/AccountSetting', {
        templateUrl: siteUrl + '/Home/AccountSetting',
        controller: 'AccountSettingCtrl'
    })
     .when('/ManageUsers', {
         templateUrl: siteUrl + '/Home/ManageUsers',
         controller: 'ManageUserCtrl'
     })
     .when('/Addusers', {
         templateUrl: siteUrl + '/Home/Addusers',
         controller: 'AddUsersCtrl'
     })
    .when('/AddExternaluser', {
        templateUrl: siteUrl + '/Home/AddExternaluser',
        controller: 'AddExternalCtrl'
    })
    

    // .otherwise({ redirectTo: '/' });
    // $locationProvider.html5Mode(true);
};




// configure our routes
app.config(routing);
app.controller('dashboardCtrl', dashboardCtrl);
