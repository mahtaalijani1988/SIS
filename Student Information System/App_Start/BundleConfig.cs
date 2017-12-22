using System.Web;
using System.Web.Optimization;

namespace Student_Information_System
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Content/js/Validation/jqueryval-default.min.js",
                "~/Content/js/Validation/jquery.validate*"));
            
            bundles.Add(new ScriptBundle("~/jquery").Include(
                "~/Content/js/jquery.js"
                ));

            bundles.Add(new ScriptBundle("~/js").Include(
                        "~/Content/js/jquery-ui-1.8.16.custom.min.js",
                        "~/Content/js/jquery.ui.touch-punch.js",
                        "~/Content/js/bootstrap.js",
                        "~/Content/js/prettify.js",
                        "~/Content/js/jquery.sparkline.min.js",
                        "~/Content/js/jquery.nicescroll.min.js",
                        "~/Content/js/accordion.jquery.js",
                        "~/Content/js/smart-wizard.jquery.js",
                        "~/Content/js/vaidation.jquery.js",
                        "~/Content/js/jquery-dynamic-form.js",
                        "~/Content/js/fullcalendar.js",
                        "~/Content/js/raty.jquery.js",
                        "~/Content/js/jquery.noty.js",
                        "~/Content/js/jquery.cleditor.min.js",
                        "~/Content/js/data-table.jquery.js",
                        "~/Content/js/TableTools.min.js",
                        "~/Content/js/ColVis.min.js",
                        "~/Content/js/plupload.full.js",
                        "~/Content/js/elfinder/elfinder.min.js",
                        "~/Content/js/chosen.jquery.js",
                        "~/Content/js/uniform.jquery.js",
                        "~/Content/js/jquery.tagsinput.js",
                        "~/Content/js/jquery.colorbox-min.js",
                        "~/Content/js/check-all.jquery.js",
                        "~/Content/js/inputmask.jquery.js",
                        "~/Content/js/plupupload/jquery.plupload.queue/jquery.plupload.queue.js",
                        "~/Content/js/excanvas.min.js",
                        "~/Content/js/jquery.jqplot.min.js",
                        "~/Content/js/chart/jqplot.highlighter.min.js",
                        "~/Content/js/chart/jqplot.cursor.min.js",
                        "~/Content/js/chart/jqplot.dateAxisRenderer.min.js",
                        "~/Content/js/custom-script.js",

                        "~/Content/js/jquery.sparkline.min.js"));

            bundles.Add(new ScriptBundle("~/Main/cssRtl").Include(
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/bootstrap-responsive.css",
                        "~/Content/css/stylesrtl.css",
                        "~/Content/css/themes.css",
                        "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Highcharts").Include(
                "~/js/Highcharts-4.0.1/js/jdate.min.js",
                "~/js/Highcharts-4.0.1/js/highcharts.js",
                "~/js/Highcharts-4.0.1/js/modules/exporting.js"));
            
            bundles.Add(new ScriptBundle("~/Main/css").Include(
                        "~/Content/css/bootstrap.css",
                        "~/Content/css/bootstrap-responsive.css",
                        "~/Content/css/styles.css",
                        "~/Content/css/themes.css",
                        "~/Content/Site.css",

                        "~/Content/sweet-alert.css"));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/js/jquery-{version}.js",
                "~/Content/js/Validation/jquery.unobtrusive-ajax.js"));


            bundles.Add(new ScriptBundle("~/Main/js").Include(
                        "~/Content/js/bootstrap.js",
                        "~/Content/js/jquery-ui-1.8.16.custom.min.js",
                        "~/Content/js/jquery.ui.touch-punch.js",
                        "~/Content/js/prettify.js",
                        "~/Content/js/jquery.sparkline.min.js",
                        "~/Content/js/jquery.nicescroll.min.js",
                        "~/Content/js/accordion.jquery.js",
                        "~/Content/js/smart-wizard.jquery.js",
                        "~/Content/js/jquery-dynamic-form.js",
                        "~/Content/js/fullcalendar.js",
                        "~/Content/js/raty.jquery.js",
                        "~/Content/js/jquery.noty.js",
                        "~/Content/js/jquery.cleditor.min.js",
                        "~/Content/js/data-table.jquery.js",
                        "~/Content/js/TableTools.min.js",
                        "~/Content/js/ColVis.min.js",
                        "~/Content/js/plupload.full.js",
                        "~/Content/js/elfinder/elfinder.min.js",
                        "~/Content/js/chosen.jquery.js",
                        "~/Content/js/uniform.jquery.js",
                        "~/Content/js/jquery.tagsinput.js",
                        "~/Content/js/jquery.colorbox-min.js",
                        "~/Content/js/check-all.jquery.js",
                        "~/Content/js/inputmask.jquery.js",
                        "~/Content/js/plupupload/jquery.plupload.queue/jquery.plupload.queue.js",
                        "~/Content/js/excanvas.min.js",
                        "~/Content/js/jquery.jqplot.min.js",
                        "~/Content/js/chart/jqplot.highlighter.min.js",
                        "~/Content/js/chart/jqplot.cursor.min.js",
                        "~/Content/js/chart/jqplot.dateAxisRenderer.min.js",
                        "~/Content/js/custom-script.js",

                        "~/Content/sweet-alert.js"));



            bundles.Add(new StyleBundle("~/bundles/plupload/css").Include(
                        "~/Content/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css"));

            bundles.Add(new ScriptBundle("~/bundles/plupload/js").Include(
                        "~/Content/plupload/plupload.full.min.js",
                        "~/Content/plupload/jquery.plupload.queue/jquery.plupload.queue.js",
                        "~/Content/plupload/i18n/fa.js"));
            
        }
    }
}
