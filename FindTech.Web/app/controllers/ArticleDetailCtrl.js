angular.module('FindTech.ArticleDetail', [])
    .controller('ArticleDetailCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            $scope.states = {                
                ratedOpinion: false
            };

            $scope.hotNewses = {};
            $http.get('/Article/GetHotNewses?skip=0&take=10').success(function (data) {
                $scope.hotNewses = data;
            });

            $scope.pinnedArticles = {};
            $http.get('/Article/GetPinnedArticles').success(function (data) {
                $scope.pinnedArticles = data;
            });

            $scope.absUrl = $location.absUrl();
            var path = $scope.absUrl.split('/bai-viet/')[1];
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

                $scope.sameCategoryNewses = {};
                $http.get('/Article/GetArticleByCatogories?categories=' + $scope.article.ArticleCategorySeoName + '&articleType=1&skip=0&take=10').success(function (sameCategoryNewses) {
                    $scope.sameCategoryNewses = sameCategoryNewses;
                });
                
                $scope.relatedNewses = {};
                $http.get('/Article/GetArticleByCatogories?tags=' + $scope.article.Tags + '&articleType=1&skip=0&take=10').success(function (relatedNewses) {
                    $scope.relatedNewses = relatedNewses;
                });
            });

            $scope.rateOpinion = function (articleId, opinionLevel) {
                $http.post('/Article/RateOpinion', { articleId: articleId, opinionLevel: opinionLevel }).success(function(data) {
                    $scope.article.Opinions = data;
                    $scope.states.ratedOpinion = true;
                });
            };
        }
    ]);