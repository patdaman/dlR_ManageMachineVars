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

        var viewAppDetail;
        var viewMachineDetail;
        var keepAlive;
        var pages;

        var machineApps = [];
        var appIndex;
        var appPoolName;
        var appMessage;
        var active;
        var physicalPath;
        var siteId;
        var siteName;
        var state;
        var configKeys = [];
        var bindings = [];

        vm.machineData = machineData;
        vm.machineName = machineData.machine_name;
        vm.environment = machineData.environment;
        vm.uri = machineData.uri;
        vm.applications = machineData.Applications;

        vm.appIndex = 0;
        vm.viewAppDetail = 'no';
        vm.viewMachineDetail = true;
        vm.viewNav = false;
        vm.keepAlive = 'n/a';
        vm.stopStart = 'Stopped';

        vm.page = function () {
            return '' + (vm.appIndex + 1)
        }

        vm.viewApplications = function (siteName) {
            var params;
            if (siteName)
                params = {
                    machineName: vm.uri,
                    application: siteName,
                }
            else if (vm.selectedApplication)
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
                vm.machineApps = data;
                vm.pages = data.length;
                vm.siteData();
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
            .success(function () {
                viewAppDetail = 'yes';
            })
            .error(function (error) {
                console.log(error);
            })
            return def.promise;
        };

        vm.siteData = function () {

            vm.appPoolName = vm.machineApps[vm.appIndex].appPoolName;
            vm.active = vm.machineApps[vm.appIndex].active;
            if (vm.active)
                vm.keepAlive = 'OK';
            vm.appMessage = vm.machineApps[vm.appIndex].message;
            vm.physicalPath = vm.machineApps[vm.appIndex].physicalPath;
            vm.siteId = vm.machineApps[vm.appIndex].siteId;
            vm.siteName = vm.machineApps[vm.appIndex].name;
            vm.state = vm.machineApps[vm.appIndex].state;
            vm.configKeys = vm.machineApps[vm.appIndex].configKeys;
            vm.bindings = vm.machineApps[vm.appIndex].bindings;
        }

        vm.stopStartSite = function () {
            var message;
            if (vm.state == 'Started')
                message = vm.siteName + " will be stopped!";
            else
                message = vm.siteName + " will be started";
            swal({
                title: "Are you sure?",
                text: message,
                type: "warning",
                showCancelButton: true,
                //confirmButtonColor: "#AEDEF4",
                confirmButtonText: "Yes, stop it!",
                closeOnConfirm: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    vm.updateSite(vm.siteName)
                    .then(function () {
                        message = vm.siteName + " " + vm.state;
                        swal({
                            title: vm.siteName,
                            text: message,
                            type: "success",
                            imageUrl: "/Assets/thumbs-up.jpg",
                            confirmButtonText: "Cool"
                        });
                        vm.viewApplications();
                    });
                }
                else {
                    swal({
                        title: "Stop / Start " + vm.siteName,
                        text: vm.siteName + " status not changed",
                        type: "warning",
                        confirmButtonText: "Cool"
                    });
                };
            });
        }

        vm.updateSite = function (siteName) {
            if (!selectedApplication)
                vm.viewAppDetail = 'yes';
        }

        vm.previous = function () {
            if (vm.appIndex != 0) {
                vm.appIndex = vm.appIndex - 1;
                vm.siteData();
            }
        }
        vm.next = function () {
            if (vm.appIndex != vm.pages) {
                vm.appIndex = vm.appIndex + 1;
                vm.siteData();
            }
        }
        vm.refresh = function (application) {
            vm.viewApplications(vm.siteName);
        }

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
        'header', 'varId', 'noteText', 'title',
        'createDate', 'lastModifiedUser', 'lastModifiedDate',
    function ($rootScope, $scope, $element, close,
        header, varId, noteText, title,
        createDate, lastModifiedUser, lastModifiedDate) {

        var vm = $scope;
        var save;
        var modalSize;

        vm.header = header;
        vm.varId = varId;
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
                machine_id: vm.varId,
                noteText: vm.noteText,
            }, 500);
        };
    }]);