app.controller('commonLayoutCtrl', function ($scope, myFactory) {
    $scope.email = null;
    
    $scope.subscribeNewsLetter = function () {

        var inputs = ["subsemail"];
        var isValid = isValidElements(inputs);
        console.log(isValid);

        if (isValid) {
            var validEmail = isValidEmail($scope.email)
            if (!validEmail) {
                alert('Please Enter a valid email !');
                return false;
            }

            var obj = {
                email: $scope.email
            }

            myFactory.postData(EndPoint.Service, "subscribeNewsLetter", obj, function (res) {
                if (res.Success == true) {
                    $scope.email = null;
                    alert('Subscribe Successfully');
                    LoaderStop();
                }
                else { 
                    LoaderStop();
                }
            });
        }
    }

});


app.controller('searchCtrl', function ($scope, myFactory) {
    $scope.searchText = null;
    
    GetEventsforSearch();
    function GetEventsforSearch() {
        $scope.eventList = [];
        var obj = {
            searchText: $scope.searchText
        }
        myFactory.postData(EndPoint.Service, "GetEventsforSearch", obj, function (res) {
            console.log(res);
            if (res.Success == true) {
                $scope.eventList = res.Result
                LoaderStop();
            }
            else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    $scope.selectSeach = function (d) {
        $scope.eventDeatil = d;
    }

    //$scope.$watch('searchText', function (newValue, oldValue) {
    //    $scope.eventDeatil = null;
    //});

    $scope.goSearch = function () {
        //alert($scope.eventDeatil);
        if ($scope.eventDeatil != null) {
            window.location.href = siteUrl + '/webapp/eventdetail/' + $scope.eventDeatil.eventId + '/' + $scope.eventDeatil.urlKey;
        }
       
    }

});