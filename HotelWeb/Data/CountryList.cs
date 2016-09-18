using System.Collections.Generic;
using System.Linq;

namespace HotelWeb.Data
{
    public class CountryList
    {
        public IEnumerable<Country> Countries { get; set; }
        private static string Key => "Countries";

        public static CountryList Create(IEnumerable<string> countryKeys, HotelListOptions options)
        {
            var keys = RedisClient.GetSet(Key).ToList();
            var values = RedisClient.GetStrings(keys);
            var countries = keys.Zip(values, (k, v) => new Country {Key = k, Name = v, IsSelected = options.CountryFilters.Contains(k)}).ToList();

            var optionsWithoutCurrentFilter = HotelListOptions.Clone(options);
            optionsWithoutCurrentFilter.CountryFilters = null;

            var facetCounts = HotelList.FacetCount(optionsWithoutCurrentFilter, keys.Select(x => $"{x}:Hotels"));

            countries.ForEach(x => x.FacetCount = facetCounts.ContainsKey($"{x.Key}:Hotels") ? facetCounts[$"{x.Key}:Hotels"] : 0);
            
            return new CountryList {Countries = countries};
        }
    }
}