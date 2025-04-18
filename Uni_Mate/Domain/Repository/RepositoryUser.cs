using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Uni_Mate.Models.UserManagment;
namespace Uni_Mate.Domain.Repository;

public class RepositoryUser : IRepositoryIdentity<User>
{
    public Task<DbSet<User>> Query()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveIncludeAsync(User entity, params string[] properties)
    {
        throw new NotImplementedException();
    }

    public Task Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public Task HardDelete(User entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetAllWithDeleted()
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> Get(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<User> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(ICollection<User> entities)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}