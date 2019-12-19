using Microsoft.EntityFrameworkCore;
using OA.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OA.Repo.Classes
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext context;
        private readonly DbSet<T> entities;
        readonly string errorMessage = "Entity missing.";

        public Repository(ApplicationContext _context)
        {
            context = _context;
            entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public T GetOne(int id)
        {
            return entities.SingleOrDefault(s => s.EmployeeNumber == id);
        }

        public T GetOne(string id)
        {
            return entities.SingleOrDefault(s => s.DepartmentNumber == id);
        }

        public T GetOne(int numericId, string stringId)
        {
            return entities.SingleOrDefault(s => s.EmployeeNumber == numericId && s.DepartmentNumber.Equals(stringId));
        }

        public T GetOne(int numericId, DateTime dateId)
        {
            return entities.SingleOrDefault(s => s.EmployeeNumber == numericId && s.FromDate.Equals(dateId));
        }

        public T GetOne(int numericId, string stringId, DateTime dateId)
        {
            return entities.SingleOrDefault(s => s.EmployeeNumber == numericId && s.Title.Equals(stringId) && s.FromDate.Equals(dateId));
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(errorMessage);
            }

            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(errorMessage);
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(errorMessage);
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public void Remove(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(errorMessage);
            }
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IQueryable<T> ExecuteQuery(string query, params object[] parameters)
        {
            return entities.FromSqlRaw(query, parameters);
        }

        public IEnumerable<T> GetAll(int empNo)
        {
            return entities
                .Where(e => e.EmployeeNumber == empNo)
                .AsEnumerable();
        }

        public IEnumerable<T> GetAll(string deptNo)
        {
            return entities
                .Where(e => e.DepartmentNumber.Equals(deptNo))
                .AsEnumerable();
        }
    }
}
