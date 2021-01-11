using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface ITravelRepository
    {
        Task<TravelDto> GetTravelAsync(int id);
        Task<PagedList<TravelDto>> GetTravelsAsync(TravelParams travelParams, IList<string> roles);
        void Update(Travel travel);
        void AddTravel(Travel travel);
        Task<Travel> GetCheckTravelExist(int id);
        void AddApplication(Application application);
        void RemoveApplication(int travelId, int userAppliedAdId);
        Task<IEnumerable<ApplicationDto>> GetApplications(int travelId);
        Task<Application> ApplicationExists(int userAppliedAdId, int travelId);
        Task<Application> GetApplicationAsync(int travelId, int userAppliedAdId);
        //void AddTravelCity(TravelCity travelCity);
        void RemoveTravelCity(TravelCity travelCityToRemove);
        Task<TravelCity> GetTravelCityAsync(int travelId, int cityId);
        IQueryable<Travel> GetTravelsHangfire();
        void RemoveTravel(int id);
    }
}
