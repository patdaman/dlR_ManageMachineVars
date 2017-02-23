module ManageAppConfig_Typescript.Model {
    export class EnvironmentVariable {
        public id: number;
        public key: string;
        public value: string;
        public type: string;
        public path: string;
        public create_date: Date;
        public modify_date: Date;
        public active: Boolean
    }
}