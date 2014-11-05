﻿using System.Web;
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
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/customscripts").Include(
                        "~/Scripts/app.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/knockout-2.2.0.debug.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/raphael1.js",
                        "~/Scripts/morris.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.blockUI.js",
                        "~/Scripts/bootstrap-slider/bootstrap-slider.js",
                        "~/Scripts/datepicker/bootstrap-datepicker.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/newcss").Include(
                        "~/Content/font-awesome.css",
                        "~/Content/bootstrap.css",
                        "~/Content/datepicker3.css",
                        "~/Content/morris.css",
                        "~/Content/slider.css",
                        "~/Content/overlay-growl.css",
                        "~/Content/bootstrap-theme.css"));

            bundles.Add(new StyleBundle("~/Content/newcsslogin").Include(
                        "~/Content/newSiteLogin.css"));

            bundles.Add(new StyleBundle("~/Content/newcsscommon").Include(
                        "~/Content/common.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}