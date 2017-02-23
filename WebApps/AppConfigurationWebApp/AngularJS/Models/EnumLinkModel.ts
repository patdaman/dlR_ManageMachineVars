module ManageAppConfig_Typescript.Model {
    export class EnumLinkModel {
        public EnumName: string;
        public DBName: string;
        public TBLName: string;
        public EnumList: Model.EnumListItemModel[];
        public EnumMap: any;
    }
}