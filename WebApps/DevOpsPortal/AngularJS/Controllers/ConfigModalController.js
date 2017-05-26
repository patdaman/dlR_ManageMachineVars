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
ConfigApp.controller('ConfigViewer', ['$rootScope', '$scope', '$element', '$http', '$q',
        'component', 'files', 'environments', 'environment', 'Admin', 'close',
    function ($rootScope, $scope, $element, $http, $q,
        component, files, environments, environment, Admin, close) {

        //var vm = this;
        var vm = $scope;
        var selectedVmEnvironment;
        var selectedFile;
        var filePath;
        var fileName;
        var configXml;
        var modalSize;
        var displayFileSelect;
        var displayGetFile;
        var publish;
        var download;

        vm.Admin = Admin;
        vm.displayGetFile = false;
        vm.environments = environments;
        vm.environment = environment;
        if (vm.environment)
            angular.forEach(vm.environments, function (value) {
                if (value.name === vm.environment)
                    vm.selectedVmEnvironment = value;
            });
        vm.files = files;
        if (vm.files.length < 2) {
            vm.displayFileSelect = false;
            vm.filePath = files[0].path;
            vm.fileName = files[0].fileName;
        }
        else {
            vm.displayFileSelect = true;
            vm.filePath = '';
            vm.fileName = '';
        }
        vm.configXml = '';
        vm.component = component;
        vm.modalSize = "modal-dialog modal-lg";
        vm.download = false;
        vm.publish = false;

        vm.updateFile = function (selectedFile) {
            vm.fileName = selectedFile.fileName;
            vm.displayGetFile = true;
        };
        vm.updateEnvironment = function (selectedVmEnvironment) {
            vm.environment = selectedVmEnvironment.value;
            if (vm.environment && vm.environment != '')
                vm.displayGetFile = true;
        };

        vm.modalSize = function () {
            if (vm.selectedVmEnvironment.name != '' && vm.selectedFile.fileName != '')
                modalSize = "modal-dialog modal-lg";
        };

        vm.getFile = function () {
            if ((!vm.component || !vm.environment || !vm.selectedFile) &&
                (vm.component !== '' || vm.environment !== '' || vm.selectedFile !== '')) {
                swal({
                    title: "Data Missing",
                    text: "Null Value(s)",
                    type: "error",
                    configButtonText: "OK"
                });
            }
            else {
                var def = $q.defer();
                $http({
                    method: 'GET',
                    url: 'api:/ConfigApi',
                    params: {
                        componentName: vm.component,
                        environment: vm.environment,
                        fileName: vm.selectedFile.fileName,
                    }
                })
                .success(def.resolve)
                .success(function (data) {
                    vm.configXml = data.text,
                    vm.filePath = data.path,
                    vm.displayGetFile = false,
                    vm.modalSize()
                })
            }
        };

        if (vm.files.length === 1) {
            vm.selectedFile = vm.files[0];
            if (vm.environment && vm.environment != '') {
                vm.displayFileSelect = false;
                vm.getFile();
            }
        };

        vm.close = function () {
            $element.modal('hide');
            close({
                publish: vm.publish,
                download: vm.download,
                fileName: vm.selectedFile.fileName,
                environment: vm.environment,
                component: vm.component
            }, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.publish = function () {
            vm.publish = true;
            vm.close();
        };
        vm.download = function () {
            vm.download = true;
            vm.close();
        };
    }]);

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
ConfigApp.controller('AddComponent', ['$rootScope', '$scope', '$element', '$http', '$timeout', 'Upload', 'close', 'components', 'applications', 'environments', 'environment',
    function ($rootScope, $scope, $element, $http, $timeout,
        Upload, close, components, applications, environments, environment) {

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
        var isNew;
        var uploaded;
        var isGlobal;
        var localComponent;
        var publish;

        vm.availableApplications = applications;
        vm.componentApplications = [];
        vm.componentName = "";
        vm.componentEnvironment = environment;
        vm.fileName = "";
        vm.applications = applications;
        vm.components = components;
        vm.environments = environments;
        vm.environment = environment;
        vm.isNew = true;
        vm.uploaded = false;
        vm.isGlobal = false;
        vm.publish = false;

        vm.selectEnvironment = function (environment) {
            vm.componentEnvironment = environment;
            var test = vm.componentComponents;
        };

        vm.selectComponent = function (component) {
            if (component) {
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
                    vm.applicationNames = vm.componentApplications.map(function (item) {
                        return item['name'].replace(/"/g, '').replace(/'/g, '')
                            .replace(/\[/g, '').replace(/]/g, '');
                    });
                    for (var i = vm.availableApplications.length - 1; i >= 0; i--) {
                        for (var j = 0; j < vm.componentApplications.length; j++) {
                            if (vm.availableApplications[i] && (vm.availableApplications[i].name === vm.componentApplications[j].name)) {
                                vm.availableApplications.splice(i, 1);
                            }
                        }
                    }
                    vm.isNew = false;
                });
            }
            else {
                vm.componentName = '';
                vm.componentApplications = [];
                vm.filePath = '';
                vm.availableApplications = vm.applications;
                vm.isNew = true;
            };
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
                    if (vm.componentName == '') {
                        swal({
                            title: "Add Component",
                            text: "No Component Name Supplied",
                            type: "error",
                            confirmButtonText: "Try Again"
                        });
                    }
                    else if (!vm.componentEnvironment) {
                        swal({
                            title: "Add Component",
                            text: "No Environment Supplied",
                            type: "error",
                            confirmButtonText: "D'oh!"
                        });
                    }
                    else {
                        swal({
                            title: "Are you sure?",
                            text: "Existing Values will be overwritten in this environment",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#AEDEF4",
                            confirmButtonText: "Yes, upload it!",
                            closeOnConfirm: false
                        },
                        function (isConfirm) {
                            if (isConfirm) {
                                vm.applicationNames = vm.componentApplications.map(function (item) {
                                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                                        .replace(/\[/g, '').replace(/]/g, '');
                                });
                                var jsonApps = JSON.stringify(vm.componentApplications.map(function (item) {
                                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                                        .replace(/\[/g, '').replace(/]/g, '');
                                }));
                                Upload.upload({
                                    url: 'api:/ConfigPublishApi',
                                    params: {
                                        componentName: vm.componentName,
                                        environment: vm.componentEnvironment.name,
                                        applications: vm.applicationNames,
                                        userName: $rootScope.UserName,
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
                                .then(function () {
                                    swal({
                                        title: vm.componentName,
                                        text: "Config File Added",
                                        type: "success",
                                        imageUrl: "/Assets/thumbs-up.jpg",
                                        confirmButtonText: "Cool"
                                    });
                                    vm.uploaded = true;
                                });
                            }
                            else {
                                swal({
                                    title: "Add Component File",
                                    text: "No File Uploaded",
                                    type: "warning",
                                    confirmButtonText: "Cool"
                                });
                            };
                        });
                    };
                };
            };
        };
        vm.close = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.publish = function () {
            if (!vm.componentName) {
                swal({
                    title: "Publish Component",
                    text: "No Component Selected",
                    type: "error",
                    confirmButtonText: "D'oh!"
                });
            }
            else if (!vm.componentEnvironment) {
                swal({
                    title: "Publish Component",
                    text: "No Environment Selected",
                    type: "error",
                    confirmButtonText: "D'oh!"
                });
            }
            else {
                var applicationNames = [];
                var applicationNameString;
                vm.applicationNames = vm.componentApplications.map(function (item) {
                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                        .replace(/\[/g, '').replace(/]/g, '');
                });
                var applicationString = vm.applicationNames.join(',');
                swal({
                    title: "Are you sure?",
                    text: "Existing Component Files for applications:\n" + applicationString + "\nwill be overwritten in " + $vm.componentEnvironment,
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#AEDEF4",
                    confirmButtonText: "Yes, publish them!",
                    closeOnConfirm: false
                },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.publish = true;
                        vm.save();
                    }

                })
            }
        };
        vm.save = function () {
            vm.applicationNames = vm.componentApplications.map(function (item) {
                return item['name'].replace(/"/g, '').replace(/'/g, '')
                    .replace(/\[/g, '').replace(/]/g, '');
            });
            var applicationString = vm.applicationNames.join(',');
            $element.modal('hide');
            close({
                save: true,
                publish: vm.publish,
                isNew: vm.isNew,
                uploaded: vm.uploaded,
                isGlobal: vm.isGlobal,
                applicationNames: applicationString,
                componentApplications: vm.componentApplications,
                componentName: vm.componentName,
                environment: vm.componentEnvironment,
                filePath: vm.filePath,
            }, 500);
        }
    }]);

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
ConfigApp.controller('AddApplication', ['$rootScope', '$scope', '$element', '$http', '$timeout', 'close', 'components', 'applications', 'environments',
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
        var isNew;

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
        vm.isNew = true;
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
                vm.componentNames = vm.applicationComponents.map(function (item) {
                    return item['name'].replace(/"/g, '').replace(/'/g, '')
                        .replace(/\[/g, '').replace(/]/g, '');
                });
                for (var i = vm.availableComponents.length - 1; i >= 0; i--) {
                    for (var j = 0; j < vm.applicationComponents.length; j++) {
                        if (vm.availableComponents[i] && (vm.availableComponents[i].name === vm.applicationComponents[j].name)) {
                            vm.availableComponents.splice(i, 1);
                        }
                    }
                };
                vm.localApplication = {
                    componentData: vm.applicationComponents,
                    componentNames: vm.componentNames,
                    id: vm.applicationData.id,
                    release: vm.release,
                    applicationName: vm.applicationName,
                };
                vm.isNew = false;
            });
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
            vm.componentNames = vm.applicationComponents.map(function (item) {
                return item['name'].replace(/"/g, '').replace(/'/g, '')
                    .replace(/\[/g, '').replace(/]/g, '');
            });
            $element.modal('hide');
            close({
                save: true,
                isNew: vm.isNew,
                applicationComponents: vm.applicationComponents,
                componentNames: vm.componentNames,
                applicationName: vm.applicationName,
                id: vm.localApplication.id,
                release: vm.release,
            }, 500);
        }
    }]);

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
ConfigApp.controller('AddVar', ['$rootScope', '$scope', '$element',
        'close', 'componentName', 'parentRows', 'parentElement', 'element',
        'attribute', 'key', 'valueName', 'show', 'isNew', 'files',
    function ($rootScope, $scope, $element,
        close, componentName, parentRows, parentElement, element,
        attribute, key, valueName, show, isNew, files) {

        var vm = $scope;
        var noAttributeKey = key;
        var selectedFile;
        var fileName;

        vm.files = files;
        vm.componentName = componentName;
        vm.element = element;
        vm.parentRows = parentRows;
        vm.parentElement = parentElement;
        vm.attribute = attribute;
        vm.key = key;
        vm.valueName = valueName;
        vm.show = show;
        vm.isNew = isNew;
        vm.fileName = '';

        if (vm.files.length === 1) {
            vm.selectedFile = vm.files[0];
        };

        vm.updateFile = function (selectedFile) {
            vm.fileName = selectedFile.fileName;
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
            if (vm.valueName === '') {
                vm.element = vm.key;
                vm.attribute = '';
            }
            $element.modal('hide');
            close({
                save: true,
                componentName: vm.componentName,
                element: vm.element,
                parentElement: vm.parentElement,
                attribute: vm.attribute,
                key: vm.key,
                valueName: vm.valueName,
                fileName: vm.selectedFile.fileName,
            }, 500);
        };
    }]);

