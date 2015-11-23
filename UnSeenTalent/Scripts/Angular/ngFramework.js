//var siteUrl = 'http://54.169.34.219//Unseentalant/'
var siteUrl = ''

var app = angular.module('myApp', ['ngTable', 'ui.bootstrap', 'kendo.directives']);


app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
});


//use <input type="text" ng-model="contactData.phone" valid-number maxlength="15">
app.directive('validNumber', function () {
    return {
        require: '?ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            if (!ngModelCtrl) {
                return;
            }

            ngModelCtrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    var val = '';
                }
                var clean = val.replace(/[^0-9]+/g, '');
                if (val !== clean) {
                    ngModelCtrl.$setViewValue(clean);
                    ngModelCtrl.$render();
                }
                return clean;
            });

            element.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});

// for file upload test 
app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

app.service('fileUpload', ['$http', function ($http) {
    this.uploadFileToUrl = function(file, uploadUrl) {
        var fd = new FormData();
        fd.append('file', file);
        $http.post(uploadUrl, fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
            .success(function() {
            })
            .error(function() {
            });
    };
}]);
// for  file upload test end

var EndPoint = {
    Service: siteUrl + '/api/UnseentalentApi/',
    Adminurl: siteUrl + '/api/UnseentalentApi/',
};


app.factory("myFactory", function ($http, $modal, $log, $filter, ngTableParams, $locale) {
    var uploadedPer = 0;
    return {
        postData: function (EndPoint, method, postData, onSuccess) {
            LoaderStart(); //Loader Stop on your success method
            //$http.post(EndPoint + method, JSON.stringify(postData)).success(function (data) {
            //    onSuccess(data);
            //}).error(onError);

            $http({
                method: "POST",
                headers: { 'Authentication': 'web60134' },
                url: EndPoint + method,
                data: JSON.stringify(postData)
            }).success(onSuccess).error(function (e, m) {
                console.log(m);
                if (m == 401) {
                    redirectToLogin();
                }
                console.log(e);
                LoaderStop();
            });
        },
        uploadfile: function (EndPoint, formdata, onSuccess) {
            LoaderStart(); //Loader Stop on your success method
            $http.post(EndPoint, formdata,
{
    withCredentials: true,
    headers: { 'Content-Type': undefined }, //'undefined'
    transformRequest: angular.identity
}).success(onSuccess).error(function (e, m) {

    if (m == 401) {
        redirectToLogin();
    }
    console.log(e);
    LoaderStop();
});


        },


        uploadfileWithData: function (EndPoint, formdata, data, onSuccess) {
            LoaderStart(); //Loader Stop on your success method
            $http.post(EndPoint, formdata,
{
    withCredentials: true,
    headers: { 'Content-Type': undefined }, //'undefined'
    transformRequest: angular.identity,
    data: JSON.stringify(data)
}).success(onSuccess).error(function (e, m) {

    if (m == 401) {
        redirectToLogin();
    }
    console.log(e);
    LoaderStop();
});


        },


        getData: function (EndPoint, method, onSuccess) {
            LoaderStart(); //Loader Stop on your success method
            $http.get(EndPoint + method).success(onSuccess).error(function (e, m) {
                console.log(m);
                if (m == 401) {
                    redirectToLogin();
                }
                console.log(e);
                LoaderStop();
            });
        },

    }
});



function scrollTop() {
    window.scrollTo(500, 0);
}

function LoaderStart() {
    $('#loaderdiv').show();
}


function LoaderStop() {
    $('#loaderdiv').hide();
}


//Get this index of item via attr from array
var getIndexIfObjWithOwnAttr = function (newArr, attr, value) {
    for (var i = 0; i < newArr.length; i++) {
        if (newArr[i].hasOwnProperty(attr) && newArr[i][attr] === value) {
            return i;
        }
    }
    return -1;
}


function isValidEmail(email) {
    var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
    if (reg.test(email)) {
        return true;
    }
    else {
        return false;
    }
}
function isPasswordMatch(val1, val2) {
    if (val1 == val2) {
        return true;
    }
    else {
        return false;
    }
}

function isValidNumber(num) {
    var Number = /^[A-Za-z]+$/;
    if (Number.test(num)) {
        return false;
    }
    else {
        return true;

    }
}

//isNumber
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

//Validate form data jquery

function formValidation(id) {

    jQuery(id).validate({
        highlight: function (element) {
            jQuery(element).closest('.input-group').removeClass('has-success').addClass('has-error');
        },
        success: function (element) {
            jQuery(element).closest('.input-group').removeClass('has-error').addClass('has-success');

        },
        //Disable error msg lable
        errorPlacement: function (error, element) {
            return true;
        }
    });

    return $(id).valid();;
}

//Check elements by name
function isValidElements(arr) {
    var isValid = true;
    $.each(arr, function (i, e) {
        var v = $('#' + e);
        console.log(v.val());
        if (!v.val()) {
            $(v).addClass('inValidElement');
            isValid = false;
        }
        else {
            $(v).removeClass('inValidElement');
        }

    });
    return isValid;
}

//Put error border on elements
function inValidElements(arr) {
    $.each(arr, function (i, e) {
        $('#' + e).addClass('inValidElement');
    });
}

function isdropdownSelected(arr) {
    var isValid = true;
    $.each(arr, function (i, e) {
        var v = $('#' + e);
        console.log(v.val());
        if (v.val()== "0") {
            inValidDropdown(e,true);
            isValid = false;
        }
        else {
            inValidDropdown(e, false);
        }

    });
    return isValid;
}


//Put error border on elements
function inValidDropdown(id, isError) {
    if (isError)
        $('#' + id).addClass('inValidElement');
    else
        $('#' + id).removeClass('inValidElement');
}

//Check dropdown by name initial value should be 0
function isValidDropdown(arr) {
    var isValid = true;
    $.each(arr, function (i, e) {
        var v = $('#' + e);
        console.log(v.val());
        if (v.val() == 0) {
            $(v).closest('.input-group').removeClass('has-success').addClass('has-error');;
            isValid = false;
        }
        else {
            $(v).closest('.input-group').removeClass('has-error');
        }

    });
    return isValid;
}


//Open Bootstrap Modal and disable close out side click

function openModal(id) {
    $('#' + id).modal({
        backdrop: 'static',
        keyboard: false  // to prevent closing with Esc button (if you want this too)
    })
}

function closeModal(id) {
    $('#' + id).modal('hide')
}
