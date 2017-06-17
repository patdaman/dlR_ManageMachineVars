MachineApp.controller('MachineDetail', ['$rootScope', '$scope', '$element',
    '$q', '$httpParamSerializer', '$http',
    'close', 'machineData',
    function ($rootScope, $scope, $element,
        $q, $httpParamSerializer, $http,
        close, machineData) {

        var vm = $scope;
        var save = false;
        var publish = false;
        var machineName;
        var uri;
        var environment;
        var configVars;
        var selectedApplication;
        var applications;

        vm.machineData = machineData;
        vm.machineName = machineData.machine_name;
        vm.environment = machineData.environment;
        vm.uri = machineData.uri;
        vm.applications = machineData.Applications;

        vm.viewApplications = function (application) {
            var params;
            if (vm.selectedApplication)
                params = {
                    machineName: vm.uri,
                    application: vm.selectedApplication.application_name,
                }
            else {
                params = {
                    machineName: vm.uri,
                }
            };
            vm.getIisData(params)
            .then(function (data) {

            });
        };

        vm.getIisData = function (params) {
            var def = $q.defer();
            $http({
                method: 'GET',
                url: 'api:/IISApi',
                params: params
            })
            .success(def.resolve)
            //.success(function (data) {
            //    vm.configVars = data.configKeys;
            //})
            .error(function (error) {
                console.log(error);
            })
            return def.promise;
        };

        vm.close = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
                save: vm.save,
                publish: vm.publish,
            }, 500);
        };
    }])

.controller('noteViewer', ['$rootScope', '$scope', '$element', 'close',
        'machineName', 'machine_id', 'noteText', 'title',
        'createDate', 'lastModifiedUser', 'lastModifiedDate',
    function ($rootScope, $scope, $element, close,
        machineName, machine_id, noteText, title,
        createDate, lastModifiedUser, lastModifiedDate) {

        var vm = $scope;
        var save;
        var modalSize;

        vm.machineName = machineName;
        vm.machine_id = machine_id;
        vm.noteText = "";
        vm.lastModifiedDate = lastModifiedDate;
        vm.lastModifiedUser = lastModifiedUser;
        vm.noteText = noteText;
        vm.modalSize = "modal-dialog modal-md";
        vm.title = title;

        vm.close = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                machine_id: vm.machine_id,
                noteText: vm.noteText,
            }, 500);
        };
    }]);