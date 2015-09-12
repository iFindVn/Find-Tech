angular.module('FindTech.BO.Device', ["kendo.directives", "FindTech.Service", "uiSlider"])
    .controller('DeviceBOController', ['$scope', '$http', '$location', 'DeviceService',
        function ($scope, $http, $location, DeviceService) {
            $scope.pros = [];
            $scope.cons = [];
            $scope.proValue = '';
            $scope.conValue = '';

            $scope.addPro = function () {
                $scope.pros.push($scope.proValue);
                $scope.proValue = '';
                $scope.onAddPro = false;
            };

            $scope.removePro = function (pro) {
                var index = $scope.pros.indexOf(pro);
                $scope.pros.splice(index, 1);
            };

            $scope.addCon = function () {
                $scope.cons.push($scope.conValue);
                $scope.conValue = '';
                $scope.onAddCon = false;
            };

            $scope.removeCon = function (con) {
                var index = $scope.pros.indexOf(con);
                $scope.cons.splice(index, 1);
            };

            $scope.testApi = function () {
                DeviceService.getDevice().then(
                    function (resp) {
                        alert(JSON.stringify(resp));
                    }, function (err) {
                        alert(JSON.stringify(ress));
                    });
            };
         
        }])