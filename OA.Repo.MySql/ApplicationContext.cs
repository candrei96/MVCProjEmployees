using Microsoft.EntityFrameworkCore;
using OA.Data.Entities;
using OA.Repo.MySql.Maps;

namespace OA.Repo.Classes
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new EmployeeMap(modelBuilder.Entity<Employee>()
                .Ignore(e => e.DepartmentNumber)
                .Ignore(e => e.FromDate)
                .Ignore(e => e.Title)
                );
            new DepartmentMap(modelBuilder.Entity<Department>()
                .Ignore(e => e.EmployeeNumber)
                .Ignore(e => e.FromDate)
                .Ignore(e => e.Title)
                );
            new TitleMap(modelBuilder.Entity<Title>()
                .Ignore(e => e.DepartmentNumber)
                );
            new SalaryMap(modelBuilder.Entity<Salary>()
                .Ignore(e => e.Title)
                .Ignore(e => e.DepartmentNumber)
                );
            new DepartmentEmployeeMap(modelBuilder.Entity<DepartmentEmployee>()
                .Ignore(e => e.Title)
                );
            new DepartmentManagerMap(modelBuilder.Entity<DepartmentManager>()
                .Ignore(e => e.Title)
                );
        }
    }
}
