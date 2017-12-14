using System.Web;
using System.Web.Optimization;

namespace Homework05
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/Script.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/auth").Include(
                        "~/Scripts/Auth.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Scripts/DataTables/datatables.min.css",
                      "~/Scripts/DataTables/datatables.css",
                      "~/Scripts/DataTables/jquery.datatables.min.css",
                      "~/Scripts/DataTables/buttons.datatables.css",
                      "~/Content/sidenav.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                     "~/Scripts/knockout-{version}.js",
                     "~/Scripts/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/Dashboard").Include(
                     "~/Scripts/knockout-{version}.js",
                      "~/Scripts/Dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/PublishSurveys").Include(
                     "~/Scripts/knockout-{version}.js",
                      "~/Scripts/PublishSurveys.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                      
                      "~/Scripts/DataTables/datatables.min.js",
                      "~/Scripts/DataTables/jquery.datatables.min.js",
                      "~/Scripts/DataTables/buttons.datatables.js",
                      "~/Scripts/DataTables/buttons.print.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/SurveyMange").Include(
                     "~/Scripts/knockout-{version}.js",
                      "~/Scripts/SurveyManage.js"));

            bundles.Add(new ScriptBundle("~/bundles/ResponseMange").Include(
                     "~/Scripts/knockout-{version}.js",
                      "~/Scripts/ResponseManage.js"));
            bundles.Add(new ScriptBundle("~/bundles/MessageMange").Include(
                     "~/Scripts/knockout-{version}.js",
                      "~/Scripts/MessageManage.js"));

        }
    }
}
