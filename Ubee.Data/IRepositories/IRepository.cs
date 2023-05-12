﻿using System.Linq.Expressions;
using Ubee.Domain.Commons;

namespace Ubee.Data.IRepositories;

public interface IRepository<TEntity> where TEntity : Auditable
{
    ValueTask<TEntity> InsertAsync(TEntity entity);

    ValueTask<TEntity> UpdateAsync(TEntity entity);
    IQueryable<TEntity> SelectAll(Expression<Func<TEntity, bool>> expression = null, string[] includes = null);
    ValueTask<TEntity> SelectAsync(Expression<Func<TEntity, bool>> expression, string[] includes = null);
    ValueTask<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
    bool DeleteMany(Expression<Func<TEntity, bool>> expression);

    ValueTask SaveAsync();
}
