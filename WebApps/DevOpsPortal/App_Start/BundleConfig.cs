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
                        "~/Scripts/jquery.signalR-{version}.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                        "~/Scripts/toastr.min.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

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
                      "~/Scripts/codemirror/theme/elegant.css",
                      "~/Scripts/codemirror/theme/blackboard.css",
                      "~/Scripts/codemirror/theme/hopscotch.css",
                      "~/Scripts/codemirror/theme/dracula.css",
                      "~/Scripts/codemirror/theme/solarized.css"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-animate.js",
                      "~/Scripts/angular-ui-router.js",
                      "~/Scripts/angular-modal-service.js",
                      "~/Scripts/angular-resource.js",
                      "~/Scripts/angular-material/angular-material.min.js",
                      "~/Scripts/angular-aria/angular-aria.min.js",
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
                      "~/Scripts/sweetalert.min.js",
                      "~/Scripts/AngularFileSaver/angular-file-saver.bundle.min.js",
                      "~/Scripts/angular-busy.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/app")
                    .IncludeDirectory("~/AngularJS", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/dashboard")
                    .IncludeDirectory("~/Scripts/Dashboard", "*.js", true));
            bundles.Add(new StyleBundle("~/bundles/dashboardcss")
                    .IncludeDirectory("~/Content/Dashboard", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     //"~/Content/bootstrap-combined.min.css",
                     "~/Content/angular-busy.min.css",
                     "~/Content/angular-material.min.css",
                     "~/Content/bootstrap.min.css",
                     "~/Content/toastr.min.css",
                     "~/Content/sweetalert.css",
                     "~/Content/angular-multi-select.css",
                     "~/Content/ui-grid.css",
                     // Marcom Specific
                     //"~/Content/ui-standard.css",
                     //
                     "~/Content/site.css"
                     ));
        }
    }
}
