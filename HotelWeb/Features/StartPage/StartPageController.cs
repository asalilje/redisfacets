using System;
using System.Diagnostics;
using System.Timers;
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
            var model = StartPageViewModel.Create(_redisStore.GetCountries(), GetHotels(), "SortByName", "asc");
            return View("~/Features/StartPage/StartPage.cshtml", model);
        }

        public ActionResult Hotels(string sortBy = "", string sortOrder = "", string countryFilters = "", int priceMin = int.MinValue, int priceMax = int.MaxValue)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var hotelList = GetHotels(sortBy, sortOrder, countryFilters, priceMin, priceMax);
            var model = StartPageViewModel.Create(_redisStore.GetCountries(), hotelList, sortBy, sortOrder);
            stopWatch.Stop();
            Trace.WriteLine("======= |||||||||||| Hotels, elapsed: " + stopWatch.ElapsedMilliseconds);
            return PartialView("~/Features/StartPage/_HotelList.cshtml", model);
        }

      
        private HotelList GetHotels(string sortBy = "SortByName", string sortOrder = "asc", string countryFilters = "", int priceMin = int.MinValue, int priceMax = int.MaxValue)
        {
            var hotels = _redisStore.GetHotels(sortBy, sortOrder, countryFilters, priceMin, priceMax);
            return hotels;
        }

    
    }
}