using System.Web.Mvc;

namespace HotelWeb
{
    public class FeatureViewEngine : RazorViewEngine
    {
        public FeatureViewEngine()
        {
            ViewLocationFormats = new[]
                        {
                "~/Features/{1}/{0}.cshtml",
            };
            PartialViewLocationFormats = new[]
            {
                "~/Features/{1}/{0}.cshtml",
            };
        }
    }
}