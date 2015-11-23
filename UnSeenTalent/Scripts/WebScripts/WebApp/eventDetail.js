app.controller('EventDetailCtrl', function ($scope, myFactory, $log, $filter, ngTableParams, $locale, $sce) {

    checkEventStatus();
    getEventPosts();
    getRecentEvents();

    //----CheckEventStatus
    function checkEventStatus() {
        

        $scope.interface = {};
       var obj = {
            eventId: $('#hdnEventId').val(),
            userId: $('#hdnUserId').val(),
        };
        myFactory.postData(EndPoint.Service, "CheckEventStatus", obj, function(res) {
            if (res.Success == true) {
                //isUploading 1: BS 0: Ready to vote
                $scope.isUploading = res.Result;
                $scope.$apply();
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    $scope.eveeventPostsntList = [];

    $scope.ParseVideoUrl = function (videoPath) {
        return $sce.trustAsResourceUrl(videoPath);
    };

    function getEventPosts() {
        var obj = {
            eventId: $('#hdnEventId').val(),
            userId: $('#hdnUserId').val(),
        };

        myFactory.postData(EndPoint.Service, "GetEventPosts", obj, function(res) {
            if (res.Success == true) {
                $scope.eveeventPostsntList = res.Result;
                console.log($scope.eveeventPostsntList);
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    function getRecentEvents() {
        myFactory.postData(EndPoint.Service, "GetRecentEvents", null, function(res) {
            if (res.Success == true) {
                $scope.recentevents = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    //------------------Add/Edit Data----------------------
    $scope.SavePost = function() {
        $scope.ErrorMsg = null;
        $scope.SucessMsg = null;

        var obj = {
            eventId: $('#hdnEventId').val(),
            userId: $('#hdnUserId').val(),
            postText: $('#txtEventPost').val(),
            file: ''
        };

        //--Code for Image
        var imgSmall = $("#upload_upload").get(0).files;
        var data = new FormData();
        if (imgSmall.length > 0) {
            for (i = 0; i < imgSmall.length; i++) {
                data.append("image" + (i + 1), imgSmall[i]);
            }
        } else {
            alert("Please upload a video.");
        }
        var jsonString = JSON.stringify(obj);
        data.append("json", jsonString);
        LoaderStart();
        $.ajax({
            type: "POST",
            headers: { 'Authentication': 'web60134' },
            url: EndPoint.Service + "SavePost",
            contentType: false,
            processData: false,
            async: false,
            data: data,
            success: function(response) {
                if (response.Success == true) {
                    $('#upload_upload').val("");
                    getEventPosts();
                    //if (response.result == "1") {
                    //    $('#upload_upload').val("");
                    //    GetEventPosts();
                    //}
                    //else {
                    //    LoaderStop();
                    //    alert("video is not in correct formate");
                    //}

                } else {
                    $("#err").show();
                    //  ErrMsg(response.Message);
                    LoaderStop();

                }
            },
            error: function() {
                LoaderStop();
            }
        });
    };


    $scope.ToggleComment = function(index, post) {
        //var cs = $('#divComment' + id).css('display');
        //if (cs == 'none') {
        //    $('#spnComment' + id).addClass('active');
        //}
        //else {
        //    $('#spnComment' + id).removeClass('active');
        //}
        var obj = {
            videoId: post.videoId,
        };
        //$http({
        //    method: "POST",
        //    headers: { 'Authentication': 'web60134' },
        //    url: siteUrl + "/api/Services/GetCommentsBlogWise",
        //    data: JSON.stringify(obj)
        //}).success(function (response) {
        //    $scope.allBlogData[myindex].commentBlogWise = response.Result;
        //    if (response.Result.length > 0) {
        //        post.commentCount = response.Result.length;
        //    }
        //}).error(function () { });
        myFactory.postData(EndPoint.Service, "GetVideoComments", obj, function(res) {
            if (res.Success == true) {
                console.log(res.Result);
                $scope.eveeventPostsntList[index].postComments = res.Result;
                //for (var i = 0; i < res.Result.length; i++) {
                //    $scope.allBlogData.push(res.Result[i]);
                //}
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
        $('#divcommentbox_' + post.videoId).slideToggle('slow');
    };


    $scope.postVote = function(index, data) {
        var isVoteNew = 'False';
        if (data.isVote == 'False') {
            isVoteNew = 'True';
            $scope.eveeventPostsntList[index].isVote = 'True';
            $scope.eveeventPostsntList[index].voteCount = parseInt($scope.eveeventPostsntList[index].voteCount) + 1;
        } else {
            $scope.eveeventPostsntList[index].isVote = 'False';
            $scope.eveeventPostsntList[index].voteCount = parseInt($scope.eveeventPostsntList[index].voteCount) - 1;
        }

        var obj = {
            postId: data.videoId,
            eventId: $('#hdnEventId').val(),
            userId: $('#hdnUserId').val(),
            isLike: isVoteNew
        };

        myFactory.postData(EndPoint.Service, "postVote", obj, function(res) {
            if (res.Success == true) {
                console.log(res.Result);
                //$scope.eveeventPostsntList[index].postComments = res.Result;
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    };


    function loadComment(index, post) {
        var obj = {
            videoId: post.videoId,
        };
        myFactory.postData(EndPoint.Service, "GetVideoComments", obj, function(res) {
            if (res.Success == true) {
                console.log(res.Result);
                $scope.eveeventPostsntList[index].postComments = res.Result;
                $scope.eveeventPostsntList[index].commentCount = res.Result.length;
                //for (var i = 0; i < res.Result.length; i++) {
                //    $scope.allBlogData.push(res.Result[i]);
                //}
                LoaderStop();
            } else {
                console.log(res.Result);
                LoaderStop();
            }
        });
    }

    $scope.AddPostComment = function(data, index) {
        $scope.commentText = $('#txtCommentTextArea_' + data.videoId).val();
        if ($.trim($scope.commentText) != '') {

            var obj = {
                videoId: data.videoId,
                eventId: $('#hdnEventId').val(),
                userId: $('#hdnUserId').val(),
                commentText: $scope.commentText
            };
            myFactory.postData(EndPoint.Service, "AddComment", obj, function(res) {
                if (res.Success == true) {
                    //$scope.eveeventPostsntList = res.Result
                    loadComment(index, data);
                    $('#txtCommentTextArea_' + data.videoId).val(''); //LoaderStop();
                } else {
                    console.log(res.Result);
                    LoaderStop();
                }
            });
        }
    };
});