using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Entities;

namespace OA.Repo.MySql.Maps
{
    public class SalaryMap
    {
        public SalaryMap(EntityTypeBuilder<Salary> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("salaries");
            entityTypeBuilder.HasKey(prop => new { prop.EmployeeNumber, prop.FromDate });
            entityTypeBuilder
                .Property(prop => prop.ToDate)
                .HasColumnName("to_date")
                .HasColumnType("date");
            entityTypeBuilder
                .Property(prop => prop.FromDate)
                .HasColumnName("from_date")
                .HasColumnType("date");
            entityTypeBuilder
                .Property(prop => prop.EmployeeNumber)
                .HasColumnName("emp_no")
                .HasColumnType("int(11)");
            entityTypeBuilder
                .Property(prop => prop.EmployeeSalary)
                .HasColumnName("salary")
                .HasColumnType("int(11)");
            entityTypeBuilder
                .HasOne(s => s.Employee)
                .WithMany();
        }
    }
}
