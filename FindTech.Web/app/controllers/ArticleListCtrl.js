angular.module('FindTech.ArticleList', [])
    .controller('ArticleListCtrl', ['$scope', '$http', '$location', 
        function ($scope, $http, $location) {

            $scope.loadMoreNewses = function () {
                $scope.newses.loadMoreLoading = true;
                $http.get($scope.newses.Url + 'skip=' + ($scope.newses.skip + $scope.newses.take) + '&take=' + $scope.newses.take).success(function (data) {
                    $scope.newses.Articles = $scope.newses.Articles.concat(data);
                    $scope.newses.skip += $scope.newses.take;
                    $scope.newses.loadMoreLoading = false;
                });
            };

            $scope.loadMoreReviews = function () {
                $scope.reviews.loadMoreLoading = true;
                $http.get($scope.reviews.Url + 'skip=' + ($scope.reviews.skip + $scope.reviews.take) + '&take=' + $scope.reviews.take).success(function (data) {
                    $scope.reviews.Articles = $scope.reviews.Articles.concat(data);
                    $scope.reviews.skip += $scope.reviews.take;
                    $scope.reviews.loadMoreLoading = false;
                });
            };
            
            $scope.newses = {
                Title: 'Tin tức',
                loadMore: $scope.loadMoreNewses,
                loadMoreLoading: false,
                skip: 0,
                take: 20
            };
            
            $scope.reviews = {
                Title: 'Soi',
                loadMore: $scope.loadMoreReviews,
                loadMoreLoading: false,
                skip: 0,
                take: 20
            };
            $scope.hotReviews = {
                Title: 'Soi nóng',
                TitleStyleClass: 'fa fa-eye background-danger'
            };
            $scope.hotNewses = {
                Title: 'Tin nóng',
                TitleStyleClass: 'fa fa-rss background-danger'
            };

            $scope.init = function (newses, newsesUrl, reviews, reviewsUrl, hotNewses, hotReviews, keyword) {
                $scope.newses.Articles = newses;
                $scope.newses.Url = newsesUrl;
                $scope.reviews.Articles = reviews;
                $scope.reviews.Url = reviewsUrl;
                $scope.hotNewses.Articles = hotNewses;
                $scope.hotReviews.Articles = hotReviews;
                $scope.$parent.keyword = keyword;
            };
            
            $scope.$on('onRepeatLast', function (scope, element, attrs) {
                bannerSetCarousel();
                carousel();
            });
        }
    ]);