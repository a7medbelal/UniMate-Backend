using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using Uni_Mate.Models.UserManagment;
namespace Uni_Mate.Domain.Repository;

public class RepositoryUser<Entity> : IRepositoryIdentity<Entity> where Entity : User
{
    private readonly Context _context;
    private readonly DbSet<Entity> _dbSet;
    public RepositoryUser(Context context)
    {
        _context = context;
        _dbSet = _context.Set<Entity>();
    }
    public Task<DbSet<Entity>> Query()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveIncludeAsync(Entity entity, params string[] properties)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Entity entity)
    {
        throw new NotImplementedException();
    }

    public Task HardDelete(User entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Entity> GetAll()
    {
        return  _dbSet.Where(x => !x.IsActive);
    }

    public IQueryable<Entity> GetAllWithDeleted()
    {
        throw new NotImplementedException();
    }

    public IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate)
    {
        return  GetAll().Where(predicate);
    }
    public Task<Entity> GetByIDAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
    {
        return await Get(predicate).AnyAsync();
    }
}