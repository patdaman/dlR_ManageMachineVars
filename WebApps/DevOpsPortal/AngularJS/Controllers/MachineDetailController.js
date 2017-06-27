MachineApp.controller('MachineDetailController', ['$rootScope', '$scope', '$element',
    '$q', '$httpParamSerializer', '$http', '$timeout',
    'close', 'machineData', 'machineApplications',
    'locations', 'applications', 'environments', 'environment',
    'header', 'varId', 'noteText', 'title',
    'createDate', 'lastModifiedUser', 'lastModifiedDate',
    function ($rootScope, $scope, $element,
        $q, $httpParamSerializer, $http, $timeout,
        close, machineData, machineApplications,
        locations, applications, environments, environment,
        header, varId, noteText, title,
        createDate, lastModifiedUser, lastModifiedDate) {

        var vm = $scope;
        var myPromise;
        var save = false;
        var publish = false;
        var machineName;
        var uri;
        var environment;
        var configVars;
        var selectedApplication;
        var applications;
        var selectedSite;
        var sites = [];

        var viewAppDetail;
        var viewMachineDetail;
        var pages;

        var machineApps = [];
        var appIndex;
        var appPoolName;
        var appMessage;
        var active;
        var physicalPath;
        var hostName;
        var serverName;
        var siteId;
        var siteName;
        var state;
        var changeState;
        var keepAlive;
        var changeKeepAlive;
        var configKeys = [];
        var bindings = [];
        var keysCollapsed;
        var bindingsCollapsed;

        var myPromise;
        var availableApplications = [];
        var machineId;
        var machineEnvironment;
        var machineLocation;
        var active;
        var save;
        var saveMachine;
        var deleteMachine;

        vm.machineData = machineData;
        vm.sites = [];
        vm.machineName = machineData.machine_name;
        vm.environment = machineData.environment;
        vm.uri = machineData.machine_name + '.' + machineData.uri;
        vm.applications = machineData.Applications;

        vm.appIndex = 0;
        vm.viewAppDetail = 'no';
        vm.viewMachineDetail = true;
        vm.keysCollapsed = false;
        vm.bindingsCollapsed = true;

        vm.active = true;
        vm.saveMachine = false;
        vm.deleteMachine = false;

        vm.availableApplications = applications;
        vm.machineApplications = machineApplications;
        vm.machineEnvironment = environment;
        vm.applications = applications;
        vm.environments = environments.map(function (value) { return { environment: value }; });
        vm.environment = environment;

        vm.save = false;
        vm.header = header;
        vm.varId = varId;
        vm.noteText = "";
        vm.lastModifiedDate = lastModifiedDate;
        vm.lastModifiedUser = lastModifiedUser;
        vm.noteText = noteText;
        vm.title = title;

        vm.page = function () {
            return '' + (vm.appIndex + 1)
        }

        vm.collapseKeys = function () {
            vm.keysCollapsed = !vm.keysCollapsed;
        };
        vm.collapseBindings = function () {
            vm.bindingsCollapsed = !vm.bindingsCollapsed;
        };

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
                angular.forEach(data, function (data, index) {
                    var name = data.name;
                    vm.sites.push({
                        index: index,
                        name: name,
                    });
                });
                vm.pages = '' + data.length;
                vm.siteData(vm.appIndex);
            });
        };

        vm.getIisData = function (params) {
            var def = $q.defer();
            vm.myPromise = $http({
                method: 'GET',
                url: 'api:/IISApi',
                params: params
            })
            .success(def.resolve)
            .error(function (error) {
                console.log(error);
            })
            return def.promise;
        };

        vm.siteData = function (index) {
            vm.appIndex = index;
            vm.appPoolName = vm.machineApps[index].appPoolName;
            vm.active = vm.machineApps[index].active;
            if (vm.active == true)
                vm.changeState = 'Started';
            else
                vm.changeState = 'Stopped';
            vm.keepAlive = vm.machineApps[index].keepAlive;
            if (vm.keepAlive == true)
                vm.changeKeepAlive = 'OK';
            else if (vm.keepAlive == false)
                vm.changeKeepAlive = 'Inactive';
            else
                vm.changeKeepAlive = 'n/a'
            vm.appMessage = vm.machineApps[index].message;
            vm.physicalPath = vm.machineApps[index].physicalPath;
            vm.siteId = vm.machineApps[index].siteId;
            vm.siteName = vm.machineApps[index].name;
            vm.state = vm.machineApps[index].state;
            vm.configKeys = vm.machineApps[index].configKeys;
            vm.bindings = vm.machineApps[index].bindings;
            vm.hostName = vm.machineApps[index].hostName;
            vm.ipAddress = vm.machineApps[index].ipAddress;
            vm.serverName = vm.machineApps[index].serverName;
        };

        vm.stopStartSite = function () {
            var message;
            if (vm.active == true)
                message = vm.siteName + " will be stopped!";
            else
                message = vm.siteName + " will be started";
            //if (vm.updateSiteWarning(message)) {
            swal({
                title: "Are you sure?",
                text: message,
                type: "warning",
                showCancelButton: true,
                //confirmButtonColor: "#AEDEF4",
                confirmButtonText: "Yes!",
                closeOnConfirm: true
            },
            function (isConfirm) {
                if (isConfirm) {
                    if (vm.active == false) {
                        vm.active = true;
                        message = vm.siteName + ' has been started';
                    }
                    else {
                        vm.active = false;
                        message = vm.siteName + ' has been stopped';
                    };
                    vm.updateSite()
                    .then(function () {
                        swal({
                            title: vm.siteName,
                            text: message,
                            type: "success",
                            imageUrl: "/Assets/thumbs-up.jpg",
                            confirmButtonText: "Cool"
                        });
                    });
                }
                else {
                    vm.$apply();
                    vm.siteData(vm.appIndex);
                };
            });
        };

        vm.updateKeepAlive = function () {
            var message;
            if (vm.keepAlive == false)
                message = vm.siteName + " will be added to server pool";
            else
                message = vm.siteName + " will be removed from server pool";
            //if (vm.updateSiteWarning(message)) {
            swal({
                title: "Are you sure?",
                text: message,
                type: "warning",
                showCancelButton: true,
                //confirmButtonColor: "#AEDEF4",
                confirmButtonText: "Yes!",
                closeOnConfirm: true
            },
            function (isConfirm) {
                if (isConfirm) {
                    if (vm.keepAlive == false) {
                        vm.keepAlive = true;
                        message = vm.siteName + ' has been added to server pool';
                    }
                    else {
                        vm.keepAlive = false;
                        message = vm.siteName + ' has been removed from server pool';
                    };
                    vm.updateSite()
                    .then(function () {
                        swal({
                            title: vm.siteName,
                            text: message,
                            type: "success",
                            imageUrl: "/Assets/thumbs-up.jpg",
                            confirmButtonText: "Cool"
                        });
                    });
                }
                else {
                    vm.$apply();
                    vm.siteData(vm.appIndex);
                };
            });
        };

        vm.confirmSiteChange = function (message) {
            swal({
                title: vm.siteName,
                text: message,
                type: "success",
                imageUrl: "/Assets/thumbs-up.jpg",
                confirmButtonText: "Cool"
            });
        };

        vm.updateSite = function () {

            // REFACTOR!
            //  Use OO Model!!
            var data;
            data = {
                hostName: vm.hostName,
                ipAddress: vm.ipAddress,
                message: vm.appMessage,
                serverName: vm.serverName,
                appPoolName: vm.appPoolName,
                active: vm.active,
                keepAlive: vm.keepAlive,
                physicalPath: vm.physicalPath,
                siteId: vm.siteId,
                name: vm.siteName,
                state: vm.state,
                configKeys: vm.configKeys,
                bindings: vm.bindings,
            };
            // end refactor

            var def = $q.defer();
            vm.myPromise = $http({
                method: 'PUT',
                url: 'api:/IISApi',
                data: data,
            })
            .success(def.resolve)
            .error(function () {
                vm.viewApplications();
            })
            return def.promise
            .then(function (data) {

                // Refactor!
                // use OO model
                vm.machineApps[vm.appIndex].appPoolName = data.appPoolName;
                vm.machineApps[vm.appIndex].active = data.active;
                vm.machineApps[vm.appIndex].keepAlive = data.keepAlive;
                vm.machineApps[vm.appIndex].message = data.appMessage;
                vm.machineApps[vm.appIndex].physicalPath = data.physicalPath;
                vm.machineApps[vm.appIndex].siteId = data.siteId;
                vm.machineApps[vm.appIndex].siteName = data.name;
                vm.machineApps[vm.appIndex].state = data.state;
                vm.machineApps[vm.appIndex].configKeys = data.configKeys;
                vm.machineApps[vm.appIndex].bindings = data.bindings;
                vm.machineApps[vm.appIndex].hostName = data.hostName;
                vm.machineApps[vm.appIndex].ipAddress = data.ipAddress;
                vm.machineApps[vm.appIndex].serverName = data.serverName;
                // end refactor
                vm.siteData(vm.appIndex);
            });
        }

        vm.previous = function () {
            if (vm.appIndex != 0) {
                vm.appIndex = vm.appIndex - 1;
                vm.siteData(vm.appIndex);
            }
        }
        vm.next = function () {
            if (vm.appIndex != vm.pages) {
                vm.appIndex = vm.appIndex + 1;
                vm.siteData(vm.appIndex);
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
        vm.viewApplications();
    }])
