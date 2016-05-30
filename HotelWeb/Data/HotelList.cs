using System.Collections.Generic;

namespace HotelWeb.Data
{
    public class HotelList
    {
        public IEnumerable<Hotel> Hotels { get; set; }
    }

    public class Hotel
    {
        public int Index { get; set; }
        public string Key { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int Stars { get; set; }
        public bool Pool { get; set; }
        public bool Bar { get; set; }
        public bool Restaurant { get; set; }
        public int DistanceToBeach { get; set; }
        public int DistanceToShopping { get; set; }
        public int PricePerNight { get; set; }
    }
}