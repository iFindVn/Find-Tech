angular.module('FindTech.ArticleDetail', [])
    .controller('ArticleDetailCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            $scope.latestReviews = {};
            $http.get('/Article/GetLatestReviews').success(function (data) {
                $scope.latestReviews = data;
            });

            $scope.pinnedArticles = {};
            $http.get('/Article/GetPinnedArticles').success(function (data) {
                $scope.pinnedArticles = data;
            });

            var absUrl = $location.absUrl();
            var path = absUrl.split('/bai-viet/')[1];
            $scope.seoTitle = path.split('/')[1];
            $scope.currentPage = path.split('/')[2] ? path.split('/')[2] : 1;
            $scope.article = {};
            $scope.contentSectionPageManager = {};
            $scope.comments = {};
            $http.get('/Article/GetArticleDetail?seoTitle=' + $scope.seoTitle + '&page=' + $scope.currentPage).success(function (data) {
                $scope.article = data;
                Page.setTitle($scope.article.Title);
                Page.setDescription($scope.article.Description);

                $http.get('/Article/GetContentSectionPages?articleId=' + +$scope.article.ArticleId + '&page=' + $scope.currentPage).success(function (contentSectionPageManager) {
                    $scope.contentSectionPageManager = contentSectionPageManager;
                });


                $http.get('/Comment/GetComments?objectType=1&objectId=' + $scope.article.ArticleId).success(function (comments) {
                    $scope.comments = comments;
                });
            });
        }
    ]);