
// Invoice Grid Angular Code
//<summary>
//Title:Invoice.js
//Description:This page contain code for paging,display Currency in dropdown,codes connecting to pasdej api controller methods etc.
//Copyrights:
//Company:HCL
//Date:29-june-2015
//@filename:Invoice.js
//@author:<a href"mail to:bhumika_s@hcl.com">Bhumika Sharma</a>
//</summary>
//<remarks>
// ********  Modification History  **********************
// Sr No:   Date   Modified by    Tracker     Description
// ******************************************************
// </remarks>

app.controller('MyGridCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale, $http, $window) {

    var cities = [];
    var currency = [];
    var checkdataisedit = [];
    var checkAllEdit = 0;
    var statusforinvoicegrid = 0;
    var isFromSave = 0;
    showError(null, null);
    $scope.data = {
        emailId: null,
        password: null
    };
    // $scope.theContent = "";
    function showError(type, msg) {
        $scope.errorType = type;
        $scope.errorMsg = msg;
    }
    $scope.GetStatusValue = function ondrdchange(a) {
        //var checkedit = CheckDataEdited();
        showError(null, null);
        //if (CheckDataEdited() == true) {
        // if (checkAllEdit == 0) {
        statusforinvoicegrid = a;
        if (statusforinvoicegrid != 10 && statusforinvoicegrid != 11) {
            $('.k-grid-filter').eq(17).addClass("displaynone");
            $('.k-grid-filter').eq(18).addClass("displaynone");
            $('.k-grid-filter').eq(17).removeClass("displayblock");
        }
        else {
            $('.k-grid-filter').eq(17).addClass("displayblock");
            $('.k-grid-filter').eq(18).addClass("displaynone");
            $('.k-grid-filter').eq(17).removeClass("displaynone");
        }
        $scope.InvoiceGrid.dataSource.read();
        //var grid = $("#kendoinvoicegrid").data("kendoGrid");
        //grid.dataSource.filter({});
        //console.log(grid);
        //GetGridDropDown();
        //cityFilter(grid);

        //}

    };


    $scope.init = function () {
        GetGridDropDown();
        GetCurrencyDropDown();
    }


    function GetGridDropDown() {
        var requestData = {
            page: 1,
            pageSize: 1,
            viewId: $scope.selectedItemvalue,
            type: "hello"
        };

        $http({ method: 'POST', url: siteUrl + '/api/pasdejapi/GetStatusForDrd', data: requestData }).
            success(function (values, status, headers, config) {
                //console.log(values.Result.data);
                $.each(values.Result.data, function (key, val) {

                    cities.push(val.Name);

                    LoaderStop();
                });
                cities.splice(-2, 2);
                //cities = [];
            }).
            error(function (data, status, headers, config) {
                console.log(status);
            });
    }

    //filter Currency dropdown
    function GetCurrencyDropDown() {
        var requestData = {
            page: 1,
            pageSize: 1,
            viewId: $scope.selectedItemvalue,
            type: "hello"
        };

        $http({ method: 'POST', url: siteUrl + '/api/PasdejApi/GetCurrencyFilter', data: requestData }).
            success(function (values, status, headers, config) {
                //console.log(values.Result.data);
                $.each(values.Result.data, function (key, val) {
                    //console.log(val);
                    currency.push(val.Currency);
                    LoaderStop();
                });
                //cities = [];
            }).
            error(function (data, status, headers, config) {
                console.log(status);
            });
    }

    function cityFilter(element) {
        //cities = ["Seattle", "Tacoma", "Kirkland", "Redmond", "London", "Philadelphia", "New York", "Seattle", "London", "Boston"],
        //console.log(element);
        //alert(1);
        element.kendoDropDownList({
            dataSource: cities,
            optionLabel: "--Select Value--"
        });
    }


    function currencyFilter(element) {
        element.kendoDropDownList({
            dataSource: currency,
            optionLabel: "--Select Value--"
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
        if (statusforinvoicegrid === '1') {

        }
        else {
            var editor = $('<input  kendo-drop-down-list required k-data-text-field="\'Currency\'" k-data-value-field="\'Currency\'" k-data-source="ddlcurrencyDataSource" data-bind="value:' + options.field + '" name="gridcurrency" id="gridcurrency" />')
           .appendTo(container);
        }

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
                $http({ method: 'POST', url: siteUrl + '/api/pasdejapi/GetStatusForDrd', data: requestData }).
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
                $http({ method: 'POST', url: siteUrl + '/api/pasdejapi/GetRowNumForDrd', data: requestData }).
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
        showError(null, null);
        // if (CheckDataEdited() == true) {
        //if (checkAllEdit == 0) {
        numberofrowsforinvoicegrid = a;
        $scope.InvoiceGrid.dataSource.pageSize(a);
        $scope.InvoiceGrid.dataSource.read();

        // }
    };





    //var filter = {
    //    logic: "and",
    //    filters: [
    //        { field: "CreatedOn", operator: "ge" },
    //        { field: "CreatedOn", operator: "le" }
    //    ]ata.Reco
    //};





    $scope.InvoiceGrid = {
        // specify the columns on the grid
        columns: [

              { field: "", title: "", filterable: false, sort: false, width: "100px", template: '<div class="icons"><ul><a id="infoId" ng-click=\"clickToInvoiceDetail(#=RecordKey#)\"><li class="info infoIcons infoIconsOne"></a></li><li class="delete infoIcons"><img rel="popover" id="#=RecordKey#" style="cursor: pointer; cursor: hand;" ng-hide="isDisabled" data-content="Click to delete." ng-click=\"clickToDelete(#=RecordKey#)"\ title="Click to delete." src="' + siteUrl + 'Images/Invoices/delete.PNG"/></li><li class="matched infoIcons"><img style="cursor: pointer; cursor: hand;" rel="popover" id="u' + '#=RecordKey#" ng-hide="isDisabled" data-content="Click to unmatch." title="Click to unmatch." src="' + siteUrl + '/Images/Invoices/unmatch.PNG" ng-click=\"clickToUnmatch(#=RecordKey#)\" /></ul></div>' },
              {
                  field: "CreatedOn", title: "Created", width: "160px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"], filterable: true,
                  // parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"], filterable: { ui: "datetimepicker", logic: "or", extra: true, format: "{0:yyyy-MM-dd HH:MM}", parseFormats: ["yyyy-MM-dd'T'HH:MM"] }
              },

              { field: "DocumentNumber", title: "Document no", width: "150px" },

              { field: "InvoiceNumber", title: "Invoice no", width: "150px", validation: { max: 15 } },
              { field: "VendorNumber", title: "Vendor no", maxlength: "8", width: "120px", },

              {
                  field: "InvoiceDate", title: "Invoice date", width: "150px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"]
              },
              { field: "BilledValue", title: "Billed value", width: "140px", max: "10" },
              { field: "VATValue", title: "VAT value", width: "130px" },
              { field: "VATCode", title: "VAT code", width: "130px" },
              { field: "DeliveryNote", title: "Delivery note", width: "140px" },
              { field: "ArticleNumber", title: "Article no", width: "150px" },
              { field: "Quantity", title: "Quantity", width: "130px" },
              { field: "TotalPricePerLine", title: "Total price", width: "160px" },
              { field: "Currency", title: "Currency", width: "130px", editor: $scope.currencyDropDownEditor, template: "#=Currency#", filterable: { ui: currencyFilter, extra: false } },
            
              { field: "VATValueSEK", title: "VAT-value SEK", width: "150px" },
              { field: "DocumentID", title: "Document ID", width: "160px" },
              { field: "Match", title: "Match", width: "90px" },
              {
                  field: "MatchDate", title: "Match date", width: "150px", type: "date", format: "{0:yyyy-MM-dd HH:MM}",
                  parseFormats: ["yyyy-MM-dd'T'HH:mm:ss.zz"]
              },
              { field: "StatusName", title: "Status", width: "150px", filterable: { ui: cityFilter, extra: false } },

              { field: "Comment", title: "Add comments", width: "200px", filterable: true }, // false just for testing purpose
              //editable: false, template: '<input type="checkbox" ng-model="dataItem.processed" />'
              //{ field: "Action", width: "100px" },

        ],
        //dataBound: function () {
        //    $(".invoice .k-grid-content tr td .icons ul li.info").kendoTooltip({

        //        filter: "a",
        //        content: kendo.template($("#template").html()),
        //        //content: kendo.template('<a onclick="clickToGOInvoiceDetail(31499)"><img src="http://localhost:50330/Images/Invoices/info-hover.PNG" id="infoHover" /></a>'),
        //        // content:"hello",
        //        width: 32,
        //        height: 34,
        //        position: "left"
        //    });


        //alert($(".invoice .k-grid-content tr td .icons ul li.info").find('a').attr("id"));//.click(true);

        //},
        // the schema defines the schema of the JSON coming
        // back from the server so the datasource can parse it
        dataSource: new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    //if (CheckDataEdited(e) == true) {
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
                    $http({ method: 'POST', url: siteUrl + '/api/pasdejapi/GetRecordbypaging', data: requestData }).
                    success(function (data, status, headers, config) {
                        e.success(data);
                        console.log(data);

                        //cities.push(val.Name);
                        $scope.deletedRows.length = 0;
                        $scope.unmatchRows.length = 0;
                        $scope.updatedRecords = [];
                        checkdataisedit = [];

                        if (statusforinvoicegrid === '10') {  // special check for Posted in SAP.
                            $scope.isDisabled = true;
                            $('.delete, .matched').addClass("removeBorder");
                        }
                        else {
                            $scope.isDisabled = false;
                        }
                        //console.log(data);
                        //alert(data.RecordKey);
                        //$scope.theContent = kendo.template('<div ng-repeat="datas in data"> <img src="http://localhost:50330/Images/Invoices/info-hover.PNG" id="infoHover" ng-click="clickToInvoiceDetail(data.RecordKey)"  /></div>');
                        LoaderStop();
                    }).
                    error(function (data, status, headers, config) {
                        $scope.isDisabled = true;
                        $('.delete, .matched').addClass("removeBorder");
                    });
                    // }
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

                    $http({ method: 'POST', url: siteUrl + '/api/pasdejapi/GetRecordbypaging', data: requestData }).
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
                        IsDeleted: { editable: false, filterable: false },
                        CreatedOn: { editable: false, type: "date", format: "{0:yyyy-dd-MM hh:mm}" },

                        InvoiceNumber: { type: "string", validation: { min: 1, maxlength: 35 } },
                        VendorNumber: { validation: { maxlength: 20 } },
                        VATValue: { type: "number", validation: { maxlength: 18 } },
                        VendorNumber: { validation: { maxlength: 20 } },
                        VATCode: { validation: { maxlength: 20 } },
                        DeliveryNote: { validation: { maxlength: 35 } },
                        ArticleNumber: { validation: { maxlength: 35 } },
                        InvoiceDate: { type: "date", format: "{0:dd-MMM-yyyy hh:mm}" },
                        DocumentNumber: { editable: false, type: "string" },
                        DocumentID: { editable: false },
                        Match: { editable: false, type: "string" },
                        MatchDate: { editable: false, type: "date" },
                        BilledValue: { type: "number", editable: "#if('" + statusforinvoicegrid + "' === 1) {#false#} else{#true#}#", validation: { maxlength: 18 } },
                        TotalPricePerLine: { type: "number", validation: { maxlength: 18 } },
                        VATValueSEK: { type: "number", validation: { maxlength: 12 } },
                        StatusName: { editable: false },
                        Comment: { filterable: false },
                        Currency: { validation: { maxlength: 3 }, editable: "#if('" + statusforinvoicegrid + "' === 1) {#false#} else{#true#}#" },
                        Quantity: { type: "number", validation: { maxlength: 15 } }
                    }
                },

            },
            batch: true,

            // pageSize: 15,//modelParam.noOfRows,
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,



            requestStart: function (e) {
                console.log(e);
                if (isFromSave == 0) {
                    if ($scope.InvoiceGrid.dataSource.hasChanges()) {
                        if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?") == false) {
                            e.preventDefault();
                        }
                    }

                    if ($scope.deletedRows.length > 0) {
                        if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                            $scope.deletedRows = [];
                            LoaderStop();
                            return true;
                        }
                        else {

                            e.preventDefault();
                            LoaderStop();
                            return false;
                        }
                    }
                    if ($scope.unmatchRows.length > 0) {
                        if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                            $scope.unmatchRows = [];
                            LoaderStop();
                            return true;
                        }
                        else {
                            e.preventDefault();
                            LoaderStop();
                            return false;
                        }

                    }
                }
                //var confermboxvalue = true;
                //if ($scope.InvoiceGrid.dataSource.hasChanges()) {
                //    if (confirm("You have made edits to data! Click Cancel to stay on this page or click Ok to abandon your changes and continue.") == false) {

                //        e.preventDefault();
                //    }
                //}


                //    console.log(e);
                //    if ($scope.InvoiceGrid.dataSource.hasChanges()) {



                //        kendoWindow.data("kendoWindow")
                //            .content($("#delete-confirmation").html())
                //            .center().open();

                //        kendoWindow
                //            .find(".delete-confirm,.delete-cancel")
                //                .click(function () {
                //                    console.log($(this).hasClass("delete-confirm"));
                //                    if ($(this).hasClass("delete-confirm") == false) {

                //                        kendoWindow.data("kendoWindow").close();
                //                        e.preventDefault();

                //                    }
                //                    else {

                //                        kendoWindow.data("kendoWindow").close();
                //                        return true;
                //                    }


                //                })
                //                .end()
                ////        //if (confermboxvalue == false)
                ////        //{

                ////        //    console.log(confermboxvalue);
                ////        //}


                //   }


            },

        }),


        // selectable: "row",
        navigatable: true,
        pageable: true,
        pageable: {
            pageSize: 15,
            change: function (e) {

            }
        },
        sortable: true,
        groupable: true,
        filterable: true,
        filterMenuInit: onFilterMenuInit,
        filterable: {
            //logic: "and",
            extra: false,
            operators: {
                string: {
                    startswith: "Starts with",
                    contains: "Contains",
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
                    },
                number:
                    {
                        eq: "Is equal to",
                        neq: "Is not equal to",
                        gte: "Is after or equal to",
                        gt: "Is after",
                        lte: "Is before or equal to",
                        lt: "Is before"
                    },

            }
        },

        beforeEdit: function (e) {


        },

        editable: { //disables the deletion functionality
            update: true,
            // destroy: false
        },


        change: function (e) {
            //alert('coming on page click');
            //console.log("grid pager clicked!");
        },
        edit: function OnEdit(e) {
            if (statusforinvoicegrid === '1') {  // special check for Posted in SAP.
                e.container.find("input[name='VendorNumber']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='InvoiceDate']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='BilledValue']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='VATValue']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='VATCode']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='DeliveryNote']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='ArticleNumber']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='Quantity']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='TotalPricePerLine']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='Currency']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='gridcurrency']").each(function () { $(this).attr("readonly", "true") });
                e.container.find("input[name='VATValueSEK']").each(function () { $(this).attr("disabled", "disabled") });
                e.container.find("input[name='Comment']").each(function () { $(this).attr("disabled", "disabled") });
                $('#gridcurrency').each(function () { $(this).attr("disabled", "disabled") });
                $("#gridcurrency").attr("readonly", "true");


                //Size will be editable only when the Area is not empty 
                if (e.container.find("input[name='Currency']") == "Currency") {
                    this.closeCell();
                }


                var disableField = e.container.find("input[name=gridcurrency]").data("kendoDropDownList");
                disableField.enable(false);

            }
        },
        change: Onchange,
        //event: { databound: "onDataBound" }




    }
    function onFilterMenuInit(e) {
        if (e.field == "StatusName") {
            console.log(e);
        }
    }

    function onDataBound(e) {

    }

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
        $("#gridcurrency").attr('disabled', 'disabled');
        var disableField = e.container.find("input[name=Currency]").data("kendoDropDownList");
        disableField.enable(false);
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
    //filter test
    // Find the Role filter menu.
    ////var filterMenu = $("#kendoinvoicegrid").thead.find("th:not(.k-hierarchy-cell,.k-group-cell):last").data("kendoFilterMenu");
    //////var filterMenu = $scope.InvoiceGrid.thead.find("th:not(.k-hierarchy-cell,.k-group-cell):last").data("kendoFilterMenu");

    ////filterMenu.form.find("div.k-filter-help-text").text("Select an item from the list:");
    ////filterMenu.form.find("span.k-dropdown:first").css("display", "none");


    //filter test end
    //for containing the delete recordkeys 
    $scope.deletedRows = [];
    $scope.clickToDelete = function (i) {
        if ($scope.deletedRows.indexOf(i) == -1) {
            if (confirm("Are you sure to delete this record?")) {
                $scope.deletedRows.push(i);
                $('#' + i).attr('src', '' + siteUrl + 'Images/Invoices/delete-hover.PNG'); //DeletedRed.PNG
                $('#' + i).attr('title', 'Click to unselect');
            }
            else {

            }

        }
        else {
            if (confirm("Record already selected for delete. Do you want to unselect?")) {
                $scope.deletedRows.pop(i);
                $('#' + i).attr('src', '' + siteUrl + 'Images/Invoices/Delete.PNG');//DeletedRed.PNG
                $('#' + i).attr('title', 'Click to delete');
            }
            else {

            }

        }
    };

    //for containning the unmatch keys
    $scope.unmatchRows = [];
    $scope.clickToUnmatch = function (i) {
        if ($scope.unmatchRows.indexOf(i) == -1) {

            if (confirm("Are you sure to unmatch this record?")) {
                $scope.unmatchRows.push(i);
                $('#u' + i).attr('src', '' + siteUrl + '/Images/Invoices/unmatch-hover.GIF'); //UnmatchRed.PNG
                $('#u' + i).attr('title', 'Click to unselect');
            }
            else {

            }
        }
        else {
            if (confirm("Record already selected for unmatch. Do you want to unselect?")) {
                $scope.unmatchRows.pop(i);
                $('#u' + i).attr('src', '' + siteUrl + 'Images/Invoices/unmatch.PNG'); //Unmatch.PNG
                $('#u' + i).attr('title', 'Click to unmatch');
            }
            else {

            }
        }
    };


    $scope.clickToInvoiceDetail = function (i) {
        window.location.href = siteUrl + "InvoiceMatching/InvoiceDetails?Id=" + i;
        // alert(i);
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
            //console.log(data[i]);
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

            o.currentUser = $("#hdnUserEmail").val();
            o.AllRecords = data;
            o.data = $scope.updatedRecords;//data; // $scope.InvoiceGrid.dataSource.read();
            o.deletekeys = $scope.deletedRows,
            o.unmatchkeys = $scope.unmatchRows,

            myFactory.postData(EndPoint.Service, "SaveUpdateGrid", o, function (res) {
                console.log(res);
                if (res.Success == true) {
                    // for refreshing grid useful           
                    //$scope.deletedRows = [];
                    $scope.deletedRows.length = 0;
                    //$scope.unmatchRows = [];
                    $scope.unmatchRows.length = 0;
                    $scope.updatedRecords = [];
                    checkdataisedit = [];
                    $scope.grid.refresh();
                    var dt = new Date();
                    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                    $("#spnMsg").html("");
                    $("#spnMsg").html("Saved: " + time);
                    $("#spnMsg").fadeOut(60000);
                    isFromSave = 1;
                    $scope.InvoiceGrid.dataSource.read();
                    LoaderStop();
                }
                else {
                    //$scope.deletedRows.length = 0;
                    //$scope.unmatchRows.length = 0;
                    //$scope.updatedRecords = [];
                    //alert('Failed!');

                    $("#spnMsg").html("");
                    $("#spnMsg").html("Failed to update the data.");
                    $("#spnMsg").fadeOut(60000);
                    LoaderStop();
                }
                // console.log();
            });

        }
    }

    //Export Invoices
    $scope.ExportInvoice = function (val) {
        if (CheckDataEdited()) {
            var o = new Object();
            o.ID = val.viewId;
            var status = $scope.selectedItemvalue;
            window.location.href = EndPoint.Service + "DownloadInvoices?ID=" + status;
            LoaderStop();
        }
    }

    //Import Invoice popup open
    $scope.ImportInvoice = function (val) {
        //console.log(val);
        if (CheckDataEdited()) {
            $("#fileinvoiceupdate").replaceWith($("#fileinvoiceupdate").clone());
            $('#myModal').modal('show');
        }
    }

    //Export Invoices
    //Import modified invoices


    $scope.SelectedFileForModifiedInvoiceUpload = null;
    $scope.uploadFileModifiedInvoice = function (file) {
        $scope.SelectedFileForModifiedInvoiceUpload = file[0];
        $('.testfile').val($('.fileInput').val());

    };

    $scope.imprtInvoicesUpdateClickfunc = function () {
        var fd = new FormData();
        fd.append("file", $scope.SelectedFileForModifiedInvoiceUpload);
        fd.append("currentuser", $("#hdnUserEmail").val());

        var url = EndPoint.Service + "ImportModifiedInvoices";
        myFactory.uploadfile(url, fd, function (res) {
            if (res.Success == true) {
                //alert('coming');
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
            //console.log();
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
        if (CheckDataEdited()) {
            myFactory.postData(EndPoint.Service, "MatchInvoices", o, function (res) {
                if (res.Success == true) {
                    var dt = new Date();
                    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
                    $("#spnMsg").html("");
                    $("#spnMsg").html("Matched Invoice: " + time);
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
    }


    // function to check whether any changes has been made to grid or not
    function CheckDataEdited(e) {
        showError(null, null);
        if (isFromSave == 0) {
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
                            LoaderStop();
                            return true;
                        }
                        else {
                            //kendo.ui.progress($("#kendoinvoicegrid"), false);
                            $(".k-loading-mask").css("display", "none");

                            LoaderStop();
                            return false;
                        }
                        //$('#myConfirmModal').modal('show');
                    }
                }
                if ($scope.deletedRows.length > 0) {
                    if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                        $scope.deletedRows = [];

                        LoaderStop();
                        return true;
                    }
                    else {


                        $(".k-loading-mask").css("display", "none");

                        LoaderStop();
                        return false;
                    }
                    //bootbox.confirm("You have unsaved changes. Are you sure you want to leave this page without saving?", function (result) {

                    //    if (result == 'true') {

                    //        $scope.deletedRows = [];
                    //        return true;
                    //    }
                    //    else {
                    //        return false;
                    //    }
                    //});



                }
                if ($scope.unmatchRows.length > 0) {
                    if (confirm("You have unsaved changes. Are you sure you want to leave this page without saving?")) {
                        $scope.unmatchRows = [];
                        LoaderStop();
                        return true;
                    }
                    else {
                        $(".k-loading-mask").css("display", "none");

                        LoaderStop();
                        return false;
                    }

                }

            }
            LoaderStop();
            return true;
        }
        else {
            LoaderStop();
            isFromSave = 0;
            return true;
        }
        // return IsEdit;
    }
    //import from EDI\Readsoft 
    $scope.imprtInvoiceEDIReadsoftClickfunc = function (txt) {
        console.log(txt);
        var o = new Object();
        o.page = 1;

        myFactory.postData(EndPoint.Service, "ImportFromEdiReadsoftInvoice", o, function (res) {
            if (res.Success == true) {
                //alert('coming');
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
        });
    }



    //clear all filters
    $scope.clearFilterClickfunc = function (txt) {
        $scope.InvoiceGrid.dataSource.filter({});
        $('.clearFilter').hide();
    }
});

