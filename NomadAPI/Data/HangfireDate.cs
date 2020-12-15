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
                if (travel.TravelFromDate != null)
                {
                    if (IsDateSame(travel.TravelFromDate) && travel.Active)
                    {
                        travel.Active = false;
                        isAnythingChanged = true;
                    }
                }
            }

            if (isAnythingChanged)
            {
                _context.SaveChanges();
            }
        }

        private bool IsDateSame(DateTime? travelFromDate)
        {
            if (travelFromDate.Value.Day == DateTime.Now.Day &&
                travelFromDate.Value.Month == DateTime.Now.Month &&
                travelFromDate.Value.Year == DateTime.Now.Year)
                return true;
            return false;
        }
    }
}
