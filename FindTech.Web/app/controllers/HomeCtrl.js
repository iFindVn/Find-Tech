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
                Title: 'Tin tức',
                loadMore: $scope.loadMoreNewses,
                loadMoreLoading: false,
                skip: 0,
                take: 10
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
            $scope.entertainmentChannelArticles = {
                Title: 'Kênh giải trí'
            };
            $scope.hotReviews = {
                Title: 'Soi nóng',
                TitleStyleClass: 'fa fa-eye background-danger'
            };

            $scope.init = function (hotArticles, latestReviews, trickAndTipArticles, entertainmentChannelArticles, appAndGameArticles, productAndTechToyArticles, brandAndDigiLifeArticles, latestNewses, hotReviews) {
                $scope.hotArticles.Articles = hotArticles;
                $scope.latestReviews.Articles = latestReviews;
                $scope.latestReviews.Url = '/soi';
                $scope.hotReviews.Articles = hotReviews;
                $scope.hotReviews.Url = '/soi-nong';
                $scope.appAndGameArticles.Articles = appAndGameArticles;
                $scope.appAndGameArticles.Url = '/danh-muc/ung-dung-va-game';
                $scope.productAndTechToyArticles.Articles = productAndTechToyArticles;
                $scope.productAndTechToyArticles.Url = '/danh-muc/san-pham,do-choi-cong-nghe';
                $scope.brandAndDigiLifeArticles.Articles = brandAndDigiLifeArticles;
                $scope.brandAndDigiLifeArticles.Url = '/danh-muc/thuong-hieu,doi-song-so';
                $scope.trickAndTipArticles.Articles = trickAndTipArticles;
                $scope.trickAndTipArticles.Url = '/danh-muc/thu-thuat-va-meo-vat';
                $scope.entertainmentChannelArticles.Articles = entertainmentChannelArticles;
                $scope.entertainmentChannelArticles.Url = '/danh-muc/kenh-giai-tri';
                $scope.latestNewses.Articles = latestNewses;
                $scope.latestNewses.Url = '/tin-tuc';
            };
            
            $scope.$on('onRepeatLast', function (scope, element, attrs) {
                bannerSetCarousel();
                carousel();
            });
        }
    ]);