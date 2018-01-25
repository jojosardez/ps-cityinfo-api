using CityInfo.API.Models;
using System.Collections.Generic;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore(); // auto property assignment syntax (new in C# 6.0)
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto
                {
                    Id = 1,
                    Name = "Melbourne",
                    Description = "One of the most liveable cities in the world."
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Singapore",
                    Description = "One of the cities with the highest standard of living."
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Manila",
                    Description = "Capital city of the Philippines."
                }
            };
        }
    }
}
