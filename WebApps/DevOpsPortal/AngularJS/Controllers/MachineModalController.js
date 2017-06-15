MachineApp.controller('MachineDetail', ['$rootScope', '$scope', '$element',
        'close', 'machineData',
    function ($rootScope, $scope, $element,
        close, machineData) {

        var vm = $scope;
        var save = false;
        var publish = false;
        var machineName;

        vm.machineData = machineData;
        vm.machineName = machineData.machine_name;

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