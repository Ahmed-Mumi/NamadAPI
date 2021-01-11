using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Extensions;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NomadAPI.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpPost("{email}")]
        public async Task<ActionResult> DeActivateUser(string email)
        {
            var userToDeactivate = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);

            if (email == null)
                return BadRequest("User does not exist");

            userToDeactivate.IsActive = !userToDeactivate.IsActive;

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Failed to DeActivate user");
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<NomadDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserEmail = User.GetEmail();

            var users = await _unitOfWork.UserRepository.GetNomadsAsync(userParams);


            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }


        [HttpGet("{email}", Name = "GetUser")]
        public async Task<ActionResult<NomadDto>> GetUser(string email)
        {
            return await _unitOfWork.UserRepository.GetNomadAsync(email);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(NomadUpdateDto nomadUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(User.GetEmail());

            _mapper.Map(nomadUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Failed to update user");
        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(User.GetEmail());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUser", new { email = user.Email }, _mapper.Map<PhotoDto>(photo));

            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(User.GetEmail());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain)
                return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null)
            {
                currentMain.IsMain = false;
            }

            photo.IsMain = true;

            if (await _unitOfWork.Complete())
                return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(User.GetEmail());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null)
                return NotFound();

            if (photo.IsMain)
                return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Failed to delete a photo");
        }

        [HttpPost("AddReport")]
        public async Task<ActionResult<ReportDto>> AddReport(ReportDto reportDto)
        {
            var userReports = User.GetUserId();

            reportDto.UserReportsId = userReports;

            var userReported = _unitOfWork.UserRepository.GetUserByIdAsync(reportDto.UserReportedId);

            if (userReported == null)
                return BadRequest("No such user");

            var reportToAdd = _mapper.Map<Report>(reportDto);

            _unitOfWork.UserRepository.AddReport(reportToAdd);

            if (await _unitOfWork.Complete())
            {
                return Ok(reportDto);
            }

            return BadRequest("Cannot add report");
        }

        [HttpGet("GetReport/{id}")]
        public async Task<ActionResult<ReportDto>> GetReport(int id)
        {
            return await _unitOfWork.UserRepository.GetReport(id);
        }

        [HttpGet("GetReports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetTravels(ReportParams reportParams)
        {
            var reports = await _unitOfWork.UserRepository.GetReportsAsync(reportParams);

            Response.AddPaginationHeader(reports.CurrentPage, reports.PageSize, reports.TotalCount, reports.TotalPages);

            return reports;
        }

        [HttpPost("SendFriendship/{userReceivedRequestId}")]
        public async Task<ActionResult> SendFriendship(int userReceivedRequestId)
        {
            var userSentRequestId = User.GetUserId();

            var userReceivedRequest = await _unitOfWork.UserRepository.GetUserByIdAsync(userReceivedRequestId);

            if (userReceivedRequest == null)
            {
                return BadRequest("No such user");
            }

            var friendship = new Friendship()
            {
                FriendshipStatusId = FriendshipStatusHelper.Pending,
                UserReceivedRequestId = userReceivedRequest.Id,
                UserSentRequestId = userSentRequestId,
            };

            _unitOfWork.UserRepository.AddFriendship(friendship);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Cannot add friendship");
        }

        [HttpPut("AcceptFriendship")]
        public async Task<ActionResult> AcceptFriendship(int userSentRequestId, int userReceivedRequestId)
        {
            var friendship = await _unitOfWork.UserRepository.GetFriendship(userSentRequestId, userReceivedRequestId);

            if (friendship == null)
            {
                return BadRequest("Friendship does not exist");
            }

            friendship.ConfirmedFriendshipDate = DateTime.UtcNow;
            friendship.FriendshipStatusId = FriendshipStatusHelper.Accepted;

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Cannot accept friendship");
        }

        [HttpPut("RejectFriendship")]
        public async Task<ActionResult> RejectFriendship(int userSentRequestId, int userReceivedRequestId)
        {
            var friendship = await _unitOfWork.UserRepository.GetFriendship(userSentRequestId, userReceivedRequestId);

            if (friendship == null)
            {
                return BadRequest("Friendship does not exist");
            }

            if (friendship.FriendshipStatusId == FriendshipStatusHelper.Accepted)
            {
                return BadRequest("Friendship already accepted");
            }

            friendship.FriendshipStatusId = FriendshipStatusHelper.Rejected;

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Cannot accept friendship");
        }
    }
}
