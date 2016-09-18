using System;
using System.Linq;
using System.Web.Mvc;
using HotelWeb.Data;

namespace HotelWeb.Features.StartPage
{
    public class StartPageController: Controller
    {

        public ActionResult Index()
        {
            var model = GetModel("SortByName", "asc");
            return View("~/Features/StartPage/StartPage.cshtml", model);
        }

        public ActionResult Hotels(string sortBy = "", string sortOrder = "", string countryFilters = "", string serviceFilters = "", string priceMin = "", string priceMax = "",
             string beachMin = "", string beachMax = "", string shoppingMin = "", string shoppingMax = "", string starFilters = "")
        {
            var model = GetModel(sortBy, sortOrder, countryFilters, serviceFilters, priceMin, priceMax, beachMin, beachMax, shoppingMin, shoppingMax, starFilters);
            return PartialView("~/Features/StartPage/_HotelList.cshtml", model);
        }

        private StartPageViewModel GetModel(string sortBy = "", string sortOrder = "", string countryFilters = "", string serviceFilters = "", string priceMin = "", string priceMax = "",
            string beachMin = "", string beachMax = "", string shoppingMin = "", string shoppingMax = "", string starFilters = "")
        {
            var hotelListOptions = new HotelListOptions
            {
                SortBy = Enum.GetNames(typeof (HotelListOptions.HotelListSortBy)).Contains(sortBy)
                    ? (HotelListOptions.HotelListSortBy) Enum.Parse(typeof (HotelListOptions.HotelListSortBy), sortBy)
                    : HotelListOptions.HotelListSortBy.SortByName,
                SortOrder = sortOrder,
                CountryFilters = countryFilters.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries), 
                ServiceFilters = serviceFilters.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries),
                PriceMin = priceMin,
                PriceMax = priceMax,
                BeachMin = beachMin,
                BeachMax = beachMax,
                ShoppingMin = shoppingMin,
                ShoppingMax = shoppingMax,
                StarFilters = starFilters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
            };

            var model = StartPageViewModel.Create(hotelListOptions);
            return model;
        }
     
    }
}