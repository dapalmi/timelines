using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelines.Domain.Event;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp;
using Timelines.Domain.Person;
using Timelines.Domain.Place;
using Timelines.ViewModels;


namespace Timelines.Service
{
    public class PlaceService
    {
        private readonly PlaceRepository _placeRepository;

        public PlaceService(PlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public IEnumerable<Place> GetAll()
        {
            return _placeRepository.GetAll();
        }

        public Place ById(int id)
        {
            return _placeRepository
                .GetAll()
                .FirstOrDefault(p => p.Id == id);
        }

        public async Task<bool> Add(Place newPlace)
        {
            _placeRepository.Add(newPlace);
            return await _placeRepository.SaveChangesAsync();
        }

        public async Task<bool> Update(int id, Place place)
        {
            var oldPlace = _placeRepository
                .GetAll()
                .FirstOrDefault(p => p.Id == id);

            oldPlace.Name = place.Name;
            oldPlace.Latitude = place.Latitude;
            oldPlace.Longitude = place.Longitude;

            return await _placeRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var place = _placeRepository
                .GetAll()
                .FirstOrDefault(p => p.Id == id);

            _placeRepository.Remove(place);

            return await _placeRepository.SaveChangesAsync();
        }
    }
}
