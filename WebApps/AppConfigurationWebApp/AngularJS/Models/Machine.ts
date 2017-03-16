module AppConfigurationWebApp.Model {
    export class Machine {
        public id: number;
        public machine_name: string;
        public location: string;
        public usage: string;
        public create_date: Date;
        public modify_date: Date;
        public active: Boolean
    }
}