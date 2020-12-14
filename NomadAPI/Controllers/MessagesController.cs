using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Extensions;
using NomadAPI.Helpers;
using NomadAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var email = User.GetEmail();

            if (email == createMessageDto.RecipientEmail)
                return BadRequest("You cannot send messages to yourself");

            var sender = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            var recipient = await _unitOfWork.UserRepository.GetUserByEmailAsync(createMessageDto.RecipientEmail);

            if (recipient == null)
                return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderFullName = sender.FullName,
                RecipientFullName = recipient.FullName,
                COntent = createMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
                return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to save message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Email = User.GetEmail();


            var messagesFromRepo = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageDto>>(messagesFromRepo);

            Response.AddPaginationHeader(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        //[HttpGet("thread/{email}")]
        //public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string email)
        //{
        //    var currentEmail = User.GetEmail();

        //    return Ok(await _messageRepository.GetMessageThread(currentEmail, email));
        //}
    }
}
