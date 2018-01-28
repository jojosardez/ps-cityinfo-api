using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository _repository;

        public CitiesController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            #region Using in memory data store

            // Using in memory data store
            //return Ok(CitiesDataStore.Current.Cities);

            #endregion


            // Using persistent data store
            var cityEntities = _repository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            //if (city == null)
            //    return NotFound();
            //
            //return Ok(city);

            #endregion


            // Using persistent data store
            var city = _repository.GetCity(id, includePointsOfInterest);

            if (city == null)
                return NotFound();

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}
