app.controller('EventFileUploadCtrl', function($scope, $http, myFactory, $log, $filter, ngTableParams, $locale) {
    reSet();
    bindCategoryDropdown();
    bindEventDropdown();
    bindAvaliableTokenDropdown();
    showError(null, null);
    getRecentEvents();

    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }

    function getRecentEvents() {
        myFactory.postData(EndPoint.Service, "GetRecentEvents", null, function (res) {
            if (res.Success == true) {
                $scope.recentevents = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    function bindAvaliableTokenDropdown() {
        myFactory.postData(EndPoint.Service, "GetAvaliableTokenForUser", null, function (res) {
            if (res.Success == true) {
                debugger;
                $scope.avaliableTokens = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    function reSet() {
        $("div").removeClass("has-error");
        $scope.eventUpload = {
            SelectedCategory: 0,
            SelectedEvent: 0,
           // Token: null,
            Description: null,
            FileUrl: null,
            FileName: null,
            File: null,
            SelectedToken : null
        };
        $scope.validationErrors = [''];
    }

    $scope.saveEventUploadData = function(postData) {
        myFactory.postData(EndPoint.Service, "SaveUploadedFileDetails", postData, function (res) {
            if (res.Success == true) {
                bindAvaliableTokenDropdown();
                reSet();
                LoaderStop();
                showError("success", res.Message);
               
            } else {
                bindAvaliableTokenDropdown();
                showError("danger", res.Message);
                LoaderStop();
            }
        });
    };

    function bindCategoryDropdown() {
        var categoriesArray = new Array();
        $.ajax({
            type: "GET",
            headers: { 'Authentication': 'web60134' },
            url: EndPoint.Service + "GetAllCategories",
            contentType: false,
            processData: false,
            async: false,
            data: {},
            success: function(response) {
                if (response.Success) {
                    for (var i = 0; i < response.Result.length; i++) {
                        var obj = new Object();
                        obj.Name = response.Result[i].CategoryName;
                        obj.Id = response.Result[i].CategoryId;
                        obj.Selected = false;
                        categoriesArray.push(obj);
                    }
                    $scope.Categories = categoriesArray;
                }
            },
            error: function() {
                console.log(response.Result);
            }
        });
    }

    function bindEventDropdown() {
        var events = new Array();
        myFactory.postData(EndPoint.Service, "GetAllEventsForVideoUpload", null, function (res) {
            if (res.Success == true) {
                LoaderStop();
                if (res.Success) {
                    for (var i = 0; i < res.Result.EventList.length; i++) {
                        var obj = new Object();
                        obj.Name = res.Result.EventList[i].eventtitle;
                        obj.Id = res.Result.EventList[i].eventid;
                        obj.Selected = false;
                        events.push(obj);
                    }
                    $scope.Events = events;
                }
            } else {
                console.log(res.Result);
            }
        });
    }

});