﻿using System.Web;
using System.Web.Optimization;

namespace AppConfigurationWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-route.js",
                      "~/Scripts/ui-grid.min.js",
                      "~/Scripts/csv.js",
                      "~/Scripts/pdfmake.js",
                      "~/Scripts/vfs_fonts.js",
                      //"~/AngularJS/app.ts",
                      //"~/AngularJS/configApp.ts",
                      "~/AngularJS/Controllers/MachineController.ts",
                      //"~/AngularJS/Controllers/MachineController2.js",
                      "~/AngularJS/Controllers/ConfigController.ts"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ui-grid.css",
                      "~/Content/Site.css"));
        }
    }
}
