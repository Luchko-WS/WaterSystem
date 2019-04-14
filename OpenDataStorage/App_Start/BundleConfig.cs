using System.Web.Optimization;

namespace OpenDataStorage
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/libs/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs")
                .Include("~/Scripts/libs/angularjs/angular.js")
                .Include("~/Scripts/libs/angularjs/angular-cookies.js")
                .Include("~/Scripts/libs/angularjs/angular-translate.js")
                .Include("~/Scripts/libs/angularjs/angular-translate-loader-url.js")
                .Include("~/Scripts/libs/angularjs/angular-ui/ui-bootstrap.js")
                .Include("~/Scripts/libs/angularjs/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/libs/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/libs/bootstrap/bootstrap.js")
                .Include("~/Scripts/libs/bootstrap/bootstrap-treeview.min.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                      "~/Content/css/bootstrap/bootstrap.css",
                      "~/Content/css/angular-block-ui/angular-block-ui.css",
                      "~/Content/css/ui-bootstrap-csp.css",
                      "~/Content/css/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/Scripts/app/app.js")
                .IncludeDirectory("~/Scripts/app/Common/Services", "*.js", true)
                .IncludeDirectory("~/Scripts/app/Common/Controllers", "*.js", true)
                .IncludeDirectory("~/Scripts/app/Common/Directives", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/app/Characteristic")
                .IncludeDirectory("~/Scripts/app/Characteristic", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/app/HierarchyObject")
                .IncludeDirectory("~/Scripts/app/HierarchyObject", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/app/HierarchyObjectType")
                .IncludeDirectory("~/Scripts/app/HierarchyObjectType", "*.js", true));
        }
    }
}
