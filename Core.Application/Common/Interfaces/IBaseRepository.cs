﻿namespace Core.Application.Common.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync(PaginationParams pParams);
    Task<int> AddAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(int id);
    Task<int> GetTotalCountAsync(string searchBy);
    Task<int> AddListAsync(List<T> entity);

}
