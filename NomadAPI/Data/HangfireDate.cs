using NomadAPI.Interfaces;
using System;

namespace NomadAPI.Data
{
    public class HangfireDate : IHangfireDate
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public HangfireDate(DataContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public void DeactiveExpiredTravels()
        {
            var travels = _unitOfWork.TravelRepository.GetTravelsHangfire();
            var isAnythingChanged = false;
            foreach (var travel in travels)
            {
                if (travel.TravelToDate != null)
                {
                    if (IsDateSame(travel.TravelToDate) && travel.Active)
                    {
                        travel.Active = false;
                        isAnythingChanged = true;
                    }
                }
            }

            if (isAnythingChanged)
            {
                _unitOfWork.Complete();
            }
        }

        private bool IsDateSame(DateTime? travelToDate)
        {
            if (travelToDate.Value.Day == DateTime.UtcNow.Day &&
                travelToDate.Value.Month == DateTime.UtcNow.Month &&
                travelToDate.Value.Year == DateTime.UtcNow.Year)
                return true;
            return false;
        }
    }
}
