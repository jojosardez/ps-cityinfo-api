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
                    Description = "One of the most liveable cities in the world.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Flinder's Street Railway Station",
                            Description = "Melbourne's iconic train station."
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Shrine of Remembrance",
                            Description = "A popular tourist spot in the city."
                        }
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Singapore",
                    Description = "One of the cities with the highest standard of living.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 3,
                            Name = "Merlion Park",
                            Description = "This is where the iconic Merlion statue is located."
                        },
                        new PointOfInterestDto
                        {
                            Id = 4,
                            Name = "Gardens by the Bay",
                            Description = "A garden in the city enclosed in huge glass domes."
                        }
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Manila",
                    Description = "Capital city of the Philippines.",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto
                        {
                            Id = 5,
                            Name = "Intramuros",
                            Description = "Known as the walled city, it was the former base of Spain's governor general to the Philippines."
                        },
                        new PointOfInterestDto
                        {
                            Id = 6,
                            Name = "Manila Bay",
                            Description = "Known for its perfect sunset."
                        }
                    }
                }
            };
        }
    }
}
