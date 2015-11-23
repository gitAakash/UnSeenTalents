app.controller('registration', function ($scope, myFactory, $log, $filter, ngTableParams, $locale) {

    var siteUrl = '';

    showError(null, null);
    $scope.modelLogin = {
        userName: null,
        emailId: null,
        confirmEmailId: null,
        password: null,
        confirmPassword: null,
        image: '',
    };
    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }
    var expires_day = 2

    function resetModel() {

        $scope.modelLogin = {
            userName: null,
            emailId: null,
            confirmEmailId: null,
            password: null,
            confirmPassword: null,
            image: '',
        };
    }



    //.log($scope.modelLogin);
    // Login(modelLogin);
    //modelLoginvalue.username=$.cookie('pm[email]');
    //modelLogin.password=$.cookie('pm[password]');
    ////modelLoginvalue.rememberme = true;


    //console.log($scope.modelLogin.rememberme);
    $scope.registrationClickfunc = function (modelLogin) {

        var inputs = ["userName", "password", "confirmPassword", "email"];
        var isValid = isValidElements(inputs);
        if (isValid) {
            if (isValidEmail(modelLogin.emailId)) {
                if (isPasswordMatch(modelLogin.password, modelLogin.confirmPassword)) {
                    LoaderStart();
                    registration(modelLogin);

                }
                else {
                    showError("danger", "Confirm password does not match.");
                }

            }
            else {
                showError("danger", "The email address is not in correct formate.");
            }
        }
        else {
            showError("danger", "Highlighted fields are required.");
        }


    }

    function registration(model) {
        var o = new Object();
        o.userName = model.userName;
        o.emailId = model.emailId;
        o.password = model.password;
        o.image = $('#hdnProfileImage').val() + '.png';
        $.ajax({
            type: "POST",
            url: "/api/UnseentalentApi/UserRegistration",
            data: JSON.stringify(o),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            error: function (xhr) {
                $("#message").text(xhr.statusText);
            },
            success: function (result) {
                if (result.Success == true) {
                    if (result.Result == '3') {
                        showError("danger", "The email address already exists.");
                        $scope.$apply();
                        LoaderStop();
                    } 
                    else if (result.Result == '2') {
                        showError("danger", "Opps!!! Something went wrong. Please try again.");
                        $scope.$apply();
                        LoaderStop();
                    } else {
                        window.location.href = siteUrl + '/webapp/welcome';
                    }
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