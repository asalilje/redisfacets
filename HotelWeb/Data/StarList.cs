using System.Collections.Generic;
using System.Linq;

namespace HotelWeb.Data
{
    public class StarList
    {
        public IEnumerable<Star> Stars { get; set; }

        public static StarList Create(HotelListOptions options)
        {
            var starValues = new [] {"1","2","3","4","5"}; 
            var stars = starValues.Select(x => new Star { Key = $"Stars:{x}", Rating = x, IsSelected = options.StarFilters.Contains($"Stars:{x}") }).ToList();

            var optionsWithoutCurrentFilter = HotelListOptions.Clone(options);
            optionsWithoutCurrentFilter.StarFilters = null;

            var facetCounts = HotelList.FacetCount(optionsWithoutCurrentFilter, stars.Select(x => $"{x.Key}:Hotels"));

            stars.ForEach(x => x.FacetCount = facetCounts.ContainsKey($"{x.Key}:Hotels") ? facetCounts[$"{x.Key}:Hotels"] : 0);

            return new StarList { Stars = stars };
        }
    }

    public class Star: FilterOption
    {
        public string Rating { get; set; }
    }
}