using System.Web;
using System.Web.Optimization;

namespace Estimator.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css",
                    //"~/Content/bootstrap-theme.css",
                    "~/Content/font-awesome.css",
                    "~/Scripts/ui-select/select.css",
                    "~/Content/site.css"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include(
                    "~/Scripts/modernizr-*"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/moment.js",
                    "~/Scripts/underscore.js",
                    "~/Scripts/linq.js",
                    "~/Scripts/bootstrap.js"
                )
            );

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-messages.js",
                    "~/Scripts/angular-resource.js",
                    "~/Scripts/angular-sanitize.js",
                    /* addon */
                    "~/Scripts/angular-moment.js",
                    "~/Scripts/ui-router/angular-ui-router.js",
                    "~/Scripts/ui-utils/ui-utils.js",
                    "~/Scripts/ui-select/select.js",
                    "~/Scripts/angular-ui/ui-bootstrap.js"
                )
            );


            bundles.Add(new ScriptBundle("~/bundles/estimator")
                .IncludeDirectory("~/app/common/", "*.js")

                .Include("~/app/app.js")
                .Include("~/app/app.config.js")

                .IncludeDirectory("~/app/services/", "*.js")

                .IncludeDirectory("~/app/home/", "*.js")                
                .IncludeDirectory("~/app/estimate/", "*.js")
                .IncludeDirectory("~/app/factor/", "*.js")
            );

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