///-------------------------------------------------------------------------------------------------
/// <summary>   Note Modal Controller. </summary>
///
/// <remarks>   Pdelosreyes, 5/15/2017. </remarks>
///
/// <param name="'note'">           The 'note'. </param>
/// <param name="($rootScope">      The $root scope. </param>
/// <param name="$scope">           The $scope. </param>
/// <param name="$element">         The $element. </param>
/// <param name="close">            The close. </param>
/// <param name="componentName">    Name of the component. </param>
/// <param name="key">              The key. </param>
/// <param name="fullElement">      The full element. </param>
/// <param name="configVarId">      Identifier for the configuration variable. </param>
/// <param name="noteText">         The note text. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('noteViewer', ['$rootScope', '$scope', '$element', 'close',
        'componentName', 'key', 'fullElement', 'configVarId', 'createDate',
        'lastModifiedUser', 'lastModifiedDate', 'noteText',
    function ($rootScope, $scope, $element, close,
        componentName, key, fullElement, configVarId, createDate,
        lastModifiedUser, lastModifiedDate, noteText) {

        var vm = $scope;
        var save;
        var modalSize;

        vm.componentName = componentName;
        vm.key = key;
        vm.fullElement = fullElement;
        vm.configVarId = configVarId;
        vm.noteText = noteText;
        vm.lastModifiedDate = lastModifiedDate;
        vm.lastModifiedUser = lastModifiedUser;
        vm.modalSize = "modal-dialog modal-md";

        vm.close = function () {
            $element.modal('hide');
            close({}, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                configVarId: vm.configVarId,
                noteText: vm.noteText,
            }, 500);
        };
    }]);