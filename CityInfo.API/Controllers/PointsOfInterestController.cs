﻿using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _repository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository repository)
        {
            _logger = logger;
            _mailService = mailService;
            _repository = repository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                #region Using in memory data store

                // Using in memory data store
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                //    return NotFound();
                //}

                //return Ok(city.PointsOfInterest);

                #endregion


                // Using persistent data store
                if (!_repository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var pointsOfInterestForCity = _repository.GetPointsOfInterestForCity(cityId);
                var pointsOfInterestForCityResults =
                    Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for the city with id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterest == null)
            //    return NotFound();

            //return Ok(pointOfInterest);

            #endregion


            // Using persistent data store
            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterest == null)
                return NotFound();

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var maxPointOfInterestId = CitiesDataStore.Current.Cities
            //    .SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            //var finalPointOfInterest = new PointOfInterestDto
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            //city.PointsOfInterest.Add(finalPointOfInterest);

            //return CreatedAtRoute(
            //    "GetPointOfInterest",
            //    new { cityId = cityId, id = finalPointOfInterest.Id },
            //    finalPointOfInterest);

            #endregion


            // Using persistent data store
            if (!_repository.CityExists(cityId))
                return NotFound();

            var finalPointOfInterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);
            _repository.AddPointOfInterestForCity(cityId, finalPointOfInterest);
            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            var createdPointOfInterestToReturn = Mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new {cityId = cityId, id = createdPointOfInterestToReturn.Id},
                createdPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (pointOfInterest.Description == pointOfInterest.Name)
                ModelState.AddModelError("Description", "The provided description should be different from the name.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;

            //return NoContent();

            #endregion


            // Using persistent data store
            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }

        /*
        Sample patch body:
            [
                {
    	            "op": "replace",
                    "path": "/name",
                    "value": "Patched - Intramuros."
                },
                    {
    	            "op": "replace",
                    "path": "/description",
                    "value": "Patched - Description."
                }
            ] 
        */
        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            //var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            //{
            //    Name = pointOfInterestFromStore.Name,
            //    Description = pointOfInterestFromStore.Description
            //};

            //patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            //    ModelState.AddModelError("Description", "The provided description should be different from the name.");
            //TryValidateModel(pointOfInterestToPatch);

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            //return NoContent();

            #endregion


            // Using persistent data store
            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            #region Using in memory data store

            // Using in memory data store
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //    return NotFound();

            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterestFromStore == null)
            //    return NotFound();

            //city.PointsOfInterest.Remove(pointOfInterestFromStore);

            #endregion


            // Using persistent data store
            if (!_repository.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            _repository.DeletePointOfInterest(pointOfInterestEntity);

            if (!_repository.Save())
                return StatusCode(500, "A problem happened while handling your request.");

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }
    }
}
