using System.Web;
using System.Web.Optimization;

namespace FindTech.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bo/progressive/css").Include(
                    "~/Areas/BO/UIFramework/Progressive/css/buttons/buttons.css",
                    "~/Areas/BO/UIFramework/Progressive/css/buttons/social-icons.css",
                    "~/Areas/BO/UIFramework/Progressive/css/font-awesome.min.css",
                    "~/Areas/BO/UIFramework/Progressive/css/bootstrap.min.css",
                    "~/Areas/BO/UIFramework/Progressive/css/custom.bootstrap.css",
                    "~/Areas/BO/UIFramework/Progressive/css/jslider.css",
                    "~/Areas/BO/UIFramework/Progressive/css/settings.css",
                    "~/Areas/BO/UIFramework/Progressive/css/jquery.fancybox.css",
                    "~/Areas/BO/UIFramework/Progressive/css/animate.css",
                    "~/Areas/BO/UIFramework/Progressive/css/video-js.min.css",
                    "~/Areas/BO/UIFramework/Progressive/css/morris.css",
                    "~/Areas/BO/UIFramework/Progressive/css/royalslider/royalslider.css",
                    "~/Areas/BO/UIFramework/Progressive/css/royalslider/skins/minimal-white/rs-minimal-white.css",
                    "~/Areas/BO/UIFramework/Progressive/css/layerslider/layerslider.css",
                    "~/Areas/BO/UIFramework/Progressive/css/ladda.min.css",
                    "~/Areas/BO/UIFramework/Progressive/css/datepicker.css",
                    "~/Areas/BO/UIFramework/Progressive/css/jquery.scrollbar.css",
                    "~/Areas/BO/UIFramework/Progressive/css/style.css",
                    "~/Areas/BO/UIFramework/Progressive/css/responsive.css",
                    "~/Areas/BO/UIFramework/Progressive/css/customizer/pages.css",
                    "~/Areas/BO/UIFramework/Progressive/css/customizer/home-pages-customizer.css",
                    "~/Areas/BO/UIFramework/Progressive/css/bootstrap-tagsinput.css",
                    "~/Areas/BO/UIFramework/Progressive/css/ie/ie.css"));

            bundles.Add(new StyleBundle("~/bo/kendoui/css").Include(
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.common-bootstrap.min.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.bootstrap.min.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.custom.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.dataviz.min.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.dataviz.default.min.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.default.min.css",
                    "~/Areas/BO/UIFramework/KendoUI/styles/kendo.common.min.css"));

            bundles.Add(new StyleBundle("~/bo/validation/css").Include(
                "~/Areas/BO/UIFramework/Validation/styles/formValidation.css"));

            bundles.Add(new ScriptBundle("~/bo/progressive/js").Include(
                    "~/Areas/BO/UIFramework/Progressive/js/bootstrap.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/jshashtable-2.1_src.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/jquery.numberformatter-1.2.3.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/tmpl.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/jquery.dependClass-0.1.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/draggable-0.1.js",
                    "~/Areas/BO/UIFramework/Progressive/js/price-regulator/jquery.slider.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.carouFredSel-6.2.1-packed.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.touchSwipe.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.elevateZoom-3.0.8.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.imagesloaded.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.appear.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.sparkline.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.easypiechart.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.easing.1.3.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.fancybox.pack.js",
                    "~/Areas/BO/UIFramework/Progressive/js/isotope.pkgd.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.knob.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.stellar.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.selectBox.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.royalslider.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.tubular.1.0.js",
                    "~/Areas/BO/UIFramework/Progressive/js/SmoothScroll.js",
                    "~/Areas/BO/UIFramework/Progressive/js/country.js",
                    "~/Areas/BO/UIFramework/Progressive/js/spin.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/ladda.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/masonry.pkgd.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/morris.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/raphael.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/video.js",
                    "~/Areas/BO/UIFramework/Progressive/js/pixastic.custom.js",
                    "~/Areas/BO/UIFramework/Progressive/js/livicons-1.3.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/layerslider/greensock.js",
                    "~/Areas/BO/UIFramework/Progressive/js/layerslider/layerslider.transitions.js",
                    "~/Areas/BO/UIFramework/Progressive/js/layerslider/layerslider.kreaturamedia.jquery.js",
                    "~/Areas/BO/UIFramework/Progressive/js/revolution/jquery.themepunch.plugins.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/revolution/jquery.themepunch.revolution.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/bootstrapValidator.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/bootstrap-datepicker.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jplayer/jquery.jplayer.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jplayer/jplayer.playlist.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/jquery.scrollbar.min.js",
                    "~/Areas/BO/UIFramework/Progressive/js/holder.js",
                    "~/Areas/BO/UIFramework/Progressive/js/bootstrap-tagsinput.js",
                    "~/Areas/BO/UIFramework/Progressive/js/typeahead.js",
                    "~/Areas/BO/UIFramework/Progressive/js/main.js"));

            bundles.Add(new ScriptBundle("~/bo/kendoui/js").Include(
                    "~/Areas/BO/UIFramework/KendoUI/js/jquery.min.js",
                    "~/Areas/BO/UIFramework/KendoUI/js/angular.min.js",
                    "~/Areas/BO/UIFramework/KendoUI/js/kendo.all.min.js",
                    "~/Areas/BO/UIFramework/KendoUI/js/jquery.bootstrap.wizard.js"));

            bundles.Add(new ScriptBundle("~/bo/validation/js").Include(
                    "~/Areas/BO/UIFramework/Validation/js/formValidation.js",
                    "~/Areas/BO/UIFramework/Validation/js/formValidation-bootstrap.js"));

            bundles.Add(new StyleBundle("~/fo/progressive/css").Include(
                    "~/UIFramework/Progressive/css/buttons/buttons.css",
                    "~/UIFramework/Progressive/css/buttons/social-icons.css",
                    "~/UIFramework/Progressive/css/font-awesome.min.css",
                    "~/UIFramework/Progressive/css/bootstrap.min.css",
                    "~/UIFramework/Progressive/css/custom.bootstrap.css",
                    "~/UIFramework/Progressive/css/jslider.css",
                    "~/UIFramework/Progressive/css/settings.css",
                    "~/UIFramework/Progressive/css/jquery.fancybox.css",
                    "~/UIFramework/Progressive/css/animate.css",
                    "~/UIFramework/Progressive/css/video-js.min.css",
                    "~/UIFramework/Progressive/css/morris.css",
                    "~/UIFramework/Progressive/css/royalslider/royalslider.css",
                    "~/UIFramework/Progressive/css/royalslider/skins/minimal-white/rs-minimal-white.css",
                    "~/UIFramework/Progressive/css/layerslider/layerslider.css",
                    "~/UIFramework/Progressive/css/jssorslider/jssorslider.css",
                    "~/UIFramework/Progressive/css/ladda.min.css",
                    "~/UIFramework/Progressive/css/datepicker.css",
                    "~/UIFramework/Progressive/css/jquery.scrollbar.css",
                    "~/UIFramework/Progressive/css/style.css",
                    "~/UIFramework/Progressive/css/responsive.css",
                    "~/UIFramework/Progressive/css/customizer/pages.css",
                    "~/UIFramework/Progressive/css/customizer/home-pages-customizer.css",
                    "~/UIFramework/Progressive/css/bootstrap-tagsinput.css",
                    "~/UIFramework/Progressive/css/ie/ie.css"));

            bundles.Add(new StyleBundle("~/fo/article/detail/css").Include(
                    "~/UIFramework/Progressive/css/jssorslider/jssorslider.css"));

            bundles.Add(new ScriptBundle("~/fo/progressive/js").Include(
                    "~/UIFramework/Progressive/js/bootstrap.min.js",
                    "~/UIFramework/Progressive/js/price-regulator/jshashtable-2.1_src.js",
                    "~/UIFramework/Progressive/js/price-regulator/jquery.numberformatter-1.2.3.js",
                    "~/UIFramework/Progressive/js/price-regulator/tmpl.js",
                    "~/UIFramework/Progressive/js/price-regulator/jquery.dependClass-0.1.js",
                    "~/UIFramework/Progressive/js/price-regulator/draggable-0.1.js",
                    "~/UIFramework/Progressive/js/price-regulator/jquery.slider.js",
                    "~/UIFramework/Progressive/js/jquery.carouFredSel-6.2.1-packed.js",
                    "~/UIFramework/Progressive/js/jquery.touchSwipe.min.js",
                    "~/UIFramework/Progressive/js/jquery.elevateZoom-3.0.8.min.js",
                    "~/UIFramework/Progressive/js/jquery.imagesloaded.min.js",
                    "~/UIFramework/Progressive/js/jquery.appear.js",
                    "~/UIFramework/Progressive/js/jquery.sparkline.min.js",
                    "~/UIFramework/Progressive/js/jquery.easypiechart.min.js",
                    "~/UIFramework/Progressive/js/jquery.easing.1.3.js",
                    "~/UIFramework/Progressive/js/jquery.fancybox.pack.js",
                    "~/UIFramework/Progressive/js/isotope.pkgd.min.js",
                    "~/UIFramework/Progressive/js/jquery.knob.js",
                    "~/UIFramework/Progressive/js/jquery.stellar.min.js",
                    "~/UIFramework/Progressive/js/jquery.selectBox.min.js",
                    "~/UIFramework/Progressive/js/jquery.royalslider.min.js",
                    "~/UIFramework/Progressive/js/jquery.tubular.1.0.js",
                    "~/UIFramework/Progressive/js/SmoothScroll.js",
                    "~/UIFramework/Progressive/js/country.js",
                    "~/UIFramework/Progressive/js/spin.min.js",
                    "~/UIFramework/Progressive/js/ladda.min.js",
                    "~/UIFramework/Progressive/js/masonry.pkgd.min.js",
                    "~/UIFramework/Progressive/js/morris.min.js",
                    "~/UIFramework/Progressive/js/raphael.min.js",
                    "~/UIFramework/Progressive/js/video.js",
                    "~/UIFramework/Progressive/js/pixastic.custom.js",
                    "~/UIFramework/Progressive/js/livicons-1.4.min.js",
                    "~/UIFramework/Progressive/js/layerslider/greensock.js",
                    "~/UIFramework/Progressive/js/layerslider/layerslider.transitions.js",
                    "~/UIFramework/Progressive/js/layerslider/layerslider.kreaturamedia.jquery.js",
                    "~/UIFramework/Progressive/js/jssorslider/jssor.js",
                    "~/UIFramework/Progressive/js/jssorslider/jssor.slider.js",
                    "~/UIFramework/Progressive/js/revolution/jquery.themepunch.plugins.min.js",
                    "~/UIFramework/Progressive/js/revolution/jquery.themepunch.revolution.min.js",
                    "~/UIFramework/Progressive/js/bootstrapValidator.min.js",
                    "~/UIFramework/Progressive/js/bootstrap-datepicker.js",
                    "~/UIFramework/Progressive/js/jplayer/jquery.jplayer.min.js",
                    "~/UIFramework/Progressive/js/jplayer/jplayer.playlist.min.js",
                    "~/UIFramework/Progressive/js/jquery.scrollbar.min.js",
                    "~/UIFramework/Progressive/js/holder.js",
                    "~/UIFramework/Progressive/js/bootstrap-tagsinput.js",
                    "~/UIFramework/Progressive/js/typeahead.js",
                    "~/UIFramework/Progressive/js/jquery.zclip.min.js",
                    "~/UIFramework/Progressive/js/main.js"));

            bundles.Add(new ScriptBundle("~/fo/moment/js").Include(
                   "~/UIFramework/Moment/js/moment-with-locales.js"));

            bundles.Add(new ScriptBundle("~/fo/article/detail/js").Include(
                   "~/UIFramework/Progressive/js/jssorslider/jssor.js",
                   "~/UIFramework/Progressive/js/jssorslider/jssor.slider.js",
                   "~/Content/Article/js/Detail.js"));

            bundles.Add(new Bundle("~/fo/angularjs").Include(
                    "~/UIFramework/Progressive/js/angular-isotope.js",
                    "~/UIFramework/Progressive/js/angular-ladda.min.js")
                    .IncludeDirectory("~/app", "*.js", true));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
