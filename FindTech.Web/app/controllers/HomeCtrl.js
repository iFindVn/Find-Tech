angular.module('FindTech.Home', [])
    .controller('HomeCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            Page.setTitle('Tìm là thấy');
            Page.setDescription('Cổng thông tin công nghệ, thiết bị di động, so sánh sản phẩm công nghệ, đánh giá smart phone, tablet,...');

            $scope.hotArticles = {};
            $http.get('/Article/GetHotArticles?skip=0&take=10').success(function (data) {
                $scope.hotArticles = data;
            });

            $scope.latestReviews = {};
            $http.get('/Article/GetLatestReviews?skip=0&take=10').success(function (data) {
                $scope.latestReviews = data;
            });

            $scope.pinnedArticles = {};
            $http.get('/Article/GetPinnedArticles').success(function (data) {
                $scope.pinnedArticles = data;
            });

            $scope.getPinnedClass = function (articleId) {
                console.log('pinned' + articleId);
                return 'active';
            };
        }
    ]);