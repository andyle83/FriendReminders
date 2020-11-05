using System.Threading.Tasks;

namespace RemindersManagement.API.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRemindersRepository RemindersRepository { get; }
        ICategoriesRepository ListsRepository { get; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}