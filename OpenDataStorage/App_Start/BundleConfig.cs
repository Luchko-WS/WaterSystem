using System.Web.Optimization;

namespace OpenDataStorage
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery")
                .Include("~/Scripts/libs/jquery/jquery-{version}.js")
                .Include("~/Scripts/libs/jquery/jquery-ui-1.12.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/angularjs")
                .Include("~/Scripts/libs/angularjs/angular.js")
                .Include("~/Scripts/libs/angularjs/angular-cookies.js")
                .Include("~/Scripts/libs/angularjs/angular-translate.js")
                .Include("~/Scripts/libs/angularjs/angular-translate-loader-url.js")
                .Include("~/Scripts/libs/angularjs/angular-ui/ui-bootstrap.js")
                .Include("~/Scripts/libs/angularjs/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jqueryval")
                .Include("~/Scripts/libs/jquery/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/scripts/modernizr")
                .Include("~/Scripts/libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/bootstrap")
                .Include("~/Scripts/libs/bootstrap/bootstrap.js")
                .Include("~/Scripts/libs/bootstrap/bootstrap-treeview.js"));

            bundles.Add(new StyleBundle("~/Content/styles/css")
                .Include(
                      "~/Content/css/bootstrap/bootstrap.css",
                      "~/Content/css/angular-block-ui/angular-block-ui.css",
                      "~/Content/css/ui-bootstrap-csp.css",
                      "~/Content/css/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app")
                .Include("~/Scripts/app/app.js")
                .IncludeDirectory("~/Scripts/app/Common/Services", "*.js", true)
                .IncludeDirectory("~/Scripts/app/Common/Controllers", "*.js", true)
                .IncludeDirectory("~/Scripts/app/Common/Directives", "*.js", true)
                .IncludeDirectory("~/Scripts/app/Common/Filters", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/Characteristic")
                .IncludeDirectory("~/Scripts/app/Characteristic", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/HierarchyObject")
                .IncludeDirectory("~/Scripts/app/HierarchyObject", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/ObjectType")
                .IncludeDirectory("~/Scripts/app/ObjectType", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/Data")
                .IncludeDirectory("~/Scripts/app/Data", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/Reports")
                .IncludeDirectory("~/Scripts/app/Reports", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/Aliases")
                .IncludeDirectory("~/Scripts/app/Aliases", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app/Administration")
                .IncludeDirectory("~/Scripts/app/Administration", "*.js", true));
        }
    }
}
