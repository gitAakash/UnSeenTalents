app.controller('EventListCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale) {


    getAllEvents();
    //---Fetch Country Data for Dropdown
    function getAllEvents() {
        myFactory.postData(EndPoint.Service, "GetAllevntsforWeb", null, function (res) {
            if (res.Success == true) {
                $scope.eventList = res.Result;
                LoaderStop();
            }
            else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }
});