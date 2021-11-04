using Manager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Infra.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Mapeando a tabela user do DB com a entidade do sistema.            
            builder.ToTable("User");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.Id)
                .UseMySqlIdentityColumn()
                .HasColumnType("BIGINT");

            builder.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnName("name")
                .HasColumnType("VARCHAR(80)");

            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(180)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(180)");

            builder.Property(user => user.Password)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("password")
                .HasColumnType("VARCHAR(30)");

        }
    }
}
