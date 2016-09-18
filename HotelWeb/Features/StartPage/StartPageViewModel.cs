using System.Linq;
using HotelWeb.Data;

namespace HotelWeb.Features.StartPage
{
    public class StartPageViewModel
    {
        public CountryList CountryList { get; set; }
        public HotelList HotelList { get; set; }
        public Range PriceRange { get; set; }
        public Range BeachRange { get; set; }
        public Range ShoppingRange { get; set; }
        public ServiceList Services { get; set; }
        public StarList StarList { get; set; }

        public static StartPageViewModel Create(HotelListOptions options)
        {
            var hotelList = HotelList.Create(options);
            var model = new StartPageViewModel
            {
                HotelList = hotelList,
                CountryList = CountryList.Create(hotelList.Hotels.Select(x => x.CountryKey), options),
                Services = ServiceList.Create(hotelList.Hotels, options),
                StarList = StarList.Create(options),
                PriceRange = Range.Create("Price:Hotels", options.PriceMin, options.PriceMax),
                BeachRange = Range.Create("DistanceToBeach:Hotels", options.BeachMin, options.BeachMax),
                ShoppingRange = Range.Create("DistanceToShopping:Hotels", options.ShoppingMin, options.ShoppingMax),
            };
            return model;
        }
    }
}