'use strict'

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
    uiGridConstants, $q, $interval, ModalService) {
    $scope.title = "Application Configuration";
    //var vm = $scope;
    var data = [];
    var i;
    var editMode = false;

    var rowIndex;
    var configXml;
    var filePath;

    $scope.gridOptions = {
        enablePaging: true,
        paginationPageSizes: [10, 20, 50, 100],
        paginationPageSize: 25,

        enablePinning: true,
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableHorizontalScrollbar: 0,

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

        enableSelectAll: false,
        //enableEditing: false,
        //enableCellSelection: false,
        enableRowSelection: true,

        expandableRowTemplate: '<div ui-grid="row.entity.subGridOptions" ui-grid-edit ui-grid-row-edit ui-grid-selection style="width:100%; float:right"></div>',
        //expandableRowTemplate: '<div style="padding:5px;"><div ui-grid="row.entity.subGridOptions[0]" ui-grid-edit  ui-grid-row-edit ui-grid-selection style="display:inline-block;"></div><div ui-grid="row.entity.subGridOptions[1]" ui-grid-edit  ui-grid-row-edit ui-grid-selection style="height:340px;width:48%;display:inline-block;margin-left:5px"></div></div>',
        expandableRowHeight: 125,
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
    };

    $scope.gridOptions.columnDefs = [
        { field: 'applicationNames', enableCellEdit: false, cellTemplate: basicCellTemplate },
        { field: 'componentId', visible: false, enableCellEdit: false },
        { field: 'componentName', enableCellEdit: false, grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'configElement', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'attribute', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        {
            field: 'key',
            cellTemplate: 'Content/Templates/keyCellTemplate.html',
            //cellTemplate: '<div class="grid-tooltip"><div class="ui-grid-cell-contents"><div ng-class="{\'viewr-dirty\' : row.inlineEdit.entity[col.field].isValueChanged }">{{row.entity[col.field]}}</div></div></div>',
            //cellToolTip: function (row, col) {
                //if (grid.appScope.isBlank(row.entity.attribute)) {
                //    return '<' + row.entity.configParentElement + '>\n\t<' + row.entity.key + '> {value} </' + row.entity.key + '>\n\t. . .\n</' + row.entity.configParentElement + '>';
                //}
                //else {
                //    return '<' + row.entity.configParentElement + ' . . . /> \n\t <' + row.entity.configElement + ' ' + row.entity.attribute + '="{value}" ' + row.entity.valueName + '="{value}" />\n\t. . .\n</' + row.entity.configParentElement + ' . . .>" tooltip-placement="right" ';
                //}
            //}
        },
        { field: 'valueName', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        //{
        //    field: "Action",
        //    width: 150,
        //    enableCellEdit: false,
        //    enableFiltering: false,
        //    cellTemplate: '<div class="inline-block"><div ng-if="!row.groupHeader">' +
        //    '<button id="editBtn" ng-if="!row.inlineEdit.isEditModeOn" type= "button" class="btn btn-xs btn-info"  ng-click="toggleEdit()" >Edit </button>' +
        //    '<button id="editBtn" ng-if="!row.inlineEdit.isEditModeOn" type= "button" class="btn btn-xs btn-waring"  ng-click="publish()" >Publish </button>' +
        //    '&nbsp <button id="delBtn" ng-if="row.inlineEdit.isEditModeOn" type="button" class="btn btn-xs btn-danger"  ng-click="saveRow()">Save </button>' +
        //    '&nbsp <button id="delBtn" ng-if="row.inlineEdit.isEditModeOn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()">Remove </button>' + 
        //    '</div></div>'
        //    //cellTemplate: '<div><div ng-if="!row.groupHeader"><button id="pubBtn" type="button" class="btn btn-xs btn-primary"  ng-click="publishCell()" >Publish </button></div><div ng-if="!row.groupHeader"><button id="delBtn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()" >Remove </button></div></div>'
        //}
        {
            name: "Actions",
            cellTemplate: '<div ng-if="!row.groupHeader"><div class="ui-grid-cell-contents">' +
            '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
            '<button value="Edit" class="btn btn-xs btn-danger" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Delete</button>' +
            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
            '</div></div>' +
            '<div ng-if="row.groupHeader"><div class="ui-grid-cell-contents"><button class="btn btn-xs btn-default" ng-click="grid.appScope.showFile(row.entity)">View File</button></div>',
            enableCellEdit: false
        },
    ];

    //var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ng-click="editCell(row.entity, row.getProperty(col.field), col.field)"><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';
    var basicCellTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';
    //var basicCellTemplate = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div>';

    //var editableCellTemplate = '~/Content/Templates/editableRowTemplate.html';

    function isBlank(str) {
        return (!str || /^\s*$/.test(str));
    }

    $scope.selectedCell;
    $scope.selectedRow;
    $scope.selectedColumn;

    $scope.editCell = function (row, cell, column) {
        $scope.selectedCell = cell;
        $scope.selectedRow = row;
        $scope.selectedColumn = column;
    };

    $scope.updateCell = function () {
        //   alert("checking");  
        $scope.selectedRow[$scope.selectedColumn] = $scope.selectedCell;
    };

    $scope.editable = function (row) {
        $scope.editMode = !editMode;
    };

    $scope.changeGroupBy = function (group1, group2) {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.$gridScope.configGroups.push(group1);
        $scope.gridOptions.$gridScope.configGroups.push(group2);
        $scope.gridOptions.groupBy();
    };

    $scope.clearGroupBy = function () {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.groupBy();
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            var msg = 'row selected ' + row.isSelected;
            $log.log(msg);
        });

        gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            var msg = 'rows changed ' + rows.length;
            $log.log(msg);
        });

        gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
            var selectedRows = $scope.gridApi.selection.getSelectedRows();

            if (newValue !== oldValue) {

                rowEntity.state = "Changed";
                //Get column
                var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;

                angular.forEach(selectedRows, function (item) {
                    item[rowCol] = rowEntity[rowCol];
                    item.state = "Changed";
                    item.isDirty = false;
                    item.isError = false;
                });

            }
        });

        gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
    };

    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);

    };

    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    $scope.toggleEdit = function (rowNum) {
        $scope.gridOptions1.data[rowNum].editable = !$scope.gridOptions1.data[rowNum].editable;
        $scope.grid1Api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
    };

    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);

    };

    $scope.saveSubGridRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigValues/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // These need to come from Api:
    $scope.environments = ["development", "qa", "production"];
    $scope.machines = ["sdsvc01.dc.pti.com", "hqdev07.dev.corp.printable.com", "hqdev08.dev.corp.printable.com"];
    $scope.components = ["Commerce", "DAL", "ManagerI18N", "Services"];
    // End Todo

    $scope.expandAllRows = function () {
        $scope.gridApi.expandable.expandAllRows();
    };

    $scope.collapseAllRows = function () {
        $scope.gridApi.expandable.collapseAllRows();
    };

    $scope.showFile = function (rowEntity) {
        var configFile = '';
        ModalService.showModal({
            templateUrl: "Content/Templates/configFileModal.html",
            controller: "ConfigViewer",
            inputs: {
                //title: "A More Complex Example",
                title: rowEntity.componentName,
                //title: configFile.fileName,
                filePath: "file Path",
                //filePath: configFile.filePath,
                configXml: "config XML"
                //configXml: configFile.fileText
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                $scope.complexResult = "Name: " + result.name + ", age: " + result.age;
            });
        });

    };



    //$http({
    //    method: "GET",
    //    url: '/api/ConfigApi'
    //}).then(function (data) {

    $http.get('/api/ConfigApi')
        .success(function (data) {
            for (i = 0; i < data.length; i++) {
                data[i].subGridOptions = {
                    enableHorizontalScrollbar: 0,
                    appScopeProvider: $scope,
                    columnDefs: [
                        { name: "id", field: "id", visible: false },
                        { name: "Variable id", field: "configvar_id", visible: false },
                        { name: "Environment", field: "environment", visible: true, enableCellEdit: false, cellTemplate: basicCellTemplate },
                        //{ name: "Value", field: "value", visible: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
                        { name: "Value", field: "value", visible: true, cellEditableContition: false, cellTemplate: basicCellTemplate },
                        { name: "Create Date", field: "create_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { name: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { name: "Last Publish Date", field: "publish_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { name: "Is Published", field: "published", visible: true, enableCellEdit: false, type: 'boolean' },
                        {
                            name: "Actions",
                            cellTemplate: '<div class="ui-grid-cell-contents" >' +
                            '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                            '<button value="Edit" class="btn btn-xs btn-warning" ng-if="!row.inlineEdit.isEditModeOn" ng-click="grid.appScope.publishValue(row.entity)">Publish</button>' +
                            //'<button value="Edit" class="btn btn-xs btn-warning" ng-if="!row.inlineEdit.isEditModeOn" ng-click="appScopeProvider.publishValue(row.entity)">Publish</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                            '</div>',
                            enableCellEdit: false
                        },
                    ],
                    data: data[i].values,
                    onRegisterApi: function (gridApi) {
                        //set gridApi on scope
                        $scope.gridApi = gridApi;

                        //gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                        //    var selectedRows = $scope.gridApi.selection.getSelectedRows();
                        //    var parentRow = rowEntity.grid.appScope.row;
                        //    var index = $scope.subGridOptions.data.indexOf(rowEntity.entity);
                        //    if (newValue != oldValue) {

                        //        rowEntity.state = "Changed";
                        //        //Get column
                        //        var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;

                        //        angular.forEach(selectedRows, function (item) {
                        //            item[rowCol] = rowEntity[rowCol];
                        //            item.state = "Changed";
                        //            item.isDirty = false;
                        //            item.isError = false;
                        //        });
                        //        $scope.subGridOptions.data.splice(index, 1);

                        //        $scope.subGridOptions.data.push({
                        //            "id": rowEntity.id,
                        //            "configvar_id": rowEntity.configvar_id,
                        //            "environment": rowEntity.environment,
                        //            "value": rowEntity.value,
                        //            "create_date": rowEntity.create_date,
                        //            "modify_date": rowEntity.modify_date,
                        //            "publish_date": rowEntity.publish_date,
                        //            "published": rowEntity.published,
                        //        });
                        //    }
                        //});
                        gridApi.rowEdit.on.saveRow($scope, $scope.saveSubGridRow);
                    }
                };
            }
            $scope.gridOptions.data = data;
        });
}
);

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


