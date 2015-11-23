// Invoice Grid Angular Code

app.controller('MyGridCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale, $http) {

    var cities = [];
    var checkdataisedit = [];
    var checkAllEdit = 0;
    var statusforinvoicegrid = 0;
    showError(null, null);
    $scope.data = {
        emailId: null,
        password: null
    };
    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }
    $scope.GetStatusValue = function ondrdchange(a) {
        //var checkedit = CheckDataEdited();
        if (CheckDataEdited() == true) {
            // if (checkAllEdit == 0) {
            statusforinvoicegrid = a;
            //GetGridDropDown();
            $scope.InvoiceGrid.dataSource.read();
            //}
        }

    };


    $scope.init = function () {
        var requestData = {
            page: 1,
            pageSize: 1,
            viewId: $scope.selectedItemvalue,
            type: "hello"
        };

        $http({ method: 'POST', url: '/api/pasdejapi/GetStatusForGridDrd', data: requestData }).
            success(function (values, status, headers, config) {
                //console.log(values.Result.data);
                $.each(values.Result.data, function (key, val) {
                    cities.push(val.Name);
                    LoaderStop();
                });
            }).
            error(function (data, status, headers, config) {
                console.log(status);
            });


    }


    function GetGridDropDown() {
        var requestData = {
            page: 1,
            pageSize: 1,
            viewId: $scope.selectedItemvalue,
            type: "hello"
        };

        $http({ method: 'POST', url: '/api/pasdejapi/GetStatusForGridDrd', data: requestData }).
            success(function (values, status, headers, config) {
                //console.log(values.Result.data);
                $.each(values.Result.data, function (key, val) {
                    cities.push(val.Name);
                    LoaderStop();
                });
            }).
            error(function (data, status, headers, config) {
                console.log(status);
            });
    }

    //Grid Dropdown start
    $scope.ddlcurrencyDataSource = new kendo.data.DataSource({
        type: "jsonp",
        transport: {
            read: siteUrl + "/api/PasdejApi/GetCurrency"
        }
    });

    $scope.currencyDropDownEditor = function (container, options) {

        var editor = $('<input kendo-drop-down-list required k-data-text-field="\'Currency\'" k-data-value-field="\'Currency\'" k-data-source="ddlcurrencyDataSource" data-bind="value:' + options.field + '"/>')
         .appendTo(container);


    }

    //griddropdown end





    $scope.drdStatus = new kendo.data.DataSource({
        serverFiltering: true,
        data: 'data',
        transport: {
            dataType: "jsonp",
            read: function (e) {
                var requestData = {
                    page: e.data.page,
                    pageSize: e.data.pageSize,
                    viewId: statusforinvoicegrid,
                    type: "hello"
                };
                $http({ method: 'POST', url: '/api/pasdejapi/GetStatusForDrd', data: requestData }).
                success(function (data, status, headers, config) {
                    e.success(data.Result.data);
                    $scope.selectedItemvalue = "0";
                    LoaderStop();
                }).
                error(function (data, status, headers, config) {

                    console.log(status);
                });

            },
        }


    });
    //ddl num of rows
    $scope.drdRowNum = new kendo.data.DataSource({
        serverFiltering: true,
        data: 'data',
        transport: {
            dataType: "jsonp",
            read: function (e) {
                var requestData = {
                    page: e.data.page,
                    pageSize: e.data.pageSize,
                    viewId: statusforinvoicegrid,
                    noOfRows: modelParam.noOfRows,
                    type: "hello"
                };
                $http({ method: 'POST', url: '/api/pasdejapi/GetRowNumForDrd', data: requestData }).
                success(function (data, status, headers, config) {
                    e.success(data.Result.data);
                    LoaderStop();
                }).
                error(function (data, status, headers, config) {

                    console.log(status);
                });

            },
        }


    });



    var modelParam = {
        viewId: 0,
        noOfRows: 15
    };

    var numberofrowsforinvoicegrid = 15;
    $scope.GetNoOfRows = function ondrdchange(a) {
        if (CheckDataEdited() == true) {
            //if (checkAllEdit == 0) {
            numberofrowsforinvoicegrid = a;
            $scope.InvoiceGrid.dataSource.pageSize(a);
            $scope.InvoiceGrid.dataSource.read();
        }
    };



    function cityFilter(element) {
        //cities = ["Seattle", "Tacoma", "Kirkland", "Redmond", "London", "Philadelphia", "New York", "Seattle", "London", "Boston"],
        element.kendoDropDownList({
            //dataSource: cities,

            datasource: new kendo.data.DataSource({
                serverFiltering: true,
                data: 'data',
                transport: {
                    dataType: "jsonp",
                    read: function (e) {
                        var requestData = {
                            page: e.data.page,
                            pageSize: e.data.pageSize,
                            viewId: statusforinvoicegrid,
                            noOfRows: modelParam.noOfRows,
                            type: "hello"
                        };
                        $http({ method: 'POST', url: '/api/pasdejapi/GetStatusForGridDrd', data: requestData }).
                        success(function (data, status, headers, config) {
                            e.success(data.Result.data);
                            LoaderStop();
                        }).
                        error(function (data, status, headers, config) {

                            console.log(status);
                        });

                    },
                }


            })
            //optionLabel: "--Select Value--"
        });
    }

    //var filter = {
    //    logic: "and",
    //    filters: [
    //        { field: "CreatedOn", operator: "ge" },
    //        { field: "CreatedOn", operator: "le" }
    //    ]
    //};




    $scope.InvoiceGrid = {
        // specify the columns on the grid
        columns: [
              //{ field: "RecordKey" },

              //{ field: "", template: '<button class=k-button ng-click=OnEdit(this)><img src="' + siteUrl + '" +Images/Invoices/Delete.PNG /></button>' },
              { field: "", title: "", filterable: true, sort: false, width: "100px", template: '<img rel="popover" style="cursor: pointer; cursor: hand;" data-content="Click to go to invoice detail." title="Click to go to invoice detail." src="' + siteUrl + 'Images/Invoices/Info.PNG" ng-click=\"clickToInvoiceDetail(#=RecordKey#)\" /><img rel="popover" id="#=RecordKey#" style="cursor: pointer; cursor: hand;" ng-hide="isDisabled" data-content="Click to delete." title="Click to delete." src="' + siteUrl + 'Images/Invoices/Delete.PNG" ng-click=\"clickToDelete(#=RecordKey#)\" /><img style="cursor: pointer; cursor: hand;" rel="popover" id="u' + '#=RecordKey#" ng-hide="isDisabled" data-content="Click to unmatch." title="Click to unmatch." src="http://localhost:50330/Images/Invoices/Unmatch.PNG" ng-click=\"clickToUnmatch(#=RecordKey#)\" />' },
               //{ field: "", title: "", filterable: false, sort: false, width: "100px", template: '<img rel="popover" data-content="Click to unmatch." title="Click to unmatch." src="http://localhost:50330/Images/Invoices/Unmatch.PNG" ng-click=\"clickToUnmatch(#=RecordKey#)\" />' },
              {
                  field: "CreatedOn", title: "CreatedOn", width: "200px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"], filterable: true,
                  // parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"], filterable: { ui: "datetimepicker", logic: "or", extra: true, format: "{0:yyyy-MM-dd HH:MM}", parseFormats: ["yyyy-MM-dd'T'HH:MM"] }
              },
              { field: "DocumentNumber", title: "DocumentNumber", width: "150px" },
              { field: "InvoiceNumber", title: "InvoiceNumber", width: "150px", validation: { max: 15 } },
              { field: "VendorNumber", title: "VendorNumber", width: "200px" },
              {
                  field: "InvoiceDate", title: "InvoiceDate", width: "200px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"]
              },
              { field: "BilledValue", title: "BilledValue", width: "200px", },
              { field: "VATValue", title: "VATValue", width: "200px" },
              { field: "VATCode", title: "VATCode", width: "200px" },
              { field: "DeliveryNote", title: "DeliveryNote", width: "200px" },
              { field: "ArticleNumber", title: "ArticleNumber", width: "200px" },
              { field: "Quantity", title: "Quantity", width: "200px" },
              { field: "TotalPricePerLine", title: "TotalPricePerLine", width: "200px" },
              { field: "Currency", title: "Currency", width: "180px", editor: $scope.currencyDropDownEditor, template: "#=Currency#" },
              //{ field: "Currency", title: "Currency", width: "200px" },
              { field: "VATValueSEK", title: "VATValueSEK", width: "200px" },
              { field: "DocumentID", title: "DocumentID", width: "200px" },
              { field: "Match", title: "Match", width: "200px" },
              {
                  field: "MatchDate", title: "Match Date", width: "200px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"]
              },
              { field: "StatusName", title: "Status", width: "200px", filterable: { ui: cityFilter, extra: false } },
               { field: "Comment", title: "Comment", width: "200px" },
              //editable: false, template: '<input type="checkbox" ng-model="dataItem.processed" />'
              //{ field: "Action", width: "100px" },

        ],
        // the schema defines the schema of the JSON coming
        // back from the server so the datasource can parse it
        dataSource: new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    if (CheckDataEdited() == true) {
                        //if (checkAllEdit == 0) {
                        var requestData = {
                            page: e.data.page,
                            pageSize: e.data.pageSize,
                            viewId: $scope.selectedItemvalue,
                            noOfRows: numberofrowsforinvoicegrid,
                            filter: e.data.filter,
                            skip: e.data.skip,
                            sort: e.data.sort,
                            type: "hello"
                        };

                        //  console.log(requestData);
                        $http({ method: 'POST', url: '/api/pasdejapi/GetRecordbypaging', data: requestData }).
                        success(function (data, status, headers, config) {
                            e.success(data);
                            console.log(data);
                            //cities.push(val.Name);
                            if (statusforinvoicegrid === '10') {  // special check for Posted in SAP.

                                $scope.isDisabled = true;
                            }
                            else {
                                $scope.isDisabled = false;
                            }
                            //console.log(data);
                            LoaderStop();
                        }).
                        error(function (data, status, headers, config) {

                            // console.log(status);
                        });
                    }
                    else {

                    }


                },
                update: function (e) {
                    // batch is enabled
                    //var updateItems = e.data.models;
                    // batch is disabled
                    var updatedItem = e.data;
                    // alert(updatedItem);

                    var requestData = {
                        page: e.data.page,
                        pageSize: e.data.pageSize,
                        viewId: statusforinvoicegrid,
                        noOfRows: modelParam.noOfRows,
                        filter: e.data.filters,
                        skip: e.data.skip,
                        sort: e.data.sort,
                        type: "hello"
                    };

                    $http({ method: 'POST', url: '/api/pasdejapi/GetRecordbypaging', data: requestData }).
                    success(function (data, status, headers, config) {
                        e.success(data);
                        LoaderStop();
                        //console.log(data);
                    }).
                    error(function (data, status, headers, config) {

                        console.log(status);
                    });


                    // save the updated item to the original datasource
                    // ...

                    // on success
                    // e.success();
                    // on failure
                    //e.error("XHR response", "status code", "error message");
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
                // the array of repeating data elements (employees)
                data: "d",
                total: "Total",
                model: {
                    id: "RecordKey",
                    fields: {
                        // specify all the model fields, along with validation rules and whether or
                        // not they can be edited or nulled out.
                        IsDeleted: { editable: false, filterable: false, template: '# { <img src="http://localhost:50330/Images/Invoices/Delete.PNG" ng-click=\"clickToDelete(#=RecordKey#)\" }#' },
                        CreatedOn: { editable: false, type: "date", format: "{0:yyyy-dd-MM hh:mm}" },
                        InvoiceNumber: { type: "string", validation: { min: 1, max: 15 } },
                        VATValue: { type: "number", validation: { max: 10 } },
                        InvoiceDate: { type: "date", format: "{0:dd-MMM-yyyy hh:mm}" },
                        DocumentNumber: { editable: false, type: "string" },
                        DocumentID: { editable: false },
                        Match: { editable: false, type: "string" },
                        MatchDate: { editable: false, type: "date" },
                        BilledValue: { type: "number", editable: "#if('" + statusforinvoicegrid + "' == 1) {#false#} else{#true#}#" },
                        TotalPricePerLine: { type: "number" },
                        VATValueSEK: { type: "number" }
                    }
                },

            },
            batch: true,
            pageSize: 15,//modelParam.noOfRows,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,


        }),


        // selectable: "row",
        navigatable: true,
        pageable: true,
        sortable: true,
        groupable: true,
        filterable: true,
        filterable: {
            //logic: "and",
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    eq: "Is equal to",
                    neq: "Is not equal to"

                },
                date:
                    {
                        eq: "Is equal to",
                        neq: "Is not equal to",
                        gte: "Is after or equal to",
                        gt: "Is after",
                        lte: "Is before or equal to",
                        lt: "Is before"
                    }

            }
        },

        //filterable: {
        //    logic: "and",
        //    extra: false,
        //    operators: {
        //        date: {
        //            gte: "Begin Date",
        //            lte: "End Date"
        //        },
        //        //date:
        //        //    {
        //        //        logic: "and",
        //        //        gte: "Begin Date",
        //        //        lte: "End Date"
        //        //    }
        //    }
        //},
        editable: { //disables the deletion functionality
            update: true,
            // destroy: false
        },


        change: function (e) {
            //alert('coming on page click');
            //console.log("grid pager clicked!");
        },
        edit: function OnEdit(e) {
            if (!e.model.isNew()) {
                //var kendoTextBox = e.container.find("input[name=InvoiceNumber]")[0];
                //console.log("kendoTextBox" + kendoTextBox.html);
                //alert(kendoTextBox);
                //if (kendoTextBox.value == 1) {
                //    var disableField = e.container.find("input[name=InvoiceNumber]")[0];
                //    disableField.enable(false);
                //    //this.closeCell();
                //}

                //var catalogproductid = e.container.find("input[name=InvoiceNumber]").data("numerictextbox").value();
                if (statusforinvoicegrid == 1) {
                    //alert('coming');
                    $("input[name=InvoiceNumber]").attr("readonly", true);
                    //var disableField = e.container.find("input[name=InvoiceNumber]").data("numerictextbox");
                    //disableField.enable(false);
                }
                //var catalogproductid = e.container.find("input[name=Status]").value();
                //// Disable DiscountPercentageMRC if catalog productid = 100
                //if (catalogproductid == 1) {


                //    var disableField = e.container.find("input[name=InvoiceNumber]").data();
                //    disableField.enable(false);
                //}
            }
        },
        change: Onchange,
        // event: { databound: "onDataBound" }




    }

    //function onDataBound()
    //{
    //    alert('Coming');
    //}

    function Onchange(e) {
        console.log(e);
    }

    function OnEdit(e) {

        // Make sure it's not a new entry

        //if (!e.model.isNew()) {
        //    var catalogproductid = e.container.find("input[name=CatalogProductId]").data("kendoNumericTextBox").value();

        //    // Disable DiscountPercentageMRC if catalog productid = 100
        //    if (catalogproductid == 100) {
        //        var disableField = e.container.find("input[name=DiscountPercentageMRC]").data("kendoNumericTextBox");
        //        disableField.enable(false);
        //    }
        //}
        LoaderStop();
    }

    $scope.handleChange = function (data, dataItem, columns) {

        $scope.data = data;
        $scope.columns = columns;
        $scope.dataItem = dataItem;
        //console.log(data);
        //console.log(dataItem);
        //console.log(columns);
    };

    $scope.refresh = function () {
        // scope.grid is the widget reference
        $scope.grid.refresh();
    }

    //for containing the delete recordkeys 
    $scope.deletedRows = [];
    $scope.clickToDelete = function (i) {
        if ($scope.deletedRows.indexOf(i) == -1) {
            if (confirm("Are you sure?  You want to delete this record?")) {
                $scope.deletedRows.push(i);
                $('#' + i).attr('src', '' + siteUrl + 'Images/Invoices/delete-hover.PNG'); //DeletedRed.PNG
            }
            else {

            }

        }
        else {
            if (confirm("Record already selected for delete. Do you want to unselect?")) {
                $scope.deletedRows.pop(i);
                $('#' + i).attr('src', '' + siteUrl + 'Images/Invoices/Delete.PNG'); //DeletedRed.PNG
            }
            else {

            }

        }
    };

    //for containning the unmatch keys
    $scope.unmatchRows = [];
    $scope.clickToUnmatch = function (i) {
        if ($scope.unmatchRows.indexOf(i) == -1) {

            if (confirm("Are you sure?  You want to unmatch this record?")) {
                $scope.unmatchRows.push(i);
                $('#u' + i).attr('src', '' + siteUrl + '/Images/Invoices/UnmatchRed.PNG'); //UnmatchRed.PNG
            }
            else {

            }
        }
        else {
            if (confirm("Record already selected for unmatch. Do you want to unselect?")) {
                $scope.unmatchRows.pop(i);
                $('#u' + i).attr('src', '' + siteUrl + 'Images/Invoices/Unmatch.PNG'); //Unmatch.PNG
            }
            else {

            }
        }
    };


    $scope.clickToInvoiceDetail = function (i) {
        //window.locatio.href = siteUrl + "";
        window.location.href = siteUrl + "InvoiceMatching/InvoiceDetails?Id=" + i;
    };




    //SaveUpdateGrid
    $scope.SaveGrid = function (txt) {
        //grid.saveChanges();


        //alert("Has class or not" + $("#divGrid").hasClass("k-invalid"));

        var o = new Object();
        o.page = 1;
        $scope.updatedRecords = [];
        var data = $scope.InvoiceGrid.dataSource.data();
        for (var i = 0; i < data.length; i++) {
            console.log(data[i]);
            if (data[i].isNew()) {
                //this record is new
                //newRecords.push(currentData[i].toJSON());
            } else if (data[i].dirty) {
                $scope.updatedRecords.push(data[i].toJSON());
                //alert(updatedRecords);
                //console.log(updatedRecords);
            }
        }
        if ($scope.deletedRows.length > 0 || $scope.unmatchRows.length > 0 || $scope.updatedRecords.length > 0) {
            if (confirm("Are you sure to save changes?")) {
                o.currentUser = $("#hdnUserEmail").val();
                o.AllRecords = data;
                o.data = $scope.updatedRecords;//data; // $scope.InvoiceGrid.dataSource.read();
                o.deletekeys = $scope.deletedRows,
                o.unmatchkeys = $scope.unmatchRows,

                myFactory.postData(EndPoint.Service, "SaveUpdateGrid", o, function (res) {
                    if (res.Success == true) {
                        // for refreshing grid useful           
                        //$scope.deletedRows = [];
                        $scope.deletedRows.length = 0;
                        //$scope.unmatchRows = [];
                        $scope.unmatchRows.length = 0;
                        $scope.updatedRecords = [];
                        checkdataisedit = [];
                        var dt = new Date();
                        var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                        $("#spnMsg").html("");
                        $("#spnMsg").html("Saved: " + time);
                        $("#spnMsg").fadeOut(60000);
                        $scope.InvoiceGrid.dataSource.read();
                        LoaderStop();
                    }
                    else {
                        $scope.deletedRows.length = 0;
                        $scope.unmatchRows.length = 0;
                        updatedRecords.length = 0;
                        //alert('Failed!');
                        $("#spnMsg").html("");
                        $("#spnMsg").html("Failed to update the data.");
                        $("#spnMsg").fadeOut(60000);
                        LoaderStop();
                    }
                    console.log();
                });
            }
        }
    }

    //Export Invoices
    $scope.ExportInvoice = function (val) {
        var o = new Object();
        o.ID = val.viewId;

        //var gridData = $("#grid").data("kendoGrid");
        //gridData.dataSource.filter();

        //var data = $scope.InvoiceGrid.dataSource.data().filter();
        console.log(data);
        var status = $scope.selectedItemvalue;

        window.location.href = EndPoint.Service + "DownloadInvoices?ID=" + status;
        LoaderStop();
    }

    //Import Invoice popup open
    $scope.ImportInvoice = function (val) {
        console.log(val);
        $("#fileinvoiceupdate").replaceWith($("#fileinvoiceupdate").clone());
        $('#myModal').modal('show');
    }

    //Export Invoices
    //Import modified invoices


    $scope.SelectedFileForModifiedInvoiceUpload = null;
    $scope.uploadFileModifiedInvoice = function (file) {
        $scope.SelectedFileForModifiedInvoiceUpload = file[0];

    };

    $scope.imprtInvoicesUpdateClickfunc = function () {
        var fd = new FormData();
        fd.append("file", $scope.SelectedFileForModifiedInvoiceUpload);
        fd.append("currentuser", $("#hdnUserEmail").val());

        var url = EndPoint.Service + "ImportModifiedInvoices";
        myFactory.uploadfile(url, fd, function (res) {
            if (res.Success == true) {

                var message = res.Result;
                showError("success", message);

                HidePopUp();
                LoaderStop();
                $scope.InvoiceGrid.dataSource.read();
            }
            else {

                var message = res.Result;
                showError("warning", message);
                HidePopUp();
                //LoaderStop();
                //$scope.InvoiceGrid.dataSource.read();

                LoaderStop();
            }
            console.log();
        });
    }


    function HidePopUp() {
        $('#myModal').modal('hide');
        $scope.SelectedFileForModifiedInvoiceUpload = null;
        LoaderStop();
    }

    //Match Invoice Functionality
    $scope.MatchInvoice = function (val) {
        var o = new Object();
        o.page = 1;
        myFactory.postData(EndPoint.Service, "MatchInvoices", o, function (res) {
            if (res.Success == true) {
                var dt = new Date();
                var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                $("#spnMsg").html("");
                $("#spnMsg").html("Match Invoice: " + time);
                $("#spnMsg").fadeOut(60000);
                LoaderStop();
            }
            else {
                $("#spnMsg").html("");
                $("#spnMsg").html("Failed to Match Invoice");
                $("#spnMsg").fadeOut(60000);
                LoaderStop();
            }

        });
    }


    // function to check whether any changes has been made to grid or not
    function CheckDataEdited() {
        var IsEdit = false;
        var data = $scope.InvoiceGrid.dataSource.data();
        for (var i = 0; i < data.length; i++) {
            if (data[i].isNew()) {
                //this record is new
                //newRecords.push(currentData[i].toJSON());
            } else if (data[i].dirty) {
                checkdataisedit.push(data[i].toJSON());
                if (checkdataisedit.length > 0) {
                    if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                        return true;
                    }
                    else {
                        return false;
                    }
                    //$('#myConfirmModal').modal('show');
                }
            }
            if ($scope.deletedRows.length > 0) {
                if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                    $scope.deletedRows = [];

                    return true;
                }
                else {
                    return false;
                }
                //$('#myConfirmModal').modal('show');
            }
            if ($scope.unmatchRows.length > 0) {
                if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                    $scope.unmatchRows = [];
                    return true;
                }
                else {
                    return false;
                }

            }

        }
        return true;
        //   return IsEdit;
    }
   
});
