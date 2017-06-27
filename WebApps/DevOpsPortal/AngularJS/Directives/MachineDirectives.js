MachineApp.filter('griddropdown', function () {
    return function (input, context) {
        if (context.grid.cellNav.focusedCells[0] && context.entity) {
            var fieldLevel = (context.editDropdownOptionsArray == undefined) ? context.grid.cellNav.focusedCells[0].col.colDef : context;
            var map = fieldLevel.editDropdownOptionsArray;
            var idField = fieldLevel.editDropdownIdLabel;
            var valueField = fieldLevel.editDropdownValueLabel;
            var initial = context.entity[context.grid.cellNav.focusedCells[0].col.field];
            if (typeof map !== "undefined") {
                for (var i = 0; i < map.length; i++) {
                    if (map[i][idField] == input) {
                        return map[i][valueField];
                    }
                }
            }
            else if (initial) {
                return initial;
            }
        };
        return input;
    }
})
;