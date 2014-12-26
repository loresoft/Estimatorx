using System;
using System.Web.Optimization;

namespace Estimatorx.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/bootstrap-dialog.css",
                    "~/Content/font-awesome.css",
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
                    "~/Scripts/ObjectId.js",
                    "~/Scripts/linq.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrap-dialog.js"
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
                    "~/Scripts/angular-ui/ui-bootstrap.js",
                    "~/Scripts/angular-ui/ui-bootstrap-tpls.js"
                )
            );


            bundles.Add(new ScriptBundle("~/bundles/estimator")
                .IncludeDirectory("~/app/common/", "*.js")

                .Include("~/app/app.js")

                .IncludeDirectory("~/app/services/", "*.js")
                .IncludeDirectory("~/app/filters/", "*.js")
                .IncludeDirectory("~/app/directives/", "*.js")
                
                .IncludeDirectory("~/app/project/", "*.js")
                .IncludeDirectory("~/app/factor/", "*.js")
            );

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
