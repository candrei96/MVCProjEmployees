using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Entities;

namespace OA.Repo.MySql.Maps
{
    public class DepartmentEmployeeMap
    {
        public DepartmentEmployeeMap(EntityTypeBuilder<DepartmentEmployee> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("dept_emp");
            entityTypeBuilder.HasKey(prop => new { prop.EmployeeNumber, prop.DepartmentNumber });
            entityTypeBuilder
                .Property(prop => prop.DepartmentNumber)
                .HasColumnName("dept_no")
                .HasColumnType("char(4)");
            entityTypeBuilder
                .Property(prop => prop.EmployeeNumber)
                .HasColumnName("emp_no")
                .HasColumnType("int(11)");
            entityTypeBuilder
                .Property(prop => prop.ToDate)
                .HasColumnName("to_date")
                .HasColumnType("date");
            entityTypeBuilder
                .Property(prop => prop.FromDate)
                .HasColumnName("from_date")
                .HasColumnType("date");
            entityTypeBuilder
                .HasOne(prop => prop.Employee)
                .WithMany();
            entityTypeBuilder
                .HasOne(prop => prop.Department)
                .WithMany();
        }
    }
}
