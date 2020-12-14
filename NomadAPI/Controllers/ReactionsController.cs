using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Extensions;
using NomadAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Controllers
{
    [Authorize]
    public class ReactionsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReactionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{userReceivedId}")]
        public async Task<ActionResult> AddReaction(int userReceivedId)
        {
            var reactedUserId = User.GetUserId();
            var reactedByUser = await _unitOfWork.UserRepository.GetUserByIdAsync(userReceivedId);
            var reactedUser = await _unitOfWork.ReactionsRepository.GetUserWithReactions(reactedUserId);

            if (reactedByUser == null)
                return NotFound();

            var userReaction = await _unitOfWork.ReactionsRepository.GetUserReaction(reactedUserId, reactedByUser.Id);

            if (userReaction != null)
                return BadRequest("You already liked this user");

            userReaction = new UserReaction
            {
                ReactedByUserId = reactedByUser.Id,
                ReactedUserId = reactedUserId
            };

            reactedUser.ReactedUsers.Add(userReaction);


            if (await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to react to user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReactionDto>>> GetUsersReacted()
        {
            var users = await _unitOfWork.ReactionsRepository.GetUsersReacted(User.GetUserId());
            return Ok(users);
        }
    }
}
