app.controller('ManageeventCtrl', function ($scope, $modal, $http, myFactory, $log, $filter, ngTableParams, $locale) {


    //------------------Reset All Fields----------------------
    function reSet() {
        $("div").removeClass("has-error");
        $scope.event = {
            eventid: null,
            eventtitle: null,
            eventdesc: null,
            eventstartdate: null,
            eventenddate: null,
            eventImage: null,
        };
        $scope.add = false;
        $scope.validationErrors = [''];
    }
    reSet();
    getAllEvents();
    //---Fetch Country Data for Dropdown
    function getAllEvents() {
        myFactory.postData(EndPoint.Service, "GetAllevntsforadmin", null, function (res) {
            if (res.Success == true) {
                bindData(res.Result.EventList);
            }
            else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }
    var listData = [];
    function bindData(data) {
        listData = data;

        $scope.tableParams = new ngTableParams({
            page: 1,            // show first page
            count: 15,          // count per page
            sorting: {
                cityName: 'asc'  // initial sorting
            }
        }, {
            total: listData.length, // length of data
            getData: function ($defer, params) {
                // use build-in angular filter
                var filteredData = params.filter() ?
                        $filter('filter')(listData, params.filter()) :
                        listData;
                var orderedData = params.sorting() ?
                        $filter('orderBy')(filteredData, params.orderBy()) :
                        listData;

                params.total(orderedData.length); // set total for recalc pagination
                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        });
        LoaderStop();
    }


    //----Reload Table Data-----
    function reloadTableData() {
        LoaderStart();

        $http({
            method: "POST",
            url: EndPoint.Service + 'GetAllevntsforadmin',
            data: {}
        }).success(function (data) {
            listData = data.Result.EventList;
            $scope.tableParams.reload();
            $scope.tableParams.page(1);
            LoaderStop();
        });
    }

    //......Date Picker for EventFromDate........
    $('#txtStartDate').datepicker({
        controlType: 'select',
        timeFormat: 'hh:mm tt',
        changeYear: true,
        yearRange: '-90:+0',
        onSelect: function (selected) {
            var from = new Date($(this).datepicker('getDate'));
            $("#txtEndDate").datepicker("option", "minDate", selected)
            $('#txtEndDate').val("");
            console.log(selected);
            $scope.event.eventstartdate = selected;
        }
    });

    //......Date Picker for End Date........
    //$('#txtEndDate').datepicker({
    //    controlType: 'select',
    //    timeFormat: 'hh:mm tt',
    //    changeYear: true,
    //    yearRange: '-90:+0',
    //    onSelect: function (selected) {
    //        var to = new Date($(this).datepicker('getDate'));
    //        console.log(selected);
    //        $scope.event.eventenddate = selected;
    //    }
    //});

    //------------------Add/Edit Data----------------------
    $scope.saveData = function(postData) {
        var inputs = ["txtTitle", "txtStartDate"];
        var isValid = isValidElements(inputs);
        if (isValid) {

            myFactory.postData(EndPoint.Service, "SaveEditEvents", postData, function(res) {
                if (res.Success == true) {
                    reloadTableData();
                    $('#dataModelPoup').modal('hide');
                    reSet();
                    showError("success", "Event has been saved successfully");
                } else {
                    showError("danger", "Error occured");
                    LoaderStop();
                }
            });

        }
    };

    $scope.UpdateEvents = function (updateData) {
        $scope.event = {
            eventid: updateData.eventid,
            eventtitle: updateData.eventtitle,
            eventdesc: updateData.eventdesc,
            eventstartdate: updateData.eventstartdate,
            eventenddate: updateData.eventenddate,
        };
        $('#dataModelPoup').modal('show');
        scrollTop();
    }

    //-----ConfirmBox(Active/InActive/Delete)
    $scope.ActiveInactiveDeleteEvents = function(type, data) {
        var con = 'InActivate';
        if (type == 'Unblock') {
            con = 'Activate';
        } else if (type == 'Delete') {
            con = 'Delete';
        }
        var modalInstance = $modal.open({
            templateUrl: '/HtmlTemplates/ConfirmBox.html',
            controller: function($scope, $modalInstance) {
                $scope.id = data.eventid;
                $scope.option = con;
                $scope.ok = function(type1, id) {
                    $modalInstance.close(); //Close modal
                    var postData = {
                        Type: type,
                        ID: id
                    };

                    myFactory.postData(EndPoint.Service, "EventAction", postData, function(res) {
                        if (res.Success == true) {
                            showError("success", res.Message);
                            reloadTableData();
                            LoaderStop();
                        } else {
                            showError("danger", res.Message);
                            LoaderStop();
                        }
                    });

                };
                $scope.cancel = function() {
                    $modalInstance.dismiss('cancel');
                };

            },

        });
    };
    showError(null, null);
    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }

    //------File Uploader-----
    $scope.uploadFile = function (files) {
        LoaderStart();
        var fd = new FormData();
        //Take the first selected file
        fd.append("file", files[0]);
        $http.post('/Uploader.ashx', fd, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (img) {
            $scope.event.eventImage = img;
            LoaderStop();
        }).error(function () { });
    };
});