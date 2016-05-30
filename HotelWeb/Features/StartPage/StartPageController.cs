using System;
using System.Web.Mvc;
using HotelWeb.Data;

namespace HotelWeb.Features.StartPage
{
    public class StartPageController: Controller
    {
        private readonly RedisStore _redisStore;

        public StartPageController()
        {
            _redisStore = new RedisStore();
        }

        public ActionResult Index()
        {
            var model =  new StartPageViewModel
            {
                CountryList = _redisStore.GetCountries(),
                HotelList = GetHotels(),
            };
            return View("StartPage", model);
        }

        public ActionResult Hotels(string sortBy = "", string sortOrder = "", string filters = "", int priceMin = int.MinValue, int priceMax = int.MaxValue)
        {
            var model = GetHotels(sortBy, sortOrder, filters, priceMin, priceMax);
            return PartialView("~/Features/StartPage/_HotelList.cshtml", model);
        }

      
        private HotelList GetHotels(string sortBy = "SortByName", string sortOrder = "asc", string filters = "", int priceMin = int.MinValue, int priceMax = int.MaxValue)
        {
            return _redisStore.GetHotels(sortBy, sortOrder, filters, priceMin, priceMax);
        }

    
    }
}