namespace HotelWeb.Data
{
    public class Range
    {
        public string CurrentMin { get; set; }
        public string CurrentMax { get; set; }
        public string TotalCurrentMin { get; set; }
        public string TotalCurrentMax { get; set; }

        public static Range Create(string key, string currentMin, string currentMax)
        {
            var range = RedisClient.GetRange(key);
            range.CurrentMin = string.IsNullOrWhiteSpace(currentMin) ? range.TotalCurrentMin : currentMin;
            range.CurrentMax = string.IsNullOrWhiteSpace(currentMax) ? range.TotalCurrentMax : currentMax;
            return range;
        }
    }
}