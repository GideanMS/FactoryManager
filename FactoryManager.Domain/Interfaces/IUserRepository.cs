using FactoryManager.Domain.Entities;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task SaveAsync(User user);
}