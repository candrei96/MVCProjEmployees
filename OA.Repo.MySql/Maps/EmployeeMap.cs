using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Entities;

namespace OA.Repo.MySql.Maps
{
    public class EmployeeMap
    {
        public EmployeeMap(EntityTypeBuilder<Employee> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("employees");
            entityTypeBuilder.HasKey(prop => prop.EmployeeNumber);
            entityTypeBuilder
                .Property(prop => prop.EmployeeNumber)
                .HasColumnName("emp_no")
                .HasColumnType("int(11)");
            entityTypeBuilder.Property(prop => prop.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(14)")
                .IsRequired()
                .HasMaxLength(14);
            entityTypeBuilder.Property(prop => prop.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(16)")
                .IsRequired()
                .HasMaxLength(16);
            entityTypeBuilder.Property(prop => prop.BirthDate)
                .HasColumnName("birth_date")
                .HasColumnType("date")
                .IsRequired();
            entityTypeBuilder.Property(prop => prop.Gender)
                .HasColumnName("gender")
                .HasColumnType("enum('M','F')")
                .IsRequired();
            entityTypeBuilder.Property(prop => prop.HireDate)
                .HasColumnName("hire_date")
                .HasColumnType("date")
                .IsRequired();
        }
    }
}
