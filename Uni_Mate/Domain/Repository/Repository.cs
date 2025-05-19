using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Uni_Mate.Models;

namespace Uni_Mate.Domain.Repository
{
    public class Repository<Entity> : IRepository<Entity> where Entity : BaseEntity
    {
        protected Context _context;
        private readonly DbSet<Entity> _dbSet;
        private readonly string[] immutableProps = { nameof(BaseEntity.Id), nameof(BaseEntity.CreatedBy), nameof(BaseEntity.CreatedDate) };
        public Repository(Context context)
        {
            _context = context;
            _dbSet = _context.Set<Entity>();
        }

        public async Task<DbSet<Entity>> Query()
        {
            return  _dbSet;
        }
        public async Task Add(Entity entity)
        {
            entity.CreatedDate = DateTime.Now;
           await _dbSet.AddAsync(entity);
        }

        public async Task<int> AddAsync(Entity entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }
        //public async Task SaveInclude(Entity entity, params string[] properties)
        //{
        //    var local = _dbSet.Local.FindEntry(entity.ID) ?? _dbSet.Entry(entity);
        //    EntityEntry entry = null;

        //    if (local is null)
        //    {
        //        entry = _context.Entry(entity);
        //    }
        //    else
        //    {
        //        entry = _context.ChangeTracker.Entries<Entity>()
        //            .FirstOrDefault(x => x.Entity.ID == entity.ID);
        //    }

        //    foreach (var property in entry.Properties)
        //    {
        //        if (properties.Contains(property.Metadata.Name) && !immutableProps.Contains(property.Metadata.Name))
        //        {
        //            property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name).GetValue(entity);
        //            property.IsModified = true;
        //        }
        //    }
        //    entity.UpdatedDate = DateTime.Now;
        //    entry.Property(nameof(entity.UpdatedBy)).IsModified = true;
        //}


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
                    if (properties.Contains(property.Metadata.Name) && !immutableProps.Contains(property.Metadata.Name))
                    {
                        property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name)?.GetValue(entity);
                        property.IsModified = true;
                    }
                }

                entity.UpdatedDate = DateTime.UtcNow;
                entry.Property(nameof(entity.UpdatedBy)).IsModified = true;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task DeleteAsync(Entity entity)
        {
            entity.Deleted = true;
            await SaveIncludeAsync(entity, nameof(BaseEntity.Deleted));
        }

        public async Task HardDeleteAsync(Entity entity)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }


        public  IQueryable<Entity>Get(Expression<Func<Entity, bool>> predicate)
        {
            return  GetAll().Where(predicate);
        }

        public IQueryable<Entity> GetAll()
        {
            return _dbSet.Where(x => !x.Deleted);
        }

        public IQueryable<Entity> GetAllWithDeleted()
        {
            return _dbSet;
        }

        //public  Task<Entity> GetByID(int id)
        //{
        //    return  Get(x => x.ID == id).FirstOrDefault();
        //}

        public async Task<Entity> GetByIDAsync(int id)
        {
            return await Get(x => x.Id == id).FirstOrDefaultAsync();
        }

        //public void SaveChanges()
        //{
        //    _context.SaveChanges();
        //}

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await Get(predicate).AnyAsync();
        }
        public async Task AddRangeAsync(IEnumerable<Entity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task DeleteRangeAsync(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {

                entity.Deleted = true;
                await SaveIncludeAsync(entity, nameof(entity.Deleted));
            }
        }

        
        public async Task<Entity> GetWithIncludeAsync(int id, params string[] include)
        {
            IQueryable<Entity> query = _dbSet;

            foreach (var parm in include)
            {
                query = query.Include(parm);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
         
        //Task IRepository<Entity>.HardDelete(Entity entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
