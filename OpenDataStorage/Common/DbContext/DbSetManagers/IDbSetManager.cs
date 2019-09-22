﻿using OpenDataStorageCore.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpenDataStorage.Common.DbContext.DbSetManagers
{
    public interface IDbSetManager<T> where T : BaseEntity
    {
        string TableName { get; }

        IQueryable<T> Entities { get; }

        IQueryable<T> GetQuery(Guid id, params Expression<Func<T, object>>[] includedPath);

        IQueryable<T> GetQueryWithAllDependencies(Guid id);
        
        IQueryable<T> GetAllQuery(params Expression<Func<T, object>>[] includedPath);

        IQueryable<T> GetAllQueryWithAllDependencies();

        Task<Guid> Create(T entity);

        Task Update(T entity);

        Task Delete(Guid id);
    }
}
