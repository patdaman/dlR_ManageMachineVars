module AppConfigurationWebApp.Model {
    export class MachineAppVar {
        public machineId: number;
        public machine_name: string;
        public location: string;
        public usage: string;
        public machineCreate_date: Date;
        public machineModify_date: Date;
        public machineActive: string;
        public applicationId: number;
        public applicationName: string;
        public applicationRelease: string;
        public componentId: number;
        public componentName: string;
        public varId: number;
        public varType: string;
        public configParentElement: string;
        public configElement: string;
        public configAttribute: string;
        public keyName: string;
        public key: string;
        public configValue_name: string;
        public valueName: string;
        public value: string;
        public varPath: string;
        public varActive: Boolean;
        public envType: string;
        public varCreate_date: Date;
        public varModify_date: Date;
    }
}