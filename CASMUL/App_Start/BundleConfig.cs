using System.Web.Optimization;

namespace CASMUL
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           bundles.Add(new StyleBundle("~/bundles/CSS").Include(
                "~/assets/plugins/jquery-ui/themes/base/minified/jquery-ui.min.css",
                "~/assets/plugins/bootstrap/css/bootstrap.min.css",
                "~/assets/plugins/font-awesome/css/font-awesome.min.css",
                "~/assets/css/animate.min.css",
                "~/assets/css/style.min.css",
                "~/assets/css/style-responsive.min.css",
                "~/assets/css/theme/default.css",
                "~/assets/plugins/jquery-jvectormap/jquery-jvectormap.css",
                "~/assets/plugins/bootstrap-calendar/css/bootstrap_calendar.css",
                "~/assets/plugins/gritter/css/jquery.gritter.css",

                 "~/assets/plugins/DataTables/media/css/dataTables.bootstrap.min.css",
                 "~/assets/plugins/DataTables/extensions/Responsive/css/responsive.bootstrap.min.css",

                 "~/assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker.css",
                 "~/assets/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.css",
                 "~/assets/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                 "~/assets/plugins/bootstrap-select/bootstrap-select.min.css",
                 "~/assets/plugins/bootstrap-daterangepicker/daterangepicker.css",
                 "~/assets/plugins/select2/dist/css/select2.min.css",
                 "~/assets/plugins/bootstrap-eonasdan-datetimepicker/build/css/bootstrap-datetimepicker.min.css",
                 "~/assets/plugins/fullcalendar/fullcalendar.css",
                 "~/assets/plugin/bootstrap-sweetalert/sweetalert.css"
                ));
                     

            bundles.Add(new ScriptBundle("~/bundles/ColorAdmin").Include(
                       "~/assets/plugins/jquery/jquery-1.9.1.min.js",
                       "~/assets/plugins/jquery/jquery-migrate-1.1.0.min.js",
                       "~/assets/plugins/jquery-ui/ui/minified/jquery-ui.min.js",
                       "~/assets/plugins/bootstrap/js/bootstrap.min.js",
                       "~/assets/plugins/slimscroll/jquery.slimscroll.min.js",
                       "~/assets/plugins/jquery-cookie/jquery.cookie.js",
                       "~/assets/plugins/masked-input/masked-input.min.js",

                       "~/assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                       "~/assets/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                       "~/assets/plugins/bootstrap-select/bootstrap-select.min.js",
                       "~/assets/plugins/bootstrap-daterangepicker/moment.min.js",
                       "~/assets/plugins/bootstrap-daterangepicker/daterangepicker.js",
                       "~/assets/plugins/select2/dist/js/select2.min.js",
                       "~/assets/plugins/bootstrap-eonasdan-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
    
                       "~/assets/plugins/jquery-jvectormap/jquery-jvectormap.min.js",
                       "~/assets/plugins/jquery-jvectormap/jquery-jvectormap-world-merc-en.js",
                       "~/assets/plugins/bootstrap-calendar/js/bootstrap_calendar.min.js",
                       "~/assets/plugins/gritter/js/jquery.gritter.js",
                      
                       "~/assets/js/apps.min.js",

                       "~/assets/plugins/DataTables/media/js/jquery.dataTables.min.js",
                       "~/assets/plugins/DataTables/media/js/dataTables.bootstrap.min.js",
                       "~/assets/plugins/DataTables/extensions/Responsive/js/dataTables.responsive.min.js",
                       "~/assets/plugins/fullcalendar/fullcalendar.min.js",
                       "~/assets/plugin/bootstrap-sweetalert/sweetalert.min.js",

                       "~/assets/Layout.js "
                       
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/assets/InputFormat.js ",
                "~/Scripts/jquery.validate*"));


            bundles.Add(new StyleBundle("~/bundles/LoginCSS").Include(
                    "~/assets/plugins/jquery-ui/themes/base/minified/jquery-ui.min.css",
                    "~/assets/plugins/bootstrap/css/bootstrap.min.css",
                    "~/assets/plugins/font-awesome/css/font-awesome.min.css",
                    "~/assets/css/animate.min.css",
                    "~/assets/css/style.min.css",
                    "~/assets/css/style-responsive.min.css",
                    "~/assets/css/theme/default.css",
                    "~/assets/plugins/pace/pace.min.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/LoginColorAdmin").Include(
                   "~/assets/plugins/jquery/jquery-1.9.1.min.js",
                   "~/assets/plugins/jquery/jquery-migrate-1.1.0.min.js",
                   "~/assets/plugins/jquery-ui/ui/minified/jquery-ui.min.js",
                   "~/assets/plugins/bootstrap/js/bootstrap.min.js",
                   "~/assets/plugins/slimscroll/jquery.slimscroll.min.js",
                   "~/assets/plugins/jquery-cookie/jquery.cookie.js",
                   "~/assets/js/apps.min.js"
                   ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
