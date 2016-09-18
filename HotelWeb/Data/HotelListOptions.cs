namespace HotelWeb.Data
{
    public class HotelListOptions
    {
        public HotelListSortBy SortBy { get; set; } = HotelListSortBy.SortByName;
        public string SortOrder { get; set; } = "asc";
        public string[] CountryFilters { get; set; } = {};
        public string[] ServiceFilters { get; set; } = {};
        public long Size { get; set; } = long.MaxValue;
        public long Offset { get; set; } = long.MinValue;
        public string SortType => GetSortType();
        public string PriceMin { get; set; }
        public string PriceMax { get; set; }
        public string BeachMin { get; set; }
        public string BeachMax { get; set; }
        public string ShoppingMin { get; set; }
        public string ShoppingMax { get; set; }
        public string[] StarFilters { get; set; }

        private string GetSortType()
        {
            switch (SortBy)
            {
                case HotelListSortBy.SortByCountry:
                case HotelListSortBy.SortByName:
                    return "alphabetic";
                default:
                    return "numeric";
            }
        }

        public static HotelListOptions Clone(HotelListOptions options)
        {
            return new HotelListOptions
            {
                SortBy = options.SortBy,
                SortOrder = options.SortOrder,
                CountryFilters = options.CountryFilters,
                ServiceFilters = options.ServiceFilters,
                StarFilters = options.StarFilters,
                Size = options.Size,
                Offset = options.Offset,
                PriceMin = options.PriceMin,
                PriceMax = options.PriceMax,
                ShoppingMin = options.ShoppingMin,
                ShoppingMax = options.ShoppingMax,
                BeachMin = options.BeachMin,
                BeachMax = options.BeachMax,
            };
        }

        public enum HotelListSortBy
        {
            SortByName,
            SortByCountry,
            SortByPrice,
            SortByStars,
            SortByDistanceToBeach,
            SortByDistanceToShopping,
            SortByPool,
            SortByBar,
            SortByRestaurant
        }
    }
}