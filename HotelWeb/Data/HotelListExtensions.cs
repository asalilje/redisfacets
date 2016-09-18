using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace HotelWeb.Data
{
    public static class HotelListExtensions
    {
        public static HotelList FilterByFacet(this HotelList hotelList, IEnumerable<string> options)
        {
            if (options == null || !options.Any())
                return hotelList;

            var facetFilterKey = $"{string.Join(":", options)}:{HotelList.HotelsKey}";
            var finalListKey = $"{hotelList.FinalListKey}_{facetFilterKey}:{HotelList.HotelsKey}";
            RedisClient.UnionAndStore(facetFilterKey, options.Select(x => $"{x}:{HotelList.HotelsKey}"));
            RedisClient.IntersectAndStore(finalListKey, new[] { facetFilterKey, hotelList.FinalListKey });
            hotelList.FinalListKey = finalListKey;
            return hotelList;
        }

        public static HotelList FilterByRange(this HotelList hotelList, string rangeMin, string rangeMax, string sortedSetKey)
        {
            if (string.IsNullOrWhiteSpace(rangeMin) && string.IsNullOrWhiteSpace(rangeMax))
                return hotelList;

            double min;
            double max;

            if (!double.TryParse(rangeMin, out min))
                min = double.MinValue;

            if (!double.TryParse(rangeMax, out max))
                max = double.MaxValue;

            var rangeFilterKey = $"{sortedSetKey}_{min}:{max}";
            var finalListKey = $"{hotelList.FinalListKey}_{rangeFilterKey}:{HotelList.HotelsKey}";
            RedisClient.UnionAndStoreSortedSet(rangeFilterKey, new [] { sortedSetKey });
            RedisClient.RemoveStartingRange(rangeFilterKey, min);
            RedisClient.RemoveEndingRange(rangeFilterKey, max);
            RedisClient.IntersectAndStoreSortedSet(finalListKey, new[] { rangeFilterKey, hotelList.FinalListKey });
            hotelList.FinalListKey = finalListKey;
            return hotelList;
        }

        public static HotelList SortAndGet(this HotelList hotelList, HotelListOptions options)
        {
            var hotels = RedisClient.GetSortedSet(hotelList.FinalListKey, options.SortBy.ToString(), options.SortOrder,
                options.SortType, options.Offset, options.Size);

            hotelList.Hotels = hotels.Select((x, i) =>
            {
                var hotel = JsonConvert.DeserializeObject<Hotel>(x);
                hotel.Index = i;
                return hotel;
            });
            hotelList.SortBy = options.SortBy;
            hotelList.SortOrder = options.SortOrder;
            hotelList.TotalCount = RedisClient.GetSetCount(hotelList.FinalListKey);
            return hotelList;
        }
        
    }
}