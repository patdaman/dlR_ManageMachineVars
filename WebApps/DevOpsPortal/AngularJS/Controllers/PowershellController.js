'use strict'

PowershellApp.value('ui.config', {
    codeMirror: {
        mode: 'powershell',
        lineNumbers: true,
        theme: 'midnight',
        tabMode: 'shift',
        readOnly: 'nocursor',
        lineWrapping: false,
        matchBrackets: true,
    }
});

PowershellApp.controller('PowershellController', function ($rootScope, $scope, $http, $log, $timeout, $q, $interval, $attrs, ModalService) {
    $scope.title = "Script Execution ";

    var vm = $scope;

    var apiRelPath = "api:/PowershellApi";
    var scripts;
    var scriptName;
    var scriptText;

    var theme;
    var codeMirror;
    var machine_name;
    var location;
    var usage;
    var create_date;
    var modify_date;
    var active;

    var environments = [];
    var components = [];
    var applications = [];
    var machines = [];
    var themes = [];

    var componentId;
    var componentName;

    var selectedScript;
    var script;
    var selectedMachine;
    var machine;
    var selectedComponent;
    var component;
    var selectedApplication;
    var application;
    var selectedEnvironment;
    var environment;

    $scope.scripts = [];
    $scope.scriptName = '';
    $scope.scriptText = 'Code goes here';

    $scope.themes = ['midnight', 'eclipse', 'abcdef', 'rubyblue', 'solarized'];
    $scope.theme = $scope.themes[0];

    //$scope.readOnly = 'nocursor';
    $scope.readOnly = false;
    $scope.mode = 'powershell';

    // The ui-codemirror option
    $scope.cmOptions = {
        lineNumbers: true,
        indentWithTabs: true,
        theme: 'midnight',
        readOnly: $scope.readOnly,
        lineWrapping: false,
        tabMode: 'shift',
        matchBrackets: true,
        mode: $scope.mode,
        onLoad: function (_cm) {
            $scope.updateTheme = function () {
                _cm.setOption("theme", $scope.theme.toLowerCase());
                //_cm.setOption("lineNumbers", true);
                //_cm.setOption("indentWithTabs", true);
                _cm.setOption("readOnly", $scope.readOnly);
                //_cm.setOption("lineWrapping", false);
                //_cm.setOption("tabMode", "shift");
                //_cm.setOption("matchBrackets", true);
                _cm.setOption("mode", $scope.mode);
            };
        }
    };

    /// Display current API path and link to Help page
    $scope.ApiBaseUrl = ApiPath;
    $scope.ApiBaseUrlHelp = $scope.ApiBaseUrl.slice(0, -4) + '/Help';

    /// Grid Filters
    //$scope.environment = 'development';
    $scope.environment = 'all';
    $scope.filterEnvironment = function () {
        return $scope.environment;
    };
    $scope.application = '';
    $scope.component = '';
    $scope.dateTimeString = function () {
        date: 'yyyy-MM-dd_HH:mm(Z)'
        return date;
    };

    // List of scripts:
    $http({
        method: 'GET',
        url: 'api:/PowershellApi',
        //withCredentials: true,
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.scripts = result.data;
    });

    // List of environments:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "environment"
            //parameters: "type=environment"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.environments = result.data;
    });

    // List of components:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "component"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.components = result.data;
    });

    // List of applications:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "application"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.applications = result.data;
    });

    // List of applications:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "machine"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.machines = result.data;
    });

    // Function call from Index page dropdown OnChange
    $scope.updateEnvironment = function () {
        $scope.environment = $scope.selectedEnvironment.value;
    };
    // Function call from Index page dropdown OnChange
    $scope.updateComponent = function () {
        $scope.component = $scope.selectedComponent;
    };
    // Function call from Index page dropdown OnChange
    $scope.updateApplication = function () {
        $scope.application = $scope.selectedApplication;
    };

    // Function call from Index page dropdown OnChange
    $scope.updateMachine = function () {
        $scope.machine = $scope.selectedMachine;
    };

    // Function call from Index page dropdown OnChange
    $scope.updateScript = function () {
        $scope.script = $scope.selectedScript;
        $scope.scriptName = $scope.selectedScript.ScriptName;
        $scope.scriptText = $scope.selectedScript.ScriptText;
    };

    // Function call from Index page dropdown OnChange
    $scope.updateTheme = function () {
        $scope.theme = $scope.selectedScript;
        $scope.scriptName = $scope.selectedScript.ScriptName;
        $scope.scriptText = $scope.selectedScript.ScriptText;
    };

    $scope.editScript = function () {

    };
    $scope.addScript = function () {

    };
    $scope.updateScript = function () {

    };
    $scope.executeScript = function (scriptText) {
        var machineName = $scope.selectedMachine.name;
        var deferred = $q.defer();
        var data = JSON.stringify({
            "scriptText": scriptText,
        });
        $http({
            method: 'POST',
            url: 'api:/PowershellApi',
            //withCredentials: true,
            params: {
                machineName: machineName,
                //machineName: 'HAL9000',
                scriptText: scriptText
            },
            //data: data,
            headers: {
                'Content-Type': 'application/json'
            },
            transformResponse: function (data) {return {list: angular.fromJson(data)} }
        })
        .success(function (data) {
            ModalService.showModal({
                templateUrl: "/Content/Templates/powershellExecuteModal.html",
                controller: "ScriptResults",
                inputs: {
                    machineName: machineName,
                    //executionLogs: data,
                    executionLogs: data.list,
                }
            })
                .then(function (modal) {
                    modal.element.modal();
                    modal.close.then(function (result) {
                        if (result.download) {
                            $scope.downloadConfig(result.title, result.fileName)
                        };
                        if (result.publish) {
                            $scope.downloadConfig(result.title, result.fileName)
                        };
                    });
                })
        });
    }
});

PowershellApp.controller('ScriptResults',
    function ($rootScope, $scope, $element, machineName, executionLogs) {
        //var vm = this;
        var vm = $scope;
        var title;
        vm.machineName = machineName;
        vm.executionLogs = executionLogs;
        vm.title = machineName;
        vm.cancel = function () {
            $element.modal('hide');
            close({
            }, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
            }, 500);
        }
        vm.ngClickCopy = function () {
            vm.ngClickCopy;
            //$rootscope.ngClickCopy;
        }
    });