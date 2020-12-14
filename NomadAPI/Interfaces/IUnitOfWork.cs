using System.Threading.Tasks;

namespace NomadAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IMessageRepository MessageRepository { get; }
        IReactionsRepository ReactionsRepository { get; }
        ITravelRepository TravelRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
