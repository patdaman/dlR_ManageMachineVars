module ManageAppConfig_Typescript.Model {
    export class Application {
        public id: number;
        public name: string;
        public release: string;
        public configVariable_id: string;
        public create_date: Date;
        public modify_date: Date;
        public active: Boolean
    }
}