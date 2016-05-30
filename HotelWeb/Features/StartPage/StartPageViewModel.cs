using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HotelWeb.Data;
using HotelWeb.Data.HotelWeb.Data;

namespace HotelWeb.Features.StartPage
{
    public class StartPageViewModel
    {
        public CountryListViewModel CountryList { get; set; }
        public HotelListViewModel HotelList { get; set; }
        public RangeViewModel PriceRange { get; set; }

        public static StartPageViewModel Create(CountryList countries, HotelList hotels, string sortBy, string sortOrder)
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new StartPageViewModel
            {
                HotelList = HotelListViewModel.Create(hotels, sortBy, sortOrder),
                CountryList = CountryListViewModel.Create(countries, hotels),
                PriceRange = RangeViewModel.Create(hotels, hotel => hotel.PricePerNight),
            };
            sw.Stop();
            Trace.WriteLine("StartPageViewModel " + sw.ElapsedMilliseconds);
            return model;
        }
    }

    public class RangeViewModel
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public static RangeViewModel Create(HotelList hotels, Func<Hotel, int> selector)
        {
            return new RangeViewModel
            {
                Min = hotels.Hotels.Min(selector),
                Max = hotels.Hotels.Max(selector),
            };
        }
    }

    public class HotelListViewModel
    {
        public IEnumerable<Hotel> Hotels { get; set; }
        public string SortOrder { get; set; }
        public string SortBy { get; set; }

        public static HotelListViewModel Create(HotelList hotels, string sortBy, string sortOrder)
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new HotelListViewModel
            {
                SortBy = sortBy,
                SortOrder = sortOrder,
                Hotels = hotels.Hotels,
            };
            sw.Stop();
            Trace.WriteLine("HotelListViewModel " + sw.ElapsedMilliseconds);
            return model;
        }

        public string GetSortOrder(string sortBy)
        {
            return sortBy == SortBy ? SortOrder.ToLower() : "";
        }
    }

    public class CountryListViewModel
    {
        public IEnumerable<CountryViewModel> Countries { get; set; }
      
        public static CountryListViewModel Create(CountryList countries, HotelList hotels)
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new CountryListViewModel
            {
                Countries = countries.Countries.Select(x => CountryViewModel.Create(x, hotels)),
            };
            sw.Stop();
            Trace.WriteLine("CountryListViewModel " + sw.ElapsedMilliseconds);
            return model;
        }
        
    }

    public class CountryViewModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public int Count { get; set; }

        public static CountryViewModel Create(Country country, HotelList hotels)
        {
            var sw = new Stopwatch();
            sw.Start();
            var model = new CountryViewModel
            {
                Key = country.Key,
                Name = country.Name,
            };
            int countryId = 0;
            if (int.TryParse(country.Key.Substring(country.Key.LastIndexOf(":") + 1), out countryId))
            {
                model.IsActive = hotels.Hotels.Any(x => x.CountryId == countryId).ToString().ToLower();
            }
            sw.Stop();
            Trace.WriteLine("CountryViewModel " + sw.ElapsedMilliseconds);
            return model;
        }
    }
}