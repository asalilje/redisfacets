﻿using System.Collections.Generic;

namespace HotelWeb.Data
{
    namespace HotelWeb.Data
    {
        public class CountryList
        {
            public IList<Country> Countries { get; set; }
        }

        public class Country
        {
            public string Key { get; set; }
            public string Name { get; set; }
        }
    }
}