//'use strict'

var ConfigApp = angular.module('ConfigApp',
        ['ui.grid',
            'ui.grid.edit',
            'ui.grid.grouping',
            'ui.grid.saveState',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.cellNav',
            'ui.grid.selection',
            'ui.grid.rowEdit',
            'ui.grid.resizeColumns',
            'ui.grid.pinning',
            'ui.grid.exporter',
            'ui.grid.moveColumns',
            'ui.grid.infiniteScroll',
            'ui.grid.importer',
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap'
        ]);

ConfigApp.controller('ConfigController', function ($rootScope, $scope, $http, $log, $timeout,
    uiGridConstants, $q, $interval, $templateCache, ModalService) {
    $scope.title = "Application Configuration";
    //var vm = $scope;
    var data = [];
    var i;
    var editMode = false;

    var rowIndex;
    var var_id;
    var keyName;
    var configXml;
    var filePath;
    var componentId;
    var componentName;

    $scope.environment = 'development';
    
    $scope.edit = false;
    $scope.canEdit = function () {
        return $scope.edit;
    };

    $scope.gridOptions = {
        enablePaging: true,
        paginationPageSizes: [10, 20, 50, 100],
        paginationPageSize: 25,
        enableHorizontalScrollbar: 0,

        enablePinning: true,
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,
        appScopeProvider: $scope,

        enableGridMenu: true,
        exporterMenuCsv: true,
        exporterMenuPdf: true,
        exporterCsvFilename: 'AppConfig.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [20, 10, 20, 20] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterPdfHeader: { text: "Marcom Central - Component Configuration", style: 'headerStyle' },
        exporterPdfFooter: function (currentPage, pageCount) {
            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        },
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
        },
        exporterPdfOrientation: 'landscape',
        exporterPdfPageSize: 'LETTER',
        exporterPdfMaxGridWidth: 500,
        exporterSuppressColumns: ['Action'],

        treeRowHeaderAlwaysVisible: false,

        enableCellSelection: true,
        enableCellEditOnFocus: true,
        enableRowSelection: false,

        expandableRowTemplate: 'Content/Templates/expandableRowTemplate.html',
        expandableRowHeight: 125,
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
    };

    $scope.gridOptions.columnDefs = [
        { field: 'applicationNames', enableCellEdit: false },
        { field: 'componentId', visible: false, enableCellEdit: false },
        { field: 'componentName', enableCellEdit: false, grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true },
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: false, enableCellEdit: false },
        { field: 'configElement', visible: false, enableCellEdit: false },
        { field: 'attribute', visible: false, enableCellEdit: false },
        {
            field: 'key',
            //cellTemplate: editableTemplate,
            cellEditableCondition: $scope.canEdit,
            width: "50%",
            cellToolTip: function (row, col) {
                if (grid.appScope.isBlank(row.entity.attribute)) {
                    return '<' + row.entity.configParentElement + '><br>&nbsp;<' + row.entity.key + '> {value} </' + row.entity.key + '><br>&nbsp;. . .<br></' + row.entity.configParentElement + '>';
                }
                else {
                    return '<' + row.entity.configParentElement + ' . . . /><br>&nbsp; <' + row.entity.configElement + '&nbsp;' + row.entity.attribute + '="{value}"&nbsp;' + row.entity.valueName + '="{value}" /><br>&nbsp;. . .<br></' + row.entity.configParentElement + ' . . .>" tooltip-placement="right" ';
                }
            }
        },
        { field: 'valueName', visible: false, enableCellEdit: 'false' },
        {
            name: "Actions",
            width: 150,
            cellTemplate: 'Content/Templates/actionsTemplate.html',
            enableCellEdit: false,
            visible: true,
            enableFiltering: false
        }
    ];

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    var editableTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';
    $scope.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    }

    $scope.editCell = function (row) {
        $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf(row);
        var rowId = row.entity.index;
        $scope.var_id = row.entity.configvar_id;
        $scope.keyName = row.entity.key;
        $scope.gridOptions.data[rowId].editable = true;
        $scope.edit = true;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
        $scope.scrollToFocus($scope.rowIndex, 6);
        $scope.scrollToFocus($scope.rowIndex, 7);
    };

    $scope.cancelEdit = function (rowEntity) {
        rowEntity.key = $scope.keyName;
        $scope.edit = false;
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) { return gridRow.entity; });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        var rowId = rowEntity.index;
        $scope.gridOptions.data[rowId].editable = false;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
    };

    //$templateCache.put('ui-grid/uiGridViewport',
    //    "<div class=\"ui-grid-viewport\" ng-style=\"colContainer.getViewportStyle()\"><div class=\"ui-grid-canvas\"><div ng-repeat=\"(rowRenderIndex, row) in rowContainer.renderedRows track by $index\" ng-if=\"grid.appScope.showRow(row.entity)\" class=\"ui-grid-row\" ng-style=\"Viewport.rowStyle(rowRenderIndex)\"><div ui-grid-row=\"row\" row-render-index=\"rowRenderIndex\"></div></div></div></div>"
    //);

    $scope.scrollToFocus = function (rowIndex, colIndex) {
        $scope.canEdit();
        $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[rowIndex], $scope.gridOptions.columnDefs[colIndex]);
    };

    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) { return gridRow.entity; });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        var rowId = rowEntity.index;
        $scope.gridOptions.data[rowId].editable = false;
    };

    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
    };

    $scope.saveSubGridRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigValuesApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // These need to come from Api:
    $scope.environments = ["development", "qa", "production"];
    $scope.machines = ["sdsvc01.dc.pti.com", "hqdev07.dev.corp.printable.com", "hqdev08.dev.corp.printable.com"];
    $scope.components = ["Commerce", "DAL", "ManagerI18N", "Services"];
    // End Todo

    $scope.showFile = function (row) {
        var componentRow = '$$' + row.uid;
        var rowEntity = row.entity;
        var componentAggObject = rowEntity['$$uiGrid-000B'];
        $http.get('/api/ConfigApi?componentName=' + componentAggObject.groupVal.value + '&environment=' + $scope.environment)
            .success(function (data) {
                ModalService.showModal({
                    templateUrl: "Content/Templates/configFileModal.html",
                    controller: "ConfigViewer",
                    inputs: {
                        title: data.componentName,
                        filePath: data.path,
                        configXml: data.text
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        //modal.close.then(function (result) {
                        //    $scope.complexResult = "Name: " + result.name + ", age: " + result.age;
                        //});
                    });
            }
    )};

    $scope.addVar = function (row) {
        ModalService.showModal({
            templateUrl: "",
            controller: "",
            inputs: {
                title: "",
                componentName: "",
            }
        })
    };

    $http.get('/api/ConfigApi')
        .success(function (data) {
            for (i = 0; i < data.length; i++) {
                data[i].subGridOptions = {
                    enableHorizontalScrollbar: 0,
                    appScopeProvider: $scope,
                    enableCellSelection: true,
                    enableCellEditOnFocus: true,
                    enableRowSelection: false,
                    columnDefs: [
                        { displayName: "id", field: "id", visible: false, resizable: true },
                        { displayName: "Variable id", field: "configvar_id", visible: false },
                        { displayName: "Environment", field: "environment", visible: true, enableCellEdit: false },
                        { displayName: "Value", field: "value", visible: true, cellEditableCondition: $scope.canEdit, width: "70%" },
                        { displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean' }
                        //{
                        //    name: "Actions",
                        //    cellTemplate: 'Content/Templates/actionsTemplate.html',
                        //    enableCellEdit: false
                        //},
                    ],
                    data: data[i].values,
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        gridApi.rowEdit.on.saveRow($scope, $scope.saveSubGridRow);
                    }
                };
            }
            $scope.gridOptions.data = data;
            angular.forEach(data, function (data, index) {
                data["index"] = index + 1;
            });
        });
});

ConfigApp.controller('ConfigViewer',
    function ($scope, $element, title, filePath, configXml, close) {

        $scope.filePath = filePath;
        $scope.configXml = configXml;
        $scope.title = title;
        $scope.close = function () {
            close({
                filePath: $scope.filePath,
                configXml: $scope.configXml
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        $scope.cancel = function () {
            $element.modal('hide');
            close({
                filePath: $scope.filePath,
                configXml: $scope.configXml
            }, 500); // close, but give 500ms for bootstrap to animate
        };
    });

