module AppConfigurationWebApp.Model {
    export class ConfigVariable {
        public id: number;
        public element: string;
        public attribute: string;
        public key: string;
        public value_name: string;
        public value: string;
        public config_path: string;
        public create_date: Date;
        public modify_date: Date;
        public active: Boolean
    }
}