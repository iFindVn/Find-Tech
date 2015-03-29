angular.module('FindTech.Home', [])
    .controller('HomeCtrl', ['$scope', '$http', '$location',
        function ($scope, $http, $location) {
            $scope.latestReviews = {                
                Title: 'Soi mới nhất',
                TitleStyleClass: 'fa fa-eye background-info'
            };
            $scope.hotArticles = {};

            $scope.loadMoreNewses = function () {
                $scope.latestNewses.loadMoreLoading = true;
                $http.get('/Article/GetLatestNewses?skip=' + ($scope.latestNewses.skip + $scope.latestNewses.take) + '&take=' + $scope.latestNewses.take).success(function (data) {
                    $scope.latestNewses.Articles = $scope.latestNewses.Articles.concat(data);
                    $scope.latestNewses.skip += $scope.latestNewses.take;
                    $scope.latestNewses.loadMoreLoading = false;
                });
            };
            $scope.latestNewses = {
                Title: 'Tin tức Troy',
                loadMore: $scope.loadMoreNewses,
                loadMoreLoading: false,
                skip: 0,
                take: 20
            };
            $scope.appAndGameArticles = {
                Title: 'Ứng dụng và game',
                TitleStyleClass: 'fa fa-android background-success'
            };
            $scope.productAndTechToyArticles = {
                Title: 'Sản phẩm',
                TitleStyleClass: 'fa fa-tablet background-primary'
            };
            $scope.brandAndDigiLifeArticles = {
                Title: 'Đời sống số',
                TitleStyleClass: 'fa fa-apple background-info'
            };
            $scope.trickAndTipArticles = {
                Title: 'Thủ thuật, Mẹo vặt'
            };

            $scope.init = function (hotArticles, latestReviews, trickAndTipArticles, appAndGameArticles, productAndTechToyArticles, brandAndDigiLifeArticles, latestNewses) {
                $scope.hotArticles.Articles = hotArticles;
                $scope.latestReviews.Articles = latestReviews;
                $scope.appAndGameArticles.Articles = appAndGameArticles;
                $scope.productAndTechToyArticles.Articles = productAndTechToyArticles;
                $scope.brandAndDigiLifeArticles.Articles = brandAndDigiLifeArticles;
                $scope.trickAndTipArticles.Articles = trickAndTipArticles;
                $scope.latestNewses.Articles = latestNewses;
            };
            
            $scope.$on('onRepeatLast', function (scope, element, attrs) {
                bannerSetCarousel();
                carousel();
            });
        }
    ]);