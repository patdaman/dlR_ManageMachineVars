using System.Web;
using System.Web.Optimization;

namespace DevOpsPortal
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-animate.js",
                      "~/Scripts/angular-ui-router.js",
                      "~/Scripts/angular-modal-service.js",
                      "~/Scripts/angular-resource.js",
                      "~/Scripts/csv.js",
                      "~/Scripts/pdfmake.js",
                      "~/Scripts/vfs_fonts.js",
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                      "~/Scripts/ui-grid.js",
                      "~/Scripts/ng-file-upload.js",
                      "~/Scripts/ng-file-upload-shim.js",
                      "~/Scripts/angular-multi-select.min.js",
                      "~/Scripts/clipboard.min.js",
                      "~/Scripts/ngclipboard.min.js",
                      "~/Scripts/sweetalert.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/AngularJS/app.js",
                      "~/AngularJS/Controllers/ConfigController.js",
                      "~/AngularJS/Controllers/ConfigModalController.js",
                      "~/AngularJS/Controllers/DashboardController.js",
                      "~/AngularJS/Controllers/MachineController.js",
                      "~/AngularJS/Controllers/PowershellController.js",
                      "~/AngularJS/Controllers/LogController.js",
                      "~/Scripts/ngclipboard.min.js",
                      "~/AngularJS/Directives/appDirectives.js",
                      "~/AngularJS/Directives/ConfigDirectives.js",
                      "~/AngularJS/Directives/powershellCodeMirror.js",

                      "~/AngularJS/Services/ConfigService.js",
                      "~/AngularJS/Services/DashboardHubService.js",
                      "~/AngularJS/Services/EnumListService.js",
                      "~/AngularJS/Services/LogHubService.js",
                      "~/AngularJS/Services/MachineService.js"
                      //"~/Scripts/Site.js"
                      //"~/AngularJS/Services/Utils.js",
                      //"~/AngularJS/Services/ngClickCopy.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     //"~/Content/bootstrap-combined.min.css",
                     "~/Content/bootstrap.css",
                     "~/Content/sweetalert.css",
                     "~/Content/angular-multi-select.css",
                     "~/Content/ui-grid.css",
                     "~/Content/site.css",
                     // Marcom Specific
                     "~/Content/ui-standard.css"
                     //
                     //"~/Content/site.css"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/codemirror").Include(
                      "~/Scripts/codemirror/lib/codemirror.js",
                      "~/Scripts/codemirror/ui-codemirror.min.js",
                      "~/Scripts/codemirror/mode/powershell/powershell.js",
                      "~/Scripts/codemirror/mode/matchbrackets.js",
                      "~/Scripts/codemirror/addon/hint/show-hint.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/codemirror").Include(
                      "~/Scripts/codemirror/addon/hint/show-hint.css",
                      "~/Scripts/codemirror/lib/codemirror.css",
                      "~/Scripts/codemirror/theme/midnight.css",
                      "~/Scripts/codemirror/theme/eclipse.css",
                      "~/Scripts/codemirror/theme/abcdef.css",
                      "~/Scripts/codemirror/theme/rubyblue.css",
                      "~/Scripts/codemirror/theme/solarized.css"
                    ));
        }
    }
}
