using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Uni_Mate.Models;

namespace Uni_Mate.Domain.Repository
{
    public interface IRepository<Entity> where Entity : BaseEntity 
    {
        Task<DbSet<Entity>> Query();
        Task Add(Entity entity);
        Task<bool> SaveIncludeAsync(Entity entity, params string[] properties);
        //Task SaveInclude(Entity entity, params string[] properties);
        Task DeleteAsync(Entity entity);
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
        Task HardDeleteAsync(Entity entity);
        Task HardDeleteRangeAsync(ICollection<Entity> entity);
        Task SaveChangesAsync();
        /// <summary>
        /// Get Specific Entity With Include The Relation Like Get Apartment With it's Rooms
        /// </summary>
        /// <param name="id"> The Id </param>
        /// <param name="include">The ICollection Like ["Images","Rooms"]</param>
        /// <returns></returns>
        Task<Entity> GetWithIncludeAsync(int id , params string[] include);
    }
}
