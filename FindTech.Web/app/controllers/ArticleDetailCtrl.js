angular.module('FindTech.ArticleDetail', [])
    .controller('ArticleDetailCtrl', ['$scope', '$http',
        function ($scope, $http) {
            $scope.article = {};
            $http.get('/Comment/GetArticleDetail?article=1&objectId=1384').success(function (data) {
                $scope.comments = data;
            });

            $scope.comments = {};
            $http.get('/Comment/GetComments?objectType=1&objectId=1384').success(function (data) {
                $scope.comments = data;
            });
        }
    ]);