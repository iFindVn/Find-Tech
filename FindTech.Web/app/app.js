var app = angular.module('FindTechApp', ['FindTech.ArticleDetail']);

app.controller('FindTechCtrl', ['$scope', 'Page', function ($scope, Page) {
    $scope.Page = Page;
}]);

app.factory('Page', function () {
    var title = '';
    var description = '';
    return {
        getTitle: function() { return title; },
        setTitle: function (newTitle) { title = newTitle; },
        getDescription: function () { return description; },
        setDescription: function(newDescription) { description = newDescription; }
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

app.directive('miniListItem', function () {
    return {
        restrict: 'E',
        scope: {
            model: '='
        },
        templateUrl: '/app/templates/mini-list-item.html'
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