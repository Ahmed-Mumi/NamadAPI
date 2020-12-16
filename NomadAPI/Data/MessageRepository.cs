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
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        //public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .OrderByDescending(m => m.MessageSent).AsQueryable();

            query = query.Where(u => u.Recipient.Email == messageParams.Email || u.Sender.Email == messageParams.Email)
                .OrderByDescending(d => d.MessageSent);

            var chats = await _context.Chats
                .Where(u => u.RecipientId == messageParams.Id || u.SenderId == messageParams.Id)
                .ToListAsync();

            var chatMessage = new List<Message>();

            foreach (var chat in chats)
            {
                var messageToInsert = query.Where(m => (m.RecipientId == chat.SenderId && m.SenderId == chat.RecipientId)
                    || (m.RecipientId == chat.RecipientId && m.SenderId == chat.SenderId)).FirstOrDefault();
                chatMessage.Add(messageToInsert);
            }

            var queryableMessages = chatMessage.AsQueryable();


            //var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);


            return await PagedList<Message>.CreateAsync(queryableMessages, messageParams.PageNumber, messageParams.PageSize);
            //return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentEmail, string recipientEmail)
        {
            var messages = await _context.Messages
                //.Include(u => u.Sender).ThenInclude(p => p.Photos)
                //.Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m => (m.Recipient.Email == currentEmail && m.Sender.Email == recipientEmail) ||
                    (m.Recipient.Email == recipientEmail && m.Sender.Email == currentEmail))
                .OrderBy(m => m.MessageSent)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientEmail == currentEmail).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
                //await _context.SaveChangesAsync();
            }

            //return _mapper.Map<IEnumerable<MessageDto>>(messages);
            return messages;

        }

        public async Task<Chat> GetChat(int senderId, int recipientId)
        {
            return await _context.Chats.FirstOrDefaultAsync(m => (m.RecipientId == senderId && m.SenderId == recipientId)
                    || (m.RecipientId == recipientId && m.SenderId == senderId)); ;
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public void AddChat(Chat chat)
        {
            _context.Chats.Add(chat);
        }
        //public async Task<bool> SaveAllAsync()
        //{
        //    return await _context.SaveChangesAsync() > 0;
        //}
    }
}
