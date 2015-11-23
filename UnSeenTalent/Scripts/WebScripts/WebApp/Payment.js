app.controller('PaymentCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale) {

    showError(null, null);
    getUserAvaliableTokens();
    getAllTokens();

    $scope.modelpayment = {
        nameoncard: null,
        cardnumber: null,
        cardexpmonth: null,
        year: null,
       
    };

    $scope.showpaymntform = false;

    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }

    function resetModel() {

        $scope.modelLogin = {
            nameoncard: null,
            cardnumber: null,
            cardexpmonth: null,
            year: null,
            
        };
    }

    function getUserAvaliableTokens() {
        myFactory.postData(EndPoint.Service, "GetAvaliableTokenForUser", null, function (res) {
            if (res.Success == true) {
                $scope.avaliableTokens = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    function getAllTokens() {
        myFactory.postData(EndPoint.Service, "GetAllToken", null, function (res) {
            if (res.Success == true) {
                $scope.allTokens = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    $scope.setpriceinbox = function (price,tokenId) {
        debugger;
        $scope.pricetobepaid = price;
        $scope.showpaymntform = true;
        $scope.tokenId = tokenId;
    };


    //console.log($scope.modelLogin.rememberme);
    $scope.Paymentclickfunc = function(modelLogin) {

        var inputs = ["nameoncard", "cardnumber", "cardexpmonth", "year"];
        var isValid = isValidElements(inputs);
        if (isValid) {

            var o = new Object();
            o.nameoncard = modelLogin.nameoncard;
            o.cardnumber = modelLogin.cardnumber;
            o.cardexpmonth = modelLogin.cardexpmonth;
            o.year = modelLogin.year;
            o.amount = $scope.pricetobepaid;
            o.tokenId = $scope.tokenId;
            o.userId = $('#hdnuserid').val();
            myFactory.postData(EndPoint.Service, "DoPayment", o, function(res) {
                if (res.Success == true) {
                    //alert('coming');
                    var message = res.Result;
                    showError("success", message);

                    LoaderStop();

                } else {
                    var message = res.Result;
                    showError("warning", message);


                    LoaderStop();
                }
            });
        } else {
            showError("danger", "Highlighted fields are required.");
        }


    };


});