var app = angular.module('FindTechApp', ['FindTech.ArticleDetail', 'FindTech.Home', 'iso.directives', 'angular-ladda']);

app.controller('FindTechCtrl', ['$scope', '$http', 'Page', function ($scope, $http, Page) {
    $scope.Page = Page;
}]);

app.run(function ($rootScope, $http) {
    $rootScope.pinnedArticles = {
        Title: 'Đã ghim',
        TitleStyleClass: 'fa fa-thumb-tack background-warning',
        ClientId: 'pinnedArticles'
    };
    $rootScope.pinArticle = function (articleId) {
        $http.post('/Article/Pin', { articleId: articleId }).success(function (data) {
            $rootScope.pinnedArticles.Articles = data;
        });
    };

    $http.get('/Article/GetPinnedArticles').success(function (data) {
        $rootScope.pinnedArticles.Articles = data;
    });

    $rootScope.getPinnedClass = function (articleId) {
        var pinnedClass = '';
        if ($rootScope.pinnedArticles.Articles) {
            $rootScope.pinnedArticles.Articles.some(function (article) {
                if (article.hasOwnProperty('ArticleId') && article['ArticleId'] === articleId) {
                    pinnedClass = 'active';
                }
            });
        }
        return pinnedClass;
    };
});

app.factory('Page', function () {
    var title = '';
    var description = '';
    return {
        getTitle: function() { return title; },
        setTitle: function (newTitle) { title = newTitle; },
        getDescription: function () { return description; },
        setDescription: function (newDescription) { description = newDescription; }
    };
});

app.directive('miniListBox', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/mini-list-box.html'
    };
});

app.directive('miniSliderBox', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/mini-slider-box.html'
    };
});

app.directive('largeSliderBox', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/large-slider-box.html'
    };
});

app.directive('imageGallerySlider', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/image-gallery-slider.html'
    };
});

app.directive('opinionPanel', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/opinion-panel.html'
    };
});

app.directive('opinionForm', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/opinion-form.html'
    };
});

app.directive('onLastRepeat', function() {
    return function(scope, element, attrs) {
        if (scope.$last)
            setTimeout(function() {
                scope.$emit('onRepeatLast', element, attrs);
            }, 1);
    };
});

app.filter('html', function ($sce) {
    return function (htmlString) {
        return $sce.trustAsHtml(htmlString);
    };
});

app.filter('moment', function() {
    return function (date) {
        moment.locale('vi');
        return moment(date).fromNow();
    };
});

app.filter('width', function () {
    return function (url, width, defaultUrl) {
        return url ? url.replace('{width}', width) : defaultUrl;
    };
});