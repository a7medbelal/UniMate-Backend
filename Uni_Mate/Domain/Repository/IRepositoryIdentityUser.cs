using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Uni_Mate.Models;
using Uni_Mate.Models.UserManagment;

namespace Uni_Mate.Domain.Repository
{
    public interface IRepositoryIdentity<Entity> where Entity : User
    {
        Task<DbSet<Entity>> Query();
        Task<bool> SaveIncludeAsync(Entity entity, params string[] properties);
        //Task SaveInclude(Entity entity, params string[] properties);
        Task Delete(Entity entity);
        IQueryable<Entity> GetAll();
        IQueryable<Entity> GetAllWithDeleted();
        IQueryable<Entity> Get(Expression<Func<Entity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate);
        Task SaveChangesAsync();


    }
}
