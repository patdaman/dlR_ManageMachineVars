﻿'use strict'

///-------------------------------------------------------------------------------------------------
/// <summary>   Controller for the Config File Viewer / Downloader Modal. </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="'ConfigViewer'">   The configuration viewer'. </param>
/// <param name="($rootScope">      The $root scope. </param>
/// <param name="$scope">           The $scope. </param>
/// <param name="$element">         The $element. </param>
/// <param name="title">            The title. </param>
/// <param name="filePath">         Full pathname of the file. </param>
/// <param name="configXml">        The configuration XML. </param>
/// <param name="close">            The close. </param>
/// <param name="publish">          The publish. </param>
/// <param name="download">         The download. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('ConfigViewer',
    function ($rootScope, $scope, $element,
        title, filePath, fileName, configXml,
        close, publish, download) {

        //var vm = this;
        var vm = $scope;
        vm.filePath = filePath;
        vm.fileName = fileName;
        vm.configXml = configXml;
        vm.title = title;

        vm.Copy = function () {

        };

        vm.close = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: false,
            }, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: false,
            }, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                publish: true,
                download: false,
                fileName: vm.fileName,
                title: vm.title
            }, 500);
        }
        vm.download = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: true,
                fileName: vm.fileName,
                title: vm.title
            }, 500);
        }
    });

///-------------------------------------------------------------------------------------------------
/// <summary>   Controllers. </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="'AddComponent'">       The add component'. </param>
/// <param name="($rootScope">          The $root scope. </param>
/// <param name="$scope">               The $scope. </param>
/// <param name="$element">             The $element. </param>
/// <param name="close">                The close. </param>
/// <param name="filePath">             Full pathname of the file. </param>
/// <param name="components">           The components. </param>
/// <param name="componentComponents">  The component components. </param>
/// <param name="componentName">        Name of the component. </param>
/// <param name="fileName">             Filename of the file. </param>
/// <param name="componentEnvironment"> The component environment. </param>
/// <param name="environments">         The environments. </param>
/// <param name="file">                 The file. </param>
/// <param name="publish">              The publish. </param>
/// <param name="upload">               The upload. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('AddComponent',
    function ($rootScope, $scope, $element, $http, $timeout,
        Upload, close, components, applications, environments) {

        var apiRelPath = "api:/ComponentApi";
        var vm = $scope;
        var componentData;
        var componentComponents;
        var filePath = filePath;
        var availableApplications = [];
        var componentApplications = [];
        var componentName;
        var componentEnvironment;
        var fileName;
        var localComponent;

        vm.availableApplications = applications;
        vm.componentApplications = [];
        vm.componentName = "";
        vm.componentEnvironment = "";
        vm.fileName = "";
        vm.applications = applications;
        vm.components = components;
        vm.environments = environments;
        vm.localComponent = {
            componentName: vm.componentName,
            filePath: vm.filePath,
            applications: vm.componentApplications,
        }

        vm.selectComponent = function (component) {
            $http({
                method: 'GET',
                url: apiRelPath,
                params: {
                    componentName: component.name,

                },
            }).then(function (result) {
                vm.componentData = result.data;
                vm.availableApplications = vm.applications;
                if (typeof vm.componentData.ConfigFile !== "undefined")
                    vm.fileName = vm.componentData.ConfigFiles.file_name;
                vm.filePath = vm.componentData.relative_path;
                vm.componentName = vm.componentComponents.name;
                vm.componentApplications = [];
                angular.forEach(result.data.Applications, function (Applications) {
                    var name = Applications.application_name;
                    var id = Applications.id;
                    var value = Applications.application_name;
                    vm.componentApplications.push({
                        id: id,
                        name: name.replace(/"/g, '').replace(/'/g, '')
                            .replace(/\[/g, '').replace(/]/g, ''),
                        value: value.replace(/"/g, '').replace(/'/g, '')
                            .replace(/\[/g, '').replace(/]/g, ''),
                    });
                });
                vm.applicationNames = JSON.stringify(vm.componentApplications.map(function (item) {
                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                        .replace(/\[/g, '').replace(/]/g, '');
                }));
                for (var i = vm.availableApplications.length - 1; i >= 0; i--) {
                    for (var j = 0; j < vm.componentApplications.length; j++) {
                        if (vm.availableApplications[i] && (vm.availableApplications[i].name === vm.componentApplications[j].name)) {
                            vm.availableApplications.splice(i, 1);
                        }
                    }
                }
                vm.localComponent = {
                    componentName: vm.componentName,
                    filePath: vm.filePath,
                    applications: vm.componentApplications,
                }
            });
        };

        vm.$watch('files', function () {
            $scope.upload($scope.files);
        });
        vm.$watch('file', function () {
            if ($scope.file !== null) {
                $scope.files = [$scope.file];
            }
        });
        vm.log = '';

        vm.upload = function (file) {
            if (typeof file !== 'undefined') {
                if (!file.$error && typeof file.name !== 'undefined') {
                    if (vm.componentName === '') {
                        swal({
                            title: "Add Component",
                            text: "No Component Name / Environment Supplied",
                            type: "error",
                            confirmButtonText: "Cool"
                        });
                    }
                    else {
                        var jsonApps = JSON.stringify(vm.componentApplications.map(function (item) {
                            return item['name'].replace(/"/g, '').replace(/'/g, '')
                                .replace(/\[/g, '').replace(/]/g, '');
                        }));
                        Upload.upload({
                            url: 'api:/ConfigPublishApi',
                            params: {
                                componentName: vm.componentName,
                                environment: vm.componentEnvironment.name,
                                applications: jsonApps,
                                //applications: vm.componentApplications,
                            },
                            data: {
                                file: file
                            }
                        }).then(function (resp) {
                            $timeout(function () {
                                $scope.log = 'file: ' +
                                resp.config.data.file.name +
                                ', Response: ' + JSON.stringify(resp.data) +
                                '\n' + $scope.log;
                            });
                        }, null, function (evt) {
                            var progressPercentage = parseInt(100.0 *
                                    evt.loaded / evt.total);
                            $scope.log = 'progress: ' + progressPercentage +
                                '% ' + evt.config.data.file.name + '\n' +
                              $scope.log;
                        })
                        .then(
                            swal({
                                title: vm.componentName,
                                text: "Config File Added",
                                type: "success",
                                confirmButtonText: "Cool"
                            }))
                        .then(vm.close);
                    }
                }
            }
        }

        vm.close = function () {
            $element.modal('hide');
            close({
            }, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');

            close({
                publish: false,
                save: false,
            }, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                save: true,
                publish: true,
                componentApplications: vm.componentApplications,
                componentName: vm.componentName,
                filePath: vm.filePath,
            }, 500);
        }
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                publish: false,
                componentApplications: vm.componentApplications,
                componentName: vm.componentName,
                filePath: vm.filePath,
            }, 500);
        }
    });

