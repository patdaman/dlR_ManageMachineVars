'use strict'

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
    function ($rootScope, $scope, $element, title, filePath, fileName, configXml, close, publish, download) {
        //var vm = this;
        var vm = $scope;
        vm.filePath = filePath;
        vm.fileName = fileName;
        vm.configXml = configXml;
        vm.title = title;
        vm.close = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: false,
            }, 500); // close, but give 500ms for bootstrap to animate
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
        vm.ngClickCopy = function () {
            vm.ngClickCopy;
            //$rootscope.ngClickCopy;
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
    function ($rootScope, $scope, $element, $http, $timeout, Upload, close, components, applications, environments) {
        var apiRelPath = "api:/ConfigApi";

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

        vm.availableApplications = [];
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
                url: 'api:/ComponentApi',
                params: {
                    componentName: component.name,
                },
            }).then(function (result) {
                vm.componentData = result.data;
                if (typeof vm.componentData.ConfigFile !== "undefined")
                    vm.fileName = vm.componentData.ConfigFiles.file_name;
                vm.filePath = vm.componentData.relative_path;
                vm.componentName = vm.componentComponents.name;
                vm.componentApplications = [];
                angular.forEach(result.data.Applications, function (Applications) {
                    var name = Applications.application_name;
                    var id = Applications.id;
                    var value = Applications.application_name;
                    vm.componentApplications.push({ id: id, name: name.replace('/', '').replace('"', '').replace("'", "").replace('[', '').replace(']', ''), value: value });
                });
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
            if (file) {
                if (!file.$error) {
                    var jsonApps = JSON.stringify(vm.componentApplications.map(function (item) { return item["name"].replace('/', '').replace('"', '').replace("'", ""); }));
                    Upload.upload({
                        url: 'api:/ConfigPublishApi',
                        params: {
                            componentName: vm.componentName,
                            environment: vm.componentEnvironment.name,
                            applications: jsonApps,
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
                    });
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


ConfigApp.controller('AddApplication',
    function ($rootScope, $scope, $element, $http, $timeout, close, components, applications, environments) {
        var apiRelPath = "api:/ApplicationApi";

        var vm = $scope;
        var applicationData;
        var applicationApplications;
        var filePath = filePath;
        var availableComponents = [];
        var applicationComponents = [];
        var applicationName;
        var localApplication;
        var appApplications;
        var id;
        var release;

        vm.availableComponents = [];
        vm.componentApplications = [];
        vm.applicationName = "";
        vm.release = "";
        vm.componentEnvironment = "";
        vm.fileName = "";
        vm.applications = applications;
        vm.components = components;
        vm.environments = environments;
        vm.localApplication = {
            //components: vm.components,
            //id: vm.id,
            release: vm.release,
            applicationName: vm.applicationName,
        }

        vm.selectApplication = function (application) {
            $http({
                method: 'GET',
                url: 'api:/ApplicationApi',
                //url: apiRelPath,
                params: {
                    applicationName: application.name,
                },
            }).then(function (result) {
                vm.applicationData = result.data;
                vm.applicationName = vm.applicationData.application_name;
                //vm.applicationName = result.application_name;
                vm.release = vm.applicationData.release;
                //vm.id = result.id;
                vm.applicationComponents = [];
                //angular.forEach(result.data.Applications, function (Applications) {
                //    var name = Application.application_name;
                //    var id = Application.id;
                //    var value = Application.application_name;
                //    vm.applicationComponents.push({ id: id, name: name.replace('/', '').replace('"', '').replace("'", "").replace('[', '').replace(']', ''), value: value });
                //});
                vm.localApplication = {
                    //components: vm.applicationComponents,
                    // id: vm.id,
                    id: '',
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
            $element.modal('hide');
            close({
                save: true,
                publish: true,
                //componentApplications: vm.componentApplications,
                applicationName: vm.applicationName,
                release: vm.release,
            }, 500);
        }
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                publish: false,
                //componentApplications: vm.componentApplications,
                applicationName: vm.applicationName,
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
    function ($rootScope, $scope, $element, close, componentName, parentElement, element, keyName, key, valueName, show, isNew, save) {
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