﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;

namespace AcademicChatBot.DAL.Contract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PageResult<T>> GetAllDataByExpression(Expression<Func<T, bool>>? filter,
            int pageNumber, int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool isAscending = true,
            params Expression<Func<T, object>>[]? includes);

        Task<T> GetById(object id);

        Task<T?> GetByExpression(Expression<Func<T?, bool>> filter,
            params Expression<Func<T, object>>[]? includeProperties);
        Task<T?> GetFirstByExpression(Expression<Func<T?, bool>> filter,
            params Expression<Func<T, object>>[]? includeProperties);

        Task<T> Insert(T entity);

        Task<List<T>> InsertRange(IEnumerable<T> entities);

        Task<List<T>> DeleteRange(IEnumerable<T> entities);

        Task<T> Update(T entity);

        Task<List<T>> UpdateRange(IEnumerable<T> entities);

        Task<T?> DeleteById(object id);
    }
}