///-------------------------------------------------------------------------------------------------
/// <summary>   Add Application Controller </summary>
///
/// <remarks>   Pdelosreyes, 5/5/2017. </remarks>
///
/// <param name="'AddApplication'"> The add application'. </param>
/// <param name="($rootScope">      The $root scope. </param>
/// <param name="$scope">           The $scope. </param>
/// <param name="$element">         The $element. </param>
/// <param name="$http">            The $http. </param>
/// <param name="$timeout">         The $timeout. </param>
/// <param name="close">            The close. </param>
/// <param name="components">       The components. </param>
/// <param name="applications">     The applications. </param>
/// <param name="environments">     The environments. </param>
///
/// <returns>
/// A /ApplicationApi"; var vm = $scope; var applicationData; var applicationApplications; var
/// filePath = filePath; var availableComponents = []; var applicationComponents = [];
/// </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('AddApplication',
    function ($rootScope, $scope, $element, $http, $timeout,
        close, components, applications, environments) {

        var apiRelPath = "api:/ApplicationApi";
        var vm = $scope;
        var applicationData;
        var applicationApplications;
        var availableComponents = [];
        var applicationComponents = [];
        var filteredApplicationComponents = [];
        var componentNames;
        var id;
        var applicationName;
        var localApplication;
        var release;

        vm.availableComponents = components;
        vm.applicationComponents = [];
        vm.filteredApplicationComponents = [];
        vm.id = "";
        vm.applicationName = "";
        vm.release = "";
        vm.applications = applications;
        vm.componentNames = "";
        vm.components = components;
        vm.environments = environments;
        vm.localApplication = {
            componentData: vm.applicationComponents,
            id: vm.id,
            applicationName: vm.applicationName,
            release: vm.release,
            components: vm.applicationComponents,
        }

        vm.selectApplication = function (application) {
            $http({
                method: 'GET',
                url: apiRelPath,
                params: {
                    applicationName: application.name,
                },
            }).then(function (result) {
                vm.availableComponents = vm.components;
                vm.applicationData = result.data;
                vm.applicationName = vm.applicationApplications.name;
                vm.release = vm.applicationData.release;
                vm.applicationComponents = [];
                angular.forEach(vm.applicationData.Components, function (Components) {
                    var name = Components.component_name;
                    var id = Components.id;
                    var value = Components.component_name;
                    vm.applicationComponents.push({
                        id: id,
                        name: name.replace(/\[/g, '').replace(/]/g, '')
                            .replace(/"/g, '').replace(/'/g, ''),
                        value: value.replace(/\[/g, '').replace(/]/g, '')
                            .replace(/"/g, '').replace(/'/g, ''),
                    });
                });
                vm.componentNames = JSON.stringify(vm.applicationComponents.map(function (item) {
                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                        .replace(/\[/g, '').replace(/]/g, '');
                }));
                for (var i = vm.availableComponents.length - 1; i >= 0; i--) {
                    for (var j = 0; j < vm.applicationComponents.length; j++) {
                        if (vm.availableComponents[i] && (vm.availableComponents[i].name === vm.applicationComponents[j].name)) {
                            vm.availableComponents.splice(i, 1);
                        }
                    }
                }
                vm.localApplication = {
                    componentData: vm.applicationComponents,
                    componentNames: vm.componentNames,
                    id: vm.applicationData.id,
                    release: vm.release,
                    applicationName: vm.applicationName,
                }
            });
        };

        vm.close = function () {
            $element.modal('hide');
            close({
            }, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');

            close({
                publish: false,
                save: false,
            }, 500);
        };
        vm.publish = function () {
            vm.componentNames = JSON.stringify(vm.applicationComponents.map(function (item) {
                return item['name'].replace(/"/g, '').replace(/'/g, '')
                    .replace(/\[/g, '').replace(/]/g, '');
            }));
            $element.modal('hide');
            close({
                save: true,
                publish: true,
                applicationComponents: vm.localApplication.applicationComponents,
                applicationName: vm.applicationName,
                componentNames: vm.componentNames,
                id: vm.localApplication.id,
                release: vm.release,
            }, 500);
        }
        vm.save = function () {
            vm.componentNames = JSON.stringify(vm.applicationComponents.map(function (item) {
                return item['name'].replace(/"/g, '').replace(/'/g, '')
                    .replace(/\[/g, '').replace(/]/g, '');
            }));
            $element.modal('hide');
            close({
                save: true,
                publish: false,
                applicationComponents: vm.filteredApplicationComponents,
                componentNames: vm.componentNames,
                applicationName: vm.applicationName,
                id: vm.localApplication.id,
                release: vm.release,
            }, 500);
        }
    });

///-------------------------------------------------------------------------------------------------
/// <summary>   AddVar Controller </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="close">            The close. </param>
/// <param name="componentName">    Name of the component. </param>
/// <param name="parentElement">    The parent element. </param>
/// <param name="element">          The element. </param>
/// <param name="keyName">          Name of the key. </param>
/// <param name="key">              The key. </param>
/// <param name="valueName">        Name of the value. </param>
/// <param name="save">             The save. </param>
/// <param name="publish">          The publish. </param>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('AddVar',
    function ($rootScope, $scope, $element,
        close, componentName, parentElement, element,
        keyName, key, valueName, show, isNew, save) {

        var vm = $scope;
        var additionalParentElements = [];
        vm.componentName = componentName;
        vm.element = element;
        vm.parentElement = parentElement;
        vm.keyName = keyName;
        vm.key = key;
        vm.valueName = valueName;
        vm.show = show;
        vm.isNew = isNew;
        vm.close = function () {
            $element.modal('hide');
            close({
                save: false,
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({
                save: false,
            }, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                componentName: vm.componentName,
                element: vm.element,
                parentElement: vm.parentElement,
                keyName: vm.keyName,
                key: vm.key,
                valueName: valueName,
                additionalParentElements: vm.additionalParentElements
            }, 500);
        };
    });