using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Entities;

namespace OA.Repo.MySql.Maps
{
    public class DepartmentMap
    {
        public DepartmentMap(EntityTypeBuilder<Department> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("departments");
            entityTypeBuilder.HasKey(prop => prop.DepartmentNumber);
            entityTypeBuilder
                .Property(prop => prop.DepartmentNumber)
                .HasColumnName("dept_no")
                .HasColumnType("char(4)");
            entityTypeBuilder
                .Property(prop => prop.DepartmentName)
                .HasColumnName("dept_name")
                .HasColumnType("varchar(40)")
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}
