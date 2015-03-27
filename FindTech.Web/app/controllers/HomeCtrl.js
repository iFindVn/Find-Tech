angular.module('FindTech.Home', [])
    .controller('HomeCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            Page.setTitle('Tìm là thấy');
            Page.setDescription('Cổng thông tin công nghệ, thiết bị di động, so sánh sản phẩm công nghệ, đánh giá smart phone, tablet,...');

            $scope.latestReviews = {                
                Title: 'Soi mới nhất',
                TitleStyleClass: 'fa fa-eye background-info'
            };
            $scope.hotArticles = {};
            $scope.latestNewses = {
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

            $scope.loadMoreNewses = function () {
                $scope.loadMoreLoading = true;
                $http.get('/Article/GetLatestNewses?skip=' + ($scope.latestNewses.skip + $scope.latestNewses.take) + '&take=' + $scope.latestNewses.take).success(function (data) {
                    $scope.latestNewses.Articles = $scope.latestNewses.Articles.concat(data);
                    $scope.latestNewses.skip += $scope.latestNewses.take;
                    $scope.loadMoreLoading = false;
                });
            };
            
            $scope.$on('onRepeatLast', function (scope, element, attrs) {
                bannerSetCarousel();
                carousel();
            });
        }
    ]);