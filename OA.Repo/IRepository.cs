using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Repo
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int empNo);
        IEnumerable<T> GetAll(string deptNo);
        T GetOne(int empNo);
        T GetOne(string id);
        T GetOne(int numericId, string stringId);
        T GetOne(int numericId, DateTime dateId);
        T GetOne(int numericId, string stringId, DateTime dateId);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
        IQueryable<T> ExecuteQuery(string query, params object[] parameters);
    }
}
