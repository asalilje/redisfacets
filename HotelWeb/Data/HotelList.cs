using System.Collections.Generic;
using System.Linq;

namespace HotelWeb.Data
{
    public class HotelList
    {
        public IEnumerable<Hotel> Hotels { get; set; }
        public static string HotelsKey => "Hotels";
        public string FinalListKey { get; set; } = HotelsKey;
        public long TotalCount { get; set; }
        public HotelListOptions.HotelListSortBy SortBy { get; set; }
        public string SortOrder { get; set; }


        public static HotelList Create(HotelListOptions options)
        {
            var filteredList = CreateHotelList(options);
            return filteredList.SortAndGet(options);
        }

        public static Dictionary<string, long> FacetCount(HotelListOptions options, IEnumerable<string> facetKeys)
        {
            var filteredList = CreateHotelList(options);
            var facets = new Dictionary<string, long>();
            facetKeys.ToList().ForEach(x => facets.Add(x, RedisClient.IntersectSortedSet($"{x}_{filteredList.FinalListKey}", new [] {x, filteredList.FinalListKey}).Count()));
            return facets;
        }

        private static HotelList CreateHotelList(HotelListOptions options)
        {
            return new HotelList()
                .FilterByFacet(options.CountryFilters)
                .FilterByFacet(options.ServiceFilters.Where(x => x.StartsWith("Restaurant:")))
                .FilterByFacet(options.ServiceFilters.Where(x => x.StartsWith("Bar:")))
                .FilterByFacet(options.ServiceFilters.Where(x => x.StartsWith("Pool:")))
                .FilterByFacet(options.StarFilters)
                .FilterByRange(options.PriceMin, options.PriceMax, "Price:Hotels")
                .FilterByRange(options.BeachMin, options.BeachMax, "DistanceToBeach:Hotels")
                .FilterByRange(options.ShoppingMin, options.ShoppingMax, "DistanceToShopping:Hotels");
        }

        public string GetSortOrder(string sortBy)
        {
            return sortBy.ToLower() == SortBy.ToString().ToLower() ? SortOrder.ToLower() : "";
        }

    }
}