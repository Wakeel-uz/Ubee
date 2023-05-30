using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Ubee.Data.Contexts;
using Ubee.Data.IRepositories;
using Ubee.Domain.Commons;

namespace Ubee.Data.Repositories;
#pragma warning disable

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Auditable
{
    protected readonly AppDbContext appDbContext;
    protected readonly DbSet<TEntity> dbSet;

    public Repository(AppDbContext appDbContext, DbSet<TEntity> dbSet)
    {
        this.appDbContext = appDbContext;
        this.dbSet = dbSet;
    }
    public async ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await this.SelectAsync(expression);

        if(entity is null)
        {
            entity.IsDeleted = true;
            return true;
        }

        return false;
    }

    public bool DeleteMany(Expression<Func<TEntity, bool>> expression)
    {
        var entities = dbSet.Where(expression);
        if(entities is null)
        {
            foreach (var entity in entities)
                entity.IsDeleted = true;

            return true;
        }
        return false;
    }

    public async ValueTask<TEntity> InsertAsync(TEntity entity)
    {
        EntityEntry<TEntity> entry = await this.dbSet.AddAsync(entity);

        return entry.Entity;
    }

    public async ValueTask SaveAsync()
    {
        appDbContext.SaveChangesAsync();
    }

    public IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression = null, string[] includes = null)
    {
        IQueryable<TEntity> query = expression is null ? this.dbSet : this.dbSet.Where(expression);

        if(includes is not null)
        {
            foreach (string include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public async ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression, string[] includes = null)
        => await this.SelectAll(expression, includes).FirstOrDefaultAsync();
        
    public TEntity Update(TEntity entity)
    {
        EntityEntry<TEntity> entryentity = this.appDbContext.Update(entity);

        return entryentity.Entity;
    }
}
