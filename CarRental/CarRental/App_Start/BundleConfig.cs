using System;
using System.Web;
using System.Web.Optimization;

namespace CarRental
{
    public class BundleConfig
    {

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
           // ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }



        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/moment.js",
                "~/Scripts/bootstrap-datetimepicker.js"));


            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                 "~/Scripts/kendo/kendo.all.min.js",
                 "~/Scripts/kendo/kendo.combobox.min.js",
                 "~/Scripts/kendo/kendo.datepicker.min.js",
                 "~/Scripts/kendo/kendo.maskedtextbox.min.js",
                 "~/Scripts/kendo/kendo.upload.min.js",
                 "~/Scripts/kendo/kendo.maskedtextbox.min.js",
                 "~/Scripts/kendo/kendo.grid.min.js",
                 "~/Scripts/kendo/kendo.validator.min.js",
                 "~/Scripts/kendo/kendo.autocomplete.min.js",
                 "~/Scripts/kendo/kendo.aspnetmvc.min.js"));

            bundles.Add(new ScriptBundle("~/Content/kendo/css").Include(
                 "~/Content/kendo/kendo.common-bootstrap.min.css",
                 "~/Content/kendo/kendo.material.min.css",
                 "~/Content/kendo/kendo.bootstrap.min.css"));       
        }
    }
}
