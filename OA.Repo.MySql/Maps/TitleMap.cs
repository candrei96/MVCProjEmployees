using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace OA.Repo.MySql.Maps
{
    public class TitleMap
    {
        public TitleMap(EntityTypeBuilder<Title> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("titles");
            entityTypeBuilder.HasKey(prop => new { prop.EmployeeNumber, prop.Title, prop.FromDate });
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
                .Property(prop => prop.Title)
                .HasColumnName("title")
                .HasColumnType("varchar(50)");
            entityTypeBuilder
                .HasOne(t => t.Employee)
                .WithMany();
        }
    }
}
