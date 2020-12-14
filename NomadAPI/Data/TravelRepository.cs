using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Data
{
    //TODO: make generic repository
    public class TravelRepository : ITravelRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TravelRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Travel
        public async Task<TravelDto> GetTravelAsync(int id)
        {
            var user = await _context.Travels
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();

            return await _context.Travels
                .Where(t => t.Id == id)
                .ProjectTo<TravelDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Travel> GetCheckTravelExist(int id)
        {
            return await _context.Travels
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Application> ApplicationExists(int userAppliedAdId, int travelId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.UserAppliedAdId == userAppliedAdId && a.TravelId == travelId);
        }

        public async Task<PagedList<TravelDto>> GetTravelsAsync(TravelParams travelParams, IList<string> roles)
        {
            IQueryable<Travel> query;
            if (roles.Contains("Admin"))
            {
                query = _context.Travels.AsQueryable();
            }
            else
            {
                query = _context.Travels.Where(x => x.Active).AsQueryable();
            }

            if (!String.IsNullOrEmpty(travelParams.CityName))
            {
                var travelsCities = _context.TravelCities.Where(x => x.City.Name == travelParams.CityName);

                query = query.Where(x =>
                        travelsCities.Any(p => p.TravelId == x.Id));
            }

            if (travelParams.TravelFromDate != null || travelParams.TravelToDate != null)
            {
                query = FilterTravelByDate(query, travelParams);
            }

            return await PagedList<TravelDto>.CreateAsync(query.ProjectTo<TravelDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                travelParams.PageNumber, travelParams.PageSize);
        }

        private IQueryable<Travel> FilterTravelByDate(IQueryable<Travel> query, TravelParams travelParams)
        {
            if (travelParams.TravelFromDate != null && travelParams.TravelToDate != null)
            {
                query = query.Where(x => x.TravelFromDate >= travelParams.TravelFromDate && x.TravelToDate <= travelParams.TravelToDate);
            }
            else if (travelParams.TravelFromDate != null)
            {
                query = query.Where(x => x.TravelFromDate >= travelParams.TravelFromDate);
            }
            else if (travelParams.TravelToDate != null)
            {
                query = query.Where(x => x.TravelToDate <= travelParams.TravelToDate);
            }

            return query;
        }

        public void Update(Travel travel)
        {
            _context.Entry(travel).State = EntityState.Modified;
        }

        public void RemoveTravel(int id)
        {
            var travelToRemove = _context.Travels.SingleOrDefault(t => t.Id == id);
            _context.Remove(_context.Travels.Remove(travelToRemove));
        }

        public void AddTravel(Travel travel)
        {
            _context.Travels.Add(travel);
        }

        //Application
        public void AddApplication(Application application)
        {
            _context.Applications.Add(application);
        }
        public void RemoveApplication(int travelId, int userAppliedAdId)
        {
            var applicationToRemove = _context.Applications.SingleOrDefault(t => t.TravelId == travelId && t.UserAppliedAdId == userAppliedAdId);
            _context.Applications.Remove(applicationToRemove);
        }
        public async Task<IEnumerable<ApplicationDto>> GetApplications(int travelId)
        {
            return await _context.Applications
                .Include(x => x.UserAppliedAd)
                .Where(t => t.TravelId == travelId)
                .ProjectTo<ApplicationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<Application> GetApplicationAsync(int travelId, int userAppliedAdId)
        {
            return await _context.Applications
                .SingleOrDefaultAsync(x => x.TravelId == travelId && x.UserAppliedAdId == userAppliedAdId);
        }

        //TravelCity
        //public void AddTravelCity(TravelCity travelCity)
        //{
        //    _context.TravelCities.Add(travelCity);
        //}

        public void RemoveTravelCity(TravelCity travelCityToRemove)
        {
            _context.TravelCities.Remove(travelCityToRemove);
        }
        public async Task<TravelCity> GetTravelCityAsync(int travelId, int cityId)
        {
            return await _context.TravelCities
                .SingleOrDefaultAsync(x => x.TravelId == travelId && x.CityId == cityId);
        }
    }
}