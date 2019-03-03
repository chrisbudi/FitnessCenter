using System.Web.Optimization;

namespace FitnessCenter
{
    public static class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* ----- JS AREA ----- */

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundle/script/menu").Include(
                "~/Scripts/Menu.js"));

            bundles.Add(new ScriptBundle("~/bundle/script/types").Include(
                "~/Scripts/Converter/Decimal.js"));
            // core js
            bundles.Add(new ScriptBundle("~/bundle/script/core")
                .Include(
                    "~/Scripts/jquery-2.1.1.js", new CssRewriteUrlTransform()).Include(
                        "~/assets/global/plugins/jquery-ui/jquery-ui.min.js",
                        "~/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                        "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                        "~/assets/global/plugins/jquery.blockui.min.js",
                        "~/assets/global/plugins/jquery.cokie.min.js",
                        "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                        "~/Scripts/respond.js")
                .Include("~/assets/global/plugins/uniform/jquery.uniform.min.js", new CssRewriteUrlTransform())
                .Include("~/Scripts/jquery.validate.js").Include(
                    "~/assets/global/plugins/moment.min.js",
                    "~/Scripts/id.js"));

            bundles.Add(new ScriptBundle("~/bundle/script/globalize").Include(
                "~/Scripts/cldr.js",
                "~/Scripts/cldr/event.js",
                "~/Scripts/cldr/supplemental.js",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize/date.js",
                "~/Scripts/globalize/number.js",
                "~/Scripts/globalize/plural.js",
                "~/Scripts/globalize/run.js"
                ));


            bundles.Add(new ScriptBundle("~/bundle/script/jqueryAjax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"
                ));

            bundles.Add(new ScriptBundle("~/bundle/script/jqueryval").Include(
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.globalize.js"
            ));

            // level plugins js
            bundles.Add(new ScriptBundle("~/bundle/script/datatable").Include(
                "~/assets/global/plugins/datatables/media/js/jquery.dataTables.min.js",
                "~/assets/global/scripts/datatable.js",
                "~/assets/global/scripts/gridEvent.js",
                "~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js",
                "~/assets/global/plugins/datatables/extensions/ColVis/js/dataTables.colVis.js",
                "~/assets/global/plugins/typeahead/handlebars.min.js",
                "~/assets/global/plugins/typeahead/typeahead.bundle.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/script/dashboard").Include(

                ));

            bundles.Add(new ScriptBundle("~/bundle/script/ImagePlugin").Include(
                "~/assets/global/plugins/jcrop/js/jquery.Jcrop.min.js",
                "~/assets/global/plugins/jcrop/js/jquery.color.js"));

            bundles.Add(new ScriptBundle("~/bundle/script/login").Include(
                "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js")
                .Include("~/assets/global/plugins/select2/select2.min.js", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/backstretch/jquery.backstretch.min.js", new CssRewriteUrlTransform())
                .Include("~/assets/admin/pages/scripts/login-soft.js", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundle/script/component").Include(
                ));

            bundles.Add(new ScriptBundle("~/bundle/script/forminput").Include(
                "~/assets/global/plugins/fuelux/js/spinner.min.js",
                "~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js",
                "~/assets/global/plugins/jquery.input-ip-address-control-1.0.min.js",
                "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                "~/assets/global/plugins/jquery-tags-input/jquery.tagsinput.min.js",
                "~/assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.js",
                "~/assets/global/plugins/typeahead/handlebars.min.js",
                "~/assets/global/plugins/typeahead/typeahead.bundle.min.js",
                "~/assets/global/plugins/ckeditor/ckeditor.js",
                "~/assets/global/plugins/select2/select2.min.js",
                "~/assets/global/plugins/bootstrap-select/bootstrap-select.min.js",
                "~/assets/global/plugins/jquery-multi-select/js/jquery.multi-select.js",
                "~/assets/global/plugins/jquery-multi-select/js/jquery.quicksearch.js",
                // pickers plugins
                "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js",
                "~/assets/global/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                "~/assets/global/plugins/clockface/js/clockface.js",
                "~/assets/global/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.js",
                "~/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
                "~/Scripts/select2FormControl.js",
                //Component
                "~/assets/admin/pages/scripts/components-pickers.js",
                "~/assets/admin/pages/scripts/components-dropdowns.js",
                "~/assets/global/plugins/jquery-inputmask/min/jquery.inputmask.bundle.min.js"
                ));

            //<!-- IMPORTANT! fullcalendar depends on jquery-ui.min.js for drag & drop support -->
            bundles.Add(new ScriptBundle("~/bundle/script/dragdrop").Include(
                "~/assets/global/plugins/morris/morris.min.js",
                "~/assets/global/plugins/morris/raphael-min.js",
                "~/assets/global/plugins/jquery.sparkline.min.js"));

            //plugin Manual Scripting
            bundles.Add(new ScriptBundle("~/bundle/script/init").Include(
                "~/assets/global/scripts/metronic.js",
                "~/assets/admin/layout4/scripts/layout.js",
                "~/assets/admin/layout4/scripts/demo.js"
                ));


            //plugin for wizard
            bundles.Add(new ScriptBundle("~/bundle/script/wizard").Include(
                "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js",
                "~/assets/global/plugins/bootstrap-wizard/jquery.bootstrap.wizard.min.js",
                "~/assets/admin/layout4/scripts/quick-sidebar.js"));


            //plugin modal manager
            bundles.Add(new ScriptBundle("~/bundle/script/bootstrapModal").Include(
                "~/assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                "~/assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js",
                "~/assets/admin/pages/scripts/ui-extended-modals.js"));


            //plugin for calendar
            bundles.Add(new ScriptBundle("~/bundle/script/calendar").Include(
                "~/assets/global/plugins/fullcalendar/fullcalendar.min.js"
                ));


            /* ----- CSS AREA ----- */
            // mandatory styles
            bundles.Add(new StyleBundle("~/bundle/style/mandatory").Include(
                "~/Content/css.css")
                .Include("~/Content/font-awesome.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/simple-line-icons.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css", new CssRewriteUrlTransform())
                );

            // level styles
            bundles.Add(new StyleBundle("~/bundle/style/level")
                .Include("~/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css",
                    new CssRewriteUrlTransform())
                .Include(
                    "~/assets/global/plugins/gritter/css/jquery.gritter.css",
                    "~/Content/equal-height-columns.css",
                    "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css",
                    "~/assets/global/plugins/jqvmap/jqvmap/jqvmap.css",
                    "~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css",
                    "~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css",
                    // pickers plugins
                    "~/assets/global/plugins/clockface/css/clockface.css",
                    "~/assets/global/plugins/bootstrap-datepicker/css/datepicker3.css",
                    "~/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                    "~/assets/global/plugins/bootstrap-datetimepicker/css/datetimepicker.css",
                    // dropdowns plugins
                    "~/assets/global/plugins/bootstrap-select/bootstrap-select.min.css",
                    "~/assets/global/plugins/jquery-multi-select/css/multi-select.css",
                    // upload image plugins
                    "~/assets/global/plugins/colorbox/colorbox.css",
                    "~/assets/global/plugins/jcrop/css/jquery.Jcrop.min.css",
                    "~/assets/global/plugins/typeahead/typeahead.css"

                ).Include("~/assets/global/plugins/bootstrap-colorpicker/css/colorpicker.css",
                    new CssRewriteUrlTransform()
                ).Include("~/assets/global/plugins/select2/select2.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundle/style/loginsoft").Include(
                "~/assets/admin/pages/css/login-soft.css", new CssRewriteUrlTransform()
                ));

            //login controller
            bundles.Add(new StyleBundle("~/bundle/style/login").Include(
                // login
                "~/assets/admin/pages/css/login.css"));


            // themes styles
            bundles.Add(new StyleBundle("~/bundle/style/theme")
                .Include("~/assets/global/css/components-rounded.css", new CssRewriteUrlTransform())
                .Include("~/assets/admin/layout4/css/themes/light.css", new CssRewriteUrlTransform())
                .Include("~/assets/global/css/components.css", new CssRewriteUrlTransform())
                .Include(
                    "~/assets/global/css/plugins.css",
                    "~/assets/admin/layout4/css/layout.css",
                    "~/assets/admin/layout4/css/custom.css",
                    "~/assets/admin/pages/css/tasks.css")
                );

            //css Component
            bundles.Add(new StyleBundle("~/bundle/style/calendar").Include(
                "~/assets/global/plugins/fullcalendar/fullcalendar.min.css", new CssRewriteUrlTransform()));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit https://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}