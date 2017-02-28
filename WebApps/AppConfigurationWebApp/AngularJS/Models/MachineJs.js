factory('Machine', function (Organisation) {

    /**
     * Constructor, with class name
     */
    function Machine(id, machine_name, location, usage, create_date, modify_date, active) {
        // Public properties, assigned to the instance ('this')
        this.id = id;
        this.machine_name = machine_name;
        this.location = location;
        this.usage = usage;
        this.create_date = create_date;
        this.modify_date = modify_date;
        this.active = active;
        //this.MachineGroup = MachineGroup;
    }

    ///**
    // * Private property
    // */
    //var possibleusages = ['development', 'production', 'qa'];

    ///**
    // * Private function
    // */
    //function checkusage(usage) {
    //    return possibleusages.indexOf(usage) !== -1;
    //}

    ///**
    // * Static property
    // * Using copy to prevent modifications to private property
    // */
    //Machine.possibleusages = angular.copy(possibleusages);

    /**
     * Static method, assigned to class
     * Instance ('this') is not available in static context
     */
    Machine.build = function (data) {
        //if (!checkusage(data.usage)) {
        //    return;
        //}
        return new Machine(
          data.id,
          data.machine_name,
          data.location,
          data.usage,
          data.create_date,
          data.modify_date,
          data.active
          //Organisation.build(data.MachineGroup) // another model
        );
    };

    /**
     * Return the constructor function
     */
    return Machine;
})