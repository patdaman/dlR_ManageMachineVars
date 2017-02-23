///-------------------------------------------------------------------------------------------------
///  <summary>enum list service class</summary>
///
/// <remarks>   Pdelosreyes, 2/15/2017. </remarks>
///-------------------------------------------------------------------------------------------------
module ManageAppConfig_Typescript.Service {
    export class EnumListService {
        private httpService: ng.IHttpService;
        private qService: ng.IQService;

        private EnumLinker: Model.EnumLinkModel[];
        public NumEnums: number;
        public EnumServiceReady: boolean;
        private gotEnumsLists: number;

        private EnumList: Model.EnumListItemModel[]; //
        public EnumMap: any;//

        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.qService = $q;
            this.EnumServiceReady = false;
            this.EnumList = null; //

            this.EnumLinker = new Array<Model.EnumLinkModel>();
            this.EnumLinker.push({
                EnumName: "Usage", EnumList: null, EnumMap: null, DBName: "DevOps", TBLName: "enum_MachineUsageType"
            });
            this.EnumLinker.push({
                EnumName: "Locations", EnumList: null, EnumMap: null, DBName: "DevOps", TBLName: "enum_Locations"
            });
            this.EnumLinker.push({
                EnumName: "EnvType", EnumList: null, EnumMap: null, DBName: "DevOps", TBLName: "enum_EnvironmentVariableType"
            });

            this.NumEnums = this.EnumLinker.length;
            this.gotEnumsLists = 0;
            this.PopulateEnumListsAsync();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Stuff enum list. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/15/2017. </remarks>
        ///
        /// <param name="dresp">    The dresp. </param>
        /// <param name="elink">    The elink. </param>
        ///
        /// <returns>   . </returns>
        ///-------------------------------------------------------------------------------------------------
        private stuffEnumList(dresp: any, elink: Model.EnumLinkModel) {
            var EnumList: Model.EnumListItemModel[] = new Array<Model.EnumListItemModel>();
            var EnumMap = [];
            var d = dresp;
            for (var i = 0; i < d.length; i++) {
                if (d[i].Active = 1) {
                    var pm = new Model.EnumListItemModel();
                    pm.Active = d[i].Active;
                    pm.Description = d[i].Description;
                    pm.id = d[i].id;
                    pm.Item = d[i].Item;


                    EnumMap.push({
                        "value": pm.id, "text": pm.Item
                    });

                    EnumList.push(pm);
                }
            }

            elink.EnumList = EnumList;
            elink.EnumMap = EnumMap;

            elink.EnumList.sort(function (a, b) {
                var A = a.Item.toLowerCase(), B = b.Item.toLowerCase()
                if (A < B) //sort string ascending
                    return -1
                if (A > B)
                    return 1
                return 0 //default return value (no sorting)
            });
            return elink.EnumList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets enum link. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/15/2017. </remarks>
        ///
        /// <param name="ename">    The ename. </param>
        ///
        /// <returns>   The enum link. </returns>
        ///-------------------------------------------------------------------------------------------------
        public GetEnumLink(ename: string): Model.EnumLinkModel {
            var retlink: Model.EnumLinkModel = null;
            for (var i = 0; i < this.EnumLinker.length; i++) {
                if (this.EnumLinker[i].EnumName == ename) {
                    retlink = this.EnumLinker[i];
                    break;
                }
            }
            return retlink;
        }

        private GetEnumListAsync(enumName: string): ng.IPromise<Model.EnumLinkModel> {

            var dfo = this.qService.defer<Model.EnumLinkModel>();
            var current: any = this;

            var elink: Model.EnumLinkModel = this.GetEnumLink(enumName);

            if (elink.EnumList)
                dfo.resolve(elink);
            else {

                var getstr: string = "api:/EnumListItems?DBname=" + elink.DBName + "&tablename=" + elink.TBLName;

                this.httpService.get(getstr,
                    { withCredentials: true })
                    .success(function (response) {
                        current.stuffEnumList(response, elink);
                        dfo.resolve(elink);
                    })
                    .error(function (error) {
                        dfo.reject('Failed to get Enumlist');
                    });
            }
            return dfo.promise;
        }

        public PopulateEnumListsAsync(): ng.IPromise<Model.EnumLinkModel[]> {

            var dfo = this.qService.defer<Model.EnumLinkModel[]>();
            this.gotEnumsLists = 0;

            for (var i = 0; i < this.NumEnums; i++) {

                var elink: Model.EnumLinkModel = this.EnumLinker[i];
                var enumname = elink.EnumName;

                var current = this;
                current.GetEnumListAsync(enumname).then(function (data) {
                    current.gotEnumsLists++;
                    if (current.gotEnumsLists >= current.NumEnums) {
                        current.EnumServiceReady = true;
                        dfo.resolve(current.EnumLinker);
                    }
                }, function (reason) {
                    console.log("Hellish");
                })
            }

            return dfo.promise;
        }
    }
}