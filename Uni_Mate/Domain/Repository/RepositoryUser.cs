using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public async Task<bool> SaveIncludeAsync(Entity entity, params string[] properties)
    {

        try
        {
            var localEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);
            EntityEntry entry;

            if (localEntity is null)
            {
                _dbSet.Attach(entity);
                entry = _context.Entry(entity);
            }
            else
            {
                entry = _context.Entry(localEntity);
            }

            if (entry == null)
            {
                return false;
            }


            foreach (var property in entry.Properties)
            {
                if (properties.Contains(property.Metadata.Name))
                {
                    property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name)?.GetValue(entity);
                    property.IsModified = true;
                }
            }


            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
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
        return _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public  async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
    {
        return await Get(predicate).AnyAsync();
    }
    public Entity GetInclude(Expression<Func<Entity, bool>> predicate, params string[] properties)
    {
        var query = _dbSet.Where(predicate);

        foreach (var property in properties)
        {
            query = query.Include(property);
        }

        return query.FirstOrDefault();
    }

   
    public async Task<bool> UpdateAsync(Entity entity)
    {
        try
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id);
            if (existingEntity == null)
            {
                return false;
            }

            _dbSet.Update(entity);
            await SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}