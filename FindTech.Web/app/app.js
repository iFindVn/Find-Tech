var app = angular.module('FindTechApp', ['FindTech.ArticleDetail', 'FindTech.ArticleList', 'FindTech.Home', 'iso.directives', 'angular-ladda']);

//app.config(function($locationProvider) {
//    $locationProvider.html5Mode(true).hashPrefix('!');
//});

app.controller('FindTechCtrl', ['$scope', '$http', 'Page', function ($scope, $http, Page) {
    $scope.Page = Page;
}]);

app.run(function ($rootScope, $http) {
    $rootScope.pinnedArticles = {
        Title: 'Đã ghim',
        TitleStyleClass: 'fa fa-thumb-tack background-warning',
        ClientId: 'pinnedArticles',
        Url: '/ghim'
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

    $rootScope.getBoxSizeClass = function (boxSize) {
        return boxSize != 2 ? 'col-sm-6 col-md-3' : 'col-sm-12 col-md-6';
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

app.directive('largeSliderBox', function ($rootScope) {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/large-slider-box.html',
        link: function(scope) {
            scope.getPinnedClass = $rootScope.getPinnedClass;
            scope.pinArticle = $rootScope.pinArticle;
            scope.getBoxSizeClass = $rootScope.getBoxSizeClass;
        }
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

app.directive('isotopeBox', function ($rootScope) {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/isotope-box.html',
        link: function (scope) {
            scope.getPinnedClass = $rootScope.getPinnedClass;
            scope.pinArticle = $rootScope.pinArticle;
            scope.getBoxSizeClass = $rootScope.getBoxSizeClass;
        }
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