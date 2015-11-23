//Login


app.controller('LoginCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale) {

  //  var siteUrl = '';

    showError(null, null);
    $scope.data = {
        emailId: null,
        password: null
    };
    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }

    var expires_day = 2;

    function resetModel() {

        $scope.modelLogin = {
            username: null,
            password: null,
            rememberme: false
        };
    }



    //.log($scope.modelLogin);
    // Login(modelLogin);
    //modelLoginvalue.username=$.cookie('pm[email]');
    //modelLogin.password=$.cookie('pm[password]');
    ////modelLoginvalue.rememberme = true;


    //console.log($scope.modelLogin.rememberme);
    $scope.loginClickfunc = function(modelLogin) {

        var inputs = ["email", "pwd"];
        var isValid = isValidElements(inputs);
        if (isValid) {
            //if (isValidEmail(modelLogin.username)) {
            LoaderStart();
            login(modelLogin);
            //}
            //else {
            //    showError("danger", "The email address or password is incorrect.");
            //}
        } else {
            showError("danger", "Highlighted fields are required.");
        }
    };

    function login(model) {
        var o = new Object();
        o.Email = model.username;
        o.Password = model.password;
        o.rememberme = false;
        $.ajax({
            type: "POST",
            url: siteUrl+"/api/UnseentalentApi/UserLogin",
            data: JSON.stringify(o),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
                $("#message").text(xhr.statusText);
            },
            success: function (result) {
                if (result.Success == true) {
                    window.location.href = siteUrl + '/webapp/index';
                    LoaderStop();

                }
                else {
                    showError("danger", "The email address or password is incorrect.");
                    LoaderStop();
                    $scope.$apply();
                }
            }
        });

    }
});
