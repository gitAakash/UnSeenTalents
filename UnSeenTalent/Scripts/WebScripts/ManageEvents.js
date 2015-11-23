app.controller('ManageeventCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale) {





    //griddropdown end

    $scope.approvergriddatasource = new kendo.data.DataSource({
        transport: {
            read: function (e) {
              
                    var requestData = {
                        page: e.data.page,
                        pageSize: 20,
                        viewId: $scope.selectedItemvalue,
                        noOfRows: 1,
                        filter: e.data.filter,
                        skip: e.data.skip,
                        sort: e.data.sort,
                        type: "hello"
                    };

                    myFactory.postData(EndPoint.Service, "GetAllevntsforadmin", requestData, function (res) {
                        if (res.Success == true) {
                            e.success(res.Result);
                            LoaderStop();
                        }
                        else
                        {
                            e.success(res.Result);
                            LoaderStop();
                        }
                    });

                
            },
            update: function (e) {



            },
            create: function (options) {
                var localData = JSON.parse(localStorage["grid_data"]);
                options.data.ID = localData[localData.length - 1].ID + 1;
                localData.push(options.data);
                localStorage["grid_data"] = JSON.stringify(localData);
                options.success(options.data);
            },

        },
        schema: {

            data: "EventList",
            total: "total",
            model: {
                id: "eventid",
                fields: {
                    eventtitle: { type: "string", validation: { required: true, min: 1, Name: "Event name" } },
                    eventdesc: { type: "string", validation: { required: true, min: 1, Name: "Desc" } },
                    eventstartdate: { type: "date", validation: { required: true, min: 1, Name: "Startdate" } },
                    eventenddate: { type: "date", validation: { required: true, min: 1, Name: "End date" } },
                    isactive: { type: "byte", validation: { required: true, min: 1, Name: "Quantity name" } },
                  
                }

            },

        },
       // batch: true,
        //pageSize: 15,//modelParam.noOfRows,
        //serverPaging: true,
      //  serverSorting: true,
        //serverFiltering: true,


    });
    $scope.currencyDropDownEditor = function (container, options) {
        var editor = $('<input type="file" id="files" name="files" />')
              .appendTo(container).kendoUpload({

                  async: {

                      saveUrl: "http://rces-web/rcesonly/oandt/OandtWebService.asmx/UploadFile",

                      autoUpload: true

                  }

              });

    }

    $scope.ApproverGrid = {

        // specify the columns on the grid
        columns: [
                { field: "", title: "", filterable: true, sort: false, width: "30px", template: '<img rel="popover" id="#=eventid#" style="cursor: pointer; cursor: hand;" ng-hide="isDisabled" data-content="Click to delete." title="Click to delete." src="' + siteUrl + 'Images/Invoices/Delete.PNG" ng-click=\"clickToDelete(#=eventid#)\" />' },
               { field: "eventtitle", title: "Event Name", width: "150px" },
               { field: "eventdesc", title: "Event Desc", width: "150px" },
               { field: "eventstartdate", title: "Event Start date", width: "140px" },
               { field: "eventenddate", title: "Event end date", width: "140px" },
                //{ field: "eventimage", title: "Event end date", width: "180px", editor: $scope.currencyDropDownEditor },
            
              


        ],


        dataSource: $scope.approvergriddatasource,


        // selectable: "row",
      //  navigatable: true,
        pageable: true,
        pageable: {
            pageSize: 15,
            change: function (e) {

            }
        },
       // sortable: true,

        toolbar: ["create"],
        editable: "popup",
        //groupable: true,
        //filterable: true,
        //filterable: {
            //logic: "and",
            //extra: false,
            //operators: {
            //    string: {
            //        startswith: "Starts with",
            //        eq: "Is equal to",
            //        neq: "Is not equal to"

            //    },
            //    date:
            //        {
            //            eq: "Is equal to",
            //            neq: "Is not equal to",
            //            gte: "Is after or equal to",
            //            gt: "Is after",
            //            lte: "Is before or equal to",
            //            lt: "Is before"
            //        }

            //}
       // },


        editable: { //disables the deletion functionality
            update: true,

        },


        change: function (e) {

        },


    }


});