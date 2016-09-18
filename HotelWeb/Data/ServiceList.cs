using System.Collections.Generic;
using System.Linq;

namespace HotelWeb.Data
{
    public class ServiceList
    {
        public IEnumerable<Service> Services { get; set; }

        public static ServiceList Create(IEnumerable<Hotel> hotels, HotelListOptions options)
        {
         
            var barFacetCounts = HotelList.FacetCount(HotelOptionsWithoutCurrentService("Bar:", options), new[] {
                "Bar:False:Hotels",
                "Bar:True:Hotels",
            });

            var restaurantFacetCounts = HotelList.FacetCount(HotelOptionsWithoutCurrentService("Restaurant:", options), new[] {
                "Restaurant:False:Hotels",
                "Restaurant:True:Hotels",
            });

            var poolFacetCounts = HotelList.FacetCount(HotelOptionsWithoutCurrentService("Pool:", options), new[] {
                "Pool:False:Hotels",
                "Pool:True:Hotels",
            });

            var services = new ServiceList
            {
                Services = new List<Service>()
                {
                   new Service{ Key = "Bar:False", Name = "Bar", Value = false, IsSelected = options.ServiceFilters.Contains("Bar:False"), FacetCount = barFacetCounts.ContainsKey("Bar:False:Hotels") ? barFacetCounts["Bar:False:Hotels"] : 0},
                   new Service{ Key = "Bar:True", Name = "Bar", Value = true, IsSelected = options.ServiceFilters.Contains("Bar:True"), FacetCount = barFacetCounts.ContainsKey("Bar:True:Hotels") ? barFacetCounts["Bar:True:Hotels"] : 0},
                   new Service{ Key = "Restaurant:False", Name = "Restaurant", Value = false, IsSelected = options.ServiceFilters.Contains("Restaurant:False"), FacetCount = restaurantFacetCounts.ContainsKey("Restaurant:False:Hotels") ? restaurantFacetCounts["Restaurant:False:Hotels"] : 0},
                   new Service{ Key = "Restaurant:True", Name = "Restaurant", Value = true, IsSelected = options.ServiceFilters.Contains("Restaurant:True"), FacetCount = restaurantFacetCounts.ContainsKey("Restaurant:True:Hotels") ? restaurantFacetCounts["Restaurant:True:Hotels"] : 0},
                   new Service{ Key = "Pool:False", Name = "Pool", Value = false, IsSelected = options.ServiceFilters.Contains("Pool:False"), FacetCount = poolFacetCounts.ContainsKey("Pool:False:Hotels") ? poolFacetCounts["Pool:False:Hotels"] : 0},
                   new Service{ Key = "Pool:True", Name = "Pool", Value = true, IsSelected = options.ServiceFilters.Contains("Pool:True"), FacetCount = poolFacetCounts.ContainsKey("Pool:True:Hotels") ? poolFacetCounts["Pool:True:Hotels"] : 0},
                }
            };

            return services;
        }

        private static HotelListOptions HotelOptionsWithoutCurrentService(string serviceName, HotelListOptions options)
        {
            var clonedOptions = HotelListOptions.Clone(options);
            clonedOptions.ServiceFilters = options.ServiceFilters.Where(x => !x.StartsWith(serviceName)).ToArray();
            return clonedOptions;
        }
    }
}