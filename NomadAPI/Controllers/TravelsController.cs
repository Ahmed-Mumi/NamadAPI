using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Extensions;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Controllers
{
    //[Authorize]
    public class TravelsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public TravelsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        //Travels
        [HttpPost]
        public async Task<ActionResult<TravelDto>> AddTravel(CreateTravelDto createTravelDto)
        {
            var userId = User.GetUserId();

            createTravelDto.UserId = userId;

            var travel = _mapper.Map<Travel>(createTravelDto);

            _unitOfWork.TravelRepository.AddTravel(travel);

            if (await _unitOfWork.Complete())
            {
                return Ok(_mapper.Map<TravelDto>(travel));
            }

            return BadRequest("Failed to save travel");
        }

        [HttpPut]
        public async Task<ActionResult> EditTravel(TravelUpdateDto travelUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (user.Id != travelUpdateDto.UserId)
                return BadRequest("This post does not belong to this user.");

            var travel = await _unitOfWork.TravelRepository.GetCheckTravelExist(travelUpdateDto.Id);

            _mapper.Map(travelUpdateDto, travel);

            _unitOfWork.TravelRepository.Update(travel);

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Cannot update travel");
        }

        [HttpGet("GetTravel/{id}")]
        public async Task<ActionResult<TravelDto>> GetTravel(int id)
        {
            return await _unitOfWork.TravelRepository.GetTravelAsync(id);
        }

        [HttpPost("ChangeActiveTravel/{id}")]
        public async Task<ActionResult> ChangeActiveTravel(int id)
        {
            var travelToChangeActive = await _unitOfWork.TravelRepository.GetCheckTravelExist(id);
            if (travelToChangeActive == null)
                return BadRequest("No such travel");

            travelToChangeActive.Active = !travelToChangeActive.Active;

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Cannot change travel active");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelDto>>> GetTravels(TravelParams travelParams)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var roles = _userManager.GetRolesAsync(user).Result;

            var travels = await _unitOfWork.TravelRepository.GetTravelsAsync(travelParams, roles);


            Response.AddPaginationHeader(travels.CurrentPage, travels.PageSize, travels.TotalCount, travels.TotalPages);

            return travels;
        }

        //Aplications
        [HttpPost("AddApplication/{travelId}")]
        public async Task<ActionResult<ApplicationDto>> AddApplication(int travelId)
        {
            var userAppliedAdId = User.GetUserId();

            var applicationExists = await _unitOfWork.TravelRepository.ApplicationExists(userAppliedAdId, travelId);
            if (applicationExists != null)
                return BadRequest("You already applied to this ad");

            var travel = await _unitOfWork.TravelRepository.GetCheckTravelExist(travelId);

            if (travel == null)
                return BadRequest("No such travel");

            if (travel.UserId == userAppliedAdId)
                return BadRequest("You cannot apply to your own ad");

            travel.NumberOfApplicants++;

            var application = new Application()
            {
                UserAppliedAdId = userAppliedAdId,
                AppliedDate = DateTime.UtcNow,
                TravelId = travelId
            };

            _unitOfWork.TravelRepository.AddApplication(application);

            //travel.Applications.Add(application);

            if (await _unitOfWork.Complete())
            {
                return Ok(_mapper.Map<ApplicationDto>(application));
            }

            return BadRequest();
        }

        [HttpDelete("MakeApplicationOfficial")]
        public async Task<ActionResult> MakeApplicationOfficial(int travelId, int userAppliedAdId)
        {
            var applicationToOfficial = await _unitOfWork.TravelRepository.GetApplicationAsync(travelId, userAppliedAdId);

            applicationToOfficial.Official = true;

            var travelToChangeActive = await _unitOfWork.TravelRepository.GetTravelAsync(applicationToOfficial.TravelId);

            travelToChangeActive.Active = false;

            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }

            return BadRequest("Cannot make application official");
        }

        [HttpDelete("RemoveApplication")]
        public async Task<ActionResult> RemoveApplication(int travelId, int userAppliedAdId)
        {
            var travel = await _unitOfWork.TravelRepository.GetCheckTravelExist(travelId);

            if (travel == null)
                return BadRequest("No such travel");

            var application = await _unitOfWork.TravelRepository.GetApplicationAsync(travelId, userAppliedAdId);
            if (application.Official)
            {
                travel.Active = true;
            }


            travel.NumberOfApplicants--;

            _unitOfWork.TravelRepository.RemoveApplication(travelId, userAppliedAdId);

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Cannot remove application");
        }

        [HttpGet("GetApplications/{travelId}")]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetApplications(int travelId)
        {
            var applications = await _unitOfWork.TravelRepository.GetApplications(travelId);

            return Ok(applications);
        }

        //TravelCity
        //[HttpPost("AddTravelCity")]
        //public async Task<ActionResult> AddTravelCity(int travelId, int cityId)
        //{
        //    var travel = await _unitOfWork.TravelRepository.GetCheckTravelExist(travelId);

        //    if (travel == null)
        //        return BadRequest("No such travel");

        //    var travelCityToAdd = new TravelCity()
        //    {
        //        TravelId = travel.Id,
        //        CityId = cityId
        //    };

        //    _unitOfWork.TravelRepository.AddTravelCity(travelCityToAdd);

        //    if (await _unitOfWork.Complete())
        //        return NoContent();

        //    return BadRequest("Cannot remove application");
        //}

        [HttpDelete("RemoveTravelCity")]
        public async Task<ActionResult> RemoveTravelCity(int travelId, int cityId)
        {
            var travelCityToRemove = await _unitOfWork.TravelRepository.GetTravelCityAsync(travelId, cityId);

            if (travelCityToRemove == null)
                return BadRequest("No such travelcity");

            _unitOfWork.TravelRepository.RemoveTravelCity(travelCityToRemove);

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Cannot remove travelcity");
        }
    }
}
