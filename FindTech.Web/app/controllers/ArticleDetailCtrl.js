angular.module('FindTech.ArticleDetail', [])
    .controller('ArticleDetailCtrl', ['$scope', '$http', '$location', 'Page',
        function ($scope, $http, $location, Page) {
            $scope.states = {                
                ratedOpinion: false
            };

            $scope.absUrl = $location.absUrl();
            var path = $scope.absUrl.split('/bai-viet/')[1];
            $scope.seoTitle = path.split('/')[1];
            $scope.currentPage = path.split('/')[2] ? path.split('/')[2] : 1;
            $scope.article = {};
            $scope.contentSectionPageManager = {};
            $scope.comments = {};
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
            $http.get('/Article/GetArticleDetail?seoTitle=' + $scope.seoTitle + '&page=' + $scope.currentPage).success(function (data) {
                $scope.article = data.article;
                Page.setTitle($scope.article.Title);
                Page.setDescription($scope.article.Description);

                $scope.contentSectionPageManager = data.contentSectionPageManager;
                $scope.sameCategoryNewses.Articles = data.sameCategoryNewses;
                $scope.relatedNewses.Articles = data.relatedNewses;
                $scope.hotNewses.Articles = data.hotNewses;
                $http.get('/Comment/GetComments?objectType=1&objectId=' + $scope.article.ArticleId).success(function (comments) {
                    $scope.comments = comments;
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