ConfigApp.factory('configModal', function ($compile, $rootScope) {
    return function () {
        var elm;
        var modal = {
            open: function (entity) {
                //var title = entity.title;
                var title = 'Component.config';
                //var filePath = entity.filePath;
                var filePath = 'entity.filePath';
                //var configXml = entity.configXml;
                var configXml = 'entity.configXml';
                var html = '<div class="modal" ng-style="modalStyle">{{modalStyle}}<div class="modal-dialog modal-lg"><div class="modal-content"><div class="modal-header"><button type="button" class="close" ng-click="close()" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title">{{title}}</h4></div><div class="modal-body"><p>{{filePath}}</p><div class="form-group"><textarea id="code" name="code">{{configXml}}</textarea></div></div><div class="modal-footer"><button type="button" ng-click="close()" class="btn btn-primary" data-dismiss="modal">Save</button><button id="buttonClose" ng-click="close()" class="btn">Close</button></div></div></div></div>';
                elm = angular.element(html);
                angular.element(document.body).prepend(elm);

                $rootScope.close = function () {
                    modal.close();
                };

                $rootScope.modalStyle = { "display": "block" };

                $compile(elm)($rootScope);
            },
            close: function () {
                if (elm) {
                    elm.remove();
                }
            }
        };
        return modal;
    };
});
