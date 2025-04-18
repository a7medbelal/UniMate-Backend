using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Models;

namespace Uni_Mate.Domain.Repository
{
    public interface IRepositoryIdentity<Entity> where Entity : class
    {
        Task<DbSet<Entity>> Query();
        Task<bool> SaveIncludeAsync(Entity entity, params string[] properties);
        //Task SaveInclude(Entity entity, params string[] properties);
        Task Delete(Entity entity);
        Task HardDelete(Entity entity);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllWithDeleted();
        IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
       // Task<Entity> GetByID(int id);

        Task<Entity> GetByIDAsync(int id);
        Task<int> AddAsync(Entity entity);
        Task AddRangeAsync(IEnumerable<Entity> entities);
        Task DeleteRangeAsync(ICollection<Entity> entities);
        Task SaveChangesAsync();

    }
}
