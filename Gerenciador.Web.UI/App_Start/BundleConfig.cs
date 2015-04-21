using System.Web;
using System.Web.Optimization;

namespace Gerenciador.Web.UI {
    public class BundleConfig {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/custom/fix-date-validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/customscripts").Include(
                        "~/Scripts/app.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/mansory/masonry.pkgd.js",
                        "~/Scripts/raphael1.js",
                        "~/Scripts/morris.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/bootstrap-slider/bootstrap-slider.js",
                        "~/Scripts/datepicker/bootstrap-datepicker.js",
                        "~/Scripts/knockout-2.2.1.js",
                        "~/Scripts/custom/layout.js",
                        "~/Scripts/custom/ajaxCall.js",
                        "~/Scripts/async-emitter.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/newcss").Include(
                        "~/Content/font-awesome.css",
                        "~/Content/bootstrap.css",
                        "~/Content/datepicker3.css",
                        "~/Content/morris.css",
                        "~/Content/slider.css",
                        "~/Content/overlay-growl.css",
                        "~/Content/bootstrap-theme.css",
                        "~/Content/custom-breadcumb.css",
                        "~/Content/custom-todo-list.css"));

            bundles.Add(new StyleBundle("~/Content/newcsslogin").Include(
                        "~/Content/newSiteLogin.css"));

            bundles.Add(new StyleBundle("~/Content/newcsscommon").Include(
                        "~/Content/common.css",
                        "~/Content/common-extensions.css"));

            RegisterBundlesForAdminDashboard(bundles);

            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterBundlesForAdminDashboard(BundleCollection bundles){
             bundles.Add(new ScriptBundle("~/bundles/admin-dashboard").Include("~/Scripts/jquery-ui-{version}.js", 
                                        "~/Scripts/custom/todo-list/todo.js",
                                        "~/Scripts/custom/dashboard/dashboard-admin.js"));
        }
    } //class
}