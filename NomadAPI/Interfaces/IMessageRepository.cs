using NomadAPI.Dtos;
using NomadAPI.Entities;
using NomadAPI.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams message);
        //Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams message);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentEmail, string recipientEmail);
        //Task<bool> SaveAllAsync();
        Task<Chat> GetChat(int senderId, int recipientId);
        void AddChat(Chat chat);

    }
}
