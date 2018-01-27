using CityInfo.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            // Check if the database has records
            if (context.Cities.Any())
                return;

            // Initialize seed data
            var cities = new List<City>()
            {
                new City
                {
                    Name = "Melbourne",
                    Description = "One of the most liveable cities in the world.",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest
                        {
                            Name = "Flinder's Street Railway Station",
                            Description = "Melbourne's iconic train station."
                        },
                        new PointOfInterest
                        {
                            Name = "Shrine of Remembrance",
                            Description = "A popular tourist spot in the city."
                        }
                    }
                },
                new City
                {
                    Name = "Singapore",
                    Description = "One of the cities with the highest standard of living.",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest
                        {
                            Name = "Merlion Park",
                            Description = "This is where the iconic Merlion statue is located."
                        },
                        new PointOfInterest
                        {
                            Name = "Gardens by the Bay",
                            Description = "A garden in the city enclosed in huge glass domes."
                        }
                    }
                },
                new City
                {
                    Name = "Manila",
                    Description = "Capital city of the Philippines.",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest
                        {
                            Name = "Intramuros",
                            Description =
                                "Known as the walled city, it was the former base of Spain's governor general to the Philippines."
                        },
                        new PointOfInterest
                        {
                            Name = "Manila Bay",
                            Description = "Known for its perfect sunset."
                        }
                    }
                }
            };

            // Seed the database
            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
