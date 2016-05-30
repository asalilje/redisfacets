using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HotelWeb.Data.HotelWeb.Data;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HotelWeb.Data
{
    public class RedisStore
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisStore()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect("machost");
        }

        public CountryList GetCountries()
        {
            var db = _connectionMultiplexer.GetDatabase();
            var countryList = new CountryList
            {
                Countries = new List<Country>(),
            };
            var countryKeys = db.Sort("Countries", by: "SortByName_*", sortType: SortType.Alphabetic);
            foreach (var key in countryKeys)
            {
                var country = db.StringGet(key.ToString());
                countryList.Countries.Add(new Country() { Key = key, Name = country });
            }
            return countryList;
        }

        public HotelList GetHotels(string sortBy = "SortByName", string sortOrder = "asc", string filters = "", int priceMin = int.MinValue, int priceMax = int.MaxValue)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var listKey = $"Hotels";
            RedisValue[] finalHotelList;

            var sortType = sortBy.Contains("Name") || sortBy.Contains("Country")
                ? SortType.Alphabetic
                : SortType.Numeric;

            var orderType = sortOrder == "asc" ? Order.Ascending : Order.Descending;

            if (!string.IsNullOrWhiteSpace(filters))
            {
                var filterArray = filters.Split(',');
                if (filterArray.Length > 1)
                {
                    var redisKeys = filterArray.Select(filter => (RedisKey)$"{filter}:Hotels");
                    listKey = $"{string.Join(":", filterArray)}:Hotels";
                    db.SetCombineAndStore(SetOperation.Union, listKey, redisKeys.ToArray());
                }
                else
                {
                    listKey = $"{filterArray[0]}:Hotels";
                }
            }

            if (priceMin > int.MinValue || priceMax < int.MaxValue)
            {
                var intervalPriceKey = $"Price:Interval:{priceMin}:{priceMax}";
                db.SortedSetCombineAndStore(SetOperation.Union, intervalPriceKey, "Price", "");
                db.SortedSetRemoveRangeByScore(intervalPriceKey, int.MinValue, priceMin, Exclude.Stop);
                db.SortedSetRemoveRangeByScore(intervalPriceKey, priceMax, int.MaxValue);
                db.SortedSetCombineAndStore(SetOperation.Intersect, $"{intervalPriceKey}:Hotels", intervalPriceKey, listKey, Aggregate.Max);
                listKey = $"{intervalPriceKey}:Hotels";
            }
            
            finalHotelList = db.Sort(listKey, by: $"{sortBy}_*", sortType: sortType, order: orderType);

            var hotels = finalHotelList.ToList().Select((x, i) =>
            {
                var hotelJson = db.StringGet(x.ToString());
                var hotel = Deserialize<Hotel>(hotelJson);
                hotel.Index = i;
                hotel.CountryName = db.StringGet($"Countries:{hotel.CountryId}");
                return hotel;
            });

            var hotelList = new HotelList
            {
                Hotels = hotels,
            }; 

            return hotelList;
        }

        private TResult Deserialize<TResult>(string jsonString) where TResult : class
        {
            return JsonConvert.DeserializeObject<TResult>(jsonString);
        }
    }
}