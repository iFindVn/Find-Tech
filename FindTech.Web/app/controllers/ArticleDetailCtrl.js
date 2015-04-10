angular.module('FindTech.ArticleDetail', [])
    .controller('ArticleDetailCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            $scope.states = {                
                ratedOpinion: false
            };

            $scope.absUrl = $location.absUrl();
            $scope.article = {};
            $scope.contentSectionPageManager = {};
            $scope.commentManager = {
                skip: 0,
                take: 5
            };
            $scope.sameCategoryNewses = {
                Title: 'Tin cùng chuyên mục',
                TitleStyleClass: 'fa fa-folder-open background-success'
            };
            $scope.hotNewses = {
                Title: 'Tin nóng',
                TitleStyleClass: 'fa fa-rss background-danger'
            };
            $scope.relatedNewses = {
                Title: 'Tin liên quan'
            };
            $scope.latestReviews = {
                Title: 'Soi mới nhất',
                TitleStyleClass: 'fa fa-eye background-info'
            };
            $scope.hotReviews = {
                Title: 'Soi nóng',
                TitleStyleClass: 'fa fa-eye background-danger'
            };
            $scope.relatedReviews = {
                Title: 'Soi liên quan'
            };
            $scope.init = function (article, contentSectionPageManager, sameCategoryNewses, relatedNewses, hotNewses, latestReviews, hotReviews, relatedReviews, likedCommentIds) {
                $scope.article = article;
                $scope.contentSectionPageManager = contentSectionPageManager;
                $scope.sameCategoryNewses.Articles = sameCategoryNewses;
                $scope.sameCategoryNewses.Title = article.ArticleCategoryName;
                $scope.sameCategoryNewses.Url = '/danh-muc/' + article.ArticleCategorySeoName;
                $scope.relatedNewses.Articles = relatedNewses;
                $scope.relatedNewses.Title = 'Tin tức liên quan đến \'' + article.Tags + '\'';
                $scope.relatedNewses.Url = '/nhan/' + article.Tags + '/1';
                $scope.hotNewses.Articles = hotNewses;
                $scope.hotNewses.Url = '/tin-nong';
                $scope.latestReviews.Articles = latestReviews;
                $scope.latestReviews.Url = '/soi';
                $scope.hotReviews.Articles = hotReviews;
                $scope.hotReviews.Url = '/soi-nong';
                $scope.relatedReviews.Articles = relatedReviews;
                $scope.relatedReviews.Title = 'Soi liên quan đến \'' + article.Tags + '\'';
                $scope.relatedReviews.Url = '/nhan/' + article.Tags + '/2';
                $scope.likedCommentIds = likedCommentIds;
                $http.get('/Comment/GetComments?objectType=1&objectId=' + $scope.article.ArticleId + '&skip=' + $scope.commentManager.skip + '&take=' + $scope.commentManager.take).success(function (commentData) {
                    $scope.commentManager.comments = commentData.comments;
                    $scope.commentManager.commentCount = commentData.commentCount;
                    $scope.commentManager.skip = $scope.commentManager.skip + $scope.commentManager.take;
                });

                $scope.newComment = {
                    ObjectType: 1,
                    ObjectId: $scope.article.ArticleId,
                    Content: '',
                    CommentatorEmail: ''
                };
            };

            $scope.rateOpinion = function (articleId, opinionLevel) {
                $http.post('/Article/RateOpinion', { articleId: articleId, opinionLevel: opinionLevel }).success(function(data) {
                    $scope.article.Opinions = data;
                    $scope.states.ratedOpinion = true;
                });
            };
            
            $scope.openReplyForm = function (comment, $event, index) {
                $scope.CommentObjectIndex = index;
                $scope.newComment.ObjectType = 3;
                $scope.newComment.ObjectId = comment.CommentId;
                $($event.target.closest('.comment')).after($('#commentForm'));
            };

            $scope.submitComment = function () {
                if ($scope.commentForm.$invalid) {
                    return false;
                }
                $scope.commentSubmitting = true;
                $http.post('/Comment/Create', $scope.newComment).success(function (data) {
                    if ($scope.newComment.ObjectType == 1) {
                        $scope.commentManager.comments.unshift(data.comment);
                    } else {
                        $scope.commentManager.comments[$scope.CommentObjectIndex].Replies.unshift(data);
                    }
                    $scope.commentManager.commentCount = data.commentCount;
                    $scope.commentForm.$setPristine();
                    $scope.cancelComment();
                    $scope.commentSubmitting = false;
                });
            };

            $scope.cancelComment = function () {
                $scope.newComment = {
                    ObjectType: 1,
                    ObjectId: $scope.article.ArticleId,
                    Content: '',
                    CommentatorEmail: ''
                };
                $('#rootCommentList').after($('#commentForm'));
            };

            $scope.loadMoreComments = function (articleId) {
                $scope.loadMoreCommentLoading = true;
                $http.get('/Comment/GetComments?objectType=1&objectId=' + articleId + '&skip=' + $scope.commentManager.skip + '&take=' + $scope.commentManager.take).success(function (commentData) {
                    $scope.commentManager.comments = $scope.commentManager.comments.concat(commentData.comments);
                    $scope.commentManager.commentCount = commentData.commentCount;
                    $scope.commentManager.skip = $scope.commentManager.skip + $scope.commentManager.take;
                    $scope.loadMoreCommentLoading = false;
                });
            };

            $scope.loadMoreReplyLoading = [];
            $scope.loadMoreReplies = function (commentId, $index) {
                $scope.loadMoreReplyLoading[$index] = true;
                $scope.commentManager.comments[$index].skip = $scope.commentManager.comments[$index].skip || 2;
                $scope.commentManager.comments[$index].take = $scope.commentManager.comments[$index].take || 5;
                $http.get('/Comment/GetComments?objectType=3&objectId=' + commentId + '&skip=' + $scope.commentManager.comments[$index].skip + '&take=' + $scope.commentManager.comments[$index].take).success(function (replyData) {
                    $scope.commentManager.comments[$index].Replies = $scope.commentManager.comments[$index].Replies.concat(replyData.comments);
                    $scope.commentManager.comments[$index].ReplyCount = replyData.commentCount;
                    $scope.loadMoreReplyLoading[$index] = false;
                });
            };

            $scope.submitCommentLike = function (comment) {
                $http.post('/Like/Create', { ObjectId: comment.CommentId, ObjectType: 3 }).success(function (data) {
                    comment.LikeCount = data.likeCount;
                    $scope.likedCommentIds.push(comment.CommentId);
                });
            };

            $scope.checkLiked = function (commentId) {
                return $scope.likedCommentIds.indexOf(commentId) > -1;
            };
            
            $scope.getLikedClass = function (commentId) {
                return $scope.likedCommentIds.indexOf(commentId) > -1 ? 'active' : '';
            };

            $scope.$on('onRepeatLast', function (scope, element, attrs) {
                $scope.ScaleSlider();
            });
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizes
            $scope.ScaleSlider = function() {
                $.each($('.content-section .jssor_slider_containers'), function(index, jssor_slider) {
                    $(jssor_slider).imagesLoaded(function() {
                        var jssor_slider_id = $(jssor_slider).attr('id');
                        if (jssor_slider_id) {
                            if (!jssor_sliders[jssor_slider_id]) {
                                jssor_sliders[jssor_slider_id] = new $JssorSlider$(jssor_slider_id, options);
                            }
                            var parentWidth = $('section.content-section').width();
                            if (parentWidth)
                                jssor_sliders[jssor_slider_id].$ScaleWidth(parentWidth);
                            else
                                window.setTimeout(ScaleSlider, 30);
                        }
                    });
                });
            };
            
            var jssor_sliders = {};
            
            var _SlideshowTransitions = [
    //Fade in L
        { $Duration: 1200, x: 0.3, $During: { $Left: [0.3, 0.7] }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade out R
        , { $Duration: 1200, x: -0.3, $SlideOut: true, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade in R
        , { $Duration: 1200, x: -0.3, $During: { $Left: [0.3, 0.7] }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade out L
        , { $Duration: 1200, x: 0.3, $SlideOut: true, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }

    //Fade in T
        , { $Duration: 1200, y: 0.3, $During: { $Top: [0.3, 0.7] }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade out B
        , { $Duration: 1200, y: -0.3, $SlideOut: true, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade in B
        , { $Duration: 1200, y: -0.3, $During: { $Top: [0.3, 0.7] }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade out T
        , { $Duration: 1200, y: 0.3, $SlideOut: true, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }

    //Fade in LR
        , { $Duration: 1200, x: 0.3, $Cols: 2, $During: { $Left: [0.3, 0.7] }, $ChessMode: { $Column: 3 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade out LR
        , { $Duration: 1200, x: 0.3, $Cols: 2, $SlideOut: true, $ChessMode: { $Column: 3 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade in TB
        , { $Duration: 1200, y: 0.3, $Rows: 2, $During: { $Top: [0.3, 0.7] }, $ChessMode: { $Row: 12 }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade out TB
        , { $Duration: 1200, y: 0.3, $Rows: 2, $SlideOut: true, $ChessMode: { $Row: 12 }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }

    //Fade in LR Chess
        , { $Duration: 1200, y: 0.3, $Cols: 2, $During: { $Top: [0.3, 0.7] }, $ChessMode: { $Column: 12 }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade out LR Chess
        , { $Duration: 1200, y: -0.3, $Cols: 2, $SlideOut: true, $ChessMode: { $Column: 12 }, $Easing: { $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade in TB Chess
        , { $Duration: 1200, x: 0.3, $Rows: 2, $During: { $Left: [0.3, 0.7] }, $ChessMode: { $Row: 3 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade out TB Chess
        , { $Duration: 1200, x: -0.3, $Rows: 2, $SlideOut: true, $ChessMode: { $Row: 3 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }

    //Fade in Corners
        , { $Duration: 1200, x: 0.3, y: 0.3, $Cols: 2, $Rows: 2, $During: { $Left: [0.3, 0.7], $Top: [0.3, 0.7] }, $ChessMode: { $Column: 3, $Row: 12 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }
    //Fade out Corners
        , { $Duration: 1200, x: 0.3, y: 0.3, $Cols: 2, $Rows: 2, $During: { $Left: [0.3, 0.7], $Top: [0.3, 0.7] }, $SlideOut: true, $ChessMode: { $Column: 3, $Row: 12 }, $Easing: { $Left: $JssorEasing$.$EaseInCubic, $Top: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2, $Outside: true }

    //Fade Clip in H
        , { $Duration: 1200, $Delay: 20, $Clip: 3, $Assembly: 260, $Easing: { $Clip: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade Clip out H
        , { $Duration: 1200, $Delay: 20, $Clip: 3, $SlideOut: true, $Assembly: 260, $Easing: { $Clip: $JssorEasing$.$EaseOutCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade Clip in V
        , { $Duration: 1200, $Delay: 20, $Clip: 12, $Assembly: 260, $Easing: { $Clip: $JssorEasing$.$EaseInCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
    //Fade Clip out V
        , { $Duration: 1200, $Delay: 20, $Clip: 12, $SlideOut: true, $Assembly: 260, $Easing: { $Clip: $JssorEasing$.$EaseOutCubic, $Opacity: $JssorEasing$.$EaseLinear }, $Opacity: 2 }
            ];

            var options = {
                $AutoPlay: false,                                    //[Optional] Whether to auto play, to enable slideshow, this option must be set to true, default value is false
                $AutoPlayInterval: 1500,                            //[Optional] Interval (in milliseconds) to go for next slide since the previous stopped if the slider is auto playing, default value is 3000
                $PauseOnHover: 1,                                //[Optional] Whether to pause when mouse over if a slider is auto playing, 0 no pause, 1 pause for desktop, 2 pause for touch device, 3 pause for desktop and touch device, 4 freeze for desktop, 8 freeze for touch device, 12 freeze for desktop and touch device, default value is 1

                $DragOrientation: 3,                                //[Optional] Orientation to drag slide, 0 no drag, 1 horizental, 2 vertical, 3 either, default value is 1 (Note that the $DragOrientation should be the same as $PlayOrientation when $DisplayPieces is greater than 1, or parking position is not 0)
                $ArrowKeyNavigation: true,   			            //[Optional] Allows keyboard (arrow key) navigation or not, default value is false
                $SlideDuration: 800,                                //Specifies default duration (swipe) for slide in milliseconds

                $SlideshowOptions: {                                //[Optional] Options to specify and enable slideshow or not
                    $Class: $JssorSlideshowRunner$,                 //[Required] Class to create instance of slideshow
                    $Transitions: _SlideshowTransitions,            //[Required] An array of slideshow transitions to play slideshow
                    $TransitionsOrder: 1,                           //[Optional] The way to choose transition to play slide, 1 Sequence, 0 Random
                    $ShowLink: true                                    //[Optional] Whether to bring slide link on top of the slider when slideshow is running, default value is false
                },

                $ArrowNavigatorOptions: {                       //[Optional] Options to specify and enable arrow navigator or not
                    $Class: $JssorArrowNavigator$,              //[Requried] Class to create arrow navigator instance
                    $ChanceToShow: 1                               //[Required] 0 Never, 1 Mouse Over, 2 Always
                },

                $ThumbnailNavigatorOptions: {                       //[Optional] Options to specify and enable thumbnail navigator or not
                    $Class: $JssorThumbnailNavigator$,              //[Required] Class to create thumbnail navigator instance
                    $ChanceToShow: 2,                               //[Required] 0 Never, 1 Mouse Over, 2 Always

                    $ActionMode: 1,                                 //[Optional] 0 None, 1 act by click, 2 act by mouse hover, 3 both, default value is 1
                    $SpacingX: 8,                                   //[Optional] Horizontal space between each thumbnail in pixel, default value is 0
                    $DisplayPieces: 10,                             //[Optional] Number of pieces to display, default value is 1
                    $ParkingPosition: 360                          //[Optional] The offset position to park thumbnail
                }
            };

            $(window).bind("load", $scope.ScaleSlider);
            $(window).bind("resize", $scope.ScaleSlider);
            $(window).bind("orientationchange", $scope.ScaleSlider);
        }
    ]);