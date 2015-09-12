angular.module('FindTech.Service', [])
    .factory('DeviceService', ['$http',
        function ($http) {
            var service = {};

            service.getDevice = function () {
                return $http.get("/BO/DeviceBO/GetSpecs");
            };

            return service;

        }])